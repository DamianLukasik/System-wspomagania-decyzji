<?php
    session_start();
    if(!isset($_SESSION['zalogowany']))
    {
        header('Location:index.php');
        exit();
    }
    require_once "showimage.php";
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
    <?php include 'panel/wybierz_styl.php'; ?>
    <!-- Pasek nawigacji   -->
    <table id="pasek_" class="panel_użytkownika">
        <tr>
            <td style="width: 100%" >
                <form action="/glowna.php" method="post">
                    <input id="przycisk52" type="submit" value="Wycofaj" class="przycisk-standard" style="width: 195px">
                    <?php
                    echo "<label id='lab_id' class='napis3' >Twój profil ".$_SESSION['user']."?</label>";
                    ?>  
                </form>
            </td>            
            <td>
                <form action="/php/edytuj.php" method="post">
                    <input id="przycisk24" style="float: right" type="submit" value="Edytuj profil" class="przycisk-standard">
                </form>  
            </td>
        </tr>
        <tr>
            <td>
                <input id="przycisk99" type="button" value="Dla słabowidzących" class="przycisk-standard" styl="width: 120%" style="width: 195px" onclick="zmien_styl()">
            </td>
	</tr>
        <tr>
            <td>
		<input id="przycisk98" type="button" value="" class="przycisk-standard" onclick="wylacz_glos()" style="width: 195px">
            </td>
        </tr>
    </table>
    <br>
    <input id="pole0" style="visibility:hidden;" value="<?php echo "".$_SESSION['idek']."";?>" />
    </br></br>    
    </br></br>
    
    
    <table id="pole" class="profl_standard">
        <tr>
            <td class="tekst-zwykly">
                Nazwa użytkownika:
            </td>
            <td>
                <label id="pole1" class="tekst-zwykly"></label>
            </td>
        </tr>
        <tr>
            <td class="tekst-zwykly">
                Adres e-mail:
            </td>
            <td>
                <label id="pole2" class="tekst-zwykly"></label>
            </td>
        </tr>
        <tr>
            <td class="tekst-zwykly">
                Data dołączenia:
            </td>
            <td>
                <label id="pole3" class="tekst-zwykly"></label>
            </td>
        </tr>
        <tr>
            <td class="tekst-zwykly">
                Data ostatniego logowania:
            </td>
            <td>
                <label id="pole4" class="tekst-zwykly"></label>
            </td>
        </tr>
    </table>
    
    <script>
    
    var log=document.getElementById("pole0").value;
    
    załaduj_dane(log,"pole1","login"); 
    załaduj_dane(log,"pole2","email");
    załaduj_dane(log,"pole3","data_dolaczenia");
    załaduj_dane(log,"pole4","data_ostatniego_logowania");       
    
    function załaduj_dane(idek,id,co){
        $.ajax({
            data: 'id=' + idek + "&co=" + co,
            url: 'showuser.php',
            method: 'POST', // or GET
            success: function(msg) {
                document.getElementById(id).innerHTML = msg;
                skonstruuj_kontener(msg);
                /*
                kontener_inputow = null;
                pojemnik_inputow = null;
                kontener_inputow = [];
                pojemnik_inputow = [];
                kontener_inputow.length = 0;
                pojemnik_inputow.length = 0;
                
                var tab = new Array();
                tab[0] = [];
                tab[1] = [];
                tab[0][0] = "przycisk52";
                tab[0][1] = "przycisk24";
                tab[1][1] = "przycisk99";
                pojemnik_inputow = "przycisk52","przycisk24","przycisk99";
                kontener_inputow = tab;
                numer_kolumny = 0;
                numer_wiersza = 0;
                
                alert(kontener_inputow[0]);*/
                
             //  kontener_inputow[0][1] = document.getElementsByTagName("INPUT")[i].id;   
            }
        });
    }
    
    
    </script>
    <?php include 'autorstwo.php';?>       
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>
</body>
</html>