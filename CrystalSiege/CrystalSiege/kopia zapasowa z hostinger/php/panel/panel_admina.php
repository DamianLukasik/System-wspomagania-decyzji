<?php

if(isset($_SESSION['lektor']))
{
    if($_SESSION['lektor']=="1")
    {
        $lektor="Włącz głos lektora";
    }
    else
    {   
        $lektor="Wyłącz głos lektora";
    }    
}
else
{
    $lektor="Włącz głos lektora";
}

echo '<table id="pasek_" class="panel_użytkownika">
        <tr>            
            <td colspan="3">    
                <label id="lab_id" class="powitanie">'.'Dzień dobry panie Administratorze <a id="profil" href="/php/profil.php">'.$_SESSION['user'].'</a>!</label>'.'        
            </td>
        </tr>
        <tr>
            <td>
                <form action="/php/przeslij.php" method="post">
                    <input id="przycisk21" type="submit" value="Prześlij" class="przycisk-standard" style="width: 195px" >
                </form>  
            </td>
            <td>
                <form action="/php/przeszukaj.php" method="post">
                    <input id="przycisk22" type="submit" value="Przeszukaj" class="przycisk-standard" style="width: 195px" >
                </form>
            </td>
            <td>
                <form action="/php/baza_zdjec.php" method="post">
                    <input id="przycisk23" type="submit" value="Przeglądaj" class="przycisk-standard" >
                </form>            
            </td>
            <td>
                <form action="/php/edytuj.php" method="post">
                    <input id="przycisk24" type="submit" value="Edytuj profil" class="przycisk-standard">
                </form> 
            </td>
			<td>
                <form action="/index.php" method="post">
                    <input id="przycisk52" type="submit" value="Strona główna" class="przycisk-standard" >
                </form>                
            </td>
            <td style="width: 100%" >
                <form action="/logout.php" method="post">
                    <input id="przycisk25" style="float: right; width: 105px;" type="submit" value="Wyloguj" class="przycisk-standard">
                </form>  
            </td>
        </tr>
        <tr>
            <td>
                <form action="/php/zarzadzaj.php" method="post">
                    <input id="przycisk26" type="submit" value="Zarządzaj bazą" class="przycisk-standard" style="width: 195px" >
                </form> 
            </td>
            <td>
                <form action="/php/weryfikuj_zdjecia.php" method="post">
                    <input id="przycisk27" type="submit" value="Weryfikuj zdjęcia" class="przycisk-standard" style="width: 195px" >
                </form> 
            </td>
            <td>
                <form action="/php/odbierz_zgloszenie.php" method="post">
                    <input id="przycisk28" type="submit" value="Odbierz zgłoszenie" class="przycisk-standard">
                </form> 
            </td>
        </tr>
		<tr>
			<td>
			    <input id="przycisk99" type="button" value="" class="przycisk-standard" onclick="zmien_styl()" style="width: 195px" >
			</td>
                        <td>
			    <input id="przycisk98" type="button" value="'.$lektor.'" class="przycisk-standard" onclick="wylacz_glos()" style="width: 195px" >
			</td>
		</tr>
    </table> 
    <br>';        
