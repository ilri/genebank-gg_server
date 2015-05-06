using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using GrinGlobal.Core;
using GrinGlobal.Interface;

namespace GrinGlobal.Search.Engine.Hosting {
    public class TcpSearchServer : IDisposable {

        private TcpListener _listener;
        private Thread _listenThread;
        private int _port;
        private ISearchHost _searcher;

        public TcpSearchServer(int port, ISearchHost searcher) {
            _port = port;
            _searcher = searcher;
        }

        public void Start() {
            if (_listener != null) {
                _listener.Stop();
            }
            if (_listenThread != null) {
                _listenThread.Abort();
            }

            _listener = new TcpListener(IPAddress.Any, _port);
            _listenThread = new Thread(new ThreadStart(beginListening));
            _listenThread.Start();
        }

        public bool IsListening {
            get {
                return _listening;
            }
        }

        public void Stop() {
            _listening = false;
            if (_listener != null) {
                _listener.Stop();
            }
            _listener = null;

            _listenThread = null;
        }

        private bool _listening;

        private void beginListening() {

            try {
                _listener.Start(Toolkit.GetSetting("SearchEngineConnectionQueueLength", 10));
                _listening = true;
                while (true) {
                    try {
                        var client = _listener.AcceptTcpClient();
                        var clientThread = new Thread(new ParameterizedThreadStart(handleRequest));
                        clientThread.Start(client);
                    } catch (SocketException se) {
                        if (se.ErrorCode == 10004 && !_listening) {
                            // ignore socket exceptions, the server is probably shutting down.
                            Console.WriteLine("Assuming server is shutting down... SocketException: " + se.Message);
                            Logger.LogText("Exception in TcpSearchServer on listen thread, assuming shutting down... " + se.Message, se);
                            break;
                        } else {
                            Console.WriteLine("SocketException, server will continue listening: " + se.Message);
                            Logger.LogText("Exception in TcpSearchServer on listen thread, server will continue listening: " + se.Message, se);
                        }
                    }
                }
            } catch (Exception ex){
                Logger.LogTextForcefully("Error when listening in basic TCP server: " + ex.Message, ex);
            } finally {
                _listening = false;
            }
        }

        private void handleRequest(object clientParam) {
            var client = clientParam as TcpClient;
            ServerSearchEngineRequest req = null;
            string inputXml = null;
            try {

                NetworkStream clientStream = client.GetStream();

                inputXml = Toolkit.ReadAllStreamText(clientStream, "</req>");

                req = ServerSearchEngineRequest.Parse(inputXml, _searcher);

                // determine which method they're wanting to call and call it
                // NOTE: Execute() NEVER throws exceptions, just puts them into its .Exception property
                req.Execute();

            } catch (Exception ex){
                if (req == null){
                    req = new ServerSearchEngineRequest(_searcher);
                    req.Exception = ex;
                }
                Logger.LogText("Error when handling request in basic TCP server: " + ex.Message, ex);
            } finally {

                // stream result back to them 
                try {
                    var outputBytes = UTF8Encoding.UTF8.GetBytes(req.ToXml());
                    var outputStream = client.GetStream();
                    outputStream.Write(outputBytes, 0, outputBytes.Length);
                    outputStream.Flush();
                } finally {
                    // clean up
                    client.Close();
                }
            }

        }
    
        #region IDisposable Members

        public void  Dispose()
        {
            Stop();
        }

        #endregion
    }
}
