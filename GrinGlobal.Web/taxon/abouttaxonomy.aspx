<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="abouttaxonomy.aspx.cs" Inherits="GrinGlobal.Web.help.abouttaxonomy" MasterPageFile="~/Site1.Master" Title="About GRIN Taxonomy for Plants"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<asp:Panel ID="pnlIndex" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center> 
	<h3><b>TAXONOMIC INFORMATION ON CULTIVATED PLANTS IN THE USDA-ARS GERMPLASM RESOURCES INFORMATION NETWORK (GRIN)</b></h3>
	
	<font size="1">
	<a href="http://ars.usda.gov/pandp/people/people.htm?personid=6062"
     onclick="javascript:site('ars.usda.gov/pandp/people/people.htm?personid=6062');return false;"
    title="Link to Personal Page for John Wiersema" target="blank">John H. Wiersema</a><br/>
	National Germplasm Resources Laboratory<br />
	Agricultural Research Service<br/>
	United States Department of Agriculture<br/>
	Beltsville, Maryland 20705-2350, U.S.A.
	<br/>
	<hr>
	Updated, modified version of paper presented to the
    "Second International Symposium on the Taxonomy of 
	Cultivated Plants" in Seattle, Washington, USA (10-15 August1994)
	</font><br/><br/>
 		<table border='0' cellpadding='5' summary="table of contents">
		<tr>
		<td valign="top" align="left">
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=summ">Summary</a></b></li>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=intro">Introduction</a></b></li>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=hist">History of GRIN Taxonomy</a></b></li>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=scope">Scope of GRIN Taxonomy</a></b></li>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=scient">Scientific Names</a></b></li>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=common">Common Names</a></b></li>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=econ">Economic Importance</a></b></li>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=distrib">Geographical Distribution</a></b></li>
		</td>
        <td width="60"></td>
		<td valign="top" align="left">
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=liter">Literature References</a></b>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=spec">Special-Purpose Data Sets</a></b>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=basis">Basis of Taxonomic Decisions</a></b>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=concl">Concluding Remarks</a></b>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=acknowl">Acknowledgements</a></b>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=refer">References Cited</a></b>
		<li><b><a href="abouttaxonomy.aspx?language=en&chapter=symb">Symbols and Abbreviations</a></b>
		</td>
		</tr>
		</table>
		<br />
    <asp:Button ID="btnNext" runat="server" Text="Next" onclick="DisplaySummary" />
	</center>
	<br />
    <hr />
	<br /><br />
</asp:Panel>
<asp:Panel ID="pnlSumm" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>SUMMARY</b>
<p align="justify">The National Plant Germplasm System of the Agricultural
Research Service, U.S. Department of Agriculture maintains a computer
database, the Germplasm Resources Information Network (GRIN), for the
management of and as a source of information on its <font size=1><b><%=accnt%></b></font> germplasm
accessions.  The taxonomic portion of GRIN provides the classification and
nomenclature for these genetic resources and many other economic plants on
a worldwide basis.  Included in GRIN T<font size=1>AXONOMY</font> are
scientific names for <font size=1><b><%=gncnt%></b></font> genera (<font size=1><b><%=acgncnt%></b></font> accepted) and <font size=1><b><%=igncnt%></b></font>
infragenera (<font size=1><b><%=acigncnt%></b></font> accepted) and <font size=1><b><%=spcnt%></b></font> species or infraspecies
(<font size=1><b><%=acspcnt%></b></font> accepted) with common names, geographical distributions,
literature references, and economic impacts.  Generally recognized
standards for abbreviating author's names and botanical literature have
been adopted in GRIN.  The scientific names are verified, in accordance
with the international rules of botanical nomenclature, by taxonomists of
the National Germplasm Resources Laboratory using available taxonomic
literature and consultations with taxonomic specialists.  Included in GRIN
T<font size=1>AXONOMY</font> are federal- and state-regulated noxious
weeds and federally and internationally listed threatened and endangered
plants.  Since 1994 <a href="taxonomyquery.aspx" title="Search GRIN Taxonomy">GRIN taxonomic data</a> have been searchable on the World-Wide-Web.</p>
<br />
<asp:Button ID="btnSummPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnSummPre_Click" /><asp:Button ID="btnSummCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnSummCont_Click" />
    <asp:Button ID="btnSummNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnSummNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlIntro" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>INTRODUCTION</b>
<p align="justify">The United States Department of Agriculture (USDA),
Agricultural Research Service's (ARS) National Plant Germplasm System
(NPGS) currently maintains over <font size=1><b><%=accnt%></b></font> accessions of mostly 
economically important vascular plants.  It also coordinates the
activities of more than 25 USDA seed and clonal germplasm sites, and
interacts with the international germplasm community and scientific public
through the Germplasm Resources Information Network (GRIN).  The GRIN
database contains information on all genetic resources preserved by NPGS,
including accessions of both domestic and foreign origin.  Though the
emphasis is on major, minor, or potential crops and their wild and weedy
relatives, many other categories of plants are represented including
ornamentals and some rare and endangered plants.  A range of
data--including passport, taxonomic, descriptor, observation, evaluation,
and inventory data--for each germplasm accession is available in GRIN. The
taxonomic data providing the overall organization for germplasm accessions
in GRIN is the focus of this paper. For information on other aspects of GRIN or NPGS see Janick (1989) or consult the <a href="http://www.ars-grin.gov/npgs/" target="_blank">GRIN database.</a> </p>
<br />
<asp:Button ID="btnIntroPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnIntroPre_Click" /><asp:Button ID="btnIntroContent"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnIntroCont_Click" />
    <asp:Button ID="btnIntroNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnIntroNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlHist" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>HISTORY OF GRIN TAXONOMY</b>
<p align=JUSTIFY>GRIN taxonomic data was originally extracted from the
Nomenclature File of the former Plant Exploration and Taxonomy Laboratory
(PETL).  The origin of the Nomenclature File and its relationship to the
Plant Introduction Office (PIO) since 1898 were described at the First 
International Symposium on Cultivated Plants (Terrell, 1986a). The
purpose of the File from the beginning was to provide correct scientific
names for the plants introduced into NPGS.</p>

<p align=JUSTIFY>Many germplasm introductions were received by exchange
with foreign institutions, and others were collected throughout the world
by American plant explorers.  All the introductions accessioned through
PIO were assigned consecutive Plant Inventory (P.I.) numbers, and
distributed to the appropriate specialist or germplasm site.  Others have
gone directly to germplasm stations, many of these to be later processed
by PIO.</p>

<p align=JUSTIFY>For each accession a determination of the correct
taxonomic nomenclature was made by taxonomists maintaining the
Nomenclature File. While most scientific names in the File were for
introductions, many names, mainly of economic plants, were added by USDA
taxonomists for other reasons.  Prior to GRIN-2, the version of GRIN
initiated at the time of the First Symposium, the PIO accession data and
PETL nomenclature data were in separate card files.  The transfer of the
Nomenclature File to GRIN-2 was completed in 1987 thus making this
taxonomy directly accessible to the entire NPGS community.</p>

