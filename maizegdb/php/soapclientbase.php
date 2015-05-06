<?php
require_once('util.php');
require_once('parsers.php');
class SoapClientBase {
	private $url;
	
	function __construct($url){
		$this->url = $url;
	}

	function _parse($fn, $result){
		$parser = new DataSetParser();
//		debug($result, 'result');
		$tgt = get_object_vars($result);
		$tgt2 = get_object_vars($tgt[$fn . 'Result']);
		$ret_val = $parser->parse($tgt2);
		//debug($ret_val, 'return value');
		return $ret_val;
	}

	function exec($fn, $args=null){
		echo $args;
		$this->client = new SoapClient($this->url);
		$result = $this->client->__soapCall($fn, array($args));
		$output = $this->_parse($fn, $result);
		return $output;
	}
	
}
?>