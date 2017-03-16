<?php
	
    if(!isset($_SESSION)) 
    {
        session_start(); 
    }	
    
    if(!isset($_SESSION['lektor']))
    {
        $_SESSION['lektor']="1";
        echo $_SESSION['lektor'];
    }
    else
    {
        //echo $_SESSION['lektor'];
    }
    
    if(isset($_GET["glos"]))
    {
	if($_GET['glos']!="0")
        {
            if($_GET['glos']=="2")
            {
                $_SESSION['lektor']="0";
            }
            if($_GET['glos']=="1")
            {
                $_SESSION['lektor']="1";
            }
           // echo $_SESSION['lektor'];
        }
    }
    
    echo $_SESSION['lektor'];