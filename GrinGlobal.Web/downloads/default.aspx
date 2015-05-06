<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site2.Master" CodeBehind="default.aspx.cs" Inherits="GrinGlobal.Web.downloads.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Documentation and Installers - GRIN-Global Web v <%=Application["VERSION"] %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
   <div>
    <h2>Documentation</h2>
    <p>
		Please bookmark the following link: 
	</p>
	<ul>
		<li><a href='http://www.grin-global.org/index.php/Training' target='_blank'>http://www.grin-global.org/index.php/Training</a></li>
	</ul>
	<p>
    	This page contains links to the following:
	</p>
    <ul>
	    <li>Installation, user, and administrator guides</li>
	    <li>“How-to” videos</li>
	    <li>Training exercises</li>
	</ul>
    <p>	
		Under the “Manuals &amp; Training Documents” section, there is an installation guide.
		Use that to assist you in installing GRIN-Global.  After installation, we recommend all new users to view the videos and to complete the tutorials.
	</p>
	<p>
	    Additional videos and exercises will be posted to this page on an ongoing basis, so visit often.
	</p>
        <!-- <a href='http://www.ars-grin.gov/npgs/gringlobal/docs/gg_curator_tool_usermanual.pdf' target='_blank'>Curator Tool User Manual (work-in-progress)</a> -->
    </p>


    <h2>Installers</h2>
        <a href="javascript: Disclaimer" onclick="javascript:window.open('/gringlobal/disclaimer.aspx','','scrollbars=yes,titlebar=no,width=600,height=480')"> View GRIN-Global disclaimer</a>
    <br /><br />

    <asp:GridView ID="gvExecutables" runat="server" AutoGenerateColumns="False" 
        onrowdeleting="gvExecutables_RowDeleting" DataKeyNames="FileName" CssClass="grid" >
        <AlternatingRowStyle CssClass="altrow" />
        <EmptyDataTemplate>
            There are currently no installer files available on this server.
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="File">
                <ItemTemplate>
                    <a href='../uploads/installers/<%# Eval("FileName") %>'><%# Eval("DisplayName") %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Modified">
                <ItemTemplate>
                    <%# Eval("LastWriteTime", "{0:yyyy-MM-dd HH:mm:ss tt zzz}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File Size (MB)">
                <ItemTemplate>
                    <div style='width:100%;text-align:right'>
                    <%# Eval("Size", "{0:###,###,##0.0#}") %>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="Button1" runat="server" CausesValidation="false" 
                        CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <ul><li>Additional software may be needed to meet the installation or enhancement requirements for the GRIN-Global system.</li><br />
    <li>During the installation process, the Updater file listed above (Grin-Global_Updater_Setup.exe) will launch a second file, a Grin-Global_Updater_Setup.msi file. Some firewalls will not allow this companion .msi file to run. When that is the case, you will need to manually download a compressed file which contains both of these files (GrinGlobal_Updater_Setup.zip  file listed under the “Other Supporting Files” heading (below).  For more information, refer to the Installation Guide section, <i>Installing with Firewalls</i>. 
    </ul>
    <p>&nbsp;</p>
    <asp:Panel ID="pnlUploadExecutable" runat="server">
        <fieldset style='width:600px'>
            <legend>Upload Executable File</legend>
            <asp:FileUpload ID="filUploadExecutable" runat="server" />
            <br />
            <asp:Button id="btnUploadExecutable" runat="server" Text="Upload" 
                onclick="btnUploadExecutable_Click" />
        </fieldset>
    </asp:Panel>
    
    <h2>Dataviews</h2>

    <asp:GridView ID="gvDataviews" runat="server" AutoGenerateColumns="False" 
        onrowdeleting="gvDataviews_RowDeleting" DataKeyNames="FileName" CssClass="grid" >
        <AlternatingRowStyle CssClass="altrow" />
        <EmptyDataTemplate>
            There are currently no dataview files available on this server.
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="File">
                <ItemTemplate>
                    <a href='../uploads/dataviews/<%# Eval("FileName") %>'><%# Eval("DisplayName") %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Modified">
                <ItemTemplate>
                    <%# Eval("LastWriteTime", "{0:yyyy-MM-dd HH:mm:ss tt zzz}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File Size (MB)">
                <ItemTemplate>
                    <div style='width:100%;text-align:right'>
                    <%# Eval("Size", "{0:###,###,##0.0#}") %>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="false" 
                        CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Panel ID="pnlUploadDataview" runat="server">
        <fieldset style='width:600px'>
            <legend>Upload Dataview File</legend>
            <asp:FileUpload ID="filUploadDataview" runat="server" />
            <br />
            <asp:Button id="btnUploadDataview" runat="server" Text="Upload" 
                onclick="btnUploadDataview_Click" />
        </fieldset>
    </asp:Panel>

    <h2>Other Supporting Files</h2>

    <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False" 
        onrowdeleting="gvFiles_RowDeleting" DataKeyNames="FileName" CssClass="grid" >
        <AlternatingRowStyle CssClass="altrow" />
        <EmptyDataTemplate>
            There are currently no other files available on this server.
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="File">
                <ItemTemplate>
                    <a href='../uploads/documents/<%# Eval("FileName") %>'><%# Eval("DisplayName") %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Modified">
                <ItemTemplate>
                    <%# Eval("LastWriteTime", "{0:yyyy-MM-dd HH:mm:ss tt zzz}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File Size (MB)">
                <ItemTemplate>
                    <div style='width:100%;text-align:right'>
                    <%# Eval("Size", "{0:###,###,##0.0#}") %>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="false" 
                        CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Panel ID="pnlUploadFile" runat="server">
        <fieldset style='width:600px'>
            <legend>Upload Files</legend>
            <asp:FileUpload ID="filUploadFile" runat="server" />
            <br />
            <asp:Button id="btnUploadFile" runat="server" Text="Upload" 
                onclick="btnUploadFile_Click" />
        </fieldset>
    </asp:Panel>
    <br />
    </div>
</asp:Content>