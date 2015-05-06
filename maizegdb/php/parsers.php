<?php
abstract class BaseParser {
	
	protected $output = array();
	
	private $parser;
	private $xmldata;
	private $nodename;
	private $insertedNode = 'dummy_baseparser_node';

	public function parse($xml_or_arr) {
	
		if (is_array($xml_or_arr)){
			$output = array();
			foreach($xml_or_arr as $val){
				$to = $this->parse($val);
				$output = array_merge($output, $to);
			}
			return $output;
		}
		
//		echo '<h1>xml in parser:</h1><pre>'; print_r($xml); echo '</pre>';

		// if we don't start with a processing instruction,
		// we add an outer node just to ensure it's a valid xml document
		// (i.e. only a single root node)
		if (substr($xml_or_arr,0,2) != '<?'){
			$xml_or_arr = "<{$this->insertedNode}>" . $xml_or_arr . "</{$this->insertedNode}>";
		}
		
	    $this->parser = xml_parser_create();
	    xml_set_object($this->parser,$this);
	    xml_set_element_handler($this->parser, "tagOpen", "tagClosed");
	    xml_parser_set_option($this->parser, XML_OPTION_CASE_FOLDING, 0);
	    xml_parser_set_option($this->parser, XML_OPTION_SKIP_WHITE, 1);
	    xml_set_character_data_handler($this->parser, "tagData");

	    $this->xmldata = xml_parse($this->parser,$xml_or_arr );
	    if(!$this->xmldata) {
	       die(sprintf("XML error: %s at line %d",
	    xml_error_string(xml_get_error_code($this->parser)),
	    xml_get_current_line_number($this->parser)));
	    }
	    
	    xml_parser_free($this->parser);
	    
	    $this->parseCompleted();

	    return $this->output;

	}

	// overrideable functions	
	protected function nodeOpen($parser, $name, $attrs){}
	protected function nodeData($parser, $name, $nodeData){}
	protected function nodeClosed($parser, $name){}
	protected function parseCompleted(){}

	private function tagOpen($parser, $name, $attrs) {
		if ($name != $this->insertedNode){
			$this->nodename = $name;
			$this->nodeOpen($parser, $name, $attrs);
		}
	}
	
	private function tagData($parser, $tagData) {
		if ($this->nodename != $this->insertedNode){
			$this->nodeData($parser, $this->nodename, $tagData);
		}
	}
	
	private function tagClosed($parser, $name) {
		if ($name != $this->insertedNode){
			$this->nodename = null;
			$this->nodeClosed($parser, $name);
		}
	}
}

class ScalarParser extends BaseParser {
	function nodeData($parser, $name, $nodeData) {
		if ($name != null){
			$this->output = $nodeData;
		}
	}
}

class ScalarArrayParser extends BaseParser {
	function nodeData($parser, $name, $nodeData) {
		if ($name != null) {
			$pos = strpos($name, 'ArrayOf');
			if ($pos === false){
				$this->output[] = $nodeData;
			}
		}
	}
}

class DictionaryParser extends BaseParser {
	private $array_output = array();
	private $key;
	private $is_array;
	
	function nodeData($parser, $name, $nodeData) {
		if (stripos($name, 'ArrayOf') > -1){
			$this->is_array = true;
		}
		if ($name == 'string'){
			$this->key = $nodeData;
			// default the node data to empty string just in case it's an empty node
			// (we always want to add the key even if it's empty)
			$this->output[$this->key] = '';
		} else if ($name == 'anyType'){
			$this->output[$this->key] = $nodeData;
		}
	}

	function nodeClosed($parser, $name) {
		if ($name == 'SerializableDictionary'){
			if ($this->is_array){
				$this->array_output[] = $this->output;
				$this->output = array();
			}
		}

	}
	
	function parseCompleted(){
		if ($this->is_array){
			$this->output = $this->array_output;
		}
	}
}


class DataSetParser extends BaseParser {
	private $rowData = array();

	private $key;
	private $value;
	private $is_array;

	private $foundDSDef = false;
	private $foundTableDef = false;
	private $foundColumnDef = false;
	private $foundDiffgram = false;
	private $curTableName;

	private $pullingData = false;
	private $columnNames = array();
	private $tableSchemas = array();
	private $curColumnName;

	function nodeOpen($parser, $name, $attrs) {
		
		if ($this->pullingData){
			// we are in data slurping mode!
			if ($name != null){
				$this->curColumnName = $name;
			}
		} else {
			// we are either in the schema definition portion
			// or preparing to slurp data
			if ($name == 'xs:element'){
				if ($this->foundDSDef){
					// still in the dataset definition
					if (!array_key_exists('type', $attrs)){
						// defining the table name.
						$this->foundTableDef = true;
						$this->curTableName = $attrs['name'];
						$this->tableSchemas[$this->curTableName] = array();
					} else {
						// definining a column
						$this->foundColumnDef = true;
						$this->tableSchemas[$this->curTableName][] = $attrs['name'];
					}
				} else if (array_key_exists('msdata:IsDataSet', $attrs)){
					// this is describing the DataSet. nothing to save except remember that the DataSet definition started.
					$this->foundDSDef = true;
				}
			} else {
				if ($this->foundDiffgram){
					// begin slurping data as we're now in the data portion
					if (array_key_exists($name, $this->tableSchemas)){
						// we found a node that matches one of our table schemas.
						// copy the schema in from the table schemas array
						$this->rowData = array();
						foreach($this->tableSchemas[$name] as $offset => $colName){
							$this->rowData[$colName] = '';
						}

						$this->curTableName = $name;
						$this->pullingData = true;
					} else {
					}
				} else if ($name == 'diffgr:diffgram'){
					$this->foundDiffgram = true;
				}
			}

		}
	}

	function nodeData($parser, $name, $nodeData) {
		// note nodeData is also called for #text nodes!
		if ($this->pullingData){
			if (isset($this->rowData) && $name == $this->curColumnName){
				if (array_key_exists($this->curColumnName, $this->rowData)){
					$newdata = $this->rowData[$this->curColumnName] . rtrim($nodeData);
				} else {
					$newdata = rtrim($nodeData);
				}
				$this->rowData[$this->curColumnName] = $newdata;
			}
		}
	}

	function nodeClosed($parser, $name) {
		if ($this->pullingData){
			if ($name == $this->curTableName){
				// done pulling data for now.
				$this->output[$this->curTableName][] = $this->rowData;
				unset($this->rowData);
				$this->pullingData = false;
			}
		} else {
			if ($name == 'xs:element'){
				if ($this->foundColumnDef){
					// closing column definition
					$this->foundColumnDef = false;
				} else if ($this->foundTableDef) {
					// closing Table definition
					$this->foundTableDef = false;
					$this->output[$this->curTableName . '#Columns'] = $this->tableSchemas[$this->curTableName];
					$this->curTableName = null;
				} else if ($this->foundDSDef){
					// closing DataSet definition
					$this->foundDSDef = false;
				}
			} else if ($name == 'xs:sequence') {
				// closing Table schema portion
				$this->pullingTableSchema = false;
			} else if ($name == 'diffgr:diffgram'){
				// closing diffgram
				$this->foundDiffgram = false;
				// if there's any tables w/ no rows, create an empty array for them here
				foreach($this->tableSchemas as $tblName => $cols){
					if (!isset($this->output[$tblName]) || !is_array($this->output[$tblName])){
						$this->output[$tblName] = array();
					}
				}
			}
		}
	}	
}
?>