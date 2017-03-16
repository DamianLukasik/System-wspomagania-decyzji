<?php
    session_start();
    require_once "showimage.php";

    unset($_SESSION['Odtwarzanie'])
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
    <?php include 'panel/panel_.php'; ?>
    <!--    Przeszukaj galerię  -->
    <table>
        <tr>
            <td>
        <table id="katalogi">
            <tr id="galeria" class="galeria">
                <td>
                    <form action="/php/baza_zdjec.php" method="post">
                        <input type="submit" id="cofaj" value="Wycofaj" class="przycisk-dluzszy">
                    </form>
                </td>
            </tr>
        </table>
            </td>
            <td>
                <div id="galeria-zdjecia" style="overflow:auto; max-width: 850px;">
                    <table id="widok">                        
                    </table>
                </div>               
            </td>
        </tr>
    </table>
	<script type="text/javascript" src="/js/wyswietl_galerie.js" ></script>
	<script type="text/javascript" src="/js/cookie.js" ></script>
	<script type="text/javascript" src="/js/wyswietl.js" ></script>
	<script type="text/javascript" src="/js/przegladanie.js" ></script>
	<script type="text/javascript" src="/js/ext.js" ></script>
    <?php include 'autorstwo.php';?>      
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>   
</body>
</html>