<p align=JUSTIFY>Since the assimilation of the Nomenclature File into
GRIN, GRIN taxonomic data has continued to expand in response to the needs
of NPGS, the Agricultural Research Service, and other agricultural
agencies.  An extensive publication on world economic plants was completed
from GRIN data in 1999 and further extended the coverage of GRIN taxonomic
data to all plants in international commerce.  This publication, entitled
<i>World Economic Plants: a standard reference</i>, may be obtained from 
<a href="http://www.crcpress.com:80/us/product.asp?sku=2119+++&dept%5Fid=1target=_top"
title="Link to CRC Press" target="_blank">CRC Press</a>.  Data from this publication may
be <a href="taxonomyquery.aspx">queried</a> on the internet here as well.</p>

<p align=JUSTIFY>From a previous gopher server, the World-Wide-Web
interface for GRIN taxonomic data was developed and implemented in 1994,
enabling users from throughout the world to access this information easily
and efficiently.  GRIN taxonomic data can thus be queried by scientific
name (family, genus, or species), common name, economic uses, or
distribution. Specialized searches on GRIN data relating to economic
plants, rare plants, noxious weeds, families and genera, or seed
associations are also possible.  Since GRIN taxonomic data have been
available online, usage has grown at a nearly exponential rate.  Currently
over 200,000 reports per month of GRIN taxonomic data are output to users
from around the world as a result of these searches.</p> 
<br />
<asp:Button ID="btnHistPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnHistPre_Click" /><asp:Button ID="btnHistCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnHistCont_Click" />
    <asp:Button ID="btnHistNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnHistNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlScope" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>SCOPE OF GRIN TAXONOMY</b>
<p align=JUSTIFY>Taxonomic and nomenclatural needs of NPGS are now met
through GRIN by botanists of the <a href="http://www.ars.usda.gov/main/site_main.htm?modecode=12-75-15-00"
title="Link to National Germplasm Resources Lab" target="blank">National Germplasm
Resources Laboratory</a> (NGRL) which is responsible for the taxonomy
area of the database.  GRIN T<font size=1>AXONOMY</font> includes all
accepted family and generic names from throughout the world.  By necessity
all <font size=1><b><%=acctaxcnt%></b></font> specific and infraspecific taxa represented by germplasm in
the NPGS are also included in this taxonomy, although that represents only
about a quarter of all accepted names from these ranks in GRIN.  A broad
range of economically important plants are treated by GRIN nomenclature,
including food or spice, timber, fiber, drug, forage, soil-building or
erosion-control, genetic resource, poisonous, weedy, and ornamental
plants.  Most or all species of important agricultural crop genera are
represented in GRIN; for other less economic genera only a portion of the
species may be represented.  When all species of a genus are represented
in GRIN this is indicated by a comment in the GRIN genus report.  Reference to the literature cited in GRIN may
provide information relating to the treatment of other species.</p>

<p align=JUSTIFY>The taxonomy area encompasses names governed by the
International Code of Botanical Nomenclature (ICBN; McNeill et al., 2006).
Names treated under the cultivated code (Brickell et al., 2009), such as
cultivars, may be linked to individual accessions in the accession area of
GRIN.  These cultivar or other designations are provided only to the
extent that they are represented by germplasm accessions.  Their inclusion
and verification is the responsibility of the site where the germplasm is
maintained.</p>
<br />
<asp:Button ID="btnScopePre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnScopePre_Click" /><asp:Button ID="btnScopeCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnScopeCont_Click" />
    <asp:Button ID="btnScopeNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnScopeNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlScient" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>CONTENT OF GRIN TAXONOMY</b>
<p align=JUSTIFY>Several types of data records are contained in GRIN
T<font size=1>AXONOMY</font>. These include accepted or synonymic
scientific names, common names, distributions, literature references, and
economic impacts.  Each of these is discussed below, and the number of
records currently in GRIN relating to each type is indicated.</p>
<br />
    <b>SCIENTIFIC NAMES</b>
<%--<p align=JUSTIFY>Accepted name records are searchable at the level of 
<a href=""; title="Search GRIN Taxonomy Family/Genus Data">family and genus</a> or 
<a href=""; title="Search GRIN Taxonomy Species Data">species and infraspecies</a>. 
--%><p align=JUSTIFY>
Accepted name records are searchable at the level of <a href="famgensearch.aspx">family and genus</a> or <a href="taxonomysearch.aspx">species and infraspecies</a>. The generic records include a complete
listing of the <font size=1><b><%=acgncnt%></b></font> accepted spermatophyte genera in the world and an
additional <font size=1><b><%=syngncnt%></b></font> synonym genera.  For each genus the author
is cited in accordance with Articles 46-50 of the ICBN (McNeill et al.,
2006), and conserved or rejected names are indicated.  The family to which
each genus is assigned is provided, and any alternative family
classifications in current use are indicated.  For genera whose acceptance
is doubtful or disputed an alternatively accepted genus may be indicated.
Many genera are provided with literature references documenting their
acceptance or family placement in GRIN, a recent taxonomic revision or
monograph, or recent molecular-based phylogenetic study of the genus. 
Nomenclatural comments are provided for problem genera.  An increasing
number of genera [<font size=1><b><%=infragencnt%></b></font>] (and families [<font size=1><b><%=infrafamcnt%></b></font>]) now have 
infrageneric (or infrafamilial) classification data present in GRIN, with 
the subordinate species (or genera) linked to the appropriate infrageneric 
(or infrafamilial) category.  The generic and family data in GRIN are 
derived from USDA Technical Bulletin 1796 (Gunn et al., 1992), <i>Families 
and genera of spermatophytes recognized by the Agricultural Research 
Service</i>. Generic and family concepts in that publication were 
formulated with the aid of over 200 taxonomic specialists.  Family and 
generic data continue to be periodically updated from current literature, 
and have been expanded to include pteridophytes.</p>

<p align=JUSTIFY>Species and subspecific records now total <font size=1><b><%=acspcnt%></b></font>
accepted and <font size=1><b><%=synspcnt%></b></font> synonym names in GRIN.  Binomials (<font size=1><b><%=bispcnt%></b></font>),
trinomials (<font size=1><b><%=trispcnt%></b></font>), and quadrinomials (<font size=1><b><%=quadspcnt%></b></font>) are included among
these. All such names
are assigned a unique identifying number in GRIN, the nomen number or
"taxno."  Names can be queried using these numbers at GRIN T<font
size=1>AXONOMY</font>'s
<a href="taxonomysimple.aspx">simple query option</a>. The inclusion of
infraspecific names for a given species is selective and not necessarily
exhaustive.  Each name at whatever rank is accompanied by author and place
of original publication.  Comments relating to nomenclatural matters,
parentage for hybrid taxa, or alternative Group names under the
cultivated code (Brickell et al., 2009) are provided for many names.
Author abbreviations conform to the international standard reference
<i>Authors of Plant Names</i> (Brummitt and Powell, 1992) and its updated 
<a href="http://www.rbgkew.org.uk/data/authors.html"
title="Link to on-line version of Authors of Plant Names" target="blank">on-line version</a>. Nonserial
botanical works (pre-1950) have been abbreviated according to the standard
reference <i>Taxonomic Literature</i> (Stafleu and Cowan, 1976-1988) and
its supplements (Stafleu and Mennega, 1992-2000; Dorr and Nicolson, 2008-2009), and 
publication dates
have been verified using that work. Serial publications are abbreviated
according to <i>Botanico-Periodicum-Huntianum</i>, its
<i>Supplementum</i> (Lawrence et al., 1968; Bridson and Smith, 1991), and BPH-2 
(Bridson et al., 2004). 
</p>

