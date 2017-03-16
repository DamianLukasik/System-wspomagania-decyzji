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
    <!--    Przeszukaj galerię  -->
    <table id="pasek_" class="formularz_przesyłania-mini">
         <tr>                  
            <td>
                <div style="float: right" >
                    <input type="text" id="slowa_" value="" class="text-standard" style="font-size: 22px; width: 500px;" />
                </div>
                <span id="nazwa_error"></span>
            </td>
            <td>
                <input id="szukaj" class="przycisk-standard" type="submit" value="Szukaj" onclick="wyszukaj()" >
                <span id="wyszukaj_error"></span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                    <table>
                        <tr>
                            <td><label class="opis_zdjecia">Od</label></td>
                        <td>
                            <input id="data1" type="date" name="data1" value="" class="data-standard-przeslij" max="2020-12-31" min="1890-01-01"/>                           
                        </td>
                            <td><label class="opis_zdjecia">Do</label></td>
                        <td>
                            <input id="data2" type="date" name="data2" value="" class="data-standard-przeslij" max="2020-12-31" min="1890-01-01"/>                           
                        </td>   
                      </tr>                
                    </table>    
            </td>                
        </tr>
        <tr>
            <td class="opis-dluzsze">
                <label type="text" name="szukaj" style="visibility: hidden;"/>
            </td>
        </tr>
    </table>
    
    <!--
        <table id="pasek_" class="formularz_przesyłania-mini">
            <tr>
                <td class="opis-dluzsze">
                    <a>Wprowadź nazwę zdjęcia:</a>
                </td>                    
                <td>
                    <div style="float: right" >
                    <input type="text" id="nazwa" value="" class="text-standard"/>
                    </div>
                    <span id="nazwa_error"></span>
                </td>
            </tr>
            <tr>
                <td class="opis-dluzsze">
                    <a>Wprowadź słowa kluczowe:</a>
                </td>                    
                <td>
                    <div style="float: right" >
                    <input type="text" id="slowa" value="" class="text-standard"/>
                    </div>
                    <span id="slowa_error"></span>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="opis-dluzsze">
                    <a>Wyznacz przedział czasowy daty wykonania zdjęcia:</a>
                </td>   
            </tr>    
            <tr>
                <td colspan="2">
            <div style="float: right" >
            <table>
                <tr>
                <td><a>Od</a></td>
                <td>
                    <input id="data1" type="date" name="data1" value="" class="data-standard-przeslij" max="2020-12-31" min="1890-01-01"/>                           
                </td>
                <td><a>Do</a></td>
                <td>
                    <input id="data2" type="date" name="data2" value="" class="data-standard-przeslij" max="2020-12-31" min="1890-01-01"/>                           
                </td>   
              </tr>                
            </table>    
            </div>
            </td>                
            </tr>
            <tr>
                <td class="opis-dluzsze">
                    <a>Wprowadź tagi:</a>
                </td>                    
                <td>
                    <div style="float: right" >
                    <input type="text" id="tag" value="" class="text-standard"/>
                    </diV>
                </td>
            </tr>
            <tr>    
                <td colspan="2"><a>
                    <?php
                        if(isset($_SESSION['error_tagi']))
                        {
                            echo '<span class="komunikat_błąd">'.$_SESSION['error_tagi'].'</span>';   
                            unset($_SESSION['error_tagi']);
                        }
                    ?></a>
                </td>
            </tr>
            <tr>
                <td class="opis-dluzsze">
                    <label type="text" name="szukaj" style="visibility: hidden;"/>
                </td>                    
                <td>
                    <div style="float: right" >
                        <input id="szukaj" class="przycisk-standard" type="submit" value="Szukaj" onclick="wyszukaj()" >
                    </div>
                    <span id="wyszukaj_error"></span>
                </td>
            </tr>
        </table>-->
    <!--  Wyniki wyszukiwań  -->
    <br>
    <div id="wyniki" style="margin: 0 auto" >
        
    </div>
    <script>
    
    function wyswietl(idek)
    {
        $("#wyniki").html("");
        $.ajax({
            data: 'id=' + idek,
            url: 'search.php',
            method: 'POST', // or GET
            success: function(msg) {                
                $("#wyniki").html(msg);
                skonstruuj_kontener(msg);
            }
        });     
    }
    
    function wyszukaj()
    {
       // var nazwa = $("#nazwa").val();
        var slowa = $("#slowa_").val(); 
        var data1 = $("#data1").val();
        var data2 = $("#data2").val();  
      //  var tag = $("#tag").val();
        if(slowa=="" && data1=="" && data2=="")
        {
            $("#wyszukaj_error").html("Brakuje danych");
            $("#wyszukaj_error").attr("class","komunikat_błąd");  
        }
        else
        {
            $("#wyszukaj_error").html("");
            $("#wyniki").html("");
            $.ajax({
                data: "zapytanie=1" + "&slowa=" + slowa + "&data1=" + data1 + "&data2=" + data2,
                url: 'search.php',
                method: 'POST', // or GET
                success: function(msg) {
                    $("#wyniki").html(msg);    
                    skonstruuj_kontener(msg); 
                }
            });
        }
    }
    
    </script>
	<script type="text/javascript" src="/js/cookie.js" ></script>
	<script type="text/javascript" src="/js/ext.js" ></script>
	<script type="text/javascript" src="/js/przegladanie.js" ></script>
    <?php include 'autorstwo.php';?>      
    <div id="czas" hidden="hidden">
        <?php echo date('H:i:s');?>
    </div>    
</body>
</html>