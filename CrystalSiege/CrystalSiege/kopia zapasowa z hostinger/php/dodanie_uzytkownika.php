<?php
    require_once "connect.php"; 
    
    foreach (glob("*.jpg") as $filename) {
        unlink($filename);
    }
    
    $conn = @new mysqli($host,$db_user, $password,$name_db);
     if($conn->connect_errno!=0)
    {
        echo "Error:".$conn->connect_errno;       
    }else{ 
        $nick = $_POST['nick'];
        $pass = $_POST['pass'];
        $mail = $_POST['mail'];    
        $sql = "INSERT INTO uzytkownicy (id, login, haslo, email) VALUES (NULL, '$nick', '$pass', '$mail')";  
        if (@$conn->query($sql) === TRUE) {
            echo "New record created successfully";
        } else {
            echo "Error: " . $sql . "<br>" . $conn->error;
        }   
        $conn->close(); 
    }
