<?php
    require_once 'showimage.php';
    
    try
    {
        require_once 'connect.php';
        $conn = new mysqli($host,$db_user, $password,$name_db);
        if($conn->connect_errno!=0)
        {
            throw new Exception(mysqli_connect_errno());     
        }
        else
        {
            if($wynik = @$conn->query("SELECT id, zdjecie, id_autora, typ, nazwa_zdjecia, opis_zdjecia,"
            . " autor, data_wykonania, urzadzenie, lokalizacja FROM zdjecia WHERE zweryfikowany=0"))
            {
                if(!$wynik) throw new Exception($conn->error);
                $liczba_zdjec = $wynik->num_rows;
                if($liczba_zdjec>0)
                {     
                    $numer = 1;    /*  */
                    if(isset($_GET['idek']))
                    {
                        $idek = $_GET['idek'];
                        if($wynik = @$conn->query(sprintf("SELECT id, zdjecie, id_autora, typ, nazwa_zdjecia, opis_zdjecia, autor, "
                        . "data_wykonania, urzadzenie, lokalizacja FROM zdjecia WHERE zweryfikowany=0 AND id='%s'",
                        mysqli_real_escape_string($conn,$idek))))
                        {    
                            if($liczby = @$conn->query("SELECT COUNT(id) AS liczby "
                            . "FROM zdjecia WHERE zweryfikowany=0 AND id<'%s'",
                            mysqli_real_escape_string($conn,$idek)))
                            {
                                
                                if($liczby->num_rows>=0)
                                {
                                    $liczby = mysqli_fetch_array($liczby);  
                                    $numer = $liczby['liczby'] + 1;
                                }                     
                            }                     
                        }  
                    }     
                    $wiersz = mysqli_fetch_array($wynik);  
                    $stala=1.0;
                    $zdjecie = skaluj_obraz($stala,$wiersz['zdjecie'],$wiersz['typ']);  
                    $img = '<input id="obraz_" class="img" type="image" src="'.$zdjecie.'">';                    
                    $id=$wiersz['id'];
                    
                    if($wynik1 = @$conn->query(
                    sprintf("SELECT id FROM zdjecia WHERE zweryfikowany=0 AND id<'%s' "
                    . " ORDER BY id DESC LIMIT 1",
                    mysqli_real_escape_string($conn,$id)
                    )))
                    {
                        $liczba1 = mysqli_fetch_array($wynik1); 
                        if($liczba1['id']==NULL)
                        {
                            if($wynik1 = @$conn->query(
                            sprintf("SELECT id FROM zdjecia WHERE zweryfikowany=0 AND id>'%s' "
                            . " ORDER BY id DESC LIMIT 1",
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
                    sprintf("SELECT id FROM zdjecia WHERE zweryfikowany=0 AND id>'%s' "
                    . " ORDER BY id ASC LIMIT 1",
                    mysqli_real_escape_string($conn,$id)
                    )))
                    {
                        $liczba2 = mysqli_fetch_array($wynik2); 
                        if($liczba2['id']==NULL)
                        {
                            if($wynik2 = @$conn->query(
                            sprintf("SELECT id FROM zdjecia WHERE zweryfikowany=0 AND id<'%s' "
                            . " ORDER BY id ASC LIMIT 1",
                            mysqli_real_escape_string($conn,$id)
                            )))
                            {
                                $liczba2 = mysqli_fetch_array($wynik2); 
                                $nastepnik = $liczba2['id'];
                            }                                            
                        }
                        $nastepnik = $liczba2['id'];
                    }
                    
                    $poprzedni = '<div onclick="poprzedni()">'
                    .'<input id="popd_0" class="ukryty" name="popd" value="'.$poprzednik.'" >'
                    .'<input id="popd_" style="width: 95%;" type="image" src="/grafika/strzalka_w_lewo.gif"></div>';    

                    $nastepny = '<div onclick="nastepny()">'
                    .'<input id="nast_0" class="ukryty" name="nast" value="'.$nastepnik.'" >'
                    .'<input id="nast_" style="width: 95%;" type="image" src="/grafika/strzalka_w_prawo.gif"></div>';
                    
                    
                    echo '<td style="width: 5%;">'.$poprzedni.'</td>';
                    
                    echo '<td colspan="4"><table class="kafelek-standard"><tr><td colspan="3">'
                    .'<center><label id="numeracja_" class="lokalizacja_zdjecia">'.$numer.'\\'.$liczba_zdjec.'</label>'
                    . '</center></td></tr><tr><td colspan="3">'.$img.'</td></tr>'
                    . '<tr><td colspan="3"><label id="tytul_zdjecia_" class="tytul_zdjecia">'.$wiersz['nazwa_zdjecia'].'</label></td></tr>'
                    . '<tr><td style="width: 10%"><label id="autor_" class="autor_zdjecia">'.$wiersz['autor'].'</label></td>'
                    . '<td style="width: 20%"><label id="data_wykonania_" class="data_zdjecia">'.$wiersz['data_wykonania'].'</label></td>'
                    . '<td style="width: 70%"><label id="lokalizacja_" class="lokalizacja_zdjecia">'.$wiersz['lokalizacja'].'</label></td></tr>'
                    . '<tr><td colspan="3"><label id="opis_zdjecia_" class="opis_zdjecia">Opis:</br>'.$wiersz['opis_zdjecia'].'</label></td></tr>'
                    . '<tr><td colspan="3"><label id="urzadzenie_" class="urzadzenie_zdjecia">Urządzenie : '.$wiersz['urzadzenie'].'</label></td></tr>'
                    . '</table></td>';  
                    
                    echo '<td style="width: 5%;">'.$nastepny.'</td>';
                    
                    $autor = $wiersz['autor'];  
                    $_SESSION['fr_autor'] = $autor;
                    $_SESSION['fr_id'] = $wiersz['id'];    
                                        
                    $wynik = $conn->query("SELECT blokada FROM uzytkownicy WHERE login='$autor'");
                    if(!$wynik) throw new Exception($conn->error);
                    $liczba_uzytkownikow = $wynik->num_rows;
                    if($liczba_uzytkownikow>0)
                    {
                        $wiersz = mysqli_fetch_array($wynik);  
                        $_SESSION['fr_blokada'] = $wiersz['blokada'];
                    }
                }
                else
                {
                    $_SESSION['brak']="0";
                }
            }
        }
        $conn->close();        
    }
    catch(Exception $e)
    {
        echo 'błąd </br> '.$e;
    }
     