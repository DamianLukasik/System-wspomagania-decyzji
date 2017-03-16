
        var windowHeight = $(window).height();
        var windowWidth = $(window).width();
        $("#galeria-zdjecia").height(windowHeight*0.9);//0.8
        $("#galeria-zdjecia").width(windowWidth*0.78);
        
        $(".img").height(windowHeight*0.7);          
        
        var str     = document.cookie;
        
        var a       = str.search("galeria");
        var b       = str.search("#2");    
        var galeria = str.substring(a+8,b);   
        
        var a       = str.search("katalog");
        var b       = str.search("#1");    
        var katalog = str.substring(a+8,b); 
        
        var a = str.search("zdjecie");
        var b = str.search("#3");
        var id = str.substring(a+8,b);
						
       // var i = znajdz_idek(c);
        zaladuj_dane();

        function zaladuj_dane(){
            $.ajax({
                data: 'id=' + id + '&galeria=' + galeria + "&katalog=" + katalog,
                url: 'wyswietl_zdjecie.php',
                method: 'POST', // or GET
                success: function(msg) {
                    $("#widok").html(msg);
                //    var kafel = document.getElementById("widok");
                //    kafel.innerHTML =msg;
                zaladuj_dane();
                }
            });
        }
        
        function poprzedni(){
            $("#nast_0").val($("#popd_0").val());
            wyswietl();
            zmien_numery(false);
        }
        
        function nastepny(){
            wyswietl();
            zmien_numery(true);
        }
        
        function zatzymanie(){
            if($("#odtw").val()==1)
            {
                $("#odtw").attr("value",0);
                $("#odtw_g").attr("src", "/grafika/play.gif");
                $("#odtw_p").attr("onclick", "odtwarzanie()");
            }
            location.reload();
        }
     
        function odtwarzanie(){
            $("#odtw").attr("value",1);
            $("#odtw_g").attr("src", "/grafika/pause.gif");
            $("#odtw_p").attr("onclick", "zatzymanie()");
            setInterval(dalej, 5000);//5000
        }
        
        function dalej(){
            wyswietl();
            zmien_numery(true);
        }
        
        function wyswietl(){
            var test = $("#in_1").val();  
            var id = $("#nast_0").val();   
            if(test!=undefined)
            { 
                var user = document.getElementById("user").value;
                $.ajax({
                    type: 'POST',
                    url: 'wyswietl_zdjecie.php',
                    data: 'idx=' + id + '&zweryfikowany=0' + "&user=" + user,
                    dataType: 'json',
                    cache: false,
                    success: function(result) {
                        $("#obraz_").attr("src",result[0]+"");
                        $("#tytul_zdjecia_").html(result[1]);
                        $("#opis_zdjecia_").html("Opis:</br>"+result[2]);//opis
                        $("#urzadzenie_").html("Urządzenie : "+result[3]);
                        $("#autor_").html(result[4]);
                        $("#data_wykonania_").html(result[5]);
                        $("#lokalizacja_").html(result[6]);
                        $("#nast_0").val(result[7]);
                        $("#popd_0").val(result[8]); 
                        $("#in_1").val(result[9]);
                        $("#in_2").val(result[9]);
                        $("#in_3").val(result[4]);
                        if(result[10])
                        {
							$("#but_3").val("Odblokuj użytkownika");
							$("#form_").attr("action", "/php/weryfikuj_zdjecia.php?akc=4");							
                        }
                        else
                        {
							$("#but_3").val("Zablokuj użytkownika");
							$("#form_").attr("action", "/php/weryfikuj_zdjecia.php?akc=2");
                        }
						if(result[11])
                        {
							$("#but_3").css("visibility","hidden");						
                        }
                        else
                        {
							$("#but_3").css("visibility","visible");
                        }
                        $("#in_4").val(result[9]);
                        var data = new Date();
                        data.setTime(data.getTime()+(5*60*1000));
                        var expires = "; expires="+data.toGMTString();
                        document.cookie = "zdjecie=" + id + "#3" + expires + "; path=/"; 
                        zaladuj_dane();
                    }
                });                
            }
            if(test==undefined)
            {
                $.ajax({
                    type: 'POST',
                    url: 'wyswietl_zdjecie.php',
                    data: 'idx=' + id + '&zweryfikowany=1' + '&galeria=' + galeria + "&katalog=" + katalog + "&user=" + user,
                    dataType: 'json',
                    cache: false,
                    success: function(result) {
                        $("#obraz_").attr("src",result[0]+"");
                        $("#tytul_zdjecia_").html(result[1]);
                        $("#opis_zdjecia_").html("Opis:</br>"+result[2]);//opis
                        $("#urzadzenie_").html("Urządzenie : "+result[3]);
                        $("#autor_").html(result[4]);
                        $("#data_wykonania_").html(result[5]);
                        $("#lokalizacja_").html(result[6]);
                        $("#nast_0").val(result[7]);
                        $("#popd_0").val(result[8]); 
                        var data = new Date();
                        data.setTime(data.getTime()+(5*60*1000));
                        var expires = "; expires="+data.toGMTString();
                        document.cookie = "zdjecie=" + id + "#3" + expires + "; path=/"; 
                    //    zaladuj_dane();
                    }
                });
            }            
        }
        
        function zmien_numery(nast){
            var num = $("#numeracja_").html();
            var numery = num.split("\\");           
            var i = parseInt(numery[0]);
            var j = parseInt(numery[1]);            
            if(nast==true)
            {
                i++;
                if(i>j){i=1;} 
            }
            else
            {
                i--;
                if(i==0){i=j;}                
            }               
            numery[0]=i;
            numery[1]=j;
            $("#numeracja_").html(i+"\\"+j);
        }