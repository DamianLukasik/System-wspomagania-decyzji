<?php
    session_start();
    
    require_once "php/showimage.php";    
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
    <?php include 'php/panel/panel_.php'; ?>
    <!--    Galeria zdjęć       -->
    <center>
        <h1>Twoje zdjecie zostało 
        <?php 
            if(isset($_SESSION['_powodzenie']))
            {
                echo $_SESSION['_powodzenie'];   
                unset($_SESSION['_powodzenie']);
            }
        ?></h1>
    <table>
        <tr>
            <td>
                <input id="_obrazek" type='image' style='width: 500px;' src='' class="ukryty" >
                <?php
                    echo '<input id="obrazek_" type="text" value="'.$_SESSION['fr_idek'].'" class="ukryty">';  
                    unset($_SESSION['fr_idek']);
                ?>
                <script>
                    
                    setTimeout(function(){ załaduj_obraz(); }, 400); 
                    
                    function załaduj_obraz(){
                        var nazwa = $("#obrazek_").val();
                        $.ajax({
                           data: 'naz=' + nazwa,
                           url: 'php/wyswietl_miniature.php',
                           method: 'POST', // or GET
                           success: function(msg) {
                                $("#_obrazek").attr("class","");  
                                $("#_obrazek").attr("src",msg);                      
                           }
                       });	
                    }
                   
                </script>
            </td>
        </tr>
    </table>
    </center>
	<script type="text/javascript" src="/js/cookie.js" ></script>
	<script type="text/javascript" src="/js/wyswietl.js" ></script>
	<script type="text/javascript" src="/js/przegladanie.js" ></script>
    <?php include 'php/autorstwo.php';?>      
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>    
</body>
</html>