<p align=JUSTIFY>Each nomenclature record, as well as most other record
types, contains the date of and individual responsible for the most recent
modification. Since a change could be strictly editorial, a special field
also indicates if the name itself has been verified recently.  Usage of
GRIN taxonomic information should be confined to records which have been
verified. Currently all generic names and about 95% of species and
infraspecific names meet this criterion.  Since revisions of GRIN 
T<font size=1>AXONOMY</font> formerly proceeded on a family-by-family
basis, certain families are more thoroughly treated than others,
particularly those with important crop genera.  An example is the 
Fabaceae, for which the GRIN data were extensively reviewed and published
as USDA Technical Bulletin 1757, <i>Legume (Fabaceae) nomenclature in the
USDA germplasm system</i> (Wiersema et al., 1990).</p>
<br />
<asp:Button ID="btnScientPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnScientPre_Click" /><asp:Button ID="btnScientCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnScientCont_Click" />
    <asp:Button ID="btnScientNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnScientNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlCommon" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>COMMON NAMES</b>
<p align=JUSTIFY>Presently <font size=1><b><%=cncnt%></b></font> common names for <font size=1><b><%=taxcncnt%></b></font> taxa,
including <font size=1><b><%=langcncnt%></b></font> common names of non-English origin,
have been entered into GRIN. To avoid the necessity of treating the
multiple variations of a common name that can arise from differences in
spelling, word union, or hyphenation (e.g., sugar beet, sugar-beet, or
sugarbeet), we have attempted to standardize treatment of common names in
GRIN by adopting the conventions of Kartesz and Thieret (1991), on matters
of union or hyphenation of group names and modifiers.  Further decisions
on joining or separating the elements of common names follow usage in
<i>Webster's Third New International Dictionary</i> (Gove et al., 1961).
These rules dictate that group names are correctly applied only to certain
genera (such as rose for 
<a href="../taxonomygenus.aspx?id=10544" 
title="Link to GRIN Taxonomy for Rosa"><i>Rosa</i></a>
or vetch for 
<a href="../taxonomygenus.aspx?id=12701" 
title="Link to GRIN Taxonomy for Vicia"><i>Vicia</i></a>) 
or families (e.g., grass for Poaceae). Some <font size=1><b><%=gncncnt%></b></font> &quot;true
group&quot; 
names are provided in GRIN for genera. Usage of these true group names for
plants in other genera or families requires hyphenation or adjoining to
preceding modifiers (such as moss-rose for 
<a href="../taxonomydetail.aspx?id=29451" 
title="Link to GRIN Taxonomy for P. grandiflora"><i>Portulaca
grandiflora</i></a> or milk-vetch for 
<a href="../taxonomygenus.aspx?id=1094" 
title="Link to GRIN Taxonomy for Astragalus"><i>Astragalus</i></a>). 
General terms, such as tree, weed, or wort, that cannot be linked to any
particular plant group always require adjoining or hyphenation. A few
exceptions to allow usage of some true group names for more than one genus
exist, such as pitcherplant for 
<a href="../taxonomygenus.aspx?id=8171" 
title="Link to GRIN Taxonomy for Nepenthes"><i>Nepenthes</i></a>
and 
<a href="../taxonomygenus.aspx?id=10761" 
title="Link to GRIN Taxonomy for Sarracenia"><i>Sarracenia</i></a>,
especially when genera have been recently dismembered, such as wheatgrass
for 
<a href="../taxonomygenus.aspx?id=313" 
title="Link to GRIN Taxonomy for Agropyron"><i>Agropyron</i></a>,
<a href="../taxonomygenus.aspx?id=4201" 
title="Link to GRIN Taxonomy for Elymus"><i>Elymus</i></a>,
and 
<a href="../taxonomygenus.aspx?id=14438" 
title="Link to GRIN Taxonomy for Elytrigia"><i>Elytrigia</i></a>. 
</p>

<p align=JUSTIFY>Common names have been extracted from a variety of
sources, such as floras, agronomic or horticultural works, or economic
botany literature.  Although some names appear in several sources, at
least one source is presented in GRIN for each common name.  Sources are
frequently indicated using GRIN literature abbreviations, expansions of
which can usually be found by consulting the references cited for that
taxon.  No effort has been made to include every locally used common name
appearing in the literature; instead the focus has been to record those in
wider usage. Some common names clearly in restricted use, such as those
accompanying rare and endangered taxa, have been entered for reference
purposes.</p>
<br />
<asp:Button ID="btnCommonPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnCommonPre_Click" /><asp:Button ID="btnCommonCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnCommonCont_Click" />
    <asp:Button ID="btnCommonNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnCommonNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlEcon" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>ECONOMIC IMPORTANCE</b>
<p align=JUSTIFY>Currently <font size=1><b><%=econcnt%></b></font> economic impact records exist in GRIN 
for the <font size=1><b><%=taxeconcnt%></b></font> taxa for which economic plant data are provided.  GRIN
economic data are classified to two levels
adapted from the <i>Economic Botany Data Collection Standard</i> (Cook,
1995).  A total of 16 classes are recognized, including 13 from this
Standard: food, food additives, animal food, bee plants, invertebrate
food, materials, fuels, social uses, vertebrate poisons, non-vertebrate
poisons, medicines, environmental uses, and gene sources, with the
addition of classes for weeds, harmful organism hosts, and
CITES-regulated plants. Note that two of these added categories plus
vertebrate poisons do not represent beneficial uses, but are mostly
negative in their economic impact. The 16 classes are further subdivided
into 113 subclasses.  Sources of economic data are referenced in GRIN.  A
thorough discussion of GRIN economic data can be found at <a
href="http://www.ars-grin.gov/cgi-bin/npgs/html/wep.pl?language=en&chapter=econ1"  
title="Link to 'World Economic Plants: A Standard Reference'" target="blank">World
Economic Plants: A Standard Reference</a>.</p>
<br />
<asp:Button ID="btnEconPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnEconPre_Click" /><asp:Button ID="btnEconCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnEconCont_Click" />
    <asp:Button ID="btnEconNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnEconNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlDistrib" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>GEOGRAPHICAL DISTRIBUTION</b>
<p align=JUSTIFY>Currently <font size=1><b><%=distcnt%></b></font> distribution records exist in GRIN for
the <font size=1><b><%=taxdistcnt%></b></font> taxa for which distributional data are provided.  Each
record is a linkage between a continent, country, or state occurrence and
an accepted taxon name.  Country designations follow standards of the U.S.
Government as implemented in GRIN.  GRIN distribution records are grouped
into areas and regions in accordance with the standard publication
<i>World Geographical Scheme for Recording Plant Distributions</i> (Hollis
and Brummitt, 1992), which divides the terrestrial world into nine areas:
Africa, Antarctic, Asia-Temperate, Asia-Tropical, Australasia, Europe,
Northern America, Pacific, and Southern America.</p>

<p align=JUSTIFY>Distributions are given as reported in the literature or
by consulted specialists.  Native or potentially native distributions are 
recorded and displayed separately from cultivated, adventive, or 
naturalized distributions.  For weedy species this distinction is 
sometimes obscure, and some widespread taxa may have their entire 
distributions summarized as a comment.  Similarly, state distributions for 
most countries are not itemized for taxa widespread within those 
countries. However, a distributional report for a taxon in a geographical 
or political region does not necessarily imply widespread occurrence in 
that region, but only indicates that a literature citation or other basis
exists for that report.  When available more specificity in GRIN
distributional reports is given as comments, but the available information
may vary greatly from one taxon or region to another.  Among regions, the
greatest gaps in information exist mainly for tropical regions.</p>

