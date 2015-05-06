<?
require_once('util.php');
include_once('db_config.php');

class DB {
	var $conn;
	var $stmt;
	
	var $conn_str;
	var $user;
	var $password;
	
	function DB($arr=null){
			// NOTE: to override the connection values, edit db_config.php or call register!!!
			// keys that are required are as follows:
			// DB_CONNECTION_STRING
			// DB_USER
			// DB_PASSWORD
			if ($arr == null){
				$arr = $GLOBALS;
			}
			$this->register($arr);
	}
	
	function register($arr){
		$this->conn_str = @$arr['DB_CONNECTION_STRING'];
		$this->user = @$arr['DB_USER'];
		$this->password = @$arr['DB_PASSWORD'];
		$this->init();
		return $this;
	}

	function init(){
		try {
			// if a connection exists, close it
			if ($this->conn != null){
				$this->conn->close();
				unset($this->conn);
			}
			// echo "|{$this->conn_str}|<hr />"; echo "|{$this->user}|<hr />"; echo "|{$this->password}|<hr />";
			$this->conn = new PDO($this->conn_str, $this->user, $this->password);
		} catch (PDOException $e){
			print "Error connecting to db: " . $e->getMessage() . "<br />";
			die();
		}
	}
	
	function exec($sql, $params=null, $addl_params=null, $debug=false){
		try {
			if ($params == null){
				$params = $addl_params;
			} else if ($addl_params != null){
				$params = $params + $addl_params;
			}
			
			$inserting = false;
			$updating = false;
			$deleting = false;
			if (stripos($sql, 'insert into') !== FALSE){
				// we're inserting. append sql to grab the inserted id.
				$inserting = true;
			} else if (stripos(ltrim($sql), 'update ') !== FALSE){
				$updating = true;
			} else if (stripos(ltrim($sql), 'delete from') !== FALSE){
				$deleting = true;
			}
			$this->stmt = $this->conn->prepare($sql);
			if ($params != null && is_array($params)){
				foreach($params as $key => $val){
					if (stripos($key, '_multivalue') !== FALSE && is_array($val)){
						// multivalue field. write the count instead of the 'real' value.
						$temp = count($val);
					} else {
						if (is_array($val)){
							$temp = $val[0];
						} else {
							$temp = $val;
						}
					}
					
//debug($temp, "appending param name={$key}, value:");
					$this->stmt->bindValue($key, $temp);
				}
			}
			if ($debug){
				debug($sql, 'executing sql...');
				debug($params, 'with params');
			}
			$ret = $this->stmt->execute();
			if ($ret){
				if ($inserting){
					// grab the last inserted id
					return $this->conn->lastInsertId();
				} else if ($updating) {
					// return # rows affected
					return $ret;
				} else if ($deleting){
					// return # rows affected
					return $ret;
				} else {
					// just fetch all rows, let them treat it as an indexed array of hashes
					$rows = $this->stmt->fetchAll();
					unset($this->stmt);
					return $rows;
				}
			} else {
				// for more info about error codes for ODBC connections...
				// http://www.easysoft.com/developer/interfaces/odbc/sqlstate_status_return_codes.html
				$errinfo = $this->stmt->errorInfo();
				debug($errinfo, "SQL error " . $this->stmt->errorCode() . ":  SQL: --- {$sql} ---.  Add'l info:");
				debug($params, "Params given to previous sql:");
				unset($this->stmt);
				die();
			}
		} catch (PDOException $e){
			print "Error running sql '{$sql}' w/ params=" . print_r($params) . ": " . $e.getMessage() . "<br />";
			die();
		}
	}
	
	// params should be generated from generate_params
	function generate_insert_clause($params){
		$field_names = '';
		$param_names = '';
		foreach($params as $key => $val){
			$nm = $key;
			if (stripos($nm, ':') !== FALSE) {
				$nm = substr($key, 1);
			}
			switch($nm){
				default:
					$field_names .= "{$nm}, ";
					$param_names .= ":{$nm}, ";
					break;
			}
		}
		// trim off trailing ', ' from both
		$field_names = substr($field_names, 0, -2);
		$param_names = substr($param_names, 0, -2);
		
		$ret = "({$field_names}) VALUES ({$param_names})";
		return $ret;
	}
	
	function generate_update_clause($params){
		$ret = '';
		foreach($params as $key => $val){
			if (is_numeric($key)){
				$nm = $val;
			} else {
				$nm = $key;
			}
			if (stripos($nm, ':') !== FALSE){
				$nm = substr($nm, 1);
			}
			$ret .= "{$nm} = :{$nm}, ";
		}
		// trim off trailing ', '
		$ret = substr($ret, 0, -2);
		return $ret;
	}
	
