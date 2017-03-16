<?php
    session_start();
    if(isset($_POST['tytul']))
    {
        //Udana walidacja
        $poprawnosc = true;
        $tytul = $_POST['tytul'];  
        $opis  = $_POST['opis'];  
        if(strlen($tytul)==0 || $tytul=="")
        {
            $poprawnosc = false;
            $_SESSION['error_tytul'] = "Nie wpisałeś tytułu";
        }
        if(strlen($opis)<=16 || $opis=="")
        {
            $poprawnosc = false;
            $_SESSION['error_opis'] = "Proszę napisać szczegółowo na czym polega problem";
        }
                
        $_SESSION['fr_tytul'] = $tytul;
        $_SESSION['fr_opis']  = $opis;
        if($poprawnosc)
        {
            wyslij_zgloszenie();
        }
    }
    
    function wyslij_zgloszenie()
    {
        if(isset($_SESSION['user'])){$user="Użytkownik ".$_SESSION['user'];}else{$user="Jeden z gości";}
        
        $headers="MIME-Version: 1.0\r\n";
        $header .= "Content-type: text/html; charset=iso-8859-2r\n"; 
        $header .= "Content-Transfer-Encoding: 8bitr\n";  
        $content="\n
        ".$user." zgłasza problem!\n\n
        ".$_POST['tytul']."\n
        ".$_POST['opis']."\n";
        mail("archiwumzdjecczestochowy@gmail.com", "Archiwum Zdjec Czestochowy - Zgloszenie bledu od ".$_SESSION['user'], $content, $headers);       
        header('Location:/index.php');       
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
                    <input id="przycisk52" type="submit" value="Wycofaj" class="przycisk-standard" style="width: 195px">
                    <?php
					if(isset($_SESSION['user']))
					{
						echo "<label id='lab_id' class='napis3'>Co chcesz zgłosić <a href='profil.php'>".$_SESSION['user']."</a>?</label>";
					}
					else
					{
						echo "<label id='lab_id' class='napis3'>Co chcesz zgłosić?</label>";
					}
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
    <!-- Formularz zgłoszenia -->    
    <form method="post">
        <table id="pasek_" class="formularz_przesyłania">
            <tr>
                <td class="opis">
                    <label class="napis">Tytuł:</label>
                </td>                    
                <td>
                    <input id="przycisk55" type="text" name="tytul" value="<?php
                        if(isset($_SESSION['fr_tytul']))
                        {
                            echo $_SESSION['fr_tytul'];   
                            unset($_SESSION['fr_tytul']);
                        }
                        ?>" class="text-standard-przeslij"/>
                </td>                
            </tr>    
            <tr>    
                <td colspan="3">
                    <?php
                        if(isset($_SESSION['error_tytul']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_tytul'].'</span>';   
                            unset($_SESSION['error_tytul']);
                        }
                    ?>
                </td>
            </tr>
            <tr>
                <td class="opis">
                    <label class="napis">Treść zgłoszenia:</label>
                </td>                    
                <td>
                    <textarea id="przycisk56" name="opis" cols="50" rows="10" class="obszar_tekstowy-przeslij"><?php
                        if(isset($_SESSION['fr_opis']))
                        {
                            echo $_SESSION['fr_opis'];   
                            unset($_SESSION['fr_opis']);
                        }
                        ?></textarea>
                </td>
            </tr> 
            <tr>    
                <td colspan="3">
                    <?php
                        if(isset($_SESSION['error_opis']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_opis'].'</span>';   
                            unset($_SESSION['error_opis']);
                        }
                    ?>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>                    
                <td>
                    <input id="przycisk57" type="submit" value="Wyślij" class="przycisk-standard" >
                </td>
            </tr>
        </table>
    </form>      
    </br></br>    
	<script type="text/javascript" src="/js/cookie.js" ></script>
    <?php include 'autorstwo.php';?>       
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>   
</body>
</html>

