<?php     

    if(isset($_GET["wyb"]))
    {        
        $w = $_GET["wyb"];   
        $i = $_GET["idek"];
        $k = $_GET["kat"];
        załaduj($w,$i,$k);
    }

    $idek=0;
    
    function załaduj($wybor,$id,$kategoria)
    {     
        require_once "connect.php";
        try
        {
            $conn = new mysqli($host,$db_user, $password,$name_db);
            if($wynik = @$conn->query(
                sprintf("SELECT id, zdjecie, typ FROM zdjecia WHERE %s='%s' AND id='%s'",
                mysqli_real_escape_string($conn,$kategoria),
                mysqli_real_escape_string($conn,$wybor),
                mysqli_real_escape_string($conn,$id)
            )))
            {
                if(!$wynik) throw new Exception($conn->error);
                $liczba_userow = $wynik->num_rows;
                if($liczba_userow>0)
                {
                    $wiersz = mysqli_fetch_array($wynik);                  
                    echo $wiersz['id']."=".$wiersz['typ']."".base64_encode( $wiersz['zdjecie'] );//odbiór danych binarnych
                }
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }

