<?php
    require_once 'showimage.php';
 
    if(isset($_POST['galeria']) || isset($_SESSION['galeria']))
    {
        if(isset($_POST['galeria']))
        {
            $galeria = $_POST['galeria'];
            $_SESSION['galeria'] = $galeria;
        }
        else
        {
            $galeria = $_SESSION['galeria'];
        }
    
        if(isset($_POST['katalog']))
        {
            $katalog = $_POST['katalog'];            
        }        
        require_once "connect.php";
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
                sprintf("SELECT MAX(id) AS count FROM zdjecia WHERE %s='%s' AND zweryfikowany=1",                
                mysqli_real_escape_string($conn,$katalog),
                mysqli_real_escape_string($conn,$galeria)
                )))
                {
                    $wiersz = mysqli_fetch_array($wynik);   
                    $ile = $wiersz['count'];
                }       
                
                $par = true;
                for($i = 0; $i <= $ile ; $i++)
                {
                    if($wynik = @$conn->query(
                        sprintf("SELECT id, zdjecie, typ, nazwa_zdjecia FROM zdjecia WHERE %s='%s' AND id='$i' AND zweryfikowany=1",
                        mysqli_real_escape_string($conn,$katalog),
                        mysqli_real_escape_string($conn,$galeria)
                    )))
                    {
                        $wiersz = mysqli_fetch_array($wynik);  
                        if(!$wiersz['id']=="")
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

                            $wyraz[0] = $wyraz[0]*0.3;
                            $wyraz[1] = $wyraz[1]*0.3;
                            
                            $zdjecie = ładuj_zdjecie($wiersz['typ'],$wiersz['zdjecie'],$wyraz[0],$wyraz[1]);           

                            $img = '<input id="zdjecie_id'.$wiersz['id'].'" name="id" value="'.$wiersz['id'].'" class="img" alt="Submit" '
                            . 'type="image" src="'.$zdjecie.'">';
                            
                            if($par)
                            {
                                echo '<tr>';                                
                            }
                            echo '<td><form action="/php/zdjecie.php" method="post"><table '
                            . 'class="kafelek-standard" style="100%" onclick="wyswietl_zdjecie('.$wiersz['id'].')"><tr><td>'
                            . $img.'</td></tr>'
                            . '<tr><td>'
                            . '<label id="zdjecie_i'.$wiersz['id'].'" class="lokalizacja_zdjecia">'.$wiersz['nazwa_zdjecia'].'</label>'
                            . '</td></tr>'
                            . '</table></form></td>';
                            if($par)
                            {                               
                                $par=false;
                            }  
                            else
                            {   echo '</tr>';  
                                $par=true;
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
    /*
    if(isset($_POST['idx']))
    {
        require_once 'connect.php';
        mysqli_report(MYSQLI_REPORT_STRICT);
        try
        {
            $conn2 = new mysqli($host,$db_user, $password,$name_db);
            if($conn2->connect_errno!=0)
            {
                throw new Exception(mysqli_connect_errno());     
            }
            else
            {
                $idx = $_POST['idx'];    
                if($wynik = @$conn2->query(
                    sprintf("SELECT nazwa_zdjecia FROM zdjecia WHERE id='%s' AND zweryfikowany=1",
                    mysqli_real_escape_string($conn2,$idx) 
                )))
                {
                    $wiersz = mysqli_fetch_array($wynik);   
                    echo ''.$wiersz['nazwa_zdjecia'];
                }                
            }
            $conn2->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }       
    }
    */