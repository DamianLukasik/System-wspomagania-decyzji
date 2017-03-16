<?php

function connection() { 
    // nawiazujemy polaczenie 
    $connection = @mysql_connect('localhost', 'admin', 'vaCarp4TmWrXBsWF')
    // w przypadku niepowodznie wyświetlamy komunikat 
    or die('Brak połączenia z serwerem MySQL.<br />Błąd: '.mysql_error()); 
    // połączenie nawiązane ;-) 
    // nawiązujemy połączenie z bazą danych 
    $db = @mysql_select_db('baza', $connection) 
    // w przypadku niepowodzenia wyświetlamy komunikat 
    or die('Nie mogę połączyć się z bazą danych<br />Błąd: '.mysql_error()); 
    // połączenie nawiązane ;-) 
    return $connection;
}

function query($query) {      
    @mysql_query($query);
}

function close_connection() {  
    // zamykamy połączenie     
    mysql_close($connection); 
}

?>

