(function($) {
   
    var obrazek = "";
    var kod_z_obrazka = "";
    var nr = 0;
   
    $.fn.test = function(a) {       
        alert("Działa = "+a);       
    }
    
    $.fn.ustaw_kod_z_obrazka = function(str) {
        kod_z_obrazka = str;
    }
   
    $.fn.wygeneruj_obrazek = function () {
        var http = new XMLHttpRequest();
        var url = "php/generuj_kod.php";
        nr += 1;
        var params = "nr="+nr+"";
        http.open("POST", url, true);
        http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        http.onreadystatechange = function() {
        if(http.readyState == 4 && http.status == 200) {         
            kod_z_obrazka = http.responseText;
            var txt = http.responseText;
            $.fn.ustaw_kod_z_obrazka(txt);            
            obrazek = "php/kod_obrazka"+nr+".jpg";  
            document.getElementById("kod_z_obrazka").src = obrazek;
            }
        };
        http.send(params);      
    }
    
    $.fn.odswiez = function () {          
        $.fn.wygeneruj_obrazek();   
    }       
    
    $.fn.zwróć_kod = function() {
        return kod_z_obrazka;
    }
    
    $.fn.zwróć_obrazek = function() {
        return obrazek;
    }
    
    $.fn.zwróć_nr = function() {
        return nr;
    }
    
})(jQuery);