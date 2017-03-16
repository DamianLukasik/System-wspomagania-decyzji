<?php     

    if((isset($_POST["id"])) || (isset($_POST["co"])))
    {        
        $i = sprintf('%s',$_POST["id"]); 
        $s = sprintf('%s',$_POST["co"]);
        załaduj($i,$s);
    }

    function załaduj($id,$co)
    {
        require_once "connect.php";
        try
        {
            $conn = new mysqli($host,$db_user, $password,$name_db);
            if($wynik = @$conn->query("SELECT * FROM uzytkownicy WHERE id='".$id."'"))
            {
                if(!$wynik) throw new Exception($conn->error);
                $liczba_userow = $wynik->num_rows;
                if($liczba_userow>0)
                {
                    $wiersz = mysqli_fetch_array($wynik); 
                    if($co=='haslo'){                        
                        $haslo = $wiersz[$co];
                        $dane = rtrim(mcrypt_decrypt(MCRYPT_RIJNDAEL_256, md5('m98o87c5pe'), base64_decode($haslo), MCRYPT_MODE_CBC, md5(md5('m98o87c5pe'))),"\0");
                        echo iconv("ISO-8859-2","UTF-8", $dane)."";//kodowanie w netbeans nie działa, na stronie jest ok i wyświetla hasło
                    }
                    else
                    {
                        echo $wiersz[$co];  
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
