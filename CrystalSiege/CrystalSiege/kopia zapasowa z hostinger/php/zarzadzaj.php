<?php
    session_start();
    if(!isset($_SESSION['zalogowany']) || $_SESSION['prawa']==1)
    {
        header('Location:/index.php');
        exit();
    }  
    if(isset($_POST['uprawnienia']))
    {
        $blok  = $_POST['uprawnienia']; 
        $login = $_GET['login'];
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
                $sql = "UPDATE uzytkownicy SET uprawnienia='%s' WHERE login='%s'";
                              
                if(@$conn->query(sprintf($sql,mysqli_real_escape_string($conn,$blok),
                        mysqli_real_escape_string($conn,$login))))
                {
                    header('Location: zarzadzaj.php?baza=Użytkownicy');
                }
            }
            $conn->close();        
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    if(isset($_POST['blok']))
    {
        $blok  = $_POST['blok']; 
        $login = $_GET['login'];       
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
                $sql = "UPDATE uzytkownicy SET blokada='%s' WHERE login='%s'";
                              
                if(@$conn->query(sprintf($sql,mysqli_real_escape_string($conn,$blok),
                        mysqli_real_escape_string($conn,$login))))
                {
                    header('Location: zarzadzaj.php?baza=Użytkownicy');
                }
            }
            $conn->close();        
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    if(isset($_POST['skasuj']))
    {
        if($_POST['skasuj']==0)
        {
            $id=sprintf('%s',$_GET['id']);
            $tabela = "uzytkownicy";
            $str = 'Użytkownicy';
        }
        if($_POST['skasuj']==1)
        {
            $id=sprintf('%s',$_GET['id']);
            $tabela = "zdjecia";
            $str = 'Zdjęcia';
        }
        
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
				$wynik = @$conn->query("SELECT zdjecie, miniatura FROM zdjecia WHERE id=".$id."");
                $wiersz = mysqli_fetch_array($wynik);  
                unlink($wiersz['zdjecie']);
				unlink($wiersz['miniatura']);
                if(@$conn->query(
                sprintf("DELETE FROM %s WHERE id='%s'",
                mysqli_real_escape_string($conn,$tabela),
                mysqli_real_escape_string($conn,$id))))
                {
                    header('Location: zarzadzaj.php?baza='.$str);
                }
            }
            $conn->close();        
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    if(isset($_POST['zapisz']))
    {
        if($_POST['zapisz']==0)
        {
            $str = 'Użytkownicy';echo 'OK';
            $id=sprintf('%s',$_POST['id']);
            $login=sprintf('%s',$_POST['login']);
            $stary_login = $_POST['stary_login'];
            $war2 =sprintf('%s',$_POST['email']);
            $sql = 'UPDATE uzytkownicy SET login="'.$login.'", email="'.$war2.'" WHERE id="'.$id.'"';    
            if($id==$_SESSION['idek'])
            {
                echo 'ok';
                $_SESSION['user']=sprintf('%s',$_POST['login']);    
            }
        }
        if($_POST['zapisz']==1)
        {            
            $str = 'Zdjęcia';
            $war3 = sprintf('%s',$_POST['data']);
            $data = new DateTime($war3);
                //YYYY-MM-DD
                if($data>= new DateTime('1890-01-01')&& $data<= new DateTime('1914-07-28'))
                {
                    $okres = "Okres uprzemysłowienia";
                }
                if($data>= new DateTime('1914-07-28')&& $data<= new DateTime('1918-11-11'))
                {                        
                    $okres = "I wojna światowa";
                }
                if($data>= new DateTime('1918-11-11')&& $data<= new DateTime('1939-09-01'))
                {                        
                    $okres = "Okres międzywojenny";
                }
                if($data>= new DateTime('1939-09-01')&& $data<= new DateTime('1945-01-16'))
                {                                   
                    $okres = "II wojna światowa";
                }
                if($data>= new DateTime('1945-01-16')&& $data<= new DateTime('1952-07-22'))
                {                        
                    $okres = "Okres stabilizacji";
                }
                if($data>= new DateTime('1952-07-22')&& $data<= new DateTime('1989-06-04'))
                {                        
                    $okres = "Czasy PRL";
                }
                    if($data>= new DateTime('1989-06-04')&& $data<= new DateTime('1999-01-01'))
                {                        
                    $okres = "Przemianny ustrojowe";
                }
                if($data>= new DateTime('1999-01-01'))
                {                        
                    $okres = "Współczesność";
                }    
            $idek = sprintf('%s',$_POST['id']);
            $war1 = sprintf('%s',$_POST['nazwa']);
            $war2 = sprintf('%s',$_POST['opis']);            
            $war4 = sprintf('%s',$_POST['lokalizacja']);
            $war5 = sprintf('%s',$_POST['urzadzenie']);
            $war6 = sprintf('%s',$_POST['tag']);
            $sql = 'UPDATE zdjecia SET nazwa_zdjecia="'.$war1.'", opis_zdjecia="'.$war2.'",'
            . ' data_wykonania="'.$war3.'", lokalizacja="'.$war4.'", urzadzenie="'.$war5.'", '
            . 'tag="'.$war6.'", okres="'.$okres.'" WHERE id="'.$idek.'"';            
        }
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
                if(@$conn->query($sql))
                {           
                    if(isset($_POST['login']))
                    {             
                       // @$conn->query('UPDATE zdjecia SET autor="'.$_POST['login'].'" WHERE autor="'.$_GET['stary_login'].'"');                      
                    }  
                    header('Location: zarzadzaj.php?baza='.$str);                   
                }
            }
            $conn->close();        
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
        if(isset($_GET['cofnij'])){header('Location: weryfikuj_zdjecia.php?idek='.$idek);}
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
    <script>
    $(document).ready(function(){
        var blok=true;
        $('div').mouseover(function(){
		$('body').append('<div class="chmurka"/>');
                nazwa = $(this).text();
                var obiekt = $(this);
                $.ajax({
                    data: 'naz=' + nazwa,
                    url: 'wyswietl_miniature.php',
                    method: 'POST', // or GET
                    success: function(msg) {
                       // obiekt.append("<input type='image' src='"+msg+"'>");
                        if(blok){
                            blok=false;
                            $('div.chmurka').prepend("<input type='image' style='width: 500px;' src='"+msg+"'>").slideDown('fast');
                        }
                    }
                });		
	})
	.mouseout(function(){
		$('div.chmurka').hide().remove();
                blok=true;
	})
	.mousemove(function(e){
		$('div.chmurka').css('left',e.pageX + 20);
		$('div.chmurka').css('top',e.pageY - 100);
	});
    });
    </script>
