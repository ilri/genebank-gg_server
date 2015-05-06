<%@ Page Title="Order" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="order.aspx.cs" Inherits="GrinGlobal.Web.Order" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
        $(document).ready(function() {
            $("#ddlUse").change(getUseSubitems);
            $("#dvItemComment").draggable();
        });

        var aid = '';
        var oldNote = '';

        function noteItemsClicked(item) {
            if (item == 'cancel') {
                $("#dvItemComment").hide("fast");
            }
            else if (item == 'save') {
                var itemKey = aid;
                var itemValue = $("#txtNote").val();
                var newNote = '&&&' + itemKey + ':' + itemValue + '|||';

                var curNotes = document.getElementById('<%= hfItemNotes.ClientID %>').value;
                
                if (oldNote != '') {
                    curNotes = curNotes.replace(oldNote, newNote);
                }
                else {
                    curNotes += newNote;
                }
                document.getElementById('<%= hfItemNotes.ClientID %>').value = curNotes;

                $("#dvItemComment").hide("fast");    
            }
        }

        function addNote(id, pinumber) {
            $("#lblNumber").text(pinumber);
            aid = id;

            var itemNotes = document.getElementById('<%= hfItemNotes.ClientID %>').value
            var newKey = '&&&' + aid + ':';
            // if there is any key here:
            var theNote = '';
            var i = itemNotes.indexOf(newKey);
            
            if (i > -1) {
                var sub = itemNotes.substring(i + newKey.length, itemNotes.length);
                var j = sub.indexOf('|||');
                theNote = sub.substring(0, j);
                
                oldNote = newKey + theNote + '|||';
            }
            else {
                oldNote = '';
            }
            $("#txtNote").val(theNote);
            $("#dvItemComment").show();
            $("#txtNote").focus();
        }

        function statementRead() {
            if (!document.getElementById('cbStatement').checked) {
                document.getElementById('<%= btnProcess.ClientID %>').disabled = true;
                document.getElementById('<%= btnProcess.ClientID %>').title = 'Please read above statement and check the box';
                $("#divMandatory").show();
            }
            else {
                document.getElementById('<%= btnProcess.ClientID %>').disabled = false;
                document.getElementById('<%= btnProcess.ClientID %>').title = '';
                $("#divMandatory").hide("fast");
            }      
            //document.getElementById('<%= btnProcess.ClientID %>').disabled = !document.getElementById('cbStatement').checked;
        }
        
        function getUseSubitems() {
            var selected = $("#ddlUse option:selected").text();
            $("#dvSelectUse").hide();
            
            if (selected == 'Other') {
                $("#dvOther").show();
                $("#divOther").show();
                $("#dvSubitems").hide();
                $("#ddlUseSub").html("");
            }
            else {
                $("#dvOther").hide();
                $("#divOther").hide();
                $("#txtOther").val();

                switch (selected) {
                    case "- Select -":
                        $("#ddlUseSub").html("");
                        $("#dvSubitems").hide();
                        $("#dvSelectUse").show();
                        break;
                    case "Research":
                        $("#ddlUseSub").html("");
                        var listResearch = new Array("Botanical/taxonomic investigations",
                                        "Genetic studies",
                                        "Entomological investigations",
                                        "Plant Pathological investigations",
                                        "Chemistry",
                                        "Varietal Development",
                                        "Bioremediation",
                                        "Weed Science",
                                        "Historical, cultural and anthropological research");

                        for (var i = 0; i <listResearch.length; i++) {
                            $("#ddlUseSub").append('<option value="' + listResearch[i] + '">' + listResearch[i] + '</option>');
                        }
                        $("#dvSubitems").show();
                        break;
                    case "Education":
                        $("#ddlUseSub").html("");
                        var listEducation = new Array("Public education, demonstrations",
                                        "Class instruction");
                        for (var i = 0; i < listEducation.length; i++) {
                            $("#ddlUseSub").append('<option value="' + listEducation[i] + '">' + listEducation[i] + '</option>');
                        }
                        $("#dvSubitems").show();
                        break;
                    case "Repatriation":
                        $("#ddlUseSub").html("");
                        $("#dvSubitems").hide();
                        break;
                    case "Home Gardening":
                        $("#ddlUseSub").html("");
                        $("#dvSubitems").hide();
                        break;
                    default:
                        break;
                }
            }
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel runat="server" id="pnlPlaceOrder" visible="True">
    <table runat="server"><tr><td>
    <b>Please review your cart items<asp:Label ID="lblCnt" runat="server" Text=""></asp:Label>: </b></td><td width="320"></td><td id="ShowHideButton">
        <asp:LinkButton ID="btnShowAll" runat="server" ForeColor="#0099CC" 
            onclick="btnShowAll_Click">Show All Comments</asp:LinkButton>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp
         <asp:LinkButton ID="btnHideAll" runat="server" ForeColor="#0099CC" 
            onclick="btnHideAll_Click">Hide All Comments</asp:LinkButton>
    </td> </tr></table>
        <div id="dvItemComment" class='<%= DVItemComment%>' 
        style='right:5%; position: absolute; height: 225px; width: 225px;background-color: #FFFF00;'>
            &nbsp;&nbsp;&nbsp;<small> For Item:</small><br />
            &nbsp;&nbsp;&nbsp;
            <label id="lblNumber" style="font-size: small"></label>
            <br />
            &nbsp;&nbsp;
            <textarea id="txtNote" style="height:150px; width:198px"></textarea>
            <br />
            &nbsp;&nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" height="20px" 
                width="95px" 
                OnClientClick="javascript:return noteItemsClicked('cancel');return false;" 
                CausesValidation="False" UseSubmitBehavior="False" />
            &nbsp;
            <asp:Button ID="btnSave" runat="server" height="19px" OnClientClick="javascript:return noteItemsClicked('save');return false;" 
                Text="Save" width="95px" CausesValidation="False" 
                UseSubmitBehavior="False" />
       </div>

        <asp:GridView ID="gvCart" runat="server" ShowFooter="True" 
        AutoGenerateColumns="False" DataKeyNames="accession_id"  CssClass="grid" 
        onrowdeleting="gvCart_RowDeleting" onrowdatabound="gvCart_RowDataBound" 
            onrowcommand="gvCart_RowCommand">
        <AlternatingRowStyle CssClass="altrow" />
<EmptyDataTemplate>
    You have no items in your cart.
</EmptyDataTemplate>
<Columns>
    <asp:TemplateField HeaderText="Quantity" Visible="false">
    <HeaderStyle CssClass="" />
        <ItemTemplate>
            <asp:TextBox ID="txtQuantity" CssClass='handle2' runat="server" Text='<%# Bind("quantity") %>' MaxLength="4" Width="40px"></asp:TextBox>
        </ItemTemplate>
    </asp:TemplateField> 
    <asp:TemplateField HeaderText="ID">
        <ItemTemplate>
            <a href="accessiondetail.aspx?id=<%# Eval("accession_id") %>"><%#Eval("pi_number") %></a> 
            <asp:Label runat="server" ID="lblNoteSign1" Font-Bold="True" ForeColor="Red" Visible="False"><sup>!</sup></asp:Label>
            <asp:Label runat="server" ID="lblNoteSign2" Font-Bold="True" ForeColor="Red" Visible="False"><sup>+</sup></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="btnPlus" runat="server" ImageUrl="~/images/plus.ico" Visible="False" ToolTip="Show Availability Comment"  CommandName="ShowNote"  CommandArgument="<%#((GridViewRow) Container).RowIndex %>" />
            <asp:ImageButton ID="btnMinus" runat="server" ImageUrl="~/images/minus.ico" Visible="False" ToolTip="Hide Availability Comment"  CommandName="HideNote"  CommandArgument="<%#((GridViewRow) Container).RowIndex %>" />
            <asp:Label runat="server"  ID="lblAvailCmt" Font-Size="Smaller"  Visible="False" CssClass="blueInfor"></asp:Label>
        </ItemTemplate>
        <ItemStyle />
    </asp:TemplateField>
    <asp:BoundField DataField="top_name" HeaderText="Plant Name" />
    <asp:TemplateField HeaderText="Taxonomy">
        <ItemTemplate>
            <nobr><a href='taxonomydetail.aspx?id=<%#Eval("taxonomy_species_id") %>'><%#Eval("taxonomy_name") %></a></nobr>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="standard_distribution_quantity" HeaderText="Amount" />
    <asp:TemplateField HeaderText="Form Distributed">
    <ItemTemplate>
         <asp:Label runat="server" ID="lblFormDistributed"></asp:Label>
         &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:ImageButton ID="btnPlusF" runat="server" ImageUrl="~/images/plus.ico" Visible="False" ToolTip="Show Distribution Comment"  CommandName="ShowNoteF"  CommandArgument="<%#((GridViewRow) Container).RowIndex %>" />
        <asp:ImageButton ID="btnMinusF" runat="server" ImageUrl="~/images/minus.ico" Visible="False" ToolTip="Hide Distribution Comment"  CommandName="HideNoteF"  CommandArgument="<%#((GridViewRow) Container).RowIndex %>" />
         <asp:Label runat="server"  ID="lblFormD2" Font-Size="Smaller" Visible="False" CssClass="blueInfor"></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Maintained by">
         <ItemTemplate>
             <a href='site.aspx?id=<%# Eval("site_id") %>'>
             <%#Eval("site") %></a>
         </ItemTemplate>
     </asp:TemplateField>
    <asp:BoundField DataField="type_code" HeaderText="Restrictions" />
      <asp:TemplateField>
        <ItemTemplate>
            <asp:LinkButton ID="btnRemove" runat="server" CssClass="system" Text="Remove" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to remove this item?');" ></asp:LinkButton>
        </ItemTemplate>
        <FooterTemplate>
            <asp:LinkButton ID="btnRemoveAll" runat="server" CssClass="system" Text="Empty Cart" OnClick="btnRemoveAll_Click" OnClientClick="javascript:return confirm('Are you sure you want to remove all items?');" ></asp:LinkButton>
        </FooterTemplate>
    </asp:TemplateField>
        <asp:TemplateField>
        <ItemTemplate>
            <a href='#' onclick="javascript:addNote('<%# Eval("accession_id")%>','<%# Eval("pi_number")%>'); return false;" title='Add/Edit Note'><img src='images/note.ico' alt='Note' /></a> 
        </ItemTemplate>
    </asp:TemplateField>

</Columns>
</asp:GridView>
<asp:Label runat="server" ID="lblFootNote" Visible="False" Font-Size="Smaller"></asp:Label>
<br />
    <table>
    <tr><td><span style="color: #ff0066">*</span><b>Intended use for this germplasm:</b></td>
    <td>
        <asp:DropDownList ID="ddlUse" runat="server"
            CausesValidation="True" 
            onselectedindexchanged="ddlUse_SelectedIndexChanged" AutoPostBack="True" >
        </asp:DropDownList>    
         </td><td>
        <div id="dvSelectUse" class='<%= DVSelectUse %>' style="color: red; font-weight: bold">Please select a value from the list</div>
       </td>
    </tr>  
    <tr><td></td>
    <td>  
        <asp:DropDownList ID="ddlUseSub" runat="server" Visible="False" 
            AutoPostBack="True" onselectedindexchanged="ddlUseSub_SelectedIndexChanged1">
        </asp:DropDownList>
    </td><td>
    <div id="dvSelectSubUse" class='<%= DVSelectSubUse %>' style="color: red; font-weight: bold">Please select a value from the list</div>
    </td></tr>
    <tr><td align="right">
        <asp:Label ID="lblOther" runat="server" Text="Please provide more detail:" 
            Visible="False"></asp:Label></td><td  colspan="2"> 
        <asp:TextBox ID="txtOther" runat="server" Width="400px" 
            Height="70px" TextMode="MultiLine" Visible="False"></asp:TextBox>
     </td></tr>
    </table>
    <asp:Panel ID="pnlPlanned" runat="server" Visible="False">
    <br />
    <table><tr><td>
    <span style="color: #ff0066">*</span>
    <b>Describe your planned research use of this NPGS Germplasm:</b> </td><td></td></tr>
    <tr><td>&nbsp;&nbsp;(Information on your results is expected to be provided at the conclusion of your research)</td>
    <td><div id="dvPlanned" class='<%= DVPlannedUse %>' style="color: red; font-weight: bold">Please enter your response</div></td></tr></table>
    <asp:TextBox ID="txtPlanned" runat="server" Height="61px" TextMode="MultiLine" Width="626px"></asp:TextBox>
    </asp:Panel>
    <b>
    <br />
    Special instructions for the order:<br />
    <asp:TextBox ID="txtSpecial" runat="server" Height="61px" TextMode="MultiLine" 
        Width="626px"></asp:TextBox>
    </b>
    <br />
    <b>
    <br />
    Shipping Information:</b><br/>
    <div id="divShipping" align="left" style="width: 619px;">
    <asp:Label ID="lblShippingName" runat="server" Text=""></asp:Label><br/>
    <asp:Label ID="lblShipping1" runat="server" Text=""></asp:Label><br/>
    <asp:Label ID="lblShipping2" runat="server" Text=""></asp:Label> 
    <asp:Label ID="lblShipping3" runat="server" Text=""></asp:Label> 
    <asp:Label ID="lblShipping4" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="lblCarrier" runat="server"></asp:Label>
        <br />
    <table>
    <tr><td>Phone:</td><td>
    <asp:Label ID="lblShippingPhone" runat="server" Text=""></asp:Label></td></tr>
    <tr runat="server" id="tr_altPhone"><td>Alt Phone:</td><td>
    <asp:Label ID="lblShippingAltPhone" runat="server" Text=""></asp:Label></td></tr>
    <tr runat="server" id="tr_fax"><td>Fax:</td><td>
    <asp:Label ID="lblShippingFax" runat="server" Text=""></asp:Label></td></tr>
    </table>
    </div>
        <asp:Panel ID="pnlSMTA" runat="server" Width="828px">
    <br />
        <b><asp:Label ID="lblSMTA" runat="server" 
        Text='Please read the SMTA statements below and check your option box:' 
        ForeColor="Red"></asp:Label></b> 

        <asp:Label ID="lblSMTAIds" runat="server" Font-Size="Smaller" CssClass="blueInfor"></asp:Label> <br />
        <asp:Panel ID="Panel1" runat="server" BorderStyle="Groove" Width="800px" 
                ForeColor="Black">
            <b>SMTA</b><br />
        <br />
        <asp:Label ID="Label1" runat="server" Text='The material you have ordered is covered by 
<a href="http://www.planttreaty.org" target="_blank">The International Treaty on Plant Genetic Resources for Food and Agriculture</a>
and is therefore distributed under a 
<a href="http://www.planttreaty.org/smta_en.htm" target="_blank">Standard Material Transfer
Agreement</a> (SMTA).  By accepting shipment of the requested material you
are accepting the terms of the SMTA a recognize that your name and contact
information (address, phone number, e-mail address)  will be submitted as a
recipient of this material to the Governing Body of the Treaty.'></asp:Label><br /><br /></asp:Panel>
            <asp:RadioButtonList ID="rblSMTA" runat="server" AutoPostBack="True" 
                onselectedindexchanged="rblSMTA_SelectedIndexChanged">
                <asp:ListItem Value="YES">YES, I accept the terms of the SMTA and want to order the germplasm</asp:ListItem>
                <asp:ListItem Value="NO">NO, I do not accept the terms of the SMTA and do not want to order the germplasm.</asp:ListItem>
            </asp:RadioButtonList>
            <span>&nbsp;&nbsp; ( <b>If you click NO, the germplasm with SMTA requirement will be 
            removed from the order ! </b>)
            <br />
            </span>
        <br />
        </asp:Panel>
<%--       <b><asp:Label ID="lblReadStatement" runat="server" Text='Please read the statements below and check the box, before clicking "Process Order" button:' ForeColor="Red"></asp:Label></b> 
       <br />
        <asp:TextBox ID="tbAgree" runat="server" Height="35px" ReadOnly="True" 
            Width="804px" TextMode="MultiLine">The accessions within the NPGS are available in small quantities for research, breeding, and education purposes. 
       </asp:TextBox>--%><br />
       <asp:Label ID="lblGuidance" runat="server" 
            Text="The accessions within the NPGS are available in small quantities for research, breeding, and education purposes. &nbsp;" Font-Bold="True" BorderStyle="Solid"></asp:Label>
       <br /><br />
       <asp:Label ID="lblMultiShip" runat="server" 
            Text="You may have ordered accessions from more than one NPGS site and your order maybe split between sites and may be handled in different ways. <br /> You may receive your material in several shipments. &nbsp;" Font-Bold="True" CssClass="blueInfor"></asp:Label>

       <asp:CheckBox ID="cbStatement" runat="server" 
            Text="I have read and understand the statement provided above." 
            AutoPostBack="True" oncheckedchanged="cbStatement_CheckedChanged" Visible="False" Checked="True" />
        <br />
        <br />
        <asp:Label ID="lblConfirmEmail" runat="server" Text=""></asp:Label>
        <br /><br />
            <asp:Button ID="btnProcess" runat="server" Text="Submit Order" onclick="btnProcess_Click" Width="107px" Enabled="False" ToolTip="Please answer all questions on the form in order to proceed" /> 
        <br /><br />
        <a href='#' onclick='javascript:$("#divPaperwork").toggle("fast");return false;'>Privacy and Paperwork Reduction Act Statements</a>   
        <br />
        <div id='divPaperwork' class='hide'>
<asp:TextBox ID="txtPirvacy" runat="server" Height="175px" ReadOnly="True" 
            TextMode="MultiLine" Width="804px">
Privacy Act Information: This information is provided pursuant to Public Law 93-579 (Privacy Act of 1974) for individuals completing Federal records and forms that solicit personal information. The authority is Title 5 of the U.S. Code, sections 1302, 3301, 3304, and 7201.

Purpose and Routine Uses: The information from this form is used solely to respond to you regarding the service you have requested. No other uses will be made of this information.

Effects of Non-Disclosure: Providing this information is voluntary, however, without it we may not be able to respond to you regarding the service you have requested.

Paperwork Reduction Act Statement: A Federal agency may not conduct or sponsor, and a person is not required to respond to a collection of information unless it displays a current valid OMB control number.

Public Burden Statement: Public reporting burden for this collection of information is estimated to vary from two to four minutes with an average to three minutes per response, including time for reviewing instructions, and completing and reviewing the collection of information. Send comments regarding this burden estimate or any other aspect of this collection of information, including suggestions for reducing this burden, to Department of Agriculture, Clearance Officer, OIRM, Room 404-W, Washington, D.C. 20250, and to the Office of Information and Regulatory Affairs, Office of Management and Budget, Washington, D.C. 20503.
</asp:TextBox>
</div> <br/>
         <input type='hidden' runat="server" value='' id='hfItemNotes' name='hfItemNotes' />
    </asp:Panel>
 <asp:Panel runat="server" id="pnlDisplayOrder" visible="false">
 <h2><b>Confirmation of Orders</b></h2>
 <br />
 
 <table>
 <tr><td width="250" ></td><td width="350"></td></tr>
     <tr style="background-color:#2f571b; color: #FFFFFF">
         <td  ><b >Order Detail Number: </b> 
             &nbsp;<asp:Label ID="lblOrderIDs" runat="server" ></asp:Label>
        </td>
         <td  
             align="right"><b>Order Status:</b> Submitted
        </td>
     </tr>
     <tr>
         <td >
        </td>
         <td>
        </td>
     </tr>
     <tr>
         <td >
             <b>Requestor:</b></td>
         <td>
             <b>Ship To:</b></td>
     </tr>
     <tr>
         <td >
        </td>
         <td >
        </td>
     </tr>
     <tr>
         <td>
             <asp:Label ID="lblName" runat="server"></asp:Label>
        </td>
         <td >
             <asp:Label ID="lblAdd1" runat="server"></asp:Label>
        </td>
     </tr>
     <tr>
         <td >
             <asp:Label ID="lblOrganization" runat="server"></asp:Label>
        </td>
         <td>
             <asp:Label ID="lblAdd2" runat="server"></asp:Label>
        </td>
     </tr>
     <tr>
         <td >
             <asp:Label ID="lblPhone" runat="server"></asp:Label>
         </td>
         <td>
             <asp:Label ID="lblAdd3" runat="server"></asp:Label>
         </td>
     </tr>
     <tr>
         <td >
             <asp:Label ID="lblFax" runat="server"></asp:Label>
         </td>
         <td>
             <asp:Label ID="lblAdd4" runat="server"></asp:Label>
         </td>
     </tr>
 </table>
 
 <br />
     <b>&nbsp;Ordered Items:</b><br />
     <asp:GridView ID="gvOrderItems" runat="server" AutoGenerateColumns="False" 
         CssClass="grid" onrowdatabound="gvOrderItems_RowDataBound">
         <AlternatingRowStyle CssClass="altrow" />
         <EmptyDataTemplate>
             You have no order items.
         </EmptyDataTemplate>
         <Columns>
             <asp:TemplateField HeaderText="ID">
                 <ItemTemplate>
                     <a href='accessiondetail.aspx?id=<%# Eval("accession_id") %>'>
                     <%#Eval("pi_number") %></a>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:BoundField DataField="top_name" HeaderText="Plant Name" />
             <asp:TemplateField HeaderText="Taxonomy">
                 <ItemTemplate>
                     <nobr>
                     <a href='taxonomydetail.aspx?id=<%#Eval("taxonomy_species_id") %>'>
                     <%#Eval("taxonomy_name") %></a></nobr>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:BoundField DataField="standard_distribution_quantity" 
                 HeaderText="Distribution Amt" />
            <asp:TemplateField HeaderText="Form Distributed">
            <ItemTemplate>
                 <asp:Label runat="server" ID="lblFormDistributed"></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Maintained by">
                 <ItemTemplate>
                     <a href='site.aspx?id=<%# Eval("site_id") %>'>
                     <%#Eval("site") %></a>
                 </ItemTemplate>
             </asp:TemplateField>
            <asp:BoundField DataField="type_code" HeaderText="Restrictions" />
            <asp:BoundField DataField="note" HeaderText="Comments" ItemStyle-Width="200"/>
             </Columns>
         </asp:GridView>
     <br />
     <b>Intended use for this germplasm:</b><br />
     &nbsp;&nbsp;&nbsp;
     <asp:Label ID="lblIntended" runat="server"></asp:Label>
     <br />
     <b>
     <br />
     Special instructions for the order:<br />
     </b> 
     &nbsp;&nbsp;&nbsp;<asp:Label ID="lblSpecial" runat="server"></asp:Label>
     
     <br />
     <asp:Label ID="lblCarrierConfirm" runat="server"></asp:Label>
     <br />
     
     <br />
     <asp:Label ID="lblEmail" runat="server"></asp:Label>
     &nbsp;It is recommended that you print <a href="#" 
         onclick="window.print(); return false;" style="border:none">
     <input alt="" src="images/printer.ico" type="image" /></a> this page for your records.<br />
     <br />
     Status of this order can be found by returning to this site and click <a href="OrderHistory.aspx">My Order History</a>. Also use this link to add attachment(s).
     <br />
     <br /><br />
 </asp:Panel>
</asp:Content>