<p align=JUSTIFY>For species with subspecies or varieties in GRIN, the
main entry for the species provides the overall distribution, including
distributions for any subspecies or varieties not appearing in GRIN.
Autonym entries provide distributions of only the typical subspecies or
variety which occupies all or only a portion of the total distribution for
the species.</p>
<br />
<asp:Button ID="btnDistribPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnDistribPre_Click" /><asp:Button ID="btnDistribCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnDistribCont_Click" />
    <asp:Button ID="btnDistribNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnDistribNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlLiter" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>LITERATURE REFERENCES</b>
<p align=JUSTIFY>For ease of computerization, <font size=1><b><%=litcnt%></b></font> literature
abbreviations have thus far been developed in GRIN for standard
references, floras, and serial publications commonly seen in the database. 
These are only cursorily displayed to public users of GRIN, although for
brevity they have been used in publications such as Technical Bulletins
1757 and 1796.  They are employed for the <font size=1><b><%=tcitcnt%></b></font> literature citations in
GRIN which link to <font size=1><b><%=actcitcnt%></b></font> accepted and <font size=1><b><%=syntcitcnt%></b></font> synonym species or
infraspecies names. An additional <font size=1><b><%=gcitcnt%></b></font> references exist in GRIN for
genera, these mainly documenting recent taxonomic revisions or monographs
of all or part of a genus or recent phylogenetic studies. Though the
number of references presented for a given taxon may be extensive, the
listings should not be considered exhaustive.  If all reported information
(taxonomy, nomenclature, distribution, etc.) is documented in a few
references, these might be the only ones cited.  Other references may
treat the taxon, but add no new information, so these may not be entered
in GRIN.  This is particularly true for genera with recent comprehensive
monographic treatments that are the source of most GRIN taxonomic data for
those genera.  Other references may be included only to document
alternative taxonomic treatments, orthographies, or authorship for a name. 
Generally these alternatives will be indicated with comments following the
reference citation.  The absence of a comment can usually be taken to
imply correspondence in treatment between 
GRIN T<font size=1>AXONOMY</FONT> and the particular reference.</p>
<br />
<asp:Button ID="btnLiterPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnLiterPre_Click" /><asp:Button ID="btnLiterCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnLiterCont_Click" />
    <asp:Button ID="btnLiterNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnLiterNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlSpec" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>SPECIAL-PURPOSE DATA SETS</b>
<p align=JUSTIFY>A number of specialized data sets are incorporated into 
GRIN T<font size=1>AXONOMY</font>, most of these arising from
publications of National Germplasm Resources Laboratory
(formerly Systematic Botany and Mycology Laboratory) botanists. One
example is the <a href="famgensearch.aspx";
title="Search GRIN Taxonomy Family/Genus Data" target="blank"
>family and generic</a>
data in USDA Technical Bulletin 1796 which was already discussed.  Also
included are the scientific names endorsed by 
<a href="http://www.ars-grin.gov/cgi-bin/npgs/html/taxassoc.pl?language=en";
title="Search Seed Association Names in GRIN" target="blank"
>seed-testing associations</a> such as Association of Official Seed 
Analysts (AOSA) and
International Seed Testing Association (ISTA) from the publications AOSA
Contribution No. 25 to the Handbook on Seed Testing, <i>Uniform
classification of weed and crop seeds</i> (Meyer and Wiersema, 2006) and
<i>ISTA List of Stabilized Plant Names</i> (ed. 4, Wiersema et al., 2001),
for which the nomenclature is being verified in GRIN. The AOSA data set
includes the 
<a href="http://www.aphis.usda.gov/ppq/weeds/"
onclick="javascript:site('www.aphis.usda.gov/ppq/weeds/');return false;"
title="Link to Federal Noxious Weed List" target="blank">federal noxious weeds</a>
controlled by the USDA Animal and Plant Health Inspection Service (APHIS)
and the <a href="http://www.ams.usda.gov/lsg/seed/seed_pub.htm"
onclick="javascript:site('www.ams.usda.gov/lsg/seed/seed_pub.htm');return false;"
title="Link to List of Seeds regulated by Federal Seed Act" target="blank">state
noxious-weed seeds</a> regulated by the Federal Seed Act.  A separate
<a href="http://www.ars-grin.gov/cgi-bin/npgs/html/taxweed.pl?language=en";
 title="Query GRIN Taxonomy for Noxious Weed Data" target="blank"
>query page</a> has been set up to search all federal and state noxious
weeds, both aquatic and terrestrial, and state noxious-weed seeds in
GRIN with links to federal and state regulatory resources.</p>

<p>A third publication linked to GRIN T<font size=1>AXONOMY</font> is the
new revision of former USDA Agricultural Handbook 505, <i>A checklist of
names for 3,000 vascular plants of economic importance</i> (Terrell,
1986b). This new revision, which treats over 9,500 economically important
vascular plants, was published in 1999 by 
<a
href="http://www.crcpress.com:80/us/product.asp?sku=2119+++&dept%5Fid=1"
onclick="javascript:site('www.crcpress.com:80/us/product.asp?sku=2119+++&dept%5Fid=1');return false;"
title="Link to CRC Press" target="blank">CRC
Press</a> under the title <i>World Economic Plants: a standard 
reference.</i>  Data from this publication may be 
<a href="http://www.ars-grin.gov/cgi-bin/npgs/html/taxecon.pl?language=en";
title="Query GRIN Taxonomy for Economic Plant Data" target="blank"
>queried</a> on the web.</p>

<p>Another data set incorporated into GRIN relates to threatened and
endangered plants.  Among these are the plants listed in Appendices I, II,  
and III of the Convention on International Trade in Endangered Species of
Wild Fauna and Flora <a href="http://www.cites.org/"
onclick="javascript:site('www.cites.org');return false;"
title="Link to CITES" target="blank">(CITES)</a>.  Also included are the federal 
<a href="http://endangered.fws.gov/wildlife.html?&code=V"
onclick="javascript:site('endangered.fws.gov/wildlife.html?&code=V');return false;"
title="Link to US-FWS List of T & E Plants" target="blank">list</a> of
threatened and endangered plants maintained by the United States Fish and
Wildlife Service <a href="http://www.fws.gov"
onclick="havascript:site('www.fws.gov');return false;"
title="Link to US-FWS" target="blank">(US-FWS)</a>, Department of the Interior and the
list of rare plants maintained by the Center for Plant Conservation 
<a href="http://www.mobot.org/CPC/welcome.html"
onclick="javascript:site('www.mobot.org/CPC/welcome.html');return false;"
title="Link to Center for Plant Conservation" target="blank">(CPC)</a>.</p>

<p>A final specialized data set in GRIN provides information on
all published rhizobial nodulation reports for genera and species.  These
data, concerning mainly legumes, can also be 
<a href="http://www.ars-grin.gov/~sbmljw/cgi-bin/taxnodul.pl";
title="Query GRIN Taxonomy for Rhizobial Nodulation Data" target="blank"
>queried</a> on the web.</p>

<br />
<asp:Button ID="btnSpecPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnSpecPre_Click" /><asp:Button ID="btnSpecCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnSpecCont_Click" />
    <asp:Button ID="btnSpecNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnSpecNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlBasis" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>BASIS OF GRIN TAXONOMIC DECISIONS</b>
