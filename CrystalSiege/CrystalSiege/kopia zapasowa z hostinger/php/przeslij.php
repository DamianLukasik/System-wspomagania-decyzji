<?php
    session_start();
    if(!isset($_SESSION['zalogowany']))
    {
        header('Location:/index.php');
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
                    echo "<label class='napis3' >Co dziś przesyłamy ".$_SESSION['user']."?</;abel>";
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
            
    <form action="upload.php" method="post" enctype="multipart/form-data">
        <table id="pasek_" class="formularz_przesyłania">
            <tr>
                <td class="opis" style="width: 70%">
                    <label id="lab_id" class="napis">Nazwa zdjęcia:</label>
                </td>                    
                <td>
                    <input id="przycisk30" type="text" name="nazwa" value="<?php
                        if(isset($_SESSION['fr_nazwa']))
                        {
                            echo $_SESSION['fr_nazwa'];   
                            unset($_SESSION['fr_nazwa']);
                        }
                    ?>" class="text-standard-przeslij"/>
                </td>
            </tr>
            <tr>    
                <td colspan="2">
                    <?php
                        if(isset($_SESSION['error_nazwa']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_nazwa'].'</span>';   
                            unset($_SESSION['error_nazwa']);
                        }
                    ?>
                </td>
            </tr>
            <tr>
                <td class="opis">
                    <label class="napis">Opis zdjecia (Co przedstawia? Co na nim widać?):</label>
                </td>                    
                <td>
                    <textarea id="przycisk31" name="opis" rows="5" class="obszar_tekstowy-przeslij"><?php
                            if(isset($_SESSION['fr_opis']))
                            {
                                echo $_SESSION['fr_opis'];   
                                unset($_SESSION['fr_opis']);
                            }?></textarea>
                </td>
            </tr>
            <tr>    
                <td colspan="2">
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
                <td class="opis">
                    <label class="napis">Data wykonania zdjęcia:</label>
                </td>                    
                <td class="wiersz-standard">
                    <input id="data" type="date" name="data" value="<?php
                        if(isset($_SESSION['fr_data']))
                        {
                            echo $_SESSION['fr_data'];   
                            unset($_SESSION['fr_data']);
                        }
                    ?>" class="data-standard-przeslij" max="2020-12-31" min="1890-01-01"/>                           
                </td>
            </tr>
            <tr>    
                <td colspan="2">
                    <?php
                        if(isset($_SESSION['error_data']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_data'].'</span>';   
                            unset($_SESSION['error_data']);
                        }
                    ?>
                </td>
            </tr>
            <tr>
                <td class="opis">
                    <label class="napis">Lokalizacja wykonania zdjęcia: </label>
                </td>                    
                <td>
                    <select id="przycisk32" name="lokalizacja" class="text-standard-przeslij">
                        <?php
                        $tablica = array("-brak-","-nieznane okolice-","Błeszno",
                            "Częstochówka-Parkitka","Dźbów","Gnaszyn-Kawodrza","Grabówka","Kiedrzyn",
                            "Lisiniec","Mirów","Ostatni Grosz","Podjasnogórska","Północ","Raków",
                            "Stare Miasto","Stradom","Śródmieście","Trzech Wieszczów","Tysiąclecie",
                            "Wrzosowiak","Wyczerpy-Aniołów","Zawodzie-Dąbie");
                        $ile = count($tablica);
                        $tekst="";
                        for($i=0; $i<$ile; $i++)
                        {
                            if(isset($_SESSION['fr_lokalizacja'])==$tablica[$i])
                            {
                                $tekst='selected="selected" ';
                            }
                            else
                            {
                                $tekst="";
                            }
                            echo '<option '.$tekst.">".$tablica[$i].'</option>';
                        }
                        unset($_SESSION['fr_lokalizacja']);
                        ?>
                    </select>
                </td>
            </tr>    
            <tr>    
                <td colspan="2">
                    <?php
                        if(isset($_SESSION['error_lokalizacja']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_lokalizacja'].'</span>';   
                            unset($_SESSION['error_lokalizacja']);
                        }
                    ?>
                </td>
            </tr>
            <tr>
                <td class="opis">
                    <label class="napis">Urządzenie jakim wykonano zdjęcie: </label>
                </td>                    
                <td>
                    <select id="przycisk33" name="urzadzenie" class="text-standard-przeslij">
                        <?php
                        $tablica = array("-brak-","-nieznane urządzenie-","Aparat analogowy",
                            "Aparat fotograficzny","Aparat cyfrowy","Komórka z aparatem",
                            "Smartfon","iPad","Tablet");
                        $ile = count($tablica);
                        $tekst="";
                        for($i=0; $i<$ile; $i++)
                        {
                            if(isset($_SESSION['fr_urzadzenie'])==$tablica[$i])
                            {
                                $tekst='selected="selected" ';
                            }
                            else
                            {
                                $tekst="";
                            }
                            echo '<option '.$tekst.">".$tablica[$i].'</option>';
                        }
                        unset($_SESSION['fr_urzadzenie']);
                        ?>
                    </select>
                </td>
            </tr>
            <tr>    
                <td colspan="2">
                    <?php
                        if(isset($_SESSION['error_urzadzenie']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_urzadzenie'].'</span>';   
                            unset($_SESSION['error_urzadzenie']);
                        }
                    ?>
                </td>
            </tr>
            <tr>
                <td class="opis">
                    <label class="napis">Tagi (powinny być oddzielone spacją)</label>
                </td>                    
                <td>
                    <input id="przycisk34" type="text" name="tag" value="<?php
                        if(isset($_SESSION['fr_tagi']))
                        {
                            echo $_SESSION['fr_tagi'];   
                            unset($_SESSION['fr_tagi']);
                        }
                    ?>" class="text-standard-przeslij"/>
                </td>
            </tr>
            <tr>    
                <td colspan="2">
                    <?php
                        if(isset($_SESSION['error_tagi']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_tagi'].'</span>';   
                            unset($_SESSION['error_tagi']);
                        }
                    ?>
                </td>
            </tr>
            <tr>
                <td id="upload-file-container">
                    <input id="przycisk35" class="plik-standard-przeslij" type="file" name="plik" value="" size="30">
                </td>                    
                <td>
                    <input id="przycisk36" class="przycisk-standard" type="submit" value="Prześlij">
                </td>
            </tr>
            <tr>    
                <td>
                    <?php
                        if(isset($_SESSION['error_plik']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_plik'].'</span>';   
                            unset($_SESSION['error_plik']);
                        }
                    ?>
                </td>
                <td>
                    <?php
                        if(isset($_SESSION['error_autor']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_autor'].'</span>';   
                            unset($_SESSION['error_autor']);
                        }
                    ?>
                </td>
            </tr>
        </table>
    </form>      
    </br></br>
    <?php include 'autorstwo.php';?>       
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>   
</body>
</html>

