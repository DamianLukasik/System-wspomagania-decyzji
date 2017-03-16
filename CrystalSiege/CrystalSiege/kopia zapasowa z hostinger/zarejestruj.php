<?php
    if(!isset($_SESSION)) 
    {
        session_start(); 
    }
    
    unset($_SESSION['Odtwarzanie']);
    
    if(isset($_POST['email']))
    {
        //Udana walidacja
        $poprawnosc = true;        
        $nick = $_POST['nick'];    
        if((strlen($nick)<3) || (strlen($nick)>30))
        {
            $poprawnosc = false;
            $_SESSION['error_nick'] = "Nazwa użytkownika może posiadać od 3 do 30 znaków";
        }
        
        if(ctype_print($nick)==false)
        {
            $poprawnosc = false;
            $_SESSION['error_nick'] = "Nazwa użytkownika może składać się z liter i cyfr, ale bez polskich znaków";
        }
        
        $mail  = $_POST['email'];
        
        if(filter_var($mail, FILTER_VALIDATE_EMAIL) === false) 
        {
            $poprawnosc = false;
            $_SESSION['error_mail'] = "Podaj poprawny adres mailowy";
        }
        
        $haslo1 = $_POST['haslo1'];
        $haslo2 = $_POST['haslo2'];
        
        if((strlen($haslo1)<8) || (strlen($haslo1)>20))
        {
            $poprawnosc = false;
            $_SESSION['error_haslo'] = "Hasło musi posiadać od 8 do 20 znaków";
        }
        
        if($haslo1!=$haslo2)
        {
            $poprawnosc = false;
            $_SESSION['error_haslo'] = "Podane hasła nie są takie same";
        }
          
        $haslo_hash = base64_encode(mcrypt_encrypt(MCRYPT_RIJNDAEL_256, md5('m98o87c5pe'), $haslo1, MCRYPT_MODE_CBC, md5(md5('m98o87c5pe'))));
        
        if(!isset($_POST['regulamin']))
        {
            $poprawnosc = false;
            $_SESSION['error_regulamin'] = "Potwierdź akceptacje regulaminu";
        }
        
        $kod1 = $_POST['captcha'];
	$kod2 = $_COOKIE['kod_captchy'];
        
        if(!isset($_COOKIE['kod_captchy']))//Cookies
        {
	    $poprawnosc = false;
            $_SESSION['error_bot'] = "Czas wpisywania kodu minął";
        }
        else
        {
            if(!isset($_POST['captcha']) || $kod1!=$kod2)//Captcha
            {
                $poprawnosc = false;
                $_SESSION['error_bot'] = "Potwierdź, że nie jesteś botem!";
            }
        }

        $_SESSION['fr_nick'] = $nick;
        $_SESSION['fr_haslo1'] = $haslo1;
        $_SESSION['fr_haslo2'] = $haslo2;
        $_SESSION['fr_mail'] = $mail;
        if(isset($_POST['regulamin'])) $_SESSION['fr_regulamin'] = true;
        
        require_once 'php/connect.php';
        mysqli_report(MYSQLI_REPORT_STRICT);
        try
        {
            $conn = new mysqli($host,$db_user, $password,$name_db);
            if($conn->connect_errno!=0)
            {
                throw new Exception(mysqli_connect_errno());     
            }
            else
            {
                $wynik = $conn->query(sprintf("SELECT id FROM uzytkownicy WHERE email='%s'",
                mysqli_real_escape_string($conn,$mail)));
                if(!$wynik) throw new Exception($conn->error);
                $ile_jest_takich_maili = $wynik->num_rows;
                if($ile_jest_takich_maili>0)
                {
                    $poprawnosc = false;
                    $_SESSION['error_mail'] = "Istnieje już konto o takim adresie mailowym";
                }
                
                $wynik = $conn->query(sprintf("SELECT id FROM uzytkownicy WHERE login='%s'",
                mysqli_real_escape_string($conn,$nick)));
                if(!$wynik) throw new Exception($conn->error);
                $ile_jest_takich_nickow = $wynik->num_rows;
                if($ile_jest_takich_nickow>0)
                {
                    $poprawnosc = false;
                    $_SESSION['error_nick'] = "Nazwa użytkownika jest zajęta";
                }
                
                if($poprawnosc==true)
                {
                    $actCode=str_shuffle("qwertyuiopasdfghjklzxcvbnm1234567890");
                    if($conn->query("INSERT INTO uzytkownicy (id, login, haslo, email, aktywacja) VALUES (NULL, '$nick', '$haslo_hash', '$mail','$actCode')"))
                    {echo '<p>testy3</p>';
                        $_SESSION['udana_rejestracja']=true;
                        wyslij_mail_weryfikacyjny($actCode);   
						header('Location: /witamy.php');						
                    }
                    else
                    {
                        throw new Exception($conn->error);
                    }
                }
                $conn->close();
                //header('Location: /index.php');
            }
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    function wyslij_mail_weryfikacyjny($actCode)
    {
		require_once 'php/default.php';
        $_POST["haslo1"]=SHA1($_POST["haslo1"]);        
        $headers="MIME-Version: 1.0\r\n";
        $header .= "Content-type: text/html; charset=utf-8r\n"; 
        $header .= "Content-Transfer-Encoding: 8bitr\n";  
        $content="\n
        Cześć ".$_POST['nick'].", właśnie założyłeś konto w serwisie Archiwum Zdjęć Częstochowy!\n\n
        Wystarczy aktywować konto, aby to zrobić musisz kliknąć poniższy link aktywujący:\n".
        $adres_strony."/zarejestruj.php?active=".$actCode."\n\n
        Jeśli mail trafił do Ciebie przez pomyłkę, proszę go zignorować\n\n
        Dziękuje za uwagę\n
        ~Administrator serwisu";
        $title = "=?UTF-8?B?".base64_encode("Archiwum Zdjec Czestochowy - Aktywacja Konta")."?=";
        mail($_POST["email"], $title , $content, $headers);       
    }
    
    if(isset($_GET["active"]))
    {
        $kod = $_GET["active"];
        require_once 'php/connect.php';
        mysqli_report(MYSQLI_REPORT_STRICT);
        try
        {
            $conn = new mysqli($host,$db_user, $password,$name_db);
            if($conn->connect_errno!=0)
            {
                throw new Exception(mysqli_connect_errno());     
            }
            else                
            {
                if(@$conn->query(
                sprintf("UPDATE uzytkownicy SET zweryfikowany=1 WHERE aktywacja='%s'",
                mysqli_real_escape_string($conn,$kod))))
                {
                    print "Aktywacja ukonczona pomyślnie. Możesz już korzystać z naszego serwisu.";
                    header('Location: witamy.php');
                }
                else
                {
                    print "Podano nieistniejący kod aktywacyjny.";
                    header('Location: witamy.php');
                }
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    if(isset($_POST["odswiez"]))
    {
        print " XXXXXXXXXXXXX  ";
        $_SESSION['fr_nick'] = $_POST['nick'];    
        $_SESSION['fr_haslo1'] = $_POST['haslo1'];
        $_SESSION['fr_mail'] = $_POST['email'];
        $_SESSION['fr_regulamin'] = $_POST['regulamin'];
        header('refresh: 1;');
    }
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
	<?php include 'php/panel/wybierz_styl.php'; ?>
    <table id="pasek_" class="panel_użytkownika">
        <tr>
            <td>
                <form action="glowna.php" method="post">
                    <input id="przycisk52" type="submit" value="Wycofaj" class="przycisk-standard" style="width: 195px">
                    <?php
                        echo "<label id='lab_id' class='napis3'>Załóż nowe konto!</label>";
                    ?>
                </form>
			</td>
		</tr>
		<tr>
                    <td>
			<input id="przycisk99" type="button" value="Dla słabowidzących" class="przycisk-standard" style="width: 195px" onclick="zmien_styl()">
                    </td>
		</tr>
                <tr>
                    <td>
			<input id="przycisk98" type="button" value="" class="przycisk-standard" onclick="wylacz_glos()" style="width: 195px">
                    </td>
                </tr>
    </table>
    <br>
    <!-- Panel rejestracji -->
    <form method="post">
        <table id="panel_rejestracji" class="pole-standard">
            <tr id="wiersz-0" class="wiersz">
                <td id="dane-0" class="kolumna-dane">
                    <label class="napis3">Podaj nazwę użytkownika</label>
                </td>
                <td id="pole-0" class="kolumna-pole">
                    <input id="pole0" type="text" name="nick" value="<?php
                        if(isset($_SESSION['fr_nick']))
                        {
                            echo $_SESSION['fr_nick'];   
                            unset($_SESSION['fr_nick']);
                        }
                        ?>" class="text-standard" onkeyup="weryfikacja_nick()"/>
                </td>
                <td id="znak-0" class="kolumna-znak">
                    <img id="znak0" type="image" src="grafika/Z.gif" width="40" height="40">
                </td>
            </tr>
            <tr class="wiersz">    
                <td colspan="3" class="kolumna-pole">
                    <?php
                        if(isset($_SESSION['error_nick']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_nick'].'</span>';   
                            unset($_SESSION['error_nick']);
                        }
                    ?>
                </td>
            </tr>
            <tr id="wiersz-1" class="wiersz">
                <td id="dane-1" class="kolumna-dane">
                    <label class="napis3">Wprowadź hasło</label>
                </td>
                <td id="pole-1" class="kolumna-pole">
                    <input id="pole1" type="password" name="haslo1" value="<?php
                        if(isset($_SESSION['fr_haslo1']))
                        {
                            echo $_SESSION['fr_haslo1'];   
                            unset($_SESSION['fr_haslo1']);
                        }
                    ?>" class="text-standard" onkeyup="wprowadz_hasło()" />
                </td>
                <td id="znak-1" class="kolumna-znak">
                    <img id="znak1" type="image" src="grafika/Z.gif" width="40" height="40">
                </td>
            </tr>
            <tr class="wiersz">
                <td colspan="3" class="kolumna-pole">
                    <?php
                        if(isset($_SESSION['error_haslo']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_haslo'].'</span>';
                            unset($_SESSION['error_haslo']);
                        }
                    ?>
                </td>
            </tr>
            <tr id="wiersz-2" class="wiersz">
                <td id="dane-2" class="kolumna-dane">
                    <label class="napis3">Powtórz hasło</label>
                </td>
                <td id="pole-2" class="kolumna-pole">
                    <input id="pole2" type="password" name="haslo2" value="<?php
                        if(isset($_SESSION['fr_haslo2']))
                        {
                            echo $_SESSION['fr_haslo2'];   
                            unset($_SESSION['fr_haslo2']);
                        }
                    ?>" class="text-standard" onkeyup="porównaj_hasło()" />  
                </td>
                <td id="znak-2" class="kolumna-znak">
                    <img id="znak2" type="image" src="grafika/Z.gif" width="40" height="40">
                </td>
            </tr>
            <tr id="wiersz-3" class="wiersz">
                <td id="dane-3" class="kolumna-opis">
                    <label class="napis3">Siła hasła
                        <script src="js/skrypt.js" type="text/javascript"></script>
                    </label>
                </td>
                <td id="pole-3" class="kolumna-opis">
                    <table width="100%" height="6px">
                        <tr>
                            <label id="hasło" class="napis3"></label>
                            <td id="siła1" class="komórka-siła"></td>
                            <td id="siła2" class="komórka-siła"></td>
                            <td id="siła3" class="komórka-siła"></td>
                            <td id="siła4" class="komórka-siła"></td>
                            <td id="siła5" class="komórka-siła"></td>
                        </tr>
                    </table>
                </td>
                <td id="znak-3" class="kolumna-znak"></td>
            </tr>
            <tr id="wiersz-4" class="wiersz">
                <td id="dane-4" class="kolumna-dane">
                    <label class="napis3">Podaj adres e-mail</label>
                </td>
                <td id="pole-4" class="kolumna-pole">
                    <input id="pole4" type="text" name="email" value="<?php
                        if(isset($_SESSION['fr_mail']))
                        {
                            echo $_SESSION['fr_mail'];   
                            unset($_SESSION['fr_mail']);
                        }
                    ?>" class="text-standard" onkeyup="weryfikacja_maila()"/>  
		</td>
                <td id="znak-4" class="kolumna-znak">
                    <img id="znak4" type="image" src="grafika/Z.gif" width="40" height="40">
                </td>
            </tr>
            <tr class="wiersz">    
                <td colspan="3" class="kolumna-pole">
                    <?php
                        if(isset($_SESSION['error_mail']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_mail'].'</span>';   
                            unset($_SESSION['error_mail']);
                        }
                    ?>
                </td>
            </tr>
            <tr id="wiersz-5" class="wiersz">
                <td id="pole-5" class="kolumna-pole" colspan="3" >                    
                    <script>
                        
                        var fonty = new Array("times", "arial", "comic", "trebuc", "verdana", "consola","Comic Sans MS");//7
			var kolor = new Array("lightcoral","turquoise","orange","springgreen","blueviolet","chartreuse","darkorange","lightskyblue","greenyellow","tomato","mediummaquamarine","lawngreen");//10
			var znaki = "ABCDEFGHIJKLMNOPRQSTUWYZ1234567890";
			var tekst = "";
                        
			var imageObj = new Image();                        
                        setTimeout(function(){ nowy_obraz(); }, 400);   
                        
                        function nowy_obraz() {
                            imageObj.src = "php/captcha_bcg/bcg_"+losuj(6)+".jpg";
                            imageObj.onload = function(){
                                generuj_obraz(this);
                            };
                        }
    
			function losuj(i) {
			    return Math.floor((Math.random() * i) + 1);
			}
                        
			function generuj_obraz(img) {
                            var canvas = document.getElementById("obrazek");
                            var ctx = canvas.getContext('2d');
                            ctx.drawImage(img, 0, 0);

                            var len = losuj(3) + 4;
                            tekst = "";
                            for(var i=0;i<len;i++)//poprawić captchę
                            {
				ctx.font = "bold "+(losuj(25)+15)+"px "+fonty[losuj(7)]+"";
    				ctx.textBaseline = "middle";
				var x = losuj(znaki.length);
				var w = znaki.substr(x,1);
				ctx.fillStyle = kolor[losuj(12)];
    				ctx.fillText(w, (15+losuj(10))+(i*22), losuj(20)+35);
				tekst += ""+w;	
                            }
                            //zapisywanie w ciasteczku                            
                            var data = new Date();
                            data.setTime(data.getTime()+(10*60*1000));
                            var expires = "; expires="+data.toGMTString();
                            document.cookie = "kod_captchy=" + tekst + expires + "; path=/";  
			}
                        
                    </script>
                    <input id="pole5" type="button" value="Odśwież" onclick="nowy_obraz()" class="przycisk-standard">
                    <input type="text" id="znak550" name="captcha" value="" class="text-standard">
                    <canvas id="obrazek" width="220" height="100">
                    </canvas>
                </td>
            </tr>
            <tr class="wiersz">
                <td colspan="3" class="kolumna-pole">
                    <?php
                        if(isset($_SESSION['error_bot']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_bot'].'</span>';
                            unset($_SESSION['error_bot']);
                        }
                    ?>
                </td>
            </tr>
            <tr id="wiersz-6" class="wiersz">
                <td id="dane-6" class="kolumna-dane" >
                    <label class="napis3">Zaakceptuj <a id="regulamin" href="/php/regulamin.php">regulamin</a></label>
                </td>
                <td id="pole-6" class="kolumna-pole">                    
                    <input id="pole6" type="checkbox" name="regulamin" class="checkbox-standard" <?php
                        if(isset($_SESSION['fr_regulamin']))
                        {
                            echo "checked";   
                            unset($_SESSION['fr_regulamin']);
                        }
                    ?> onchange="zaakceptuj_regulamin()"/>  
                </td>
                <td id="znak-6" class="kolumna-znak">
                    <img id="znak6" type="image" src="grafika/Z.gif" width="40" height="40">
                </td>
            </tr>
            <tr class="wiersz">    
                <td colspan="3" class="kolumna-pole">
                    <?php
                        if(isset($_SESSION['error_regulamin']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_regulamin'].'</span>';   
                            unset($_SESSION['error_regulamin']);
                        }
                    ?>
                </td>
            </tr>
            <tr id="wiersz-7" class="wiersz">
                <td id="pole-7" class="kolumna-przycisk" colspan="3" align="center" valign="middle">
                    <input id="przycisk77" type="submit" value="Zarejestruj się" class="przycisk-standard">
                </td>
            </tr>
        </table>        
    </form>
	<script type="text/javascript" src="/js/cookie.js" ></script>
    <script>
        porównaj_hasło();
        wprowadz_hasło();
        weryfikacja_maila();
        weryfikacja_nick();
        zaakceptuj_regulamin();
    </script>     
    <?php include 'php/autorstwo.php';?>       
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>   
</body>
</html>