</head>
<body>
    <!-- Pasek nawigacji   -->
    <?php include 'panel/panel_.php'; ?>    
    </br></br>   
    <center>
    <?php
        echo '<form method="get">';
        echo '<input id="guzik_wybierz" type="submit" value="Wybierz" class="przycisk-standard-mini">';
        echo '<select id="wybrano" name="baza" class="text-standard-przeslij"> ';

        $lab1 = "<label class='napis2'>";
        $lab2 = "</label>";
        
        $tablica = array('Użytkownicy','Zdjęcia');
        
        if(isset($_GET['baza']))
        {
            if($_GET['baza']==$tablica[0])
            {echo '334';
                echo '<option selected="selected">'.$tablica[0].'</option>';
                echo '<option >'.$tablica[1].'</option>'; 
                echo '</select></form>'; 
                echo '<table class="tabela_" > ';
            }
            if($_GET['baza']==$tablica[1])
            {
                echo '<option >'.$tablica[0].'</option>';
                echo '<option selected="selected">'.$tablica[1].'</option>';
                echo '</select></form>'; 
                echo '<table class="tabela_" > ';
            }
        }
        else
        {
            echo '<option selected="selected">'.$tablica[0].'</option>';
            echo '<option >'.$tablica[1].'</option>'; 
            echo '</select></form>'; 
            echo '<table class="tabela_" > ';
        }
         
        
        if(isset($_GET['baza']))
        {
            $baza = sprintf('%s',$_GET['baza']);  
            echo '<br><label id="wybrana_tabela" class="napis" >Zawartość tabeli '.$baza.'</label>'; 
            if(isset($_GET['sort']))
            {
                $sortuj = sprintf('%s',$_GET['sort']);
                $_SESSION['sort']=sprintf('%s',$_GET['sort']);
                echo '<br>Posortowane wg. '.$_GET['sort'];
                if($_SESSION['sort_order'] == 1)
                {
                    echo ' w kolejności rosnącej';
                    $kolejnosc = "ASC";
                    $_SESSION['sort_order'] = 0;
                }
                else
                {
                    echo ' w kolejności malejącej';
                    $kolejnosc = "DESC";      
                    $_SESSION['sort_order'] = 1;
                }                
            }
            else
            {
                $sortuj = "id";
                $kolejnosc = "ASC";  
                $_SESSION['sort_order'] = 1;
            }              
            $ilosc=0;
            $td1 = '<td class="tabela">';
            $td2 = '</td>';
            $td3 = '</td><td class="tabela">';           
            try
            {
                require_once "connect.php";
                $conn = new mysqli($host,$db_user, $password,$name_db);
                if($baza == 'Użytkownicy')
                {
                    $sql = "SELECT * FROM uzytkownicy";
                }
                if($baza == 'Zdjęcia')
                {
                    $sql = "SELECT (SELECT login FROM uzytkownicy WHERE id=T1.id_autora) AS login, zweryfikowany, data_weryfikacji, data_przeslania, tag, okres, urzadzenie, T1.id, typ, opis_zdjecia, nazwa_zdjecia, data_wykonania, nazwa_pliku, lokalizacja, miniatura FROM zdjecia AS T1";
                }
                if(isset($_GET['id']))
                {
                    $sql = $sql." WHERE id='".$_GET['id']."'";
                    $styl_sort = "hidden";
                }
                else
                {
                    $sql = $sql." ORDER BY ".$sortuj." ".$kolejnosc;
                    $styl_sort = "visible";
                }                
                $sort1 = '<form action="zarzadzaj.php?baza='.$baza.'&sort=';
                $sort2 = '" method="post"><input id="sort';
                $sort3 = '" type="image" style="visibility: '.$styl_sort.'" src="/grafika/S.png" height=20 width=20></form>';
                if($baza == 'Użytkownicy')
                {
                    echo '<tr>'.$td1.$lab1.'id'.$lab2.$sort1.'id'.$sort2.'0'.$sort3.$td3.$lab1.'login'.$lab2.$sort1.'login'.$sort2.'1'.$sort3
                    .$td3.$lab1.'e-mail'.$lab2.$sort1.'email'.$sort2.'2'.$sort3.$td3.$lab1.'zapisz zmiany'.$lab2.$td3.$lab1.'data dołączenia'.$lab2
                    .$sort1.$lab1.'data_dolaczenia'.$lab2.$sort2.'3'.$sort3.$td3.$lab1.'data ostatniego logowania'.$lab2
                    .$sort1.'data_ostatniego_logowania'.$sort2.'4'.$sort3.$td3.$lab1.'data usunięcia'.$lab2
                    .$sort1.'data_usuniecia'.$sort2.'5'.$sort3.$td3.$lab1.'blokada'.$lab2
                    .$sort1.'blokada'.$sort2.'6'.$sort3.$td3.$lab1.'uprawnienia'.$lab2.$sort1.'uprawnienia'.$sort2.'7'.$sort3.$td3
                    .$lab1.'Zwer'.$lab2.$sort1.'zweryfikowany'.$sort2.'8'.$sort3.$td2.'</tr>';
                   
                }
                if($baza == 'Zdjęcia')
                {
                    echo '<tr>'.$td1.$lab1.'id'.$lab2.$sort1.'id'.$sort2.'0'.$sort3.$td3.$lab1.'typ'.$lab2.$td3.$lab1.'nazwa pliku'.$lab2
                    .$sort1.'nazwa_pliku'.$sort2.'1'.$sort3.$td3.$lab1.'nazwa zdjęcia'.$lab2.$sort1.'nazwa_zdjecia'.$sort2.'2'.$sort3
                    .$td3.$lab1.'opis zdjęcia'.$lab2.$td3.$lab1.'data wykonania'.$lab2.$sort1.$lab1.'data_wykonania'.$lab2.$sort2.'3'.$sort3.$td3.$lab1.'lokalizacja'.$lab2
                    .$sort1.'lokalizacja'.$sort2.'4'.$sort3.$td3.$lab1.'urządzenie'.$lab2.$sort1.'urzadzenie'.$sort2.'5'.$sort3.$td3.$lab1.'tagi'.$lab2
                    .$td3.$lab1.'okres'.$lab2.$sort1.'okres'.$sort2.'6'.$sort3.$td3.$lab1.'autor'.$lab2.$sort1.'autor'.$sort2.'7'.$sort3.$td3
                    .$lab1.'Zwer'.$lab2.$sort1.'zweryfikowany'.$sort2.'8'.$sort3
                    .$td3.$lab1.'data przeslania'.$lab2.$sort1.'data_przeslania'.$sort2.'9'.$sort3
                    .$td3.$lab1.'data weryfikacji'.$lab2.$sort1.'data_weryfikacji'.$sort2.'10'.$sort3
                    .$td2.'</tr>';                   
                }                
                if($wynik = @$conn->query($sql))
                {  
                    if ($wynik->num_rows > 0) 
                    {
                        // output data of each row
                        $in=0;
                        while($wiersz = $wynik->fetch_assoc()) {
                            if($wiersz['zweryfikowany']==1)
                            {
                                $upublicznione = '<center><img id="V_'.$in.'" src="/grafika/V.gif" height=25 width=25></center>';           
                            }
                            else
                            {
                                $upublicznione = '<center><img id="X_'.$in.'" src="/grafika/X.gif" height=25 width=25></center>';    
                            }
                            if($baza == 'Użytkownicy')
                            {
                                $button = '<td class="tabela_"><form action="zarzadzaj.php?id='.$wiersz['id'].'" '
                                . 'method="post"><input id="ukryty" class="ukryty" name="skasuj" value="0"><input id="skasuj_to_'.$in.'" type="submit" '
                                . 'value="Usuń" class="przycisk-ikona-usun" style="background-color: #c86f82"></form></td>';
                                $button_edit = '<input id="ukryty" class="ukryty" name="zapisz" value="0"><input id="zapisz_to_'.$in.'" type="button" onclick="zapisz_rubryke('.$in.')" '
                                . 'value="Zapisz" class="przycisk-ikona-zmien"></form>';
                                $zablokuj1='<form action="zarzadzaj.php?login='.$wiersz['login'].'" method="post"><input '
                                . 'class="ukryty" id="ukryty" name="blok" value="';      
                                $zablokuj2=' class="przycisk-ikona-zmien"></form>';   
                                $zablokuj3='<form action="zarzadzaj.php?login='.$wiersz['login'].'" method="post"><input '
                                . 'class="ukryty" id="ukryty" name="uprawnienia" value="';  
                                $ukryj = 'visibility';
                                if($_SESSION['idek']==$wiersz['id'] || $wiersz['uprawnienia']==2)
                                {
                                    $ukryj = 'hidden';
                                }
                                if($wiersz['blokada']==0)
                                {
                                    $zablokuj = $zablokuj1.'1"><input id="zablokuj_to_'.$in.'" style="visibility: '.$ukryj.'" type="submit" value="Zablokuj"'.$zablokuj2;                               
                                }
                                else
                                {
                                    $zablokuj = $zablokuj1.'0"><input id="zablokuj_to_'.$in.'" style="visibility: '.$ukryj.'" type="submit" value="Odblokuj"'.$zablokuj2; 
                                }
                                if($wiersz['uprawnienia']==1)
                                {
                                    $uprawnienia = $zablokuj3.'2">'.$lab1.'User'.$lab2.'</br><input id="uprawnij_to_'.$in.'" type="submit" value="Zmień"'.$zablokuj2;                               
                                }
                                if($wiersz['uprawnienia']==2)
                                {
                                    $uprawnienia = $zablokuj3.'1">'.$lab1.'Admin'.$lab2.'</br><input id="uprawnij_to_'.$in.'" type="submit" value="Zmień"'.$zablokuj2; 
                                }
                                
                                echo '<tr>'.$td1.'<label class="napis2" id="id_'.$in.'" >'.$wiersz['id'].$lab2.'<input class="ukryty" id="il_'.$in.'" value="'.$wiersz['login'].'">'
                                .$td3.'<input id="login_'.$in.'" type="text" value="'.$wiersz['login'].'" class="text-standard-mini" name="login">'
                                .$td3.'<input id="email_'.$in.'" type="text" value="'.$wiersz['email'].'" class="text-standard-mini" name="email">'
                                .$td3.$button_edit.$td3.$lab1.$wiersz['data_dolaczenia'].$lab2.$td3.$lab1.$wiersz['data_ostatniego_logowania'].$lab2
                                .$td3.$lab1.$wiersz['data_usuniecia'].$lab2.$td3.$zablokuj.$td3.$uprawnienia
                                .$td3.$upublicznione.$td2.''.$button.'</tr>';
                            }
                              
                            if($baza == 'Zdjęcia')
                            {
                                $button = '<form action="zarzadzaj.php?id='.$wiersz['id'].'" '
                                . 'method="post"><input id="ukryty" class="ukryty" name="skasuj" value="1"><input id="usun_to_'.$in.'" type="submit" '
                                . 'value="Usuń" class="przycisk-ikona-usun" style="background-color: #c86f82"></form></td>';
                                if(isset($_GET['id']))
                                {
                                    echo '<tr><form action="zarzadzaj.php?zapisz=1&id='.$wiersz['id'].'&cofnij=1';
                                }
                                else
                                {
                                    echo '<tr><form action="zarzadzaj.php?zapisz=1&id='.$wiersz['id'];
                                }
                                if(isset($_GET['id']))
                                {
                                    $button_edit = '<td><input id="ukryty" class="ukryty" name="zapisz" value="1">'
                                    . '<input id="zapisz_to_'.$in.'" type="button" onclick="zapisz_rubryke2('.$in.','.$wiersz['id'].')" '
                                    . 'value="Zapisz" class="przycisk-ikona-dluzszy">';   
                                }
                                else
                                {
                                    $button_edit = '<td><input id="ukryty" class="ukryty" name="zapisz" value="1">'
                                    . '<input id="zapisz_to_'.$in.'" type="button" onclick="zapisz_rubryke('.$in.')" '
                                    . 'value="Zapisz" class="przycisk-ikona-dluzszy">';   
                                }                          
                                echo '" method="post">'.$td1.'<label class="napis2" id="id_'.$in.'" >'.$wiersz['id'].$lab2
                                .$td3.$lab1.substr($wiersz['typ'], -4).$lab2
                                .$td3.'<div>'.$lab1.$wiersz['nazwa_pliku'].$lab2.'</div>'
                                .$td3.'<input id="nazwa_zdjecia'.$in.'" type="text" value="'.$wiersz['nazwa_zdjecia'].'" class="text-standard-mini" name="nazwa">'
                                .$td3.'<input id="opis_zdjecia'.$in.'" type="text" value="'.$wiersz['opis_zdjecia'].'" class="text-standard-mini" name="opis">'
                                .$td3.'<input id="data_wykonania'.$in.'" type="date" value="'.$wiersz['data_wykonania'].'" class="data-standard-przeslij" name="data">';
				echo $td3.lokalizacja($wiersz['lokalizacja'],$in);
                                echo $td3.urzadzenie($wiersz['urzadzenie'],$in);//'<input type="text" value="'.$wiersz['urzadzenie'].'" class="text-standard-mini" name="urzadzenie">'
                                echo $td3.'<input id="tag_'.$in.'" type="text" value="'.$wiersz['tag'].'" class="text-standard-mini" name="tag">'
                                .$td3.$lab1.$wiersz['okres'].$lab2.$td3.$lab1.$wiersz['login'].$lab2.$td3.$upublicznione
                                .$td3.$lab1.$wiersz['data_przeslania'].$lab2.$td3.$lab1.$wiersz['data_weryfikacji'].$lab2
                                .$button_edit.'</form>'.$button.'</tr>';
                            }
                            $in++;
                        }
                    }
                    else
                    {
                        echo "0 results";
                    }
                }
                $conn->close();            
            }
            catch(Exception $e)
            {
                echo 'błąd </br> '.$e;
            }
        }
        
        function urzadzenie($wiersz,$idek) {
            $tablica = ["-nieznane urządzenie-","Aparat analogowy",
                           "Aparat fotograficzny","Aparat cyfrowy","Komórka z aparatem",
                           "Smartfon","iPad","Tablet"];        
            $ile = count($tablica);
            $tekst="";
            $opcje='';
            for($i=0; $i<$ile; $i++)
            {
                $opcje = $opcje.'<option '.select($wiersz,$tablica[$i]).'>'.$tablica[$i].'</option>';
            }            
            return '<select id="urzadzenie__'.$idek.'"name="urzadzenie" class="text-standard-mini">'.$opcje. '</select>';                              
        }
        
        function lokalizacja($wiersz,$idek) {
            $tablica = ["-brak-","-nieznane okolice-","Błeszno",
                            "Częstochówka-Parkitka","Dźbów","Gnaszyn-Kawodrza","Grabówka","Kiedrzyn",
                            "Lisiniec","Mirów","Ostatni Grosz","Podjasnogórska","Północ","Raków",
                            "Stare Miasto","Stradom","Śródmieście","Trzech Wieszczów","Tysiąclecie",
                            "Wrzosowiak","Wyczerpy-Aniołów","Zawodzie-Dąbie"];           
            $ile = count($tablica);
            $tekst="";
            $opcje='';
            for($i=0; $i<$ile; $i++)
            {
                $opcje = $opcje.'<option '.select($wiersz,$tablica[$i]).'>'.$tablica[$i].'</option>';
            }            
            return '<select id="lokalizacja__'.$idek.'" name="lokalizacja" class="text-standard-mini">'.$opcje. '</select>';                              
        }
                            
        function select($argument,$test){
            if($argument==$test){return 'selected="selected" ';}
        }
        
        echo '</table>';        
    ?>
    </center>
    </br></br>
    <script>
    
    function zapisz_rubryke2(idek,i_)
    {
         var id    = $("#id_"+idek).html();
            var opis  = $("#opis_zdjecia"+idek).val();
            var nazwa = $("#nazwa_zdjecia"+idek).val();
            var tag   = $("#tag_"+idek).val();
            var data  = $("#data_wykonania"+idek).val();
            var loka  = $("#lokalizacja__"+idek).val();
            var urza  = $("#urzadzenie__"+idek).val();
            $.ajax({
                data: 'zapisz=1&data='+data+'&lokalizacja='+loka+'&urzadzenie='+urza+'&nazwa='+nazwa+'&tag='+tag+'&opis='+opis+'&id='+id,
                url: 'zarzadzaj.php',
                method: 'POST', // or GET
                success: function(msg) {
                    location.assign("weryfikuj_zdjecia.php?idek="+i_);
                }
            });
        //
    }
    
    function zapisz_rubryke(idek)
    {
        var wybrano = $("#wybrano").val();        
        if(wybrano=="Użytkownicy")
        {
            var login = $("#login_"+idek).val();
            var email = $("#email_"+idek).val();
            var id = $("#id_"+idek).html();
            var li = $("#il_"+idek).val();
            $.ajax({
                data: 'zapisz=0&login='+login+'&stary_login='+li+'&id='+id+'&email='+email,
                url: 'zarzadzaj.php',
                method: 'POST', // or GET
                success: function(msg) {
                    location.reload();
                }
            });
        }
        else
        {    
            var id    = $("#id_"+idek).html();
            var opis  = $("#opis_zdjecia"+idek).val();
            var nazwa = $("#nazwa_zdjecia"+idek).val();
            var tag   = $("#tag_"+idek).val();
            var data  = $("#data_wykonania"+idek).val();
            var loka  = $("#lokalizacja__"+idek).val();
            var urza  = $("#urzadzenie__"+idek).val();
            $.ajax({
                data: 'zapisz=1&data='+data+'&lokalizacja='+loka+'&urzadzenie='+urza+'&nazwa='+nazwa+'&tag='+tag+'&opis='+opis+'&id='+id,
                url: 'zarzadzaj.php',
                method: 'POST', // or GET
                success: function(msg) {
                    location.reload();
                }
            });
        }
        
        
        
    }
    
    </script>
    <?php include 'autorstwo.php';?>       
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>
</body>
</html>