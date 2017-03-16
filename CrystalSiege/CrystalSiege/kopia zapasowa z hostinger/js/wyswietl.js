    function wyswietl_katalog(nr){    
            var katalog="";            
            switch(nr)
            {
                case 1:
                        katalog="lokalizacja";
                        tab=lokalizacja;
                        liczba=lokalizacja.length;
                    break;
                case 2:
                        katalog="okres";
                        tab=okres;
                        liczba=okres.length;
                    break;
                case 3:
                        katalog="urzadzenie";
                        tab=urzadzenie;
                        liczba=urzadzenie.length;
                    break;
                default:
                    break;
            }        
            var data = new Date();
            data.setTime(data.getTime()+(5*60*1000));
            var expires = "; expires="+data.toGMTString();
            document.cookie = "katalog=" + katalog + "#1" + expires + "; path=/";  

            location.assign("/php/katalog.php");
        }
    
        var str     = document.cookie;
        var n       = str.search("katalog");//cookie są kłopotliwe
        var m       = str.search("#1");
        var katalog = str.substring(n+8,m);    
        var tab=[];
        var liczba=0;
        var lokalizacja = ["-nieznane okolice-", "Błeszno", "Częstochówka-Parkitka", "Dźbów", 
            "Gnaszyn-Kawodrza", "Grabówka", "Kiedrzyn", "Lisiniec", "Mirów", "Ostatni Grosz", "Podjasnogórska",
            "Północ", "Raków", "Stare Miasto", "Stradom", "Śródmieście", "Trzech Wieszczów", "Tysiąclecie",
            "Wrzosowiak", "Wyczerpy-Aniołów", "Zawodzie-Dąbie"];

        var urzadzenie = ["-nieznane urządzenie-", "Aparat analogowy","Aparat fotograficzny",
            "Aparat cyfrowy","Komórka z aparatem","Smartfon","iPad","Tablet"];

        var okres = ['Okres uprzemysłowienia' ,'I wojna światowa' ,'Okres międzywojenny' ,'II wojna światowa' ,
            'Okres stabilizacji' ,'Czasy PRL' , 'Przemianny ustrojowe' , 'Współczesność'];    
        switch(katalog)
        {
            case "lokalizacja":
                tab=lokalizacja;
                liczba=lokalizacja.length;
                break;
            case "okres":
                tab=okres;
                liczba=okres.length;
                break;
            case "urzadzenie":
                tab=urzadzenie;
                liczba=urzadzenie.length;
                break;
            default:
               break;
        }    
        
        if(window.location.pathname=="/php/katalog.php")
        {            
            zaladuj_dane();
        }

        function zaladuj_dane(){
            $.ajax({
                data: 'katalog=' + katalog,
                url: 'wyswietl_katalogi.php',
                method: 'POST', // or GET
                success: function(msg) {
                    var arr = msg.split(",");
                    var kat = document.getElementById("katalogi");
                    for(var i=0;i<liczba;i++)
                    {
                        var tr = document.createElement('tr'); 
                        var td = document.createElement('input'); 
                        td.setAttribute("type", "button");
                        td.setAttribute("class", "przycisk-dluzszy");
                        td.setAttribute("value", tab[i]);    
                        td.setAttribute("id", "wyb"+i);  
                        td.addEventListener("click", wyswietl_galerie);
                        tr.appendChild(td);//rekonstruuj
						//pojemnik_inputow.push("wyb"+i);
						//kontener_inputow.push("wyb"+i);//?
                        var tb = document.createElement('input'); 
                        tb.setAttribute("type", "button");
                        tb.setAttribute("class", "przycisk-ikona");
                        tb.setAttribute("value", arr[i]);
                        tb.setAttribute("id", "ilosc_"+i);  
                        tr.appendChild(tb);
                        kat.appendChild(tr);
                    }
                    skonstruuj_kontener(msg);
                    
					//liczba_przyciskow = pojemnik_inputow.length;
                }
            });
        }
        
        function wyswietl_galerie(){
            var wys=this.value;
            var data = new Date();
            data.setTime(data.getTime()+(5*60*1000));
            var expires = "; expires="+data.toGMTString();
            document.cookie = "galeria=" + wys + "#2" + expires + "; path=/";  
            location.assign("/php/galeria.php");            
        }