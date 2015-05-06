namespace GrinGlobal.Admin.ChildForms {
    partial class frmUser {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUser));
            this.tcUser = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.gbCooperatorInformation = new System.Windows.Forms.GroupBox();
            this.tcCooperator = new System.Windows.Forms.TabControl();
            this.tpCoopGeneral = new System.Windows.Forms.TabPage();
            this.ddlDisciplineCode = new System.Windows.Forms.ComboBox();
            this.btnSelectCooperator = new System.Windows.Forms.Button();
            this.lblCurrentCooperator = new System.Windows.Forms.Label();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.ddlLanguage = new System.Windows.Forms.ComboBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtOrganizationAbbreviation = new System.Windows.Forms.TextBox();
            this.lblOrganizationAbbreviation = new System.Windows.Forms.Label();
            this.txtInitials = new System.Windows.Forms.TextBox();
            this.lblInitials = new System.Windows.Forms.Label();
            this.lblDiscipline = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtOrganization = new System.Windows.Forms.TextBox();
            this.lblOrganization = new System.Windows.Forms.Label();
            this.txtJob = new System.Windows.Forms.TextBox();
            this.lblJob = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.tpWebLogin = new System.Windows.Forms.TabPage();
            this.lblWebLastName = new System.Windows.Forms.Label();
            this.txtWebLastName = new System.Windows.Forms.TextBox();
            this.lblWebFirstName = new System.Windows.Forms.Label();
            this.txtWebFirstName = new System.Windows.Forms.TextBox();
            this.lblWebUserName = new System.Windows.Forms.Label();
            this.txtWebUserName = new System.Windows.Forms.TextBox();
            this.btnWebCooperatorSearch = new System.Windows.Forms.Button();
            this.tpCoopContactInfo = new System.Windows.Forms.TabPage();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtFax = new System.Windows.Forms.TextBox();
            this.lblFax = new System.Windows.Forms.Label();
            this.txtSecondaryPhone = new System.Windows.Forms.TextBox();
            this.lblSecondaryPhone = new System.Windows.Forms.Label();
            this.lblPrimaryPhone = new System.Windows.Forms.Label();
            this.txtPrimaryPhone = new System.Windows.Forms.TextBox();
            this.lblPhones = new System.Windows.Forms.Label();
            this.txtPostalIndex = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.txtAddressLine3 = new System.Windows.Forms.TextBox();
            this.txtAddressLine2 = new System.Windows.Forms.TextBox();
            this.txtAddressLine1 = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.tpCoopGeographic = new System.Windows.Forms.TabPage();
            this.ddlOrganizationRegionCode = new System.Windows.Forms.ComboBox();
            this.ddlCategoryCode = new System.Windows.Forms.ComboBox();
            this.btnGeography = new System.Windows.Forms.Button();
            this.txtGeography = new System.Windows.Forms.TextBox();
            this.lblGeography = new System.Windows.Forms.Label();
            this.lblCategoryCode = new System.Windows.Forms.Label();
            this.ddlSiteCode = new System.Windows.Forms.ComboBox();
            this.lblRegionCode = new System.Windows.Forms.Label();
            this.lblSiteCode = new System.Windows.Forms.Label();
            this.tpCoopNotes = new System.Windows.Forms.TabPage();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.lblPasswordNotSet = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.tpPermissions = new System.Windows.Forms.TabPage();
            this.gbEffectivePermissions = new System.Windows.Forms.GroupBox();
            this.lblDelete = new System.Windows.Forms.Label();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.lblRead = new System.Windows.Forms.Label();
            this.lblCreate = new System.Windows.Forms.Label();
            this.lblDeleteTitle = new System.Windows.Forms.Label();
            this.lblUpdateTitle = new System.Windows.Forms.Label();
            this.lblReadTitle = new System.Windows.Forms.Label();
            this.lblCreateTitle = new System.Windows.Forms.Label();
            this.ddlTable = new System.Windows.Forms.ComboBox();
            this.ddlDataView = new System.Windows.Forms.ComboBox();
            this.lblShowEffectivePermissions = new System.Windows.Forms.Label();
            this.gbAssignedPermissions = new System.Windows.Forms.GroupBox();
            this.lblUserIsDisabled = new System.Windows.Forms.Label();
            this.btnAddPermission = new System.Windows.Forms.Button();
            this.lvPermissions = new System.Windows.Forms.ListView();
            this.colPermGroup = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colResource = new System.Windows.Forms.ColumnHeader();
            this.colCreate = new System.Windows.Forms.ColumnHeader();
            this.colRead = new System.Windows.Forms.ColumnHeader();
            this.colUpdate = new System.Windows.Forms.ColumnHeader();
            this.colDelete = new System.Windows.Forms.ColumnHeader();
            this.colRestriction = new System.Windows.Forms.ColumnHeader();
            this.cmPermissions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiPermissionAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiPermissionRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiPermissionsExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiPermissionProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.tpGroups = new System.Windows.Forms.TabPage();
            this.gbGroupMembership = new System.Windows.Forms.GroupBox();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.lvGroups = new System.Windows.Forms.ListView();
            this.colGroupName = new System.Windows.Forms.ColumnHeader();
            this.colDescription = new System.Windows.Forms.ColumnHeader();
            this.cmGroups = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiGroupAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiGroupRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiGroupsExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiGroupProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSetWebPassword = new System.Windows.Forms.Button();
            this.tcUser.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.gbCooperatorInformation.SuspendLayout();
            this.tcCooperator.SuspendLayout();
            this.tpCoopGeneral.SuspendLayout();
            this.tpWebLogin.SuspendLayout();
            this.tpCoopContactInfo.SuspendLayout();
            this.tpCoopGeographic.SuspendLayout();
            this.tpCoopNotes.SuspendLayout();
            this.tpPermissions.SuspendLayout();
            this.gbEffectivePermissions.SuspendLayout();
            this.gbAssignedPermissions.SuspendLayout();
            this.cmPermissions.SuspendLayout();
            this.tpGroups.SuspendLayout();
            this.gbGroupMembership.SuspendLayout();
            this.cmGroups.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcUser
            // 
            this.tcUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcUser.Controls.Add(this.tpGeneral);
            this.tcUser.Controls.Add(this.tpPermissions);
            this.tcUser.Controls.Add(this.tpGroups);
            this.tcUser.Location = new System.Drawing.Point(13, 13);
            this.tcUser.Name = "tcUser";
            this.tcUser.SelectedIndex = 0;
            this.tcUser.Size = new System.Drawing.Size(650, 390);
            this.tcUser.TabIndex = 2;
            this.tcUser.TabIndexChanged += new System.EventHandler(this.tabUser_TabIndexChanged);
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.gbCooperatorInformation);
            this.tpGeneral.Controls.Add(this.chkEnabled);
            this.tpGeneral.Controls.Add(this.lblPasswordNotSet);
            this.tpGeneral.Controls.Add(this.txtUserName);
            this.tpGeneral.Controls.Add(this.lblUserName);
            this.tpGeneral.Controls.Add(this.btnChangePassword);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(642, 364);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // gbCooperatorInformation
            // 
            this.gbCooperatorInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCooperatorInformation.Controls.Add(this.tcCooperator);
            this.gbCooperatorInformation.Location = new System.Drawing.Point(7, 87);
            this.gbCooperatorInformation.Name = "gbCooperatorInformation";
            this.gbCooperatorInformation.Size = new System.Drawing.Size(537, 271);
            this.gbCooperatorInformation.TabIndex = 7;
            this.gbCooperatorInformation.TabStop = false;
            this.gbCooperatorInformation.Text = "Cooperator Information";
            // 
            // tcCooperator
            // 
            this.tcCooperator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcCooperator.Controls.Add(this.tpCoopGeneral);
            this.tcCooperator.Controls.Add(this.tpWebLogin);
            this.tcCooperator.Controls.Add(this.tpCoopContactInfo);
            this.tcCooperator.Controls.Add(this.tpCoopGeographic);
            this.tcCooperator.Controls.Add(this.tpCoopNotes);
            this.tcCooperator.Location = new System.Drawing.Point(6, 19);
            this.tcCooperator.Name = "tcCooperator";
            this.tcCooperator.SelectedIndex = 0;
            this.tcCooperator.Size = new System.Drawing.Size(525, 246);
            this.tcCooperator.TabIndex = 23;
            this.tcCooperator.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tpCoopGeneral
            // 
            this.tpCoopGeneral.Controls.Add(this.ddlDisciplineCode);
            this.tpCoopGeneral.Controls.Add(this.btnSelectCooperator);
            this.tpCoopGeneral.Controls.Add(this.lblCurrentCooperator);
            this.tpCoopGeneral.Controls.Add(this.lblLanguage);
            this.tpCoopGeneral.Controls.Add(this.ddlLanguage);
            this.tpCoopGeneral.Controls.Add(this.chkIsActive);
            this.tpCoopGeneral.Controls.Add(this.txtOrganizationAbbreviation);
            this.tpCoopGeneral.Controls.Add(this.lblOrganizationAbbreviation);
            this.tpCoopGeneral.Controls.Add(this.txtInitials);
            this.tpCoopGeneral.Controls.Add(this.lblInitials);
            this.tpCoopGeneral.Controls.Add(this.lblDiscipline);
            this.tpCoopGeneral.Controls.Add(this.txtFullName);
            this.tpCoopGeneral.Controls.Add(this.lblFullName);
            this.tpCoopGeneral.Controls.Add(this.txtOrganization);
            this.tpCoopGeneral.Controls.Add(this.lblOrganization);
            this.tpCoopGeneral.Controls.Add(this.txtJob);
            this.tpCoopGeneral.Controls.Add(this.lblJob);
            this.tpCoopGeneral.Controls.Add(this.txtTitle);
            this.tpCoopGeneral.Controls.Add(this.lblTitle);
            this.tpCoopGeneral.Controls.Add(this.txtLastName);
            this.tpCoopGeneral.Controls.Add(this.lblLastName);
            this.tpCoopGeneral.Controls.Add(this.txtFirstName);
            this.tpCoopGeneral.Controls.Add(this.lblFirstName);
            this.tpCoopGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpCoopGeneral.Name = "tpCoopGeneral";
            this.tpCoopGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpCoopGeneral.Size = new System.Drawing.Size(517, 220);
            this.tpCoopGeneral.TabIndex = 0;
            this.tpCoopGeneral.Text = "General";
            this.tpCoopGeneral.UseVisualStyleBackColor = true;
            // 
            // ddlDisciplineCode
            // 
            this.ddlDisciplineCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDisciplineCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDisciplineCode.FormattingEnabled = true;
            this.ddlDisciplineCode.Location = new System.Drawing.Point(252, 130);
            this.ddlDisciplineCode.Name = "ddlDisciplineCode";
            this.ddlDisciplineCode.Size = new System.Drawing.Size(259, 21);
            this.ddlDisciplineCode.TabIndex = 46;
            this.ddlDisciplineCode.SelectedIndexChanged += new System.EventHandler(this.ddlDisciplineCode_SelectedIndexChanged);
            // 
            // btnSelectCooperator
            // 
            this.btnSelectCooperator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectCooperator.Location = new System.Drawing.Point(227, 8);
            this.btnSelectCooperator.Name = "btnSelectCooperator";
            this.btnSelectCooperator.Size = new System.Drawing.Size(68, 23);
            this.btnSelectCooperator.TabIndex = 41;
            this.btnSelectCooperator.Text = "Search...";
            this.btnSelectCooperator.UseVisualStyleBackColor = true;
            this.btnSelectCooperator.Click += new System.EventHandler(this.btnSelectCooperator_Click_1);
            // 
            // lblCurrentCooperator
            // 
            this.lblCurrentCooperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentCooperator.Location = new System.Drawing.Point(5, 13);
            this.lblCurrentCooperator.Name = "lblCurrentCooperator";
            this.lblCurrentCooperator.Size = new System.Drawing.Size(216, 13);
            this.lblCurrentCooperator.TabIndex = 42;
            this.lblCurrentCooperator.Text = "Current Cooperator is (none)";
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(4, 195);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(58, 13);
            this.lblLanguage.TabIndex = 39;
            this.lblLanguage.Text = "Language:";
            // 
            // ddlLanguage
            // 
            this.ddlLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLanguage.FormattingEnabled = true;
            this.ddlLanguage.Location = new System.Drawing.Point(72, 192);
            this.ddlLanguage.Name = "ddlLanguage";
            this.ddlLanguage.Size = new System.Drawing.Size(177, 21);
            this.ddlLanguage.TabIndex = 17;
            this.ddlLanguage.SelectedIndexChanged += new System.EventHandler(this.ddlLanguage_SelectedIndexChanged);
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Location = new System.Drawing.Point(274, 194);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(56, 17);
            this.chkIsActive.TabIndex = 18;
            this.chkIsActive.Text = "Active";
            this.chkIsActive.UseVisualStyleBackColor = true;
            this.chkIsActive.CheckedChanged += new System.EventHandler(this.chkIsActive_CheckedChanged);
            // 
            // txtOrganizationAbbreviation
            // 
            this.txtOrganizationAbbreviation.Location = new System.Drawing.Point(252, 170);
            this.txtOrganizationAbbreviation.MaxLength = 10;
            this.txtOrganizationAbbreviation.Name = "txtOrganizationAbbreviation";
            this.txtOrganizationAbbreviation.Size = new System.Drawing.Size(116, 20);
            this.txtOrganizationAbbreviation.TabIndex = 16;
            this.txtOrganizationAbbreviation.TextChanged += new System.EventHandler(this.txtOrganizationCode_TextChanged);
            // 
            // lblOrganizationAbbreviation
            // 
            this.lblOrganizationAbbreviation.AutoSize = true;
            this.lblOrganizationAbbreviation.Location = new System.Drawing.Point(249, 154);
            this.lblOrganizationAbbreviation.Name = "lblOrganizationAbbreviation";
            this.lblOrganizationAbbreviation.Size = new System.Drawing.Size(131, 13);
            this.lblOrganizationAbbreviation.TabIndex = 28;
            this.lblOrganizationAbbreviation.Text = "Organization Abbreviation:";
            // 
            // txtInitials
            // 
            this.txtInitials.Location = new System.Drawing.Point(412, 52);
            this.txtInitials.Name = "txtInitials";
            this.txtInitials.Size = new System.Drawing.Size(69, 20);
            this.txtInitials.TabIndex = 10;
            this.txtInitials.Visible = false;
            this.txtInitials.TextChanged += new System.EventHandler(this.txtInitials_TextChanged);
            // 
            // lblInitials
            // 
            this.lblInitials.AutoSize = true;
            this.lblInitials.Location = new System.Drawing.Point(409, 36);
            this.lblInitials.Name = "lblInitials";
            this.lblInitials.Size = new System.Drawing.Size(39, 13);
            this.lblInitials.TabIndex = 26;
            this.lblInitials.Text = "Initials:";
            this.lblInitials.Visible = false;
            // 
            // lblDiscipline
            // 
            this.lblDiscipline.AutoSize = true;
            this.lblDiscipline.Location = new System.Drawing.Point(249, 115);
            this.lblDiscipline.Name = "lblDiscipline";
            this.lblDiscipline.Size = new System.Drawing.Size(55, 13);
            this.lblDiscipline.TabIndex = 24;
            this.lblDiscipline.Text = "Discipline:";
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(6, 93);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.ReadOnly = true;
            this.txtFullName.Size = new System.Drawing.Size(360, 20);
            this.txtFullName.TabIndex = 12;
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(3, 77);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(113, 13);
            this.lblFullName.TabIndex = 22;
            this.lblFullName.Text = "Full Name (for display):";
            // 
            // txtOrganization
            // 
            this.txtOrganization.Location = new System.Drawing.Point(6, 170);
            this.txtOrganization.MaxLength = 100;
            this.txtOrganization.Name = "txtOrganization";
            this.txtOrganization.Size = new System.Drawing.Size(240, 20);
            this.txtOrganization.TabIndex = 15;
            this.txtOrganization.TextChanged += new System.EventHandler(this.txtOrganization_TextChanged);
            // 
            // lblOrganization
            // 
            this.lblOrganization.AutoSize = true;
            this.lblOrganization.Location = new System.Drawing.Point(5, 154);
            this.lblOrganization.Name = "lblOrganization";
            this.lblOrganization.Size = new System.Drawing.Size(69, 13);
            this.lblOrganization.TabIndex = 20;
            this.lblOrganization.Text = "Organization:";
            // 
            // txtJob
            // 
            this.txtJob.Location = new System.Drawing.Point(6, 131);
            this.txtJob.MaxLength = 100;
            this.txtJob.Name = "txtJob";
            this.txtJob.Size = new System.Drawing.Size(240, 20);
            this.txtJob.TabIndex = 13;
            this.txtJob.TextChanged += new System.EventHandler(this.txtJob_TextChanged);
            // 
            // lblJob
            // 
            this.lblJob.AutoSize = true;
            this.lblJob.Location = new System.Drawing.Point(3, 116);
            this.lblJob.Name = "lblJob";
            this.lblJob.Size = new System.Drawing.Size(27, 13);
            this.lblJob.TabIndex = 18;
            this.lblJob.Text = "Job:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(6, 54);
            this.txtTitle.MaxLength = 10;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(43, 20);
            this.txtTitle.TabIndex = 8;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(3, 38);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(30, 13);
            this.lblTitle.TabIndex = 16;
            this.lblTitle.Text = "Title:";
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(215, 52);
            this.txtLastName.MaxLength = 100;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(153, 20);
            this.txtLastName.TabIndex = 11;
            this.txtLastName.TextChanged += new System.EventHandler(this.txtLastName_TextChanged);
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(212, 36);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(61, 13);
            this.lblLastName.TabIndex = 14;
            this.lblLastName.Text = "Last Name:";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(55, 53);
            this.txtFirstName.MaxLength = 100;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(154, 20);
            this.txtFirstName.TabIndex = 9;
            this.txtFirstName.TextChanged += new System.EventHandler(this.txtFirstName_TextChanged);
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(52, 36);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(60, 13);
            this.lblFirstName.TabIndex = 12;
            this.lblFirstName.Text = "First Name:";
            // 
            // tpWebLogin
            // 
            this.tpWebLogin.Controls.Add(this.btnSetWebPassword);
            this.tpWebLogin.Controls.Add(this.lblWebLastName);
            this.tpWebLogin.Controls.Add(this.txtWebLastName);
            this.tpWebLogin.Controls.Add(this.lblWebFirstName);
            this.tpWebLogin.Controls.Add(this.txtWebFirstName);
            this.tpWebLogin.Controls.Add(this.lblWebUserName);
            this.tpWebLogin.Controls.Add(this.txtWebUserName);
            this.tpWebLogin.Controls.Add(this.btnWebCooperatorSearch);
            this.tpWebLogin.Location = new System.Drawing.Point(4, 22);
            this.tpWebLogin.Name = "tpWebLogin";
            this.tpWebLogin.Padding = new System.Windows.Forms.Padding(3);
            this.tpWebLogin.Size = new System.Drawing.Size(517, 220);
            this.tpWebLogin.TabIndex = 4;
            this.tpWebLogin.Text = "Web Login";
            this.tpWebLogin.UseVisualStyleBackColor = true;
            // 
            // lblWebLastName
            // 
            this.lblWebLastName.AutoSize = true;
            this.lblWebLastName.Location = new System.Drawing.Point(17, 84);
            this.lblWebLastName.Name = "lblWebLastName";
            this.lblWebLastName.Size = new System.Drawing.Size(61, 13);
            this.lblWebLastName.TabIndex = 50;
            this.lblWebLastName.Text = "Last Name:";
            // 
            // txtWebLastName
            // 
            this.txtWebLastName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWebLastName.Location = new System.Drawing.Point(86, 81);
            this.txtWebLastName.Name = "txtWebLastName";
            this.txtWebLastName.ReadOnly = true;
            this.txtWebLastName.Size = new System.Drawing.Size(257, 20);
            this.txtWebLastName.TabIndex = 49;
            // 
            // lblWebFirstName
            // 
            this.lblWebFirstName.AutoSize = true;
            this.lblWebFirstName.Location = new System.Drawing.Point(17, 58);
            this.lblWebFirstName.Name = "lblWebFirstName";
            this.lblWebFirstName.Size = new System.Drawing.Size(60, 13);
            this.lblWebFirstName.TabIndex = 48;
            this.lblWebFirstName.Text = "First Name:";
            // 
            // txtWebFirstName
            // 
            this.txtWebFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWebFirstName.Location = new System.Drawing.Point(86, 55);
            this.txtWebFirstName.Name = "txtWebFirstName";
            this.txtWebFirstName.ReadOnly = true;
            this.txtWebFirstName.Size = new System.Drawing.Size(257, 20);
            this.txtWebFirstName.TabIndex = 47;
            // 
            // lblWebUserName
            // 
            this.lblWebUserName.AutoSize = true;
            this.lblWebUserName.Location = new System.Drawing.Point(17, 32);
            this.lblWebUserName.Name = "lblWebUserName";
            this.lblWebUserName.Size = new System.Drawing.Size(63, 13);
            this.lblWebUserName.TabIndex = 46;
            this.lblWebUserName.Text = "User Name:";
            // 
            // txtWebUserName
            // 
            this.txtWebUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWebUserName.Location = new System.Drawing.Point(86, 29);
            this.txtWebUserName.Name = "txtWebUserName";
            this.txtWebUserName.ReadOnly = true;
            this.txtWebUserName.Size = new System.Drawing.Size(257, 20);
            this.txtWebUserName.TabIndex = 45;
            // 
            // btnWebCooperatorSearch
            // 
            this.btnWebCooperatorSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWebCooperatorSearch.Location = new System.Drawing.Point(275, 107);
            this.btnWebCooperatorSearch.Name = "btnWebCooperatorSearch";
            this.btnWebCooperatorSearch.Size = new System.Drawing.Size(68, 23);
            this.btnWebCooperatorSearch.TabIndex = 43;
            this.btnWebCooperatorSearch.Text = "Search...";
            this.btnWebCooperatorSearch.UseVisualStyleBackColor = true;
            this.btnWebCooperatorSearch.Click += new System.EventHandler(this.btnWebCooperatorSearch_Click);
            // 
            // tpCoopContactInfo
            // 
            this.tpCoopContactInfo.Controls.Add(this.txtEmail);
            this.tpCoopContactInfo.Controls.Add(this.lblEmail);
            this.tpCoopContactInfo.Controls.Add(this.txtFax);
            this.tpCoopContactInfo.Controls.Add(this.lblFax);
            this.tpCoopContactInfo.Controls.Add(this.txtSecondaryPhone);
            this.tpCoopContactInfo.Controls.Add(this.lblSecondaryPhone);
            this.tpCoopContactInfo.Controls.Add(this.lblPrimaryPhone);
            this.tpCoopContactInfo.Controls.Add(this.txtPrimaryPhone);
            this.tpCoopContactInfo.Controls.Add(this.lblPhones);
            this.tpCoopContactInfo.Controls.Add(this.txtPostalIndex);
            this.tpCoopContactInfo.Controls.Add(this.txtCity);
            this.tpCoopContactInfo.Controls.Add(this.txtAddressLine3);
            this.tpCoopContactInfo.Controls.Add(this.txtAddressLine2);
            this.tpCoopContactInfo.Controls.Add(this.txtAddressLine1);
            this.tpCoopContactInfo.Controls.Add(this.lblAddress);
            this.tpCoopContactInfo.Location = new System.Drawing.Point(4, 22);
            this.tpCoopContactInfo.Name = "tpCoopContactInfo";
            this.tpCoopContactInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpCoopContactInfo.Size = new System.Drawing.Size(517, 220);
            this.tpCoopContactInfo.TabIndex = 1;
            this.tpCoopContactInfo.Text = "Contact Info";
            this.tpCoopContactInfo.UseVisualStyleBackColor = true;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(10, 166);
            this.txtEmail.MaxLength = 100;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(232, 20);
            this.txtEmail.TabIndex = 24;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(7, 149);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 37;
            this.lblEmail.Text = "Email:";
            // 
            // txtFax
            // 
            this.txtFax.Location = new System.Drawing.Point(327, 81);
            this.txtFax.MaxLength = 30;
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(116, 20);
            this.txtFax.TabIndex = 27;
            this.txtFax.TextChanged += new System.EventHandler(this.txtFax_TextChanged);
            // 
            // lblFax
            // 
            this.lblFax.Location = new System.Drawing.Point(251, 84);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(70, 13);
            this.lblFax.TabIndex = 35;
            this.lblFax.Text = "Fax:";
            this.lblFax.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSecondaryPhone
            // 
            this.txtSecondaryPhone.Location = new System.Drawing.Point(327, 55);
            this.txtSecondaryPhone.MaxLength = 30;
            this.txtSecondaryPhone.Name = "txtSecondaryPhone";
            this.txtSecondaryPhone.Size = new System.Drawing.Size(116, 20);
            this.txtSecondaryPhone.TabIndex = 26;
            this.txtSecondaryPhone.TextChanged += new System.EventHandler(this.txtSecondaryPhone_TextChanged);
            // 
            // lblSecondaryPhone
            // 
            this.lblSecondaryPhone.Location = new System.Drawing.Point(248, 58);
            this.lblSecondaryPhone.Name = "lblSecondaryPhone";
            this.lblSecondaryPhone.Size = new System.Drawing.Size(73, 13);
            this.lblSecondaryPhone.TabIndex = 33;
            this.lblSecondaryPhone.Text = "Secondary:";
            this.lblSecondaryPhone.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblPrimaryPhone
            // 
            this.lblPrimaryPhone.Location = new System.Drawing.Point(248, 32);
            this.lblPrimaryPhone.Name = "lblPrimaryPhone";
            this.lblPrimaryPhone.Size = new System.Drawing.Size(73, 13);
            this.lblPrimaryPhone.TabIndex = 32;
            this.lblPrimaryPhone.Text = "Primary:";
            this.lblPrimaryPhone.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtPrimaryPhone
            // 
            this.txtPrimaryPhone.Location = new System.Drawing.Point(327, 29);
            this.txtPrimaryPhone.MaxLength = 30;
            this.txtPrimaryPhone.Name = "txtPrimaryPhone";
            this.txtPrimaryPhone.Size = new System.Drawing.Size(116, 20);
            this.txtPrimaryPhone.TabIndex = 25;
            this.txtPrimaryPhone.TextChanged += new System.EventHandler(this.textBox10_TextChanged);
            // 
            // lblPhones
            // 
            this.lblPhones.AutoSize = true;
            this.lblPhones.Location = new System.Drawing.Point(324, 12);
            this.lblPhones.Name = "lblPhones";
            this.lblPhones.Size = new System.Drawing.Size(46, 13);
            this.lblPhones.TabIndex = 30;
            this.lblPhones.Text = "Phones:";
            this.lblPhones.Click += new System.EventHandler(this.label13_Click);
            // 
            // txtPostalIndex
            // 
            this.txtPostalIndex.Location = new System.Drawing.Point(153, 107);
            this.txtPostalIndex.MaxLength = 100;
            this.txtPostalIndex.Name = "txtPostalIndex";
            this.txtPostalIndex.Size = new System.Drawing.Size(89, 20);
            this.txtPostalIndex.TabIndex = 23;
            this.txtPostalIndex.TextChanged += new System.EventHandler(this.textBox8_TextChanged);
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(6, 107);
            this.txtCity.MaxLength = 100;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(141, 20);
            this.txtCity.TabIndex = 22;
            this.txtCity.TextChanged += new System.EventHandler(this.textBox7_TextChanged);
            // 
            // txtAddressLine3
            // 
            this.txtAddressLine3.Location = new System.Drawing.Point(6, 81);
            this.txtAddressLine3.MaxLength = 100;
            this.txtAddressLine3.Name = "txtAddressLine3";
            this.txtAddressLine3.Size = new System.Drawing.Size(236, 20);
            this.txtAddressLine3.TabIndex = 21;
            this.txtAddressLine3.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // txtAddressLine2
            // 
            this.txtAddressLine2.Location = new System.Drawing.Point(6, 55);
            this.txtAddressLine2.MaxLength = 100;
            this.txtAddressLine2.Name = "txtAddressLine2";
            this.txtAddressLine2.Size = new System.Drawing.Size(236, 20);
            this.txtAddressLine2.TabIndex = 20;
            this.txtAddressLine2.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // txtAddressLine1
            // 
            this.txtAddressLine1.Location = new System.Drawing.Point(6, 29);
            this.txtAddressLine1.MaxLength = 100;
            this.txtAddressLine1.Name = "txtAddressLine1";
            this.txtAddressLine1.Size = new System.Drawing.Size(236, 20);
            this.txtAddressLine1.TabIndex = 19;
            this.txtAddressLine1.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(3, 12);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(48, 13);
            this.lblAddress.TabIndex = 23;
            this.lblAddress.Text = "Address:";
            this.lblAddress.Click += new System.EventHandler(this.label15_Click);
            // 
            // tpCoopGeographic
            // 
            this.tpCoopGeographic.Controls.Add(this.ddlOrganizationRegionCode);
            this.tpCoopGeographic.Controls.Add(this.ddlCategoryCode);
            this.tpCoopGeographic.Controls.Add(this.btnGeography);
            this.tpCoopGeographic.Controls.Add(this.txtGeography);
            this.tpCoopGeographic.Controls.Add(this.lblGeography);
            this.tpCoopGeographic.Controls.Add(this.lblCategoryCode);
            this.tpCoopGeographic.Controls.Add(this.ddlSiteCode);
            this.tpCoopGeographic.Controls.Add(this.lblRegionCode);
            this.tpCoopGeographic.Controls.Add(this.lblSiteCode);
            this.tpCoopGeographic.Location = new System.Drawing.Point(4, 22);
            this.tpCoopGeographic.Name = "tpCoopGeographic";
            this.tpCoopGeographic.Padding = new System.Windows.Forms.Padding(3);
            this.tpCoopGeographic.Size = new System.Drawing.Size(517, 220);
            this.tpCoopGeographic.TabIndex = 3;
            this.tpCoopGeographic.Text = "Geographic";
            this.tpCoopGeographic.UseVisualStyleBackColor = true;
            // 
            // ddlOrganizationRegionCode
            // 
            this.ddlOrganizationRegionCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlOrganizationRegionCode.FormattingEnabled = true;
            this.ddlOrganizationRegionCode.Location = new System.Drawing.Point(9, 81);
            this.ddlOrganizationRegionCode.Name = "ddlOrganizationRegionCode";
            this.ddlOrganizationRegionCode.Size = new System.Drawing.Size(199, 21);
            this.ddlOrganizationRegionCode.TabIndex = 46;
            this.ddlOrganizationRegionCode.SelectedIndexChanged += new System.EventHandler(this.ddlOrganizationRegionCode_SelectedIndexChanged);
            // 
            // ddlCategoryCode
            // 
            this.ddlCategoryCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlCategoryCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCategoryCode.FormattingEnabled = true;
            this.ddlCategoryCode.Location = new System.Drawing.Point(214, 81);
            this.ddlCategoryCode.Name = "ddlCategoryCode";
            this.ddlCategoryCode.Size = new System.Drawing.Size(297, 21);
            this.ddlCategoryCode.TabIndex = 45;
            this.ddlCategoryCode.SelectedIndexChanged += new System.EventHandler(this.ddlCategoryCode_SelectedIndexChanged);
            // 
            // btnGeography
            // 
            this.btnGeography.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGeography.Location = new System.Drawing.Point(321, 139);
            this.btnGeography.Name = "btnGeography";
            this.btnGeography.Size = new System.Drawing.Size(75, 23);
            this.btnGeography.TabIndex = 44;
            this.btnGeography.Text = "&Search...";
            this.btnGeography.UseVisualStyleBackColor = true;
            this.btnGeography.Click += new System.EventHandler(this.btnGeography_Click);
            // 
            // txtGeography
            // 
            this.txtGeography.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGeography.Location = new System.Drawing.Point(9, 139);
            this.txtGeography.Name = "txtGeography";
            this.txtGeography.ReadOnly = true;
            this.txtGeography.Size = new System.Drawing.Size(306, 20);
            this.txtGeography.TabIndex = 43;
            this.txtGeography.TextChanged += new System.EventHandler(this.txtGeography_TextChanged);
            // 
            // lblGeography
            // 
            this.lblGeography.AutoSize = true;
            this.lblGeography.Location = new System.Drawing.Point(6, 122);
            this.lblGeography.Name = "lblGeography";
            this.lblGeography.Size = new System.Drawing.Size(62, 13);
            this.lblGeography.TabIndex = 42;
            this.lblGeography.Text = "Geography:";
            // 
            // lblCategoryCode
            // 
            this.lblCategoryCode.AutoSize = true;
            this.lblCategoryCode.Location = new System.Drawing.Point(211, 65);
            this.lblCategoryCode.Name = "lblCategoryCode";
            this.lblCategoryCode.Size = new System.Drawing.Size(80, 13);
            this.lblCategoryCode.TabIndex = 39;
            this.lblCategoryCode.Text = "Category Code:";
            // 
            // ddlSiteCode
            // 
            this.ddlSiteCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSiteCode.FormattingEnabled = true;
            this.ddlSiteCode.Location = new System.Drawing.Point(6, 26);
            this.ddlSiteCode.Name = "ddlSiteCode";
            this.ddlSiteCode.Size = new System.Drawing.Size(352, 21);
            this.ddlSiteCode.TabIndex = 28;
            this.ddlSiteCode.SelectedIndexChanged += new System.EventHandler(this.ddlSiteCode_SelectedIndexChanged);
            // 
            // lblRegionCode
            // 
            this.lblRegionCode.AutoSize = true;
            this.lblRegionCode.Location = new System.Drawing.Point(6, 66);
            this.lblRegionCode.Name = "lblRegionCode";
            this.lblRegionCode.Size = new System.Drawing.Size(72, 13);
            this.lblRegionCode.TabIndex = 36;
            this.lblRegionCode.Text = "Region Code:";
            // 
            // lblSiteCode
            // 
            this.lblSiteCode.AutoSize = true;
            this.lblSiteCode.Location = new System.Drawing.Point(5, 12);
            this.lblSiteCode.Name = "lblSiteCode";
            this.lblSiteCode.Size = new System.Drawing.Size(56, 13);
            this.lblSiteCode.TabIndex = 34;
            this.lblSiteCode.Text = "Site Code:";
            // 
            // tpCoopNotes
            // 
            this.tpCoopNotes.Controls.Add(this.txtNote);
            this.tpCoopNotes.Location = new System.Drawing.Point(4, 22);
            this.tpCoopNotes.Name = "tpCoopNotes";
            this.tpCoopNotes.Padding = new System.Windows.Forms.Padding(3);
            this.tpCoopNotes.Size = new System.Drawing.Size(517, 220);
            this.tpCoopNotes.TabIndex = 2;
            this.tpCoopNotes.Text = "Notes";
            this.tpCoopNotes.UseVisualStyleBackColor = true;
            // 
            // txtNote
            // 
            this.txtNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNote.Location = new System.Drawing.Point(7, 7);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(504, 207);
            this.txtNote.TabIndex = 33;
            this.txtNote.TextChanged += new System.EventHandler(this.txtNote_TextChanged);
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Checked = true;
            this.chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnabled.Location = new System.Drawing.Point(13, 64);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 6;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // lblPasswordNotSet
            // 
            this.lblPasswordNotSet.AutoSize = true;
            this.lblPasswordNotSet.ForeColor = System.Drawing.Color.Red;
            this.lblPasswordNotSet.Location = new System.Drawing.Point(314, 44);
            this.lblPasswordNotSet.Name = "lblPasswordNotSet";
            this.lblPasswordNotSet.Size = new System.Drawing.Size(177, 13);
            this.lblPasswordNotSet.TabIndex = 5;
            this.lblPasswordNotSet.Text = "Password must be set before saving";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(13, 39);
            this.txtUserName.MaxLength = 50;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(185, 20);
            this.txtUserName.TabIndex = 4;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(10, 23);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(63, 13);
            this.lblUserName.TabIndex = 3;
            this.lblUserName.Text = "User Name:";
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.Location = new System.Drawing.Point(204, 39);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(104, 23);
            this.btnChangePassword.TabIndex = 5;
            this.btnChangePassword.Text = "Set Password...";
            this.btnChangePassword.UseVisualStyleBackColor = true;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // tpPermissions
            // 
            this.tpPermissions.Controls.Add(this.gbEffectivePermissions);
            this.tpPermissions.Controls.Add(this.gbAssignedPermissions);
            this.tpPermissions.Location = new System.Drawing.Point(4, 22);
            this.tpPermissions.Name = "tpPermissions";
            this.tpPermissions.Padding = new System.Windows.Forms.Padding(3);
            this.tpPermissions.Size = new System.Drawing.Size(642, 364);
            this.tpPermissions.TabIndex = 1;
            this.tpPermissions.Text = "Permissions";
            this.tpPermissions.UseVisualStyleBackColor = true;
            // 
            // gbEffectivePermissions
            // 
            this.gbEffectivePermissions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEffectivePermissions.Controls.Add(this.lblDelete);
            this.gbEffectivePermissions.Controls.Add(this.lblUpdate);
            this.gbEffectivePermissions.Controls.Add(this.lblRead);
            this.gbEffectivePermissions.Controls.Add(this.lblCreate);
            this.gbEffectivePermissions.Controls.Add(this.lblDeleteTitle);
            this.gbEffectivePermissions.Controls.Add(this.lblUpdateTitle);
            this.gbEffectivePermissions.Controls.Add(this.lblReadTitle);
            this.gbEffectivePermissions.Controls.Add(this.lblCreateTitle);
            this.gbEffectivePermissions.Controls.Add(this.ddlTable);
            this.gbEffectivePermissions.Controls.Add(this.ddlDataView);
            this.gbEffectivePermissions.Controls.Add(this.lblShowEffectivePermissions);
            this.gbEffectivePermissions.Location = new System.Drawing.Point(3, 268);
            this.gbEffectivePermissions.Name = "gbEffectivePermissions";
            this.gbEffectivePermissions.Size = new System.Drawing.Size(633, 90);
            this.gbEffectivePermissions.TabIndex = 2;
            this.gbEffectivePermissions.TabStop = false;
            this.gbEffectivePermissions.Text = "Effective Permissions";
            // 
            // lblDelete
            // 
            this.lblDelete.AutoSize = true;
            this.lblDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelete.Location = new System.Drawing.Point(294, 69);
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(11, 13);
            this.lblDelete.TabIndex = 10;
            this.lblDelete.Text = "-";
            // 
            // lblUpdate
            // 
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdate.Location = new System.Drawing.Point(195, 69);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(11, 13);
            this.lblUpdate.TabIndex = 9;
            this.lblUpdate.Text = "-";
            // 
            // lblRead
            // 
            this.lblRead.AutoSize = true;
            this.lblRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRead.Location = new System.Drawing.Point(105, 69);
            this.lblRead.Name = "lblRead";
            this.lblRead.Size = new System.Drawing.Size(11, 13);
            this.lblRead.TabIndex = 8;
            this.lblRead.Text = "-";
            // 
            // lblCreate
            // 
            this.lblCreate.AutoSize = true;
            this.lblCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreate.Location = new System.Drawing.Point(10, 69);
            this.lblCreate.Name = "lblCreate";
            this.lblCreate.Size = new System.Drawing.Size(11, 13);
            this.lblCreate.TabIndex = 7;
            this.lblCreate.Text = "-";
            // 
            // lblDeleteTitle
            // 
            this.lblDeleteTitle.AutoSize = true;
            this.lblDeleteTitle.Location = new System.Drawing.Point(294, 52);
            this.lblDeleteTitle.Name = "lblDeleteTitle";
            this.lblDeleteTitle.Size = new System.Drawing.Size(41, 13);
            this.lblDeleteTitle.TabIndex = 6;
            this.lblDeleteTitle.Text = "Delete:";
            // 
            // lblUpdateTitle
            // 
            this.lblUpdateTitle.AutoSize = true;
            this.lblUpdateTitle.Location = new System.Drawing.Point(195, 52);
            this.lblUpdateTitle.Name = "lblUpdateTitle";
            this.lblUpdateTitle.Size = new System.Drawing.Size(45, 13);
            this.lblUpdateTitle.TabIndex = 5;
            this.lblUpdateTitle.Text = "Update:";
            // 
            // lblReadTitle
            // 
            this.lblReadTitle.AutoSize = true;
            this.lblReadTitle.Location = new System.Drawing.Point(105, 52);
            this.lblReadTitle.Name = "lblReadTitle";
            this.lblReadTitle.Size = new System.Drawing.Size(36, 13);
            this.lblReadTitle.TabIndex = 4;
            this.lblReadTitle.Text = "Read:";
            // 
            // lblCreateTitle
            // 
            this.lblCreateTitle.AutoSize = true;
            this.lblCreateTitle.Location = new System.Drawing.Point(10, 52);
            this.lblCreateTitle.Name = "lblCreateTitle";
            this.lblCreateTitle.Size = new System.Drawing.Size(41, 13);
            this.lblCreateTitle.TabIndex = 3;
            this.lblCreateTitle.Text = "Create:";
            // 
            // ddlTable
            // 
            this.ddlTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTable.FormattingEnabled = true;
            this.ddlTable.Location = new System.Drawing.Point(408, 17);
            this.ddlTable.Name = "ddlTable";
            this.ddlTable.Size = new System.Drawing.Size(219, 21);
            this.ddlTable.TabIndex = 24;
            this.ddlTable.SelectedIndexChanged += new System.EventHandler(this.ddlTable_SelectedIndexChanged);
            // 
            // ddlDataView
            // 
            this.ddlDataView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDataView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataView.FormattingEnabled = true;
            this.ddlDataView.Location = new System.Drawing.Point(166, 17);
            this.ddlDataView.Name = "ddlDataView";
            this.ddlDataView.Size = new System.Drawing.Size(236, 21);
            this.ddlDataView.TabIndex = 23;
            this.ddlDataView.SelectedIndexChanged += new System.EventHandler(this.ddlDataView_SelectedIndexChanged);
            // 
            // lblShowEffectivePermissions
            // 
            this.lblShowEffectivePermissions.AutoSize = true;
            this.lblShowEffectivePermissions.Location = new System.Drawing.Point(7, 20);
            this.lblShowEffectivePermissions.Name = "lblShowEffectivePermissions";
            this.lblShowEffectivePermissions.Size = new System.Drawing.Size(153, 13);
            this.lblShowEffectivePermissions.TabIndex = 0;
            this.lblShowEffectivePermissions.Text = "Show effective permissions for:";
            // 
            // gbAssignedPermissions
            // 
            this.gbAssignedPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAssignedPermissions.Controls.Add(this.lblUserIsDisabled);
            this.gbAssignedPermissions.Controls.Add(this.btnAddPermission);
            this.gbAssignedPermissions.Controls.Add(this.lvPermissions);
            this.gbAssignedPermissions.Location = new System.Drawing.Point(3, 6);
            this.gbAssignedPermissions.Name = "gbAssignedPermissions";
            this.gbAssignedPermissions.Size = new System.Drawing.Size(633, 256);
            this.gbAssignedPermissions.TabIndex = 1;
            this.gbAssignedPermissions.TabStop = false;
            this.gbAssignedPermissions.Text = "Assigned Permissions";
            // 
            // lblUserIsDisabled
            // 
            this.lblUserIsDisabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUserIsDisabled.AutoSize = true;
            this.lblUserIsDisabled.ForeColor = System.Drawing.Color.Red;
            this.lblUserIsDisabled.Location = new System.Drawing.Point(6, 232);
            this.lblUserIsDisabled.Name = "lblUserIsDisabled";
            this.lblUserIsDisabled.Size = new System.Drawing.Size(321, 13);
            this.lblUserIsDisabled.TabIndex = 25;
            this.lblUserIsDisabled.Text = "User is disabled so all Effective Permissions will display as DENIED";
            this.lblUserIsDisabled.Visible = false;
            this.lblUserIsDisabled.Click += new System.EventHandler(this.lblUserIsDisabled_Click);
            // 
            // btnAddPermission
            // 
            this.btnAddPermission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPermission.Location = new System.Drawing.Point(550, 227);
            this.btnAddPermission.Name = "btnAddPermission";
            this.btnAddPermission.Size = new System.Drawing.Size(77, 23);
            this.btnAddPermission.TabIndex = 22;
            this.btnAddPermission.Text = "Add...";
            this.btnAddPermission.UseVisualStyleBackColor = true;
            this.btnAddPermission.Click += new System.EventHandler(this.btnAddPermission_Click_1);
            // 
            // lvPermissions
            // 
            this.lvPermissions.AllowDrop = true;
            this.lvPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPermissions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPermGroup,
            this.colName,
            this.colResource,
            this.colCreate,
            this.colRead,
            this.colUpdate,
            this.colDelete,
            this.colRestriction});
            this.lvPermissions.ContextMenuStrip = this.cmPermissions;
            this.lvPermissions.FullRowSelect = true;
            this.lvPermissions.HideSelection = false;
            this.lvPermissions.Location = new System.Drawing.Point(7, 20);
            this.lvPermissions.Name = "lvPermissions";
            this.lvPermissions.Size = new System.Drawing.Size(620, 202);
            this.lvPermissions.TabIndex = 21;
            this.lvPermissions.UseCompatibleStateImageBehavior = false;
            this.lvPermissions.View = System.Windows.Forms.View.Details;
            this.lvPermissions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvPermissions_MouseDoubleClick);
            this.lvPermissions.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvPermissions_DragDrop);
            this.lvPermissions.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvPermissions_DragEnter);
            this.lvPermissions.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvPermissions_KeyPress);
            this.lvPermissions.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvPermissions_KeyUp);
            this.lvPermissions.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvPermissions_ItemDrag);
            // 
            // colPermGroup
            // 
            this.colPermGroup.Text = "Group";
            this.colPermGroup.Width = 80;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 90;
            // 
            // colResource
            // 
            this.colResource.Text = "Resource";
            this.colResource.Width = 130;
            // 
            // colCreate
            // 
            this.colCreate.Text = "Create";
            // 
            // colRead
            // 
            this.colRead.Text = "Read";
            // 
            // colUpdate
            // 
            this.colUpdate.Text = "Update";
            // 
            // colDelete
            // 
            this.colDelete.Text = "Delete";
            // 
            // colRestriction
            // 
            this.colRestriction.Text = "Restricted To";
            this.colRestriction.Width = 200;
            // 
            // cmPermissions
            // 
            this.cmPermissions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiPermissionAdd,
            this.cmiPermissionRemove,
            this.toolStripSeparator2,
            this.cmiPermissionsExportList,
            this.cmiPermissionProperties});
            this.cmPermissions.Name = "ctxMenuPermissions";
            this.cmPermissions.Size = new System.Drawing.Size(142, 98);
            this.cmPermissions.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuPermissions_Opening);
            // 
            // cmiPermissionAdd
            // 
            this.cmiPermissionAdd.Name = "cmiPermissionAdd";
            this.cmiPermissionAdd.Size = new System.Drawing.Size(141, 22);
            this.cmiPermissionAdd.Text = "&Add...";
            this.cmiPermissionAdd.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // cmiPermissionRemove
            // 
            this.cmiPermissionRemove.Name = "cmiPermissionRemove";
            this.cmiPermissionRemove.Size = new System.Drawing.Size(141, 22);
            this.cmiPermissionRemove.Text = "&Remove";
            this.cmiPermissionRemove.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(138, 6);
            // 
            // cmiPermissionsExportList
            // 
            this.cmiPermissionsExportList.Name = "cmiPermissionsExportList";
            this.cmiPermissionsExportList.Size = new System.Drawing.Size(141, 22);
            this.cmiPermissionsExportList.Text = "E&xport List";
            this.cmiPermissionsExportList.Click += new System.EventHandler(this.cmiPermissionsExportList_Click);
            // 
            // cmiPermissionProperties
            // 
            this.cmiPermissionProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiPermissionProperties.Name = "cmiPermissionProperties";
            this.cmiPermissionProperties.Size = new System.Drawing.Size(141, 22);
            this.cmiPermissionProperties.Text = "&Properties...";
            this.cmiPermissionProperties.Click += new System.EventHandler(this.editPermissionToolStripMenuItem_Click_1);
            // 
            // tpGroups
            // 
            this.tpGroups.Controls.Add(this.gbGroupMembership);
            this.tpGroups.Location = new System.Drawing.Point(4, 22);
            this.tpGroups.Name = "tpGroups";
            this.tpGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tpGroups.Size = new System.Drawing.Size(642, 364);
            this.tpGroups.TabIndex = 2;
            this.tpGroups.Text = "Groups";
            this.tpGroups.UseVisualStyleBackColor = true;
            // 
            // gbGroupMembership
            // 
            this.gbGroupMembership.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGroupMembership.Controls.Add(this.btnAddGroup);
            this.gbGroupMembership.Controls.Add(this.lvGroups);
            this.gbGroupMembership.Location = new System.Drawing.Point(6, 6);
            this.gbGroupMembership.Name = "gbGroupMembership";
            this.gbGroupMembership.Size = new System.Drawing.Size(630, 352);
            this.gbGroupMembership.TabIndex = 4;
            this.gbGroupMembership.TabStop = false;
            this.gbGroupMembership.Text = "Group Membership";
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddGroup.Location = new System.Drawing.Point(549, 323);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(75, 23);
            this.btnAddGroup.TabIndex = 4;
            this.btnAddGroup.Text = "Add...";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // lvGroups
            // 
            this.lvGroups.AllowDrop = true;
            this.lvGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colGroupName,
            this.colDescription});
            this.lvGroups.ContextMenuStrip = this.cmGroups;
            this.lvGroups.FullRowSelect = true;
            this.lvGroups.HideSelection = false;
            this.lvGroups.Location = new System.Drawing.Point(6, 19);
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.Size = new System.Drawing.Size(618, 298);
            this.lvGroups.TabIndex = 3;
            this.lvGroups.UseCompatibleStateImageBehavior = false;
            this.lvGroups.View = System.Windows.Forms.View.Details;
            this.lvGroups.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvGroups_MouseDoubleClick);
            this.lvGroups.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvGroups_DragDrop);
            this.lvGroups.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvGroups_DragEnter);
            this.lvGroups.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvGroups_KeyUp);
            // 
            // colGroupName
            // 
            this.colGroupName.Text = "Name";
            this.colGroupName.Width = 200;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 300;
            // 
            // cmGroups
            // 
            this.cmGroups.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiGroupAdd,
            this.cmiGroupRemove,
            this.toolStripSeparator1,
            this.cmiGroupsExportList,
            this.cmiGroupProperties});
            this.cmGroups.Name = "ctxMenuPermissions";
            this.cmGroups.Size = new System.Drawing.Size(142, 98);
            this.cmGroups.Opening += new System.ComponentModel.CancelEventHandler(this.cmGroups_Opening);
            // 
            // cmiGroupAdd
            // 
            this.cmiGroupAdd.Name = "cmiGroupAdd";
            this.cmiGroupAdd.Size = new System.Drawing.Size(141, 22);
            this.cmiGroupAdd.Text = "&Add...";
            this.cmiGroupAdd.Click += new System.EventHandler(this.cmiGroupAdd_Click);
            // 
            // cmiGroupRemove
            // 
            this.cmiGroupRemove.Name = "cmiGroupRemove";
            this.cmiGroupRemove.Size = new System.Drawing.Size(141, 22);
            this.cmiGroupRemove.Text = "&Remove";
            this.cmiGroupRemove.Click += new System.EventHandler(this.cmiGroupRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(138, 6);
            // 
            // cmiGroupsExportList
            // 
            this.cmiGroupsExportList.Name = "cmiGroupsExportList";
            this.cmiGroupsExportList.Size = new System.Drawing.Size(141, 22);
            this.cmiGroupsExportList.Text = "E&xport List";
            this.cmiGroupsExportList.Click += new System.EventHandler(this.cmiGroupsExportList_Click);
            // 
            // cmiGroupProperties
            // 
            this.cmiGroupProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiGroupProperties.Name = "cmiGroupProperties";
            this.cmiGroupProperties.Size = new System.Drawing.Size(141, 22);
            this.cmiGroupProperties.Text = "&Properties...";
            this.cmiGroupProperties.Click += new System.EventHandler(this.cmiGroupProperties_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(504, 409);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 50;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(585, 409);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // btnSetWebPassword
            // 
            this.btnSetWebPassword.Location = new System.Drawing.Point(86, 152);
            this.btnSetWebPassword.Name = "btnSetWebPassword";
            this.btnSetWebPassword.Size = new System.Drawing.Size(139, 23);
            this.btnSetWebPassword.TabIndex = 51;
            this.btnSetWebPassword.Text = "Set Web Password...";
            this.btnSetWebPassword.UseVisualStyleBackColor = true;
            this.btnSetWebPassword.Click += new System.EventHandler(this.btnSetWebPassword_Click);
            // 
            // frmUser
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(675, 444);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tcUser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUser";
            this.Text = "New User";
            this.Load += new System.EventHandler(this.frmUser_Load);
            this.tcUser.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.gbCooperatorInformation.ResumeLayout(false);
            this.tcCooperator.ResumeLayout(false);
            this.tpCoopGeneral.ResumeLayout(false);
            this.tpCoopGeneral.PerformLayout();
            this.tpWebLogin.ResumeLayout(false);
            this.tpWebLogin.PerformLayout();
            this.tpCoopContactInfo.ResumeLayout(false);
            this.tpCoopContactInfo.PerformLayout();
            this.tpCoopGeographic.ResumeLayout(false);
            this.tpCoopGeographic.PerformLayout();
            this.tpCoopNotes.ResumeLayout(false);
            this.tpCoopNotes.PerformLayout();
            this.tpPermissions.ResumeLayout(false);
            this.gbEffectivePermissions.ResumeLayout(false);
            this.gbEffectivePermissions.PerformLayout();
            this.gbAssignedPermissions.ResumeLayout(false);
            this.gbAssignedPermissions.PerformLayout();
            this.cmPermissions.ResumeLayout(false);
            this.tpGroups.ResumeLayout(false);
            this.gbGroupMembership.ResumeLayout(false);
            this.cmGroups.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcUser;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.TabPage tpPermissions;
        private System.Windows.Forms.GroupBox gbAssignedPermissions;
        private System.Windows.Forms.Button btnAddPermission;
        private System.Windows.Forms.ListView lvPermissions;
        private System.Windows.Forms.ColumnHeader colResource;
        private System.Windows.Forms.ColumnHeader colCreate;
        private System.Windows.Forms.ColumnHeader colRead;
        private System.Windows.Forms.ColumnHeader colUpdate;
        private System.Windows.Forms.ColumnHeader colDelete;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.GroupBox gbEffectivePermissions;
        private System.Windows.Forms.ComboBox ddlDataView;
        private System.Windows.Forms.Label lblShowEffectivePermissions;
        private System.Windows.Forms.ComboBox ddlTable;
        private System.Windows.Forms.Label lblDelete;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.Label lblRead;
        private System.Windows.Forms.Label lblCreate;
        private System.Windows.Forms.Label lblDeleteTitle;
        private System.Windows.Forms.Label lblUpdateTitle;
        private System.Windows.Forms.Label lblReadTitle;
        private System.Windows.Forms.Label lblCreateTitle;
        private System.Windows.Forms.ColumnHeader colRestriction;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label lblPasswordNotSet;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.GroupBox gbCooperatorInformation;
        private System.Windows.Forms.TabControl tcCooperator;
        private System.Windows.Forms.TabPage tpCoopGeneral;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtOrganization;
        private System.Windows.Forms.Label lblOrganization;
        private System.Windows.Forms.TextBox txtJob;
        private System.Windows.Forms.Label lblJob;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TabPage tpCoopContactInfo;
        private System.Windows.Forms.TextBox txtPrimaryPhone;
        private System.Windows.Forms.Label lblPhones;
        private System.Windows.Forms.TextBox txtPostalIndex;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.TextBox txtAddressLine3;
        private System.Windows.Forms.TextBox txtAddressLine2;
        private System.Windows.Forms.TextBox txtAddressLine1;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblDiscipline;
        private System.Windows.Forms.TextBox txtFax;
        private System.Windows.Forms.Label lblFax;
        private System.Windows.Forms.TextBox txtSecondaryPhone;
        private System.Windows.Forms.Label lblSecondaryPhone;
        private System.Windows.Forms.Label lblPrimaryPhone;
        private System.Windows.Forms.TextBox txtInitials;
        private System.Windows.Forms.Label lblInitials;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TabPage tpCoopNotes;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtOrganizationAbbreviation;
        private System.Windows.Forms.Label lblOrganizationAbbreviation;
        private System.Windows.Forms.TabPage tpCoopGeographic;
        private System.Windows.Forms.ComboBox ddlSiteCode;
        private System.Windows.Forms.Label lblRegionCode;
        private System.Windows.Forms.Label lblSiteCode;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Label lblCategoryCode;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox ddlLanguage;
        private System.Windows.Forms.Label lblGeography;
        private System.Windows.Forms.ContextMenuStrip cmPermissions;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionAdd;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionProperties;
        private System.Windows.Forms.Button btnSelectCooperator;
        private System.Windows.Forms.Label lblCurrentCooperator;
        private System.Windows.Forms.TextBox txtGeography;
        private System.Windows.Forms.ColumnHeader colPermGroup;
        private System.Windows.Forms.TabPage tpGroups;
        private System.Windows.Forms.ListView lvGroups;
        private System.Windows.Forms.ColumnHeader colGroupName;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.GroupBox gbGroupMembership;
        private System.Windows.Forms.ContextMenuStrip cmGroups;
        private System.Windows.Forms.ToolStripMenuItem cmiGroupAdd;
        private System.Windows.Forms.ToolStripMenuItem cmiGroupRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiGroupProperties;
        private System.Windows.Forms.Label lblUserIsDisabled;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.Button btnGeography;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionsExportList;
        private System.Windows.Forms.ToolStripMenuItem cmiGroupsExportList;
        private System.Windows.Forms.ComboBox ddlOrganizationRegionCode;
        private System.Windows.Forms.ComboBox ddlCategoryCode;
        private System.Windows.Forms.ComboBox ddlDisciplineCode;
        private System.Windows.Forms.TabPage tpWebLogin;
        private System.Windows.Forms.Button btnWebCooperatorSearch;
        private System.Windows.Forms.TextBox txtWebUserName;
        private System.Windows.Forms.Label lblWebUserName;
        private System.Windows.Forms.Label lblWebLastName;
        private System.Windows.Forms.TextBox txtWebLastName;
        private System.Windows.Forms.Label lblWebFirstName;
        private System.Windows.Forms.TextBox txtWebFirstName;
        private System.Windows.Forms.Button btnSetWebPassword;
    }
}
