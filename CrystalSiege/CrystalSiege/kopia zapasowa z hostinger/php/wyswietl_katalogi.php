<?php
    if(isset($_POST['katalog']) || isset($_SESSION['katalog']))
    {
        if(isset($_POST['katalog']))
        {
            $war = $_POST['katalog'];
            $_SESSION['katalog'] = $war;
        }
        else
        {
            $war = $_SESSION['katalog'];
        }
                    
        require_once "connect.php";
        $conn = new mysqli($host,$db_user, $password,$name_db);
                    
        function liczba_zdjec($wyb,$rubryka,$conn)
        {
            try
            {
                if($wynik = @$conn->query(
                    sprintf("SELECT COUNT(id) AS count FROM zdjecia WHERE %s='%s' AND zweryfikowany=1",
                    mysqli_real_escape_string($conn,$rubryka),
                    mysqli_real_escape_string($conn,$wyb)
                )))
                {
                    $wiersz = mysqli_fetch_array($wynik);  
                    return $wiersz['count'];                               
                }
            }
            catch(Exception $e)
            {
                echo 'błąd'.$e;
            }
        }
                    
        function wypisz($tablica,$rubryka,$conn)
        {
            $ile = count($tablica);
            for($i=0; $i < $ile ;$i++)
            {                                           
                echo liczba_zdjec($tablica[$i],$rubryka,$conn).',';
            }
        }                  
        
        $tablica1 = array('-nieznane okolice-','Błeszno',
                'Częstochówka-Parkitka','Dźbów','Gnaszyn-Kawodrza','Grabówka', 'Kiedrzyn',
                'Lisiniec', 'Mirów', 'Ostatni Grosz','Podjasnogórska','Północ','Raków',
                'Stare Miasto','Stradom','Śródmieście','Trzech Wieszczów','Tysiąclecie',
                'Wrzosowiak','Wyczerpy-Aniołów','Zawodzie-Dąbie');
                    
        $tablica2 = array('Okres uprzemysłowienia' ,'I wojna światowa' ,'Okres międzywojenny' ,
                'II wojna światowa' ,'Okres stabilizacji' ,'Czasy PRL' , 'Przemianny ustrojowe' , 
                'Współczesność');
                    
        $tablica3 = array('-nieznane urządzenie-','Aparat analogowy',
                'Aparat fotograficzny','Aparat cyfrowy','Komórka z aparatem',
                'Smartfon','iPad','Tablet');
            
        switch($war)
        {
            case "lokalizacja":
                wypisz($tablica1,$war,$conn);
                break;
            case "okres":
                wypisz($tablica2,$war,$conn);
                break;
            case "urzadzenie":
                wypisz($tablica3,$war,$conn);
                break;
            default:
               break;                        
        }
        $conn->close();                    
    }