<p align=JUSTIFY>National Germplasm Resources Laboratory 
<a
href="http://www.ars.usda.gov/main/site_main.htm?modecode=12-75-15-00"
onclick="javascript:site('www.ars.usda.gov/main/site_main.htm?modecode=12-75-15-00');return false;"
title="Link to National Germplasm Resources Lab" target="blank">(NGRL)</a> botanists
are responsible for maintaining the taxonomic and nomenclatural integrity
of the scientific names in GRIN.  NGRL maintains an active collection of 
monographic and floristic literature from throughout the world to assist our 
activities. Through ongoing research into all current taxonomic literature, 
consultations with taxonomic botanists, and systematic reviews of GRIN scientific 
names for various plant families, the most recent taxonomy and nomenclature are 
incorporated into GRIN.  For major crop genera, GRIN taxonomic work may often 
involve interaction with other USDA scientists for those crops and their Crop 
Germplasm Committees (CGC).</p>

<p align=JUSTIFY>The taxonomic and nomenclatural decisions accepted in GRIN are
based on various considerations.  GRIN family taxonomy is based, with a few 
more recent exceptions, on the APG-3 classification (Bremer et al., 2009). 
Taxonomic decisions at lesser ranks ideally reflect the views
of recognized taxonomic specialists for various plant groups as determined
from published literature, such as monographs, revisions, or contributed
treatments to floras, or from direct consultation for review of GRIN
taxonomic information.  Evidence from molecular phylogenetic studies, which is 
particularly relevant to decisions regarding generic taxonomy but seldom impacts 
species-level decisions, is also taken into account.  Evaluating any proposed 
changes from such studies in relation to existing GRIN generic taxonomy, while 
nonetheless challenging, is guided by an assessment of the range of evidence 
presented, including the completeness of sampling, and the extent to which 
recognized specialists are participants in the underlying research or have embraced 
its conclusions. When a specialist opinion or specialist-generated literature is 
lacking, taxonomic decisions, particularly at species level, are based on the 
floristic literature.  Floras are generally assigned greater weight than checklists, 
and modern floras are given greater consideration than older ones in preparing the 
GRIN treatment.</p>

<p align=JUSTIFY>Other considerations being equal, when there are differences in 
taxonomic treatment or nomenclatural disputes, the GRIN treatment would generally be 
guided by current usage, with some evaluation of the impact of a change to our 
users and to the internal consistency of our treatment.  In serving the agricultural 
scientists of NPGS, it is especially necessary to consider usage among agronomists 
and horticulturalists in addition to that of taxonomists.  A requirement, however, 
is that all nomenclature adhere to the rules of the <i>International Code of 
Botanical Nomenclature</i> (McNeill et al., 2006).</p>

<p align=JUSTIFY>Nomenclatural problems or discrepancies which appear and
are unresolved in the literature often require that original references be
consulted.  The <a
href="http://www.ars.usda.gov/AboutUs/AboutUs.htm?modecode=12-75-15-00"
onclick="javascript:site('www.ars.usda.gov/AboutUs/AboutUs.htm?modecode=12-75-15-00');return false;"
title="Link to Directions for Visitors to NGRL" target="blank">location</a> 
of the NGRL on the Beltsville Agricultural Research Center, about 24 km northeast of 
Washington, D.C., also facilitates this work by providing access to several 
excellent libraries for historical and current botanical literature, including the 
National Agricultural Library (NAL), Library of Congress (LC), Smithsonian 
Institution (SI), and University of Maryland.  The wealth of on-line botanical 
resources has now become indispensable for this purpose, especially those resources 
made available through the <a href="http://www.biodiversitylibrary.org/"
onclick="javascript:site('www.biodiversitylibrary.org/');return false;"
title="Link to Biodiversity Heritage Library" target="blank">Biodiversity Heritage Library</a>, 
the <a href="http://bibdigital.rjb.csic.es/ing/index.php"
onclick="javascript:site('bibdigital.rjb.csic.es/ing/index.php');return false;"
title="Link to Digital Library del Real Jard&iacute;n Bot&aacute;nico" target="blank">Digital 
Library del Real Jard&iacute;n Bot&aacute;nico</a>, 
the <a href="http://gallica.bnf.fr"
onclick="javascript:site('gallica.bnf.fr');return false;"
title="Link to Gallica Digital Library" target="blank">Gallica Digital Library</a>,
and <a href="http://books.google.com/books"
onclick="javascript:site('books.google.com/books');return false;"
title="Link to Google Books" target="blank">Google Books</a>.</p>
<br />
<asp:Button ID="btnBasisPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnBasisPre_Click" /><asp:Button ID="btnBasisCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnBasisCont_Click" />
    <asp:Button ID="btnBasisNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnBasisNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlConcl" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>CONCLUDING REMARKS</b>
<p align=JUSTIFY>The Agricultural Research Service invites and encourages
those interested to access and utilize GRIN data over the Internet.
Errors or discrepancies in the taxonomic data that are uncovered should be
reported to <a
href="../contact.aspx";
 title="Send Message to NGRL" target="blank">NGRL</a> to ensure their
correction.  We would like to cooperate with other individuals and
organizations active in the taxonomy of cultivated plants to further our
common interests in arriving at a more stable, scientifically accurate,
nomenclature.  We hope these efforts can lead to the development of an
internationally recognized standard reference for scientific names of
cultivated plants.</p>
<br />
<asp:Button ID="btnConclPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnConclPre_Click" /><asp:Button ID="btnConclCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnConclCont_Click" />
    <asp:Button ID="btnConclNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnConclNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlAcknowl" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>ACKNOWLEDGEMENTS</b>
<p align=JUSTIFY>The GRIN taxonomists are especially grateful for the ongoing 
support and technical expertise of the USDA-ARS National Germplasm Resources
Laboratory, GRIN Database Management Unit, in particular the late Edward M. Bird, 
Jimmie D. Mowder, Quinn P. Sinnott, John A. Belt, Gorm P. Emberland, John Chung, 
Mark A. Bohning, and Allan K. Stoner.  Our ongoing dialog with many of the National 
Plant Germplasm System crop curators and their liason with the Crop Germplasm 
Committees has been very useful to us.  In addition to the author, several
individuals, over the years, have directly contributed in various ways to GRIN 
taxonomic data, including Steven R. Hill, Blanca Le&oacute;n, William E. Rice, 
Edward E. Terrell, Carole A. Ritchie, Tufail Ahmed, Vickie M. Binstock, James I.
Cohen, Sasha N. Irvin, Peter C. Garvey, Michael Jeffe, and Matthew Smith. In the
former USDA-ARS Systematic Botany and Mycology Laboratory, the collaboration and 
cooperation of fellow botanist Joseph H. Kirkbride, Jr. (now of the U.S. National 
Arboretum) has always been appreciated and the adminstrative support of Amy Y.
Rossman and technical assistance of David F. Farr and Erin B. McCray have been 
invaluable.</p>
	
<p align=JUSTIFY>Development of the web interface to GRIN taxonomy was initiated by 
the late Edward M. Bird and Vickie M. Binstock, and has progressed through work by 
the author, with the technical assistance of James S. Plaskowitz, Quinn P. Sinnott, 
and David F. Farr and the design work of James S. Plaskowitz.  Translations of 
several web pages have been possible due to the efforts of Christian Feuillet 
(French), Courtney V. Conrad (German), Jos&eacute; R. Hern&aacute;ndez (Spanish), 
and Joseph H. Kirkbride, Jr. and Blanca Le&oacute;n (Portuguese and Spanish). We 
are grateful for all these contributions.</p>

