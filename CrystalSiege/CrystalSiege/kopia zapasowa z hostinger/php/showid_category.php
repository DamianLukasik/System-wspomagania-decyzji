<?php     

    if(isset($_GET["wyb"]))
    {        
        $i = $_GET["wyb"];   
        $k = $_GET["kat"];
        załaduj($i,$k);
    }
    
    function załaduj($data,$kategoria)
    {        
        require_once "connect.php";
        try
        {
            $conn = new mysqli($host,$db_user, $password,$name_db);
            if($wynik = @$conn->query(
                sprintf("SELECT GROUP_CONCAT(id) AS wynik FROM zdjecia WHERE %s='%s'",
                mysqli_real_escape_string($conn,$kategoria),
                mysqli_real_escape_string($conn,$data)
            )))
            {
                if(!$wynik) throw new Exception($conn->error);
                $liczba_userow = $wynik->num_rows;
                if($liczba_userow>0)
                {
                    $wiersz = mysqli_fetch_array($wynik);                  
                    echo $wiersz['wynik'];
                }
            }
            
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
