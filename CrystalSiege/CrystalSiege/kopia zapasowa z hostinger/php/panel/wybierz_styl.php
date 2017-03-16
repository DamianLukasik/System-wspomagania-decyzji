<?php
	
	if(!isset($_SESSION)) 
    {
        session_start(); 
    }
	
	if(!isset($_GET['wybrano_wersje']))
	{
		if($_SESSION['wersja']=="1")
		{
			echo '<link  href="/css/styl.css" rel="stylesheet" />';
		}
		if($_SESSION['wersja']=="2")
		{
			echo '<link  href="/css/styl_2.css" rel="stylesheet" />';
		}
	}
	
	if(isset($_GET['wybrano_wersje']))
	{
		if($_GET['wybrano_wersje']=="1")
		{
			$_SESSION['wersja']="1";
			echo "1";
		}
		if($_GET['wybrano_wersje']=="2")
		{
			$_SESSION['wersja']="2";
			echo "2";
		}
	}
	
	if(!isset($_SESSION['wersja']))
    {
		echo '<link  href="/css/styl.css" rel="stylesheet" />';
		$_SESSION['wersja']="1";
	}
	else
    {
		if(!isset($_GET['wybrano_wersje']))
		{
			if($_SESSION['wersja']=="1")
			{
				echo '<link  href="/css/styl.css" rel="stylesheet" />';
			}
			if($_SESSION['wersja']=="2")
			{
				echo '<link  href="/css/styl_2.css" rel="stylesheet" />';
			}
		}
	}	
	
	if(isset($_GET["jaka_wersja"]))
	{
		if(isset($_SESSION))
		{
			echo $_SESSION['wersja'];
		}
		else
		{
			echo "1";
		}
	}
	
?>