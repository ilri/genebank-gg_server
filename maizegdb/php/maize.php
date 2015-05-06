<?php
require_once('util.php');
require_once('soapclientbase.php');
require_once('maize_config.php');

require_once('db.php');

class MaizeGDB extends SoapClientBase {

	function __construct(){
		parent::__construct(maize_setting('webservice_url'));
	}
	/*
	function TestDataTable($ACP=null){
		$ret_val = $this->exec("TestDataTable", array('acp' => $ACP)); 
		debug($ret_val, 'datatable');
		return $ret_val['Table0'];
	}
	function TestDataSet($ACP=null){
		$ret_val = $this->exec("TestDataSet", array('acp' => $ACP)); 
		debug($ret_val, 'dataset');
		return $ret_val['Table0'];
	}
	*/
	function etl_query(){
		$ret_val = $this->exec("ETLQuery");
//		debug($ret_val, 'ETL Query results');
		return $ret_val['STOCK_GRIN_AVAILABLE'];
	}
	/*
	function etl_list_non_unique_acids(){
		$ret_val = $this->exec("ETLNonUniqueAcids");
		debug($ret_val, 'ETL Non Unique Acids');
		return $ret_val['etl_non_unique_acids'];
	}
	*/
	
/*
 stock_grin
Name            Null?    Type
--------------- -------- -------------
STOCK_ID                 NUMBER(10)
AUTO_NUM        NOT NULL NUMBER(10)
AC_ID           NOT NULL NUMBER(7)
AC_NO           NOT NULL NUMBER(6)
AC_P            NOT NULL VARCHAR2(4)
GENUS           NOT NULL VARCHAR2(30)
PLANT_ID        NOT NULL VARCHAR2(60)
SEARCH_ID       NOT NULL VARCHAR2(40)
SITE            NOT NULL VARCHAR2(4)
ACS                      VARCHAR2(4)
AC_IMPT                  VARCHAR2(10
AG_NAME                  VARCHAR2(20
COUNTRY                  VARCHAR2(30)
STATE                    VARCHAR2(20)
TOP_NAME                 VARCHAR2(1)
UNIFORM                  VARCHAR2(10)
                                   
                                   
stock_grin_available               
Name            Null?    Type      
--------------- -------- -------------
AC_ID           NOT NULL NUMBER(8) 
ACP             NOT NULL VARCHAR2(6)
AC_NO           NOT NULL NUMBER(8)
ACS                      VARCHAR2(4)
D_QUANT                  NUMBER(6)
*/
	function etl_add($row){
		$prms = db_param_array(array (':AC_ID' => $row['ACID'],
									':ACP' => $row['ACP'],
									':AC_NO' => $row['ACNO'],
									':ACS' => $row['ACS'],
									':D_QUANT' => $row['DQUANT_MAX']));
		$ic = db_insert_clause($prms);
		$sql = "INSERT INTO STOCK_GRIN_AVAILABLE {$ic}";
		//debug($prms, 'parameters');
		//debug($sql, 'sql');
		db_exec($sql, $prms);
		// this table has no identity, so just return AC_ID as it is the PK
		return $row['ACID'];
	}
	
	function etl_clean(){
		db_exec("delete from STOCK_GRIN_AVAILABLE");
	}
	
	
	function etl_run(){
	
		// grab data from GRIN web service
		$rows = $this->etl_query();
		
		// delete all existing data from  table
		$this->etl_clean();
		
		// add the new data
		foreach($rows as $row){
 			// save each row to maizegdb
			$newid = $this->etl_add($row);
			debug('inserted id=' . $newid, 'inserted row');
		}
		
	}
}

?>