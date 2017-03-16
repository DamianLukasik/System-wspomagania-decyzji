<?php

    require_once 'showimage.php';

    if(isset($_POST['id']))
    {   
        $id=$_POST['id'];
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
                    . "data_wykonania, urzadzenie, lokalizacja FROM zdjecia WHERE id='%s' "
                    . "AND zweryfikowany=1",
                    mysqli_real_escape_string($conn,$id)
                    )))
                    {
                        $wiersz = mysqli_fetch_array($wynik);  
                        if($wiersz['id']!=NULL)
                        {                      
                            if($wiersz['zdjecie']!=NULL)
                            {
                                $stala = 0.8;                                    
                                $zdjecie = skaluj_obraz($stala,$wiersz['zdjecie'],$wiersz['typ']); 
                                $img = '<input id="obraz_" type="image" src="'.$zdjecie.'">';                              
                                echo '<table class="kafelek-powiekszony"  >'
                                .'<tr><td colspan="3">'.$img.'</td></tr>'
                                .'<tr><td colspan="3"><label class="tytul_zdjecia">'.$wiersz['nazwa_zdjecia'].'</label></td></tr>'
                                .'<tr><td style="width: 10%"><label class="autor_zdjecia">'.$wiersz['autor'].'</label></td>'
                                .'<td style="width: 20%"><label class="data_zdjecia">'.$wiersz['data_wykonania'].'</label></td>'
                                .'<td style="width: 70%"><label class="lokalizacja_zdjecia">'.$wiersz['lokalizacja'].'</label></td></tr>'
                                .'<tr><td colspan="3"><label class="opis_zdjecia">Opis:</br>'.$wiersz['opis_zdjecia'].'</label></td></tr>'
                                .'<tr><td colspan="3"><label class="urzadzenie_zdjecia">Urządzenie : '
                                .$wiersz['urzadzenie'].'</label></td></tr></table>';  
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

    if(isset($_POST['zapytanie']))
    {
        if(isset($_POST['slowa']))
        {
            $slowa = $_POST['slowa'];
        }
        if(isset($_POST['data1']))
        {
            $data1 = $_POST['data1'];
        }
        if(isset($_POST['data2']))
        {
            $data2 = $_POST['data2'];
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
                    echo "<center><label id='napis-1' class='text-nagłówek1'>Otrzymane wyniki</label>";
                    $sql1 = "SELECT COUNT(id) AS count, MAX(id) AS max, MIN(id) AS min";
                    $sql2  = " FROM zdjecia WHERE";
                    $and = false;
                    if($slowa!="")
                    {
                        if($and){$sql2=$sql2." AND";}
                        $slowo = explode(" ", $slowa);
                        $len = count($slowo);
                        for($j=0; $j<$len; $j++)
                        {
                            if($j==0)
                            {
                                $sql2=$sql2." (";                             
                            }
                            $slowo_ = sprintf("%s",$slowo[$j]);
                            $sql2=$sql2." tag LIKE '%".$slowo_."%' OR nazwa_zdjecia LIKE '%".$slowo_."' OR opis_zdjecia LIKE '%".$slowo_."%' OR lokalizacja LIKE '%".$slowo_."' OR urzadzenie LIKE '%".$slowo_."' OR autor LIKE '%".$slowo_."'";
                            if($j<$len-1)
                            {
                                $sql2=$sql2." OR ";                                
                            }                        
                        }
                        $sql2=$sql2.")";
                        $and=true;
                    }
                    if($data1!="")
                    {
                        if($and){$sql2=$sql2." AND";}
                        $sql2=$sql2." data_wykonania>='".$data1."'";  
                        $and=true;
                    }
                    if($data2!="")
                    {
                        if($and){$sql2=$sql2." AND";}
                        $sql2=$sql2." data_wykonania<='".$data2."'";
                        $and=true;
                    }
                    $sql2=$sql2." AND zweryfikowany=1";
                    $sql=$sql1.$sql2;
                    if($wynik = @$conn->query($sql))
                    {
                        $wiersz = mysqli_fetch_array($wynik);   
                        $max   = $wiersz['max'];
                        $min   = $wiersz['min'];
                        $count = $wiersz['count'];
                    }
                    if(isset($_POST['id']))
                    {
                        $count=1;
                        $min=$_POST['id'];
                        $max=$_POST['id'];
                        $sql2=' FROM zdjecia WHERE zweryfikowany=1';
                    }
                    if($count==0)
                    {
                        echo '<td colspan="4"><table class="kafelek-standard"><tr><td>'
                    . '<label class="opis_zdjecia">'
                    . 'Nie znaleziono zdjęć spełniających wprowadzone kryteria'
                    . '</label></td></tr></table></td>';                       
                    }
                    $par = true;
                    echo '<div id="galeria-zdjecia" class="galeria"><table id="galeria-zdjecia">';
                    for($i = $min; $i <= $max ; $i++)//pętla
                    {                  
                        $zapytanie = "SELECT autor, urzadzenie, lokalizacja, data_wykonania, id, zdjecie, opis_zdjecia, typ, nazwa_zdjecia, tag".$sql2." AND id='".$i."'"; 
                        if($wynik = @$conn->query($zapytanie))
                        {
                            $wiersz = mysqli_fetch_array($wynik);  
                            if(!$wiersz['id']=="")
                            {       
                                if(!isset($_POST['id']))
                                {
                                    $stala = 0.4;                                    
                                    $zdjecie = skaluj_obraz($stala,$wiersz['zdjecie'],$wiersz['typ']);                                    
                                    $img = '<input id="image_'.$wiersz['id'].'" name="id" value="'.$wiersz['id'].'" class="img" alt="Submit" '
                                    . 'type="image" style="width: 100%;" src="'.$zdjecie.'">';
                                    if($par)
                                    {
                                        echo '<tr>';                                
                                    }
                                    echo '<td><div onclick="wyswietl('.$wiersz['id'].')"><table '
                                    . 'class="kafelek-standard"><tr><td>'
                                    . $img.'</td></tr>'
                                    . '<tr><td>'
                                    . '<label id="tyt_img_'.$wiersz['id'].'" class="lokalizacja_zdjecia">'.$wiersz['nazwa_zdjecia'].'</label>'
                                    . '</td></tr>'
                                    . '</table></div></td>';
                                    if($par)
                                    {                               
                                        $par=false;
                                    }  
                                    else
                                    {   echo '</tr>';  
                                        $par=true;
                                    }
                                }
                                else
                                {
                                    $stala = 0.4;                                    
                                    $zdjecie = skaluj_obraz($stala,$wiersz['zdjecie'],$wiersz['typ']); 
                                    $img = '<input class="img" type="image" style="width: 100%;" src="'.$zdjecie.'">';
                                    echo '<tr><td style="width: 5%;">'.$poprzedni.'</td><td><table class="kafelek-powiekszony">'
                                    .'<tr><td colspan="3">'.$img.'</td></tr>'
                                    .'<tr><td colspan="3"><label class="tytul_zdjecia">'.$wiersz['nazwa_zdjecia'].'</label></td></tr>'
                                    .'<tr><td style="width: 10%"><label class="autor_zdjecia">'.$wiersz['autor'].'</label></td>'
                                    .'<td style="width: 20%"><label class="data_zdjecia">'.$wiersz['data_wykonania'].'</label></td>'
                                    .'<td style="width: 70%"><label class="lokalizacja_zdjecia">'.$wiersz['lokalizacja'].'</label></td></tr>'
                                    .'<tr><td colspan="3"><label class="opis_zdjecia">Opis:</br>'.$wiersz['opis_zdjecia'].'</label></td></tr>'
                                    .'<tr><td colspan="3"><label class="urzadzenie_zdjecia">Urządzenie : '
                                    .$wiersz['urzadzenie'].'</label></td></tr></table></form></td><td style="width: 5%;">'.$nastepny.'</td></tr>';
                                }
                                
                            }
                        }
                    }
                    echo '</table></div></center>';
                }
            $conn->close();
        }
        catch(Exception $e)
        {
            echo 'błąd </br> '.$e;
        }
    }
      
