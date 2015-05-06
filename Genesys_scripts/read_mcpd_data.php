<?php

/* credentials, just give guest/guest for now */
$ggserver = 'localhost';
$ggusername = 'guest';
$ggpassword = 'guest';

/* format of data and which data to retrieve */
$format = 'json';             /* can be tab, csv, json */
$dataview = 'mcpd_dataview';  /* can be name of any dataview defined in GG */
$limit = '0';                 /* limit to X records using this parameter, 0 or empty for no limit */
$offset = '0';                /* skip X records using this parameter, 0 or empty for no skip */
$prettycolumns = '1';         /* 1 to output language-specific column headings, 0 or empty to output dataview-defined column headings */
$params = '';                 /* ":accessionid=3;:cooperatorid=898;..."   for passing parameters to a dataview which expects them, empty for mcpd */

/* we separate out the login call from the data call so the login can be SSL and the data not SSL
   so we don't run all the data through SSL if we don't have to.  Right now (2010/04/16) GG is all
   on port 80 in default installs (i.e. no SSL at all), but this can be overridden by tweaking IIS settings on the server */

/* create a login token using credential information */
$h = fopen("http://$ggserver/gringlobal/streamlogin.aspx?username=$ggusername&password=$ggpassword", 'r');
$tok = fread($h, 8192);
fclose($h);

/* comes back in base64 (which may have + or = in it), so encode it so it's url friendly */
$enctok = urlencode($tok);

/* use that login token to request the data and write to a local file*/
$w = fopen('./output.txt', 'w');
$url = "http://$ggserver/gringlobal/streamdata.aspx?format=$format&dataview=$dataview&token=$enctok&limit=$limit&offset=$offset&prettycolumns=$prettycolumns&params=$params";

$h2 = fopen($url, 'r');
while($data = fread($h2, 8192)){
	fwrite($w, $data);
}
fclose($w);
fclose($h2);

// done, data is in ./output.txt

?>