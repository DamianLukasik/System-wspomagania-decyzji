<?php

if(isset($_POST['naz']))
{
    $xml = file_get_contents('http://www.nbp.pl/kursy/xml/LastA.xml');
    
    $elements = array('nazwa_waluty', 'przelicznik', 'kod_waluty', 'kurs_sredni'); 
 
    for ($i = 0; ; $i++)
    {
      foreach($elements as $element)
      {
        if (($pos = strpos($xml, '<' . $element . '>', $pos)) !== false)
        $kursy[$i][$element] = substr($xml, $p = $pos+strlen($element)+2, ($pos = strpos($xml, '</' . $element . '>', $p))-$p);
        else 
        break 2;
      }
    }
	
	echo 'X';
}