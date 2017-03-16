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
            <td>
                <form action="/glowna.php" method="post">
                    <input id="przycisk52" type="submit" value="Wycofaj" style="width: 195px" class="przycisk-standard">
                    <?php
                    echo "<label id='lab_id' class='napis3' >Co chcesz zmienić</label> <a class='tekst-zwykly' id='profil' href='profil.php'>".$_SESSION['user']."</a><label class='napis3'>?</label>";
                    ?>  
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
            <td id="pole-1">
                <label id="dane1" class="tekst-zwykly" ></label>
            </td>
            <td>
                <input id="przycisk1" type="button" value="Zmień login" class="przycisk-ikona-zmien" onclick="zmien_login()" style="width: 110px" >
            </td>            
        </tr>
        <tr><td></td>
            <td colspan="3">
                <?php
                    if(isset($_SESSION['error_nick']))
                    {
                        echo '<span class="komunikat_błąd">'.$_SESSION['error_nick'].'</span>';   
                        unset($_SESSION['error_nick']);
                    }
                ?>
            </td><td></td>
        </tr>
        <tr>
            <td class="tekst-zwykly">
                Aktualne hasło:
            </td>
            <td id="pole-2">
                <label id="dane2" class="tekst-zwykly" ></label>
            </td>
            <td>
                <input id="przycisk2" type="button" value="Zmień hasło" class="przycisk-ikona-zmien" onclick="zmien_haslo()" style="width: 110px" >
            </td>
        </tr>
        <tr>
            <td class="tekst-zwykly">
                Nowe hasło:
            </td>
            <td id="pole-3">
                <label id="dane3" class="tekst-zwykly" ></label>
            </td>
        </tr>
        <tr><td></td>
            <td colspan="3">
                <?php
                    if(isset($_SESSION['error_haslo']))
                    {
                        echo '<span class="komunikat_błąd">'.$_SESSION['error_haslo'].'</span>';
                        unset($_SESSION['error_haslo']);
                    }
                ?>
            </td><td></td>
        </tr>
        <tr>
            <td class="tekst-zwykly">
                Adres e-mail:
            </td>
            <td id="pole-4">
                <label id="dane4" class="tekst-zwykly" ></label>                 
            </td>
            <td>
                <input id="przycisk4" type="button" value="Zmień email" class="przycisk-ikona-zmien" onclick="zmien_email()" style="width: 110px" >
            </td>
        </tr>
        <tr><td></td>
            <td colspan="3">
                <?php
                    if(isset($_SESSION['error_mail']))
                    {
                        echo '<span class="komunikat_błąd">'.$_SESSION['error_mail'].'</span>';
                        unset($_SESSION['error_mail']);
                    }
                ?>
            </td>
        </tr>
        <tr>
            <td class="tekst-zwykly">
                Skasuj konto:
            </td>
            <td>
                <?php
                    if(isset($_SESSION['error_konto']))
                    {
                        echo '<span class="komunikat_błąd">'.$_SESSION['error_konto'].'</span>';
                        unset($_SESSION['error_konto']);
                    }
                ?>
            </td>
            <td>
                <?php
                    if($_SESSION['prawa']!=2)
                    {
                        echo '<form method="post"><input id="przycisk5" type="submit" value="Usuń konto" class="przycisk-ikona-usun" onclick="usun_konto()" style="width: 110px" ></form>';
                    }
                ?>
            </td>
        </tr>  
    </table>
    
    
    <script>
    
    var log=document.getElementById("pole0").value;
    var pass="";
    var ver="0";
    var ver_=true;
    
    załaduj_dane(log,"dane","login","1");  
    załaduj_dane(log,"dane","email","4");
    document.getElementById("dane3").innerHTML = "********";
    
    function załaduj_dane(login,id,co,nr){
        $.ajax({
            data: 'id=' + login + "&co=" + co,
            url: 'showuser.php',
            method: 'POST', // or GET
            success: function(msg) {
                if(nr=="2"){
                    pass=msg.substring(0,msg.length);
                }
                else
                {
                    if(id=="dane"){
                        document.getElementById(id+""+nr).innerHTML = msg;
                    }
                    if(id=="pole"){
                        document.getElementById(id+""+nr).value = msg;
                    }
                }
            }
        });
    }
    
    function zapisz_haslo(login,dane){
        $.ajax({
            data: 'id=' + login + "&dane=" + dane + "&ver=" + ver,
            url: 'editdata.php',
            method: 'POST', // or GET
            success: function() {
                location.reload();
            }
        });
    }
    
    function zapisz_login(login,dane){
        $.ajax({
            data: 'id=' + login + "&dane=" + dane,
            url: 'editdata.php',  
            method: 'POST', // or GET
            success: function() {
                location.reload();
            }
        });
    }
    
    function zmien_login(){
        $("#dane1").remove();
        $("#pole-1").append("<input id='pole1' class='tekst-zwykly' />");
        załaduj_dane(log,"pole","login","1");
        $("#pole1").focus();
        $("#pole1").attr("class","text-standard");
        tryb_edycji_danych = true;
        $("#przycisk1").val("Zapisz zmiany");
        $("#przycisk1").attr('onclick','zapisz_login(log,$("#pole1").val())');
        $(window).keydown(function (e) {
            Zatwierdzanie_enterem(e,1);
        });
    }
    
    function zmien_haslo(){
        $("#dane2").remove();
        $("#pole-2").append("<input id='pole2' class='tekst-zwykly' />");
        $("#pole2").focus();
        $("#pole2").attr("class","text-standard");
        tryb_edycji_danych = true;
        $("#pole2").attr('onkeyup','zweryfikuj_haslo()');
        $("#dane3").remove();
        $("#pole-3").append("<input id='pole3' class='tekst-zwykly' />");
        $("#pole3").attr("class","text-standard");
        załaduj_dane(log,"pole","haslo","2");
        $("#przycisk2").val("Zapisz zmiany");
        $("#przycisk2").attr('onclick','zapisz_haslo(log,$("#pole3").val())');
        $(window).keydown(function (e) {
            Zatwierdzanie_enterem(e,2);
        });
    }    
    
    function zweryfikuj_haslo(){
	var a = $("#pole2").val();
        if(a==pass)
        {
            ver="1";
        }
        else
        {
            ver="0";
        }
    }
    
    function zmien_email(){
        $("#dane4").remove();        
        $("#pole-4").append("<input id='pole4' class='tekst-zwykly' />");
        $("#pole4").focus();
        $("#pole4").attr("class","text-standard");
        tryb_edycji_danych = true;
        załaduj_dane(log,"pole","email","4"); 
        $("#przycisk4").val("Zapisz zmiany");
        $("#przycisk4").attr('onclick','zapisz_email(log,$("#pole4").val())');
        $(window).keydown(function (e) {
            Zatwierdzanie_enterem(e,3);
        });
    }
    
    function zapisz_email(login,dane){
        $.ajax({
            data: 'id=' + login + "&mail=" + dane,
            url: 'editdata.php',
            method: 'POST', // or GET
            success: function() {
                location.reload();
            }
        });
    }
    
    function usun_konto(){
        if(confirm($("#pole00").html()+'! Czy na pewno chcesz usunąć swoje konto?')){
            $.ajax({
            data: 'id=' + log,
            url: 'editdata.php',
            method: 'POST', // or GET
            success: function() {
                location.reload();
            }
        });
        }else{}  
    }
    
    function Zatwierdzanie_enterem(e,wart) {
        var keyCode = e.which;
        if (keyCode == 13) {
            switch(wart)
            {
                case 1:
                    zapisz_login(log,$("#pole1").val());
                    $("#pole1").attr("class","tekst-zwykly");
                    tryb_edycji_danych=false;
                    break;
                case 2:
                    if(ver_)
                    {
                        $("#pole3").focus();
                        ver_=false;
                    }
                    else
                    {
                        zapisz_haslo(log,$("#pole3").val());
                        tryb_edycji_danych=false;
                        $("#pole2").attr("class","tekst-zwykly");
                        $("#pole3").attr("class","tekst-zwykly");
                    }
                    break;
                case 3:
                    zapisz_email(log,$("#pole4").val())
                    $("#pole4").attr("class","tekst-zwykly");
                    tryb_edycji_danych=false;
                    break;
                default:alert();
                    break;
            }
        }
    }
    
    </script>
    <?php include 'autorstwo.php';?>       
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>
</body>
</html>
