<?php
    
    function zwroc_nastepnik()
    {
        $id = $_SESSION['id']; 
        $war = $_SESSION['galeria'];
        $rubryka = $_SESSION['katalog'];  
        
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
                sprintf("SELECT id FROM zdjecia WHERE %s='%s' AND id>'%s' "
                ."AND zweryfikowany=1 ORDER BY id ASC LIMIT 1",
                mysqli_real_escape_string($conn,$rubryka),
                mysqli_real_escape_string($conn,$war),
                mysqli_real_escape_string($conn,$id)
                )))
                {
                    $liczba = mysqli_fetch_array($wynik); 
                    if($liczba['id']==NULL)
                    {
                        if($wynik = @$conn->query(
                        sprintf("SELECT id FROM zdjecia WHERE %s='%s' AND id<'%s' "
                        ."AND zweryfikowany=1 ORDER BY id ASC LIMIT 1",
                        mysqli_real_escape_string($conn,$rubryka),
                        mysqli_real_escape_string($conn,$war),
                        mysqli_real_escape_string($conn,$id)
                        )))
                        {
                            $liczba = mysqli_fetch_array($wynik); 
                            $_SESSION['id_next'] = $liczba['id'];
                        }
                    }
                    $_SESSION['id_next'] = $liczba['id'];
                }
                $_SESSION['id'] = $liczba['id'];
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    