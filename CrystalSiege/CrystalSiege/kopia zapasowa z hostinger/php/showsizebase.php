<?php     

    if(isset($_GET["wyb"]))
    {        
        $i = $_GET["wyb"];     
        $k = $_GET["kat"]; 
        załaduj($i,$k);
    }

    function załaduj($id,$kategoria)
    {
        require_once "connect.php";
        try
        {
            $conn = new mysqli($host,$db_user, $password,$name_db);
            if($wynik = @$conn->query(
                sprintf("SELECT COUNT(*) AS size FROM zdjecia WHERE %s='%s'",
                mysqli_real_escape_string($conn,$kategoria),
                mysqli_real_escape_string($conn,$id)
            )))
            {
                if(!$wynik) throw new Exception($conn->error);
                $liczba_userow = $wynik->num_rows;
                if($liczba_userow>0)
                {
                    $wiersz = mysqli_fetch_array($wynik);  
                    echo $wiersz['size']."";
                }
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
