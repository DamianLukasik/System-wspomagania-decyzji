<?php
    session_start();
    if((isset($_SESSION['zalogowany'])) && ($_SESSION['zalogowany']==TRUE))
    {
        header('Location: /glowna.php');
        exit();
    }
    
    unset($_SESSION['Odtwarzanie']);
    
    if(isset($_POST['mail']))
    {
        //Udana walidacja
        $mail  = $_POST['mail'];
        
        if(filter_var($mail, FILTER_VALIDATE_EMAIL) === false) 
        {
            $poprawnosc = false;
            $_SESSION['error_mail'] = "Podaj poprawny adres mailowy";
        }
        else
        {
            require_once 'connect.php';
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
                    $wynik = $conn->query(sprintf("SELECT haslo, login FROM uzytkownicy WHERE email='%s'",mysqli_real_escape_string($conn,$mail)));                    
                    if(!$wynik) throw new Exception($conn->error);
                    {
                        $wiersz = mysqli_fetch_array($wynik);
                        $haslo = $wiersz['haslo'];
                        $dane = rtrim(mcrypt_decrypt(MCRYPT_RIJNDAEL_256, md5('m98o87c5pe'), base64_decode($haslo), MCRYPT_MODE_CBC, md5(md5('m98o87c5pe'))),"\0");            
                        wyslij_mail($dane,$wiersz['login']);            
                    }
                    $conn->close();
                    header('Location: /index.php');
                }
            }
            catch(Exception $e)
            {
                //echo 'błąd </br> '.$e;
            }          
        }
    }
    
    function wyslij_mail($haslo,$login)
    {
        $headers="MIME-Version: 1.0\r\n";
      //  $headers.= "Content-type: text/html; charset=iso-8859-2\r\n";        
        $header .= "Content-type: text/html; charset=iso-8859-2r\n"; 
        $header .= "Content-Transfer-Encoding: 8bitr\n";        
        $content="\n
        Cześć ".$login.", na twoje konto wysłano żądanie przypomnienia hasła w serwisie Archiwum Zdjęć Częstochowy!\n\n
        Brzmi ono : ".$haslo."\n\n
        Jeśli mail trafił do Ciebie przez pomyłkę, proszę go zignorować, jeśli wiadomość będzie się powtarzać proszę zgłosić do adminitracji technicznej.\n\n
        Dziękuje za uwagę\n
        ~Administrator serwisu";
        mail($_POST["mail"], "Archiwum Zdjec Czestochowy - Przypomnienie hasla", $content, $headers);       
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
	<?php include 'panel/wybierz_styl.php'; ?>
    <!-- Pasek nawigacji   -->
    <table id="pasek_" class="panel_użytkownika">
        <tr>
            <td>
                <form action="/glowna.php" method="post">
                    <input id="przycisk52" type="submit" value="Wycofaj" class="przycisk-standard" style="width: 195px" >
                    <?php
                        echo "<label id='lab_id' class='napis3'> Wyślij żądanie przypomnienia hasła!</label>";
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
			<input id="przycisk98" type="button" value="" class="przycisk-standard" onclick="wylacz_glos()" style="width: 195px" >
                    </td>
                </tr>
    </table>
    <br>

    <form method="post">
		<label class="napis">E-mail: </label><input id="przycisk53" type="text" name="mail" class="text-standard"/>   
        <?php
        if(isset($_SESSION['error_mail']))
        {
            echo '<span class="komunikat_błąd">'.$_SESSION['error_mail'].'</span>';   
            unset($_SESSION['error_mail']);
        }
        ?>    
        <input id="przycisk54" type="submit" value="Wyślij" class="przycisk-standard">
    </form>
	<script type="text/javascript" src="/js/cookie.js" ></script>
    <?php include 'autorstwo.php';?>       
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>   
</body>
</html>