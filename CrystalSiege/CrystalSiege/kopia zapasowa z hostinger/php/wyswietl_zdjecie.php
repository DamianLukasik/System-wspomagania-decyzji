<?php
    require_once 'showimage.php';
   
    if(isset($_POST['idx']))
    {	
        if(isset($_POST['zweryfikowany']))
        {    
            $where = "zweryfikowany='".$_POST['zweryfikowany']."'";
        }
		if(isset($_POST['katalog']))
		{
                    $str_1 = sprintf('%s',$_POST['katalog']);
                    $str_2 = sprintf('%s',$_POST['galeria']);                    
                    $where = $where." AND ".$str_1."='".$str_2."'";
		}
        $id=sprintf('%s',$_POST['idx']);
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
                    sprintf("SELECT id, typ, zdjecie, nazwa_zdjecia, opis_zdjecia, urzadzenie, "
                            ."(SELECT login FROM uzytkownicy WHERE id=T1.id_autora) AS autor, "
                            ."(SELECT blokada FROM uzytkownicy WHERE login=T1.autor) AS blokada, data_wykonania, "
                            ."lokalizacja FROM zdjecia AS T1 WHERE T1.id='%s'",
                    mysqli_real_escape_string($conn,$id)
                    )))
                    {                                              
                        $wiersz = mysqli_fetch_array($wynik);                          
                        if($wiersz['zdjecie']!=NULL)
                        {
                            if(isset($_POST['zweryfikowany']))
                            {
                                $stala=1.0;
                            }
                            else
                            {
                                $stala=0.7;
                            }
                    
                            $zdjecie = skaluj_obraz($stala,$wiersz['zdjecie'],$wiersz['typ']);                
                        }
                              
                        if($wynik1 = @$conn->query(sprintf("SELECT id FROM zdjecia WHERE ".$where." "
                        . "AND id<'".$id."' ORDER BY id DESC LIMIT 1")))
                        {
                            $liczba1 = mysqli_fetch_array($wynik1); 
                            if($liczba1['id']==NULL)
                            {
                                if($wynik1 = @$conn->query(
                                sprintf("SELECT id FROM zdjecia WHERE ".$where." "
                                . "AND id>'".$id."' ORDER BY id DESC LIMIT 1",
                                mysqli_real_escape_string($conn,$rubryka),
                                mysqli_real_escape_string($conn,$war),
                                mysqli_real_escape_string($conn,$id)
                                )))
                                {
                                    $liczba1 = mysqli_fetch_array($wynik1); 
                                    $poprzednik = $liczba1['id'];
                                }
                            }
                            $poprzednik = $liczba1['id'];                                     
                        }
                        
                        if($wynik2 = @$conn->query(
                        sprintf("SELECT id FROM zdjecia WHERE ".$where." "
                        . "AND id>'".$id."' ORDER BY id ASC LIMIT 1",
                        mysqli_real_escape_string($conn,$rubryka),
                        mysqli_real_escape_string($conn,$war),
                        mysqli_real_escape_string($conn,$id)
                        )))
                        {
                            $liczba2 = mysqli_fetch_array($wynik2); 
                            if($liczba2['id']==NULL)
                            {
                                if($wynik2 = @$conn->query(
                                sprintf("SELECT id FROM zdjecia WHERE ".$where." "
                                . "AND id<'".$id."' ORDER BY id ASC LIMIT 1",
                                mysqli_real_escape_string($conn,$rubryka),
                                mysqli_real_escape_string($conn,$war),
                                mysqli_real_escape_string($conn,$id)
                                )))
                                {
                                    $liczba2 = mysqli_fetch_array($wynik2); 
                                    $nastepnik = $liczba2['id'];
                                }                                            
                            }
                            $nastepnik = $liczba2['id'];
                        }
						if($_POST['user']==$wiersz['autor'])
                        {
                            $autor_zdjecia=TRUE;                                 
                        }
                        else
                        {
                            $autor_zdjecia=FALSE;
                        }
                        if($wiersz['blokada']=="1")
                        {
                            $blokada=TRUE;                                 
                        }
                        else
                        {
                            $blokada=FALSE;
                        }  
						
                        $array = array($zdjecie,$wiersz['nazwa_zdjecia'],$wiersz['opis_zdjecia'],
                        $wiersz['urzadzenie'],$wiersz['autor'],$wiersz['data_wykonania'],
                        $wiersz['lokalizacja'],$nastepnik,$poprzednik,$wiersz['id'],$blokada,$autor_zdjecia);
                        echo json_encode($array);                        
                    }
            }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
    
    if(isset($_POST['id']) | isset($_SESSION['id']))
    {
        if(isset($_POST['id']))
        {
            $id=sprintf('%s',$_POST['id']);
            $_SESSION['id'] = $id;
        }
        else
        {
            if(isset($_SESSION['Odtwarzanie']))
            {    
                $id = $_SESSION['id'];     
                $warunek=TRUE;
            }
            else
            {
                $warunek=FALSE;
                $id = $_SESSION['id'];                       
            }
        }
            
        if(isset($_POST['galeria']))
        {
            $war = $_POST['galeria'];
        }
        if(isset($_POST['katalog']))
        {
            $rubryka = $_POST['katalog'];            
        }
        if(isset($_POST['popd']))
        {
            $id=sprintf('%s',$_POST['popd']);
            $_SESSION['id'] = $id;
        }
        if(isset($_POST['nast']))
        {
            $id=sprintf('%s',$_POST['nast']);
            $_SESSION['id'] = $id;
        }   
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
                    sprintf("SELECT id, zdjecie, typ, nazwa_zdjecia, opis_zdjecia, autor, "
                    . "data_wykonania, urzadzenie, lokalizacja FROM zdjecia WHERE %s='%s' AND id='%s' "
                    . "AND zweryfikowany=1 ORDER BY id ASC LIMIT 1",
                    mysqli_real_escape_string($conn,$rubryka),
                    mysqli_real_escape_string($conn,$war),
                    mysqli_real_escape_string($conn,$id)
                    )))
                    {
                        $wiersz = mysqli_fetch_array($wynik);  
                        if($wiersz['id']!=NULL)
                        {
                            if(!$id=="")
                            {
                                if($ilosc = @$conn->query(
                                    sprintf("SELECT COUNT(id) AS count FROM zdjecia WHERE %s='%s' AND zweryfikowany=1",
                                    mysqli_real_escape_string($conn,$rubryka),
                                    mysqli_real_escape_string($conn,$war)
                                )))
                                {
                                    $liczba = mysqli_fetch_array($ilosc);   
                                    $liczba_zdjec = $liczba['count'];                               
                                    if($ilosc = @$conn->query(
                                    sprintf("SELECT COUNT(id) AS numer FROM zdjecia WHERE %s='%s' AND id<='%s' AND zweryfikowany=1",
                                    mysqli_real_escape_string($conn,$rubryka),
                                    mysqli_real_escape_string($conn,$war),
                                    mysqli_real_escape_string($conn,$wiersz['id'])
                                    )))
                                    {
                                        $liczba = mysqli_fetch_array($ilosc); 
                                        $numer_zdjecia = $liczba['numer'];
                                    }
                                    if($liczba_zdjec>1)        
                                    {                                        
                                        if($wynik1 = @$conn->query(
                                        sprintf("SELECT id FROM zdjecia WHERE %s='%s' AND id<'%s' "
                                        . "AND zweryfikowany=1 ORDER BY id DESC LIMIT 1",
                                        mysqli_real_escape_string($conn,$rubryka),
                                        mysqli_real_escape_string($conn,$war),
                                        mysqli_real_escape_string($conn,$id)
                                        )))
                                        {
                                            $liczba1 = mysqli_fetch_array($wynik1); 
                                            if($liczba1['id']==NULL)
                                            {
                                                if($wynik1 = @$conn->query(
                                                sprintf("SELECT id FROM zdjecia WHERE %s='%s' AND id>'%s' "
                                                . "AND zweryfikowany=1 ORDER BY id DESC LIMIT 1",
                                                mysqli_real_escape_string($conn,$rubryka),
                                                mysqli_real_escape_string($conn,$war),
                                                mysqli_real_escape_string($conn,$id)
                                                )))
                                                {
                                                    $liczba1 = mysqli_fetch_array($wynik1); 
                                                    $poprzednik = $liczba1['id'];
                                                }
                                            }
                                            $poprzednik = $liczba1['id'];
                                        }
                                        if($wynik2 = @$conn->query(
                                        sprintf("SELECT id FROM zdjecia WHERE %s='%s' AND id>'%s' "
                                        . "AND zweryfikowany=1 ORDER BY id ASC LIMIT 1",
                                        mysqli_real_escape_string($conn,$rubryka),
                                        mysqli_real_escape_string($conn,$war),
                                        mysqli_real_escape_string($conn,$id)
                                        )))
                                        {
                                            $liczba2 = mysqli_fetch_array($wynik2); 
                                            if($liczba2['id']==NULL)
                                            {
                                                if($wynik2 = @$conn->query(
                                                sprintf("SELECT id FROM zdjecia WHERE %s='%s' AND id<'%s' "
                                                . "AND zweryfikowany=1 ORDER BY id ASC LIMIT 1",
                                                mysqli_real_escape_string($conn,$rubryka),
                                                mysqli_real_escape_string($conn,$war),
                                                mysqli_real_escape_string($conn,$id)
                                                )))
                                                {
                                                    $liczba2 = mysqli_fetch_array($wynik2); 
                                                    $nastepnik = $liczba2['id'];
                                                }                                            
                                            }
                                            $nastepnik = $liczba2['id'];
                                        }
                                        if($wynik3 = @$conn->query(
                                        sprintf("SELECT id FROM zdjecia WHERE %s='%s' AND id>'%s' "
                                        . "AND zweryfikowany=1 ORDER BY id ASC LIMIT 1",
                                        mysqli_real_escape_string($conn,$rubryka),
                                        mysqli_real_escape_string($conn,$war),
                                        mysqli_real_escape_string($conn,$nastepnik)
                                        )))
                                        {
                                            $liczba3 = mysqli_fetch_array($wynik3); 
                                            if($liczba3['id']==NULL)
                                            {
                                                if($wynik3 = @$conn->query(
                                                sprintf("SELECT id FROM zdjecia WHERE %s='%s' AND id<'%s' "
                                                . "AND zweryfikowany=1 ORDER BY id ASC LIMIT 1",
                                                mysqli_real_escape_string($conn,$rubryka),
                                                mysqli_real_escape_string($conn,$war),
                                                mysqli_real_escape_string($conn,$nastepnik)
                                                )))
                                                {
                                                    $liczba3 = mysqli_fetch_array($wynik3); 
                                                    $next = $liczba3['id'];
                                                }
                                            }
                                            $next = $liczba3['id'];
                                        }
                                        if($_SESSION['wersja']=="2")
										{
											$guzik = '_2';
										}
										else
										{
											$guzik = '';
										}
											
                                        if(isset($warunek)==TRUE)
                                        { 
                                            $_SESSION['id']=$wiersz['id'];
                                            $odtwarzanie =
                                            '<div    id="odtw_p" onclick="zatzymanie()">'
                                            .'<input id="odtw" class="ukryty" value="0" >'
                                            .'<input id="odtw_g" style="width: 5%;" type="image" src="/grafika/pause'.$guzik.'.gif"></div>';
                                        }
                                        else
                                        {																						
                                            $odtwarzanie =
                                            '<div    id="odtw_p" onclick="odtwarzanie()">'
                                            .'<input id="odtw" class="ukryty" value="1" >'
                                            .'<input id="odtw_g" style="width: 5%;" type="image" src="/grafika/play'.$guzik.'.gif"></div>'; 
                                            
                                            $poprzedni = '<div onclick="poprzedni()">'
                                            .'<input id="popd_0" class="ukryty" name="popd" value="'.$poprzednik.'" >'
                                            .'<input id="popd_" style="width: 90%;" type="image" src="/grafika/strzalka_w_lewo'.$guzik.'.gif"></div>';    

                                            $nastepny = '<div onclick="nastepny()">'
                                            .'<input id="nast_0" class="ukryty" name="nast" value="'.$nastepnik.'" >'
                                            .'<input id="nast_" style="width: 90%;" type="image" src="/grafika/strzalka_w_prawo'.$guzik.'.gif"></div>';                                             
                                        }
                                    }
                                }
                                
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
                                
                                $wyraz[0] = $wyraz[0]*0.7;
                                $wyraz[1] = $wyraz[1]*0.7;
                                                                
                                $zdjecie = ładuj_zdjecie($wiersz['typ'],$wiersz['zdjecie'],$wyraz[0],$wyraz[1]);   
                                $img = '<input id="obraz_" class="img" style="width: 100%" type="image" src="'.$zdjecie.'">';
                                //imagedestroy($image); 
                                if(!isset($poprzedni)){$poprzedni='';}
								if(!isset($odtwarzanie)){$odtwarzanie='';}
								if(!isset($nastepny)){$nastepny='';}
                                //zamienić form na javascriptowe
                                echo '<tr><td style="width: 5%;">'.$poprzedni.'</td><td><table class="kafelek-powiekszony">'
                                .'<tr><td colspan="3"><center>'.$odtwarzanie.'<label id="numeracja_" class="lokalizacja_zdjecia">'
                                .$numer_zdjecia.'\\'.$liczba_zdjec.'</label></center>'.'</td></tr><tr><td  colspan="3">'.$img.'</td></tr>'
                                .'<tr><td colspan="3"><label id="tytul_zdjecia_" class="tytul_zdjecia">'.$wiersz['nazwa_zdjecia'].'</label></td></tr>'
                                .'<tr><td style="width: 10%"><label id="autor_" class="autor_zdjecia">'.$wiersz['autor'].'</label></td>'
                                .'<td style="width: 20%"><label id="data_wykonania_" class="data_zdjecia">'.$wiersz['data_wykonania'].'</label></td>'
                                .'<td style="width: 70%"><label id="lokalizacja_" class="lokalizacja_zdjecia">'.$wiersz['lokalizacja'].'</label></td></tr>'
                                .'<tr><td colspan="3"><label id="opis_zdjecia_" class="opis_zdjecia">Opis:</br>'.$wiersz['opis_zdjecia'].'</label></td></tr>'
                                .'<tr><td colspan="3"><label id="urzadzenie_" class="urzadzenie_zdjecia">Urządzenie : '
                                .$wiersz['urzadzenie'].'</label></td></tr></table></form></td><td style="width: 5%;">'.$nastepny.'</td></tr>';                           
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
    