<?
require_once('db.php');

$maize_config = array();
$maize_config["webservice_url"] = "http://mbp/maizegdb/maizegdb.asmx?WSDL";

// db settings
$maize_config["DB_CONNECTION_STRING"] = "mysql:host=mbp;port=3306;dbname=maize_test_db";
$maize_config["DB_USER"] = "test_user";
$maize_config["DB_PASSWORD"] = "passw0rd!";

db_init($maize_config);

function maize_setting($key){
	global $maize_config;
	return $maize_config[$key];
}

?>