<?php
$blad = '';
if(isset($_SESSION["blad"]))
{ 
    $blad = $_SESSION['blad'];
    unset($_SESSION['blad']);
}
    
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
            <td>
                <label id="lab_id" class="powitanie">Witaj gościu</label>'.'        
            </td>
        </tr>
        <tr>
            <td>
                <input id="przycisk01" value="Logowanie" type="button" class="przycisk-standard" onclick="zaloguj()" style="width: 195px" />
                <span class="komunikat_błąd">'.$blad.'</span>
            </td>
            <td>
                <form action="/zarejestruj.php" method="post">
                    <input id="przycisk02" type="submit" value="Rejestracja" class="przycisk-standard" style="width: 195px" >
                </form>                
            </td>
            <td>
                <form action="/php/przeszukaj.php" method="post">
                    <input id="przycisk22" type="submit" value="Przeszukaj" class="przycisk-standard" >
                </form> 
            </td>
            <td>
                <form action="/php/baza_zdjec.php" method="post">
                    <input id="przycisk23" type="submit" value="Przeglądaj" class="przycisk-standard" >
                </form>                
            </td>
			<td>
                <form action="/glowna.php" method="post">
                    <input id="przycisk52" type="submit" value="Strona główna" class="przycisk-standard" >
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
    <br>
    <table id="panel_logowania" class="pole-standard">
        <tr id="pole-1" style="visibility: hidden;" >
            <td>
                <label class="napis">Login: </label><input id="przycisk05" type="text" name="login" class="text-standard"/>
			</td>
			<td>
                <label class="napis">Hasło: </label><input id="przycisk06" type="password" name="haslo" class="text-standard"/>
			</td>
			<td>
                    <input id="przycisk07" type="submit" value="Zaloguj" class="przycisk-standard" onclick="przejd_do_strony_1()">
					<script>
					function przejd_do_strony_1() {
						var a = $("#przycisk05").val();
						var b = $("#przycisk06").val();
						var http = new XMLHttpRequest();
						var url = "/zaloguj.php";
						var params = "login="+a+"&haslo="+b+"";
						http.open("POST", url, true);
						http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
						http.onreadystatechange = function() {
							if(http.readyState == 4 && http.status == 200) {
								location.assign("/glowna.php");
							}
						}
						http.send(params);						
					}
                                        </script> 
			</td>
			<td>
                <input id="przycisk08" type="button" value="Przypomnij hasło" class="przycisk-standard" onclick="przejd_do_strony_2()">
                <script>
                function przejd_do_strony_2() {
                    window.open("/php/przypomnij_haslo.php");
                }
                </script> 
            </td>
            <td>
                <form action="/php/formularzkontaktowy.php" method="post">
                    <input id="przycisk09" type="submit" value="Zgłoś problem" class="przycisk-standard">
                </form>
            </td>
        </tr>        
    </table>
    <script>
        var tryb_logowania = false;        
        function zaloguj() {
            tryb_logowania = !tryb_logowania;
            if(tryb_logowania)
            {
                document.getElementById("pole-1").style.visibility = "visible";
            }
            else
            {
                document.getElementById("pole-1").style.visibility = "hidden";
            }    
        }       
    </script>';