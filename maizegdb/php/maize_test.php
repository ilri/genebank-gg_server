<?php
require_once('maize.php');

$output = null;
$btn = @$_POST['submit'];
switch ($btn){
	case 'Get Inventory Availability':
		$maize = new MaizeGDB();
		$output = $maize->ETLQuery();
		break;
	case 'Import Inventory':
		$maize = new MaizeGDB();
		$maize->etl_run();
		break;
	case 'List Non Unique Acids':
		$maize = new MaizeGDB();
		$output = $maize->ETLNonUniqueAcids();
		break;
	/*
	case 'List Accessions':
		$acc = @$_POST['accession'];
		$maize = new MaizeGDB();
		$output = $maize->TestDataTable($acc);
		$output = $maize->TestDataSet($acc);
		break;
	*/
}

?>
<html>
	<body>
		<form method='post'>
			<!--
			List Accessions By Code: <input type='text' name='accession' value='<?=@$_POST['accession']?>' />
			<input type='submit' name='submit' value='List Accessions' /><br />
			-->
			<input type='submit' name='submit' value='Get Inventory Availability' /><br />
			<input type='submit' name='submit' value='List Non Unique Acids' /><br />
			<input type='submit' name='submit' value='Import Inventory' /><br />
		</form>
		<?
		if (count($output) > 0){
			echo "<table border='1' cellpadding='2' cellspacing='2'>";
			foreach($output[0] as $col => $val){
				echo "<th>{$col}</th>";
			}
			echo "</tr>\n";
			foreach($output as $row){
				echo "<tr>";
				foreach($row as $name => $value){
					echo "<td>" . ($value == null ? '-' : $value) . "</td>";
				}
				echo "</tr>\n";
			}
			echo "</table>\n";
		} else {
			echo "<div style='border:1px solid green;text-align:center;color:green;padding:5px 5px 5px 5px;'>(No data found)</div>";
		}
		?>
	</body>
</html>