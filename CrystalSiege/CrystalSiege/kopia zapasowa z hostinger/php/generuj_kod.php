<?php
include 'zapis_do_pliku.php';

session_start();
if(!isset($_SESSION['zalogowany']))
{
    header('Location:/index.php');
    exit();
}

$nr = $_POST['nr'];
$byl= $_POST['byl'];
generuj_obraz($nr,$byl);

function losuj_kolor($cap) {
    $r     = rand(100, 250);
    $g     = rand(100, 250);
    $b     = rand(100, 250);
    $kolor = imagecolorallocate($cap, $r, $g, $b);
    return $kolor;
}

function generuj_obraz($nr,$byl) {
    
header('Content-Type: image/jpeg');

$tla           = glob("captcha_bcg/{*.jpg,*.jpeg}", GLOB_BRACE);
$czcionki      = glob("captcha_tff/*.ttf");

$znaki         = 'ABCDEFGHIJKLMNPQRSTUWXYZ123456789';

$obrazek_tla   = $tla[array_rand($tla)];
$liczba_znakow = rand(4, 6);

$cap           = imagecreatefromjpeg($obrazek_tla);
$kolor         = imagecolorallocate($cap, 250, 250, 250);

for($x = 1; $x <= 50; $x++)  // powtarzamy 50 razy - rysujemy 50 linii
{     
    $linie         = losuj_kolor($cap);
    imageline(                        // funkcja rysująca linię
        $cap,                         // uchwyt obrazka
        0,                            // współrzędna X początku linii
        rand(-100,imagesy($cap)+100), // współrzędna Y początku linii
        imagesx($cap),                // współrzędna X końca linii
        rand(-100,imagesy($cap)+100), // współrzędna Y końca linii
        $linie                        // kolor linii
    );   
}

$tekst = '';

for($x = 1; $x <= $liczba_znakow; $x++)
{
    $czcionka = $czcionki[array_rand($czcionki)];
    $znak     = $znaki[rand(0, strlen($znaki)-1)];

    $odleglosc_miedzy_znakami = (round(imagesx($cap) / $liczba_znakow+1)-10)*($x-1)+20;
    $tekst = $tekst.''.$znak;
    imagettftext(                      // funkcja pisząca tekst
     $cap,                             // uchwyt obrazka
     rand(20, 30),                     // rozmiar czcionki
     rand(-15, 15),                    // naczylenie znaku
     $odleglosc_miedzy_znakami,        // odległość między znakami
     rand(40, 60),                     // położenie względem górnej krawędzi obrazka
     $kolor,
     $czcionka,
     $znak
    );
}
// zapisanie pliku jako kod_obrazka.jpg
// recaptcha, zapisu generowanego obrazu do bazy danych, modyfikacja plik�w nie dzia�a.
imagejpeg($cap, 'kod_obrazka'.$nr.'.jpg');
imagedestroy($cap);
//usuwanie poprzedniego pliku
unlink('kod_obrazka'.$byl.'.jpg');

zapis($tekst);
echo $tekst;
}