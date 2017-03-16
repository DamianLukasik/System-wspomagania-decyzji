<?php
    session_start();
    require_once "showimage.php";
    
    if(isset($_SESSION['katalog']))
    {
        unset($_SESSION['katalog']); 
    }
    
    unset($_SESSION['Odtwarzanie']);  
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
    <!-- Pasek nawigacji  -->
    <?php include 'panel/panel_.php'; ?>    
    <!-- Galeria zdjęć   --> 
    <table id="panel_galerii" class="galeria" >
        <tr>
            <td id="pole-2" class="wiersz" colspan="3" >
                <label id="napis-1" class="text-nagłówek1" >Wybierz katalog</label>
            </td>
        </tr>
        <tr id="wyb">
            <td id="wyb-1" >
                <input class="obraz_kategori" id="przycisk_1" name="katalog" value="lokalizacja" type="button" width="100%" height="100%" onclick="wyswietl_katalog(1)">
            </td>
            <td id="wyb-2" >
                <input class="obraz_kategori" id="przycisk_2" name="katalog" value="historia" type="button" width="100%" height="100%" onclick="wyswietl_katalog(2)">
            </td>
            <td id="wyb-3" >
                <input class="obraz_kategori" id="przycisk_3" name="katalog" value="urządzenie" type="button" width="100%" height="100%" onclick="wyswietl_katalog(3)">
            </td>
        </tr>
        <tr id="wyb">
            <td id="wyb-1" class="tekst-zwykly">
                <label class="napis">W którym miejscu?</label>
            </td>
            <td id="wyb-2" class="tekst-zwykly">
				<label class="napis">Kiedy to się stało?</label>
            </td>
            <td id="wyb-3" class="tekst-zwykly">
                <label class="napis">Jakim aparatem?</label>
            </td>
        </tr>
    </table>
	<script type="text/javascript" src="/js/cookie.js" ></script>
	<script type="text/javascript" src="/js/wyswietl.js" ></script>
	<script type="text/javascript" src="/js/przegladanie.js" ></script>
    <?php include 'autorstwo.php';?>     
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>
</body>
</html>
