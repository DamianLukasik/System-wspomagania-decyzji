<?php

$nick = $_POST['nick'];
$pass = $_POST['pass'];
$mail = $_POST['mail'];

$to      =  'dayandey@gmail.com';
$subject =  'the subject';
$message =  'hello';
$headers =  'From: "user" <dayandey@gmail.com' . "\r\n" .
            'Reply-To: dayandey@gmail.com' . "\r\n" .
            'X-Mailer: PHP/' . phpversion();
        
//wysyłanie maila nie działa
if (mail($to, $subject, $message, $headers,"-f dayandey@gmail.com")) 
{
    echo "DZIAŁA";
} 
else 
{
    echo "Nie";
}