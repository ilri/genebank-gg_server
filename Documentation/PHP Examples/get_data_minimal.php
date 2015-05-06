<?php
/* specify dataview and parameters for it */
$dv = "get_accession";
$params = ":accessionid=382129;:cooperatorid=;:cropid=;:geographyid=;:inventoryid=;:orderrequestid=;:taxonomygenusid=;";

/* get data from web server */
$data = file_get_contents("http://localhost/gringlobal/view.aspx?dv=$dv&params=$params&format=tab");

/* write to local file */
file_put_contents('./output.txt', $data);

?>