<?php
    session_start();
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
                    <input id="przycisk52" type="submit" value="Wycofaj" class="przycisk-standard" style="width: 195px">
                    <?php
                        echo "<label id='lab_id' class='napis3'>Regulamin serwisu Archiwum Zdjęć Częstochowy</label>";
                    ?>
                </form>
            </td>
        </tr>
        <tr>
            <td>
                <input id="przycisk99" type="button" value="Dla słabowidzących" class="przycisk-standard" onclick="zmien_styl()" style="width: 195px" >
            </td>
	</tr>
    </table>
    <br>
    <!-- Przykładowy regulamin -->
<center><h1>Regulamin serwisu Archiwum Zdjęć Częstochowy</h1>
    <div align="justify" style="width: 75%;" >
    <ol type="1">
    <li>Niniejszy regulamin dotyczy serwisu Archiwum Zdjęć Częstochowy i każdy użytkownik akceptuje go od chwili aktywacji konta poprzez link aktywacyjny otrzymany w wiadomości e-mail.</li>
    <li>Każdy zarejestrony użytkownik wyraża zgodę na przetwarzanie danych osobowych.</li>
	<li>Administratorem danych, a także twórcą serwisu jest mieszkaniec Częstochowy - Damian Łukasik.</li>
	<li>Przechowywanie danych osobowych ma na celu umożliwienie zarejestrowanym użytkownikom możliwość przesyłania zdjęć, które po zweryfikowaniu przez administratora zostaną upublicznione w serwisie.</li>
	<li>Każdy użytkownik ma prawo do wglądu do swoich danych osobowych oraz do ich zmiany.</li>
	<li>Użytkownik w chwili rejestacji w systemie podaje swoje dane osobowe dobrowolnie.</li>
	<li>Użytkownicy muszą stosować się do zasad <i>Netykiety</i>.</li>
    <li>Serwis oferuje nowym użytkownikom możliwość bezpłatnej rejestracji, przeglądania oraz przeszukiwania galerii. Natomiast przesyłanie zdjęć oraz edycja danych profilowych jest możliwa tylko użytkownikom zarejestrowanym.</li>
    <li>Serwis stanowi bazę danych przechowującą zdjęcia przedstawiające krajobraz miejski Częstochowy.</li>
    <li>Zakazuje się zamieszczania zdjęć:</li>
    <ul>
        <li>Zawierające treści powszechnie uznawane za obelżywe i naruszające dobre obyczaje.</li>
        <li>Nawołujące do nienawiści, naruszające godność osób trzecich i zakazane przez Prawo Polskie.</li>
        <li>Naruszające prawa autorskie i prawo do wizerunku.</li>
        <li>Zawierające treści o tematyce erotycznej, propagujące zachowania szkodliwe społecznie, obrażające uczucia religijne.</li>
        <li>Przedstawiające sceny z poza terenu miasta Częstochowy.</li>
        <li>Przedstawiające to samo, w tym samym kadrze, a czas wykonania zdjęcia jest zbliżony.</li>
        <li>Przedstawiające sceny przemocy, śmierci, egzekucji, martwych zwierząt, budzące odrazę, strach lub niesmak.</li>
        <li>Przedstawiające osobę wykonującą zdjęcie tzw. <i>selfie.</i></li>
        <li>Przedstawiającą w dużej części zdjęcia osobę np.: <i>portret.</i></li>
        <li>Stanowiące reklamę lub inną formę autopromocji.</li>
        <li>Przedstawiające składowisko odpadów lub pojemniki komunalne.</li>
        <li>Nie ukazujący krajobrazu miejskiego.</li>
        <li>Bardzo zaciemnionych lub bardzo rozjaśnionych, lub o złej jakości technicznej.</li>
        <li>Nie podobające się administratorowi, przy czym <u>administrator ma prawo odmowy wyjaśnienia.</u></li>
    </ul>
    <li>Administrator nie ponosi odpowiedzialności za zamieszczane zdjęcia przez użytkowników.</li>
    <li>Wszelkie konflikty użytkownicy załatwiają między sobą.</li>
    <li>Administrator ma wszelkie prawo do zarządzania bazą danych zdjęć oraz użytkowników, zachowując przy tym Ochronę Danych Osobowych.</li>
    <li>Wszelkie problemy techniczne należy zgłaszać Administracji za pośrednictwem opcji <i>„Zgłoś problem”</i>.</li>
    <li>Użytkownicy mają obowiązek zgłaszać osobę łamiącą regulamin.</li>
    <li>Za złamanie regulaminu użytkownik będzie naganiany, w skrajnych przypadkach jego konto zostanie zablokowane.</li>
    <li>Administracja zastrzega sobie prawo do wprowadzania zmian regulaminu.</li>
    </ol>
    </div>
    <h2>Polityka cookies</h2>
    <div align="justify" style="width: 75%;" >
    <ol  type="1">
    <li>Poprzez pliki „cookies” należy rozumieć dane informatyczne,
        w szczególności pliki tekstowe, przechowywane w urządzeniach
        końcowych użytkowników przeznaczone do korzystania z serwisu.</li>
    <li>Pliki „cookies” używane są do utrzymywania sesji, przechowywania
        kodu Captchy potrzebnego w procesie rejestracji oraz w mechaniźmie
        odtwarzania zdjęć w formie pokazu slajdów.</li>
    <li>
        Rezygnacja z plików cookies może wpłynąć na funkcjonalności dostępne
        na stronie serwisu, w szczególności utrzymywanie sesji, odtwarzanie zdjęć
        w formie pokazów slajdów oraz przechowywania kodu Captchy w procesie rejestracji.
    </li>
    </ol>
    </div>
    </br>
    </br>
    </center>
    <?php include 'autorstwo.php';?>
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>
</body>
</html>
