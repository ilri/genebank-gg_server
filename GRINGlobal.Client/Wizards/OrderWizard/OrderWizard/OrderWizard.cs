using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GRINGlobal.Client.Common;

namespace OrderWizard
{
    interface IGRINGlobalDataWizard
    {
        string FormName { get; }
        DataTable ChangedRecords { get; }
        string PKeyName { get; }
        //string PreferredDataview { get; }
        //bool EditMode { get; set; }
    }

    public partial class OrderWizard : Form, IGRINGlobalDataWizard
    {
        SharedUtils _sharedUtils;
        DataTable _orderRequest;
        DataTable _orderRequestItem;
        DataTable _orderRequestAction;
        DataTable _orderRequestStatusCodes;
        DataTable _webOrderRequest;
        DataTable _webOrderRequestItem;
        DataTable _webOrderRequestStatusCodes;
        BindingSource _orderRequestBindingSource;
        BindingSource _orderRequestItemBindingSource;
        BindingSource _orderRequestActionBindingSource;
        BindingSource _webOrderRequestBindingSource;
        BindingSource _webOrderRequestItemBindingSource;
        string _originalPKeys = "";
        string _orderRequestPKeys = "";
        string _orderRequestStatusFilter = "";
        string _webOrderRequestStatusFilter = "";
        DataSet _changedRecords = new DataSet();

        public OrderWizard(string pKeys, SharedUtils sharedUtils)
        {
            InitializeComponent();

            // Wire up the event handlers for Order Request binding source...
            _orderRequestBindingSource = new BindingSource();
            _orderRequestBindingSource.ListChanged += new ListChangedEventHandler(_orderRequestBindingSource_ListChanged);
            _orderRequestBindingSource.CurrentChanged += new EventHandler(_orderRequestBindingSource_CurrentChanged);
            _orderRequestItemBindingSource = new BindingSource();
            _orderRequestActionBindingSource = new BindingSource();
            _sharedUtils = sharedUtils;
            _originalPKeys = pKeys;

            // Wire up the event handlers for Web Order Request binding source...
            _webOrderRequestBindingSource = new BindingSource();
            _webOrderRequestBindingSource.CurrentChanged += new EventHandler(_webOrderRequestBindingSource_CurrentChanged);
            _webOrderRequestItemBindingSource = new BindingSource();

            // Make the filter groupboxes the same size and location...
            ux_groupboxWebOrderFilters.Size = ux_groupboxOrderFilters.Size;
            ux_groupboxWebOrderFilters.Location = ux_groupboxOrderFilters.Location;
            ux_groupboxWebOrderFilters.Visible = false;
            ux_groupboxOrderFilters.Visible = true;
        }

        private void OrderWizard_Load(object sender, EventArgs e)
        {
            this.Text = FormName;

            BuildOrderRequestActionsPage();
            BuildWebOrderRequestPage();

            //DataSet ds;
            //// Get the order_request table and bind it to the main form on the General tabpage...
            //ds = _sharedUtils.GetWebServiceData("get_order_request", _origPKeys, 0, 0);
            //_orderRequestPKeys = "";
            //if (ds.Tables.Contains("get_order_request"))
            //{
            //    _orderRequest = ds.Tables["get_order_request"].Copy();
            //    _orderRequestBindingSource.DataSource = _orderRequest;
            //    // Build a list of order_request_ids to use for gathering the order_request_items...
            //    _orderRequestPKeys = ":orderrequestid=";
            //    foreach (DataRow dr in _orderRequest.Rows)
            //    {
            //        _orderRequestPKeys += dr["order_request_id"].ToString() + ",";
            //    }
            //    //_orderRequestPKeys = _origPKeys.Replace(":orderrequestid=", orderRequestIDs.TrimEnd(','));
            //    _orderRequestPKeys = _orderRequestPKeys.TrimEnd(',');
            //}

            //// Get the order_request_item table and bind it to the main form on the General tabpage...
            //ds = _sharedUtils.GetWebServiceData("get_order_request_item", _orderRequestPKeys, 0, 0);
            //if (ds.Tables.Contains("get_order_request_item"))
            //{
            //    _orderRequestItem = ds.Tables["get_order_request_item"].Copy();
            //    ux_datagridviewOrderRequestItem.DataSource = _orderRequestItemBindingSource;
            //    _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestItem, _orderRequestItem);
            //}

            // Refresh the order request data binding...
            RefreshOrderData();

            // Refresh the web order request data binding...
            RefreshWebOrderData();

            // Add items to the order request status checked listbox control...
            _orderRequestStatusCodes = _sharedUtils.GetLocalData("select * from code_value_lookup where group_name=@groupname", "@groupname=ORDER_REQUEST_ITEM_STATUS");
            if (_orderRequestStatusCodes != null &&
                _orderRequestStatusCodes.Columns.Contains("value_member") &&
                _orderRequestStatusCodes.Columns.Contains("display_member"))
            {
                ux_checkedlistboxOrderItemStatus.Items.Clear();
                _orderRequestStatusFilter = "";
                foreach (DataRow dr in _orderRequestStatusCodes.Rows)
                {
                    ux_checkedlistboxOrderItemStatus.Items.Add(dr["display_member"].ToString(), false);
                }
            }

            // Add items to the web order request status checked listbox control...
            _webOrderRequestStatusCodes = _sharedUtils.GetLocalData("select * from code_value_lookup where group_name=@groupname", "@groupname=WEB_ORDER_REQUEST_STATUS");
            if (_webOrderRequestStatusCodes != null &&
                _webOrderRequestStatusCodes.Columns.Contains("value_member") &&
                _webOrderRequestStatusCodes.Columns.Contains("display_member"))
            {
                ux_checkedlistboxWebOrderItemStatus.Items.Clear();
                _webOrderRequestStatusFilter = "";
                foreach (DataRow dr in _webOrderRequestStatusCodes.Rows)
                {
                    ux_checkedlistboxWebOrderItemStatus.Items.Add(dr["display_member"].ToString(), false);
                }
            }

            // Bind the bindingsource to the binding navigator toolstrip on the main form...
            ux_bindingnavigatorForm.BindingSource = _orderRequestBindingSource;

            // Bind the bindingsource to the binding navigator toolstrip on the web_order_request tab page...
            ux_bindingNavigatorWebOrders.BindingSource = _webOrderRequestBindingSource;

// Format the controls on this dialog...
//bindControls(this.Controls);
//formatControls(this.Controls);
            // Bind and format the controls on the Order tab...
            bindControls(OrderPage.Controls, _orderRequestBindingSource);
            formatControls(OrderPage.Controls, _orderRequestBindingSource);
            
            // Re-Bind (but don't format) the controls on the Web Order tab to a different binding source...
            bindControls(WebOrderPage.Controls, _webOrderRequestBindingSource);
            //formatControls(WebOrderPage.Controls, _webOrderRequestBindingSource);

            if (_orderRequestBindingSource.List.Count > 0)
            {
//ux_tabcontrolMain.Enabled = true;
OrderPage.Show();
ActionsPage.Show();
OrderPage.Enabled = true;
ActionsPage.Enabled = true;
            }
            else
            {
//ux_tabcontrolMain.Enabled = false;
OrderPage.Hide();
ActionsPage.Hide();
OrderPage.Enabled = false;
ActionsPage.Enabled = false;
            }

_sharedUtils.UpdateComponents(this.components.Components, this.Name);
_sharedUtils.UpdateControls(this.Controls, this.Name);

            // Force the row filters to be applied...
            _orderRequestBindingSource_CurrentChanged(sender, e);
        }

        public string FormName
        {
            get
            {
                return "Order Wizard";
            }
        }

        public DataTable ChangedRecords
        {
            get
            {
                DataTable dt = new DataTable();
                if (_changedRecords.Tables.Contains(_orderRequest.TableName))
                {
                    dt = _changedRecords.Tables[_orderRequest.TableName].Copy();
                }
                return dt;
            }
        }

        public string PKeyName
        {
            get
            {
                return "order_request_id";
            }
        }

        private void OrderWizard_FormClosed(object sender, FormClosedEventArgs e)
        {
            _orderRequestBindingSource.ListChanged -= _orderRequestBindingSource_ListChanged;
            _orderRequestBindingSource.CurrentChanged -= _orderRequestBindingSource_CurrentChanged;
            _webOrderRequestBindingSource.CurrentChanged -= _webOrderRequestBindingSource_CurrentChanged;
        }

        private void OrderWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // The user might be closing the form during the middle of edit changes in the datagridview - if so ask the
            // user if they would like to save their data...
            int intRowEdits = 0;

