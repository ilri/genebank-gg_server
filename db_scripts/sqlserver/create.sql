BEGIN;
CREATE TABLE [main.site] (
    [id] int NOT NULL PRIMARY KEY,
    [field1] nvarchar(45) NOT NULL
)
;
CREATE TABLE [prod.test1] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [site] int NULL REFERENCES [main.site] ([id])
)
;
CREATE TABLE [prod.fam] (
    [famno] int NOT NULL PRIMARY KEY,
    [validfamno] int NULL,
    [family] nvarchar(25) NOT NULL UNIQUE,
    [famauthor] nvarchar(100) NOT NULL UNIQUE,
    [altfamily] nvarchar(25) NOT NULL,
    [subfamily] nvarchar(25) NOT NULL UNIQUE,
    [tribe] nvarchar(25) NOT NULL UNIQUE,
    [subtribe] nvarchar(25) NOT NULL UNIQUE,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
ALTER TABLE [prod.fam] ADD CONSTRAINT validfamno_refs_famno_54ee6c7d FOREIGN KEY ([validfamno]) REFERENCES [prod.fam] ([famno]);
CREATE TABLE [prod.crop] (
    [cropno] int NOT NULL PRIMARY KEY,
    [crop] nvarchar(20) NOT NULL UNIQUE,
    [cmt] nvarchar(2000) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.gn] (
    [gno] int NOT NULL PRIMARY KEY,
    [validgno] int NULL,
    [qual] nvarchar(2) NOT NULL,
    [ghybrid] nvarchar(1) NOT NULL,
    [genus] nvarchar(30) NOT NULL UNIQUE,
    [gauthor] nvarchar(100) NOT NULL UNIQUE,
    [subgenus] nvarchar(30) NOT NULL UNIQUE,
    [section] nvarchar(30) NOT NULL UNIQUE,
    [series] nvarchar(30) NOT NULL UNIQUE,
    [subseries] nvarchar(30) NOT NULL UNIQUE,
    [famno] int NULL REFERENCES [prod.fam] ([famno]),
    [othfamily] nvarchar(100) NOT NULL,
    [cname] nvarchar(30) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [subsection] nvarchar(30) NOT NULL UNIQUE
)
;
ALTER TABLE [prod.gn] ADD CONSTRAINT validgno_refs_gno_5afc285 FOREIGN KEY ([validgno]) REFERENCES [prod.gn] ([gno]);
CREATE TABLE [prod.reg] (
    [regno] int NOT NULL PRIMARY KEY,
    [area] nvarchar(20) NOT NULL UNIQUE,
    [region] nvarchar(30) NOT NULL UNIQUE,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.geo] (
    [geono] int NOT NULL PRIMARY KEY,
    [validgeono] int NULL,
    [country] nvarchar(30) NOT NULL UNIQUE,
    [state] nvarchar(20) NOT NULL UNIQUE,
    [isofull] nvarchar(100) NOT NULL,
    [isoshort] nvarchar(60) NOT NULL,
    [statefull] nvarchar(60) NOT NULL,
    [iso3] nvarchar(3) NOT NULL,
    [iso2] nvarchar(2) NOT NULL,
    [st] nvarchar(3) NOT NULL,
    [cflag] nvarchar(1) NOT NULL,
    [lath] nvarchar(1) NOT NULL,
    [lonh] nvarchar(1) NOT NULL,
    [regno] int NULL REFERENCES [prod.reg] ([regno]),
    [changed] datetime NULL,
    [oldname] nvarchar(100) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
ALTER TABLE [prod.geo] ADD CONSTRAINT validgeono_refs_geono_447defb9 FOREIGN KEY ([validgeono]) REFERENCES [prod.geo] ([geono]);
CREATE TABLE [prod.coop] (
    [cno] int NOT NULL PRIMARY KEY,
    [validcno] int NULL,
    [site] int NULL REFERENCES [main.site] ([id]),
    [lname] nvarchar(40) NOT NULL,
    [title] nvarchar(5) NOT NULL,
    [fname] nvarchar(30) NOT NULL,
    [job] nvarchar(40) NOT NULL,
    [org] nvarchar(60) NOT NULL,
    [orgid] nvarchar(10) NOT NULL,
    [add1] nvarchar(60) NOT NULL UNIQUE,
    [add2] nvarchar(60) NOT NULL,
    [add3] nvarchar(60) NOT NULL,
    [city] nvarchar(20) NOT NULL UNIQUE,
    [zip] nvarchar(10) NOT NULL,
    [geono] int NULL UNIQUE REFERENCES [prod.geo] ([geono]),
    [phone1] nvarchar(30) NOT NULL,
    [phone2] nvarchar(30) NOT NULL,
    [fax] nvarchar(30) NOT NULL,
    [email] nvarchar(100) NOT NULL,
    [active] nvarchar(1) NOT NULL,
    [cat] nvarchar(4) NOT NULL,
    [arsregion] nvarchar(3) NOT NULL,
    [discipline] nvarchar(10) NOT NULL,
    [initials] nvarchar(6) NOT NULL,
    [coop] nvarchar(100) NOT NULL UNIQUE,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
ALTER TABLE [prod.coop] ADD CONSTRAINT validcno_refs_cno_44815465 FOREIGN KEY ([validcno]) REFERENCES [prod.coop] ([cno]);
CREATE TABLE [prod.tax] (
    [taxno] int NOT NULL PRIMARY KEY,
    [validtaxno] int NULL,
    [shybrid] nvarchar(1) NOT NULL,
    [species] nvarchar(30) NOT NULL,
    [sauthor] nvarchar(100) NOT NULL,
    [ssphybrid] nvarchar(1) NOT NULL,
    [subsp] nvarchar(30) NOT NULL,
    [sspauthor] nvarchar(100) NOT NULL,
    [varhybrid] nvarchar(1) NOT NULL,
    [var] nvarchar(30) NOT NULL,
    [varauthor] nvarchar(100) NOT NULL,
    [svhybrid] nvarchar(1) NOT NULL,
    [subvar] nvarchar(30) NOT NULL,
    [svauthor] nvarchar(100) NOT NULL,
    [fhybrid] nvarchar(1) NOT NULL,
    [forma] nvarchar(30) NOT NULL,
    [fauthor] nvarchar(100) NOT NULL,
    [gno] int NOT NULL REFERENCES [prod.gn] ([gno]),
    [cropno] int NULL REFERENCES [prod.crop] ([cropno]),
    [psite1] int NULL REFERENCES [main.site] ([id]),
    [psite2] int NULL REFERENCES [main.site] ([id]),
    [rest] nvarchar(10) NOT NULL,
    [lifeform] nvarchar(10) NOT NULL,
    [fert] nvarchar(10) NOT NULL,
    [pending] nvarchar(1) NOT NULL,
    [qual] nvarchar(6) NOT NULL,
    [cno] int NULL REFERENCES [prod.coop] ([cno]),
    [verified] datetime NULL,
    [taxon] nvarchar(100) NOT NULL UNIQUE,
    [taxauthor] nvarchar(100) NOT NULL UNIQUE,
    [protologue] nvarchar(240) NOT NULL,
    [taxcmt] nvarchar(2000) NOT NULL,
    [sitecmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [othname] nvarchar(240) NOT NULL
)
;
ALTER TABLE [prod.tax] ADD CONSTRAINT validtaxno_refs_taxno_30642f77 FOREIGN KEY ([validtaxno]) REFERENCES [prod.tax] ([taxno]);
CREATE TABLE [prod.taxtorc] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [taxno] int NOT NULL,
    [dno] int NOT NULL,
    [ct] int NULL,
    [taxon] nvarchar(70) NOT NULL,
    [crop] nvarchar(20) NOT NULL,
    [cropno] int NULL,
    [genus] nvarchar(30) NOT NULL
)
;
CREATE TABLE [prod.pi] (
    [pivol] int NOT NULL PRIMARY KEY,
    [piyear] int NOT NULL,
    [lowpi] int NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.acc] (
    [acid] int NOT NULL PRIMARY KEY,
    [acp] nvarchar(4) NOT NULL UNIQUE,
    [acno] int NOT NULL UNIQUE,
    [acs] nvarchar(4) NOT NULL UNIQUE,
    [site] int NULL REFERENCES [main.site] ([id]),
    [whynull] nvarchar(10) NOT NULL,
    [core] nvarchar(1) NOT NULL,
    [backup] nvarchar(1) NOT NULL,
    [lifeform] nvarchar(10) NOT NULL,
    [acimpt] nvarchar(10) NOT NULL,
    [uniform] nvarchar(10) NOT NULL,
    [acform] nvarchar(2) NOT NULL,
    [received] datetime NOT NULL,
    [datefmt] nvarchar(10) NOT NULL,
    [taxno] int NOT NULL REFERENCES [prod.tax] ([taxno]),
    [pivol] int NULL REFERENCES [prod.pi] ([pivol]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.eval] (
    [eno] int NOT NULL PRIMARY KEY,
    [ename] nvarchar(100) NOT NULL UNIQUE,
    [site] int NULL REFERENCES [main.site] ([id]),
    [geono] int NULL REFERENCES [prod.geo] ([geono]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [methods] nvarchar(4000) NOT NULL,
    [studytype] nvarchar(10) NOT NULL
)
;
CREATE TABLE [prod.aact] (
    [aactno] int NOT NULL PRIMARY KEY,
    [action] nvarchar(10) NOT NULL,
    [occurred] datetime NULL,
    [fmtoccurred] nvarchar(10) NOT NULL,
    [completed] datetime NULL,
    [fmtcompleted] nvarchar(10) NOT NULL,
    [showweb] nvarchar(1) NOT NULL,
    [narr] nvarchar(2000) NOT NULL,
    [acid] int NOT NULL REFERENCES [prod.acc] ([acid]),
    [cno] int NULL REFERENCES [prod.coop] ([cno]),
    [eno] int NULL REFERENCES [prod.eval] ([eno]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NOT NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.lit] (
    [abbr] nvarchar(20) NOT NULL PRIMARY KEY,
    [stdabbr] nvarchar(240) NOT NULL,
    [reftitle] nvarchar(240) NOT NULL,
    [editor] nvarchar(240) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [site] int NULL REFERENCES [main.site] ([id]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.acit] (
    [citno] int NOT NULL PRIMARY KEY,
    [acid] int NOT NULL REFERENCES [prod.acc] ([acid]),
    [abbr] nvarchar(20) NULL REFERENCES [prod.lit] ([abbr]),
    [cittitle] nvarchar(240) NOT NULL,
    [author] nvarchar(240) NOT NULL,
    [cityr] int NULL,
    [citref] nvarchar(60) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.ag] (
    [agname] nvarchar(20) NOT NULL PRIMARY KEY,
    [cmt] nvarchar(240) NOT NULL,
    [site] int NULL REFERENCES [main.site] ([id]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [url] nvarchar(240) NOT NULL
)
;
CREATE TABLE [prod.im] (
    [imname] nvarchar(20) NOT NULL PRIMARY KEY,
    [site] int NOT NULL PRIMARY KEY REFERENCES [main.site] ([id]),
    [ivt] nvarchar(2) NOT NULL,
    [munits] nvarchar(2) NOT NULL,
    [debit] nvarchar(1) NOT NULL,
    [dform] nvarchar(2) NOT NULL,
    [dquant] int NULL,
    [dunits] nvarchar(2) NOT NULL,
    [dcritical] int NULL,
    [rcritical] int NULL,
    [regen] nvarchar(10) NOT NULL,
    [ptests] int NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [cno] int NULL
)
;
CREATE TABLE [prod.iv] (
    [ivid] int NOT NULL PRIMARY KEY,
    [ivp] nvarchar(4) NOT NULL UNIQUE,
    [ivno] int NOT NULL UNIQUE,
    [ivs] nvarchar(8) NOT NULL UNIQUE,
    [ivt] nvarchar(2) NOT NULL UNIQUE,
    [imname] nvarchar(20) NULL REFERENCES [prod.im] ([imname]),
    [site] int NULL REFERENCES [main.site] ([id]),
    [distribute] nvarchar(1) NOT NULL,
    [loc1] nvarchar(10) NOT NULL,
    [loc2] nvarchar(10) NOT NULL,
    [loc3] nvarchar(10) NOT NULL,
    [loc4] nvarchar(10) NOT NULL,
    [onhand] int NULL,
    [munits] nvarchar(2) NOT NULL,
    [debit] nvarchar(1) NOT NULL,
    [dform] nvarchar(2) NOT NULL,
    [dquant] int NULL,
    [dunits] nvarchar(2) NOT NULL,
    [dcritical] int NULL,
    [rcritical] int NULL,
    [pstatus] nvarchar(10) NOT NULL,
    [status] nvarchar(10) NOT NULL,
    [statcmt] nvarchar(60) NOT NULL,
    [acid] int NOT NULL REFERENCES [prod.acc] ([acid]),
    [parent] int NULL,
    [cno] int NULL REFERENCES [prod.coop] ([cno]),
    [backupiv] int NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.ord] (
    [orno] int NOT NULL PRIMARY KEY,
    [origno] int NULL,
    [site] int NULL REFERENCES [main.site] ([id]),
    [localno] int NULL,
    [ortype] nvarchar(2) NOT NULL,
    [ordered] datetime NULL,
    [status] nvarchar(10) NOT NULL,
    [done] nvarchar(1) NOT NULL,
    [acted] datetime NULL,
    [source] int NULL REFERENCES [prod.coop] ([cno]),
    [orderer] int NULL REFERENCES [prod.coop] ([cno]),
    [shipto] int NULL REFERENCES [prod.coop] ([cno]),
    [final] int NOT NULL REFERENCES [prod.coop] ([cno]),
    [reqref] nvarchar(10) NOT NULL,
    [supplylow] nvarchar(1) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [request] nvarchar(900) NOT NULL
)
;
ALTER TABLE [prod.ord] ADD CONSTRAINT origno_refs_orno_244ff12b FOREIGN KEY ([origno]) REFERENCES [prod.ord] ([orno]);
CREATE TABLE [prod.al] (
    [alno] int NOT NULL PRIMARY KEY,
    [action] nvarchar(10) NOT NULL UNIQUE,
    [acted] datetime NOT NULL UNIQUE,
    [acid] int NOT NULL UNIQUE REFERENCES [prod.acc] ([acid]),
    [site] int NULL REFERENCES [main.site] ([id]),
    [cno] int NULL REFERENCES [prod.coop] ([cno]),
    [ivid] int NULL REFERENCES [prod.iv] ([ivid]),
    [orno] int NULL REFERENCES [prod.ord] ([orno]),
    [oldtaxno] int NULL REFERENCES [prod.tax] ([taxno]),
    [newtaxno] int NULL REFERENCES [prod.tax] ([taxno]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.an] (
    [anno] int NOT NULL PRIMARY KEY,
    [acid] int NOT NULL UNIQUE REFERENCES [prod.acc] ([acid]),
    [idtype] nvarchar(10) NOT NULL,
    [idrank] int NULL,
    [topname] nvarchar(1) NOT NULL,
    [plantid] nvarchar(40) NOT NULL UNIQUE,
    [searchid] nvarchar(40) NOT NULL,
    [agname] nvarchar(20) NULL UNIQUE REFERENCES [prod.ag] ([agname]),
    [ivid] int NULL REFERENCES [prod.iv] ([ivid]),
    [cno] int NULL REFERENCES [prod.coop] ([cno]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.bad] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [ivid] int NULL
)
;
CREATE TABLE [prod.dsc] (
    [dno] int NOT NULL PRIMARY KEY,
    [dqname] nvarchar(10) NOT NULL UNIQUE,
    [dname] nvarchar(30) NOT NULL,
    [cac] nvarchar(1) NOT NULL,
    [dcat] nvarchar(10) NOT NULL,
    [obtype] nvarchar(10) NOT NULL,
    [usecode] nvarchar(1) NOT NULL,
    [obmaxlen] int NULL,
    [obformat] nvarchar(15) NOT NULL,
    [obmax] int NULL,
    [obmin] int NULL,
    [orgtype] nvarchar(10) NOT NULL,
    [orgformat] nvarchar(15) NOT NULL,
    [cropno] int NOT NULL UNIQUE REFERENCES [prod.crop] ([cropno]),
    [site] int NULL REFERENCES [main.site] ([id]),
    [def] nvarchar(240) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.cd] (
    [dno] int NOT NULL PRIMARY KEY REFERENCES [prod.dsc] ([dno]),
    [code] nvarchar(30) NOT NULL PRIMARY KEY,
    [def] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.cg] (
    [cgid] nvarchar(20) NOT NULL PRIMARY KEY,
    [cgname] nvarchar(60) NOT NULL UNIQUE,
    [site] int NULL REFERENCES [main.site] ([id]),
    [historical] nvarchar(1) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.cn] (
    [taxno] int NOT NULL PRIMARY KEY REFERENCES [prod.tax] ([taxno]),
    [cname] nvarchar(50) NOT NULL PRIMARY KEY,
    [source] nvarchar(20) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [cnid] nvarchar(50) NOT NULL
)
;
CREATE TABLE [prod.del_log] (
    [del_id] int NOT NULL PRIMARY KEY,
    [owner_table] nvarchar(30) NOT NULL,
    [table_col] nvarchar(400) NOT NULL,
    [table_val] nvarchar(2000) NOT NULL,
    [site] nvarchar(8) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [date_stamp] datetime NULL
)
;
CREATE TABLE [prod.dist] (
    [distno] int NOT NULL PRIMARY KEY,
    [taxno] int NOT NULL REFERENCES [prod.tax] ([taxno]),
    [geono] int NULL REFERENCES [prod.geo] ([geono]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.durl] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [urltype] nvarchar(10) NOT NULL UNIQUE,
    [seqno] int NOT NULL UNIQUE,
    [cropno] int NOT NULL UNIQUE REFERENCES [prod.crop] ([cropno]),
    [dno] int NULL UNIQUE REFERENCES [prod.dsc] ([dno]),
    [code] nvarchar(30) NULL UNIQUE REFERENCES [prod.cd] ([code]),
    [caption] nvarchar(60) NOT NULL,
    [url] nvarchar(100) NOT NULL,
    [site] int NOT NULL REFERENCES [main.site] ([id]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NOT NULL,
    [modified] datetime NULL,
    [eno] int NULL
)
;
CREATE TABLE [prod.ecit] (
    [citno] int NOT NULL PRIMARY KEY,
    [eno] int NOT NULL REFERENCES [prod.eval] ([eno]),
    [abbr] nvarchar(20) NULL REFERENCES [prod.lit] ([abbr]),
    [cittitle] nvarchar(240) NOT NULL,
    [author] nvarchar(240) NOT NULL,
    [cityr] int NULL,
    [citref] nvarchar(240) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.embr] (
    [cno] int NOT NULL PRIMARY KEY REFERENCES [prod.coop] ([cno]),
    [eno] int NOT NULL PRIMARY KEY REFERENCES [prod.eval] ([eno]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.mrk] (
    [mrkno] int NOT NULL PRIMARY KEY,
    [cropno] int NOT NULL UNIQUE REFERENCES [prod.crop] ([cropno]),
    [site] int NOT NULL REFERENCES [main.site] ([id]),
    [marker] nvarchar(100) NOT NULL UNIQUE,
    [synonyms] nvarchar(200) NOT NULL,
    [repeat_motif] nvarchar(100) NOT NULL,
    [primers] nvarchar(200) NOT NULL,
    [assay_conditions] nvarchar(4000) NOT NULL,
    [range_products] nvarchar(60) NOT NULL,
    [known_standards] nvarchar(300) NOT NULL,
    [genbank_no] nvarchar(20) NOT NULL,
    [map_location] nvarchar(100) NOT NULL,
    [position] nvarchar(1000) NOT NULL,
    [cmt] nvarchar(4000) NOT NULL,
    [poly_type] nvarchar(10) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.ga] (
    [gano] int NOT NULL PRIMARY KEY,
    [mrkno] int NOT NULL UNIQUE REFERENCES [prod.mrk] ([mrkno]),
    [eno] int NOT NULL UNIQUE REFERENCES [prod.eval] ([eno]),
    [method] nvarchar(2000) NOT NULL,
    [scoring_method] nvarchar(1000) NOT NULL,
    [control_values] nvarchar(1000) NOT NULL,
    [no_obs_alleles] int NULL,
    [max_gob_alleles] int NULL,
    [size_alleles] nvarchar(100) NOT NULL,
    [unusual_alleles] nvarchar(100) NOT NULL,
    [cmt] nvarchar(4000) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.gcit] (
    [citno] int NOT NULL PRIMARY KEY,
    [gno] int NOT NULL REFERENCES [prod.gn] ([gno]),
    [abbr] nvarchar(20) NULL REFERENCES [prod.lit] ([abbr]),
    [cittitle] nvarchar(240) NOT NULL,
    [author] nvarchar(240) NOT NULL,
    [cityr] int NULL,
    [citref] nvarchar(60) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.general_config] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [config] nvarchar(1024) NOT NULL,
    [label] nvarchar(64) NOT NULL,
    [userid] nvarchar(32) NOT NULL
)
;
CREATE TABLE [prod.germrule] (
    [ruleno] int NOT NULL PRIMARY KEY,
    [substrata] nvarchar(70) NOT NULL,
    [temp] nvarchar(30) NOT NULL,
    [requirements] nvarchar(500) NOT NULL,
    [author] nvarchar(20) NOT NULL,
    [category] nvarchar(10) NOT NULL,
    [days] nvarchar(20) NOT NULL,
    [taxno] int NULL REFERENCES [prod.tax] ([taxno]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.gnt] (
    [gno] int NOT NULL PRIMARY KEY REFERENCES [prod.gn] ([gno]),
    [famno] int NOT NULL PRIMARY KEY REFERENCES [prod.fam] ([famno]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.gob] (
    [gobno] int NOT NULL PRIMARY KEY,
    [gano] int NOT NULL UNIQUE REFERENCES [prod.ga] ([gano]),
    [ivid] int NOT NULL UNIQUE REFERENCES [prod.iv] ([ivid]),
    [indiv] int NULL UNIQUE,
    [gob] nvarchar(1000) NOT NULL,
    [genbank_link] nvarchar(200) NOT NULL,
    [image_link] nvarchar(200) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.grinwin_ini] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [type] nvarchar(32) NOT NULL,
    [label] nvarchar(32) NOT NULL,
    [value] nvarchar(64) NOT NULL
)
;
CREATE TABLE [prod.src] (
    [srcno] int NOT NULL PRIMARY KEY,
    [srctype] nvarchar(10) NOT NULL,
    [srcdate] datetime NULL,
    [datefmt] nvarchar(10) NOT NULL,
    [origin] nvarchar(1) NOT NULL,
    [acid] int NOT NULL REFERENCES [prod.acc] ([acid]),
    [geono] int NULL REFERENCES [prod.geo] ([geono]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [srcqual] nvarchar(10) NOT NULL
)
;
CREATE TABLE [prod.hab] (
    [srcno] int NOT NULL PRIMARY KEY REFERENCES [prod.src] ([srcno]),
    [acid] int NOT NULL REFERENCES [prod.acc] ([acid]),
    [latd] int NULL,
    [latm] int NULL,
    [lats] int NULL,
    [lath] nvarchar(1) NOT NULL,
    [lond] int NULL,
    [lonm] int NULL,
    [lons] int NULL,
    [lonh] nvarchar(1) NOT NULL,
    [elev] int NULL,
    [quant] int NULL,
    [units] nvarchar(2) NOT NULL,
    [cform] nvarchar(2) NOT NULL,
    [plants] int NULL,
    [locality] nvarchar(240) NOT NULL,
    [habitat] nvarchar(240) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [gctype] nvarchar(10) NOT NULL,
    [gstype] nvarchar(10) NOT NULL
)
;
CREATE TABLE [prod.iact] (
    [iactno] int NOT NULL PRIMARY KEY,
    [action] nvarchar(10) NOT NULL,
    [occurred] datetime NULL,
    [datefmt] nvarchar(10) NOT NULL,
    [quant] int NULL,
    [units] nvarchar(2) NOT NULL,
    [iform] nvarchar(2) NOT NULL,
    [ivid] int NOT NULL REFERENCES [prod.iv] ([ivid]),
    [cno] int NULL REFERENCES [prod.coop] ([cno]),
    [eno] int NULL REFERENCES [prod.eval] ([eno]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [iactqual] nvarchar(10) NOT NULL
)
;
CREATE TABLE [prod.ig] (
    [igname] nvarchar(100) NOT NULL PRIMARY KEY,
    [site] int NOT NULL PRIMARY KEY REFERENCES [main.site] ([id]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.igm] (
    [ivid] int NOT NULL PRIMARY KEY REFERENCES [prod.iv] ([ivid]),
    [igname] nvarchar(100) NOT NULL PRIMARY KEY REFERENCES [prod.ig] ([igname]),
    [site] nvarchar(100) NOT NULL PRIMARY KEY REFERENCES [prod.ig] ([igname]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.ipr] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [acid] int NOT NULL UNIQUE REFERENCES [prod.acc] ([acid]),
    [iprtype] nvarchar(10) NOT NULL UNIQUE,
    [iprid] nvarchar(40) NOT NULL UNIQUE,
    [iprno] int NULL,
    [iprcrop] nvarchar(60) NOT NULL,
    [iprname] nvarchar(240) NOT NULL,
    [issued] datetime NULL,
    [expired] datetime NULL,
    [cno] int NULL REFERENCES [prod.coop] ([cno]),
    [citno] int NULL REFERENCES [prod.acit] ([citno]),
    [cmt] nvarchar(240) NOT NULL,
    [site] int NULL REFERENCES [main.site] ([id]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [accepted] datetime NULL,
    [expected] datetime NULL
)
;
CREATE TABLE [prod.mbr] (
    [cno] int NOT NULL PRIMARY KEY REFERENCES [prod.coop] ([cno]),
    [cgid] nvarchar(20) NOT NULL PRIMARY KEY REFERENCES [prod.cg] ([cgid]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [localid] nvarchar(10) NOT NULL
)
;
CREATE TABLE [prod.mcit] (
    [citno] int NOT NULL PRIMARY KEY,
    [mrkno] int NOT NULL REFERENCES [prod.mrk] ([mrkno]),
    [abbr] nvarchar(20) NULL REFERENCES [prod.lit] ([abbr]),
    [cittitle] nvarchar(240) NOT NULL,
    [author] nvarchar(240) NOT NULL,
    [cityr] int NULL,
    [citref] nvarchar(60) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.menu_item] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [menu_id] int NULL,
    [seqno] int NULL,
    [type] nvarchar(32) NOT NULL,
    [module] nvarchar(64) NOT NULL,
    [path] nvarchar(64) NOT NULL,
    [item] nvarchar(64) NOT NULL,
    [hint] nvarchar(64) NOT NULL,
    [arg] nvarchar(254) NOT NULL
)
;
CREATE TABLE [prod.menu_name] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [menu_id] int NULL,
    [menu_name] nvarchar(32) NOT NULL,
    [user_id] nvarchar(32) NOT NULL,
    [parent] nvarchar(32) NOT NULL,
    [title] nvarchar(64) NOT NULL
)
;
CREATE TABLE [prod.narr] (
    [acid] int NOT NULL PRIMARY KEY REFERENCES [prod.acc] ([acid]),
    [ntype] nvarchar(1) NOT NULL PRIMARY KEY,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [narr] nvarchar(4000) NOT NULL
)
;
CREATE TABLE [prod.oact] (
    [oactno] int NOT NULL PRIMARY KEY,
    [action] nvarchar(10) NOT NULL,
    [acted] datetime NOT NULL,
    [actid] nvarchar(40) NOT NULL,
    [orno] int NOT NULL REFERENCES [prod.ord] ([orno]),
    [site] int NULL REFERENCES [main.site] ([id]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [coop] nvarchar(100) NOT NULL,
    [cno] int NULL
)
;
CREATE TABLE [prod.qual] (
    [qno] int NOT NULL PRIMARY KEY,
    [qual] nvarchar(30) NOT NULL,
    [dno] int NOT NULL REFERENCES [prod.dsc] ([dno]),
    [def] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.ob] (
    [dno] int NOT NULL UNIQUE REFERENCES [prod.dsc] ([dno]),
    [ob] nvarchar(30) NOT NULL UNIQUE,
    [acid] int NOT NULL UNIQUE REFERENCES [prod.acc] ([acid]),
    [eno] int NOT NULL UNIQUE REFERENCES [prod.eval] ([eno]),
    [qno] int NULL UNIQUE REFERENCES [prod.qual] ([qno]),
    [ivid] int NULL UNIQUE REFERENCES [prod.iv] ([ivid]),
    [orgvalue] nvarchar(30) NOT NULL,
    [freq] double precision NULL,
    [mean] int NULL,
    [high] int NULL,
    [low] int NULL,
    [sdev] int NULL,
    [ssize] int NULL,
    [cmt] nvarchar(500) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [obno] int NOT NULL PRIMARY KEY,
    [rank] int NULL
)
;
CREATE TABLE [prod.oi] (
    [orno] int NULL UNIQUE REFERENCES [prod.ord] ([orno]),
    [oino] int NULL UNIQUE,
    [item] nvarchar(40) NOT NULL,
    [quant] int NULL,
    [units] nvarchar(2) NOT NULL,
    [dform] nvarchar(2) NOT NULL,
    [rest] nvarchar(10) NOT NULL,
    [status] nvarchar(10) NOT NULL,
    [acted] datetime NULL,
    [cno] int NULL REFERENCES [prod.coop] ([cno]),
    [ivid] int NULL REFERENCES [prod.iv] ([ivid]),
    [acid] int NULL REFERENCES [prod.acc] ([acid]),
    [taxno] int NULL REFERENCES [prod.tax] ([taxno]),
    [taxon] nvarchar(100) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [itno] int NOT NULL PRIMARY KEY
)
;
CREATE TABLE [prod.ped] (
    [acid] int NOT NULL PRIMARY KEY REFERENCES [prod.acc] ([acid]),
    [released] datetime NULL,
    [datefmt] nvarchar(10) NOT NULL,
    [citno] int NULL REFERENCES [prod.acit] ([citno]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [pedigree] nvarchar(2000) NOT NULL
)
;
CREATE TABLE [prod.pt] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [ivid] int NOT NULL UNIQUE REFERENCES [prod.iv] ([ivid]),
    [pttype] nvarchar(10) NOT NULL UNIQUE,
    [ptcode] nvarchar(10) NOT NULL UNIQUE,
    [began] datetime NULL,
    [finished] datetime NULL UNIQUE,
    [results] nvarchar(10) NOT NULL,
    [needed] int NULL,
    [started] int NULL,
    [completed] int NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.quar] (
    [acid] int NOT NULL PRIMARY KEY REFERENCES [prod.acc] ([acid]),
    [qtype] nvarchar(10) NOT NULL PRIMARY KEY,
    [status] nvarchar(10) NOT NULL,
    [cno] int NOT NULL REFERENCES [prod.coop] ([cno]),
    [entered] datetime NULL,
    [establish] datetime NULL,
    [expected] datetime NULL,
    [released] datetime NULL,
    [cmt] nvarchar(240) NOT NULL,
    [site] int NULL REFERENCES [main.site] ([id]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.sec_priv] (
    [id] int NOT NULL PRIMARY KEY,
    [sec_role_id] int NULL,
    [sec_user_id] int NULL,
    [table_name] nvarchar(50) NOT NULL,
    [target_sec_user_id] int NULL,
    [priv_create] nvarchar(1) NOT NULL,
    [priv_read] nvarchar(1) NOT NULL,
    [priv_update] nvarchar(1) NOT NULL,
    [priv_delete] nvarchar(1) NOT NULL,
    [enabled] nvarchar(1) NOT NULL,
    [created_at] datetime NOT NULL,
    [updated_at] datetime NULL
)
;
CREATE TABLE [prod.sec_role] (
    [id] int NOT NULL PRIMARY KEY,
    [role_name] nvarchar(50) NOT NULL
)
;
CREATE TABLE [prod.sec_user] (
    [id] int NOT NULL PRIMARY KEY,
    [user_name] nvarchar(50) NOT NULL UNIQUE,
    [password] nvarchar(255) NOT NULL,
    [enabled] nvarchar(1) NOT NULL,
    [created_at] datetime NOT NULL,
    [updated_at] datetime NULL
)
;
CREATE TABLE [prod.sec_user_role] (
    [id] int NOT NULL PRIMARY KEY,
    [sec_user_id] int NOT NULL,
    [sec_role_id] int NOT NULL
)
;
CREATE TABLE [prod.smbr] (
    [srcno] int NOT NULL PRIMARY KEY REFERENCES [prod.src] ([srcno]),
    [acid] int NOT NULL REFERENCES [prod.acc] ([acid]),
    [cno] int NOT NULL PRIMARY KEY REFERENCES [prod.coop] ([cno]),
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.sql_operators] (
    [id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [oper] nvarchar(12) NOT NULL,
    [seqno] int NULL
)
;
CREATE TABLE [prod.taut] (
    [shortaut] nvarchar(30) NOT NULL PRIMARY KEY,
    [longaut] nvarchar(100) NOT NULL,
    [smarkaut] nvarchar(30) NOT NULL,
    [lmarkaut] nvarchar(100) NOT NULL,
    [shexpaut] nvarchar(30) NOT NULL,
    [lgexpaut] nvarchar(100) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NOT NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.tcit] (
    [citno] int NOT NULL PRIMARY KEY,
    [taxno] int NOT NULL REFERENCES [prod.tax] ([taxno]),
    [abbr] nvarchar(20) NULL REFERENCES [prod.lit] ([abbr]),
    [cittitle] nvarchar(240) NOT NULL,
    [author] nvarchar(240) NOT NULL,
    [cityr] int NULL,
    [citref] nvarchar(60) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.turl] (
    [turlno] int NOT NULL PRIMARY KEY,
    [urltype] nvarchar(10) NOT NULL,
    [famno] int NOT NULL REFERENCES [prod.fam] ([famno]),
    [gno] int NULL REFERENCES [prod.gn] ([gno]),
    [taxno] int NULL REFERENCES [prod.tax] ([taxno]),
    [caption] nvarchar(240) NOT NULL,
    [url] nvarchar(100) NOT NULL,
    [site] int NOT NULL REFERENCES [main.site] ([id]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NOT NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.uses] (
    [taxno] int NOT NULL PRIMARY KEY REFERENCES [prod.tax] ([taxno]),
    [taxuse] nvarchar(10) NOT NULL PRIMARY KEY,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [usetype] nvarchar(250) NOT NULL
)
;
CREATE TABLE [prod.via] (
    [viano] int NOT NULL PRIMARY KEY,
    [tested] datetime NOT NULL,
    [datefmt] nvarchar(10) NOT NULL,
    [norm] int NULL,
    [abnorm] int NULL,
    [dormant] int NULL,
    [viable] int NULL,
    [vigor] nvarchar(10) NOT NULL,
    [sample] int NULL,
    [reps] int NULL,
    [ivid] int NOT NULL REFERENCES [prod.iv] ([ivid]),
    [eno] int NULL REFERENCES [prod.eval] ([eno]),
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL
)
;
CREATE TABLE [prod.vou] (
    [acid] int NOT NULL UNIQUE REFERENCES [prod.acc] ([acid]),
    [vtype] nvarchar(10) NOT NULL UNIQUE,
    [ivid] int NULL UNIQUE REFERENCES [prod.iv] ([ivid]),
    [cno] int NULL UNIQUE REFERENCES [prod.coop] ([cno]),
    [vouchered] datetime NULL UNIQUE,
    [datefmt] nvarchar(10) NOT NULL,
    [collid] nvarchar(40) NOT NULL UNIQUE,
    [vloc] nvarchar(500) NOT NULL UNIQUE,
    [vcontent] nvarchar(100) NOT NULL,
    [cmt] nvarchar(240) NOT NULL,
    [userid] nvarchar(10) NOT NULL,
    [created] datetime NULL,
    [modified] datetime NULL,
    [thumbnail] nvarchar(500) NOT NULL,
    [vno] int NOT NULL PRIMARY KEY
)
;
COMMIT;
