<?php
    session_start();    
    session_unset();    
    header('Location:index.php');
    if (isset($_COOKIE['Odtwarzanie'])) {
        setcookie('Odtwarzanie', null, -1, '/');//usuwa ciastko  
    }
