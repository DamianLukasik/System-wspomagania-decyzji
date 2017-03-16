
        //przeglądaj
        var tryb_przeglądania = false;
        
        function przeglądaj() {
            tryb_przeglądania = !tryb_przeglądania;
            if(tryb_przeglądania)
            {
                document.getElementById("panel_galerii").style.visibility = "visible";
                document.getElementById("wyb").style.visibility = "visible";
            }
            else
            {
                document.getElementById("panel_galerii").style.visibility = "hidden";
                document.getElementById("wyb").style.visibility = "hidden";
            }               
        }
        
        var lokalizacja = ["Błeszno", "Częstochówka-Parkitka", "Dźbów", "Gnaszyn-Kawodrza", "Grabówka", 
            "Kiedrzyn", "Lisiniec", "Mirów", "Ostatni Grosz", "Podjasnogórska", "Północ", "Raków", 
            "Stare Miasto", "Stradom", "Śródmieście", "Trzech Wieszczów", "Tysiąclecie", "Wrzosowiak", 
            "Wyczerpy-Aniołów", "Zawodzie-Dąbie"];
        
        var urzadzenie = ["nieznane urządzenie", "Aparat analogowy","Aparat fotograficzny",
            "Aparat cyfrowy","Komórka z aparatem","Smartfon","iPad","Tablet"];
        
        //format daty YYYY-MM-DD
        var daty  = ['1890-01-01','1914-07-28','1918-11-11','1939-09-01','1945-01-16','1952-07-22','1989-06-04','1999-01-01'];
        var okres = ['Okres uprzemysłowienia' ,'I wojna światowa' ,'Okres międzywojenny' ,'II wojna światowa' ,'Okres stabilizacji' ,'Czasy PRL' , 'Przemianny ustrojowe' , 'Współczesność'];
                 
        function widocznosc_paska_kategorii(widocznosc,napis,liczba){
            document.getElementById("wyb").style.visibility = widocznosc;
            document.getElementById("napis-1").textContent = napis;
            document.getElementById("wyb").style.height = liczba;
            document.getElementById("wyb-1").style.height = liczba;
            document.getElementById("wyb-2").style.height = liczba;
            document.getElementById("wyb-3").style.height = liczba;
        }
        
        function wybierz_katalog(wybor) {
            widocznosc_paska_kategorii("hidden","Wybierz galerie","0px");
            
            var tab, kategoria;
            switch(wybor)
            {
                case 1:tab=lokalizacja;kategoria="lokalizacja";                    
                    break;
                case 2:tab=okres;kategoria="okres";
                    break;
                case 3:tab=urzadzenie;kategoria="urzadzenie";
                    break;
            }
            document.getElementById("galeria").style.height=(Math.max(document.documentElement.clientHeight)-160)+"px";
            tworz_button("cofaj","Wycofaj","przycisk-dluzszy","wycofaj()","galeria-przyciski");
            document.getElementById("galeria-przyciski").appendChild(document.createElement('br'));                     
            var rozmiar = tab.length;
            for(var i=0;i<rozmiar;i++)
            {
                tworz_button("wyb-"+i,tab[i],"przycisk-dluzszy","wybierz_galerie('"+kategoria+"',this.value)","galeria-przyciski");                       
                tworz_button("liczba-"+tab[i],null,"przycisk-ikona",null,"galeria-przyciski");
                odczytaj_rozmiar_bazy(tab[i],"liczba-"+tab[i],kategoria);
                document.getElementById("galeria-przyciski").appendChild(document.createElement('br')); 
            }                   
                    
        }   
        
        function wycofaj(){
            wyczyść("galeria-przyciski"); 
            wyczyść("galeria-zdjecia"); 
            document.getElementById("galeria").style.height="0px";
            widocznosc_paska_kategorii("visible","Wybierz katalog","100%");
        }
        
        function tworz_button(id,wartosc,styl,akcja,elem){
            var element = document.createElement("input");
            element.type = "button";
            element.id = id;
            element.value = wartosc;
            element.setAttribute("class", styl);
            element.setAttribute("onclick",akcja);
            document.getElementById(elem).appendChild(element); 
        }
        
        var tab;
        
        function wybierz_galerie(kategoria,wybor) {                
            var rozmiar = document.getElementById("liczba-"+wybor).value;
            tab=new Array(rozmiar);
            tab=odczytaj_ideki_z_bazy(wybor,kategoria,rozmiar);
        }

        function załaduj_obrazek(id,elem,kategoria,idek){
            $.ajax({
                data: 'wyb=' + id + '&idek=' + idek + "&kat=" + kategoria,
                url: '/php/showimage_category.php',
                method: 'GET', //  GET or POST
                success: function(msg) {
                    var j = msg.indexOf("=");
                    var i = msg.indexOf("/",j+8);
                    idek = msg.substr(0,j);
                    document.getElementById(elem).src = 'data:'+msg.substr(j,i)+';base64,' + msg.substr(i);  
                }
            });//dopracować przeglądanie zdjęć.
        }
        
        function załaduj_dane(id,elem,kategoria,idek,dane){
            $.ajax({
                data: 'wyb=' + id + '&idek=' + idek + "&kat=" + kategoria + "&dane=" + dane,
                url: '/php/showdate_category.php',
                method: 'GET', //  GET or POST
                success: function(msg) {
                    if(dane=="urzadzenie")
                    {
                        msg = "Urządzenie : " + msg;
                    }
                    if(dane=="opis_zdjecia")
                    {
                        msg = "Opis zdjęcia :</br> " + msg;
                    }
                    if(dane=="data_wykonania")
                    {
                        var rok     = msg.substr(0,4);
                        var miesiac = msg.substr(5,2);
                        var dzien   = msg.substr(8,2);
                        switch(miesiac)
                        {
                            case "01":miesiac="styczeń";break;
                            case "02":miesiac="luty";break;
                            case "03":miesiac="marzec";break;
                            case "04":miesiac="kwiecień";break;
                            case "05":miesiac="maj";break;
                            case "06":miesiac="czerwiec";break;
                            case "07":miesiac="lipiec";break;
                            case "08":miesiac="sierpień";break;
                            case "09":miesiac="wrzesień";break;
                            case "10":miesiac="październik";break;
                            case "11":miesiac="listopada";break;
                            case "12":miesiac="grudnia";break;
                        }                        
                        msg = "" + dzien+" "+miesiac+" "+rok;
                    }
                    elem.innerHTML = msg;    
                //    buduj_element_kafelka(kafelek,msg,pole);
                }
            });//dopracować przeglądanie zdjęć.
        }
        /*
        function buduj_element_kafelka(kafelek,msg,pole){//skonstruować kafelek
            var row1 = kafelek.insertRow(1);
            var cell1 = row1.insertCell(0); 
            cell1.innerHTML = msg;
        }
        */
 //nazwa_zdjecia, opis_zdjecia, data_wykonania, lokalizacja, urzadzenie
 
        function odczytaj_rozmiar_bazy(wybor,id,kategoria){//alert(wybor +" "+ kategoria);
            $.ajax({
                data: 'wyb=' + wybor + "&kat=" + kategoria,
                url: '/php/showsizebase.php',
                method: 'GET', // POST or GET
                success: function(msg) {//alert(msg);
                    document.getElementById(id).value = msg;  
                }
            });
        }
        
        function odczytaj_ideki_z_bazy(wybor,kategoria,roz){
            $.ajax({
                data: 'wyb=' + wybor + "&kat=" + kategoria,
                url: '/php/showid_category.php',
                method: 'GET', // POST or GET
                success: function(msg) {      
                    wyczyść("galeria-zdjecia");              
                    tab = msg.split(',',roz);                                        
                    for(var i=0;i<roz;i++)
                    {         
                        stworz_kafelek(i,wybor,kategoria,tab[i]);                       
                  //      załaduj_obrazek(wybor,"img-"+i,kategoria,tab[i]);
                 //       załaduj_dane(wybor,"kafelek-"+i,kategoria,tab[i],"nazwa_zdjecia");
                      //  załaduj_dane(wybor,"kafelek-"+i,kategoria,tab[i],"nazwa_zdjecia",1);                        
                    }  
                }                
            });
        }
        
        function wyczyść(txt){
            var list = document.getElementById(txt);            
            while (list.hasChildNodes()) {
                list.removeChild(list.firstChild);
            }            
        }
        
        function stworz_kafelek(i,wybor,kategoria,idek){
            var kafelek = document.createElement("table");
            kafelek.setAttribute("class","kafelek-standard");
            kafelek.id = "kafelek-"+i;
            var row1 = kafelek.insertRow(0);
            var cell1 = row1.insertCell(0); 
            var x = document.createElement("IMG");//zrobić kafelki
            x.id = "img-"+i;
            x.style.height = "300px";
            x.style.weight = "300px";
            cell1.appendChild(x);   
            document.getElementById("galeria-zdjecia").appendChild(kafelek); 
            załaduj_obrazek(wybor,"img-"+i,kategoria,idek);
            
            var div = document.createElement("DIV");
            div.id="tytul";
            
            var row2 = kafelek.insertRow(1);
            var cell2 = row2.insertCell(0); 
            cell2.appendChild(div);
            
            załaduj_dane(wybor,div,kategoria,idek,"nazwa_zdjecia");
            kafelek.setAttribute("onclick","przeglądaj_zdjecie("+idek+",'"+kategoria+"','"+wybor+"')");
        }
        
        function przeglądaj_zdjecie(idek,kategoria,wybor){
            wyczyść("galeria-zdjecia");  
            stworz_kafel(idek,kategoria,wybor);
        }
        
        function stworz_kafel(idek,kategoria,wybor){  //przeglądaj zdjęcie   
            var kafel = document.createElement("table");
            kafel.setAttribute("class","kafelek-powiekszony");
            kafel.id = "kafel";  
            
            //konstruowanie kafla    
            
            var row1 = kafel.insertRow(0);
            var cell1 = row1.insertCell(0);
            var x = document.createElement("IMG");
            x.id = "img";
            x.style.height = "600px";
            x.style.weight = "600px";
            załaduj_obrazek(wybor,"img",kategoria,idek);
            cell1.appendChild(x);
                
            var row = kafel.insertRow(1); 
            var cell = row.insertCell(0);           
            tworz_element_kafelka("nazwa_zdjecia",wybor,kategoria,idek,cell,"tytul_zdjecia");
            
            var row = kafel.insertRow(2); 
            var cell = row.insertCell(0);            
            tworz_element_kafelka("autor",wybor,kategoria,idek,cell,"autor_zdjecia");
            tworz_element_kafelka("data_wykonania",wybor,kategoria,idek,cell,"data_zdjecia");
            tworz_element_kafelka("lokalizacja",wybor,kategoria,idek,cell,"lokalizacja_zdjecia");
            
            var row = kafel.insertRow(3); 
            var cell = row.insertCell(0);
            tworz_element_kafelka("opis_zdjecia",wybor,kategoria,idek,cell,"opis_zdjecia");
            
            var row = kafel.insertRow(4); 
            var cell = row.insertCell(0);
            tworz_element_kafelka("urzadzenie",wybor,kategoria,idek,cell,"urzadzenie_zdjecia");

            /*           
            var t = document.createTextNode("Male");
            a.appendChild(t);
            a.id = "tytul";
            załaduj_dane(wybor,"tytul",kategoria,id,"nazwa_zdjecia");
            cell1.innerHTML = a;
            
            var x = document.createElement("LABEL");
            
            x.setAttribute("for", "male");
            
            document.getElementById("myForm").insertBefore(x,document.getElementById("male"));
            */
            
            document.getElementById("galeria-zdjecia").appendChild(kafel);
        }
        
        function tworz_element_kafelka(dane,wybor,kategoria,idek,cell,klasa){                   
            var div = document.createElement("LABEL");
            div.setAttribute("class",klasa);
            document.body.appendChild(div);
            cell.appendChild(div);            
            załaduj_dane(wybor,div,kategoria,idek,dane);            
        }
        
        
        
        