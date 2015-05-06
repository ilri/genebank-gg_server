<?php

function date_fmt($dt = null, $current_if_null=true, $military=false){
	if ($dt == null || !isset($dt)){
		if ($current_if_null){
			$dt = time();
		} else {
			return null;
		}
	} else if (strpos($dt, '.') !== FALSE){
		
		// we can get this back from AD for timestamp:
		// 20071226211722.0Z
		// so handle that .0Z portion correctly...,
		$pos = strpos($dt, '.');
		//$tz = substr($dt, $pos + 1, strlen($dt) - $pos -2 );
		$dt = strtotime(substr($dt, 0, $pos));
		
		// HACK: assuming we're always in central time zone...
		$dt -= (6 * 3600);
	} else if (strpos($dt, '-') !== FALSE || strpos($dt, '/') !== FALSE){
		// formatted as 2008-01-02...
		$dt = strtotime($dt);
	}
	if ($military){
		return strftime('%Y/%m/%d %H:%M:%S', $dt);
	} else {
		return strftime('%Y/%m/%d %I:%M:%S %p', $dt);
	}
}


function debug($obj, $title='-', $html_encode=false){

	// if config file says to ignore errors, suppress debugging info as well
	$display_errors = get_cfg_var('display_errors');
	if (!$display_errors){
		return;
	}

	$out = print_r($obj, true);
	if ($html_encode){
		$out = html_encode($out);
	}
	echo "<fieldset><legend>{$title}</legend><pre>{$out}</pre></fieldset>";
	return $out;
}

function html_encode($html){
	$output = str_replace('<', '&lt;', 
					str_replace('>', '&gt;',
						str_replace("'", '&apos;', 
							str_replace('"', '&quot;', 
								str_replace('&', '&amp;', $html)))));
	return $output;
}

function html_decode($html){
	$output = str_replace('&lt;', '<', 
					str_replace('&gt;', '>',
						str_replace('&apos;', "'", 
							str_replace('&quot;', '"', 
								str_replace('&amp;', '&', $html)))));
	return $output;
}

?>