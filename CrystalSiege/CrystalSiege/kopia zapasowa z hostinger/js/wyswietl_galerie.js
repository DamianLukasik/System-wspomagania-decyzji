        var max, min;       

        var str     = document.cookie;
        var a       = str.search("galeria");
        var b       = str.search("#2");    
        var galeria = str.substring(a+8,b);   
        
        var a       = str.search("katalog");
        var b       = str.search("#1");    
        var katalog = str.substring(a+8,b); 
       // var i = znajdz_idek(c);
        załaduj_dane();

        function załaduj_dane(){//alert(katalog+"  "+galeria);
            $.ajax({
                
                data: 'galeria=' + galeria + "&katalog=" + katalog,
                url: 'wyswietl_zdjecia.php',
                method: 'POST', // or GET
                success: function(msg) {//alert();
                    //znajdz_idek_max(g,k);
                    $("#widok").html(msg);
                    zaladuj_dane();
                }
            });
                         
        }
        
        function wyswietl_zdjecie(nr){
            var wys=nr;
            var data = new Date();
            data.setTime(data.getTime()+(5*60*1000));
            var expires = "; expires="+data.toGMTString();
            document.cookie = "zdjecie=" + wys + "#3" + expires + "; path=/";  
            location.assign("/php/zdjecie.php");   
        }
        
        //skalowanie elementu
        
        var speed = 100;
        var sizeX = 320;
        var sizeY = 200;
        var stepX = 0;
        var stepY = 0;
        var steps = 20;
        var imgObj;
        
        function skaluj()        
        {
            var windowHeight = $(window).height();
            var windowWidth = $(window).width();            
            $("#galeria-zdjecia").height(windowHeight*0.8);//0.8
            $("#galeria-zdjecia").width(windowWidth*0.78); 
        }
        
        $(window).load(function() {            
            skaluj();            
        });
        $(window).resize(function() {            
            skaluj();            
        });
    
        
    
       