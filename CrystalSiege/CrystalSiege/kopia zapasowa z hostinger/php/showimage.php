<?php     

    if(isset($_GET["id"]))
    {        
        $i = $_GET["id"];        
        załaduj($i);
    }
    
    function ładuj_zdjecie($typ,$obraz,$skala1,$skala2)
    {
        ob_start();
        if ($typ=="image/gif") {
            $image = imagecreatefromgif($obraz);
            $image = imagescale($image, $skala1, $skala2);
            imagegif($image);
        }
        if ($typ=="image/jpeg") {
            $image = imagecreatefromjpeg($obraz);
            $image = imagescale($image, $skala1, $skala2);
            imagejpeg($image);
        }
        if ($typ=="image/png") {
            $image = imagecreatefrompng($obraz);
            $image = imagescale($image, $skala1, $skala2);
            imagepng($image);
        }                               
        $contents = ob_get_contents();
        ob_end_clean();

        $zdjecie = 'data:'.$typ.';base64,'.base64_encode($contents).'';            
        imagedestroy($image); 

        return $zdjecie;   
    }
    
    function załaduj($id)
    {
        require_once "connect.php";
        try
        {
            $conn = new mysqli($host,$db_user, $password,$name_db);
            if($wynik = @$conn->query(
                sprintf("SELECT id, zdjecie, typ FROM zdjecia WHERE id='%s'",
                mysqli_real_escape_string($conn,$id)
            )))
            {
                if(!$wynik) throw new Exception($conn->error);
                $liczba_userow = $wynik->num_rows;
                if($liczba_userow>0)
                {
                    $wiersz = mysqli_fetch_array($wynik);  
                    echo ''.$wiersz['typ'].''.base64_encode( $wiersz['zdjecie'] );
                } 
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    function skaluj_obraz($stala,$obraz,$typ)
    {
        $resolution = '';
        if (isset($_COOKIE['resolution']))
        {
            $resolution = $_COOKIE['resolution'];
            $wyraz = explode("x",$resolution);
        }
        else
        {
            $wyraz[0] = 1280;
            $wyraz[1] = 960;
        }
                            
        $wyraz[0] = $wyraz[0]*$stala;
        $wyraz[1] = $wyraz[1]*$stala;
        
        $zdjecie = ładuj_zdjecie($typ,$obraz,$wyraz[0],$wyraz[1]);
        
        return $zdjecie;        
    }
    