<p align=JUSTIFY>Finally, it is impossible to acknowledge here all of the numerous 
individuals whose valuable communications have greatly enriched GRIN taxonomy. 
Nevertheless, a number of regular correspondents have greatly assisted us in 
improving the quality and accuracy of GRIN taxonomy data by routinely informing us 
of errors in or necessary additions to GRIN data, directing our attention to items 
requiring further documentation, and/or providing feedback on GRIN taxonomy web 
pages. Among these are Folmer Arnklit (Botanic Garden, University of Copenhagen), 
Franklin S. Axelrod (University of Puerto Rico), Ken Becker (CAB International), 
James A. Duke (GreenPharmacy.com), Kanchi N. Gandhi (IPNI, Harvard University 
Herbaria), John R. Hosking (DPI, New South Wales, Australia), Kirsten A. Llamas 
(Tropical Flowering Tree Society), James L. Reveal (Bailey Hortorium, Cornell 
University), Mark W. Skinner (USDA-NRCS), and Thomas L. Wendt (University 
of Texas at Austin). We are equally grateful to those 
individuals who have been frequent consultants for complex nomenclatural questions, 
including Kanchi N. Gandhi (IPNI, Harvard University Herbaria), Werner Greuter 
(Botanischer Garten und Botanisches Museum Berlin-Dahlem), Joseph H. Kirkbride, 
Jr. (U.S. National Arboretum), John McNeill (Royal Botanic Gardens, Edinburgh), and 
Dan H. Nicolson (Smithsonian Institution, Washington, D.C.).</p>
<br />
<asp:Button ID="btnAcknowlPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnAcknowlPre_Click" /><asp:Button ID="btnAcknowlCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnAcknowlCont_Click" />
    <asp:Button ID="btnAcknowlNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnAcknowlNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlRefer" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>REFERENCES CITED</b>
<p align=JUSTIFY><b>Brandenburg, W. A. et al. (editors), 1988.</b>  ISTA
list of stabilized plant names.  International Seed Testing Association,
Zurich, Switzerland.</p>

<p align=JUSTIFY><b>Bremer, B. et al. 2009.</b> An update of the Angiosperm 
Phylogeny Group classification for the orders and families of flowering plants: APG 
III. Bot. J. Linn. Soc. 161:105&#150;121.</p>

<p align=JUSTIFY><b>Bridson, G.D.R., and Smith, E.R. (editors), 1991.</b>
B-P-H/S. Botanico-Periodicum-Huntianum/Supplementum.  Hunt Institute for 
Botanical Documentation, Pittsburgh, Pennsylvania.</p>

<p align=JUSTIFY><b>Bridson, G.D.R. et al. (editors), 2004.</b>
BPH-2: periodicals with botanical content.  Hunt Institute for 
Botanical Documentation, Pittsburgh, Pennsylvania.</p>

<p align=JUSTIFY><b>Brummitt, R.K, and Powell, C.E., 1992.</b>  Authors of
plant names. A list of authors of scientific names of plants, with
recommended standard forms of their names, including abbreviations.  Royal
Botanic Gardens, Kew, England.</p>

<p align=JUSTIFY><b>Cook, F.E.M., 1995.</b> Economic botany data
collection standard. Royal Botanic Gardens, Kew.</p>

<p align=JUSTIFY><b>Dorr, L.J., and Nicolson, D.H., 2008&#150;2009.</b>
Taxonomic literature, supplements VII-VIII.  2 volumes.  A.R.G. Gantner Verlag K.G., 
Ruggell.</p>

<p align=JUSTIFY><b>Gove, P.B. et al. (editors), 1961.</b> Webster's third
new international dictionary of the English language unabridged.  G. & C.
Merriam Company, Springfield, Massachusetts.</p>

<p align=JUSTIFY><b>McNeill, J., et al. (editors), 2006.</b>
International code of botanical nomenclature (Vienna Code).  Reg. Veg.
146:1&#150;568.</p>

<p align=JUSTIFY><b>Gunn, C.R., Wiersema, J.H., Ritchie, C.A., and
Kirkbride, J.H., Jr., 1992.</b>  Families and genera of spermatophytes
recognized by the Agricultural Research Service.  U.S.D.A. Tech. Bull.
1796:1&#150;500.</p>

<p align=JUSTIFY><b>Hollis, S. and Brummitt, R.K., 1992</b>. World
geographical scheme for recording plant distributions. Hunt Institute for
Botanical Documentation, Pittsburgh.</p>

<p align=JUSTIFY><b>Janick, J. (editor), 1989.</b> The National Plant
Germplasm System of the United States.  Plant Breed. Rev.
7:1&#150;230.</p>

<p align=JUSTIFY><b>Kartesz, J.T. and Thieret, J.W., 1991.</b>  Common
names for vascular plants: guidelines for use and application.  Sida
14:421&#150;434.</p>

<p align=JUSTIFY><b>Lawrence, G.H.M., Buchheim, A.F.G., Daniels, G.S., and
Dolezal, H. (editors), 1968.</b>  B-P-H.  Botanico-Periodicum-Huntianum.
Hunt Botanical Library, Pittsburgh, Pennsylvania.</p>

<p align=JUSTIFY><b>Meyer, D.L. and Wiersema, J.H. (editors), 1999. </b>
Uniform classification of weed and crop seeds.  Contribution No. 25 to the
Handbook on Seed Testing.  Association of Official Seed Analysts.</p>

<p align=JUSTIFY><b>Stafleu, F.A., and Cowan, R.S., 1976&#150;1988.</b>
Taxonomic literature, second edition.  7 volumes.  Bohn, Scheltema, and Holkema,
Utrecht.</p>

<p align=JUSTIFY><b>Stafleu, F.A., and Mennega, E.A., 1992&#150;2000.</b>
Taxonomic literature, supplements I-IV.  4 volumes.  Koeltz Scientific
Books, K&ouml;nigstein.</p>

<p align=JUSTIFY><b>Terrell, E.E., 1986a.</b>  Updating scientific names
for introduced germplasm of economically important vascular plants.  Acta
Hort., Int. Soc. Hort. Sci. 182:293&#150;300.</p>

<p align=JUSTIFY><b>Terrell, E.E., 1986b.</b>  A checklist of names for
3,000 vascular plants of economic importance.  U.S.D.A. Agric. Handb.
505:1&#150;241.</p>

<p align=JUSTIFY><b>Brickell, C. D. et al. (editors), 2009.</b>  International
code of nomenclature for cultivated plants, ed. 8.  Scripta Hort. 10:1&#150;184.</p>

<p align=JUSTIFY><b>Wiersema, J.H. and Le&oacute;n, B., 1999.</b>  World
economic plants: a standard reference.  CRC Press, Boca Raton,
Florida.</p>

<p align=JUSTIFY><b>Wiersema, J.H., Gunn, C.R., and Kirkbride, J.H., Jr.,
1990.</b> Legume (Fabaceae) nomenclature in the USDA germplasm system.
U.S.D.A. Tech. Bull. 1757:1&#150;572.</p>
<br />
<asp:Button ID="btnReferPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnReferPre_Click" /><asp:Button ID="btnReferCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnReferCont_Click" />
    <asp:Button ID="btnReferNext" runat="server" 
        Text="Next" CssClass="rightButton" onclick="btnReferNext_Click" />
