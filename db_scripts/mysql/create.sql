BEGIN;
CREATE TABLE `main`.`site` (
    `id` integer NOT NULL PRIMARY KEY,
    `field1` varchar(45) NOT NULL
)
;
CREATE TABLE `prod`.`test1` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `site` integer NULL
)
;
ALTER TABLE `prod`.`test1` ADD CONSTRAINT site_refs_id_6f90afd0 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
CREATE TABLE `prod`.`fam` (
    `famno` integer NOT NULL PRIMARY KEY,
    `validfamno` integer NULL,
    `family` varchar(25) NOT NULL UNIQUE,
    `famauthor` varchar(100) NOT NULL UNIQUE,
    `altfamily` varchar(25) NOT NULL,
    `subfamily` varchar(25) NOT NULL UNIQUE,
    `tribe` varchar(25) NOT NULL UNIQUE,
    `subtribe` varchar(25) NOT NULL UNIQUE,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`fam` ADD CONSTRAINT validfamno_refs_famno_54ee6c7d FOREIGN KEY (`validfamno`) REFERENCES `prod`.`fam` (`famno`);
CREATE TABLE `prod`.`crop` (
    `cropno` integer NOT NULL PRIMARY KEY,
    `crop` varchar(20) NOT NULL UNIQUE,
    `cmt` varchar(2000) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
CREATE TABLE `prod`.`gn` (
    `gno` integer NOT NULL PRIMARY KEY,
    `validgno` integer NULL,
    `qual` varchar(2) NOT NULL,
    `ghybrid` varchar(1) NOT NULL,
    `genus` varchar(30) NOT NULL UNIQUE,
    `gauthor` varchar(100) NOT NULL UNIQUE,
    `subgenus` varchar(30) NOT NULL UNIQUE,
    `section` varchar(30) NOT NULL UNIQUE,
    `series` varchar(30) NOT NULL UNIQUE,
    `subseries` varchar(30) NOT NULL UNIQUE,
    `famno` integer NULL,
    `othfamily` varchar(100) NOT NULL,
    `cname` varchar(30) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `subsection` varchar(30) NOT NULL UNIQUE
)
;
ALTER TABLE `prod`.`gn` ADD CONSTRAINT famno_refs_famno_75a1e695 FOREIGN KEY (`famno`) REFERENCES `prod`.`fam` (`famno`);
ALTER TABLE `prod`.`gn` ADD CONSTRAINT validgno_refs_gno_5afc285 FOREIGN KEY (`validgno`) REFERENCES `prod`.`gn` (`gno`);
CREATE TABLE `prod`.`reg` (
    `regno` integer NOT NULL PRIMARY KEY,
    `area` varchar(20) NOT NULL UNIQUE,
    `region` varchar(30) NOT NULL UNIQUE,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
CREATE TABLE `prod`.`geo` (
    `geono` integer NOT NULL PRIMARY KEY,
    `validgeono` integer NULL,
    `country` varchar(30) NOT NULL UNIQUE,
    `state` varchar(20) NOT NULL UNIQUE,
    `isofull` varchar(100) NOT NULL,
    `isoshort` varchar(60) NOT NULL,
    `statefull` varchar(60) NOT NULL,
    `iso3` varchar(3) NOT NULL,
    `iso2` varchar(2) NOT NULL,
    `st` varchar(3) NOT NULL,
    `cflag` varchar(1) NOT NULL,
    `lath` varchar(1) NOT NULL,
    `lonh` varchar(1) NOT NULL,
    `regno` integer NULL,
    `changed` date NULL,
    `oldname` varchar(100) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`geo` ADD CONSTRAINT regno_refs_regno_570ecd4c FOREIGN KEY (`regno`) REFERENCES `prod`.`reg` (`regno`);
ALTER TABLE `prod`.`geo` ADD CONSTRAINT validgeono_refs_geono_447defb9 FOREIGN KEY (`validgeono`) REFERENCES `prod`.`geo` (`geono`);
CREATE TABLE `prod`.`coop` (
    `cno` integer NOT NULL PRIMARY KEY,
    `validcno` integer NULL,
    `site` integer NULL,
    `lname` varchar(40) NOT NULL,
    `title` varchar(5) NOT NULL,
    `fname` varchar(30) NOT NULL,
    `job` varchar(40) NOT NULL,
    `org` varchar(60) NOT NULL,
    `orgid` varchar(10) NOT NULL,
    `add1` varchar(60) NOT NULL UNIQUE,
    `add2` varchar(60) NOT NULL,
    `add3` varchar(60) NOT NULL,
    `city` varchar(20) NOT NULL UNIQUE,
    `zip` varchar(10) NOT NULL,
    `geono` integer NULL UNIQUE,
    `phone1` varchar(30) NOT NULL,
    `phone2` varchar(30) NOT NULL,
    `fax` varchar(30) NOT NULL,
    `email` varchar(100) NOT NULL,
    `active` varchar(1) NOT NULL,
    `cat` varchar(4) NOT NULL,
    `arsregion` varchar(3) NOT NULL,
    `discipline` varchar(10) NOT NULL,
    `initials` varchar(6) NOT NULL,
    `coop` varchar(100) NOT NULL UNIQUE,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`coop` ADD CONSTRAINT site_refs_id_47710beb FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`coop` ADD CONSTRAINT geono_refs_geono_21043f3c FOREIGN KEY (`geono`) REFERENCES `prod`.`geo` (`geono`);
ALTER TABLE `prod`.`coop` ADD CONSTRAINT validcno_refs_cno_44815465 FOREIGN KEY (`validcno`) REFERENCES `prod`.`coop` (`cno`);
CREATE TABLE `prod`.`tax` (
    `taxno` integer NOT NULL PRIMARY KEY,
    `validtaxno` integer NULL,
    `shybrid` varchar(1) NOT NULL,
    `species` varchar(30) NOT NULL,
    `sauthor` varchar(100) NOT NULL,
    `ssphybrid` varchar(1) NOT NULL,
    `subsp` varchar(30) NOT NULL,
    `sspauthor` varchar(100) NOT NULL,
    `varhybrid` varchar(1) NOT NULL,
    `var` varchar(30) NOT NULL,
    `varauthor` varchar(100) NOT NULL,
    `svhybrid` varchar(1) NOT NULL,
    `subvar` varchar(30) NOT NULL,
    `svauthor` varchar(100) NOT NULL,
    `fhybrid` varchar(1) NOT NULL,
    `forma` varchar(30) NOT NULL,
    `fauthor` varchar(100) NOT NULL,
    `gno` integer NOT NULL,
    `cropno` integer NULL,
    `psite1` integer NULL,
    `psite2` integer NULL,
    `rest` varchar(10) NOT NULL,
    `lifeform` varchar(10) NOT NULL,
    `fert` varchar(10) NOT NULL,
    `pending` varchar(1) NOT NULL,
    `qual` varchar(6) NOT NULL,
    `cno` integer NULL,
    `verified` date NULL,
    `taxon` varchar(100) NOT NULL UNIQUE,
    `taxauthor` varchar(100) NOT NULL UNIQUE,
    `protologue` varchar(240) NOT NULL,
    `taxcmt` varchar(2000) NOT NULL,
    `sitecmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `othname` varchar(240) NOT NULL
)
;
ALTER TABLE `prod`.`tax` ADD CONSTRAINT gno_refs_gno_2623eaa4 FOREIGN KEY (`gno`) REFERENCES `prod`.`gn` (`gno`);
ALTER TABLE `prod`.`tax` ADD CONSTRAINT psite1_refs_id_1081ce98 FOREIGN KEY (`psite1`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`tax` ADD CONSTRAINT psite2_refs_id_1081ce98 FOREIGN KEY (`psite2`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`tax` ADD CONSTRAINT cno_refs_cno_326079e2 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`tax` ADD CONSTRAINT cropno_refs_cropno_5d5c5889 FOREIGN KEY (`cropno`) REFERENCES `prod`.`crop` (`cropno`);
ALTER TABLE `prod`.`tax` ADD CONSTRAINT validtaxno_refs_taxno_30642f77 FOREIGN KEY (`validtaxno`) REFERENCES `prod`.`tax` (`taxno`);
CREATE TABLE `prod`.`taxtorc` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `taxno` integer NOT NULL,
    `dno` integer NOT NULL,
    `ct` integer NULL,
    `taxon` varchar(70) NOT NULL,
    `crop` varchar(20) NOT NULL,
    `cropno` integer NULL,
    `genus` varchar(30) NOT NULL
)
;
CREATE TABLE `prod`.`pi` (
    `pivol` integer NOT NULL PRIMARY KEY,
    `piyear` integer NOT NULL,
    `lowpi` integer NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
CREATE TABLE `prod`.`acc` (
    `acid` integer NOT NULL PRIMARY KEY,
    `acp` varchar(4) NOT NULL UNIQUE,
    `acno` integer NOT NULL UNIQUE,
    `acs` varchar(4) NOT NULL UNIQUE,
    `site` integer NULL,
    `whynull` varchar(10) NOT NULL,
    `core` varchar(1) NOT NULL,
    `backup` varchar(1) NOT NULL,
    `lifeform` varchar(10) NOT NULL,
    `acimpt` varchar(10) NOT NULL,
    `uniform` varchar(10) NOT NULL,
    `acform` varchar(2) NOT NULL,
    `received` date NOT NULL,
    `datefmt` varchar(10) NOT NULL,
    `taxno` integer NOT NULL,
    `pivol` integer NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`acc` ADD CONSTRAINT site_refs_id_3f52da20 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`acc` ADD CONSTRAINT taxno_refs_taxno_7fc0cebf FOREIGN KEY (`taxno`) REFERENCES `prod`.`tax` (`taxno`);
ALTER TABLE `prod`.`acc` ADD CONSTRAINT pivol_refs_pivol_1e3b96e FOREIGN KEY (`pivol`) REFERENCES `prod`.`pi` (`pivol`);
CREATE TABLE `prod`.`eval` (
    `eno` integer NOT NULL PRIMARY KEY,
    `ename` varchar(100) NOT NULL UNIQUE,
    `site` integer NULL,
    `geono` integer NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `methods` varchar(4000) NOT NULL,
    `studytype` varchar(10) NOT NULL
)
;
ALTER TABLE `prod`.`eval` ADD CONSTRAINT site_refs_id_5252303a FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`eval` ADD CONSTRAINT geono_refs_geono_486602f5 FOREIGN KEY (`geono`) REFERENCES `prod`.`geo` (`geono`);
CREATE TABLE `prod`.`aact` (
    `aactno` integer NOT NULL PRIMARY KEY,
    `action` varchar(10) NOT NULL,
    `occurred` date NULL,
    `fmtoccurred` varchar(10) NOT NULL,
    `completed` date NULL,
    `fmtcompleted` varchar(10) NOT NULL,
    `showweb` varchar(1) NOT NULL,
    `narr` varchar(2000) NOT NULL,
    `acid` integer NOT NULL,
    `cno` integer NULL,
    `eno` integer NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NOT NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`aact` ADD CONSTRAINT acid_refs_acid_30a4e0cc FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`aact` ADD CONSTRAINT cno_refs_cno_7304feb3 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`aact` ADD CONSTRAINT eno_refs_eno_51a460aa FOREIGN KEY (`eno`) REFERENCES `prod`.`eval` (`eno`);
CREATE TABLE `prod`.`lit` (
    `abbr` varchar(20) NOT NULL PRIMARY KEY,
    `stdabbr` varchar(240) NOT NULL,
    `reftitle` varchar(240) NOT NULL,
    `editor` varchar(240) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `site` integer NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`lit` ADD CONSTRAINT site_refs_id_141bb774 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
CREATE TABLE `prod`.`acit` (
    `citno` integer NOT NULL PRIMARY KEY,
    `acid` integer NOT NULL,
    `abbr` varchar(20) NULL,
    `cittitle` varchar(240) NOT NULL,
    `author` varchar(240) NOT NULL,
    `cityr` integer NULL,
    `citref` varchar(60) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`acit` ADD CONSTRAINT acid_refs_acid_3b3632bc FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`acit` ADD CONSTRAINT abbr_refs_abbr_64f8cb80 FOREIGN KEY (`abbr`) REFERENCES `prod`.`lit` (`abbr`);
CREATE TABLE `prod`.`ag` (
    `agname` varchar(20) NOT NULL PRIMARY KEY,
    `cmt` varchar(240) NOT NULL,
    `site` integer NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `url` varchar(240) NOT NULL
)
;
ALTER TABLE `prod`.`ag` ADD CONSTRAINT site_refs_id_b40004e FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
CREATE TABLE `prod`.`im` (
    `imname` varchar(20) NOT NULL PRIMARY KEY,
    `site` integer NOT NULL PRIMARY KEY,
    `ivt` varchar(2) NOT NULL,
    `munits` varchar(2) NOT NULL,
    `debit` varchar(1) NOT NULL,
    `dform` varchar(2) NOT NULL,
    `dquant` integer NULL,
    `dunits` varchar(2) NOT NULL,
    `dcritical` integer NULL,
    `rcritical` integer NULL,
    `regen` varchar(10) NOT NULL,
    `ptests` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `cno` integer NULL
)
;
ALTER TABLE `prod`.`im` ADD CONSTRAINT site_refs_id_229b0790 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
CREATE TABLE `prod`.`iv` (
    `ivid` integer NOT NULL PRIMARY KEY,
    `ivp` varchar(4) NOT NULL UNIQUE,
    `ivno` integer NOT NULL UNIQUE,
    `ivs` varchar(8) NOT NULL UNIQUE,
    `ivt` varchar(2) NOT NULL UNIQUE,
    `imname` varchar(20) NULL,
    `site` integer NULL,
    `distribute` varchar(1) NOT NULL,
    `loc1` varchar(10) NOT NULL,
    `loc2` varchar(10) NOT NULL,
    `loc3` varchar(10) NOT NULL,
    `loc4` varchar(10) NOT NULL,
    `onhand` integer NULL,
    `munits` varchar(2) NOT NULL,
    `debit` varchar(1) NOT NULL,
    `dform` varchar(2) NOT NULL,
    `dquant` integer NULL,
    `dunits` varchar(2) NOT NULL,
    `dcritical` integer NULL,
    `rcritical` integer NULL,
    `pstatus` varchar(10) NOT NULL,
    `status` varchar(10) NOT NULL,
    `statcmt` varchar(60) NOT NULL,
    `acid` integer NOT NULL,
    `parent` integer NULL,
    `cno` integer NULL,
    `backupiv` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`iv` ADD CONSTRAINT imname_refs_imname_51e20688 FOREIGN KEY (`imname`) REFERENCES `prod`.`im` (`imname`);
ALTER TABLE `prod`.`iv` ADD CONSTRAINT site_refs_id_3b4e355f FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`iv` ADD CONSTRAINT cno_refs_cno_319db227 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`iv` ADD CONSTRAINT acid_refs_acid_5b093128 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
CREATE TABLE `prod`.`ord` (
    `orno` integer NOT NULL PRIMARY KEY,
    `origno` integer NULL,
    `site` integer NULL,
    `localno` integer NULL,
    `ortype` varchar(2) NOT NULL,
    `ordered` date NULL,
    `status` varchar(10) NOT NULL,
    `done` varchar(1) NOT NULL,
    `acted` date NULL,
    `source` integer NULL,
    `orderer` integer NULL,
    `shipto` integer NULL,
    `final` integer NOT NULL,
    `reqref` varchar(10) NOT NULL,
    `supplylow` varchar(1) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `request` varchar(900) NOT NULL
)
;
ALTER TABLE `prod`.`ord` ADD CONSTRAINT site_refs_id_37fdb39e FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`ord` ADD CONSTRAINT source_refs_cno_5711cc18 FOREIGN KEY (`source`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`ord` ADD CONSTRAINT orderer_refs_cno_5711cc18 FOREIGN KEY (`orderer`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`ord` ADD CONSTRAINT shipto_refs_cno_5711cc18 FOREIGN KEY (`shipto`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`ord` ADD CONSTRAINT final_refs_cno_5711cc18 FOREIGN KEY (`final`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`ord` ADD CONSTRAINT origno_refs_orno_244ff12b FOREIGN KEY (`origno`) REFERENCES `prod`.`ord` (`orno`);
CREATE TABLE `prod`.`al` (
    `alno` integer NOT NULL PRIMARY KEY,
    `action` varchar(10) NOT NULL UNIQUE,
    `acted` date NOT NULL UNIQUE,
    `acid` integer NOT NULL UNIQUE,
    `site` integer NULL,
    `cno` integer NULL,
    `ivid` integer NULL,
    `orno` integer NULL,
    `oldtaxno` integer NULL,
    `newtaxno` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`al` ADD CONSTRAINT site_refs_id_659bab11 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`al` ADD CONSTRAINT cno_refs_cno_2f1be117 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`al` ADD CONSTRAINT ivid_refs_ivid_6dffc94b FOREIGN KEY (`ivid`) REFERENCES `prod`.`iv` (`ivid`);
ALTER TABLE `prod`.`al` ADD CONSTRAINT acid_refs_acid_19f3d096 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`al` ADD CONSTRAINT oldtaxno_refs_taxno_4a5d9fbe FOREIGN KEY (`oldtaxno`) REFERENCES `prod`.`tax` (`taxno`);
ALTER TABLE `prod`.`al` ADD CONSTRAINT newtaxno_refs_taxno_4a5d9fbe FOREIGN KEY (`newtaxno`) REFERENCES `prod`.`tax` (`taxno`);
ALTER TABLE `prod`.`al` ADD CONSTRAINT orno_refs_orno_4cd5e0ec FOREIGN KEY (`orno`) REFERENCES `prod`.`ord` (`orno`);
CREATE TABLE `prod`.`an` (
    `anno` integer NOT NULL PRIMARY KEY,
    `acid` integer NOT NULL UNIQUE,
    `idtype` varchar(10) NOT NULL,
    `idrank` integer NULL,
    `topname` varchar(1) NOT NULL,
    `plantid` varchar(40) NOT NULL UNIQUE,
    `searchid` varchar(40) NOT NULL,
    `agname` varchar(20) NULL UNIQUE,
    `ivid` integer NULL,
    `cno` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`an` ADD CONSTRAINT acid_refs_acid_46a4d908 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`an` ADD CONSTRAINT agname_refs_agname_7d0dd032 FOREIGN KEY (`agname`) REFERENCES `prod`.`ag` (`agname`);
ALTER TABLE `prod`.`an` ADD CONSTRAINT ivid_refs_ivid_13130e1d FOREIGN KEY (`ivid`) REFERENCES `prod`.`iv` (`ivid`);
ALTER TABLE `prod`.`an` ADD CONSTRAINT cno_refs_cno_636881b9 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
CREATE TABLE `prod`.`bad` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `ivid` integer NULL
)
;
CREATE TABLE `prod`.`dsc` (
    `dno` integer NOT NULL PRIMARY KEY,
    `dqname` varchar(10) NOT NULL UNIQUE,
    `dname` varchar(30) NOT NULL,
    `cac` varchar(1) NOT NULL,
    `dcat` varchar(10) NOT NULL,
    `obtype` varchar(10) NOT NULL,
    `usecode` varchar(1) NOT NULL,
    `obmaxlen` integer NULL,
    `obformat` varchar(15) NOT NULL,
    `obmax` integer NULL,
    `obmin` integer NULL,
    `orgtype` varchar(10) NOT NULL,
    `orgformat` varchar(15) NOT NULL,
    `cropno` integer NOT NULL UNIQUE,
    `site` integer NULL,
    `def` varchar(240) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`dsc` ADD CONSTRAINT cropno_refs_cropno_69f0d99c FOREIGN KEY (`cropno`) REFERENCES `prod`.`crop` (`cropno`);
ALTER TABLE `prod`.`dsc` ADD CONSTRAINT site_refs_id_7e3772ab FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
CREATE TABLE `prod`.`cd` (
    `dno` integer NOT NULL PRIMARY KEY,
    `code` varchar(30) NOT NULL PRIMARY KEY,
    `def` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`cd` ADD CONSTRAINT dno_refs_dno_28b4aed FOREIGN KEY (`dno`) REFERENCES `prod`.`dsc` (`dno`);
CREATE TABLE `prod`.`cg` (
    `cgid` varchar(20) NOT NULL PRIMARY KEY,
    `cgname` varchar(60) NOT NULL UNIQUE,
    `site` integer NULL,
    `historical` varchar(1) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`cg` ADD CONSTRAINT site_refs_id_3f0da7c FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
CREATE TABLE `prod`.`cn` (
    `taxno` integer NOT NULL PRIMARY KEY,
    `cname` varchar(50) NOT NULL PRIMARY KEY,
    `source` varchar(20) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `cnid` varchar(50) NOT NULL
)
;
ALTER TABLE `prod`.`cn` ADD CONSTRAINT taxno_refs_taxno_721b6c1e FOREIGN KEY (`taxno`) REFERENCES `prod`.`tax` (`taxno`);
CREATE TABLE `prod`.`del_log` (
    `del_id` integer NOT NULL PRIMARY KEY,
    `owner_table` varchar(30) NOT NULL,
    `table_col` varchar(400) NOT NULL,
    `table_val` varchar(2000) NOT NULL,
    `site` varchar(8) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `date_stamp` date NULL
)
;
CREATE TABLE `prod`.`dist` (
    `distno` integer NOT NULL PRIMARY KEY,
    `taxno` integer NOT NULL,
    `geono` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`dist` ADD CONSTRAINT taxno_refs_taxno_3cab40ed FOREIGN KEY (`taxno`) REFERENCES `prod`.`tax` (`taxno`);
ALTER TABLE `prod`.`dist` ADD CONSTRAINT geono_refs_geono_89b3505 FOREIGN KEY (`geono`) REFERENCES `prod`.`geo` (`geono`);
CREATE TABLE `prod`.`durl` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `urltype` varchar(10) NOT NULL UNIQUE,
    `seqno` integer NOT NULL UNIQUE,
    `cropno` integer NOT NULL UNIQUE,
    `dno` integer NULL UNIQUE,
    `code` varchar(30) NULL UNIQUE,
    `caption` varchar(60) NOT NULL,
    `url` varchar(100) NOT NULL,
    `site` integer NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NOT NULL,
    `modified` date NULL,
    `eno` integer NULL
)
;
ALTER TABLE `prod`.`durl` ADD CONSTRAINT cropno_refs_cropno_6376eb7a FOREIGN KEY (`cropno`) REFERENCES `prod`.`crop` (`cropno`);
ALTER TABLE `prod`.`durl` ADD CONSTRAINT dno_refs_dno_6c25a40f FOREIGN KEY (`dno`) REFERENCES `prod`.`dsc` (`dno`);
ALTER TABLE `prod`.`durl` ADD CONSTRAINT site_refs_id_5b1ebab9 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`durl` ADD CONSTRAINT code_refs_code_16bcb7af FOREIGN KEY (`code`) REFERENCES `prod`.`cd` (`code`);
CREATE TABLE `prod`.`ecit` (
    `citno` integer NOT NULL PRIMARY KEY,
    `eno` integer NOT NULL,
    `abbr` varchar(20) NULL,
    `cittitle` varchar(240) NOT NULL,
    `author` varchar(240) NOT NULL,
    `cityr` integer NULL,
    `citref` varchar(240) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`ecit` ADD CONSTRAINT eno_refs_eno_3ff96fde FOREIGN KEY (`eno`) REFERENCES `prod`.`eval` (`eno`);
ALTER TABLE `prod`.`ecit` ADD CONSTRAINT abbr_refs_abbr_6f098b3c FOREIGN KEY (`abbr`) REFERENCES `prod`.`lit` (`abbr`);
CREATE TABLE `prod`.`embr` (
    `cno` integer NOT NULL PRIMARY KEY,
    `eno` integer NOT NULL PRIMARY KEY,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`embr` ADD CONSTRAINT cno_refs_cno_45b6c8dc FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`embr` ADD CONSTRAINT eno_refs_eno_3ec8492d FOREIGN KEY (`eno`) REFERENCES `prod`.`eval` (`eno`);
CREATE TABLE `prod`.`mrk` (
    `mrkno` integer NOT NULL PRIMARY KEY,
    `cropno` integer NOT NULL UNIQUE,
    `site` integer NOT NULL,
    `marker` varchar(100) NOT NULL UNIQUE,
    `synonyms` varchar(200) NOT NULL,
    `repeat_motif` varchar(100) NOT NULL,
    `primers` varchar(200) NOT NULL,
    `assay_conditions` varchar(4000) NOT NULL,
    `range_products` varchar(60) NOT NULL,
    `known_standards` varchar(300) NOT NULL,
    `genbank_no` varchar(20) NOT NULL,
    `map_location` varchar(100) NOT NULL,
    `position` varchar(1000) NOT NULL,
    `cmt` varchar(4000) NOT NULL,
    `poly_type` varchar(10) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`mrk` ADD CONSTRAINT cropno_refs_cropno_105112a FOREIGN KEY (`cropno`) REFERENCES `prod`.`crop` (`cropno`);
ALTER TABLE `prod`.`mrk` ADD CONSTRAINT site_refs_id_6bad8d69 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
CREATE TABLE `prod`.`ga` (
    `gano` integer NOT NULL PRIMARY KEY,
    `mrkno` integer NOT NULL UNIQUE,
    `eno` integer NOT NULL UNIQUE,
    `method` varchar(2000) NOT NULL,
    `scoring_method` varchar(1000) NOT NULL,
    `control_values` varchar(1000) NOT NULL,
    `no_obs_alleles` integer NULL,
    `max_gob_alleles` integer NULL,
    `size_alleles` varchar(100) NOT NULL,
    `unusual_alleles` varchar(100) NOT NULL,
    `cmt` varchar(4000) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`ga` ADD CONSTRAINT mrkno_refs_mrkno_7bfad1cc FOREIGN KEY (`mrkno`) REFERENCES `prod`.`mrk` (`mrkno`);
ALTER TABLE `prod`.`ga` ADD CONSTRAINT eno_refs_eno_685cc379 FOREIGN KEY (`eno`) REFERENCES `prod`.`eval` (`eno`);
CREATE TABLE `prod`.`gcit` (
    `citno` integer NOT NULL PRIMARY KEY,
    `gno` integer NOT NULL,
    `abbr` varchar(20) NULL,
    `cittitle` varchar(240) NOT NULL,
    `author` varchar(240) NOT NULL,
    `cityr` integer NULL,
    `citref` varchar(60) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`gcit` ADD CONSTRAINT gno_refs_gno_1b0f5acd FOREIGN KEY (`gno`) REFERENCES `prod`.`gn` (`gno`);
ALTER TABLE `prod`.`gcit` ADD CONSTRAINT abbr_refs_abbr_616baef6 FOREIGN KEY (`abbr`) REFERENCES `prod`.`lit` (`abbr`);
CREATE TABLE `prod`.`general_config` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `config` varchar(1024) NOT NULL,
    `label` varchar(64) NOT NULL,
    `userid` varchar(32) NOT NULL
)
;
CREATE TABLE `prod`.`germrule` (
    `ruleno` integer NOT NULL PRIMARY KEY,
    `substrata` varchar(70) NOT NULL,
    `temp` varchar(30) NOT NULL,
    `requirements` varchar(500) NOT NULL,
    `author` varchar(20) NOT NULL,
    `category` varchar(10) NOT NULL,
    `days` varchar(20) NOT NULL,
    `taxno` integer NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`germrule` ADD CONSTRAINT taxno_refs_taxno_2cbe12a2 FOREIGN KEY (`taxno`) REFERENCES `prod`.`tax` (`taxno`);
CREATE TABLE `prod`.`gnt` (
    `gno` integer NOT NULL PRIMARY KEY,
    `famno` integer NOT NULL PRIMARY KEY,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`gnt` ADD CONSTRAINT gno_refs_gno_1186e496 FOREIGN KEY (`gno`) REFERENCES `prod`.`gn` (`gno`);
ALTER TABLE `prod`.`gnt` ADD CONSTRAINT famno_refs_famno_478f9df0 FOREIGN KEY (`famno`) REFERENCES `prod`.`fam` (`famno`);
CREATE TABLE `prod`.`gob` (
    `gobno` integer NOT NULL PRIMARY KEY,
    `gano` integer NOT NULL UNIQUE,
    `ivid` integer NOT NULL UNIQUE,
    `indiv` integer NULL UNIQUE,
    `gob` varchar(1000) NOT NULL,
    `genbank_link` varchar(200) NOT NULL,
    `image_link` varchar(200) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`gob` ADD CONSTRAINT gano_refs_gano_e7042f6 FOREIGN KEY (`gano`) REFERENCES `prod`.`ga` (`gano`);
ALTER TABLE `prod`.`gob` ADD CONSTRAINT ivid_refs_ivid_a0d48f7 FOREIGN KEY (`ivid`) REFERENCES `prod`.`iv` (`ivid`);
CREATE TABLE `prod`.`grinwin_ini` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `type` varchar(32) NOT NULL,
    `label` varchar(32) NOT NULL,
    `value` varchar(64) NOT NULL
)
;
CREATE TABLE `prod`.`src` (
    `srcno` integer NOT NULL PRIMARY KEY,
    `srctype` varchar(10) NOT NULL,
    `srcdate` date NULL,
    `datefmt` varchar(10) NOT NULL,
    `origin` varchar(1) NOT NULL,
    `acid` integer NOT NULL,
    `geono` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `srcqual` varchar(10) NOT NULL
)
;
ALTER TABLE `prod`.`src` ADD CONSTRAINT acid_refs_acid_6b4bc38e FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`src` ADD CONSTRAINT geono_refs_geono_65f0a3e FOREIGN KEY (`geono`) REFERENCES `prod`.`geo` (`geono`);
CREATE TABLE `prod`.`hab` (
    `srcno` integer NOT NULL PRIMARY KEY,
    `acid` integer NOT NULL,
    `latd` integer NULL,
    `latm` integer NULL,
    `lats` integer NULL,
    `lath` varchar(1) NOT NULL,
    `lond` integer NULL,
    `lonm` integer NULL,
    `lons` integer NULL,
    `lonh` varchar(1) NOT NULL,
    `elev` integer NULL,
    `quant` integer NULL,
    `units` varchar(2) NOT NULL,
    `cform` varchar(2) NOT NULL,
    `plants` integer NULL,
    `locality` varchar(240) NOT NULL,
    `habitat` varchar(240) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `gctype` varchar(10) NOT NULL,
    `gstype` varchar(10) NOT NULL
)
;
ALTER TABLE `prod`.`hab` ADD CONSTRAINT srcno_refs_srcno_70d4256e FOREIGN KEY (`srcno`) REFERENCES `prod`.`src` (`srcno`);
ALTER TABLE `prod`.`hab` ADD CONSTRAINT acid_refs_acid_5adec8e9 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
CREATE TABLE `prod`.`iact` (
    `iactno` integer NOT NULL PRIMARY KEY,
    `action` varchar(10) NOT NULL,
    `occurred` date NULL,
    `datefmt` varchar(10) NOT NULL,
    `quant` integer NULL,
    `units` varchar(2) NOT NULL,
    `iform` varchar(2) NOT NULL,
    `ivid` integer NOT NULL,
    `cno` integer NULL,
    `eno` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `iactqual` varchar(10) NOT NULL
)
;
ALTER TABLE `prod`.`iact` ADD CONSTRAINT ivid_refs_ivid_7db19e21 FOREIGN KEY (`ivid`) REFERENCES `prod`.`iv` (`ivid`);
ALTER TABLE `prod`.`iact` ADD CONSTRAINT cno_refs_cno_1699beb5 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`iact` ADD CONSTRAINT eno_refs_eno_6a066232 FOREIGN KEY (`eno`) REFERENCES `prod`.`eval` (`eno`);
CREATE TABLE `prod`.`ig` (
    `igname` varchar(100) NOT NULL PRIMARY KEY,
    `site` integer NOT NULL PRIMARY KEY,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`ig` ADD CONSTRAINT site_refs_id_238e95e6 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
CREATE TABLE `prod`.`igm` (
    `ivid` integer NOT NULL PRIMARY KEY,
    `igname` varchar(100) NOT NULL PRIMARY KEY,
    `site` varchar(100) NOT NULL PRIMARY KEY,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`igm` ADD CONSTRAINT ivid_refs_ivid_1ad2da40 FOREIGN KEY (`ivid`) REFERENCES `prod`.`iv` (`ivid`);
ALTER TABLE `prod`.`igm` ADD CONSTRAINT igname_refs_igname_19db150d FOREIGN KEY (`igname`) REFERENCES `prod`.`ig` (`igname`);
ALTER TABLE `prod`.`igm` ADD CONSTRAINT site_refs_igname_19db150d FOREIGN KEY (`site`) REFERENCES `prod`.`ig` (`igname`);
CREATE TABLE `prod`.`ipr` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `acid` integer NOT NULL UNIQUE,
    `iprtype` varchar(10) NOT NULL UNIQUE,
    `iprid` varchar(40) NOT NULL UNIQUE,
    `iprno` integer NULL,
    `iprcrop` varchar(60) NOT NULL,
    `iprname` varchar(240) NOT NULL,
    `issued` date NULL,
    `expired` date NULL,
    `cno` integer NULL,
    `citno` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `site` integer NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `accepted` date NULL,
    `expected` date NULL
)
;
ALTER TABLE `prod`.`ipr` ADD CONSTRAINT acid_refs_acid_656030c9 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`ipr` ADD CONSTRAINT cno_refs_cno_3f0c6cb6 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`ipr` ADD CONSTRAINT site_refs_id_7ee4b530 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`ipr` ADD CONSTRAINT citno_refs_citno_48717d12 FOREIGN KEY (`citno`) REFERENCES `prod`.`acit` (`citno`);
CREATE TABLE `prod`.`mbr` (
    `cno` integer NOT NULL PRIMARY KEY,
    `cgid` varchar(20) NOT NULL PRIMARY KEY,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `localid` varchar(10) NOT NULL
)
;
ALTER TABLE `prod`.`mbr` ADD CONSTRAINT cno_refs_cno_42eff19c FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`mbr` ADD CONSTRAINT cgid_refs_cgid_626847f7 FOREIGN KEY (`cgid`) REFERENCES `prod`.`cg` (`cgid`);
CREATE TABLE `prod`.`mcit` (
    `citno` integer NOT NULL PRIMARY KEY,
    `mrkno` integer NOT NULL,
    `abbr` varchar(20) NULL,
    `cittitle` varchar(240) NOT NULL,
    `author` varchar(240) NOT NULL,
    `cityr` integer NULL,
    `citref` varchar(60) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`mcit` ADD CONSTRAINT mrkno_refs_mrkno_798b504b FOREIGN KEY (`mrkno`) REFERENCES `prod`.`mrk` (`mrkno`);
ALTER TABLE `prod`.`mcit` ADD CONSTRAINT abbr_refs_abbr_472caa7c FOREIGN KEY (`abbr`) REFERENCES `prod`.`lit` (`abbr`);
CREATE TABLE `prod`.`menu_item` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `menu_id` integer NULL,
    `seqno` integer NULL,
    `type` varchar(32) NOT NULL,
    `module` varchar(64) NOT NULL,
    `path` varchar(64) NOT NULL,
    `item` varchar(64) NOT NULL,
    `hint` varchar(64) NOT NULL,
    `arg` varchar(254) NOT NULL
)
;
CREATE TABLE `prod`.`menu_name` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `menu_id` integer NULL,
    `menu_name` varchar(32) NOT NULL,
    `user_id` varchar(32) NOT NULL,
    `parent` varchar(32) NOT NULL,
    `title` varchar(64) NOT NULL
)
;
CREATE TABLE `prod`.`narr` (
    `acid` integer NOT NULL PRIMARY KEY,
    `ntype` varchar(1) NOT NULL PRIMARY KEY,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `narr` varchar(4000) NOT NULL
)
;
ALTER TABLE `prod`.`narr` ADD CONSTRAINT acid_refs_acid_275bec38 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
CREATE TABLE `prod`.`oact` (
    `oactno` integer NOT NULL PRIMARY KEY,
    `action` varchar(10) NOT NULL,
    `acted` date NOT NULL,
    `actid` varchar(40) NOT NULL,
    `orno` integer NOT NULL,
    `site` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `coop` varchar(100) NOT NULL,
    `cno` integer NULL
)
;
ALTER TABLE `prod`.`oact` ADD CONSTRAINT orno_refs_orno_123d994 FOREIGN KEY (`orno`) REFERENCES `prod`.`ord` (`orno`);
ALTER TABLE `prod`.`oact` ADD CONSTRAINT site_refs_id_86ed391 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
CREATE TABLE `prod`.`qual` (
    `qno` integer NOT NULL PRIMARY KEY,
    `qual` varchar(30) NOT NULL,
    `dno` integer NOT NULL,
    `def` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`qual` ADD CONSTRAINT dno_refs_dno_11ebe29d FOREIGN KEY (`dno`) REFERENCES `prod`.`dsc` (`dno`);
CREATE TABLE `prod`.`ob` (
    `dno` integer NOT NULL UNIQUE,
    `ob` varchar(30) NOT NULL UNIQUE,
    `acid` integer NOT NULL UNIQUE,
    `eno` integer NOT NULL UNIQUE,
    `qno` integer NULL UNIQUE,
    `ivid` integer NULL UNIQUE,
    `orgvalue` varchar(30) NOT NULL,
    `freq` double precision NULL,
    `mean` integer NULL,
    `high` integer NULL,
    `low` integer NULL,
    `sdev` integer NULL,
    `ssize` integer NULL,
    `cmt` varchar(500) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `obno` integer NOT NULL PRIMARY KEY,
    `rank` integer NULL
)
;
ALTER TABLE `prod`.`ob` ADD CONSTRAINT dno_refs_dno_74ebc893 FOREIGN KEY (`dno`) REFERENCES `prod`.`dsc` (`dno`);
ALTER TABLE `prod`.`ob` ADD CONSTRAINT acid_refs_acid_12d1a476 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`ob` ADD CONSTRAINT ivid_refs_ivid_6d077927 FOREIGN KEY (`ivid`) REFERENCES `prod`.`iv` (`ivid`);
ALTER TABLE `prod`.`ob` ADD CONSTRAINT eno_refs_eno_7ff92454 FOREIGN KEY (`eno`) REFERENCES `prod`.`eval` (`eno`);
ALTER TABLE `prod`.`ob` ADD CONSTRAINT qno_refs_qno_5d4211e3 FOREIGN KEY (`qno`) REFERENCES `prod`.`qual` (`qno`);
CREATE TABLE `prod`.`oi` (
    `orno` integer NULL UNIQUE,
    `oino` integer NULL UNIQUE,
    `item` varchar(40) NOT NULL,
    `quant` integer NULL,
    `units` varchar(2) NOT NULL,
    `dform` varchar(2) NOT NULL,
    `rest` varchar(10) NOT NULL,
    `status` varchar(10) NOT NULL,
    `acted` date NULL,
    `cno` integer NULL,
    `ivid` integer NULL,
    `acid` integer NULL,
    `taxno` integer NULL,
    `taxon` varchar(100) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `itno` integer NOT NULL PRIMARY KEY
)
;
ALTER TABLE `prod`.`oi` ADD CONSTRAINT orno_refs_orno_2b0c52fb FOREIGN KEY (`orno`) REFERENCES `prod`.`ord` (`orno`);
ALTER TABLE `prod`.`oi` ADD CONSTRAINT cno_refs_cno_7718e88 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`oi` ADD CONSTRAINT ivid_refs_ivid_7092c39c FOREIGN KEY (`ivid`) REFERENCES `prod`.`iv` (`ivid`);
ALTER TABLE `prod`.`oi` ADD CONSTRAINT taxno_refs_taxno_6278301f FOREIGN KEY (`taxno`) REFERENCES `prod`.`tax` (`taxno`);
ALTER TABLE `prod`.`oi` ADD CONSTRAINT acid_refs_acid_215b139 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
CREATE TABLE `prod`.`ped` (
    `acid` integer NOT NULL PRIMARY KEY,
    `released` date NULL,
    `datefmt` varchar(10) NOT NULL,
    `citno` integer NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `pedigree` varchar(2000) NOT NULL
)
;
ALTER TABLE `prod`.`ped` ADD CONSTRAINT acid_refs_acid_f73f165 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`ped` ADD CONSTRAINT citno_refs_citno_3b688f2e FOREIGN KEY (`citno`) REFERENCES `prod`.`acit` (`citno`);
CREATE TABLE `prod`.`pt` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `ivid` integer NOT NULL UNIQUE,
    `pttype` varchar(10) NOT NULL UNIQUE,
    `ptcode` varchar(10) NOT NULL UNIQUE,
    `began` date NULL,
    `finished` date NULL UNIQUE,
    `results` varchar(10) NOT NULL,
    `needed` integer NULL,
    `started` integer NULL,
    `completed` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`pt` ADD CONSTRAINT ivid_refs_ivid_26af7288 FOREIGN KEY (`ivid`) REFERENCES `prod`.`iv` (`ivid`);
CREATE TABLE `prod`.`quar` (
    `acid` integer NOT NULL PRIMARY KEY,
    `qtype` varchar(10) NOT NULL PRIMARY KEY,
    `status` varchar(10) NOT NULL,
    `cno` integer NOT NULL,
    `entered` date NULL,
    `establish` date NULL,
    `expected` date NULL,
    `released` date NULL,
    `cmt` varchar(240) NOT NULL,
    `site` integer NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`quar` ADD CONSTRAINT acid_refs_acid_5958c330 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`quar` ADD CONSTRAINT cno_refs_cno_1bb76f51 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
ALTER TABLE `prod`.`quar` ADD CONSTRAINT site_refs_id_65f6a929 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
CREATE TABLE `prod`.`sec_priv` (
    `id` integer NOT NULL PRIMARY KEY,
    `sec_role_id` integer NULL,
    `sec_user_id` integer NULL,
    `table_name` varchar(50) NOT NULL,
    `target_sec_user_id` integer NULL,
    `priv_create` varchar(1) NOT NULL,
    `priv_read` varchar(1) NOT NULL,
    `priv_update` varchar(1) NOT NULL,
    `priv_delete` varchar(1) NOT NULL,
    `enabled` varchar(1) NOT NULL,
    `created_at` datetime NOT NULL,
    `updated_at` datetime NULL
)
;
CREATE TABLE `prod`.`sec_role` (
    `id` integer NOT NULL PRIMARY KEY,
    `role_name` varchar(50) NOT NULL
)
;
CREATE TABLE `prod`.`sec_user` (
    `id` integer NOT NULL PRIMARY KEY,
    `user_name` varchar(50) NOT NULL UNIQUE,
    `password` varchar(255) NOT NULL,
    `enabled` varchar(1) NOT NULL,
    `created_at` datetime NOT NULL,
    `updated_at` datetime NULL
)
;
CREATE TABLE `prod`.`sec_user_role` (
    `id` integer NOT NULL PRIMARY KEY,
    `sec_user_id` integer NOT NULL,
    `sec_role_id` integer NOT NULL
)
;
CREATE TABLE `prod`.`smbr` (
    `srcno` integer NOT NULL PRIMARY KEY,
    `acid` integer NOT NULL,
    `cno` integer NOT NULL PRIMARY KEY,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`smbr` ADD CONSTRAINT srcno_refs_srcno_3b85f5f0 FOREIGN KEY (`srcno`) REFERENCES `prod`.`src` (`srcno`);
ALTER TABLE `prod`.`smbr` ADD CONSTRAINT acid_refs_acid_3ad45147 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`smbr` ADD CONSTRAINT cno_refs_cno_5a7a0346 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
CREATE TABLE `prod`.`sql_operators` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `oper` varchar(12) NOT NULL,
    `seqno` integer NULL
)
;
CREATE TABLE `prod`.`taut` (
    `shortaut` varchar(30) NOT NULL PRIMARY KEY,
    `longaut` varchar(100) NOT NULL,
    `smarkaut` varchar(30) NOT NULL,
    `lmarkaut` varchar(100) NOT NULL,
    `shexpaut` varchar(30) NOT NULL,
    `lgexpaut` varchar(100) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NOT NULL,
    `modified` date NULL
)
;
CREATE TABLE `prod`.`tcit` (
    `citno` integer NOT NULL PRIMARY KEY,
    `taxno` integer NOT NULL,
    `abbr` varchar(20) NULL,
    `cittitle` varchar(240) NOT NULL,
    `author` varchar(240) NOT NULL,
    `cityr` integer NULL,
    `citref` varchar(60) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`tcit` ADD CONSTRAINT taxno_refs_taxno_5fd6e619 FOREIGN KEY (`taxno`) REFERENCES `prod`.`tax` (`taxno`);
ALTER TABLE `prod`.`tcit` ADD CONSTRAINT abbr_refs_abbr_36d45f05 FOREIGN KEY (`abbr`) REFERENCES `prod`.`lit` (`abbr`);
CREATE TABLE `prod`.`turl` (
    `turlno` integer NOT NULL PRIMARY KEY,
    `urltype` varchar(10) NOT NULL,
    `famno` integer NOT NULL,
    `gno` integer NULL,
    `taxno` integer NULL,
    `caption` varchar(240) NOT NULL,
    `url` varchar(100) NOT NULL,
    `site` integer NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NOT NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`turl` ADD CONSTRAINT famno_refs_famno_3e0feed5 FOREIGN KEY (`famno`) REFERENCES `prod`.`fam` (`famno`);
ALTER TABLE `prod`.`turl` ADD CONSTRAINT gno_refs_gno_2601597b FOREIGN KEY (`gno`) REFERENCES `prod`.`gn` (`gno`);
ALTER TABLE `prod`.`turl` ADD CONSTRAINT site_refs_id_33f62b7 FOREIGN KEY (`site`) REFERENCES `main`.`site` (`id`);
ALTER TABLE `prod`.`turl` ADD CONSTRAINT taxno_refs_taxno_4e439b58 FOREIGN KEY (`taxno`) REFERENCES `prod`.`tax` (`taxno`);
CREATE TABLE `prod`.`uses` (
    `taxno` integer NOT NULL PRIMARY KEY,
    `taxuse` varchar(10) NOT NULL PRIMARY KEY,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `usetype` varchar(250) NOT NULL
)
;
ALTER TABLE `prod`.`uses` ADD CONSTRAINT taxno_refs_taxno_645fcaaf FOREIGN KEY (`taxno`) REFERENCES `prod`.`tax` (`taxno`);
CREATE TABLE `prod`.`via` (
    `viano` integer NOT NULL PRIMARY KEY,
    `tested` date NOT NULL,
    `datefmt` varchar(10) NOT NULL,
    `norm` integer NULL,
    `abnorm` integer NULL,
    `dormant` integer NULL,
    `viable` integer NULL,
    `vigor` varchar(10) NOT NULL,
    `sample` integer NULL,
    `reps` integer NULL,
    `ivid` integer NOT NULL,
    `eno` integer NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL
)
;
ALTER TABLE `prod`.`via` ADD CONSTRAINT ivid_refs_ivid_7fba40c7 FOREIGN KEY (`ivid`) REFERENCES `prod`.`iv` (`ivid`);
ALTER TABLE `prod`.`via` ADD CONSTRAINT eno_refs_eno_93591a FOREIGN KEY (`eno`) REFERENCES `prod`.`eval` (`eno`);
CREATE TABLE `prod`.`vou` (
    `acid` integer NOT NULL UNIQUE,
    `vtype` varchar(10) NOT NULL UNIQUE,
    `ivid` integer NULL UNIQUE,
    `cno` integer NULL UNIQUE,
    `vouchered` date NULL UNIQUE,
    `datefmt` varchar(10) NOT NULL,
    `collid` varchar(40) NOT NULL UNIQUE,
    `vloc` varchar(500) NOT NULL UNIQUE,
    `vcontent` varchar(100) NOT NULL,
    `cmt` varchar(240) NOT NULL,
    `userid` varchar(10) NOT NULL,
    `created` date NULL,
    `modified` date NULL,
    `thumbnail` varchar(500) NOT NULL,
    `vno` integer NOT NULL PRIMARY KEY
)
;
ALTER TABLE `prod`.`vou` ADD CONSTRAINT acid_refs_acid_42a7c5b2 FOREIGN KEY (`acid`) REFERENCES `prod`.`acc` (`acid`);
ALTER TABLE `prod`.`vou` ADD CONSTRAINT ivid_refs_ivid_6045c89d FOREIGN KEY (`ivid`) REFERENCES `prod`.`iv` (`ivid`);
ALTER TABLE `prod`.`vou` ADD CONSTRAINT cno_refs_cno_643f6d01 FOREIGN KEY (`cno`) REFERENCES `prod`.`coop` (`cno`);
COMMIT;
