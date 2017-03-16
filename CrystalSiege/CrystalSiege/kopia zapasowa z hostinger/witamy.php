<?php
    session_start();
    if(!isset($_SESSION['udana_rejestracja']))
    {
        header('Location: index.php');
        exit();
    }
    else
    {
        unset($_SESSION['udana_rejestracja']);
    }  
    
    if(isset($_SESSION['fr_nick'])) unset($_SESSION['fr_nick']); 
    if(isset($_SESSION['fr_haslo1'])) unset($_SESSION['fr_haslo1']); 
    if(isset($_SESSION['fr_haslo2'])) unset($_SESSION['fr_haslo2']); 
    if(isset($_SESSION['fr_mail'])) unset($_SESSION['fr_mail']); 
    if(isset($_SESSION['fr_regulamin'])) unset($_SESSION['fr_regulamin']); 
    
    if(isset($_SESSION['error_nick'])) unset($_SESSION['error_nick']); 
    if(isset($_SESSION['error_mail'])) unset($_SESSION['error_mail']); 
    if(isset($_SESSION['error_haslo'])) unset($_SESSION['error_haslo']); 
    if(isset($_SESSION['error_regulamin'])) unset($_SESSION['error_regulamin']); 
    if(isset($_SESSION['error_bot'])) unset($_SESSION['error_bot']); 
?>
<!DOCTYPE html>
<html>
<head>
	<title>Archiwum Zdjęć Częstochowy</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<script src="/scripts/jquery-2.2.0.min.js"></script>
	<script src="/scripts/jquery-ui-1.11.4.min.js"></script>
	<script src="/scripts/angular.min.js"></script>
</head>
<body> 
    <!-- Pasek nawigacji   -->
    <?php include 'php/panel/panel_.php';?>    
    <!-- Panel logowania   -->
    <div style="text-align: center">
        <label class="napis">Dziękujemy za udaną rejestrację.</label>
        <label class="napis">Na twoją pocztę została wysłana wiadomość z linkiem aktywacyjnym.</label>
    </div>
	<script type="text/javascript" src="js/cookie.js" ></script>
	<script type="text/javascript" src="js/wyswietl.js" ></script>
	<script type="text/javascript" src="js/przegladanie.js" ></script>	  
    <?php include 'php/autorstwo.php';?>       
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>   
</body>
</html>