	function generate_where_clause($params){
		$ret = '';
		foreach($params as $key => $val){
			if (is_numeric($key)){
				$nm = $val;
			} else {
				$nm = $key;
			}
			if (stripos($nm, ':') !== FALSE){
				$nm = substr($nm, 1);
			}
			$ret .= "{$nm} = :{$nm} and ";
		}
		// trim off trailing ' and '
		$ret = substr($ret, 0, -5);
		return $ret;
	}
	
	// all_keys_and_values should be a hash, desired_keys a indexed array
	function generate_params($all_keys_and_values, $desired_keys=null){
		$ret = array();
//debug($all_keys_and_values, 'db->generate_params - all_keys_and_values:');
//debug($desired_keys, 'db->generate_params - desired_keys:');
		if ($desired_keys == null){
			// copy in everything
			$desired_keys = array_keys($all_keys_and_values);
		}
		foreach($desired_keys as $key => $val){
			if (is_numeric($key)){
				$nm = $val;
			}
			if (stripos($nm, ':') !== FALSE){
				$nm = substr($nm, 1);
			}
			// okay, $nm does NOT contain a colon...
			if (array_key_exists($nm, $all_keys_and_values)){
				// all_keys_and_values does not include the : in the key name
				$val = $all_keys_and_values[$nm];
			} else {
				// assume the : is in the key name, look up value from array w/ it
				$val = @$all_keys_and_values[":{$nm}"];
			}
			if (is_array($val)){
				if (count($val) > 2){
					// NOTE: if it's a proper array, it'll have 2 values minimum -- the first value and the count.
					// multivalue. just send in the count.
					$ret[$nm] = count($val) - 1;
				} else {
					// single valued array. copy in the first value.
					$ret[$nm] = $val[0];
				}
			} else {
				// scalar. copy in that value.
				$ret[$nm] = $val;
			}
			
			// special cases...
			$ret[$nm] = $this->sqlize_value($nm, $ret[$nm]);

			if ($ret[$nm] === (string)'FALSE'){
				$ret[$nm] = false;
			} else if ($ret[$nm] === (string)'TRUE') {
				$ret[$nm] = true;
			}
			
//debug($ret[$nm], "added to return value for index {$nm}:");
		}
		return $ret;
	}
	
	function sqlize_value($key, $val){
		$ret = null;
		switch($key){
			case 'created_at':
				$ret = date_fmt($val, true, true);
				break;
			case 'whencreated':
			case 'createtimestamp':
			case 'whenchanged':
			case 'modifytimestamp':
				$ret = date_fmt($val, false);
				break;
			default:
				$ret = $val;
				break;
		}
		return $ret;
	}
	
	function new_id($tbl, $id){
		if (isset($id) && is_numeric($id) && $id > 0){
			return $id;
		} else {
			// TODO: figure out how to correctly generate a valid sync id
			$row = $this->exec("select next_id from db_id_gen where table_name = :table_name", array('table_name' => $tbl));
			if ($row == null){
				$this->exec("insert into db_id_gen (table_name, next_id) VALUES (:table_name, :next_id)", array('table_name' => $tbl, "next_id" => 2));
				return 1;
			} else {
				$ret = $row[0]['next_id'];
				$this->exec("update db_id_gen set next_id = next_id + 1 where table_name = :table_name", array('table_name' => $tbl));
				return $ret;
			}
		}
	}
	
	function close(){
		if (isset($this->stmt)){
			unset($this->stmt);
		}
		if (isset($this->conn)){
			unset($this->conn);
		}
	}
}

// if given, $arr must be a hashmap that contains DB_CONNECTION_STRING, DB_USER, and DB_PASSWORD
function db_init($arr=null){
	if ($arr != null){
		$GLOBALS['db'] = new DB($arr);
	}
	if (!isset($GLOBALS['db'])){
		$GLOBALS['db'] = new DB();
	}
	return $GLOBALS['db'];
}
function db_close(){
	if (isset($GLOBALS['db'])){
		db_init()->close();
		unset($GLOBALS['db']);
	}
}

function db_exec($sql, $params=null, $addl_params=null){
	return db_init()->exec($sql, $params, $addl_params);
}
function db_param_array($all_keys_and_values, $desired_keys=null){
	return db_init()->generate_params($all_keys_and_values, $desired_keys);
}
function db_update_clause($params){
	return db_init()->generate_update_clause($params);
}
function db_insert_clause($params){
	return db_init()->generate_insert_clause($params);
}
function db_where_clause($params){
	return db_init()->generate_where_clause($params);
}
function db_sqlize_value($key, $val){
	return db_init()->sqlize_value($key, $val);
}
?>