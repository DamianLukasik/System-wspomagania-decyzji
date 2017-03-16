<?php
    session_start();
    if(!isset($_SESSION['zalogowany']))
    {
        header('Location:index.php');
        exit();
    }      
    require_once "connect.php"; 
    
    try
    {
        $conn = new mysqli($host,$db_user, $password,$name_db);
        if($conn->connect_errno!=0)
        {
            throw new Exception(mysqli_connect_errno());          
        }
        else
        {
            if(isset($_FILES['plik']))
            {
                if($_FILES['plik']['error']==UPLOAD_ERR_OK)
                {
                    $filename=$_FILES['plik']['name'];
                    $filetype=$_FILES['plik']['type'];
                    $filesize=$_FILES['plik']['size'];
                    $filesrc= $_FILES['plik']['tmp_name'];
					
                    $tmp = explode('.', $filename);
                    $sprawdzenia = substr($filename, strrpos($filename, ".")); 
                    $p_roz= array_pop($tmp);
                    
                    $p_nazwa_zm=(md5($filename)).".".$p_roz;
                    $folder='img/';
                    
                    if (file_exists($folder.$p_nazwa_zm))
                    {
                        echo "Wracaj ...";
                        exit();
                    }
                    else
                    {
                        $poprawnosc = true;
                        if($_POST['nazwa']=="")
                        {
                            $poprawnosc = false;
                            $_SESSION['error_nazwa'] = "Brakuje tytułu zdjęcia";
                        }
                        else
                        {
                            $nazw = $_POST['nazwa'];
                            $_SESSION['fr_nazwa'] = $nazw;
                        }
                        
                        if($_POST['opis']=="")
                        {
                            $poprawnosc = false;
                            $_SESSION['error_opis'] = "Brakuje opisu zdjęcia";
                        }
                        else
                        {
                            $opis = $_POST['opis'];    
                            $_SESSION['fr_opis'] = $opis;                            
                        }
                        
                        if($_POST['data']=="")
                        {
                            $poprawnosc = false;
                            $_SESSION['error_data'] = "Brakuje daty wykonania zdjęcia";
                        }
                        else
                        {
                            $date = $_POST['data'];
                            $_SESSION['fr_data'] = $date;
                        }
                        
                        if($_POST['lokalizacja']=="-brak-")
                        {
                            $poprawnosc = false;
                            $_SESSION['error_lokalizacja'] = "Brakuje lokalizacji wykonania zdjęcia";
                        }
                        else
                        {
                            $loka = $_POST['lokalizacja'];
                            $_SESSION['fr_lokalizacja'] = $loka;
                        }         
                        
                        if($_POST['urzadzenie']=="-brak-")
                        {
                            $poprawnosc = false;
                            $_SESSION['error_urzadzenie'] = "Brakuje informacji jakim urządzeniem wykonano zdjęcie";
                        }
                        else
                        {
                            $urza = $_POST['urzadzenie'];
                            $_SESSION['fr_urzadzenie'] = $urza;
                        }   
                        
                        if($_POST['tag']=="")
                        {
                            $poprawnosc = false;
                            $_SESSION['error_tagi'] = "Brakuje tagów";
                        }
                        else
                        {
                            $tagi = $_POST['tag'];
                            $_SESSION['fr_tagi'] = $tagi;
                            if(preg_match('/[^a-zA-Z ]+/', $tagi))
                            {
                                $poprawnosc = false;
                                $_SESSION['error_tagi'] = "Tagi powinny składać się z liter";
                            }
                        }
                        
                        if(isset($_SESSION['user']))
                        {
                            if($wynik__ = @$conn->query(
                            sprintf("SELECT id FROM uzytkownicy WHERE login='%s'",
                            mysqli_real_escape_string($conn,$_SESSION['user'])
                            )))
                            {
                                $wiersz__ = mysqli_fetch_array($wynik__);                          
                                $autor = $wiersz__['id'];
                            }
                        }
                        else
                        {
                            $poprawnosc = false;
                            $_SESSION['error_autor'] = "Brakuje nazwy autora zdjęcia";
                        }
                        
                        if($poprawnosc)
                        {
                            //dodatkowe dane do tabeli
                            $data = new DateTime($date);
                            //YYYY-MM-DD
                            if($data>= new DateTime('1890-01-01')&& $data<= new DateTime('1914-07-28'))
                            {
                                $okres = "Okres uprzemysłowienia";
                            }
                            if($data>= new DateTime('1914-07-28')&& $data<= new DateTime('1918-11-11'))
                            {                        
                                $okres = "I wojna światowa";
                            }
                            if($data>= new DateTime('1918-11-11')&& $data<= new DateTime('1939-09-01'))
                            {                        
                                $okres = "Okres międzywojenny";
                            }
                            if($data>= new DateTime('1939-09-01')&& $data<= new DateTime('1945-01-16'))
                            {                        
                                $okres = "II wojna światowa";
                            }
                            if($data>= new DateTime('1945-01-16')&& $data<= new DateTime('1952-07-22'))
                            {                        
                                $okres = "Okres stabilizacji";
                            }
                            if($data>= new DateTime('1952-07-22')&& $data<= new DateTime('1989-06-04'))
                            {                        
                                $okres = "Czasy PRL";
                            }
                            if($data>= new DateTime('1989-06-04')&& $data<= new DateTime('1999-01-01'))
                            {                        
                                $okres = "Przemianny ustrojowe";
                            }
                            if($data>= new DateTime('1999-01-01'))
                            {                        
                                $okres = "Współczesność";
                            }   

                            if($filetype=="image/png" || $filetype=="image/x-png" || $filetype=="image/gif" || $filetype=="image/jpeg" || $filetype=="image/pjpeg")
                            {
                                $plik=fopen($filesrc, "r");
                                //ładowanie zdjęć na serwer
                                $target_path = "zdjecia/";
                                $target_path = $target_path.rand();
                                $target_path = $target_path.basename($_FILES['plik']['name']);
                                move_uploaded_file($filesrc,$target_path);
                                //$dolacz = 'http://archiwumzdjecczestochowy.xyz/php/';
                                //miniatury
                                $target_path_min = "zdjecia/".rand().'miniatura'.basename($_FILES['plik']['name']);
                                ob_start();
                                if ($filetype=="image/gif") {
                                    $image = imagecreatefromgif($target_path);
                                    $image = imagescale($image, 440, 330);
                                    imagegif($image,$target_path_min);
                                }
                                if ($filetype=="image/jpeg") {
                                    $image = imagecreatefromjpeg($target_path);
                                    $image = imagescale($image, 440, 330);
                                    imagejpeg($image,$target_path_min);
                                }
                                if ($filetype=="image/png") {
                                    $image = imagecreatefrompng($target_path);
                                    $image = imagescale($image, 440, 330);
                                    imagepng($image,$target_path_min);
                                }
                                ob_end_clean();          
                                imagedestroy($image); 
                                //
                                //$target_path = $dolacz.$target_path;
                               // $target_path_min = $dolacz.$target_path_min;
                               // $imageProperties = getimageSize($filesrc);
                                fclose($plik);
                                //unlink($filesrc);
                               // $mysqlfiletype = $imageProperties['mime'];
                                $mysqlfilename = addslashes($filename);

                                $path_file=$folder.$p_nazwa_zm;
                                $_SESSION['fr_idek']=$mysqlfilename;

                                if($conn->query("INSERT INTO zdjecia (id, zdjecie, typ, nazwa_pliku, rozmiar, nazwa_zdjecia, "
                                . "opis_zdjecia, data_wykonania, lokalizacja, urzadzenie, tag, okres, id_autora, miniatura)"
                                . " VALUES (NULL,'".$target_path."','".$filetype."','".$mysqlfilename."','".$filesize."','".$nazw."','"
                                .$opis."','".$date."','".$loka."','".$urza."','".$tagi."','".$okres."','"
                                .$autor."','".$target_path_min."')"))
                                {
                                  //  $_SESSION['dodano_obraz']=$path_file;
                                    header('Location: /powodzenie.php');
                                }
                                else
                                {                                    
                                    throw new Exception($conn->error);
                                }
                            }
                        }
                    }
                }
                else
                {
                    $_SESSION['error_plik'] = "Nie załączyłeś pliku";                
                }
            }
            
            $conn->close();  
            if($poprawnosc)
            {
                unset($_SESSION['fr_nazwa']);
                unset($_SESSION['fr_opis']);
                unset($_SESSION['fr_data']);
                unset($_SESSION['fr_lokalizacja']);
                unset($_SESSION['fr_urzadzenie']);
                unset($_SESSION['fr_tagi']);                   
                $_SESSION['_powodzenie'] = "pomyślnie dodane do bazy";
                header('Location:/powodzenie.php');
            }
            else
            {
                header('Location:przeslij.php');
            }
        }
    }
    catch(Exception $e)
    {
        echo 'błąd </br> '.$e;
    }