            _orderRequestBindingSource.EndEdit();
            if (_orderRequest.GetChanges() != null) intRowEdits = _orderRequest.GetChanges().Rows.Count;
            _orderRequestItemBindingSource.EndEdit();
            if (_orderRequestItem.GetChanges() != null) intRowEdits += _orderRequestItem.GetChanges().Rows.Count;
            _orderRequestActionBindingSource.EndEdit();
            if (_orderRequestAction.GetChanges() != null) intRowEdits += _orderRequestAction.GetChanges().Rows.Count;
//_webOrderRequestBindingSource.EndEdit();
//if (_webOrderRequest.GetChanges() != null) intRowEdits += _webOrderRequest.GetChanges().Rows.Count;

GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have {0} unsaved row change(s), are you sure you want to cancel your edits and close this window?", "Cancel Edits and Close", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "OrderWizard_FormClosingMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, intRowEdits);
//if (intRowEdits > 0 &&
//    DialogResult.No == MessageBox.Show("You have " + intRowEdits + " unsaved row change(s), are you sure you want to cancel?", "Cancel Edits", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
if (intRowEdits > 0 && DialogResult.No == ggMessageBox.ShowDialog())
            {
                e.Cancel = true;
            }
        }

        #region Customizations to Dynamic Controls...
        private void ux_textboxFinalDestination_TextChanged(object sender, EventArgs e)
        {
            bindingNavigatorOrderNumber.Text = "";

                if (string.IsNullOrEmpty(ux_textboxFinalDestination.Text) &&
                    _orderRequest.Columns[ux_textboxFinalDestination.Tag.ToString()].ExtendedProperties.Contains("is_nullable") &&
                    _orderRequest.Columns[ux_textboxFinalDestination.Tag.ToString()].ExtendedProperties["is_nullable"].ToString() == "N" &&
                    !_orderRequest.Columns[ux_textboxFinalDestination.Tag.ToString()].ReadOnly)
            {
                ux_textboxFinalDestination.BackColor = Color.Plum;
            }
            else
            {
                ux_textboxFinalDestination.BackColor = Color.Empty;
                if (string.IsNullOrEmpty(ux_textboxRequestor.Text))
                {
                    ux_textboxRequestor.Text = ux_textboxFinalDestination.Text;
                }
                if (string.IsNullOrEmpty(ux_textboxShipTo.Text))
                {
                    ux_textboxShipTo.Text = ux_textboxFinalDestination.Text;
                }
            }
        }
        #endregion

        #region Dynamic Controls logic...
        private void bindControls(Control.ControlCollection controlCollection, BindingSource bindingSource)
        {
            foreach (Control ctrl in controlCollection)
            {
//if (ctrl != ux_bindingnavigatorForm)  // Leave the bindingnavigator alone
                if (!(ctrl is BindingNavigator))  // Leave the bindingnavigators alone
                {
                    // If the ctrl has children - bind them too...
                    if (ctrl.Controls.Count > 0)
                    {
                        bindControls(ctrl.Controls, bindingSource);
                    }
                    // Bind the control (by type)...
                    if (ctrl is ComboBox) bindComboBox((ComboBox)ctrl, bindingSource);
                    if (ctrl is TextBox) bindTextBox((TextBox)ctrl, bindingSource);
                    if (ctrl is CheckBox) bindCheckBox((CheckBox)ctrl, bindingSource);
                    if (ctrl is DateTimePicker) bindDateTimePicker((DateTimePicker)ctrl, bindingSource);
                    if (ctrl is Label) bindLabel((Label)ctrl, bindingSource);
                }
            }
        }

        private void formatControls(Control.ControlCollection controlCollection, BindingSource bindingSource)
        {
            foreach (Control ctrl in controlCollection)
            {
                if (ctrl != ux_bindingnavigatorForm)  // Leave the bindingnavigator alone
                {
                    // If the ctrl has children - set their edit mode too...
                    if (ctrl.Controls.Count > 0)
                    {
                        formatControls(ctrl.Controls, bindingSource);
                    }
                    // Set the edit mode for the control...
                    if (ctrl != null &&
                        ctrl.Tag != null &&
                        ctrl.Tag is string &&
                        bindingSource != null &&
                        bindingSource.DataSource is DataTable &&
                        ((DataTable)bindingSource.DataSource).Columns.Contains(ctrl.Tag.ToString().Trim().ToLower()))
                    {
                        if (ctrl is TextBox)
                        {
                            // TextBoxes have a ReadOnly property in addition to an Enabled property so we handle this one separate...
                            ((TextBox)ctrl).ReadOnly = ((DataTable)bindingSource.DataSource).Columns[ctrl.Tag.ToString().Trim().ToLower()].ReadOnly;
                        }
                        else if (ctrl is Label)
                        {
                            // Do nothing to the Label
                        }
                        else
                        {
                            // All other control types (ComboBox, CheckBox, DateTimePicker) except Labels...
                            ctrl.Enabled = !((DataTable)bindingSource.DataSource).Columns[ctrl.Tag.ToString().Trim().ToLower()].ReadOnly;
                        }
                    }
                }
            }
        }

        private void bindComboBox(ComboBox comboBox, BindingSource bindingSource)
        {
            comboBox.DataBindings.Clear();
            comboBox.Enabled = false;
            if (comboBox != null &&
                comboBox.Tag != null &&
                comboBox.Tag is string &&
                bindingSource != null &&
                bindingSource.DataSource is DataTable &&
                ((DataTable)bindingSource.DataSource).Columns.Contains(comboBox.Tag.ToString().Trim().ToLower()))
            {
                if (_sharedUtils != null)
                {
                    DataColumn dc = ((DataTable)bindingSource.DataSource).Columns[comboBox.Tag.ToString().Trim().ToLower()];
                    _sharedUtils.BindComboboxToCodeValue(comboBox, dc);
                    if (comboBox.DataSource.GetType() == typeof(DataTable))
                    {
                        // Calculate the maximum width needed for displaying the dropdown items and set the combobox property...
                        int maxWidth = comboBox.DropDownWidth;
                        foreach (DataRow dr in ((DataTable)comboBox.DataSource).Rows)
                        {
                            if (TextRenderer.MeasureText(dr["display_member"].ToString().Trim(), comboBox.Font).Width > maxWidth)
                            {
                                maxWidth = TextRenderer.MeasureText(dr["display_member"].ToString().Trim(), comboBox.Font).Width;
                            }
                        }
                        comboBox.DropDownWidth = maxWidth;
                    }

//if (_sharedUtils != null &&
//    _sharedUtils.LookupTablesContains("MRU_code_value_lookup"))
//{
//    DataColumn dc = ((DataTable)bindingSource.DataSource).Columns[comboBox.Tag.ToString().Trim().ToLower()];
//    DataTable cvl = _sharedUtils.LookupTablesGetMRUTable("MRU_code_value_lookup");

//    //if (dc.ExtendedProperties.Contains("code_group_id") && cvl != null)
//    if (dc.ExtendedProperties.Contains("group_name") && cvl != null)
//    {
//        //DataView dv = new DataView(cvl, "code_group_id='" + dc.ExtendedProperties["code_group_id"].ToString() + "'", "display_member ASC", DataViewRowState.CurrentRows);
//        DataView dv = new DataView(cvl, "group_name='" + dc.ExtendedProperties["group_name"].ToString() + "'", "display_member ASC", DataViewRowState.CurrentRows);
//        DataTable dt = dv.ToTable();
//        if (dc.ExtendedProperties.Contains("is_nullable") && dc.ExtendedProperties["is_nullable"].ToString() == "Y")
//        {
//            DataRow dr = dt.NewRow();
//            foreach (DataColumn cvldc in cvl.Columns)
//            {
//                // If there are any non-nullable fields - set them now...
//                if (!cvldc.AllowDBNull)
//                {
//                    dr[cvldc.ColumnName] = -1;
//                }
//            }
//            dr["display_member"] = "[Null]";
//            dr["value_member"] = DBNull.Value;
//            dt.Rows.InsertAt(dr, 0);
//            dt.AcceptChanges();
//        }
//        comboBox.DisplayMember = "display_member";
//        comboBox.ValueMember = "value_member";
//        comboBox.DataSource = dt;
//    }

//    // Calculate the maximum width needed for displaying the dropdown items and set the combobox property...
//    int maxWidth = comboBox.DropDownWidth;
//    foreach (DataRow dr in cvl.Rows)
//    {
//        if (TextRenderer.MeasureText(dr["display_member"].ToString().Trim(), comboBox.Font).Width > maxWidth)
//        {
//            maxWidth = TextRenderer.MeasureText(dr["display_member"].ToString().Trim(), comboBox.Font).Width;
//        }
//    }
//    comboBox.DropDownWidth = maxWidth;

                    // Bind the SelectedValue property to the binding source...
                    comboBox.DataBindings.Add("SelectedValue", bindingSource, comboBox.Tag.ToString().Trim().ToLower(), true, DataSourceUpdateMode.OnPropertyChanged);
  
                    // Wire up to an event handler if this column is a date_code (format) field...
                    if (dc.ColumnName.Trim().ToLower().EndsWith("_code") &&
                        dc.Table.Columns.Contains(dc.ColumnName.Trim().ToLower().Substring(0, dc.ColumnName.Trim().ToLower().LastIndexOf("_code"))))
                    {
                        comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
                    }
                }
                else
                {
                    // Bind the Text property to the binding source...
                    comboBox.DataBindings.Add("Text", bindingSource, comboBox.Tag.ToString().Trim().ToLower(), true, DataSourceUpdateMode.OnPropertyChanged);
                }
            }
        }

        private void bindTextBox(TextBox textBox, BindingSource bindingSource)
        {
            textBox.DataBindings.Clear();
            textBox.ReadOnly = true;
                if (textBox != null &&
                    textBox.Tag != null &&
                    textBox.Tag is string &&
                    bindingSource != null &&
                    bindingSource.DataSource is DataTable &&
                    ((DataTable)bindingSource.DataSource).Columns.Contains(textBox.Tag.ToString().Trim().ToLower()))
                {
                    DataTable dt = (DataTable)bindingSource.DataSource;
                    DataColumn dc = dt.Columns[textBox.Tag.ToString().Trim().ToLower()];
                    if (_sharedUtils.LookupTablesIsValidFKField(dc))
                    {
                        // Create a new binding that handles display_member/value_member conversions...
                        Binding textBinding = new Binding("Text", bindingSource, textBox.Tag.ToString().Trim().ToLower());
                        textBinding.Format += new ConvertEventHandler(textLUBinding_Format);
                        textBinding.Parse += new ConvertEventHandler(textLUBinding_Parse);
                        textBinding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                        // Bind it to the textbox...
                        textBox.DataBindings.Add(textBinding);
                    }
                    else if (dc.DataType == typeof(DateTime))
                    {
                        // Create a new binding that handles display_member/value_member conversions...
                        Binding textBinding = new Binding("Text", bindingSource, textBox.Tag.ToString().Trim().ToLower());
                        textBinding.Format += new ConvertEventHandler(textDateTimeBinding_Format);
                        textBinding.Parse += new ConvertEventHandler(textDateTimeBinding_Parse);
                        textBinding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                        // Bind it to the textbox...
                        textBox.DataBindings.Add(textBinding);
                    }
                    else
                    {
                        // Bind to a plain-old text field in the database (no LU required)...
                        textBox.DataBindings.Add("Text", bindingSource, textBox.Tag.ToString().Trim().ToLower());
                    }

                    // Add an event handler for processing the first key press (to display the lookup picker dialog)...
                    textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                }
        }

        private void bindCheckBox(CheckBox checkBox, BindingSource bindingSource)
        {
            checkBox.DataBindings.Clear();
            checkBox.Enabled = false;
            if (checkBox != null &&
                checkBox.Tag != null &&
                checkBox.Tag is string &&
                bindingSource != null &&
                bindingSource.DataSource is DataTable &&
                ((DataTable)bindingSource.DataSource).Columns.Contains(checkBox.Tag.ToString().Trim().ToLower()))
            {
                DataTable dt = (DataTable)bindingSource.DataSource;
                DataColumn dc = dt.Columns[checkBox.Tag.ToString().Trim().ToLower()];
                checkBox.Text = dc.Caption;
                Binding boolBinding = new Binding("Checked", bindingSource, checkBox.Tag.ToString().Trim().ToLower());
                boolBinding.Format += new ConvertEventHandler(boolBinding_Format);
                boolBinding.Parse += new ConvertEventHandler(boolBinding_Parse);
                boolBinding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                checkBox.DataBindings.Add(boolBinding);
            }
        }

        private void bindDateTimePicker(DateTimePicker dateTimePicker, BindingSource bindingSource)
        {
            dateTimePicker.DataBindings.Clear();
            dateTimePicker.Enabled = false;
            if (dateTimePicker != null &&
                dateTimePicker.Tag != null &&
                dateTimePicker.Tag is string &&
                bindingSource != null &&
                bindingSource.DataSource is DataTable &&
                ((DataTable)bindingSource.DataSource).Columns.Contains(dateTimePicker.Tag.ToString().Trim().ToLower()))
            {
                // Now bind the control to the column in the bindingSource...
                dateTimePicker.DataBindings.Add("Text", bindingSource, dateTimePicker.Tag.ToString().Trim().ToLower(), true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void bindLabel(Label label, BindingSource bindingSource)
        {
            if (label != null &&
                label.Tag != null &&
                label.Tag is string &&
                bindingSource != null &&
                bindingSource.DataSource is DataTable &&
                ((DataTable)bindingSource.DataSource).Columns.Contains(label.Tag.ToString().Trim().ToLower()))
            {
                //label.DataBindings.Add("Text", bindingSource, label.Tag.ToString().Trim().ToLower());
                label.Text = ((DataTable)bindingSource.DataSource).Columns[label.Tag.ToString().Trim().ToLower()].Caption;
            }
        }

        void boolBinding_Format(object sender, ConvertEventArgs e)
        {
            switch (e.Value.ToString().ToUpper())
            {
                case "Y":
                    e.Value = true;
                    break;
                case "N":
                    e.Value = false;
                    break;
                default:
                    e.Value = false;
                    break;
            }
        }

        void boolBinding_Parse(object sender, ConvertEventArgs e)
        {
            if (e.Value != null)
            {
                switch ((bool)e.Value)
                {
                    case true:
                        e.Value = "Y";
                        break;
                    case false:
                        e.Value = "N";
                        break;
                    default:
                        e.Value = "N";
                        break;
                }
            }
            else
            {
                e.Value = "N";
            }
        }

        void textDateTimeBinding_Format(object sender, ConvertEventArgs e)
        {
            Binding b = (Binding)sender;
            DataTable dt = (DataTable)((BindingSource)b.DataSource).DataSource;
            DataColumn dc = dt.Columns[b.BindingMemberInfo.BindingMember];
            if (dc.DataType == typeof(DateTime) &&
                !string.IsNullOrEmpty(e.Value.ToString()) &&
                dt.Columns.Contains(dc.ColumnName.Trim().ToLower() + "_code"))
            {
                DataRowView drv = (DataRowView)((BindingSource)b.DataSource).Current;
                string dateFormat = "MM/dd/yyyy";
                dateFormat = drv[dc.ColumnName + "_code"].ToString().Trim();
                e.Value = ((DateTime)e.Value).ToString(dateFormat);
            }
        }

        void textDateTimeBinding_Parse(object sender, ConvertEventArgs e)
        {
            Binding b = (Binding)sender;
            DataTable dt = (DataTable)((BindingSource)b.DataSource).DataSource;
            DataColumn dc = dt.Columns[b.BindingMemberInfo.BindingMember];
            if (dc.DataType == typeof(DateTime) &&
                !string.IsNullOrEmpty(e.Value.ToString()) &&
                dt.Columns.Contains(dc.ColumnName.Trim().ToLower() + "_code"))
            {
                DataRowView drv = (DataRowView)((BindingSource)b.DataSource).Current;
                string dateFormat = drv[dc.ColumnName + "_code"].ToString().Trim();
                DateTime parsedDateTime;
                if (DateTime.TryParseExact(e.Value.ToString(), dateFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out parsedDateTime))
                {
                    e.Value = parsedDateTime;
                }
            }
        }

        void textLUBinding_Format(object sender, ConvertEventArgs e)
        {
            Binding b = (Binding)sender;
            DataTable dt = (DataTable)((BindingSource)b.DataSource).DataSource;
            DataColumn dc = dt.Columns[b.BindingMemberInfo.BindingMember];
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                //e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
                e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
            }
        }

        void textLUBinding_Parse(object sender, ConvertEventArgs e)
        {
            Binding b = (Binding)sender;
            DataTable dt = (DataTable)((BindingSource)b.DataSource).DataSource;
            DataColumn dc = dt.Columns[b.BindingMemberInfo.BindingMember];
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                //e.Value = _sharedUtils.GetLookupValueMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());  ((DataRowView)((BindingSource)b.DataSource).Current).Row
                e.Value = _sharedUtils.GetLookupValueMember(((DataRowView)((BindingSource)b.DataSource).Current).Row, dc.ExtendedProperties["foreign_key_dataview_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
            }
        }

        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string dateColumnName = cb.Tag.ToString().Replace("_code", "");
            foreach (Binding b in cb.DataBindings)
            {
                foreach (Control ctrl in cb.Parent.Controls)
                {
                    if (ctrl.Tag.ToString() == dateColumnName &&
                        ctrl.GetType() == typeof(TextBox))
                    {
                        DataRowView drv = (DataRowView)((BindingSource)b.DataSource).Current;
                        string dateFormat = "MM/dd/yyyy";
                        dateFormat = drv[cb.Tag.ToString()].ToString().Trim();
                        DateTime dt;
                        if (DateTime.TryParseExact(drv[dateColumnName].ToString(), dateFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out dt))
                        {
                            ctrl.Text = ((DateTime)drv[dateColumnName]).ToString(dateFormat);
                        }
                    }
                }
            }
        }

        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;


            if (!tb.ReadOnly)
            {
                foreach (Binding b in tb.DataBindings)
                {
                    if (b.BindingManagerBase != null &&
                        b.BindingManagerBase.Current != null &&
                        b.BindingManagerBase.Current is DataRowView &&
                        b.BindingMemberInfo.BindingField != null)
                    {
                        if (_sharedUtils.LookupTablesIsValidFKField(((DataRowView)b.BindingManagerBase.Current).Row.Table.Columns[b.BindingMemberInfo.BindingField]) &&
                            e.KeyChar != Convert.ToChar(Keys.Escape)) // Ignore the Escape key and process anything else...
                        {
                            LookupTablePicker ltp = new LookupTablePicker(_sharedUtils, tb.Tag.ToString(), ((DataRowView)b.BindingManagerBase.Current).Row, tb.Text);
                            ltp.StartPosition = FormStartPosition.CenterParent;
                            if (DialogResult.OK == ltp.ShowDialog())
                            {
                                tb.Text = ltp.NewValue.Trim();
                            }
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) // Process the Delete key (since it is not passed on to the KeyPress event handler)...
            {
                TextBox tb = (TextBox)sender;

                if (!tb.ReadOnly)
                {
                    foreach (Binding b in tb.DataBindings)
                    {
                        if (b.BindingManagerBase != null &&
                            b.BindingManagerBase.Current != null &&
                            b.BindingManagerBase.Current is DataRowView &&
                            b.BindingMemberInfo.BindingField != null)
                        {
                            // Just in case the user selected only a part of the full text to delete - strip out the selected text and process normally...
                            string remainingText = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength);
                            if (string.IsNullOrEmpty(remainingText))
                            {
                                // When a textbox is bound to a table - some datatypes will not revert to a DBNull via the bound control - so
                                // take control of the update and force the field back to a null (non-nullable fields should show up to the GUI with colored background)...
                                ((DataRowView)b.BindingManagerBase.Current).Row[b.BindingMemberInfo.BindingField] = DBNull.Value;
                                b.ReadValue();
                                e.Handled = true;
                            }
                            else
                            {
                                if (_sharedUtils.LookupTablesIsValidFKField(((DataRowView)b.BindingManagerBase.Current).Row.Table.Columns[b.BindingMemberInfo.BindingField]))
                                {
                                    LookupTablePicker ltp = new LookupTablePicker(_sharedUtils, tb.Tag.ToString(), ((DataRowView)b.BindingManagerBase.Current).Row, remainingText);
                                    ltp.StartPosition = FormStartPosition.CenterParent;
                                    if (DialogResult.OK == ltp.ShowDialog())
                                    {
                                        tb.Text = ltp.NewValue.Trim();
                                        b.WriteValue();
                                        e.Handled = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region DGV control logic...
        private void ux_datagridview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
 
            DataView dv = ((DataTable)((BindingSource)dgv.DataSource).DataSource).DefaultView;
            if (dv != null && e.ColumnIndex > -1)
            {
                DataColumn dc = dv.Table.Columns[e.ColumnIndex];
                if (_sharedUtils.LookupTablesIsValidFKField(dc) && 
                    e.RowIndex < dv.Count &&
                    dv[e.RowIndex].Row.RowState != DataRowState.Deleted)
                {
                    if (dv[e.RowIndex][e.ColumnIndex] != DBNull.Value)
                    {
                        //e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
                        e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
                    }
                    dgv[e.ColumnIndex, e.RowIndex].ErrorText = dv[e.RowIndex].Row.GetColumnError(dc);
                    e.FormattingApplied = true;
                }

                if (dc.ReadOnly)
                {
                    e.CellStyle.BackColor = Color.LightGray;
                }

                if (dc.ExtendedProperties.Contains("is_nullable") &&
                    dc.ExtendedProperties["is_nullable"].ToString() == "N" && 
                    string.IsNullOrEmpty(dv[e.RowIndex][e.ColumnIndex].ToString()))
                {
                    e.CellStyle.BackColor = Color.Plum;
                }
            }
        }

        private void ux_datagridview_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
            string columnName = dgv.CurrentCell.OwningColumn.Name;
            DataColumn dc = dt.Columns[columnName];
            DataRow dr;

            if (_sharedUtils.LookupTablesIsValidFKField(dc))
            {
                //string luTableName = dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim();
                string luTableName = dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim();
                dr = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                string suggestedFilter = dgv.CurrentCell.EditedFormattedValue.ToString();
//if (_lastDGVCharPressed > 0) suggestedFilter = _lastDGVCharPressed.ToString();
////GRINGlobal.Client.Common.LookupTablePicker ltp = new GRINGlobal.Client.Common.LookupTablePicker(lookupTables, columnName, dr, suggestedFilter);
                GRINGlobal.Client.Common.LookupTablePicker ltp = new GRINGlobal.Client.Common.LookupTablePicker(_sharedUtils, columnName, dr, suggestedFilter);
//_lastDGVCharPressed = (char)0;
ltp.StartPosition = FormStartPosition.CenterParent;
                if (DialogResult.OK == ltp.ShowDialog())
                {
                    if (dr != null)
                    {
                        if (ltp.NewKey != null && dr[dgv.CurrentCell.ColumnIndex].ToString().Trim() != ltp.NewKey.Trim())
                        {
                            dr[dgv.CurrentCell.ColumnIndex] = ltp.NewKey.Trim();
                            dgv.CurrentCell.Value = ltp.NewValue.Trim();
                        }
                        else if (ltp.NewKey == null)
                        {
                            dr[dgv.CurrentCell.ColumnIndex] = DBNull.Value;
                            dgv.CurrentCell.Value = "";
                        }
                        dr.SetColumnError(dgv.CurrentCell.ColumnIndex, null);
                    }
                }
                dgv.EndEdit();
            }
        }

        private void ux_datagridview_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            string errorMessage = e.Exception.Message;
            int columnWithError = -1;

            // Find the cell the error belongs to (don't use e.ColumnIndex because it points to the current cell *NOT* the offending cell)...
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (errorMessage.Contains(col.Name))
                {
                    dgv[col.Name, e.RowIndex].ErrorText = errorMessage;
                    columnWithError = col.Index;
                }
            }
        }
        #endregion 

        #region Binding Navigator Logic...
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            // Pre-populate the form with default values...
//ux_textboxSite.Enabled = false;
            ux_textboxSite.Text = _sharedUtils.UserSite;
            //((DataRowView)_orderRequestBindingSource.Current)["site"] = _sharedUtils.UserSite;
            ux_comboboxOrderType.SelectedValue = "DI";
            ux_comboboxStatus.SelectedValue = "NEW";
            ux_textboxOrderedDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            ((DataRowView)_orderRequestBindingSource.Current)["ordered_date"] = DateTime.Today.ToString("MM/dd/yyyy");
            ux_checkboxIsCompleted.Checked = false;
            ux_checkboxIsSupplyLow.Checked = false;
        }

        private void bindingNavigatorSaveButton_Click(object sender, EventArgs e)
        {
//SaveOrderData();
//RefreshOrderData();

            int errorCount = 0;
            errorCount = SaveOrderData();
            if (errorCount == 0)
            {
//MessageBox.Show(this, "All data was saved successfully", "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("All data was saved successfully", "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "OrderWizard_bindingNavigatorSaveButtonMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
            }
            else
            {
//MessageBox.Show(this, "The data being saved has errors that should be reviewed.\n\n  Error Count: " + errorCount, "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxIcon.Warning);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The data being saved has errors that should be reviewed.\n\n  Error Count: {0}", "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "OrderWizard_bindingNavigatorSaveButtonMessage2";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
ggMessageBox.ShowDialog();
            }

            // Force the row filters to be applied to order items and order actions...
            _orderRequestBindingSource_CurrentChanged(sender, e);
        }

        private void bindingNavigatorSaveAndExitButton_Click(object sender, EventArgs e)
        {
            //SaveOrderData();
            //RefreshOrderData();
            //this.Close();
            int errorCount = 0;
            errorCount = SaveOrderData();
            if (errorCount == 0)
            {
//MessageBox.Show(this, "All data was saved successfully", "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("All data was saved successfully", "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "OrderWizard_bindingNavigatorSaveButtonMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
                this.Close();
            }
            else
            {
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The data being saved has errors that should be reviewed.\n\nWould you like to review them now?\n\nClick Yes to review the errors now.\n(Click No to abandon the errors and exit the Order Wizard).\n\n  Error Count: {0}", "Order Wizard Data Save Results", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "OrderWizard_bindingNavigatorSaveButtonMessage3";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
//if (DialogResult.No == MessageBox.Show(this, "The data being saved has errors that should be reviewed.\n\nWould you like to review them now?\n\nClick Yes to review the errors now.\n(Click No to abandon the errors and exit the Order Wizard).\n\n  Error Count: " + errorCount, "Order Wizard Data Save Results", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
if (DialogResult.No == ggMessageBox.ShowDialog())
                {
                    this.Close();
                }
                else
                {
                    // Update the row error message for this accession row...
                    if (string.IsNullOrEmpty(((DataRowView)_orderRequestBindingSource.Current).Row.RowError))
                    {
                        ux_textboxOrderRequestRowError.Visible = false;
                        ux_textboxOrderRequestRowError.Text = "";
                    }
                    else
                    {
                        ux_textboxOrderRequestRowError.Visible = true;
                        ux_textboxOrderRequestRowError.ReadOnly = false;
                        ux_textboxOrderRequestRowError.Enabled = true;
                        ux_textboxOrderRequestRowError.Text = ((DataRowView)_orderRequestBindingSource.Current).Row.RowError;
                    }
                }
            }
        }

        private void RefreshOrderData()
        {
            // Refresh the Order data...
//DataSet ds;
//// Refresh the order_request table and bind it to the main form on the General tabpage...
//ds = _sharedUtils.GetWebServiceData("get_order_request", _orderRequestPKeys, 0, 0);
//if (ds.Tables.Contains("get_order_request"))
//{
//    _orderRequest = ds.Tables["get_order_request"].Copy();
//    _orderRequestBindingSource.DataSource = _orderRequest;
//}

//// Get the order_request_item table and bind it to the main form on the General tabpage...
//ds = _sharedUtils.GetWebServiceData("get_order_request_item", _orderRequestPKeys, 0, 0);
//if (ds.Tables.Contains("get_order_request_item"))
//{
//    _orderRequestItem = ds.Tables["get_order_request_item"].Copy();
//    ux_datagridviewOrderRequestItem.DataSource = _orderRequestItemBindingSource;
//    _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestItem, _orderRequestItem);
//}
            DataSet ds;
            string dataviewParameters = "";
            string orderRequestPKeys = "";

            if (ux_radiobuttonMyOrders.Checked)
            {
                dataviewParameters = ":ownedby=" + _sharedUtils.UserCooperatorID;
            }
            else if (ux_radiobuttonMySitesOrders.Checked)
            {
                string siteCooperatorIDs = "";
                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled AND site = @site", "@accountisenabled=Y; @site=" + _sharedUtils.UserSite);
                foreach (DataRow dr in dt.Rows)
                {
                    siteCooperatorIDs += dr["value_member"].ToString() + ",";
                }
                siteCooperatorIDs = siteCooperatorIDs.TrimEnd(',');
                dataviewParameters = ":ownedby=" + siteCooperatorIDs;
            }
            else if (ux_radiobuttonAllSitesOrders.Checked)
            {
                string allCooperatorIDs = "";
                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled", "@accountisenabled=Y;");
                foreach (DataRow dr in dt.Rows)
                {
                    allCooperatorIDs += dr["value_member"].ToString() + ",";
                }
                allCooperatorIDs = allCooperatorIDs.TrimEnd(',');
                dataviewParameters = ":ownedby=" + allCooperatorIDs;
            }
            else // ux_radiobuttonSelectionOrders.Checked must be true...
            {
                dataviewParameters = _originalPKeys;
            }

            // Add on status filters to the dataview parameters...
            dataviewParameters += ";:orderitemstatus=" + _orderRequestStatusFilter;

            // Get the order_request table and bind it to the main form on the General tabpage...
            ds = _sharedUtils.GetWebServiceData("get_order_request", dataviewParameters, 0, 100);
            if (ds.Tables.Contains("get_order_request"))
            {
                _orderRequest = ds.Tables["get_order_request"].Copy();
//_orderRequestBindingSource.DataSource = _orderRequest;
                // Build a list of order_request_ids to use for gathering the order_request_items...
                orderRequestPKeys = ":orderrequestid=";
                foreach (DataRow dr in _orderRequest.Rows)
                {
                    orderRequestPKeys += dr["order_request_id"].ToString() + ",";
                }
                orderRequestPKeys = orderRequestPKeys.TrimEnd(',');
            }
            else
            {
                _orderRequest = new DataTable();
            }

            // Get the order_request_item table and bind it to the main form on the General tabpage...
            ds = _sharedUtils.GetWebServiceData("get_order_request_item", orderRequestPKeys, 0, 0);
            if (ds.Tables.Contains("get_order_request_item"))
            {
                _orderRequestItem = ds.Tables["get_order_request_item"].Copy();
                ux_datagridviewOrderRequestItem.DataSource = _orderRequestItemBindingSource;
                _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestItem, _orderRequestItem);
            }
            else
            {
                _orderRequestItem = new DataTable();
            }

            // Re-bind the bindingsource to the new table of web orders...
            // NOTE: this will force a change in the child table filter and this should not happen
            //       until all of the child table DGV have been built, so this should be done last...
            _orderRequestBindingSource.DataSource = _orderRequest;
        }

        private void RefreshWebOrderData()
        {
            // Refresh the Web Order data...
            DataSet ds;
            string dataviewParameters = "";
            string webOrderRequestPKeys = "";

            if (ux_radiobuttonMyWebOrders.Checked)
            {
                dataviewParameters = ":ownedby=" + _sharedUtils.UserCooperatorID;
            }
            else if (ux_radiobuttonMySitesWebOrders.Checked)
            {
                string siteCooperatorIDs = "";
                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled AND site = @site", "@accountisenabled=Y; @site=" + _sharedUtils.UserSite);
                foreach (DataRow dr in dt.Rows)
                {
                    siteCooperatorIDs += dr["value_member"].ToString() + ",";
                }
                siteCooperatorIDs = siteCooperatorIDs.TrimEnd(',');
                dataviewParameters = ":ownedby=" + siteCooperatorIDs;
            }
            else if (ux_radiobuttonAllSitesWebOrders.Checked)
            {
                string allCooperatorIDs = "";
                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled", "@accountisenabled=Y;");
                foreach (DataRow dr in dt.Rows)
                {
                    allCooperatorIDs += dr["value_member"].ToString() + ",";
                }
                allCooperatorIDs = allCooperatorIDs.TrimEnd(',');
                dataviewParameters = ":ownedby=" + allCooperatorIDs;
            }
            else // ux_radiobuttonSelectionOrders.Checked must be true...
            {
                dataviewParameters = _originalPKeys;
            }

            // Add on status filters to the dataview parameters...
            dataviewParameters += ";:orderstatus=" + _webOrderRequestStatusFilter;

            // Get the web_order_request table and bind it to the main form on the OrdersPage tabpage...
            ds = _sharedUtils.GetWebServiceData("get_web_order_request", dataviewParameters, 0, 100);
            if (ds.Tables.Contains("get_web_order_request"))
            {
                _webOrderRequest = ds.Tables["get_web_order_request"].Copy();
//_webOrderRequestBindingSource.DataSource = _webOrderRequest;
                // Build a list of web_order_request_ids to use for gathering the web_order_request_items...
                webOrderRequestPKeys = ":weborderrequestid=";
                foreach (DataRow dr in _webOrderRequest.Rows)
                {
                    webOrderRequestPKeys += dr["web_order_request_id"].ToString() + ",";
                }
                webOrderRequestPKeys = webOrderRequestPKeys.TrimEnd(',');
            }
            else
            {
                _webOrderRequest = new DataTable();
            }

            // Get the web_order_request_item table and bind it to the dgv on the WebOrdersPage tabpage...
            ds = _sharedUtils.GetWebServiceData("get_web_order_request_item", webOrderRequestPKeys, 0, 0);
            if (ds.Tables.Contains("get_web_order_request_item"))
            {
                _webOrderRequestItem = ds.Tables["get_web_order_request_item"].Copy();
                ux_datagridviewWebOrderRequestItem.DataSource = _webOrderRequestItemBindingSource;
                _sharedUtils.BuildReadOnlyDataGridView(ux_datagridviewWebOrderRequestItem, _webOrderRequestItem);
            }
            else
            {
                _webOrderRequestItem = new DataTable();
            }

            // Re-bind the bindingsource to the new table of web orders...
            // NOTE: this will force a change in the child table filter and this should not happen
            //       until all of the child table DGV have been built, so this should be done last...
            _webOrderRequestBindingSource.DataSource = _webOrderRequest;

            // Enable (or disable) the buttons for creating/deleting orders and cooperators...
            if (_webOrderRequest != null &&
                _webOrderRequest.Rows.Count > 0)
            {
                ux_buttonCreateOrderRequest.Enabled = true;
                ux_buttonCreateCooperator.Enabled = true;
                ux_buttonCancelWebOrderRequest.Enabled = true;
                bindingNavigatorAddNewItem1.Enabled = false;
                bindingNavigatorDeleteItem1.Enabled = false;
            }
            else
            {
                ux_buttonCreateOrderRequest.Enabled = false;
                ux_buttonCreateCooperator.Enabled = false;
                ux_buttonCancelWebOrderRequest.Enabled = false;
                bindingNavigatorAddNewItem1.Enabled = false;
                bindingNavigatorDeleteItem1.Enabled = false;
            }
        }

        private int SaveOrderData()
        {
            int errorCount = 0;
            DataSet orderRequestChanges = new DataSet();
            DataSet orderRequestSaveResults = new DataSet();
            DataSet orderRequestItemChanges = new DataSet();
            DataSet orderRequestItemSaveResults = new DataSet();
            DataSet orderRequestActionChanges = new DataSet();
            DataSet orderRequestActionSaveResults = new DataSet();
            DataSet webOrderRequestChanges = new DataSet();
            DataSet webOrderRequestSaveResults = new DataSet();

            // Process Order Requests...
            // Make sure the last edited row in the Order Header Form has been commited to the datatable...
            _orderRequestBindingSource.EndEdit();

            // Make sure the navigator is not still editing a cell...
            foreach (DataRowView drv in _orderRequestBindingSource.List)
            {
                if (drv.IsEdit ||
                    drv.Row.RowState == DataRowState.Added ||
                    drv.Row.RowState == DataRowState.Deleted ||
                    drv.Row.RowState == DataRowState.Detached ||
                    drv.Row.RowState == DataRowState.Modified)
                {

                    drv.EndEdit();
                    //drv.Row.ClearErrors();
                }
            }

            // Get the changes (if any) for the order_request table and commit them to the remote database...
            if (_orderRequest.GetChanges() != null)
            {
                orderRequestChanges.Tables.Add(_orderRequest.GetChanges());
                // Save the changes to the remote server...
                orderRequestSaveResults = _sharedUtils.SaveWebServiceData(orderRequestChanges);
                if (orderRequestSaveResults.Tables.Contains(_orderRequest.TableName))
                {
                    errorCount += SyncSavedResults(_orderRequest, orderRequestSaveResults.Tables[_orderRequest.TableName]);
                }
            }

            // Process Order Request Items...
            // Make sure no DGV cells are being edited...
            ux_datagridviewOrderRequestItem.EndEdit();
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _orderRequestItemBindingSource.EndEdit();

            // Get the changes (if any) for the order_request_item table and commit them to the remote database...
            if (_orderRequestItem.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in order_request_item have
                // a FK related to a new row in the order_request table (pkey < 0).  If so get the new pkey returned from
                // the get_order_reqeust save and update the records in order_request_item...
                DataRow[] orderRequestItemRowsWithNewParent = _orderRequestItem.Select("order_request_id<0");
                foreach (DataRow dr in orderRequestItemRowsWithNewParent)
                {
                    // "OriginalPrimaryKeyID" "NewPrimaryKeyID"
                    DataRow[] newParent = orderRequestSaveResults.Tables["get_order_request"].Select("OriginalPrimaryKeyID=" + dr["order_request_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["order_request_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }

                orderRequestItemChanges.Tables.Add(_orderRequestItem.GetChanges());
ScrubData(orderRequestItemChanges);

// Before saving the results to the remote server check to see if any order item rows status_code column has changed to 'SHIPPED'
// and if so autodeduct the inventory (when auto_deduct is set in the inventory table)...
foreach (DataRow dr in orderRequestItemChanges.Tables[_orderRequestItem.TableName].Rows)
{
    // For status changes of 'SHIPPED' - auto deduct the inventory amount...
    if (dr.Table.Columns.Contains("status_code") &&
        dr.Table.Columns.Contains("inventory_id") &&
        dr["status_code", DataRowVersion.Current].ToString().Trim().ToUpper() == "SHIPPED" &&
        (dr.RowState == DataRowState.Added ||
        (dr.RowState == DataRowState.Modified && dr["status_code", DataRowVersion.Original] != dr["status_code", DataRowVersion.Current])))
    {
        // Go get the inventory record...
        DataSet orderItemInventoryRow = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=" + dr["inventory_id"], 0, 0);
        if (orderItemInventoryRow != null &&
            orderItemInventoryRow.Tables.Contains("get_inventory") &&
            orderItemInventoryRow.Tables["get_inventory"].Rows.Count > 0)
        {
            foreach (DataRow inventoryRow in orderItemInventoryRow.Tables["get_inventory"].Rows)
            {
                if (orderItemInventoryRow.Tables["get_inventory"].Columns.Contains("quantity_on_hand") &&
                    orderItemInventoryRow.Tables["get_inventory"].Columns.Contains("quantity_on_hand_unit_code") &&
                    inventoryRow["quantity_on_hand_unit_code"].ToString().Trim().ToUpper() == dr["quantity_shipped_unit_code"].ToString().Trim().ToUpper() &&
                    orderItemInventoryRow.Tables["get_inventory"].Columns.Contains("is_auto_deducted") &&
                    inventoryRow["is_auto_deducted"].ToString().Trim().ToUpper() == "Y")
                {
                    // Deduct the inventory ammount if distribution units match...
                    inventoryRow["quantity_on_hand"] = (decimal)inventoryRow["quantity_on_hand"] - (decimal)dr["quantity_shipped"];
                    bool origReadOnlyValue = _orderRequestItem.Columns["quantity_on_hand"].ReadOnly;
                    _orderRequestItem.Columns["quantity_on_hand"].ReadOnly = false;
                    _orderRequestItem.Rows.Find(dr[dr.Table.PrimaryKey[0]])["quantity_on_hand"] = inventoryRow["quantity_on_hand"];
                    _orderRequestItem.Columns["quantity_on_hand"].ReadOnly = origReadOnlyValue;
                }
            }
            // Save the inventory deductions to the remote server...
            _sharedUtils.SaveWebServiceData(orderItemInventoryRow);
        }
    }

    // For any status changes - set the status date...
    if (dr.Table.Columns.Contains("status_code") &&
        (dr.RowState == DataRowState.Modified && dr["status_code", DataRowVersion.Original] != dr["status_code", DataRowVersion.Current]))
    {
        // Since the order request item's status has changed set the status date to UTC Now()...
        dr["status_date"] = DateTime.UtcNow;
        _orderRequestItem.Rows.Find(dr[dr.Table.PrimaryKey[0]])["status_date"] = dr["status_date"];
    }
}

                // Save the order request item changes to the remote server...
                orderRequestItemSaveResults = _sharedUtils.SaveWebServiceData(orderRequestItemChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (orderRequestItemSaveResults.Tables.Contains(_orderRequestItem.TableName))
                {
                    errorCount += SyncSavedResults(_orderRequestItem, orderRequestItemSaveResults.Tables[_orderRequestItem.TableName]);
                }
            }

            // Process Order Request Actions...
            // Make sure no DGV cells are being edited...
            ux_datagridviewOrderRequestAction.EndEdit();
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _orderRequestActionBindingSource.EndEdit();

            // Get the changes (if any) for the order_request_action table and commit them to the remote database...
            if (_orderRequestAction.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in order_request_item have
                // a FK related to a new row in the order_request table (pkey < 0).  If so get the new pkey returned from
                // the get_order_reqeust save and update the records in order_request_item...
                DataRow[] orderRequestActionRowsWithNewParent = _orderRequestAction.Select("order_request_id<0");
                foreach (DataRow dr in orderRequestActionRowsWithNewParent)
                {
                    // "OriginalPrimaryKeyID" "NewPrimaryKeyID"
                    DataRow[] newParent = orderRequestSaveResults.Tables["get_order_request"].Select("OriginalPrimaryKeyID=" + dr["order_request_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["order_request_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }

                orderRequestActionChanges.Tables.Add(_orderRequestAction.GetChanges());
ScrubData(orderRequestActionChanges);

                // Save the order request action changes to the remote server...
                orderRequestActionSaveResults = _sharedUtils.SaveWebServiceData(orderRequestActionChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (orderRequestActionSaveResults.Tables.Contains(_orderRequestAction.TableName))
                {
                    errorCount += SyncSavedResults(_orderRequestAction, orderRequestActionSaveResults.Tables[_orderRequestAction.TableName]);
                }
            }



// Process Web Order Requests...
// Make sure the last edited value in the Form has been commited to the datatable...
_webOrderRequestBindingSource.EndEdit();

// Get the changes (if any) for the web_order_request table and commit them to the remote database...
if (_webOrderRequest.GetChanges() != null)
{
    webOrderRequestChanges.Tables.Add(_webOrderRequest.GetChanges());
    ScrubData(webOrderRequestChanges);

    // Save the web order request changes to the remote server...
    webOrderRequestSaveResults = _sharedUtils.SaveWebServiceData(webOrderRequestChanges);
    // Sync the saved results with the original table (to give user feedback about results of save)...
    if (webOrderRequestSaveResults.Tables.Contains(_webOrderRequest.TableName))
    {
//errorCount += SyncSavedResults(_webOrderRequest, webOrderRequestSaveResults.Tables[_webOrderRequest.TableName]);
        SyncSavedResults(_webOrderRequest, webOrderRequestSaveResults.Tables[_webOrderRequest.TableName]);
    }
}


            
            // Now add the new changes to the _changedRecords dataset (this data will be passed back to the calling program)...
            if (orderRequestSaveResults != null && orderRequestSaveResults.Tables.Contains(_orderRequest.TableName))
            {
                string pkeyName = orderRequestSaveResults.Tables[_orderRequest.TableName].PrimaryKey[0].ColumnName;
                bool origColumnReadOnlyValue = orderRequestSaveResults.Tables[_orderRequest.TableName].Columns[pkeyName].ReadOnly;
                foreach (DataRow dr in orderRequestSaveResults.Tables[_orderRequest.TableName].Rows)
                {
                    dr.Table.Columns[pkeyName].ReadOnly = false;
                    dr[pkeyName] = dr["NewPrimaryKeyID"];
                    dr.AcceptChanges();
                }
                orderRequestSaveResults.Tables[_orderRequest.TableName].Columns[pkeyName].ReadOnly = origColumnReadOnlyValue;

                if (_changedRecords.Tables.Contains(_orderRequest.TableName))
                {
                    // If the saved results table exists - update or insert the new records...
                    _changedRecords.Tables[_orderRequest.TableName].Load(orderRequestSaveResults.Tables[_orderRequest.TableName].CreateDataReader(), LoadOption.Upsert);
                    _changedRecords.Tables[_orderRequest.TableName].AcceptChanges();

                }
                else
                {
                    // If the saved results table doesn't exist - create it (and include the new records)...
                    _changedRecords.Tables.Add(orderRequestSaveResults.Tables[_orderRequest.TableName].Copy());
                    _changedRecords.AcceptChanges();
                }
            }

            return errorCount;
        }

        private void ScrubData(DataSet ds)
        {
            // Make sure all non-nullable fields do not contain a null value - if they do, replace it with the default value...
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ExtendedProperties.Contains("is_nullable") &&
                            dc.ExtendedProperties["is_nullable"].ToString().Trim().ToUpper() == "N" &&
                            dr[dc] == DBNull.Value)
                        {
                            if (dc.ExtendedProperties.Contains("default_value") &&
                                !string.IsNullOrEmpty(dc.ExtendedProperties["default_value"].ToString()) &&
                                dc.ExtendedProperties["default_value"].ToString().Trim().ToUpper() != "{DBNULL.VALUE}")
                            {
                                dr[dc] = dc.ExtendedProperties["default_value"].ToString();
                            }
                        }
                    }
                }
            }
        }

        private int SyncSavedResults(DataTable originalTable, DataTable savedResults)
        {
            int errorCount = 0;

            if (savedResults != null && savedResults.PrimaryKey.Length == 1)
            {
                string pKeyCol = savedResults.PrimaryKey[0].ColumnName.Trim().ToUpper();
                savedResults.Columns[pKeyCol].ReadOnly = false;
                foreach (DataRow dr in savedResults.Rows)
                {
                    DataRow originalRow = originalTable.Rows.Find(dr["OriginalPrimaryKeyID"]);

                    switch (dr["SavedAction"].ToString())
                    {
                        case "Insert":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                // Set the originalTable row's status for this new row to committed (and update the pkey with the int returned from the server DB)...
                                if (originalRow != null)
                                {
                                    bool origColumnReadOnlyValue = originalRow.Table.Columns[pKeyCol].ReadOnly;
                                    originalRow.Table.Columns[pKeyCol].ReadOnly = false;
                                    originalRow[pKeyCol] = dr["NewPrimaryKeyID"];
                                    originalRow.AcceptChanges();
                                    originalRow.Table.Columns[pKeyCol].ReadOnly = origColumnReadOnlyValue;
                                    originalRow.ClearErrors();
                                }
                            }
                            else
                            {
                                errorCount++;
                                if (originalRow != null) originalRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                            }
                            break;
                        case "Update":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                originalRow.AcceptChanges();
                                originalRow.ClearErrors();
                            }
                            else
                            {
                                errorCount++;
                                if (originalRow != null) originalRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                            }
                            break;
                        case "Delete":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                // Set the row's status for this deleted row to committed...
                                if (originalRow != null)
                                {
                                    originalRow.AcceptChanges();
                                    originalRow.ClearErrors();
                                }
                            }
                            else
                            {
                                errorCount++;
                                // Find the deleted row (NOTE: datatable.rows.find() method does not work on deleted rows)...
                                foreach (DataRow deletedRow in originalTable.Rows)
                                {
                                    if (deletedRow[0, DataRowVersion.Original].Equals(dr["OriginalPrimaryKeyID"]))
                                    {
                                        deletedRow.RejectChanges();
                                        deletedRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return errorCount;
        }

        void _orderRequestBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
//formatControls(this.Controls);
            formatControls(OrderPage.Controls, _orderRequestBindingSource);
        }

        void _orderRequestBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (_orderRequestBindingSource.List.Count > 0)
            {
//ux_tabcontrolMain.Enabled = true;
OrderPage.Show();
ActionsPage.Show();
OrderPage.Enabled = true;
ActionsPage.Enabled = true;
                // Update the row error message for this accession row...
                if (string.IsNullOrEmpty(((DataRowView)_orderRequestBindingSource.Current).Row.RowError))
                {
                    ux_textboxOrderRequestRowError.Visible = false;
                    ux_textboxOrderRequestRowError.Text = "";
                }
                else
                {
                    ux_textboxOrderRequestRowError.Visible = true;
                    ux_textboxOrderRequestRowError.ReadOnly = false;
                    ux_textboxOrderRequestRowError.Enabled = true;
                    ux_textboxOrderRequestRowError.Text = ((DataRowView)_orderRequestBindingSource.Current).Row.RowError;
                }
                string pkey = ((DataRowView)_orderRequestBindingSource.Current)[_orderRequest.PrimaryKey[0].ColumnName].ToString();
                if (_orderRequestItem != null && !string.IsNullOrEmpty(pkey)) _orderRequestItem.DefaultView.RowFilter = "order_request_id=" + pkey.Trim().ToLower();
                if (_orderRequestAction != null && !string.IsNullOrEmpty(pkey)) _orderRequestAction.DefaultView.RowFilter = "order_request_id=" + pkey.Trim().ToLower();
                bindingNavigatorOrderNumber.Text = ((DataRowView)_orderRequestBindingSource.Current).Row["order_request_id"].ToString();
                bindingNavigatorItems.Text = ux_datagridviewOrderRequestItem.Rows.Count.ToString();
            }
            else
            {
//ux_tabcontrolMain.Enabled = false;
OrderPage.Hide();
ActionsPage.Hide();
OrderPage.Enabled = false;
ActionsPage.Enabled = false;
            }
        }

        void _webOrderRequestBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (_webOrderRequestBindingSource.List.Count > 0)
            {
                string pkey = ((DataRowView)_webOrderRequestBindingSource.Current)[_webOrderRequest.PrimaryKey[0].ColumnName].ToString();
//if (_webOrderRequestItem != null && !string.IsNullOrEmpty(pkey)) _webOrderRequestItem.DefaultView.RowFilter = "web_order_request_id=" + pkey.Trim().ToLower();
                if (_webOrderRequestItemBindingSource != null && !string.IsNullOrEmpty(pkey)) ((DataTable)_webOrderRequestItemBindingSource.DataSource).DefaultView.RowFilter = "web_order_request_id=" + pkey.Trim().ToLower();
            }
        }
        
        #endregion

        #region Tab Control Logic...
        private void ux_tabcontrolMain_SelectedIndexChanged(object sender, EventArgs e)
        {
//if (ux_tabcontrolMain.SelectedIndex == 0)
            if (ux_tabcontrolMain.SelectedTab == OrderPage)
            {
                bindingNavigatorAddNewItem.Enabled = true;
                bindingNavigatorDeleteItem.Enabled = true;
                ux_groupboxOrderFilters.Visible = true;
                ux_groupboxWebOrderFilters.Visible = false;
            }
            else if (ux_tabcontrolMain.SelectedTab == WebOrderPage)
            {
                bindingNavigatorAddNewItem.Enabled = false;
                bindingNavigatorDeleteItem.Enabled = false;
                ux_groupboxOrderFilters.Visible = false;
                ux_groupboxWebOrderFilters.Visible = true;
                bindingNavigatorAddNewItem1.Enabled = false;
                bindingNavigatorDeleteItem1.Enabled = false;
            }
            else
            {
                bindingNavigatorAddNewItem.Enabled = false;
                bindingNavigatorDeleteItem.Enabled = false;
                ux_groupboxOrderFilters.Visible = true;
                ux_groupboxWebOrderFilters.Visible = false;
//if(((DataRowView)_mainBindingSource.Current).Row.RowState == DataRowState.Detached) ((DataRowView)_mainBindingSource.Current).EndEdit();
                foreach (DataRowView drv in _orderRequestBindingSource.List)
                {
                    if (drv.IsEdit ||
                        drv.Row.RowState == DataRowState.Added ||
                        drv.Row.RowState == DataRowState.Deleted ||
                        drv.Row.RowState == DataRowState.Detached ||
                        drv.Row.RowState == DataRowState.Modified)
                    {
                        drv.EndEdit();
                        //drv.Row.ClearErrors();
                    }
                }
            }
        }
        #endregion

        #region Web Order Request logic...
        private void BuildWebOrderRequestPage()
        {
            DataSet ds;
            // Get the order_request_action table and bind it to the DGV on the Names tabpage...
            ds = _sharedUtils.GetWebServiceData("get_order_request_action", _originalPKeys, 0, 0);
            if (ds.Tables.Contains("get_order_request_action"))
            {
                // Copy the order_request_action table to a private variable...
                _orderRequestAction = ds.Tables["get_order_request_action"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewOrderRequestAction.DataSource = _orderRequestActionBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestAction, _orderRequestAction);

                // Order and display the columns the way the user wants...
                int i = 0;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_NAME_CODE")) ux_datagridviewOrderRequestAction.Columns["ACTION_NAME_CODE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("STARTED_DATE")) ux_datagridviewOrderRequestAction.Columns["STARTED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("STARTED_DATE_CODE")) ux_datagridviewOrderRequestAction.Columns["STARTED_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("COMPLETED_DATE")) ux_datagridviewOrderRequestAction.Columns["COMPLETED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("COMPLETED_DATE_CODE")) ux_datagridviewOrderRequestAction.Columns["COMPLETED_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_INFORMATION")) ux_datagridviewOrderRequestAction.Columns["ACTION_INFORMATION"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_COST")) ux_datagridviewOrderRequestAction.Columns["ACTION_COST"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("COOPERATOR_ID")) ux_datagridviewOrderRequestAction.Columns["COOPERATOR_ID"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("NOTE")) ux_datagridviewOrderRequestAction.Columns["NOTE"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestAction.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }
            }
        }

        #endregion

        #region Order Request Action logic...
        private void BuildOrderRequestActionsPage()
        {
            DataSet ds;
            // Get the order_request_action table and bind it to the DGV on the Names tabpage...
            ds = _sharedUtils.GetWebServiceData("get_order_request_action", _originalPKeys, 0, 0);
            if (ds.Tables.Contains("get_order_request_action"))
            {
                // Copy the order_request_action table to a private variable...
                _orderRequestAction = ds.Tables["get_order_request_action"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewOrderRequestAction.DataSource = _orderRequestActionBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestAction, _orderRequestAction);

                // Order and display the columns the way the user wants...
                int i = 0;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_NAME_CODE")) ux_datagridviewOrderRequestAction.Columns["ACTION_NAME_CODE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("STARTED_DATE")) ux_datagridviewOrderRequestAction.Columns["STARTED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("STARTED_DATE_CODE")) ux_datagridviewOrderRequestAction.Columns["STARTED_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("COMPLETED_DATE")) ux_datagridviewOrderRequestAction.Columns["COMPLETED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("COMPLETED_DATE_CODE")) ux_datagridviewOrderRequestAction.Columns["COMPLETED_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_INFORMATION")) ux_datagridviewOrderRequestAction.Columns["ACTION_INFORMATION"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_COST")) ux_datagridviewOrderRequestAction.Columns["ACTION_COST"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("COOPERATOR_ID")) ux_datagridviewOrderRequestAction.Columns["COOPERATOR_ID"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("NOTE")) ux_datagridviewOrderRequestAction.Columns["NOTE"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestAction.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }

//int i = 5;
//foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestAction.Columns)
//{
//    switch (dgvc.Name.Trim().ToUpper())
//    {
//        case "ACTION_NAME":
//            dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
//            dgvc.DisplayIndex = 0;
//            break;
//        case "ACTED_DATE":
//            dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
//            dgvc.DisplayIndex = 1;
//            break;
//        case "ACTION_FOR_ID":
//            dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
//            dgvc.DisplayIndex = 2;
//            break;
//        case "COOPERATOR_ID":
//            dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
//            dgvc.DisplayIndex = 3;
//            break;
//        case "NOTE":
//            dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
//            dgvc.DisplayIndex = 4;
//            break;
//        default:
//            dgvc.Visible = false;
//            dgvc.DisplayIndex = System.Math.Min(i++, ux_datagridviewOrderRequestAction.Columns.Count - 1);
//            break;
//    }
//}
            }
        }

        private void ux_buttonNewOrderRequestActionRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_orderRequestBindingSource.Current)[_orderRequest.PrimaryKey[0].ColumnName].ToString();
            DataRow newOrderRequestAction = _orderRequestAction.NewRow();
            newOrderRequestAction["order_request_id"] = pkey;
            _orderRequestAction.Rows.Add(newOrderRequestAction);
            //ux_datagridviewOrderRequestAction.CurrentCell = ux_datagridviewOrderRequestAction.Rows[ux_datagridviewOrderRequestAction.Rows.GetLastRow(DataGridViewElementStates.Displayed)].Cells["name"];
            int newRowIndex = ux_datagridviewOrderRequestAction.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewOrderRequestAction.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewOrderRequestAction.Rows.Count; i++)
            {
                if (ux_datagridviewOrderRequestAction["order_request_action_id", i].Value.Equals(newOrderRequestAction["order_request_action_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestAction.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewOrderRequestAction.CurrentCell = ux_datagridviewOrderRequestAction[newColIndex, newRowIndex];
        }
        #endregion

        #region Order Items DGV logic...
        private void ux_datagridviewOrderRequestItem_DragOver(object sender, DragEventArgs e)
        {
            // Okay we are in the middle of a Drag and Drop operation and the mouse is in 
            // the DGV control so lets handle this event...

            // This code will change the cursor icon to give the user feedback about whether or not
            // the drag-drop operation is allowed...
            //

            // Get the DGV object...
            DataGridView dgv = (DataGridView)sender;

            // Convert the mouse coordinates from screen to client...
            Point ptClientCoord = dgv.PointToClient(new Point(e.X, e.Y));

            // Is this a string being dragged to the DGV...
            if (e.Data.GetDataPresent(typeof(string)) && !dgv.ReadOnly)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(typeof(DataSet)) && !dgv.ReadOnly)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ux_datagridviewOrderRequestItem_DragDrop(object sender, DragEventArgs e)
        {
            // The drag-drop event is coming to a close process this event to handle the dropping of
            // data into the treeview...

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Get the DGV object...
            DataGridView dgv = (DataGridView)sender;
            DataTable destinationTable = (DataTable)((BindingSource)dgv.DataSource).DataSource;

            // Is this an allowed drop???
            if (e.Effect != DragDropEffects.None)
            {
                if (e.Data.GetDataPresent(typeof(DataSet)) && e.Effect != DragDropEffects.None)
                {
                    // Is this a collection of dataset rows being dragged to the DGV...
                    DataSet dndData = (DataSet)e.Data.GetData(typeof(DataSet));
                    DataTable sourceTable = dndData.Tables[0];
                    if (sourceTable.PrimaryKey.Length == 1)
                    {
                        if (sourceTable.PrimaryKey[0].ColumnName.ToUpper() == "INVENTORY_ID")
                        {
                            foreach (DataRow dr in sourceTable.Rows)
                            {
                                DataRow newOrderItem = BuildOrderRequestItemRow(dr[sourceTable.PrimaryKey[0].ColumnName].ToString(), destinationTable);
                                if (newOrderItem != null) destinationTable.Rows.Add(newOrderItem);
                            }
                        }
                        else if (sourceTable.PrimaryKey[0].ColumnName.ToUpper() == "ACCESSION_ID")
                        {
                            foreach (DataRow dr in sourceTable.Rows)
                            {
                                string inventoryID = FindInventoryFromAccession(dr[sourceTable.PrimaryKey[0].ColumnName].ToString());
                                DataRow newOrderItem = BuildOrderRequestItemRow(inventoryID, destinationTable);
                                if (newOrderItem != null) destinationTable.Rows.Add(newOrderItem);
                            }
                        }
                    }
                }
                else if (e.Data.GetDataPresent(typeof(string)))
                {
                    // Is this a string being dragged to the DGV...
                    char[] rowDelimiters = new char[] { '\r', '\n' };
                    char[] columnDelimiters = new char[] { '\t', ' ', ',' };
                    string rawText = (string)e.Data.GetData(typeof(string));
                    int badRows = 0;
                    int missingRows = 0;
                    bool importSuccess = false;
                    importSuccess = ImportTextToDataTableUsingAltKeys(rawText, destinationTable, rowDelimiters, columnDelimiters, out badRows, out missingRows);
                }
            }

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private bool ImportTextToDataTableUsingAltKeys(string rawImportText, DataTable destinationTable, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows)
        {
            string[] rawImportRows = rawImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
            bool processedImportSuccessfully = false;
            badRows = 0;
            missingRows = 0;
            // Make sure there is text to process - if not bail out now...
            if (rawImportRows == null || rawImportRows.Length <= 0) return false;

            // Now start processing the rows...
            for (int i = 0; i < rawImportRows.Length; i++)
            {
                // Split the row into id parts - then reassemble it using only space delimiter...
                string distributionInventoryID = "";
                string[] idParts = rawImportRows[i].Split(columnDelimiters, StringSplitOptions.None);
                string fullID = "";
                foreach (string idPart in idParts)
                {
                    fullID += " " + idPart.Trim();
                }
                // Try to find a matching accession number first...
                string accessionPKey = _sharedUtils.GetLookupValueMember(null, "accession_lookup", fullID.Trim(), "", "");
                if (!string.IsNullOrEmpty(accessionPKey))
                {
                    // Found an accession number - now decide which inventory row to use...
                    distributionInventoryID = FindInventoryFromAccession(accessionPKey);
                    #region old_code...
//                    DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + accessionPKey, 0, 0);
//                    if (ds.Tables.Contains("get_inventory") &&
//                        ds.Tables["get_inventory"].Rows.Count > 0)
//                    {
//                        // Make sure the rows are ordered oldest to newest...
//                        ds.Tables["get_inventory"].DefaultView.Sort = "inventory_id ASC";

//                        // Try to find a row that is marked as distributable and has a status of available from the user's site...
//                        if (string.IsNullOrEmpty(distributionInventoryID))
//                        {
////if (string.IsNullOrEmpty(_sharedUtils.UserSite))
////{
////    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
////        ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
////    {
////        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
////    }
////}
////else
////{
////    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
////        ds.Tables["get_inventory"].Columns.Contains("availability_status_code") &&
////        ds.Tables["get_inventory"].Columns.Contains("site"))
////    {
////        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL' AND site='" + _sharedUtils.UserSite + "'";
////    }
////}
//                            if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
//                                ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
//                            {
//                                ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
//                            }
//                            if (ds.Tables["get_inventory"].Columns.Contains("site") &&
//                                string.IsNullOrEmpty(_sharedUtils.UserSite))
//                            {
//                                ds.Tables["get_inventory"].DefaultView.RowFilter += " AND site='" + _sharedUtils.UserSite + "'";
//                            }
//                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
//                            {
//                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
//                            }
//                        }
//                        // Couldn't find a row using above criteria - try to find a row that is marked as distributable from the user's site...
//                        if (string.IsNullOrEmpty(distributionInventoryID))
//                        {
////if (string.IsNullOrEmpty(_sharedUtils.UserSite))
////{
////    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
////    {
////        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
////    }
////}
////else
////{
////    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
////       ds.Tables["get_inventory"].Columns.Contains("site"))
////    {
////        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND site='" + _sharedUtils.UserSite + "'";
////    }
////}
//                            if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
//                            {
//                                ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
//                            }
//                            if (ds.Tables["get_inventory"].Columns.Contains("site") &&
//                                string.IsNullOrEmpty(_sharedUtils.UserSite))
//                            {
//                                ds.Tables["get_inventory"].DefaultView.RowFilter += " AND site='" + _sharedUtils.UserSite + "'";
//                            }
//                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
//                            {
//                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
//                            }
//                        }
//                        // Couldn't find a row using above criteria - try to find a row that is from the user's site...
//                        if (string.IsNullOrEmpty(distributionInventoryID))
//                        {
////if (!string.IsNullOrEmpty(_sharedUtils.UserSite))
////{
////    if (ds.Tables["get_inventory"].Columns.Contains("site"))
////    {
////        ds.Tables["get_inventory"].DefaultView.RowFilter = "site='" + _sharedUtils.UserSite + "'";
////    }
////}
//                            if (ds.Tables["get_inventory"].Columns.Contains("site") &&
//                                string.IsNullOrEmpty(_sharedUtils.UserSite))
//                            {
//                                ds.Tables["get_inventory"].DefaultView.RowFilter = "site='" + _sharedUtils.UserSite + "'";
//                            }
//                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
//                            {
//                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
//                            }
//                        }
//                        // Couldn't find a row using above criteria - try to find a row that is marked as distributable and has a status of available from any site...
//                        if (string.IsNullOrEmpty(distributionInventoryID))
//                        {
//                            if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
//                               ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
//                            {
//                                ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
//                            }
//                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
//                            {
//                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
//                            }
//                        }
//                        // Couldn't find a row using above criteria - try to find a row that is marked available from any site...
//                        if (string.IsNullOrEmpty(distributionInventoryID))
//                        {
//                            if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
//                            {
//                                ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
//                            }
//                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
//                            {
//                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
//                            }
//                        }
//                        // Couldn't find a row using above criteria - use the first row found...
//                        if (string.IsNullOrEmpty(distributionInventoryID))
//                        {
//                            ds.Tables["get_inventory"].DefaultView.RowFilter = "";
//                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
//                            {
//                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
//                            }
//                        }
                    //                    }
                    #endregion
                }
                else
                {
                    // Couldn't find an accession number so try to find an inventory number...
                    distributionInventoryID = _sharedUtils.GetLookupValueMember(null, "inventory_lookup", fullID.Trim(), "", "");
                }
                // If an inventory id was found add a new row to the order items...
                if (!string.IsNullOrEmpty(distributionInventoryID))
                {
                    DataRow newOrderItem = BuildOrderRequestItemRow(distributionInventoryID, destinationTable);
                    #region old_code...
                    //// Create a new order_request_item row...
                    //DataRow newOrderItem = destinationTable.NewRow();
                    //// Find the maximum sequence number for this order...
                    //int maxSequence = 1;
                    //if (destinationTable.DefaultView.Count > 0)
                    //{
                    //    string currentSort = destinationTable.DefaultView.Sort;
                    //    destinationTable.DefaultView.Sort = "sequence_number DESC";
                    //    if (int.TryParse(destinationTable.DefaultView[0]["sequence_number"].ToString(), out maxSequence))
                    //    {
                    //        maxSequence++;
                    //    }
                    //    else
                    //    {
                    //        maxSequence = 1;
                    //    }
                    //    destinationTable.DefaultView.Sort = currentSort;
                    //}
                    //// Go get all the inventory data for the chosen distribution inventory lot...
                    //DataSet distributionInventory = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=" + distributionInventoryID, 0, 0);
                    //if (distributionInventory.Tables.Contains("get_inventory") &&
                    //   distributionInventory.Tables["get_inventory"].Rows.Count > 0)
                    //{
                    //    // Now populate the new order_request_item row with the inventory default distribution data...
                    //    DataRow inventoryDataRow = distributionInventory.Tables["get_inventory"].Rows[0];
                    //    newOrderItem["order_request_id"] = ((DataRowView)_orderRequestBindingSource.Current)[_orderRequest.PrimaryKey[0].ColumnName].ToString();
                    //    newOrderItem["sequence_number"] = maxSequence;
                    //    newOrderItem["name"] = inventoryDataRow["accession_name"].ToString();
                    //    newOrderItem["quantity_shipped"] = inventoryDataRow["distribution_default_quantity"].ToString();
                    //    newOrderItem["quantity_shipped_unit_code"] = inventoryDataRow["distribution_unit_code"].ToString();
                    //    newOrderItem["distribution_form_code"] = inventoryDataRow["distribution_default_form_code"].ToString();
                    //    newOrderItem["availability_status_code"] = inventoryDataRow["availability_status_code"].ToString();
                    //    newOrderItem["inventory_id"] = distributionInventoryID;
                    //    newOrderItem["accession_id"] = inventoryDataRow["accession_id"].ToString();
                    //    newOrderItem["taxonomy_species_id"] = inventoryDataRow["taxonomy_species_id"].ToString();
                    //    // Go get all the accession data for the chosen distribution inventory lot...
                    //    DataSet distributionAccessionIPR = _sharedUtils.GetWebServiceData("get_accession_ipr", ":accessionid=" + inventoryDataRow["accession_id"].ToString(), 0, 0);
                    //    if (distributionAccessionIPR.Tables.Contains("get_accession_ipr") &&
                    //       distributionAccessionIPR.Tables["get_accession_ipr"].Rows.Count > 0)
                    //    {
                    //        // Now populate the new order_request_item row with the accession data...
                    //        distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.Sort = "accession_ipr_id ASC";
                    //        distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.RowFilter = "expired_date is null";
                    //        if (distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.Count > 0)
                    //        {
                    //            DataRow accessionIPRDataRow = distributionAccessionIPR.Tables["get_accession_ipr"].Rows[0];
                    //            newOrderItem["ipr_restriction"] = accessionIPRDataRow["assigned_type"].ToString();
                    //        }
                    //    }
                    //}
                    #endregion
                    destinationTable.Rows.Add(newOrderItem);
                    processedImportSuccessfully = true;
                }
            }
            return processedImportSuccessfully;
        }

        private bool ImportTextToDataTableUsingBlockStyle(string rawImportText, DataGridView dgv, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows)
        {
            bool processedImportSuccessfully = true;
            DataTable destinationTable = (DataTable)((BindingSource)dgv.DataSource).DataSource;
            string[] rawImportRows = rawImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
            string[] tempColumns = null;
            string newImportText = "";
            string newImportRowText = "";
            badRows = 0;
            missingRows = 0;

            // If the DGV does not have a currently active cell bail out now...
            if (dgv.CurrentCell == null) return false;
            // If the import string is empty bail out now...
            if (string.IsNullOrEmpty(rawImportText) || rawImportRows.Length < 1) return false;

            // Okay we need to build a new importText string that has column headers that include the friendly names for the primary key columns
            // and the friendly names for the dgv columns starting at the currenly active cell in the dgv...  Why are we doing this?  Because
            // we are going to pass this new importText string off to the 'ImportTextToDataTableUsingKeys' method, and since that method
            // requires a primary key or alternate pkey we are going to get them from the dgv starting at the current row of the current cell...

            // Step 1 - Determine the number of rows and columns in the incoming rawImportText (to use later for building the new ImportText string)...
            int rawImportRowCount = 0;
            int rawImportColCount = 0;
            // Estimate the number of rows and columns in the import text (assumes a rectangular shape)
            if (rawImportRows != null && rawImportRows.Length > 0)
            {
                rawImportRowCount = rawImportRows.Length;
                tempColumns = rawImportRows[0].Split(columnDelimiters, StringSplitOptions.None);
                if (tempColumns != null && tempColumns.Length > 0)
                {
                    rawImportColCount = tempColumns.Length;
                }
            }

            int minSelectedCol = dgv.Columns.Count;
            int maxSelectedCol = -1;
            int minSelectedRow = dgv.Rows.Count;
            int maxSelectedRow = -1;
            // Check to see if the datagridview's selected cells contains the CurrentCell
            // and if so use the selected cells as the destination cells...

            // Find the bounding rectangle for the selected cells...
            if (dgv.SelectedCells.Count == 1)
            {
                minSelectedCol = dgv.CurrentCell.ColumnIndex;
                maxSelectedCol = dgv.CurrentCell.ColumnIndex + rawImportColCount - 1;
                minSelectedRow = dgv.CurrentCell.RowIndex;
                maxSelectedRow = dgv.CurrentCell.RowIndex + rawImportRowCount - 1;
            }
            else
            {
                foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                {
                    if (dgvc.ColumnIndex < minSelectedCol) minSelectedCol = dgvc.ColumnIndex;
                    if (dgvc.ColumnIndex > maxSelectedCol) maxSelectedCol = dgvc.ColumnIndex;
                    if (dgvc.RowIndex < minSelectedRow) minSelectedRow = dgvc.RowIndex;
                    if (dgvc.RowIndex > maxSelectedRow) maxSelectedRow = dgvc.RowIndex;
                }
                if ((maxSelectedCol - minSelectedCol) < (rawImportColCount - 1)) maxSelectedCol = minSelectedCol + rawImportColCount - 1;
                if ((maxSelectedRow - minSelectedRow) < (rawImportRowCount - 1)) maxSelectedRow = minSelectedRow + rawImportRowCount - 1;
            }

            string modifiedImportText = "";
            // Now fill (or clip) the import data to fit the selected cells...
            for (int iSelectedRow = 0; iSelectedRow <= (maxSelectedRow - minSelectedRow); iSelectedRow++)
            {
                // 
                tempColumns = rawImportRows[iSelectedRow % rawImportRowCount].Split(columnDelimiters, StringSplitOptions.None);
                for (int iSelectedCol = 0; iSelectedCol <= (maxSelectedCol - minSelectedCol); iSelectedCol++)
                {
                    //
                    modifiedImportText += tempColumns[iSelectedCol % rawImportColCount] + "\t";
                }
                // Strip the last tab character and add a CR LF...
                modifiedImportText = modifiedImportText.Substring(0, modifiedImportText.Length - 1) + "\r\n";
            }

            // Step 2 - Get the primary key column names for the new column header row text...
            if (destinationTable.PrimaryKey.Length > 0)
            {
                foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
                {
                    newImportText += _sharedUtils.GetFriendlyFieldName(pKeyColumn, pKeyColumn.ColumnName) + "\t";
                }
            }

            // Step 3 - Continue adding friendly column names to the import text (starting with the column name of the current cell's column HeaderText)...
            //DataGridViewColumn currColumn = dgv.CurrentCell.OwningColumn;
            DataGridViewColumn currColumn = dgv.Columns[minSelectedCol];
            // Step 4 - Now repeat this process for each additional column in the rawImportText...
            //foreach(string tempCol in tempColumns)
            for (int i = 0; i < Math.Max(rawImportColCount, maxSelectedCol - minSelectedCol + 1); i++)
            {
                if (currColumn != null)
                {
                    newImportText += currColumn.HeaderText + "\t";
                }
                else
                {
                    newImportText += "\t";
                }
                // Try to find the next visible column...
                currColumn = dgv.Columns.GetNextColumn(currColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.Frozen);
            }
            // Strip the last tab character and add a CR LF...
            newImportText = newImportText.Substring(0, newImportText.Length - 1) + "\r\n";

            // Step 5 - Get the primary key for each row receiving pasted text and prepend it to the orginal import raw text...
            string[] modifiedImportRows = modifiedImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
            ////DataGridViewRow currRow = dgv.CurrentCell.OwningRow;
            DataGridViewRow currRow = dgv.Rows[minSelectedRow];
            int nextRowIndex = currRow.Index;
            for (int i = 0; i < modifiedImportRows.Length; i++)
            {
                newImportRowText = "";
                if (currRow != null)
                {
                    if (destinationTable.PrimaryKey.Length > 0)
                    {
                        foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
                        {
                            newImportRowText += ((DataRowView)currRow.DataBoundItem).Row[pKeyColumn].ToString() + "\t";
                        }
                    }
                    // Now add the original import row text to the new import row text...
                    //newImportRowText += rawImportRows[i] + "\r\n";
                    newImportRowText += modifiedImportRows[i] + "\r\n";
                    // And now add it to the new import text string...
                    newImportText += newImportRowText;
                }

                // Finally, try to find the next visible row...
                nextRowIndex = dgv.Rows.GetNextRow(currRow.Index, DataGridViewElementStates.Visible);
                if (nextRowIndex != -1 &&
                    !dgv.Rows[nextRowIndex].IsNewRow &&
                    nextRowIndex >= minSelectedRow &&
                    nextRowIndex <= maxSelectedRow)
                {
                    currRow = dgv.Rows[nextRowIndex];
                }
                else
                {
                    // Looks like we hit the end of the rows in the DGV - bailout now...
                    //currRow = null;
                    break;
                }
            }

            // Step 6 - Now that we have built a new ImportText string that contains pkeys, we can pass it off to the 'ImportTextToDataTableUsingKeys' 
            processedImportSuccessfully = _sharedUtils.ImportTextToDataTableUsingKeys(newImportText, destinationTable, rowDelimiters, columnDelimiters, out badRows, out missingRows);

            return processedImportSuccessfully;
        }

        private string FindInventoryFromAccession(string accessionPKey)
        {
            string distributionInventoryID = "";
            DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + accessionPKey, 0, 0);
            if (ds.Tables.Contains("get_inventory") &&
                ds.Tables["get_inventory"].Rows.Count > 0)
            {
                // Make sure the rows are ordered oldest to newest...
                ds.Tables["get_inventory"].DefaultView.Sort = "inventory_id ASC";

                // Try to find a row that is marked as distributable and has a status of available from the user's site...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
//if (string.IsNullOrEmpty(_sharedUtils.UserSite))
//{
//    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
//        ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
//    {
//        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
//    }
//}
//else
//{
//    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
//        ds.Tables["get_inventory"].Columns.Contains("availability_status_code") &&
//        ds.Tables["get_inventory"].Columns.Contains("site"))
//    {
//        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL' AND site='" + _sharedUtils.UserSite + "'";
//    }
//}
                    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                        ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
                    }
                    if (ds.Tables["get_inventory"].Columns.Contains("owner_site_id") &&
                        string.IsNullOrEmpty(_sharedUtils.UserSite))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter += " AND owner_site_id='" + _sharedUtils.UserSite + "'";
                    }
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
                // Couldn't find a row using above criteria - try to find a row that is marked as distributable from the user's site...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
//if (string.IsNullOrEmpty(_sharedUtils.UserSite))
//{
//    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
//    {
//        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
//    }
//}
//else
//{
//    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
//       ds.Tables["get_inventory"].Columns.Contains("site"))
//    {
//        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND site='" + _sharedUtils.UserSite + "'";
//    }
//}
                    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
                    }
                    if (ds.Tables["get_inventory"].Columns.Contains("owner_site_id") &&
                        string.IsNullOrEmpty(_sharedUtils.UserSite))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter += " AND owner_site_id='" + _sharedUtils.UserSite + "'";
                    }
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
                // Couldn't find a row using above criteria - try to find a row that is from the user's site...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
//if (!string.IsNullOrEmpty(_sharedUtils.UserSite))
//{
//    if (ds.Tables["get_inventory"].Columns.Contains("site"))
//    {
//        ds.Tables["get_inventory"].DefaultView.RowFilter = "site='" + _sharedUtils.UserSite + "'";
//    }
//}
                    if (ds.Tables["get_inventory"].Columns.Contains("owner_site_id") &&
                        string.IsNullOrEmpty(_sharedUtils.UserSite))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter = "owner_site_id='" + _sharedUtils.UserSite + "'";
                    }
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
                // Couldn't find a row using above criteria - try to find a row that is marked as distributable and has a status of available from any site...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
                    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                       ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
                    }
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
                // Couldn't find a row using above criteria - try to find a row that is marked available from any site...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
                    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
                    }
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
                // Couldn't find a row using above criteria - use the first row found...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
                    ds.Tables["get_inventory"].DefaultView.RowFilter = "";
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
            }
            return distributionInventoryID;
        }

        private DataRow BuildOrderRequestItemRow(string distributionInventoryID, DataTable destinationTable)
        {
            // Create a new order_request_item row...
            DataRow newOrderItem = destinationTable.NewRow();
            // Find the maximum sequence number for this order...
            int maxSequence = 1;
            if (destinationTable.DefaultView.Count > 0)
            {
                string currentSort = destinationTable.DefaultView.Sort;
                destinationTable.DefaultView.Sort = "sequence_number DESC";
                if (int.TryParse(destinationTable.DefaultView[0]["sequence_number"].ToString(), out maxSequence))
                {
                    maxSequence++;
                }
                else
                {
                    maxSequence = 1;
                }
                destinationTable.DefaultView.Sort = currentSort;
            }
            // Go get all the inventory data for the chosen distribution inventory lot...
            DataSet distributionInventory = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=" + distributionInventoryID, 0, 0);
            if (distributionInventory.Tables.Contains("get_inventory") &&
               distributionInventory.Tables["get_inventory"].Rows.Count > 0)
            {
                // Now populate the new order_request_item row with the inventory default distribution data...
                DataTable dt = distributionInventory.Tables["get_inventory"];
                DataRow inventoryDataRow = dt.Rows[0];
                newOrderItem["order_request_id"] = ((DataRowView)_orderRequestBindingSource.Current)[_orderRequest.PrimaryKey[0].ColumnName].ToString();
                newOrderItem["sequence_number"] = maxSequence;
                if(dt.Columns.Contains("accession_id")) newOrderItem["accession_id"] = inventoryDataRow["accession_id"].ToString();
                newOrderItem["inventory_id"] = distributionInventoryID;
                if (newOrderItem.Table.Columns.Contains("name") && dt.Columns.Contains("accession_name") && inventoryDataRow["accession_name"] != DBNull.Value) newOrderItem["name"] = inventoryDataRow["accession_name"].ToString();
                if (newOrderItem.Table.Columns.Contains("taxonomy_species_id") && dt.Columns.Contains("taxonomy_species_id") && inventoryDataRow["taxonomy_species_id"] != DBNull.Value) newOrderItem["taxonomy_species_id"] = inventoryDataRow["taxonomy_species_id"].ToString();
                if (newOrderItem.Table.Columns.Contains("quantity_on_hand") && dt.Columns.Contains("quantity_on_hand") && inventoryDataRow["quantity_on_hand"] != DBNull.Value) newOrderItem["quantity_on_hand"] = inventoryDataRow["quantity_on_hand"].ToString();
                if (newOrderItem.Table.Columns.Contains("quantity_on_hand_unit_code") && dt.Columns.Contains("quantity_on_hand_unit_code") && inventoryDataRow["quantity_on_hand_unit_code"] != DBNull.Value) newOrderItem["quantity_on_hand_unit_code"] = inventoryDataRow["quantity_on_hand_unit_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("quantity_shipped") && dt.Columns.Contains("distribution_default_quantity") && inventoryDataRow["distribution_default_quantity"] != DBNull.Value) newOrderItem["quantity_shipped"] = inventoryDataRow["distribution_default_quantity"].ToString();
                if (newOrderItem.Table.Columns.Contains("quantity_shipped_unit_code") && dt.Columns.Contains("distribution_unit_code") && inventoryDataRow["distribution_unit_code"] != DBNull.Value) newOrderItem["quantity_shipped_unit_code"] = inventoryDataRow["distribution_unit_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("distribution_form_code") && dt.Columns.Contains("distribution_default_form_code") && inventoryDataRow["distribution_default_form_code"] != DBNull.Value) newOrderItem["distribution_form_code"] = inventoryDataRow["distribution_default_form_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("availability_status_code") && dt.Columns.Contains("availability_status_code") && inventoryDataRow["availability_status_code"] != DBNull.Value) newOrderItem["availability_status_code"] = inventoryDataRow["availability_status_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("geography_id") && dt.Columns.Contains("geography_id") && inventoryDataRow["geography_id"] != DBNull.Value) newOrderItem["geography_id"] = inventoryDataRow["geography_id"].ToString();
                if (newOrderItem.Table.Columns.Contains("is_distributable") && dt.Columns.Contains("is_distributable") && inventoryDataRow["is_distributable"] != DBNull.Value) newOrderItem["is_distributable"] = inventoryDataRow["is_distributable"].ToString();
                if (newOrderItem.Table.Columns.Contains("distribution_default_form_code") && dt.Columns.Contains("distribution_default_form_code") && inventoryDataRow["distribution_default_form_code"] != DBNull.Value) newOrderItem["distribution_default_form_code"] = inventoryDataRow["distribution_default_form_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("distribution_unit_code") && dt.Columns.Contains("distribution_unit_code") && inventoryDataRow["distribution_unit_code"] != DBNull.Value) newOrderItem["distribution_unit_code"] = inventoryDataRow["distribution_unit_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("storage_location_part1") && dt.Columns.Contains("storage_location_part1") && inventoryDataRow["storage_location_part1"] != DBNull.Value) newOrderItem["storage_location_part1"] = inventoryDataRow["storage_location_part1"].ToString();
                if (newOrderItem.Table.Columns.Contains("storage_location_part2") && dt.Columns.Contains("storage_location_part2") && inventoryDataRow["storage_location_part2"] != DBNull.Value) newOrderItem["storage_location_part2"] = inventoryDataRow["storage_location_part2"].ToString();
                if (newOrderItem.Table.Columns.Contains("storage_location_part3") && dt.Columns.Contains("storage_location_part3") && inventoryDataRow["storage_location_part3"] != DBNull.Value) newOrderItem["storage_location_part3"] = inventoryDataRow["storage_location_part3"].ToString();
                if (newOrderItem.Table.Columns.Contains("storage_location_part4") && dt.Columns.Contains("storage_location_part4") && inventoryDataRow["storage_location_part4"] != DBNull.Value) newOrderItem["storage_location_part4"] = inventoryDataRow["storage_location_part4"].ToString();
                // Go get all the accession data for the chosen distribution inventory lot...
                DataSet distributionAccessionIPR = _sharedUtils.GetWebServiceData("get_accession_ipr", ":accessionid=" + inventoryDataRow["accession_id"].ToString(), 0, 0);
                if (distributionAccessionIPR.Tables.Contains("get_accession_ipr") &&
                   distributionAccessionIPR.Tables["get_accession_ipr"].Rows.Count > 0)
                {
                    // Now populate the new order_request_item row with the accession data...
                    distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.Sort = "accession_ipr_id ASC";
                    distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.RowFilter = "expired_date is null";
                    if (distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.Count > 0)
                    {
                        DataRow accessionIPRDataRow = distributionAccessionIPR.Tables["get_accession_ipr"].Rows[0];
                        if (newOrderItem.Table.Columns.Contains("ipr_restriction") && accessionIPRDataRow.Table.Columns.Contains("assigned_type") && accessionIPRDataRow["accession_name"] != DBNull.Value) newOrderItem["ipr_restriction"] = accessionIPRDataRow["assigned_type"].ToString();
                    }
                }
            }
            return newOrderItem;
        }

        private void ux_datagridviewOrderRequestItem_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            // Check to see if this cell is in a column that needs a FK lookup...
            DataView dv = ((DataTable)((BindingSource)dgv.DataSource).DataSource).DefaultView;
            if (dv != null && e.ColumnIndex > -1)
                {
                DataColumn dc = dv.Table.Columns[e.ColumnIndex];
                if (_sharedUtils.LookupTablesIsValidFKField(dc) && 
                    e.RowIndex < dv.Count &&
                    dv[e.RowIndex].Row.RowState != DataRowState.Deleted)
                {
                    if (dv[e.RowIndex][e.ColumnIndex] != DBNull.Value)
                    {
                        //e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
                        e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
                    }
                    dgv[e.ColumnIndex, e.RowIndex].ErrorText = dv[e.RowIndex].Row.GetColumnError(dc);
                    e.FormattingApplied = true;
                }

                if (dc.ReadOnly)
                {
                    e.CellStyle.BackColor = Color.LightGray;
                }
            }
        }

        private void ux_datagridviewOrderRequestItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            string errorMessage = e.Exception.Message;
            int columnWithError = -1;

            // Find the cell the error belongs to (don't use e.ColumnIndex because it points to the current cell *NOT* the offending cell)...
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (errorMessage.Contains(col.Name))
                {
                    dgv[col.Name, e.RowIndex].ErrorText = errorMessage;
                    columnWithError = col.Index;
                }
            }
        }

        private void ux_datagridviewOrderRequestItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
            string columnName = dgv.CurrentCell.OwningColumn.Name;
            DataColumn dc = dt.Columns[columnName];
            DataRow dr;

            if (_sharedUtils.LookupTablesIsValidFKField(dc))
            {
                //string luTableName = dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim();
                string luTableName = dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim();
                dr = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                //GrinGlobal.Client.Data.LookupTablePicker ltp = new GrinGlobal.Client.Data.LookupTablePicker(lookupTables, localDBInstance, columnName, dr, dgv.CurrentCell.EditedFormattedValue.ToString());
                string suggestedFilter = dgv.CurrentCell.EditedFormattedValue.ToString();
//if (_lastDGVCharPressed > 0) suggestedFilter = _lastDGVCharPressed.ToString();
                //GRINGlobal.Client.Common.LookupTablePicker ltp = new GRINGlobal.Client.Common.LookupTablePicker(lookupTables, columnName, dr, suggestedFilter);
                GRINGlobal.Client.Common.LookupTablePicker ltp = new GRINGlobal.Client.Common.LookupTablePicker(_sharedUtils, columnName, dr, suggestedFilter);
//_lastDGVCharPressed = (char)0;
ltp.StartPosition = FormStartPosition.CenterParent;
                if (DialogResult.OK == ltp.ShowDialog())
                {
                    if (dr != null)
                    {
                        if (ltp.NewKey != null && dr[dgv.CurrentCell.ColumnIndex].ToString().Trim() != ltp.NewKey.Trim())
                        {
                            dr[dgv.CurrentCell.ColumnIndex] = ltp.NewKey.Trim();
                            dgv.CurrentCell.Value = ltp.NewValue.Trim();
                        }
                        else if (ltp.NewKey == null)
                        {
                            dr[dgv.CurrentCell.ColumnIndex] = DBNull.Value;
                            dgv.CurrentCell.Value = "";
                        }
                        dr.SetColumnError(dgv.CurrentCell.ColumnIndex, null);
                    }
                }
                dgv.EndEdit();
            }
        }

        #endregion

        private void ux_datagridviewOrderRequestItem_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
