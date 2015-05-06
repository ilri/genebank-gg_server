<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="feedback10yrsample.aspx.cs" Inherits="GrinGlobal.Web.trialsmain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <p>
        <b>Ten Year Perfomance Report for</b>: Ames, IA</p>
    <p>
        <asp:Label ID="Label1" runat="server" Text="Ames/PI Number:"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Trial Location:"></asp:Label>
        <br />
        <asp:Label ID="Label3" runat="server" Text="Location Code:"></asp:Label>
        <br />
        <asp:Label ID="Label4" runat="server" Text="Plot Name:"></asp:Label>
        <br />
        <asp:Label ID="Label5" runat="server" Text="Your Number:"></asp:Label>
    </p>
    <p>
        <asp:Label ID="Label6" runat="server" Text="Genus species:"></asp:Label>
        <br />
        <asp:Label ID="Label7" runat="server" Text="Date Planted:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;<asp:Label ID="Label35" runat="server" Text="(mm/dd/yyyy)"></asp:Label>
        <br />
        <asp:Label ID="Label8" runat="server" Text="Number Alive Year Five:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox2" runat="server" Width="60px"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="Label9" runat="server" Text="Number Alive at Present*:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox3" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label10" runat="server" Text="Current Year's Shoot Growth (cm):"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox4" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label11" runat="server" Text="Average Plant Height (m)*:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox5" runat="server" Height="21px" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label12" runat="server" Text="Average Plant Spread (m):"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox6" runat="server" Width="58px"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="Label13" runat="server" Text="Damage due to Animal Browsing*:"></asp:Label>
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label16" runat="server" 
            Text="(1-no browsing - 9-&gt;75% browsing)"></asp:Label>
        <br />
        <asp:Label ID="Label14" runat="server" Text="Damage dut to Winter Injury*:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList2" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label17" runat="server" Text="(1-no injury - 9-plant death)"></asp:Label>
        <br />
        <asp:Label ID="Label15" runat="server" 
            Text="Additional Comments for Injury or Loss"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox7" runat="server" Height="72px" TextMode="MultiLine" 
            Width="443px"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="Label18" runat="server" Text="Insect and Disease Observations:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox8" runat="server" Width="316px"></asp:TextBox>
        <br />
        <asp:Label ID="Label19" runat="server" Text="Insect or Disease Rating:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList3" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label20" runat="server" Text="(1-susceptible, 9-resistant)"></asp:Label>
    </p>
    <p>
        <asp:Label ID="Label21" runat="server" Text="Insect*:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList4" runat="server">
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label22" runat="server" Text="Disease*:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList5" runat="server">
        </asp:DropDownList>
    </p>
    <p>
        <b>FOLIAGE OBSERVATIONS</b><br />
        <asp:Label ID="Label23" runat="server" Text="Foliage Quality Rating*:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList6" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label29" runat="server" Text="(1-poor, 9-excellent)"></asp:Label>
        <br />
        <asp:Label ID="Label24" runat="server" Text="Foliage Emergence Date:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
&nbsp;<asp:Label ID="Label30" runat="server" Text="(mm/dd/yyyy)"></asp:Label>
        <br />
        <asp:Label ID="Label25" runat="server" Text="Foliage Leaf Drop Date:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
&nbsp;<asp:Label ID="Label31" runat="server" Text="(mm/dd/yyyy)"></asp:Label>
        <br />
        <asp:Label ID="Label26" runat="server" Text="Full Color Display Rating*:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList7" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label32" runat="server" Text="(1-poor, 9-excellent)"></asp:Label>
        <br />
        <asp:Label ID="Label27" runat="server" Text="Fall Color Peak Date*:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
&nbsp;<asp:Label ID="Label33" runat="server" Text="(mm/dd/yyyy)"></asp:Label>
        <br />
        <asp:Label ID="Label28" runat="server" Text="Fall Leaf Colors*:"></asp:Label>
&nbsp;<br />
        <asp:Label ID="Label36" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label37" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox14" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label38" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox15" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label39" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox16" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label40" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox17" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label41" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox18" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label42" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox19" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label43" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox20" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label34" runat="server" Text="Duration of Fall Color (days)*:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox12" runat="server" Width="60px"></asp:TextBox>
    </p>
    <p>
        <b>FLOWER OBSERVATIONS</b><br />
        <asp:Label ID="Label44" runat="server" Text="Flower Display Effectiveness*:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList8" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label62" runat="server" Text="(1-poor, 9-excellent)"></asp:Label>
        <br />
        <asp:Label ID="Label45" runat="server" Text="Flower Display Peak Date*:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox29" runat="server"></asp:TextBox>
