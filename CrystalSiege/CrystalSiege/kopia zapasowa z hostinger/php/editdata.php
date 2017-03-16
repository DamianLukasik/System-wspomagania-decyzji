<?php     
    session_start(); 
    if(isset($_POST['id']))
    {
        $l = $_POST['id']; 
        if(isset($_POST['mail']))
        {
            $d = $_POST['mail'];
            zmien_mail($l,$d);
        }
        else
        {
            if(isset($_POST['dane']))
            {
                $d = $_POST['dane'];
                if(isset($_POST['ver']))
                {
                    zmien_haslo($l,$d);
                }
                else
                {
                    zmien_nick($l,$d);
                }            
            }
            else
            {
                usun_konto($l);
            }
        }
    }
    
    function zmien_haslo($id,$dane)
    {
        $poprawnosc = true;
        require_once "connect.php";
        try
        {
            $conn = new mysqli($host,$db_user, $password,$name_db);            
            if($wynik = @$conn->query(
                sprintf("SELECT login, haslo FROM uzytkownicy WHERE id='%s'",
                mysqli_real_escape_string($conn,$id)
            )))
            {
                if($_POST['ver']=="0")
                {
                    $poprawnosc = false;
                    $_SESSION['error_haslo'] = "Nie zweryfikowano aktualnego hasła";
                }
                else
                {
                    if((strlen($dane)<8) || (strlen($dane)>20))
                    {
                        $poprawnosc = false;
                        $_SESSION['error_haslo'] = "Hasło musi posiadać od 8 do 20 znaków";
                    }
                }                
                if($poprawnosc)
                {
                    $haslo_hash = base64_encode(mcrypt_encrypt(MCRYPT_RIJNDAEL_256, md5('m98o87c5pe'), $dane, MCRYPT_MODE_CBC, md5(md5('m98o87c5pe'))));
                    if(@$conn->query(
                        sprintf("UPDATE uzytkownicy SET haslo='%s' WHERE id='%s'",
                        mysqli_real_escape_string($conn,$haslo_hash),
                        mysqli_real_escape_string($conn,$id)
                    ))){}
                }                    
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    function zmien_nick($id,$dane)
    {        
        require_once "connect.php";
        try
        {
            $conn = new mysqli($host,$db_user, $password,$name_db);            
            if($wynik = @$conn->query(
                sprintf("SELECT login FROM uzytkownicy WHERE login='%s'",
                mysqli_real_escape_string($conn,$dane)
            )))
            {
                if(!$wynik) throw new Exception($conn->error);
                $liczba_userow = $wynik->num_rows;
                if($liczba_userow>0)
                {
                    $wiersz = mysqli_fetch_array($wynik); 
                    $_SESSION['error_nick'] = "Nazwa ".$wiersz["login"]." jest już zajęta";
                }
                else
                {
                    if(@$conn->query(
                        sprintf("UPDATE zdjecia SET autor='%s' WHERE autor=(SELECT login FROM uzytkownicy WHERE id='%s')",
                        mysqli_real_escape_string($conn,$dane),
                        mysqli_real_escape_string($conn,$id)
                    )))
                    if(@$conn->query(
                        sprintf("UPDATE uzytkownicy SET login='%s' WHERE id='%s'",
                        mysqli_real_escape_string($conn,$dane),
                        mysqli_real_escape_string($conn,$id)
                    )))
                    $_SESSION['user']=$dane;                      
                }
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    } 
    
    function zmien_mail($id,$dane)
    {
        require_once "connect.php";
        try
        {
            $poprawnosc = true;
            $conn = new mysqli($host,$db_user, $password,$name_db);            
            if($wynik = @$conn->query(
                sprintf("SELECT login, email FROM uzytkownicy WHERE email='%s'",
                mysqli_real_escape_string($conn,$dane)
            )))
            {
                if(!$wynik) throw new Exception($conn->error);
                $ile_jest_takich_maili = $wynik->num_rows;                
                if($ile_jest_takich_maili>0)
                {                    
                    $poprawnosc = false;
                    $_SESSION['error_mail'] = "Istnieje już konto o takim adresie mailowym";
                }
                else
                {
                    if(filter_var($dane, FILTER_VALIDATE_EMAIL) === false) 
                    {
                        $poprawnosc = false;
                        $_SESSION['error_mail'] = "Podaj poprawny adres mailowy";
                    }
                    else
                    {
                        if($poprawnosc)
                        {
                            if($wynik = @$conn->query(
                                sprintf("SELECT email, login FROM uzytkownicy WHERE id='%s'",
                                mysqli_real_escape_string($conn,$id)
                                )))
                            {
                                $wiersz = mysqli_fetch_array($wynik);
                                $mail = $wiersz['email'];
                                $actCode=str_shuffle("qwertyuiopasdfghjklzxcvbnm1234567890");
                                if(@$conn->query(
                                sprintf("UPDATE uzytkownicy SET aktywacja='%s' WHERE id='%s'",
                                mysqli_real_escape_string($conn,$actCode),
                                mysqli_real_escape_string($conn,$id)
                                )))
                                {
									require_once 'default.php';
                                    $headers="MIME-Version: 1.0\r\n";
                                    $header .= "Content-type: text/html; charset=utf-8\n"; 
                                    $header .= "Content-Transfer-Encoding: 8bitr\n";  
                                    $content="\nCześć ".$wiersz['login'].", otrzymano żądanie zmiany twojego adresu mailowego z ".$mail." na ".$dane." w serwisie Archiwum Zdjęć Częstochowy!\n\n
                                    Wystarczy kliknąć w poniższy link aktywacyjny, aby zatwierdzić zmiany:\n".
                                    $adres_strony."/php/editdata.php?change_mail=".$actCode."&on_mail=".$dane."\n\n
                                    Jeśli mail trafił do Ciebie przez pomyłkę, proszę go zignorować\n\n
                                    Dziękuje za uwagę\n
                                    ~Administrator serwisu";
                                    $title = "=?UTF-8?B?".base64_encode("Archiwum Zdjec Czestochowy - Zmiana adresu mail")."?=";     
                                    mail($dane, $title, $content, $headers); 
                                }
                                else
                                {
                                    throw new Exception($conn->error);
                                }
                            }
                        }
                    }
                }
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    function usun_konto($id)
    {
        require_once "connect.php";
        try
        {
            $conn = new mysqli($host,$db_user, $password,$name_db);            
            if($wynik = @$conn->query(//coś nie działa
                sprintf("SELECT id, login, email FROM uzytkownicy WHERE id='%s'",
                mysqli_real_escape_string($conn,$id)
            )))
            {
                if(!$wynik) throw new Exception($conn->error);
                $ile_jest_kont = $wynik->num_rows;                
                if($ile_jest_kont>1)
                {
                    $_SESSION['error_konto'] = "Jest więcej kont o takiej nazwie użytkownika";
                }
                else
                {
                    echo 'ok';
                    $wiersz = mysqli_fetch_array($wynik);
                    $mail = $wiersz['email'];
                    $actCode=str_shuffle("qwertyuiopasdfghjklzxcvbnm1234567890");
                    if(@$conn->query(
                    sprintf("UPDATE uzytkownicy SET aktywacja='%s' WHERE id='%s'",
                    mysqli_real_escape_string($conn,$actCode),
                    mysqli_real_escape_string($conn,$id)
                    )))
                    {       
						require_once 'default.php';					
                        $headers="MIME-Version: 1.0\r\n";
                        $header .= "Content-type: text/html; charset=utf-8\n"; 
                        $header .= "Content-Transfer-Encoding: 8bitr\n";  
                        $content="\nCześć ".$wiersz['login'].", otrzymano żądanie usunięcia twojego konta w serwisie Archiwum Zdjęć Częstochowy!\n\n
                        Aby potwierdzić żądanie kliknij w poniższy link aktywacyjny. Pamiętaj, że dokonanej zmiany nie można cofnąć, administracja serwisu nie ponosi odpowiedzialności za decyzje użytkowników:\n".
                        $adres_strony."/php/editdata.php?cassation=".$actCode."\n\n
                        Jeśli mail trafił do Ciebie przez pomyłkę, proszę go zignorować\n\n
                        Dziękuje za uwagę\n
                        ~Administrator serwisu";
                        $title = "=?UTF-8?B?".base64_encode("Archiwum Zdjec Czestochowy - Usun konto")."?=";    
                        mail($mail, $title, $content, $headers);      
                    }
                    else
                    {
                        throw new Exception($conn->error);
                    }
                }
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    if($_GET["cassation"])
    {
        $kod  = $_GET["cassation"];
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
                if(@$conn->query(
                sprintf("UPDATE uzytkownicy SET data_usuniecia=SYSDATE() WHERE aktywacja='%s'",
                mysqli_real_escape_string($conn,$kod)
                )))
                {
                    print "Proces usuwania konta zakończona pomyślnie.";
                    session_unset(); 
                    header('Location: /index.php');
                }
                else
                {
                    print "Podano nieistniejący kod aktywacyjny.";                    
                    header('Location: /index.php');
                }
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    if(($_GET["change_mail"]) || ($_GET["on_mail"]))
    {
        $kod  = $_GET["change_mail"];
        $mail = $_GET["on_mail"];
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
                if(@$conn->query(
                sprintf("UPDATE uzytkownicy SET email='%s' WHERE aktywacja='%s'",
                mysqli_real_escape_string($conn,$mail),
                mysqli_real_escape_string($conn,$kod)
                )))
                {
                    print "Zmiana adresu mail zakończona pomyślnie.";
                    header('Location: /index.php');
                }
                else
                {
                    print "Podano nieistniejący kod aktywacyjny.";
                    header('Location: /index.php');
                }
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }