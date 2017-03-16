<?php

function zapis($dane) { 
    $file = "baza.txt"; 
    $fp = fopen($file, "w"); 
    flock($fp, 2); 
    fwrite($fp, $dane); 
    flock($fp, 3); 
    fclose($fp); 
}