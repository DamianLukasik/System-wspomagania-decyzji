<?php
    session_start();
    
    if((!isset($_POST['login'])) || (!isset($_POST['haslo'])))
    {
        header('Location:index.php');
        exit();
    }
    
    require_once "php/connect.php"; 
    
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
            $login = $_POST['login'];
            $haslo = $_POST['haslo'];
            $login = htmlentities($login,ENT_QUOTES,"UTF-8");     
            $blad  = 0;
            
            //echo 'dziala';
            if($wynik = @$conn->query(
                sprintf("SELECT id, zweryfikowany, login, haslo, uprawnienia, blokada FROM uzytkownicy WHERE login='%s' AND zweryfikowany=1 AND data_usuniecia IS NULL",
                mysqli_real_escape_string($conn,$login)
                )))
            {
                if(!$wynik) throw new Exception($conn->error);
                $liczba_userow = $wynik->num_rows;
                if($liczba_userow>0)
                {
                    $wiersz = mysqli_fetch_array($wynik);
                    $kodowane = $wiersz['haslo'];//dopracować
                    $zdekodowane = rtrim(mcrypt_decrypt(MCRYPT_RIJNDAEL_256, md5('m98o87c5pe'), base64_decode($kodowane), MCRYPT_MODE_CBC, md5(md5('m98o87c5pe'))),"\0");            
                    if($haslo==$zdekodowane)//w netbeans 1), na serwerze $haslo==$zdekodowane)
                    {       
                        $_SESSION['user']       = $wiersz['login'];
                        $_SESSION['prawa']      = $wiersz['uprawnienia'];
                        $_SESSION['idek']       = $wiersz['id'];
                        $_SESSION['blok']       = $wiersz['blokada'];
                        unset($_SESSION['blad']);
                        if((int)$wiersz['blokada']==1)//blokada nie działa
                        {
                            $blad = 1;//gdy jest zablokowany
                        }
                        else
                        {
                            $_SESSION['zalogowany'] = TRUE;                            
                        }
                        if(@$conn->query(
                        sprintf("UPDATE uzytkownicy SET data_ostatniego_logowania=SYSDATE() WHERE login='%s'",
                        mysqli_real_escape_string($conn,$login))))

                        $wynik->free_result();
                    }else{
                        $blad = 2;//gdy źle wpisuje hasło
                    }
                }else{
                    $blad = 0;//gdy wpisuje nazwe nieistniejacego uzytkownika
                }
            }
            $conn->close();
        }
        switch($blad)
        {
            case 0:
                $_SESSION['blad'] = 'Nie ma takiego użytkownika w bazie';
                break;
            case 1:
                $_SESSION['blad'] = 'Użytkownik został zablokowany';
                break;
            case 2:
                $_SESSION['blad'] = 'Nieprawidłowe hasło'; 
                break;
            default:
                break;
        }
        //header('Location:index.php');
    }
    catch(Exception $e)
    {
        //echo 'błąd </br> '.$e;
    }
?>