//// Set the global variables so that later processing will know where to apply the command from the context menu...
//mouseClickDGVColumnIndex = e.ColumnIndex;
//mouseClickDGVRowIndex = e.RowIndex;
            DataGridView dgv = (DataGridView)sender;
            ContextMenuStrip cms = dgv.ContextMenuStrip;
            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;

            // Change the color of the cell background so that the user
            // knows what cell the context menu applies to...
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                // If the user right clicks outside of the currently selected cells - reset the selected cells to the one under the mouser cursor...
                if (!dgv.SelectedCells.Contains(dgv[e.ColumnIndex, e.RowIndex]))
                {
                    dgv.CurrentCell = dgv[e.ColumnIndex, e.RowIndex];
                }
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.Red;
            }

            // Refresh the reports tool strip menu items...
            ux_dgvcellmenuReports.DropDownItems.Clear();
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory()); //System.Environment.GetFolderPath(Environment.SpecialFolder. System.Reflection.Assembly.GetExecutingAssembly()..Location);
            foreach (System.IO.FileInfo fi in di.GetFiles("*.rpt", System.IO.SearchOption.AllDirectories))
            {
                ToolStripItem tsi = ux_dgvcellmenuReports.DropDownItems.Add(fi.Name, null, ux_DGVCellReport_Click);
                tsi.Tag = fi.FullName;
            }

            foreach (object o in cms.Items)
            {
                if (o.GetType() == typeof(ToolStripMenuItem))
                {
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)o;
                    if (tsmi.Tag != null &&
                        !string.IsNullOrEmpty(tsmi.Tag.ToString()) &&
                        dt.Columns.Contains(tsmi.Tag.ToString().Trim().ToLower()) &&
                        dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].ExtendedProperties.Contains("gui_hint") &&
                        dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].ExtendedProperties["gui_hint"].ToString().Trim().ToUpper() == "SMALL_SINGLE_SELECT_CONTROL" &&
                        dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].ExtendedProperties.Contains("group_name") &&
                        !string.IsNullOrEmpty(dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].ExtendedProperties["group_name"].ToString()))
                    {
                        tsmi.Text = dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].Caption + "...";
                        tsmi.DropDownItems.Clear();
                        DataTable menuCodes = _sharedUtils.GetLocalData("SELECT * FROM code_value_lookup WHERE group_name = @groupname", "@groupname=" + dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].ExtendedProperties["group_name"].ToString());
                        if (menuCodes != null &&
                            menuCodes.Rows.Count > 0)
                        {
                            foreach (DataRow dr in menuCodes.Rows)
                            {
                                ToolStripItem tsiCode = tsmi.DropDownItems.Add(dr["display_member"].ToString(), null, ux_AutoDGVColumnCode_Click);
                                tsiCode.Tag = dr["value_member"].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void ux_buttonShipAllRemaining_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in ux_datagridviewOrderRequestItem.Rows)
            {
                if (ux_datagridviewOrderRequestItem.Columns.Contains("status_code") &&
                    ux_datagridviewOrderRequestItem.Columns.Contains("status_date") &&
                    ((DataRowView)dgvr.DataBoundItem).Row["status_code"] == DBNull.Value)
                {
                    ((DataRowView)dgvr.DataBoundItem).Row["status_code"] = "SHIPPED";
                    ((DataRowView)dgvr.DataBoundItem).Row["status_date"] = DateTime.UtcNow;
                }
            }
        }

        private void ux_contextmenustripDGVCell_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
        }

        void ux_AutoDGVColumnCode_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            //ContextMenuStrip cms = (ContextMenuStrip)tsmi.OwnerItem.Owner;
            //DataGridView dgv = (DataGridView)cms.SourceControl;
            DataGridView dgv = ux_datagridviewOrderRequestItem;
            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
            
            if (tsmi.Tag.ToString().Trim().ToUpper() == "SPLIT")
            {
                // Ask the user if they really wanted to split 
                DataRow currentOrderRequest = ((DataRowView)_orderRequestBindingSource.Current).Row;
                List<DataRow> selectedRows = new List<DataRow>();

                if (dgv.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow dgvr in dgv.SelectedRows)
                    {
                        selectedRows.Add(((DataRowView)dgvr.DataBoundItem).Row);
                    }
                }
                else if (dgv.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                    {
                        if (!selectedRows.Contains(((DataRowView)dgvc.OwningRow.DataBoundItem).Row))
                        {
                            selectedRows.Add(((DataRowView)dgvc.OwningRow.DataBoundItem).Row);
                        }
                    }
                }

GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You are about to split {0} items from this order.\n\nAre you sure you want to do this?", "Split Order Confirmation", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "OrderWizard_ux_AutoDGVColumnCodeMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, selectedRows.Count.ToString());
//if (DialogResult.Yes == MessageBox.Show("You are about to split " + selectedRows.Count.ToString() + " items from this order.\n\nAre you sure you want to do this?", "Split Order Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
if (DialogResult.Yes == ggMessageBox.ShowDialog())
                {
GRINGlobal.Client.Common.GGMessageBox ggMessageBox1 = new GRINGlobal.Client.Common.GGMessageBox("You must save this order before you can split it.\n\nWould you like to do this now?", "Save Data Confirmation", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox1.Name = "OrderWizard_ux_AutoDGVColumnCodeMessage2";
_sharedUtils.UpdateControls(ggMessageBox1.Controls, ggMessageBox.Name);
//if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] < 0 &&
//    DialogResult.Yes == MessageBox.Show("You must save this order before you can split it.\n\nWould you like to do this now?", "Save Data Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] < 0 &&
    DialogResult.Yes == ggMessageBox1.ShowDialog())
                    {
                        int errorCount = SaveOrderData();
                    }

                    if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] > 0)
                    {
                        // First create a copy of the Order Request record...
                        DataRow newOrderRequest = _orderRequest.NewRow();
                        if (_orderRequest.Columns.Contains("original_order_request_id")) newOrderRequest["original_order_request_id"] = currentOrderRequest["order_request_id"];
                        if (_orderRequest.Columns.Contains("web_order_request_id")) newOrderRequest["web_order_request_id"] = currentOrderRequest["web_order_request_id"];
                        if (_orderRequest.Columns.Contains("order_type_code")) newOrderRequest["order_type_code"] = currentOrderRequest["order_type_code"];
                        if (_orderRequest.Columns.Contains("ordered_date")) newOrderRequest["ordered_date"] = currentOrderRequest["ordered_date"];
                        if (_orderRequest.Columns.Contains("intended_use_code")) newOrderRequest["intended_use_code"] = currentOrderRequest["intended_use_code"];
                        if (_orderRequest.Columns.Contains("intended_use_note")) newOrderRequest["intended_use_note"] = currentOrderRequest["intended_use_note"];
                        if (_orderRequest.Columns.Contains("requestor_cooperator_id")) newOrderRequest["requestor_cooperator_id"] = currentOrderRequest["requestor_cooperator_id"];
                        if (_orderRequest.Columns.Contains("ship_to_cooperator_id")) newOrderRequest["ship_to_cooperator_id"] = currentOrderRequest["ship_to_cooperator_id"];
                        if (_orderRequest.Columns.Contains("final_recipient_cooperator_id")) newOrderRequest["final_recipient_cooperator_id"] = currentOrderRequest["final_recipient_cooperator_id"];
                        if (_orderRequest.Columns.Contains("order_obtained_via")) newOrderRequest["order_obtained_via"] = currentOrderRequest["order_obtained_via"];
                        if (_orderRequest.Columns.Contains("special_instruction")) newOrderRequest["special_instruction"] = currentOrderRequest["special_instruction"];
                        if (_orderRequest.Columns.Contains("note")) newOrderRequest["note"] = currentOrderRequest["note"];
                        if (_orderRequest.Columns.Contains("owner_site_id")) newOrderRequest["owner_site_id"] = currentOrderRequest["owner_site_id"];
                        // Now add it to the Order Request Table...
                        _orderRequest.Rows.Add(newOrderRequest);

                        // Finally move each Order Request Item record selected to the new Order Request record...
                        foreach (DataRow dr in selectedRows)
                        {
                            if (dr.Table.Columns.Contains("order_request_id")) dr["order_request_id"] = newOrderRequest["order_request_id"];
                            if (dr.Table.Columns.Contains("status_code")) dr["status_code"] = DBNull.Value;
                            if (dr.Table.Columns.Contains("status_date")) dr["status_date"] = DBNull.Value;
                        }
                    }
                }
            }
            else
            {
                // Iterate through the selected cells and modify the data in the code column associated with this context menu item...
                foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                {
                    object pKey = dgvc.OwningRow.Cells[dt.PrimaryKey[0].ColumnName].Value;
                    DataRow dr = dt.Rows.Find(pKey);
                    if (dr != null)
                    {
                        string codeColumnName = tsmi.OwnerItem.Tag.ToString().Trim().ToLower();
                        string code = tsmi.Tag.ToString().Trim().ToUpper();
                        if (!string.IsNullOrEmpty(codeColumnName) &&
                            !string.IsNullOrEmpty(code))
                        {
                            if (dt.Columns.Contains(codeColumnName)) dr[codeColumnName] = code;
                        }
                    }
                }
            }
        }

        void ux_DGVCellReport_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            //ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            //DataGridView dgv = (DataGridView)cms.SourceControl;
            DataGridView dgv = ux_datagridviewOrderRequestItem;
            DataTable dt = ((DataTable)((BindingSource)dgv.DataSource).DataSource).Clone();

            // NOTE: because of the way the DGV adds rows to the selectedRows collection
            //       we have to process the rows in the opposite direction they were selected in...
            int rowStart = 0;
            int rowStop = dgv.SelectedRows.Count;
            int stepValue = 1;
            if (dgv.SelectedRows.Count > 1 && dgv.SelectedRows[0].Index > dgv.SelectedRows[1].Index)
            {
                rowStart = dgv.SelectedRows.Count - 1;
                rowStop = -1;
                stepValue = -1;
            }

            DataGridViewRow dgvrow = null;
            // Process the rows in the opposite direction they were selected by the user...
            for (int i = rowStart; i != rowStop; i += stepValue)
            {
                dgvrow = dgv.SelectedRows[i];
                if (!dgvrow.IsNewRow)
                {
                    dt.Rows.Add(((DataRowView)dgvrow.DataBoundItem).Row.ItemArray);
                }
            }

            string fullPathName = tsmi.Tag.ToString();
            if (System.IO.File.Exists(fullPathName))
            {
                //ReportForm crustyReport = new ReportForm(dt, fullPathName);
                //crustyReport.StartPosition = FormStartPosition.CenterParent;
                //crustyReport.ShowDialog();
            }
        }

        private void RefreshDGVFormatting(DataGridView dgv)
        {
            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
                RefreshDGVRowFormatting(dgvr);
            }
        }

        private void RefreshDGVRowFormatting(DataGridViewRow dgvr)
        {
            foreach (DataGridViewCell dgvc in dgvr.Cells)
            {
                // Reset the background and foreground color...
                dgvc.Style.BackColor = Color.Empty;
                dgvc.Style.ForeColor = Color.Empty;
                dgvc.Style.SelectionBackColor = Color.Empty;
                dgvc.Style.SelectionForeColor = Color.Empty;
            }
            // If the row has changes make each changed cell yellow...
            DataRow dr = ((DataRowView)dgvr.DataBoundItem).Row;
            if (dr.RowState == DataRowState.Modified)
            {
                foreach (DataGridViewCell dgvc in dgvr.Cells)
                {
                    // If the cell has been changed make it yellow...
                    if (!dr[dgvc.ColumnIndex, DataRowVersion.Original].Equals(dr[dgvc.ColumnIndex, DataRowVersion.Current]))
                    {
                        dgvc.Style.BackColor = Color.Yellow;
                        dr.SetColumnError(dgvc.ColumnIndex, null);
                    }
                    // Use default background color for this cell...
                    else
                    {
                        dgvc.Style.BackColor = Color.Empty;
                    }
                }
            }
        }

        private void ux_datagridview_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            //_sharedUtils.ProcessDGVEditShortcutKeys(dgv, e, _sharedUtils.UserCooperatorID, ImportTextToDataTableUsingAltKeys, ImportTextToDataTableUsingBlockStyle);
            if (_sharedUtils.ProcessDGVEditShortcutKeys(dgv, e, _sharedUtils.UserCooperatorID))
            {
                RefreshDGVFormatting(dgv);
            }
        }

        private void ux_radiobuttonOrderFilter_CheckedChanged(object sender, EventArgs e)
        {
            // This event is fired for radio buttons that are being checked and unchecked
            // so ignore the event for radio buttons that are being unchecked...
            if (((RadioButton)sender).Checked)
            {
                int intRowEdits = 0;

                _orderRequestBindingSource.EndEdit();
                if (_orderRequest.GetChanges() != null) intRowEdits = _orderRequest.GetChanges().Rows.Count;
                _orderRequestItemBindingSource.EndEdit();
                if (_orderRequestItem.GetChanges() != null) intRowEdits += _orderRequestItem.GetChanges().Rows.Count;
                _orderRequestActionBindingSource.EndEdit();
                if (_orderRequestAction.GetChanges() != null) intRowEdits += _orderRequestAction.GetChanges().Rows.Count;
//_webOrderRequestBindingSource.EndEdit();
//if (_webOrderRequest.GetChanges() != null) intRowEdits += _webOrderRequest.GetChanges().Rows.Count;

                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have {0} unsaved row change(s) that will be lost.\n\nWould you like to save them now?", "Save Edits", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "OrderWizard_ux_radiobuttonOrderFilterMessage1";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, intRowEdits);
                if (intRowEdits > 0 && DialogResult.Yes == ggMessageBox.ShowDialog())
                {
                    SaveOrderData();
                }

                // If the Order tab is active, refresh the Order data...
                if (ux_tabcontrolMain.SelectedTab == OrderPage) RefreshOrderData();
                // If the Web Order tab is active, refresh the Web Order data...
                if (ux_tabcontrolMain.SelectedTab == WebOrderPage) RefreshWebOrderData();
            }
        }

        private void ux_checkedlistboxOrderItemStatus_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ux_tabcontrolMain.SelectedTab == OrderPage)
            {
                _orderRequestStatusFilter = "";
                // Process the listbox item that is changing by peeking at its 'new value'...
                for (int i = 0; i < ux_checkedlistboxOrderItemStatus.Items.Count; i++)
                {
                    if (i != e.Index &&
                        ux_checkedlistboxOrderItemStatus.GetItemChecked(i))
                    {
                        // Add all listbox items that are checked...
                        _orderRequestStatusFilter += "'" + _orderRequestStatusCodes.Rows[i]["value_member"].ToString() + "',";
                    }
                    else if (i == e.Index &&
                        e.NewValue == CheckState.Checked)
                    {
                        // Process the listbox item that is changing differently because its new checked state is not saved until after this
                        // event has finished processing - so determine if it should be added to the list by peeking at its 'new value'...
                        _orderRequestStatusFilter += "'" + _orderRequestStatusCodes.Rows[i]["value_member"].ToString() + "',";
                    }
                }
                // Drop the extra semicolon at the end of the string...
                _orderRequestStatusFilter = _orderRequestStatusFilter.TrimEnd(',');

                // Refresh the order data...
                RefreshOrderData();
            }
        }

        private void ux_checkedlistboxWebOrderItemStatus_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ux_tabcontrolMain.SelectedTab == WebOrderPage)
            {
                _webOrderRequestStatusFilter = "";
                // Process the listbox item that is changing by peeking at its 'new value'...
                for (int i = 0; i < ux_checkedlistboxWebOrderItemStatus.Items.Count; i++)
                {
                    if (i != e.Index &&
                        ux_checkedlistboxWebOrderItemStatus.GetItemChecked(i))
                    {
                        // Add all listbox items that are checked...
                        _webOrderRequestStatusFilter += "'" + _webOrderRequestStatusCodes.Rows[i]["value_member"].ToString() + "',";
                    }
                    else if (i == e.Index &&
                        e.NewValue == CheckState.Checked)
                    {
                        // Process the listbox item that is changing differently because its new checked state is not saved until after this
                        // event has finished processing - so determine if it should be added to the list by peeking at its 'new value'...
                        _webOrderRequestStatusFilter += "'" + _webOrderRequestStatusCodes.Rows[i]["value_member"].ToString() + "',";
                    }
                }
                // Drop the extra semicolon at the end of the string...
                _webOrderRequestStatusFilter = _webOrderRequestStatusFilter.TrimEnd(',');

                // Refresh the web order data...
                RefreshWebOrderData();
            }
        }

        private void ux_buttonCreateOrderRequest_Click(object sender, EventArgs e)
        {
            // If there is no active row in the web_order_request DGV - bail out now...
            if (ux_bindingNavigatorWebOrders.BindingSource.Current == null) return;
            
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Create a new order_request record...
            DataRow newOrderRequest = ((DataTable)ux_bindingnavigatorForm.BindingSource.DataSource).NewRow();
            ((DataTable)ux_bindingnavigatorForm.BindingSource.DataSource).Rows.Add(newOrderRequest);
            // Move to it in the DGV...
            ux_bindingnavigatorForm.BindingSource.MoveLast();

            // Populate the new order_request record with data from the web_order_request record...
            DataRow webOrderRequest = ((DataRowView)ux_bindingNavigatorWebOrders.BindingSource.Current).Row;
            if (newOrderRequest.Table.Columns.Contains("web_order_request_id") && webOrderRequest.Table.Columns.Contains("web_order_request_id")) newOrderRequest["web_order_request_id"] = webOrderRequest["web_order_request_id"];
            if (newOrderRequest.Table.Columns.Contains("ordered_date") && webOrderRequest.Table.Columns.Contains("ordered_date")) newOrderRequest["ordered_date"] = webOrderRequest["ordered_date"];
            if (newOrderRequest.Table.Columns.Contains("order_type_code")) newOrderRequest["order_type_code"] = "DI";
            if (newOrderRequest.Table.Columns.Contains("note") && webOrderRequest.Table.Columns.Contains("note")) newOrderRequest["note"] = webOrderRequest["note"];
            if (newOrderRequest.Table.Columns.Contains("special_instruction") && webOrderRequest.Table.Columns.Contains("special_instruction")) newOrderRequest["special_instruction"] = webOrderRequest["special_instruction"];
            if (newOrderRequest.Table.Columns.Contains("intended_use_code") && webOrderRequest.Table.Columns.Contains("intended_use_code")) newOrderRequest["intended_use_code"] = webOrderRequest["intended_use_code"];
            if (newOrderRequest.Table.Columns.Contains("intended_use_note") && webOrderRequest.Table.Columns.Contains("intended_use_note")) newOrderRequest["intended_use_note"] = webOrderRequest["intended_use_note"];
            
            // Attempt to find a cooperator that matches the web_cooperator in the web_order_request record...
            if (webOrderRequest.Table.Columns.Contains("last_name") &&
                webOrderRequest.Table.Columns.Contains("first_name") &&
                webOrderRequest.Table.Columns.Contains("organization") &&
                webOrderRequest.Table.Columns.Contains("geography_id") &&
                webOrderRequest.Table.Columns.Contains("address_line1"))
            {
                string last_name = webOrderRequest["last_name"].ToString();
                string first_name = webOrderRequest["first_name"].ToString();
                string organization = webOrderRequest["organization"].ToString();
                string geography_id = webOrderRequest["geography_id"].ToString();
                string address_line1 = webOrderRequest["address_line1"].ToString();
                string cooperator_id = FindCooperator(last_name, first_name, organization, geography_id, address_line1);
                if (!string.IsNullOrEmpty(cooperator_id))
                {
                    // Looks like a matching cooperator record exists...
                    if (newOrderRequest.Table.Columns.Contains("final_recipient_cooperator_id")) newOrderRequest["final_recipient_cooperator_id"] = cooperator_id;
                }
                else
                {
                    // Looks like no matching cooperator exists for the cooperator in the web_order_request so ask the
                    // if they would like to create a new cooperator based on the data in the web_order_request...
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The recipient in this web order is not listed in the Cooperator Table.\n\nWould you like to create a new Cooperator now?\n(Clicking Yes will create the Cooperator and add it to the new Order Request.  Clicking No will create the Order Request with Final Recipient left blank).", "Order Wizard Cooperator Missing", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "OrderWizard_ux_buttonCreateOrderRequest1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
                    if (DialogResult.Yes == ggMessageBox.ShowDialog())
                    {
                        // Create a new cooperator
                        ux_buttonCreateCooperator.PerformClick();
                        cooperator_id = FindCooperator(last_name, first_name, organization, geography_id, address_line1);
                        if (!string.IsNullOrEmpty(cooperator_id))
                        {
                            if (newOrderRequest.Table.Columns.Contains("final_recipient_cooperator_id")) newOrderRequest["final_recipient_cooperator_id"] = cooperator_id;
                        }
                    }
                }
            }

            // Create the new order_request_item records...
            foreach (DataRowView drv in ((DataTable)_webOrderRequestItemBindingSource.DataSource).DefaultView)
            {
                string distributionInventoryID = FindInventoryFromAccession(_webOrderRequestItem.Rows.Find(drv["web_order_request_item_id"])["accession_id"].ToString());
                DataTable destinationTable = (DataTable)((BindingSource)ux_datagridviewOrderRequestItem.DataSource).DataSource;
                DataRow newOrderItem = BuildOrderRequestItemRow(distributionInventoryID, destinationTable);
                _orderRequestItem.Rows.Add(newOrderItem);
            }

            // Change the status of the web order request...
            webOrderRequest["status_code"] = "ACCEPTED";

            // Put focus on the Order tab page...
            ux_tabcontrolMain.SelectedTab = OrderPage;
            
            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private string FindCooperator(string last_name, string first_name, string organization, string geography_id, string address_line1)
        {
            string cooperator_id = "";
            string findText = "";
            string searchPKeys = ":cooperatorid=";
            DataTable cooperators = new DataTable();

            // Force a refresh of the Cooperator LU table to get any brand new cooperators...
            _sharedUtils.LookupTablesUpdateTable("cooperator_lookup", false);

            // First get the cooperator_ids from the local LU table that match the lastname...
            if (!string.IsNullOrEmpty(last_name)) findText += last_name + "%";
            if (!string.IsNullOrEmpty(findText))
            {
                DataTable dt = new DataTable();

                dt = _sharedUtils.LookupTablesGetMatchingRows("cooperator_lookup", findText, 1000);
                foreach (DataRow dr in dt.Rows)
                {
                    searchPKeys += dr["value_member"].ToString() + ",";
                }
            }
            // Remove the last trailing comma...
            searchPKeys = searchPKeys.TrimEnd(',');
            // Next get the full collection of cooperator_ids that match the last name of the web_cooperator...
            DataSet ds = _sharedUtils.GetWebServiceData("get_cooperator", searchPKeys, 0, 0);
            if (ds.Tables.Contains("get_cooperator"))
            {
                cooperators = ds.Tables["get_cooperator"].Copy();
            }
            // Next iterate through the cooperator records to see if there is a perfect match for the web_cooperator info in the web_order_request...
            // based on last_name, first_name, organization, geography_id, and address_line1
            foreach (DataRow dr in cooperators.Rows)
            {
                bool match = true;
//if (cooperators.Columns.Contains("last_name") && webOrderRequest.Table.Columns.Contains("last_name") && (dr["last_name"].ToString().ToLower() != webOrderRequest["last_name"].ToString().ToLower())) match = false;
//if (cooperators.Columns.Contains("first_name") && webOrderRequest.Table.Columns.Contains("first_name") && (dr["first_name"].ToString().ToLower() != webOrderRequest["first_name"].ToString().ToLower())) match = false;
//if (cooperators.Columns.Contains("organization") && webOrderRequest.Table.Columns.Contains("organization") && (dr["organization"].ToString().ToLower() != webOrderRequest["organization"].ToString().ToLower())) match = false;
//if (cooperators.Columns.Contains("geography_id") && webOrderRequest.Table.Columns.Contains("geography_id") && (dr["geography_id"].ToString().ToLower() != webOrderRequest["geography_id"].ToString().ToLower())) match = false;
//if (cooperators.Columns.Contains("address_line1") && webOrderRequest.Table.Columns.Contains("address_line1") && (dr["address_line1"].ToString().ToLower() != webOrderRequest["address_line1"].ToString().ToLower())) match = false;
//if (match && newOrderRequest.Table.Columns.Contains("final_recipient_cooperator_id")) newOrderRequest["final_recipient_cooperator_id"] = dr["cooperator_id"].ToString();
                if (dr.Table.Columns.Contains("last_name") && (dr["last_name"].ToString().ToLower() != last_name.ToLower())) match = false;
                if (dr.Table.Columns.Contains("first_name") && (dr["first_name"].ToString().ToLower() != first_name.ToLower())) match = false;
                if (dr.Table.Columns.Contains("organization") && (dr["organization"].ToString().ToLower() != organization.ToLower())) match = false;
                if (dr.Table.Columns.Contains("geography_id") && (dr["geography_id"].ToString().ToLower() != geography_id.ToLower())) match = false;
                if (dr.Table.Columns.Contains("address_line1") && (dr["address_line1"].ToString().ToLower() != address_line1.ToLower())) match = false;
                if (match && dr.Table.Columns.Contains("cooperator_id")) cooperator_id = dr["cooperator_id"].ToString();
            }

            return cooperator_id;
        }

        private void ux_buttonCreateCooperator_Click(object sender, EventArgs e)
        {
            // If there is no active row in the web_order_request DGV - bail out now...
            if (ux_bindingNavigatorWebOrders.BindingSource.Current == null) return;

            // Get an empty cooperator table from the remote server...
            DataSet ds = _sharedUtils.GetWebServiceData("get_cooperator", ":cooperatorid=", 0, 0);
            // Get the active web_order_request record in the DGV...
            DataRow webOrderRequest = ((DataRowView)ux_bindingNavigatorWebOrders.BindingSource.Current).Row;
            if (ds.Tables.Contains("get_cooperator") && webOrderRequest != null)
            {
                string last_name = webOrderRequest["last_name"].ToString();
                string first_name = webOrderRequest["first_name"].ToString();
                string organization = webOrderRequest["organization"].ToString();
                string geography_id = webOrderRequest["geography_id"].ToString();
                string address_line1 = webOrderRequest["address_line1"].ToString();
                // Check to see if this cooperator is already in the system...
                string cooperator_id = FindCooperator(last_name, first_name, organization, geography_id, address_line1);
                // If the cooperator does not exist in the cooperator_table - create it now...
                if (string.IsNullOrEmpty(cooperator_id))
                {
                    DataRow newCooperator = ds.Tables["get_cooperator"].NewRow();

                    if (newCooperator.Table.Columns.Contains("last_name") && webOrderRequest.Table.Columns.Contains("last_name")) newCooperator["last_name"] = webOrderRequest["last_name"];
                    if (newCooperator.Table.Columns.Contains("title") && webOrderRequest.Table.Columns.Contains("title")) newCooperator["title"] = webOrderRequest["title"];
                    if (newCooperator.Table.Columns.Contains("first_name") && webOrderRequest.Table.Columns.Contains("first_name")) newCooperator["first_name"] = webOrderRequest["first_name"];
                    if (newCooperator.Table.Columns.Contains("organization") && webOrderRequest.Table.Columns.Contains("organization")) newCooperator["organization"] = webOrderRequest["organization"];
                    if (newCooperator.Table.Columns.Contains("address_line1") && webOrderRequest.Table.Columns.Contains("address_line1")) newCooperator["address_line1"] = webOrderRequest["address_line1"];
                    if (newCooperator.Table.Columns.Contains("address_line2") && webOrderRequest.Table.Columns.Contains("address_line2")) newCooperator["address_line2"] = webOrderRequest["address_line2"];
                    if (newCooperator.Table.Columns.Contains("address_line3") && webOrderRequest.Table.Columns.Contains("address_line3")) newCooperator["address_line3"] = webOrderRequest["address_line3"];
                    if (newCooperator.Table.Columns.Contains("city") && webOrderRequest.Table.Columns.Contains("city")) newCooperator["city"] = webOrderRequest["city"];
                    if (newCooperator.Table.Columns.Contains("postal_index") && webOrderRequest.Table.Columns.Contains("postal_index")) newCooperator["postal_index"] = webOrderRequest["postal_index"];
                    if (newCooperator.Table.Columns.Contains("geography_id") && webOrderRequest.Table.Columns.Contains("geography_id")) newCooperator["geography_id"] = webOrderRequest["geography_id"];
                    if (newCooperator.Table.Columns.Contains("status_code")) newCooperator["status_code"] = "ACTIVE";
                    if (newCooperator.Table.Columns.Contains("sys_lang_id")) newCooperator["sys_lang_id"] = 1;
                    if (newCooperator.Table.Columns.Contains("primary_phone") && webOrderRequest.Table.Columns.Contains("primary_phone")) newCooperator["primary_phone"] = webOrderRequest["primary_phone"];
                    if (newCooperator.Table.Columns.Contains("email") && webOrderRequest.Table.Columns.Contains("email")) newCooperator["email"] = webOrderRequest["email"];
                    // Now add the new cooperator record to the table and save the results...
                    ds.Tables["get_cooperator"].Rows.Add(newCooperator);
                    DataSet saveCooperatorResults = _sharedUtils.SaveWebServiceData(ds);
                    if (saveCooperatorResults != null &&
                        saveCooperatorResults.Tables.Contains("ExceptionTable") &&
                        saveCooperatorResults.Tables["ExceptionTable"].Rows.Count > 0)
                    {
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There were errors creating the new Cooperator record.\n\nFull error message:\n{0}", "Order Wizard Create Cooperator Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "OrderWizard_ux_buttonCreateCooperatorMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, saveCooperatorResults.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
ggMessageBox.ShowDialog();
                    }
                    else
                    {
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The new Cooperator record was successfully created.", "Order Wizard Create Cooperator Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "OrderWizard_ux_buttonCreateCooperatorMessage2";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
                    }
                }
                else
                {
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("This Cooperator is already in the system - duplicate Cooperator records are not allowed.", "Order Wizard Duplicate Cooperator Not Allowed", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "OrderWizard_ux_buttonCreateCooperatorMessage3";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
                }
            }
            // Force the new cooperator to be loaded in the Cooperator LU table...
            _sharedUtils.LookupTablesUpdateTable("cooperator_lookup", false);
        }
    }
}