<br /><br /><hr />
</asp:Panel>
<asp:Panel ID="pnlSymb" runat="server" Visible="False" BorderColor="White" 
        BorderWidth="20px" Width="940px">
<center><h3><b>TAXONOMIC INFORMATION IN GRIN</b></h3></center>
	<hr />
	<b>SYMBOLS AND ABBREVIATIONS IN GRIN TAXONOMY</b>
<br />
<p>(See also the <b><a 
href="http://mansfeld.ipk-gatersleben.de/taxcat2/default.htm"
onclick="javascript:site('mansfeld.ipk-gatersleben.de/taxcat2/default.htm');return false;"
title="Link to Mansfeld Database of Taxonomic Categories"  target="blank"><i>Database
of Botanical Taxonomic Categories</i></a></b> on the Mansfeld server of
IPK Gatersleben, Germany for further information on various taxonomic
ranks.)</p>


<p align=JUSTIFY><b>&#215;</b> denotes a cross between two species (e.g.,
<i>Sorghum bicolor</i> &#215; <i>Sorghum halepense</i>) or part of the
binomial for such a hybrid (e.g., <i>S.</i> &#215;<i>almum</i>), or
precedes an intergeneric hybrid (e.g., &#215;<i>Triticosecale</i>).</p>

<p align=JUSTIFY><b>+</b> denotes a graft-chimera, an individual composed
of two or more genetically different tissues united by grafting
(e.g., +<i>Laburnocytisus</i>) as treated under Article 4 of the
<a href="http://www.actahort.org/books/647/"
onclick="javascript:site('www.actahort.org/books/647/');return false;"
title="Link to on-line edition of ICNCP" target="blank"><i>International Code of
Nomenclature for Cultivated Plants (ICNCP)</i></a> (Brickell et al.,
2004).</p>

<p align=JUSTIFY><b>&lsquo;...&rsquo;</b> single quotation marks
surrounding a name at the rank of cultivar, a taxonomic rank applied to
cultivated plants under Article 2 of the
<a href="http://www.actahort.org/books/647/"
onclick="javascript:site('www.actahort.org/books/647/');return false;"
title="Link to on-line edition of ICNCP" target="blank"><i>International Code of
Nomenclature for Cultivated Plants (ICNCP)</i></a> (Brickell et al.,
2004).</p>

<p align=JUSTIFY><b>=</b>  follows synonyms and precedes their accepted
names; also precedes hybrid formula of hybrids, alternative accepted
cultivar names, or other alternative accepted names in literature
citations.</p>

<p align=JUSTIFY><b>=~</b>  precedes probable generic synonyms that
are treated as synonyms in GRIN but may be accepted elsewhere.</p>

<p align=JUSTIFY><b>~</b>  precedes possible generic synonyms that
are accepted in GRIN but treated as synonyms elsewhere.</p>

<p align=JUSTIFY><b>&#8801;</b>  indicates homotypic synonymy, i.e. based
on the same type as the accepted name, as per a basionym.</p>

<p align=JUSTIFY><b>aggr.</b>  aggregate, an informal grouping of related
species.</p>

<p align=JUSTIFY><b>Amer.</b>  American.</p>

<p align=JUSTIFY><b>anon.</b>  anonymous, indicating that the author
of a publication is unknown.</p>

<p align=JUSTIFY><b>auct.</b>  <i>auctorum</i> (Latin): of authors. Used
to represent a common incorrect usage of a name that has been widely used
for a different taxon than the one intended by the original author.</p>

<p align=JUSTIFY><b>auct. mult.</b>  <i>auctorum multorum</i> (Latin): of
many authors. Used to represent a common incorrect usage of a name that
has been widely used for a different taxon than the one intended by
the original author.</p>

<p align=JUSTIFY><b>auct. nonn.</b>  <i>auctorum nonnullorum</i> (Latin):
of some authors. Used to represent an occasional incorrect usage of a name
that has been sometimes used for a different taxon than the one intended
by the original author.</p>

<p align=JUSTIFY><b>auct. pl.</b>  <i>auctorum plurimorum</i> (Latin): of
most authors. Used to represent the most common incorrect usage of a name
that has been widely used for a different taxon than the one intended by
the original author.</p>

<p align=JUSTIFY><b>c.</b>  central.</p>

<p align=JUSTIFY><b>cult.</b>  cultivated, cultivation.</p>

<p align=JUSTIFY><b>cum</b> (Latin): with, together with.</p>

<p align=JUSTIFY><b>cv.</b>  cultivar, a taxonomic rank applied to
cultivated plants under the 
<a href="http://www.actahort.org/books/647/"
onclick="javascript:site('www.actahort.org/books/647/');return false;"
title="Link to on-line edition of ICNCP" target="blank"><i>International Code of
Nomenclature for Cultivated Plants (ICNCP)</i></a> (Brickell et al.,
2004).</p>

<p align=JUSTIFY><b>e.</b>  east,  <b>e.-c.</b>  east-central.</p>

<p align=JUSTIFY><b>Eur.</b>  European.</p>

<p align=JUSTIFY><b>f.</b> forma, one of the lowest taxonomic ranks,
below subspecies and variety; or, when following an author, <i>filius</i>
(Latin): son (e.g., L. f.: son of Linnaeus).</p>

<p align=JUSTIFY><b>fide</b> (Latin): according to.</p>

<p align=JUSTIFY><b>hort.</b>  <i>hortulanorum</i> (Latin): of gardeners,
signifying that the name was first used in gardens and was later published
without the name of its originator, or used here to represent a common 
incorrect usage of a name in horticulture for a different taxon than the 
one intended by the original author.</p>

<p align=JUSTIFY><b>hort. nonn.</b>  <i>hortulanorum nonnullorum</i>
(Latin): of some gardeners.</p>

<p align=JUSTIFY><b>hybr.</b> catch-all designation used in GRIN to
accommodate germplasm of hybrid parentage within a given genus for which
no hybrid binomial exists.</p>

<p align=JUSTIFY><b>in adnot.</b>  <i>in adnotatione</i> (Latin):
in annotation, in a note.</p>

<p align=JUSTIFY><b>ined.</b>  <i>ineditus</i> (Latin): unpublished.</p>

<p align=JUSTIFY><b>introd.</b>  introduced.</p>

<p align=JUSTIFY><b>n.</b>  north, <b>n.-c.</b>  north-central,
<b>n.e.</b> northeast, <b>n.w.</b>  northwest.</p>

<p align=JUSTIFY><b>natzd.</b> naturalized.</p>

<p align=JUSTIFY><b>nom. ambig.</b>  <i>nomen ambiguum </i>(Latin):
ambiguous name used in different senses which has become a long-persistent
source of error.</p>

<p align=JUSTIFY><b>nom. confus.</b>  <i>nomen confusum </i>(Latin):
confused name for which the type and/or application cannot be determined
and which therefore is no longer used.</p>

<p align=JUSTIFY><b>nom. cons.</b>  <i>nomen conservandum </i>(Latin):
name conserved under Article 14 of the <a
href="http://www.ibot.sav.sk/icbn/main.htm"
onclick="javascript:site('www.ibot.sav.sk/icbn/main.htm');return false;"
title="Link to on-line edition of ICBN" target="blank"><i>International Code of
Botanical Nomenclature (ICBN)</i></a> (McNeill et al., 2006).</p>

<p align=JUSTIFY><b>nom. cons. prop.</b>  <i>nomen conservandum
propositum</i> (Latin): name proposed to the General Committee for
conservation under Article 14 of the <a
href="http://ibot.sav.sk/icbn/main.htm"
onclick="javascript:site('ibot.sav.sk/icbn/main.htm');return false;"
title="Link to on-line edition of the ICBN" target="blank"><i>International Code of
Botanical Nomenclature (ICBN)</i></a> (McNeill et al., 2006).</p>

<p align=JUSTIFY><b>nom. dub.</b> <i>nomen dubium</i> (Latin): dubious
name, i.e., application of name uncertain.</p>

<p align=JUSTIFY><b>nom. illeg.</b>  <i>nomen illegitimum</i> (Latin):
illegitimate name according to Article 14 or 52 of the <a 
href="http://www.ibot.sav.sk/icbn/main.htm"
onclick="javascript:site('www.ibot.sav.sk/icbn/main.htm');return false;"
title="Link to on-line edition of ICBN" target="blank"><i>International Code of
Botanical Nomenclature (ICBN)</i></a> (McNeill et al., 2006).</p>

<p align=JUSTIFY><b>nom. inval.</b>  <i>nomen invalidum, nomen non rite
publicatum</i> (Latin): name not validly published according to Article
32 of the <a href="http://www.ibot.sav.sk/icbn/main.htm"
onclick="javascript:site('www.ibot.sav.sk/icbn/main.htm');return false;"
title="Link to on-line edition of ICBN" target="blank"><i>International Code of
Botanical Nomenclature (ICBN)</i></a> (McNeill et al., 2006).</p>

<p align=JUSTIFY><b>nom. nov.</b>  <i>nomen novum</i> (Latin): replacement
name for an older name typified by the type of the older name according to 
Article 7.3 of the <a href="http://www.ibot.sav.sk/icbn/main.htm"
onclick="javascript:site('www.ibot.sav.sk/icbn/main.htm');return false;"
title="Link to on-line edition of ICBN" target="blank"><i>International Code of
Botanical Nomenclature (ICBN)</i></a> (McNeill et al., 2006).</p>

<p align=JUSTIFY><b>nom. nud.</b>  <i>nomen nudum</i> (Latin): name
published without a description or reference to a published description
or diagnosis as required under Article 32 of the <a
href="http://www.ibot.sav.sk/icbn/main.htm"
onclick="javascript:site('www.ibot.sav.sk/icbn/main.htm');return false;"
title="Link to on-line edition of ICBN" target="blank"><i>International Code of
Botanical Nomenclature (ICBN)</i></a> (McNeill et al., 2006).</p>

<p align=JUSTIFY><b>nom. rej.</b>  <i>nomen rejiciendum</i> (Latin): name
rejected under Article 14 or 56 of the <a
href="http://www.ibot.sav.sk/icbn/main.htm"
onclick="javascript:site('www.ibot.sav.sk/icbn/main.htm');return false;"
title="Link to on-line edition of ICBN" target="blank"><i>International Code of
Botanical Nomenclature (ICBN)</i></a> (McNeill et al., 2006) that
cannot be used.</p>

<p align=JUSTIFY><b>nom. superfl.</b>  <i>nomen superfluum</i> (Latin):
an illegitimate name that was superfluous when published according to
Article 52 of the <a href="http://www.ibot.sav.sk/icbn/main.htm"
onclick="javascript:site('www.ibot.sav.sk/icbn/main.htm');return false;"
title="Link to on-line edition of ICBN" target="blank"><i>International Code of
Botanical Nomenclature (ICBN)</i></a> (McNeill et al., 2006).</p>

<p align=JUSTIFY><b>notho-</b> (subsp. or var.) prefix to the rank of a
hybrid taxon below the rank of species.</p>

<p align=JUSTIFY><b>orth. rej.</b> rejected orthographic variant under 
Article 14.11 of the <a href="http://www.ibot.sav.sk/icbn/main.htm"
onclick="javascript:site('www.ibot.sav.sk/icbn/main.htm');return false;"
title="Link to on-line edition of ICBN" target="blank"><i>International Code of
Botanical Nomenclature (ICBN)</i></a> (McNeill et al., 2006).</p>

<p align=JUSTIFY><b>orth. var.</b> orthographic variant, i.e., an
incorrect alternate spelling of a name according to Article 61 of the <a
href="http://www.ibot.sav.sk/icbn/main.htm"
onclick="javascript:site('www.ibot.sav.sk/icbn/main.htm');return false;"
title="Link to on-line edition of ICBN" target="blank"><i>International Code of
Botanical Nomenclature (ICBN)</i></a> (McNeill et al., 2006).</p>

<p align=JUSTIFY><b>p.p.</b> <i>pro parte</i> (Latin): in part.</p>

<p align=JUSTIFY><b>pro hyb.</b> <i>pro hybrida</i> (Latin): as a
hybrid.</p>

<p align=JUSTIFY><b>prol.</b>  proles, a taxonomic rank formerly applied
to cultivated plants and basically equivalent to the current
cultivar-group.</p>

<p align=JUSTIFY><b>prop.</b>  <i>propositus</i> (Latin): proposed.</p>

<p align=JUSTIFY><b>pro parte</b>  (Latin): in part.</p>

<p align=JUSTIFY><b>pro parte majore</b>  (Latin): for the greater
part.</p>

<p align=JUSTIFY><b>pro parte minore</b>  (Latin): for a small part.</p>

<p align=JUSTIFY><b>pro sp.</b> <i>pro specie</i> (Latin): as a
species.</p>

<p align=JUSTIFY><b>pro subsp.</b> <i>pro subspecie</i> (Latin): as a
subspecies.</p>

<p align=JUSTIFY><b>pro syn.</b> <i>pro synonymo</i> (Latin): as a
synonym.</p>

<p align=JUSTIFY><b>s.</b> south, <b>s.-c.</b>  south-central, <b>s.e.</b>
southeast, <b>s.w.</b>  southwest.</p>

<p align=JUSTIFY><b>sensu</b> (Latin): in the sense or opinion of.</p>

<p align=JUSTIFY><b>sensu lato</b> (Latin): in a broad sense.</p>

<p align=JUSTIFY><b>sensu stricto</b> (Latin): in a narrow sense.</p>

<p align=JUSTIFY><b>spp.</b> catch-all designation used in GRIN to
accommodate germplasm of an unidentified or unnamed species in a given
genus.</p>

<p align=JUSTIFY><b>subsp.</b>  subspecies, a lower taxonomic rank than
species.</p>

<p align=JUSTIFY><b>typo excl.</b>  <i>typo excluso</i> (Latin): with 
the type excluded.</p>

<p align=JUSTIFY><b>typo incl.</b>  <i>typo incluso</i> (Latin): with 
the type included.</p>

<p align=JUSTIFY><b>var.</b>  variety, a taxonomic rank below subspecies
and above forma.</p>

<p align=JUSTIFY><b>w.</b>  west, <b>w.-c.</b>  west-central.</p>
<br />
<asp:Button ID="btnSymbPre" runat="server" Text="Previous" CssClass="leftButton" 
        onclick="btnSymbPre_Click" /><asp:Button ID="btnSymbCont"
        runat="server" Text="Contents" CssClass="middleButton" 
        onclick="btnSymbCont_Click" />
<br /><br /><hr />
</asp:Panel>
</asp:Content>