&nbsp;<asp:Label ID="Label63" runat="server" Text="(mm/dd/yyyy)"></asp:Label>
        <br />
        <asp:Label ID="Label46" runat="server" Text="Flower to Foliage Ratio*:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList12" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label64" runat="server" Text="(1-none, 9-&gt;75% Flowers)"></asp:Label>
        <br />
        <asp:Label ID="Label47" runat="server" Text="Flowers are a nuisance:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList9" runat="server">
        </asp:DropDownList>
&nbsp;<br />
        <asp:Label ID="Label48" runat="server" Text="Flower Fragrance Strength:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList10" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label65" runat="server" Text="(1-weak, 9-strong)"></asp:Label>
        <br />
        <asp:Label ID="Label49" runat="server" Text="Flower Fragrance Attractiveness:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList11" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label66" runat="server" Text="(1-unattractive, 9-attractive)"></asp:Label>
        <br />
        <asp:Label ID="Label50" runat="server" Text="Flower Diameter (cm):"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox31" runat="server" Width="60px"></asp:TextBox>
&nbsp;<br />
        <asp:Label ID="Label51" runat="server" Text="Flower Colors*:"></asp:Label>
        <br />
        <asp:Label ID="Label52" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox21" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label53" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox22" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label54" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox23" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label55" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox24" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label56" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox25" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label57" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox26" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label58" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox27" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label59" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox28" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label60" runat="server" 
            Text="Duration of Flower Display (days)*:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox32" runat="server" Height="23px" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label61" runat="server" Text="Inflorescence Diameter (cm):"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox33" runat="server" Height="23px" Width="60px"></asp:TextBox>
    </p>
    <p>
        <b>FRUIT OBSERVATIONS</b><br />
        <asp:Label ID="Label74" runat="server" Text="Fruit Display Effectiveness*:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList13" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label67" runat="server" Text="(1-poor, 9-excellent)"></asp:Label>
        <br />
        <asp:Label ID="Label68" runat="server" Text="Fruit Display Peak Date*:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox34" runat="server"></asp:TextBox>
&nbsp;<asp:Label ID="Label69" runat="server" Text="(mm/dd/yyyy)"></asp:Label>
        <br />
        <asp:Label ID="Label70" runat="server" Text="Fruit to Foliage Ratio*:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList14" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Label ID="Label71" runat="server" Text="(1-none, 9-&gt;75% Flowers)"></asp:Label>
        <br />
        <asp:Label ID="Label72" runat="server" Text="Fruit are a nuisance:"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList15" runat="server">
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label75" runat="server" Text="Fruit Diameter (cm):"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox35" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label76" runat="server" Text="Fruit Colors*:"></asp:Label>
        <br />
        <asp:Label ID="Label77" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox36" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label78" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox37" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label79" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox38" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label80" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox39" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label81" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox40" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label82" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox41" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label83" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox42" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label84" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox43" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label87" runat="server" Text="Intermediate Fruit Color:"></asp:Label>
&nbsp;<br />
        <asp:Label ID="Label88" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox46" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label89" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox47" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label90" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox48" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label91" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox49" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label92" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox50" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label93" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox51" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label94" runat="server" Text="Color:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox52" runat="server"></asp:TextBox>
&nbsp;
        <asp:Label ID="Label95" runat="server" Text="Percentage:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox53" runat="server" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label85" runat="server" 
            Text="Duration of Fruit Display (days)*:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox44" runat="server" Height="23px" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label86" runat="server" Text="Fruit Cluster Diameter (cm):"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox45" runat="server" Height="23px" Width="60px"></asp:TextBox>
    </p>
    <p>
        <b>SUMMARY</b><br />
        <asp:Label ID="Label96" runat="server" Text="Summary of Plant Performance*:"></asp:Label>
&nbsp;<br />
        <asp:Label ID="Label97" runat="server" Text="Would you recommend this plant*:"></asp:Label>
&nbsp;<br />
        <asp:Label ID="Label98" runat="server" 
            Text="Please list reasons for recommendation:"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox54" runat="server" Height="68px" TextMode="MultiLine" 
            Width="385px"></asp:TextBox>
        <br />
        <asp:Label ID="Label99" runat="server" 
            Text="Please list vatiation among plants:"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox55" runat="server" Height="68px" TextMode="MultiLine" 
            Width="385px"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="Label100" runat="server" Text="Cooperator Name*:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox56" runat="server" Width="286px"></asp:TextBox>
        <br />
        <asp:Label ID="Label101" runat="server" Text="Report Date*:"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox57" runat="server"></asp:TextBox>
&nbsp;<asp:Label ID="Label102" runat="server" Text="(mm/dd/yyyy)"></asp:Label>
    </p>
</asp:Content>
