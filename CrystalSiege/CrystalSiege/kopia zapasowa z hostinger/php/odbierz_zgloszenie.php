<?php
    session_start();
    if(!isset($_SESSION['zalogowany']) || $_SESSION['prawa']==1)
    {
        header('Location:/index.php');
        exit();
    }
    require_once "showimage.php";
    unset($_SESSION['Odtwarzanie']);
?>
<!DOCTYPE html>
<html>
<head>
	<title>Archiwum Zdjęć Częstochowy</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<script src="/scripts/jquery-2.2.0.min.js"></script>
	<script src="/scripts/jquery-ui-1.11.4.min.js"></script>
	<script src="/scripts/angular.min.js"></script>
</head>
<body>
    <!-- Pasek nawigacji   -->
    <?php include 'panel/panel_.php'; ?>    
    </br></br>   
    <center>
    <?php    
    
    //Niestety, funkcjonalności z obsługą IMAP nie są dostępne na prv.pl.
     //   set_time_limit(10000); 
        //$hostname = '{pop.gmail.com:995/pop3/ssl}';
	$hostname = '{imap.gmail.com:993/imap/ssl}INBOX';		
        $username = 'archiwumzdjecczestochowy@gmail.com';
        $password = '5B\h1sP5';
        		
        if($mbox = imap_open($hostname, $username, $password))
		{
            echo '<a>Połączenie nawiązane</a></br>';
            $emails = imap_search($mbox,'ALL');
			
			if($emails) 
			{
				$output = '';
				rsort($emails);
				$id_=0;
				foreach($emails as $email_number) {		
					$overview = imap_fetch_overview($mbox,$email_number,0);
					$message = imap_fetchbody($mbox,$email_number,1);
					if($overview[0]->seen)
					{
						$output.= '<div class="wiadomosc">';
						$output.= '<input id="zgloszenie'.$id_.'" type="button" class="przycisk-ikona" ><h2>'.$overview[0]->subject.'</h2>';
						$output.= '<h4>'.$overview[0]->from.' - '.$overview[0]->date.'</h4>';
						$output.= '<p id="tresc_wiadomości">'.$message.'</p></div>';
                                                $id_=$id_+1;
					}
				}		
				echo $output;
                                #echo '<script>alert();skonstruuj_kontener("X");</script>';
			}
			else
			{
				echo '<a>Skrzynka jest pusta</a>';
			}
            imap_close($mbox);
		}
        else
		{
            echo "błąd</br>";
            print_r(imap_errors());
		}
		echo '</br>';
    ?>
    </center>
    </br></br>
    <?php include 'autorstwo.php';?>
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>
</body>
</html>