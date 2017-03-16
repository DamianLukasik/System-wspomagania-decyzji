<?php
    require_once 'showimage.php';
    
    if(isset($_POST['naz']))
    {
        $nazwa=$_POST['naz'];
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
                if($wynik = @$conn->query(
                    sprintf("SELECT typ, miniatura, nazwa_zdjecia FROM zdjecia WHERE nazwa_pliku='%s'",
                    mysqli_real_escape_string($conn,$nazwa)
                    )))
                {
                    $wiersz = mysqli_fetch_array($wynik);
                    if($wiersz['miniatura']!=NULL)
                    {
                        $stala=0.27;                    
                        $zdjecie = skaluj_obraz($stala,$wiersz['miniatura'],$wiersz['typ']);   
                        echo $zdjecie;
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