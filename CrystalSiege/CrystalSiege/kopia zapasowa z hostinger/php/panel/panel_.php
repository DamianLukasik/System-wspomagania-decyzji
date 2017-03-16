<?php
	include 'wybierz_styl.php';
	if(!isset($_SESSION)) 
    { 
        session_start(); 
    } 
    if(!isset($_SESSION['zalogowany']))
    {
        include 'panel_goscia.php';
    }
    else
    {
        if($_SESSION['prawa']==1)
        {
            include 'panel_uzytkownika.php';
        }
        if($_SESSION['prawa']==2)
        {
            include 'panel_admina.php';
        }   
    }