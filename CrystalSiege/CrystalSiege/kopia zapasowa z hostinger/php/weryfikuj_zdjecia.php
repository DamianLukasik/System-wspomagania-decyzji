<?php
    session_start();
    if(!isset($_SESSION['zalogowany']) || $_SESSION['prawa']==1)
    {
        header('Location:/index.php');
        exit();
    }
    require_once "showimage.php";     
    if(isset($_POST['id']))
    {
        $id = ''.$_POST['id'];        
        try
        {
            require_once 'connect.php';
            $conn = new mysqli($host,$db_user, $password,$name_db);
            if($conn->connect_errno!=0)
            {
                throw new Exception(mysqli_connect_errno());     
            }
            else
            {
                if($_GET['akc']==0)
                {
                    $sql = "UPDATE zdjecia SET zweryfikowany=1, data_weryfikacji=NOW() WHERE id='%s'";                   
                }
                if($_GET['akc']==1)
                {
                    $sql = "DELETE FROM zdjecia WHERE id='%s'";                    
                    $wynik = @$conn->query(sprintf("SELECT zdjecie, miniatura FROM zdjecia WHERE id='%s'",
                             mysqli_real_escape_string($conn,$id)));
                    $wiersz = mysqli_fetch_array($wynik);  
                    unlink($wiersz['zdjecie']);
					unlink($wiersz['miniatura']);
                }
                if($_GET['akc']==2)
                {
                    $sql = "UPDATE uzytkownicy SET blokada=1 WHERE login='%s'";
                }
                if($_GET['akc']==3)
                {
                    header('Location: zarzadzaj.php?baza=Zdjęcia&id='.$id.'');
                }
				if($_GET['akc']==4)
                {
                    $sql = "UPDATE uzytkownicy SET blokada=0 WHERE login='%s'";
                }
                if(@$conn->query(sprintf($sql,mysqli_real_escape_string($conn,$id))))
                {
                    header('Location: weryfikuj_zdjecia.php');
                }
            }
            $conn->close();        
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
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
    <?php include 'panel/panel_.php'; ?>    
    <!-- Panel weryfikacji -->   
    <center>
    <table style="width: 70%">
        <tr>
            <?php                
                require_once "wyswietl_dane_zdjecie.php";    
                if(isset($_SESSION['brak']))
                { 
                    echo '<td colspan="4"><table class="kafelek-standard"><tr><td>'
                    . '<label class="opis_zdjecia">Wszystkie zdjęcia zostały zweryfikowane'
                    . '</label></td></tr></table></td>';
                }
                else
                {                    
                    echo '</tr><tr><td></td><td>';                
                    echo '<form action="/php/weryfikuj_zdjecia.php?akc=0" method="post">';
                    if(isset($_SESSION['fr_id']))
                    {
                        echo '<input id="in_1" type="text" name="id" style="visibility: hidden; width: 0px;" value="';
                        echo ''.$_SESSION['fr_id'].'" >';
                    }
                    echo '<input id="zaakc" type="submit" value="Zaakceptuj" class="przycisk-standard"></form>';
                    echo '</td><td>';
                    echo '<form action="/php/weryfikuj_zdjecia.php?akc=1" method="post">';
                    if(isset($_SESSION['fr_id']))
                    {
                        echo '<input id="in_2" type="text" name="id" style="visibility: hidden; width: 0px;" value="';
                        echo ''.$_SESSION['fr_id'].'" >';                        
                    }
                    echo '<input id="odrzu" type="submit" value="Odrzuć" class="przycisk-standard"></form>';
                    echo '</td><td>';
					$napis_do_blokady = " użytkownika";
					$style='style="visibility: visible"';
					if($_SESSION['user']==$_SESSION['fr_autor'])
					{
						$style='style="visibility: hidden"';
					}
                    if($_SESSION['fr_blokada']!=0)
                    {
						echo '<form id="form_" action="/php/weryfikuj_zdjecia.php?akc=4" method="post">';		
						$napis_do_blokady = "Odblokuj".$napis_do_blokady;
                    }
                    else
                    {
						echo '<form id="form_" action="/php/weryfikuj_zdjecia.php?akc=2" method="post">';
						$napis_do_blokady = "Zablokuj".$napis_do_blokady;
                    }
                    if(isset($_SESSION['fr_autor']))
                    {
                        echo '<input id="in_3" type="text" name="id" style="visibility: hidden; width: 0px;" value="';
                        echo ''.$_SESSION['fr_autor'].'" >';
                        unset($_SESSION['fr_autor']);
                    }                    
                    echo '<input id="but_3" type="submit" value="'.$napis_do_blokady.'" '.$style.' class="przycisk-dluzszy"></form>';   
                    echo '</td><td>';
                    echo '<form action="/php/weryfikuj_zdjecia.php?akc=3" method="post">';
                    if(isset($_SESSION['fr_id']))
                    {
                        echo '<input id="in_4" type="text" name="id" style="visibility: hidden; width: 0px;" value="';
                        echo ''.$_SESSION['fr_id'].'" >';
                    }                    
                   // echo '<form action="/php/zarzadzaj.php?baza=Zdjęcia&id='.$_SESSION['fr_id'].'" method="post">';
                    echo '<input id="edytu" type="submit" value="Edytuj dane" class="przycisk-standard"></form>';                    
                    echo '</td><td></td></tr>';                     
                }
                unset($_SESSION['fr_id']);
                unset($_SESSION['brak']);
                
                echo '<input id="user" type="text" value="'.$_SESSION['user'].'" class="ukryty">';                            
            ?>
    </table>
    </center>
    <script type="text/javascript" >
        
        var windowHeight = $(window).height();
        var windowWidth = $(window).width();
        $("#galeria-zdjecia").height(windowHeight*0.9);//0.8
        $("#galeria-zdjecia").width(windowWidth*0.8);
        
        $(".img").height(windowHeight*0.7);  
                
    </script>    
    <script type="text/javascript" src="/js/wyswietl_zdjecie.js"></script>
    <script type="text/javascript" src="/js/ext.js"     ></script>
    <script type="text/javascript" src="/js/captcha.js" ></script>
    <?php include 'autorstwo.php';?>      
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>
</body>
</html>