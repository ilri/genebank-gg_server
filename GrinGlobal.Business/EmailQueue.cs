using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GrinGlobal.Core;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace GrinGlobal.Business {
    /// <summary>
    /// Manages all data in the email / email_attach tables.  Also handles sending email via SMTP based on data in those tables.
    /// </summary>
    public class EmailQueue : IDisposable {

        private static EmailQueue __background;
        /// <summary>
        /// Spawns a background thread to monitor the email table and send email when appropriate.
        /// </summary>
        public static void BeginMonitoring() {
            if (__background != null){
                __background.Dispose();
                __background = null;
            }
            __background = new EmailQueue();
            __background.run();
        }

        /// <summary>
        /// Tells background thread to finish what it's doing then exit
        /// </summary>
        public static void StopMonitoring() {
            if (__background != null) {
                __background.Stop();
            }
        }

        /// <summary>
        /// Immediately sends any email records ready to be sent. If no background thread is monitoring the email table, this will create one.  Otherwise will simply unblock the background thread.
        /// </summary>
        public static void SendQueuedEmailNow() {
            if (__background == null) {
                BeginMonitoring();
            } else {
                __background.kick();
            }
        }


        /// <summary>
        /// Remove all emails from pending queue that are associated with the given table/rowid that have not yet been sent.  Does not delete, simply updates retry_count to int.MaxValue so it falls out of the eligible to-send queue.
        /// </summary>
        /// <param name="fromTableName"></param>
        /// <param name="fromTableRowID"></param>
        public static void DequeueEmail(string fromTableName, int fromTableRowID) {
            using (var dm = DataManager.Create()) {

                var maxRetries = Toolkit.GetSetting("EmailMaxRetryCount", 5);

                // don't delete it, just mark retry count as int.MaxValue so it isn't tried again.
                dm.Write(@"
update
    email
set
    retry_count = :maxval,
    modified_date = :now1,
    modified_by = :who1
where
    id_type like :tblname
    and id_number = :rowid
    and sent_date is null
    and retry_count < :maxretries
", new DataParameters(":maxval", int.MaxValue, DbType.Int32,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", getSystemCooperatorID(dm), DbType.Int32,
     ":tblname", fromTableName, DbType.String,
     ":rowid", fromTableRowID, DbType.Int32,
     ":maxretries", maxRetries, DbType.Int32));

            }
        }

        /// <summary>
        /// Add a record to the email table that is to be sent at the given datetime.  Optionally launch and/or wake monitoring thread to immediately process it.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="replyTo"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isHtml"></param>
        /// <param name="toBeSentDate"></param>
        /// <param name="attachmentVirtualFilePaths"></param>
        /// <param name="cooperatorID"></param>
        /// <param name="fromTableName"></param>
        /// <param name="fromTableID"></param>
        /// <param name="sendImmediately"></param>
        public static void QueueEmail(string from, string to, string cc, string bcc, string replyTo, string subject, string body, bool isHtml, DateTime toBeSentDate, string[] attachmentVirtualFilePaths, int cooperatorID, string fromTableName, int fromTableRowID, bool sendImmediately) {
            using (var dm = DataManager.Create()) {

                var emailID = dm.Write(@"
insert into email
    (id_type, id_number, email_from, email_to, email_cc, email_bcc, email_reply_to, subject, body, is_html, to_be_sent_date, retry_count, created_date, created_by, owned_date, owned_by)
values
    (:id_type, :id_number, :email_from, :email_to, :email_cc, :email_bcc, :email_reply_to, :subject, :body, :is_html, :to_be_sent_date, :retry_count, :created_date, :created_by, :owned_date, :owned_by)
", true, "email_id",
 new DataParameters(
     ":id_type", fromTableName, DbType.String,
     ":id_number", (fromTableRowID < 0 ? null : (int?)fromTableRowID), DbType.Int32,
     ":email_from", from, DbType.String,
     ":email_to", to, DbType.String,
     ":email_cc", cc, DbType.String,
     ":email_bcc", bcc, DbType.String,
     ":email_reply_to", Toolkit.Coalesce(replyTo, from) as string, DbType.String,
     ":subject", subject, DbType.String,
     ":body", body, DbType.String,
     ":is_html", isHtml ? "Y" : "N", DbType.String,
     ":to_be_sent_date", toBeSentDate.ToUniversalTime(), DbType.DateTime2,
     ":retry_count", 0, DbType.Int32,
     ":created_date", DateTime.UtcNow, DbType.DateTime2,
     ":created_by", cooperatorID, DbType.Int32,
     ":owned_date", DateTime.UtcNow, DbType.DateTime2,
     ":owned_by", cooperatorID, DbType.Int32
     ));

                // also queue up associated attachments, if any
                if (attachmentVirtualFilePaths != null && attachmentVirtualFilePaths.Length > 0) {
                    foreach (var vpath in attachmentVirtualFilePaths) {

                        dm.Write(@"
insert into email_attach
    (email_id, virtual_path, is_web_visible, created_date, created_by, owned_date, owned_by)
values
    (:email_id, :virtual_path, :is_web_visible, :created_date, :created_by, :owned_date, :owned_by)
", new DataParameters(
     ":email_id", emailID, DbType.Int32,
     ":virtual_path", vpath, DbType.String,
     ":is_web_visible", "Y", DbType.String,
     ":created_date", DateTime.UtcNow, DbType.DateTime2,
     ":created_by", cooperatorID, DbType.Int32,
     ":owned_date", DateTime.UtcNow, DbType.DateTime2,
     ":owned_by", cooperatorID, DbType.Int32
     ));

                    }
                }

            }

            if (sendImmediately) {
                SendQueuedEmailNow();
            }
        }

        /// <summary>
        /// Returns all emails that exceeded the EmailMaxRetryCount limit and were never sent.
        /// </summary>
        /// <returns></returns>
        public static DataTable ListDeadEmails() {
            using (var dm = DataManager.Create()) {
                var maxRetries = Toolkit.GetSetting("EmailMaxRetryCount", 5);
                return dm.Read(@"select * from email where retry_count >= " + maxRetries + " and retry_count < " + int.MaxValue);
            }
        }

        /// <summary>
        /// Resets retry_count to 0 for all given emailIDs that were never sent and are dead.  If emailIDs is null or of length 0, resets retry_count for ALL emails that were never sent and are dead.
        /// </summary>
        /// <param name="emailIDs"></param>
        public static void ReviveDeadEmails(List<int> emailIDs) {
            using (var dm = DataManager.Create()) {

                var maxRetries = Toolkit.GetSetting("EmailMaxRetryCount", 5);

                if (emailIDs == null || emailIDs.Count == 0) {

                    // mark all dead emails as alive again

                    dm.Write(@"
update
    email
set
    retry_count = 0,
    modified_date = :now1,
    modified_by = :coop1
where
    retry_count >= :maxretrycount
    and retry_count < :maxval
    and sent_date is null
", new DataParameters(":now1", DateTime.UtcNow, DbType.DateTime2,
     ":coop1", getSystemCooperatorID(dm), DbType.Int32,
     ":maxretrycount", maxRetries, DbType.Int32,
     ":maxval", int.MaxValue, DbType.Int32));

                } else {

                    // mark specified emails as alive again if they're dead

                    dm.Write(@"
update
    email
set
    retry_count = 0,
    modified_date = :now1,
    modified_by = :coop1
where
    email_id in (:emailids)
    and retry_count >= :maxretrycount
    and retry_count < :maxval
    and sent_date is null
", new DataParameters(":now1", DateTime.UtcNow, DbType.DateTime2,
         ":coop1", getSystemCooperatorID(dm), DbType.Int32,
         ":emailids", emailIDs, DbPseudoType.IntegerCollection,
         ":maxretrycount", maxRetries, DbType.Int32,
         ":maxval", int.MaxValue, DbType.Int32));

                }
            }
        }

        private EmailQueue() {
        }

        private object _locker = new object();
        private Thread _thread;

        private void run() {
            if (_thread != null) {
                try {
                    _thread.Abort();
                } catch { }
                _thread = null;
            }

            _thread = new Thread(new ThreadStart(start));
            _thread.Start();
        }

        private bool _stopping;

        public void Stop() {
            // Debug.WriteLine("request to stop email queue, thread=" + Thread.CurrentThread.ManagedThreadId);
            _stopping = true;
            kick();
        }

        private void kick(){
            // Debug.WriteLine("acquiring lock on email queue, thread=" + Thread.CurrentThread.ManagedThreadId);
            lock (_locker) {
                // Debug.WriteLine("pulsing email queue, thread=" + Thread.CurrentThread.ManagedThreadId);
                Monitor.PulseAll(_locker);
            }
        }

        private void start() {
            
            // we are on a background thread.
            // determine the wait interval, then call process() on that interval until it returns true, which means we are done monitoring (someone called Stop())

            var wait = Toolkit.GetSetting("EmailIntervalInSeconds", 120);
            if (wait < 1){
                wait = 60;
            }
            wait *= 1000;

            // Debug.WriteLine("starting email queue, thread=" + Thread.CurrentThread.ManagedThreadId);

            while (!_stopping) {
                process();
                // Debug.WriteLine("acquiring lock on email queue, thread=" + Thread.CurrentThread.ManagedThreadId);
                lock (_locker) {
                    Monitor.Wait(_locker, wait);
                    // Debug.WriteLine("done waiting on email queue, thread=" + Thread.CurrentThread.ManagedThreadId);
                }
            }

        }

        private void process() {

            try {

                // Debug.WriteLine("processing email queue, thread=" + Thread.CurrentThread.ManagedThreadId);

                // query the database, see what all there is to do
                using (var dm = DataManager.Create()) {

                    var systemCooperatorID = getSystemCooperatorID(dm);

                    var maxRetryCount = Toolkit.GetSetting("EmailMaxRetryCount", 5);
                    var delayRetryOnFail = Toolkit.GetSetting("EmailRetryDelayInSeconds", 1200);

                    var dt = dm.Read(@"
select
    *
from
    email
where
    sent_date is null
    and to_be_sent_date < :now
    and retry_count < :maxretry
", new DataParameters(":now", DateTime.UtcNow, DbType.DateTime2,
     ":maxretry", maxRetryCount, DbType.Int32));

                    // pull the email info from the database
                    foreach (DataRow dr in dt.Rows) {

                        var email = new Email();
                        email.Id = Toolkit.ToInt32(dr["email_id"], -1); 
                        email.To = dr["email_to"].ToString();
                        email.From = dr["email_from"].ToString();
                        email.CC = dr["email_cc"].ToString();
                        email.BCC = dr["email_bcc"].ToString();
                        email.Subject = dr["subject"].ToString();
                        email.Body = dr["body"].ToString();
                        email.Format = dr["is_html"].ToString().ToUpper() == "Y" ? EmailFormat.Html : EmailFormat.Text;
                        email.ReplyTo = Toolkit.Coalesce(dr["email_reply_to"].ToString(),  email.From) as string;

                        var retries = Toolkit.ToInt32(dr["retry_count"], 0);


                        try {

                            
                            // add any attachments...
                            var dtAttach = dm.Read(@"
select
    *
from
    email_attach
where
    email_id = :emailid
", new DataParameters(":emailid", Toolkit.ToInt32("email_id", -1), DbType.Int32));

                            foreach (DataRow drAttach in dtAttach.Rows) {

                                var vpath = drAttach["virtual_file_path"].ToString();
                                if (!String.IsNullOrEmpty(vpath)) {
                                    var physpath = Toolkit.ResolveFilePath(vpath, false);
                                    if (File.Exists(physpath)) {
                                        var attach = new Attachment(physpath);
                                        email.Attachments.Add(attach);
                                    } else {
                                        throw new FileNotFoundException("Could not attach file '" + physpath + "' to emailID = " + dr["email_id"] + " because it could not be found.");
                                    }
                                }
                            }


                            // try to send it
                            email.Send();

                            // we get here, email was sent successfully.
                            dm.Write(@"
update
    email
set
    sent_date = :now1,
    modified_date = :now2,
    modified_by = :coop1
where
    email_id = :emailid
", new DataParameters(
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":now2", DateTime.UtcNow, DbType.DateTime2,
     ":coop1", systemCooperatorID, DbType.Int32,
     ":emailid", Toolkit.ToInt32("email_id", -1), DbType.Int32));

                        } catch (Exception ex) {

                            // we get here, send email failed, log it and continue.
                            retries++;

                            dm.Write(@"
update
    email
set
    retry_count = :retries,
    last_retry_error_message = :errormsg,
    last_retry_date = :now1,
    to_be_sent_date = :later1,
    modified_date = :now2,
    modified_by = :coop1
where
    email_id = :emailid
", new DataParameters(
     ":retries", retries, DbType.Int32,
     ":errormsg", ex.Message, DbType.String,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":later1", DateTime.UtcNow.AddSeconds(delayRetryOnFail), DbType.DateTime2,
     ":now2", DateTime.UtcNow, DbType.DateTime2,
     ":coop1", systemCooperatorID, DbType.Int32,
     ":emailid", email.Id, DbType.Int32));

                        }


                    }


                }
            } catch (Exception ex2) {
                // eat all errors so email thread doesn't die off
                Logger.LogText(ex2);
            }

        }

        private static int getSystemCooperatorID(DataManager dm) {
            try {
                var dt = dm.Read("select cooperator_id from cooperator where last_name = 'SYSTEM'");
                if (dt.Rows.Count < 1) {
                    return -1;
                } else {
                    return Toolkit.ToInt32(dt.Rows[0]["cooperator_id"], -1);
                }
            } catch (Exception ex) {
                // we get here if the database doesn't exist...
                return -1;
            }
        }
    
        public void  Dispose(){
            // TODO: release stuff here
            Stop();
        }

    }
}
