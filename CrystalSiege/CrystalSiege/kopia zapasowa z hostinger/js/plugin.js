//skrypt wtyczki
(function($) { 
    
    var stan = true;
    
    //funkcja wtyczki
    $.fn.test = function(){
        //wyświetlenie komunikatu
        alert("Wtyczka działa prawidłowo");
    }
    
    $.fn.akcja = function(){
        if(stan){            
            this.zwij();
        }
        else{
            this.rozwin();
        }
        stan = !stan;        
    }
    
    $.fn.zwij = function (){
        alert("Funkcja wtyczki działa!");    
        var d = $("#idek");
        d.animate({
            height: '120px',
            width: '10%',
            fontSize: '5px',
            backgroundColor: "#FFE92F",
        },2800,"easeOutBounce");
    }
    
    $.fn.rozwin = function (){
        alert("Funkcja wtyczki działa!");        
        var d = $("#idek");
        d.animate({
            height: '420px',
            width: '100%',
            fontSize: '16px',
            backgroundColor: "#9ACD32",
        },2800,"easeInBounce");
    }
    
    //wtyczka z kursem walut
    
    $.fn.pobierz_kurs = function(){
        
        $("#kurs").html("ZZ");//zmiana wartości
        
        $.ajax({
            data: 'naz=1',
            url: 'polaczenie.php',
            method: 'POST', // or GET
            success: function(msg) {
                alert(msg);
            }
        });
        
        
        /*
        if (window.XMLHttpRequest) { 
            // code for IE7+, Firefox, Chrome, Opera, Safari 
            xmlhttp = new XMLHttpRequest(); 
        } 
        else { 
            // code for IE6, IE5 
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP"); 
        } 
        xmlhttp.open("GET", "http://www.nbp.pl/kursy/xml/LastA.xml", false); 
        xmlhttp.send(null); 
        if (xmlhttp.status==200) {
            xmlDoc = xmlhttp.responseXML; 
            alert();
        }
        else if (xmlhttp.status==404) {
          alert("XML could not be found");
         }
        
        
        /*
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function() {
          if (xhttp.readyState == 4 && xhttp.status == 200) {
            document.getElementById("demo").innerHTML =
            xhttp.responseText;
            alert();
          }
        };
        xhttp.open("GET", "http://www.nbp.pl/kursy/xml/LastA.xml", true);
        xhttp.send();
        */
        
        //var xhr = new XMLHttpRequest();
                 
        /* 
        $.ajax({
            url: "http://www.nbp.pl/kursy/xml/LastA.xml",
            success : function(data){
                alert();
                $("#kurs").html("VC");
            }
        });
        */
    };
    
    function parsujXML(xml){ 
        $(xml).find('pozycja').each(function(){
            var code = $(this).find('kod_waluty').text();
            var desc = $(this).find('kurs_sredni').text();
            alert(desc);
            //$("#kurs").html(desc);
        });      
    }
        
    
    
})(jQuery);