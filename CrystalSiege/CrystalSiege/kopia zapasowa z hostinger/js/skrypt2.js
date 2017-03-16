//alert("Działa");
var glos_;
var i_rozmiar_czcionki = 0;
$.ajax({
    data: 'glos=0',
    url: 'php/panel/lektor.php',
    method: 'GET', // or GET
    success: function(msg) {
        wylacz_lektora(msg);
    }
}).fail(function() {
    $.ajax({
        data: 'glos=0',
        url: 'panel/lektor.php',
        method: 'GET', // or GET
        success: function(msg) {
            wylacz_lektora(msg);
        }
    });
});

function wylacz_glos(){
    glos_ = !glos_;   
    var glos_int;
    if(glos_)
    {
        glos_int=2;
    }
    else
    {
        glos_int=1;
    }
   // alert("głos"+glos_+"  "+glos_int);
    $.ajax({
        data: 'glos='+glos_int,
        url: 'php/panel/lektor.php',
        method: 'GET', // or GET
        success: function(msg) {
            wylacz_lektora(msg);
        }
    }).fail(function() {
        $.ajax({
            data: 'glos='+glos_int,
            url: 'panel/lektor.php',
            method: 'GET', // or GET
            success: function(msg) {
                wylacz_lektora(msg);
            }
        });
    });
}

function wylacz_lektora(msg)
{
    if(msg=="0")
    {
        glos_=true;
        $("#przycisk98").val("Wyłącz glos lektora");
    }
    if(msg=="1")
    {
        glos_=false;
        $("#przycisk98").val("Włącz glos lektora");
    }
}

document.write("<div class='obsluga' id='obsluga'></div>");
var gdzie_jestem=window.location.pathname;
var wybrano_plik = false;
var pojemnik_inputow = [];
for(var i=0;i<document.getElementsByTagName("INPUT").length;i++)
{
	pojemnik_inputow[i] = document.getElementsByTagName("INPUT")[i].id;
}
//na stronie wysyłania zgłoszenia
if(document.getElementById('przycisk56') != null) 
{
	var a = document.getElementsByTagName("TEXTAREA")[0].id;
	pojemnik_inputow.push(a);
        var tmp = pojemnik_inputow[5];
        pojemnik_inputow[5]=pojemnik_inputow[6];
        pojemnik_inputow[6]=tmp;
        tmp = pojemnik_inputow[4];
        pojemnik_inputow[4]=pojemnik_inputow[5];
        pojemnik_inputow[5]=tmp;	
}
if(document.getElementById('przycisk31') != null) 
{
	var a = document.getElementsByTagName("TEXTAREA")[0].id;
	pojemnik_inputow.push(a);  
        for(var i=4;i<8;i++)
        {
            var tmp = pojemnik_inputow[8];
            pojemnik_inputow[8]=pojemnik_inputow[i];
            pojemnik_inputow[i]=tmp;
        }
        a = document.getElementsByTagName("SELECT")[0].id;
	pojemnik_inputow.push(a);  
        for(var i=6;i<9;i++)
        {
            var tmp = pojemnik_inputow[9];
            pojemnik_inputow[9]=pojemnik_inputow[i];
            pojemnik_inputow[i]=tmp;
        }
        a = document.getElementsByTagName("SELECT")[1].id;
	pojemnik_inputow.push(a);          
        for(var i=7;i<10;i++)
        {
            var tmp = pojemnik_inputow[10];
            pojemnik_inputow[10]=pojemnik_inputow[i];
            pojemnik_inputow[i]=tmp;
        }
}
//dymek informujący o polityce cookie
pojemnik_inputow.push("akcept");
var liczba_przyciskow = pojemnik_inputow.length;//alert(pojemnik_inputow);
var wybrany_przycisk;
var numer_wiersza = 0;
var numer_kolumny = 0;
var obecny_numer_przycisku = 0;
var numer_przycisku_na_gorze = 0;
var numer_przycisku_na_dole = 0; 
var popd_przycisk, obec_przycisk, nast_przycisk, gorny_przycisk, dolny_przycisk;
var nawigator;//[lewo,obecny,prawo,gora,dol]
var tryb_wprowadzania_tekstu=false;
var tryb_wprowadzania_wyboru=false;
var tryb_edycji_danych=false;
var tryb_zatwierdzania=false;
var kolory = ["#FEFE33","#33CC66","white","#B7D0DB","yellow","#FF2400","#BCE27F","#E34234","#84BAD0","#c86f82"];

var kontener_inputow;
var wielkosc_tablicy;
var pojemnik_;
///////
if($("#lab_id").html()[0]=="R")
{    
    pojemnik_ = [1,1];
}
if($("#lab_id").html()[0]=="W")
{    
    if($("#lab_id").html()[6]!="g")
    {
        pojemnik_inputow.unshift(document.getElementsByTagName("A")[0].id);
    }
    pojemnik_ = [1,7,2];
}
if($("#lab_id").html()[0]=="D")
{
    pojemnik_inputow.unshift(document.getElementsByTagName("A")[0].id);
    pojemnik_ = [1,6,3,2];
}
if($("#lab_id").html()[6]=="g")
{
    pojemnik_ = [5,2,5];
}

if(gdzie_jestem=="/php/profil.php")
{
    pojemnik_ = [2];
    pojemnik_inputow.pop();
    pojemnik_inputow.pop();//alert(pojemnik_inputow);
}
if(gdzie_jestem=="/" || gdzie_jestem=="/index.php")//konstruować za każdym razem!
{
    pojemnik_.push(3);
    pojemnik_.push(1);
}
if(gdzie_jestem=="/glowna.php" || gdzie_jestem=="/php/baza_zdjec.php")
{
    pojemnik_.push(3);
    pojemnik_.push(1);
}
if(gdzie_jestem=="/php/edytuj.php")
{        
    pojemnik_ = [1,1,1,1,1,1,1,1];
}
if(gdzie_jestem=="/php/przypomnij_haslo.php")
{    
    pojemnik_ = [1,1,1,2,1];
}
if(gdzie_jestem=="/php/przeslij.php")
{    
    pojemnik_ = [1,1,1,1,1,1,1,1,1,1,1];
}
if(gdzie_jestem=="/php/przeszukaj.php")
{
   
    pojemnik_.push(2);
    pojemnik_.push(2); //alert(pojemnik_inputow);
   // pojemnik_ = [];     
}
if(gdzie_jestem=="/zarejestruj.php")
{
	pojemnik_ = [1,1,1,1,1,1,1,2,1,1,1];
	var tryb_logowania = false;
}
if(gdzie_jestem=="/php/katalog.php")
{
    pojemnik_.push(1);
}
if(gdzie_jestem=="/php/formularzkontaktowy.php")
{
    pojemnik_ = [1,1,1,1,1,1];    
}
wielkosc_tablicy = pojemnik_.length;
kontener_inputow = tablica_dwuwymiarowa(wielkosc_tablicy,pojemnik_);	
//test_wyświetl();

function tablica_dwuwymiarowa(liczba_wierszy,tab_) {
	var tab = new Array(liczba_wierszy);
	for (var i = 0; i < liczba_wierszy; i++) {
		tab[i] = [];
	}
	var a=0;
	for (var i = 0; i < liczba_wierszy; i++) {//alert(tab_[i]);
		for (var j = 0; j < tab_[i]; j++) {
			tab[i][j] = pojemnik_inputow[a];
			a++;
		}
	}
	return tab;
}

var str = document.cookie;
var n = str.search("akceptacja");//cookie są kłopotliwe
var m = str.substr(n+11,1);		
if(m=="0")
{
	wielkosc_tablicy--;
}

$.ajax({
    data: 'jaka_wersja=0',
    url: 'php/panel/wybierz_styl.php',
    method: 'GET', // or GET
    success: function(msg) {
        msg=msg.substr(-1,1);
	if(msg=="2")
	{
            zmien_kolory(true);			
        }
	if(msg=="1")
        {
            zmien_kolory(false);
	}
    }
}).fail(function() {
    $.ajax({
    data: 'jaka_wersja=0',
    url: 'panel/wybierz_styl.php',
    method: 'GET', // or GET
    success: function(msg) {
	msg=msg.substr(-1,1);
	if(msg=="2")
	{
            zmien_kolory(true);			
        }
	if(msg=="1")
        {
            zmien_kolory(false);
	}
    }
	});
  });


//
$(window).keydown(function (e) {
    var keyCode = e.which;
	//alert("< "+poprzedni_numer_przycisku+" | "+obecny_numer_przycisku+" | "+nastepny_numer_przycisku+" >");
	console.log(e, keyCode, e.which);  
        if(!tryb_edycji_danych){
            if (keyCode == 37 && !tryb_wprowadzania_tekstu) {//lewo
                nawigator = poprzedni_input(kontener_inputow);			
                podswietl();
            }
            if (keyCode == 38) {//góra
                document.getElementById("obsluga").innerHTML = "Nacisnąłeś strzałkę w górę";  
                playSound("sound1");
                nawigator = wgore(kontener_inputow);//zrobić poprzednik na górze i następnik na dole
                podswietl();		
            }
            if (keyCode == 39 && !tryb_wprowadzania_tekstu) {//prawo
                nawigator  = nastepny_input(kontener_inputow);
                podswietl();
            }
            if (keyCode == 40) {//dół
                document.getElementById("obsluga").innerHTML = "Nacisnąłeś strzałkę w dół";
                playSound("sound2");
                nawigator  = wdol(kontener_inputow);
                podswietl();
            }        	
            if (keyCode == 107) {//zmniejszanie - klawisz -   
                    powieksz();
                    document.getElementById("obsluga").innerHTML = "Powiększono napisy";
                    playSound("sound3");
            }
            if (keyCode == 109) {//powiększanie - klawisz +
                    pomniejsz();
                    document.getElementById("obsluga").innerHTML = "Zmniejszono napisy";
                    playSound("sound4");
            }
            if (!tryb_wprowadzania_tekstu && keyCode == 120) {//zmiana na wersje kontrastową- klawisz 0                
                if($("link[rel=stylesheet]").attr("href")=="/css/styl_2.css")
		{
                    document.getElementById("obsluga").innerHTML = "Zmień styl strony na jaśniejszą";                                
                    playSound('sound63');
                }
		else
		{
                    document.getElementById("obsluga").innerHTML = "Zmień styl strony na wersję dla niedowidzących";
                    playSound("sound64");
		}
                zmien_styl();
            }   
            if (keyCode == 13) {//enter   
                if(tryb_wprowadzania_tekstu)
                {
                    nawigator  = nastepny_input(kontener_inputow);
                    podswietl();
                }
                if(gdzie_jestem=="/zarejestruj.php"){
                    if(tryb_zatwierdzania)//checkbox
                    {
                        if($("#pole6").prop('checked'))
                        {
                            $("#pole6").prop('checked', false);
                        }
                        else
                        {
                            $("#pole6").prop('checked', true);
                        }
                    }
                }
                if(tryb_logowania==false && obec == "przycisk01")
                {
                    document.getElementById("obsluga").innerHTML = "Wyświetl panel logowania do systemu";
                    playSound("sound6");
                }
                if(tryb_logowania==true && obec == "przycisk01")
                {
                    document.getElementById("obsluga").innerHTML = "Ukryj panel logowania do systemu";
                    playSound("sound7");
                }
            }
        }
	function pomniejsz(){
            i_rozmiar_czcionki--;
            if(i_rozmiar_czcionki<0)
            {
                i_rozmiar_czcionki=0;
            }
            else
            {
                var logiczna = true;
                wybierz_element(logiczna,"font-size",2);
                wybierz_element(logiczna,"width",8);
                wybierz_element(logiczna,"height",2);
            }
	}
	function powieksz(){
            i_rozmiar_czcionki++;
            if(i_rozmiar_czcionki>7)
            {
                i_rozmiar_czcionki=7;
            }
            else
            {
                var logiczna = false;
                wybierz_element(logiczna,"font-size",2);
                wybierz_element(logiczna,"width",8);
                wybierz_element(logiczna,"height",2);
            }
	}
        function wybierz_element(logiczna,atrybut,liczba){
            wybierz_atrybut("label",atrybut,logiczna,liczba);
             wybierz_atrybut(".napis",atrybut,logiczna,liczba);
                wybierz_atrybut("a",atrybut,logiczna,liczba);
                wybierz_atrybut("p",atrybut,logiczna,liczba);
                wybierz_atrybut(".tekst-zwykly",atrybut,logiczna,liczba);
                wybierz_atrybut(".obsluga",atrybut,logiczna,liczba);
                wybierz_atrybut(".przycisk-standard",atrybut,logiczna,liczba);
                wybierz_atrybut(".przycisk-dluzszy",atrybut,logiczna,liczba);
                wybierz_atrybut(".przycisk-ikona",atrybut,logiczna,liczba);
                wybierz_atrybut(".przycisk-ikona-dluzszy",atrybut,logiczna,liczba);
                wybierz_atrybut(".przycisk-ikona-zmien",atrybut,logiczna,liczba);
                wybierz_atrybut(".przycisk-ikona-usun",atrybut,logiczna,liczba);
                wybierz_atrybut(".text-standard-mini ",atrybut,logiczna,liczba);
                wybierz_atrybut(".przycisk-standard-mini",atrybut,logiczna,liczba);  
                wybierz_atrybut("select",atrybut,logiczna,liczba);  
                wybierz_atrybut("#data",atrybut,logiczna,liczba);  
                wybierz_atrybut(".text-standard",atrybut,logiczna,liczba);  
                wybierz_atrybut(".text-standard-przeslij",atrybut,logiczna,liczba);  
                wybierz_atrybut(".data-standard-przeslij",atrybut,logiczna,liczba);  
                wybierz_atrybut(".text-standard-mini",atrybut,logiczna,liczba);  
                wybierz_atrybut(".checkbox-standard",atrybut,logiczna,liczba);  
                wybierz_atrybut(".obszar_tekstowy-przeslij",atrybut,logiczna,liczba);  
                wybierz_atrybut(".cookies",atrybut,logiczna,liczba); 
                wybierz_atrybut(".komunikat_błąd",atrybut,logiczna,liczba); 
                wybierz_atrybut("li",atrybut,logiczna,liczba); 
                wybierz_atrybut("h1",atrybut,logiczna,liczba); 
                wybierz_atrybut("h2",atrybut,logiczna,liczba);      
                wybierz_atrybut("h4",atrybut,logiczna,liczba);  
                wybierz_atrybut(".text-nagłówek1",atrybut,logiczna,liczba);
        }
	function wybierz_atrybut(elem,atryb,w,licz){
		var atr = parseInt($(elem).css(atryb));
		var tmp = atr;
		if(w){
			atr = atr - licz;
		}
		else
		{
			atr = atr + licz;
		}
		switch(atryb){
			case "font-size":
                            $(elem).css({"font-size":atr});
                            break;
			case "width":
                            $(elem).css({"width":atr});
                            break;
			case "height":
                            $(elem).css({"height":atr});
                            break;
		}
	}		
	//$("input").blur();
});  
//
$(document).ready(function(){
    //alert(pojemnik_inputow);
	//a
        $("a").focus(function(){
            if($(this).id == "profil")//select 
            {
		$(this).css("background-color", kolory[4]);
                tryb_wprowadzania_wyboru=true;
            }
        });
        $("a").blur(function(){
            if($(this).id == "profil")//select 
            {
		$(this).css("background-color", kolory[4]);
                tryb_wprowadzania_wyboru=true;
            }
        });
        $("a").on({
            mouseenter: function(){//gdy się najecha myszką na
                podmien_kolory_inputow(2,8,3,true,9,"");
              //  $(this).focus();
                var idek = $(this).attr('id');
                nawigator = obecny(pojemnik_inputow,idek);//tu skończyłem
                obec_przycisk = document.getElementById(nawigator);
                komunikat();
                podmien_kolory_inputow(4,7,1,false,9,"");
            },  
            mouseleave: function(){//gdy sie odjecha myszką z	
                podmien_kolory_inputow(2,8,3,true,9,"");
              //  $("a").blur();		
                document.getElementById("obsluga").innerHTML = " ";
                podmien_kolory_inputow(2,8,3,false,9,"");
            }, 
            click: function(){
                podmien_kolory_inputow(2,8,3,true,9,"");
                var idek = $(this).attr('id');
                nawigator = nastepny_input(pojemnik_inputow,idek);//tu skończyłem
                obec_przycisk = document.getElementById(nawigator);
                komunikat();
                if($(this).attr('class') == "text-standard-przeslij")//textarea 
                {
                    $(this).css("background-color", kolory[4]);
                }   
                if($(this).attr('class') == "text-standard-mini")//textarea 
                {
                    $(this).css("background-color", kolory[4]);
                } 
            }  
        });	
        $("select").focus(function(){
            if($(this).attr('class') == "text-standard-przeslij")//select 
            {
		$(this).css("background-color", kolory[0]);
                tryb_wprowadzania_wyboru=true;
            }
            if($(this).attr('class') == "text-standard-mini")//select 
            {
		$(this).css("background-color", kolory[0]);
                tryb_wprowadzania_wyboru=true;
            }
	});
	$("select").blur(function(){
            if($(this).attr('class') == "text-standard-przeslij")//select 
            {
		$(this).css("background-color", kolory[2]);
                tryb_wprowadzania_wyboru=false;
            }
            if($(this).attr('class') == "text-standard-mini")//select 
            {
		$(this).css("background-color", kolory[2]);
                tryb_wprowadzania_wyboru=false;
            }
	});
	$("select").on({
            mouseenter: function(){//gdy się najecha myszką na
                podmien_kolory_inputow(2,8,3,true,9,"");
              //  $(this).focus();
                var idek = $(this).attr('id');
                $(this).css("background-color", kolory[0]);
                nawigator = obecny(pojemnik_inputow,idek);//tu skończyłem
                obec_przycisk = document.getElementById(nawigator);
                komunikat();
                podmien_kolory_inputow(4,7,1,false,9,"");
            },  
            mouseleave: function(){//gdy sie odjecha myszką z	
                podmien_kolory_inputow(2,8,3,true,9,"");
              //  $("input").blur();		
                document.getElementById("obsluga").innerHTML = " ";
                $(this).css("background-color", kolory[2]);
                podmien_kolory_inputow(2,8,3,false,9,"");
            }, 
            click: function(){
                podmien_kolory_inputow(2,8,3,true,9,"");
                var idek = $(this).attr('id');
                nawigator = nastepny_input(pojemnik_inputow,idek);//tu skończyłem
                obec_przycisk = document.getElementById(nawigator);
                komunikat();
                if($(this).attr('class') == "text-standard-przeslij")//textarea 
                {
                    $(this).css("background-color", kolory[4]);
                }   
                if($(this).attr('class') == "text-standard-mini")//textarea 
                {
                    $(this).css("background-color", kolory[4]);
                } 
            }  
        });	
	$("textarea").focus(function(){
		if($(this).attr('class') == "obszar_tekstowy-przeslij")//textarea 
		{
			$(this).css("background-color", kolory[0]);
                        tryb_wprowadzania_tekstu=true;
		}
	});
	$("textarea").blur(function(){
		if($(this).attr('class') == "obszar_tekstowy-przeslij")//textarea 
		{
			$(this).css("background-color", kolory[2]);
                        tryb_wprowadzania_tekstu=false;
		}
	});
	$("textarea").on({
            mouseenter: function(){//gdy się najecha myszką na
                //$("input").blur();
                podmien_kolory_inputow(2,8,3,true,9,"");
             //   $(this).focus();
                $(this).css("background-color", kolory[0]);
                var idek = $(this).attr('id');
                nawigator = obecny(pojemnik_inputow,idek);//tu skończyłem
                obec_przycisk = document.getElementById(nawigator);
                komunikat();
                podmien_kolory_inputow(4,7,1,false,9,"");
            },  
            mouseleave: function(){//gdy sie odjecha myszką z	
                podmien_kolory_inputow(2,8,3,true,9,"");
             //   $("input").blur();	
                $(this).css("background-color", kolory[2]);
                document.getElementById("obsluga").innerHTML = " ";
                podmien_kolory_inputow(2,8,3,false,9,"");
            }, 
            click: function(){
                podmien_kolory_inputow(2,8,3,true,9,"");
                var idek = $(this).attr('id');
                nawigator = obecny(pojemnik_inputow,idek);//tu skończyłem
                obec_przycisk = document.getElementById(nawigator);
                komunikat();
                if($(this).attr('class') == "obszar_tekstowy-przeslij")//textarea 
                {
                    $(this).css("background-color", kolory[4]);
                }
            }  
        });	
	//inputy
	$("input").focus(function(){
            if($(this).attr('type')=="checkbox")//checkbox
            {
		tryb_zatwierdzania = true;						
            }
            if($(this).attr('type')=="text" || $(this).attr('type')=="password")//pole tekstowe
            {
		$(this).css("background-color", kolory[0]);
		tryb_wprowadzania_tekstu=true;
            }	
            if($(this).attr('type')=="button" || $(this).attr('type')=="submit")//przycisk
            {
		$(this).css("background-color", kolory[7]);
                $(".przycisk-ikona-usun").css("background-color", kolory[10]);
            }
            if($(this).attr('type')=="date")//data
            {
		$(this).css("background-color", kolory[0]);							
            }
            if($(this).attr('type')=="file")//prześlij plik
            {
                if(!wybrano_plik)
                {
                    $("#upload-file-container").css('background-image', 'url("/grafika/upload_c_2.gif")');
                }
                else
                {
                    $("#upload-file-container").css('background-image', 'url("/grafika/upload_d_2.gif")');
                }                
            }
            if($(this).attr('class') == "obraz_kategori")//obraz
            {
            	$(this).css("background-color", kolory[1]);
            }	
            if($(this).attr('type')=="checkbox")//data
            {
		$(this).css("background-color", kolory[1]);							
            }
	});
	$("input").blur(function(){
            if($(this).attr('type')=="checkbox")//checkbox
            {
		tryb_zatwierdzania = false;						
            }
            if($(this).attr('type')=="text" || $(this).attr('type')=="password")//pole tekstowe
            {
            	$(this).css("background-color", kolory[2]);
		tryb_wprowadzania_tekstu=false;
            }
            if($(this).attr('type')=="button" || $(this).attr('type')=="submit")//przycisk
            {
		$(this).css("background-color", kolory[8]);	
                $(".przycisk-ikona-usun").css("background-color", kolory[10]);
            }
            if($(this).attr('class') == "obraz_kategori")//obraz
            {
		$(this).css("background-color", kolory[3]);
            } 
            if($(this).attr('type')=="checkbox")//data
            {
		$(this).css("background-color", kolory[3]);							
            }
	});
	$("input").on({
            mouseenter: function(){//gdy się najecha myszką na
                najechanie_myszka(this.id)
            },  
            mouseleave: function(){//gdy sie odjecha myszką z
                odjechanie_myszka(this.id);
            }, 
            click: function(){
                klikniecie_myszka(this.id);
            }  
        });	
});

function podswietl() {
	//alert("< "+poprzedni_numer_przycisku+" | "+obecny_numer_przycisku+" | "+nastepny_numer_przycisku+" >");
	
	obec_przycisk = document.getElementById(nawigator);
	//alert(obec_przycisk);
        
	obec_przycisk.focus();
        
        podmien_kolory_inputow(2,8,3,true,9,"");
        
        if($("#"+obec_przycisk.id).attr('class') == "obraz_kategori"){$("#"+obec_przycisk.id).css("background-color", kolory[1]);}   
        if($("#"+obec_przycisk.id).attr('class') == "obszar_tekstowy-przeslij"){obec_przycisk.style.background = kolory[4];}
        if($("#"+obec_przycisk.id).attr('class') == "data-standard-przeslij"){obec_przycisk.style.background = kolory[4];}
        if($("#"+obec_przycisk.id).attr('class') == "text-standard-przeslij"){obec_przycisk.style.background = kolory[4];}
        if($("#"+obec_przycisk.id).attr('class') == "text-standard-mini"){obec_przycisk.style.background = kolory[4];}
 
        if($("#"+obec_przycisk.id).attr('class') == "przycisk-ikona"){obec_przycisk.style.background = kolory[4];}
        if($("#"+obec_przycisk.id).attr('class') == "przycisk-dluzszy"){obec_przycisk.style.background = kolory[4];}
            
        if(!wybrano_plik)
        {
            if(obec_przycisk.id == "przycisk35")
            {
                $("#upload-file-container").css('background-image', 'url("/grafika/upload_c_2.gif")');
            }
            else
            {
                $("#upload-file-container").css('background-image', 'url("/grafika/upload_a_2.gif")');
            }
        }
        else
        {
            if(obec_przycisk.id == "przycisk35")
            {
                $("#upload-file-container").css('background-image', 'url("/grafika/upload_d_2.gif")');
            }
            else
            {
                $("#upload-file-container").css('background-image', 'url("/grafika/upload_b_2.gif")');
            }
        }

        if(obec_przycisk.type=="text" || obec_przycisk.type=="password"){obec_przycisk.style.background = kolory[4];}
	if(obec_przycisk.id=="akcept"){obec_przycisk.style.background = kolory[7];}

	if(obec_przycisk.name!="katalog")
	{	
            if(obec_przycisk.type=="button" || obec_przycisk.type=="submit"){obec_przycisk.style.background = kolory[7];}	
	}
	
	komunikat();
}
	
function komunikat() {
	if(obec_przycisk!=null)
	{
                if(obec_przycisk.id=="profil")
		{             
			document.getElementById("obsluga").innerHTML = "Wejdź do profilu ";
                        playSound("sound8");
		}
                if(obec_przycisk.id=="nazwa")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź nazwę szukanego zdjęcia";
                        playSound("sound9");
		}
                if(obec_przycisk.id=="slowa_")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź słowo";
                        playSound("sound10a");
		}
                if(obec_przycisk.id=="tag")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź tagi";
                        playSound("sound11");
		}
                if(obec_przycisk.id=="data1")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź datę początkową";
                        playSound("sound12");
		}
                if(obec_przycisk.id=="data2")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź datę końcową";
                        playSound("sound13");
		}
                if(obec_przycisk.id=="szukaj")
		{
			document.getElementById("obsluga").innerHTML = "Przeszukaj bazę zdjęć";
                        playSound("sound14a");
                }
                if(obec_przycisk.id.substr(0,6)=="image_")
		{
                        var wart = obec_przycisk.id.substr(6,6);             
                        wart = $("#tyt_img_"+wart).html();
                        document.getElementById(obec_przycisk.id).value;
			document.getElementById("obsluga").innerHTML = "Wyświetl zdjęcie \""+wart+"\"";
                        playSound("sound15");
		}
                if(obec_przycisk.id=="przycisk1")
		{
			document.getElementById("obsluga").innerHTML = "Zmień login";
                        playSound("sound16");
                        $("#przycisk1").click(function (){
                            document.getElementById("obsluga").innerHTML = "Wprowadź nowy login";
                            playSound("sound17");
                        });
		}
                if(obec_przycisk.id=="przycisk2")
		{
			document.getElementById("obsluga").innerHTML = "Zmień hasło";
                        playSound("sound18");
                        $("#przycisk2").click(function (){
                            document.getElementById("obsluga").innerHTML = "Wprowadź aktualne hasło";
                            playSound("sound19");
                            $("#pole2").blur(function (){
                                document.getElementById("obsluga").innerHTML = "Wprowadź nowe hasło";     
                                playSound("sound20");
                            });
                        });
		}
                if(obec_przycisk.id=="przycisk4")
		{
			document.getElementById("obsluga").innerHTML = "Zmień adres mailowy";
                        playSound("sound21");
                        $("#przycisk4").click(function (){
                            document.getElementById("obsluga").innerHTML = "Wprowadź nowy adres mailowy";   
                            playSound("sound22");
                        });
		}
                if(obec_przycisk.id=="przycisk5")
		{
			document.getElementById("obsluga").innerHTML = "Usuń konto";
                        playSound("sound23");
		}    
                if(obec_przycisk.id=="przycisk30")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź nazwę zdjęcia";
                        playSound("sound24");
		}  
                if(obec_przycisk.id=="przycisk31")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź opis zdjęcia";
                        playSound("sound25");
		} 
                if(obec_przycisk.id=="data")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź datę wykonania zdjęcia";
                        playSound("sound26");
		} 
                if(obec_przycisk.id=="przycisk32")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź lokalizację skąd pochodzi zdjęcie";
                        playSound("sound27");
		} 
                if(obec_przycisk.id=="przycisk33")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź urządzenie jakim wykonano zdjęcie";
                        playSound("sound28");
		} 
                if(obec_przycisk.id=="przycisk34")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź tagi do zdjęcia";
                        playSound("sound29");
		} 
                if(obec_przycisk.id=="przycisk35")
		{
			document.getElementById("obsluga").innerHTML = "Załącz plik zdjęcia";
                        playSound("sound30");
		} 
                if(obec_przycisk.id=="przycisk36")
		{
			document.getElementById("obsluga").innerHTML = "Prześlij zdjęcie";
                        playSound("sound31");
		} 
		if(obec_przycisk.id=="przycisk06")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź hasło logowania";
                        playSound("sound32");
		}
		if(obec_przycisk.id=="przycisk05")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź nazwę użytkownika";
                        playSound("sound33");
		}	
		if(obec_przycisk.id=="przycisk53")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź adres e-mail";
                        playSound("sound34");
		}
		if(obec_przycisk.id=="przycisk01")
		{
			if(tryb_logowania==false)
			{
				document.getElementById("obsluga").innerHTML = "Wyświetl panel logowania do systemu";
                                playSound("sound35");
			}
			else
			{
				document.getElementById("obsluga").innerHTML = "Ukryj panel logowania do systemu";
                                playSound("sound36");
			}			
		}
		if(obec_przycisk.id=="przycisk02")
		{
			document.getElementById("obsluga").innerHTML = "Załóż konto w systemie";
                        playSound("sound37");
		}                
                if(obec_przycisk.id=="pole0")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź nazwę użytkownika";
                        playSound("sound33");
		}
                if(obec_przycisk.id=="pole1")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź hasło";
                        playSound("sound74");
		}
                if(obec_przycisk.id=="pole2")
		{
			document.getElementById("obsluga").innerHTML = "Powtórz hasło";
                        playSound("sound75");
		}
                if(obec_przycisk.id=="pole4")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź adres mail";     
                        playSound("sound34");
		}
                if(obec_przycisk.id=="pole5")
		{
			document.getElementById("obsluga").innerHTML = "Odnów kod Captcha";  
                        playSound("sound76");
		}
                if(obec_przycisk.id=="znak550")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź kod Captcha";
                        playSound("sound77");
		}
                if(obec_przycisk.id=="pole6")
		{
			document.getElementById("obsluga").innerHTML = "Zaakceptuj warunki";
                        playSound("sound38");
		}
                if(obec_przycisk.id=="przycisk77")
		{
			document.getElementById("obsluga").innerHTML = "Załóż konto";
                        playSound("sound37");
		}                
		if( obec_przycisk.id=="przycisk22")
		{
			document.getElementById("obsluga").innerHTML = "Znajdź zdjęcia";
                        playSound("sound39");
		}
		if(obec_przycisk.id=="przycisk23")
		{
			document.getElementById("obsluga").innerHTML = "Przeglądaj zdjęcia";
                        playSound("sound40");
		}                
                if(obec_przycisk.id=="zaakc")
		{
			document.getElementById("obsluga").innerHTML = "Zaakceptuj zdjęcie";
                        playSound("sound41");
		}
                if(obec_przycisk.id=="odrzu")
		{
			document.getElementById("obsluga").innerHTML = "Odrzuć zdjęcie";
                        playSound("sound42");
		}
                if(obec_przycisk.id=="but_3")
		{
                    if($("#"+obec_przycisk.id).val()[0]=="Z")
                    {
                        document.getElementById("obsluga").innerHTML = "Zablokuj użytkownika";
                        playSound("sound43");
                    }
                    else
                    {
                        document.getElementById("obsluga").innerHTML = "Odblokuj użytkownika";
                        playSound("sound44");
                    }                   
		}
                if(obec_przycisk.id=="edytu")
		{
			document.getElementById("obsluga").innerHTML = "Edytuj dane zdjęcia";
                        playSound("sound45");
		}                
		if(obec_przycisk.id=="przycisk07")
		{
			document.getElementById("obsluga").innerHTML = "Zaloguj się do systemu";
                        playSound("sound46");
		}
		if(obec_przycisk.id=="przycisk08")
		{
			document.getElementById("obsluga").innerHTML = "Podaj moje hasło";
                        playSound("sound47");
		}
		if(obec_przycisk.id=="przycisk09")
		{
			document.getElementById("obsluga").innerHTML = "Napisz o błędzie";
                        playSound("sound48");
		}	
		if(obec_przycisk.id=="przycisk52")
		{
			document.getElementById("obsluga").innerHTML = "Wróć do strony głównej";
                        playSound("sound49");
		}
		if(obec_przycisk.id=="przycisk54")
		{
			document.getElementById("obsluga").innerHTML = "Wyślij wiadomość z hasłem";
                        playSound("sound50");
		}
		if(obec_przycisk.id=="przycisk55")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź tytuł wiadomości";
                        playSound("sound51");
		}	
		if(obec_przycisk.id=="przycisk56")
		{
			document.getElementById("obsluga").innerHTML = "Wprowadź treść wiadomości";
                        playSound("sound52");
		}
		if(obec_przycisk.id=="przycisk57")
		{
			document.getElementById("obsluga").innerHTML = "Wyślij zgłoszenie";
                        playSound("sound53");
		}	
		if(obec_przycisk.id=="przycisk_1")
		{
			document.getElementById("obsluga").innerHTML = "Wybierz katalog \"Lokalizacja\"";
                        playSound("sound54");
		}	
		if(obec_przycisk.id=="przycisk_2")
		{
			document.getElementById("obsluga").innerHTML = "Wybierz katalog \"Okres historyczny\"";
                        playSound("sound55");
		}
		if(obec_przycisk.id=="przycisk_3")
		{			
			document.getElementById("obsluga").innerHTML = "Wybierz katalog \"Urządzenie\"";
                        playSound("sound56");
		}
		if(obec_przycisk.id=="przycisk21")
		{
			document.getElementById("obsluga").innerHTML = "Prześlij swoje zdjęcie";
                        playSound("sound57");
		}
		if(obec_przycisk.id=="przycisk24")
		{
			document.getElementById("obsluga").innerHTML = "Edytuj swoje dane profilowe";
                        playSound("sound58");
		}
		if(obec_przycisk.id=="przycisk25")
		{
			document.getElementById("obsluga").innerHTML = "Wyloguj się z systemu";
                        playSound("sound59");
		}
		if(obec_przycisk.id=="przycisk26")
		{
			document.getElementById("obsluga").innerHTML = "Zarządzaj bazą danych";
                        playSound("sound60");
		}
		if(obec_przycisk.id=="przycisk27")
		{
                        //stopSound();
			document.getElementById("obsluga").innerHTML = "Sprawdź zdjęcia do publikacji";
                        playSound('sound61');
		}
		if(obec_przycisk.id=="przycisk28")
		{
                        //stopSound();
			document.getElementById("obsluga").innerHTML = "Odbierz wiadomości";                        
                        playSound('sound62');
		}
                if(obec_przycisk.id=="przycisk99")
		{
			if($("link[rel=stylesheet]").attr("href")=="/css/styl_2.css")
			{
				document.getElementById("obsluga").innerHTML = "Zmień styl strony na jaśniejszą";                                
                                playSound('sound63');
                        }
			else
			{
				document.getElementById("obsluga").innerHTML = "Zmień styl strony na wersję dla niedowidzących";
                                playSound("sound64");
			}
		}
                if(obec_przycisk.id=="przycisk98")
		{
			if(glos_)
			{
				document.getElementById("obsluga").innerHTML = "Wyłącz głos lektora";                                
                                playSound('sound79');
                        }
			else
			{
				document.getElementById("obsluga").innerHTML = "Włącz głos lektora";
                                playSound("sound78");
			}
		}
		if(obec_przycisk.id=="cofaj")
		{
			document.getElementById("obsluga").innerHTML = "Wybierz inny katalog";
                        playSound("sound65");
		}
		if(obec_przycisk.id.substr(0,3)=="wyb")
		{
                    var str = "_";
                    if($("#wyb1").val()=="Błeszno")
                    {
                        str += "a"+document.getElementById(obec_przycisk.id).id.substr(3,2);
                    }
                    if($("#wyb1").val()=="I wojna światowa")
                    {
                        str += "b"+document.getElementById(obec_przycisk.id).id.substr(3,2);
                    }
                    if($("#wyb1").val()=="Aparat analogowy")
                    {
                        str += "c"+document.getElementById(obec_przycisk.id).id.substr(3,2);
                    }
                    document.getElementById("obsluga").innerHTML = "Wybierz galerie \""+document.getElementById(obec_przycisk.id).value+"\"";
                    playSound("sound"+str);
                }
                if(obec_przycisk.id.substr(0,6)=="ilosc_")
		{
                        var wart = obec_przycisk.id.substr(6,2);
                       
                        wart = $("#wyb"+wart).val();
                         //alert(wart);
                        var str;
                        if(document.getElementById(obec_przycisk.id).value==1)
                        {
                            str="jest 1 zdjęcie";
                        }
                        else
                        {
                            str = "nie ma żadnych zdjęć";
                        }
                        if(document.getElementById(obec_przycisk.id).value>1)
                        {
                            str="są "+document.getElementById(obec_przycisk.id).value+" zdjęcia";
                            if(document.getElementById(obec_przycisk.id).value>=5)
                            {
                                str="znajduje się "+document.getElementById(obec_przycisk.id).value+" zdjęć";
                            }
                        }
			document.getElementById("obsluga").innerHTML = "W galerii \""+wart+"\" "+str;
		}
                if(obec_przycisk.id.substr(0,10)=="zdjecie_id")
		{
                        var wart = obec_przycisk.id.substr(10,6);             
                        wart = $("#zdjecie_i"+wart).html();
                        document.getElementById(obec_przycisk.id).value;
			document.getElementById("obsluga").innerHTML = "Wyświetl zdjęcie \""+wart+"\"";
                        playSound("sound67");
		}
                if(obec_przycisk.id=="popd_")
		{
			document.getElementById("obsluga").innerHTML = "Przejdź do poprzedniego zdjęcia";
                        playSound("sound68");
		}
                if(obec_przycisk.id=="nast_")
		{
			document.getElementById("obsluga").innerHTML = "Przejdź do następnego zdjęcia";
                        playSound("sound69");
		}
                if(obec_przycisk.id=="odtw_g")
		{
                        if($("#odtw_g").attr("src")=="/grafika/play.gif")
                        {
                            document.getElementById("obsluga").innerHTML = "Odtwórz pokaz slajdów";
                            playSound("sound70");
                            $("#odtw_g").click(function (){
                                document.getElementById("obsluga").innerHTML = "Zatrzymaj pokaz slajdów";
                                playSound("sound71");
                            });
                        }
                        else
                        {
                            document.getElementById("obsluga").innerHTML = "Zatrzymaj pokaz slajdów";
                            playSound("sound71");
                            $("#odtw_g").click(function (){
                                document.getElementById("obsluga").innerHTML = "Odtwórz pokaz slajdów";
                                playSound("sound70");
                            });
                        }
		}
                if(obec_przycisk.id=="obraz_")
		{
			document.getElementById("obsluga").innerHTML = "Zaznaczyłeś zdjęcie";
                        playSound("sound72");
		}
		if(obec_przycisk.id=="akcept")
		{
			document.getElementById("obsluga").innerHTML = "Akceptuj warunki polityki cookies";
                        playSound("sound73");
		}
	}
}
//ku skończyłem
function obecny(pojemnik_inputow,idek) {
	
	for(var i=0;i<wielkosc_tablicy;i++)
	{
                if(kontener_inputow[i]===undefined){break;}
		var a = kontener_inputow[i].indexOf(idek);
		if(a!="-1")
		{
			numer_wiersza=i;
			numer_kolumny=a;
		}
	}
	
	obecny_numer_przycisku=pojemnik_inputow.indexOf(idek);
	var obec = pojemnik_inputow[obecny_numer_przycisku];
	
	if(obec=="przycisk21")
	{
		numer_przycisku_na_gorze = "przycisk26"; 
	}
	if(obec=="przycisk22")
	{
		numer_przycisku_na_gorze = "przycisk27"; 
	}
	if(obec=="przycisk23" || obec=="przycisk24" || obec=="przycisk25" || obec=="przycisk52")
	{
		numer_przycisku_na_gorze = "przycisk28"; 
	}
	if(obec=="przycisk26" || obec=="przycisk27" || obec=="przycisk28")
	{
		numer_przycisku_na_gorze = "przycisk99"; 
	}
	if(obec=="przycisk21")
	{
		numer_przycisku_na_gorze = "przycisk_1"; 
	}
	
	return obec;	
}

function wdol(pojemnik_inputow) {
	numer_wiersza++;
	//alert(numer_wiersza);
	if(numer_wiersza>wielkosc_tablicy-1)
	{
		numer_wiersza=0;		
	}
	var kontener_1 = kontener_inputow[numer_wiersza];
	if(tryb_logowania == false && kontener_1[0] == "przycisk05")
	{		
		numer_wiersza++;
		//alert(numer_wiersza);
		kontener_1 = kontener_inputow[numer_wiersza];
		//alert(kontener_1);	
	}
	var idek = kontener_1[numer_kolumny];	
	while(idek==undefined)
	{
		numer_kolumny = kontener_inputow[numer_wiersza].length-1;
		var idek = kontener_inputow[numer_wiersza][numer_kolumny];	
		kontener_1 = kontener_inputow[numer_wiersza];
	}
	var str = document.cookie;
	var n = str.search("akceptacja");//cookie są kłopotliwe
	var m = str.substr(n+11,1);		
	if(m=="0" && kontener_1[0]=="akcept")
	{
		numer_wiersza=0;		
		kontener_1 = kontener_inputow[numer_wiersza];
	}	
	var idek = kontener_1[numer_kolumny]; 
	
	
	return idek;
}

function wgore(pojemnik_inputow) {
	numer_wiersza--;
	if(numer_wiersza<0)
	{
		numer_wiersza=wielkosc_tablicy-1;
		//
	}	
	var kontener_1 = kontener_inputow[numer_wiersza];
	if(tryb_logowania == false && kontener_1[0] == "przycisk05")
	{		
		numer_wiersza--;
		kontener_1 = kontener_inputow[numer_wiersza];
	}
	var idek = kontener_1[numer_kolumny];
	while(idek==undefined)
	{
		numer_kolumny = kontener_inputow[numer_wiersza].length-1;
		var idek = kontener_inputow[numer_wiersza][numer_kolumny];	
		kontener_1 = kontener_inputow[numer_wiersza];
	}
	var str = document.cookie;
	var n = str.search("akceptacja");//cookie są kłopotliwe
	var m = str.substr(n+11,1);		
	if(m=="0" && kontener_1[0]=="akcept")
	{
		numer_wiersza=wielkosc_tablicy-1;		
		kontener_1 = pojemnik_inputow[numer_wiersza];
	}
	//alert(numer_wiersza+" "+numer_kolumny+" "+idek);
	return idek;
}

function poprzedni_input(pojemnik_inputow) {
	var kontener_1 = pojemnik_inputow[numer_wiersza];
	var c = numer_kolumny;
	if(numer_kolumny<=0)
	{
		numer_kolumny=pojemnik_inputow[wielkosc_tablicy-1].length;//alert(numer_kolumny);
		c = 0;
		numer_wiersza--;
		var a =numer_wiersza;
		if(a<0){a=wielkosc_tablicy}
		var kontener_1 = pojemnik_inputow[a];
		var str = document.cookie;
		var n = str.search("akceptacja");//cookie są kłopotliwe
		var m = str.substr(n+11,1);		
		if(m=="0" && kontener_1[0]=="akcept")
		{
			numer_wiersza=wielkosc_tablicy-1;		
			kontener_1 = pojemnik_inputow[numer_wiersza];
		}
		if(tryb_logowania == false && pojemnik_inputow[numer_wiersza][0] == "przycisk05")
		{
			numer_wiersza--;
			kontener_1 = pojemnik_inputow[numer_wiersza];
		}
		numer_kolumny=pojemnik_inputow[numer_wiersza].length;
		//alert(numer_wiersza+" wiersz");	
	}
	numer_kolumny--;
	var idek = pojemnik_inputow[numer_wiersza][numer_kolumny];	
	//alert(numer_wiersza+" "+numer_kolumny+" "+);
	return idek;
}

function nastepny_input(pojemnik_inputow) {	
	var kontener_1 = pojemnik_inputow[numer_wiersza];
	var c = numer_kolumny;//alert(pojemnik_inputow[numer_wiersza]+" , "+pojemnik_inputow[numer_wiersza].length);
	if(numer_kolumny>=pojemnik_inputow[numer_wiersza].length)
	{
		numer_kolumny=0;
		c = 0;
		var a =numer_wiersza-1;
		if(a<0){a=wielkosc_tablicy}
		if(pojemnik_inputow[a][0]=="akcept"){a--;}
		var b=numer_wiersza+1;
		if(tryb_logowania == false && pojemnik_inputow[numer_wiersza][0] == "przycisk05")
		{
			var b =numer_wiersza+1;
			numer_wiersza++;
		}
		else
		{
			var b =numer_wiersza+1;	
			
		}
		var kontener_2 = pojemnik_inputow[b];
		if(b>wielkosc_tablicy)
		{
			b=wielkosc_tablicy;
		}
		var str = document.cookie;
		var n = str.search("akceptacja");//cookie są kłopotliwe
		var m = str.substr(n+11,1);		
		if(m=="0" && kontener_1[0]=="akcept")
		{
			kontener_1 = pojemnik_inputow[0];
			numer_wiersza=0;
		}
		numer_wiersza++;		
	}
	numer_kolumny++;
	if(numer_kolumny>=pojemnik_inputow[numer_wiersza].length)
	{		
		numer_wiersza++;
		if(tryb_logowania == false && pojemnik_inputow[numer_wiersza][0] == "przycisk05")
		{
			numer_wiersza++;
		}
		var str = document.cookie;
		var n = str.search("akceptacja");//cookie są kłopotliwe
		var m = str.substr(n+11,1);	//alert(pojemnik_inputow[numer_wiersza][0]);
                if(pojemnik_inputow[numer_wiersza] === undefined)
                {
                    numer_wiersza=0;
                }
		if(m=="0" && pojemnik_inputow[numer_wiersza][0]=="akcept")
		{
			kontener_1 = pojemnik_inputow[0];
			numer_wiersza=0;
		}
		numer_kolumny=0;
	}
	idek = pojemnik_inputow[numer_wiersza][numer_kolumny];
	return idek;
}   

var tekst_do_inputa = ['Dla słabowidzących', 'Wersja graficzna'];
if($("link[rel=stylesheet]").attr("href")=="/css/styl_2.css")
{
    $("#przycisk99").val(tekst_do_inputa[1]);
}
else
{
    $("#przycisk99").val(tekst_do_inputa[0]);
}
      
function zmien_styl() {	
	var wybrano = 0;        
	if($("link[rel=stylesheet]").attr("href")=="/css/styl_2.css")
	{
		wybrano=1;
                $("#przycisk99").val(tekst_do_inputa[0]);
	}
	else
	{
		wybrano=2;
                $("#przycisk99").val(tekst_do_inputa[1]);
	}
	$.ajax({
        data: 'wybrano_wersje='+wybrano,
        url: 'php/panel/wybierz_styl.php',
        method: 'GET', // or GET
        success: function(msg) {
            if(msg=="2")
            {
                $("link[rel=stylesheet]").attr({href : "/css/styl_2.css"});                
                zmien_kolory(true);			
            }
            if(msg=="1")
            {
                $("link[rel=stylesheet]").attr({href : "/css/styl.css"});
		zmien_kolory(false);
            }
        }
    }).fail(function() {
    $.ajax({
    data: 'wybrano_wersje='+wybrano,
    url: 'panel/wybierz_styl.php',
    method: 'GET', // or GET
    success: function(msg) {
	if(msg=="2")
	{
            $("link[rel=stylesheet]").attr({href : "/css/styl_2.css"});
            zmien_kolory(true);			
        }
        if(msg=="1")
        {
            $("link[rel=stylesheet]").attr({href : "/css/styl.css"});
            zmien_kolory(false);
	}
    }
	});
  });
}

function zmien_kolory(i) {
	if(i)
	{
            kolory = ["black","black","black","black","black","black","black","black","black","black"];
            if(typeof siła_hasła_kolory != "undefined"){siła_hasła_kolory = ["yellow","yellow","yellow","yellow","yellow"];}		
	}
	else
	{
            kolory = ["#FEFE33","#33CC66","white","#B7D0DB","yellow","#FF2400","#BCE27F","#E34234","#84BAD0","#c86f82"];
            if(typeof siła_hasła_kolory != "undefined"){siła_hasła_kolory = ["red","orange","yellow","greenyellow","green"];}	
	}     
        
        podmien_kolory_inputow(2,8,3,true,9,"");
}

if(gdzie_jestem=="/php/edytuj.php")
{
  //  alert(kontener_inputow);
    kontener_inputow.splice(3,1);
    pojemnik_inputow.unshift(document.getElementsByTagName("A")[0].id);
    kontener_inputow[0].unshift(document.getElementsByTagName("A")[0].id);
}
if(gdzie_jestem=="/php/odbierz_zgloszenie.php")
{
    
    //kontener_inputow.splice(1,1);
}
if(gdzie_jestem=="/php/weryfikuj_zdjecia.php")
{  
    kontener_inputow[4]=["popd_","obraz_","nast_"];
    kontener_inputow[5]=["zaakc","odrzu","but_3","edytu"];
}
if(gdzie_jestem=="/php/odbierz_zgloszenie.php")
{   
    var len = document.getElementsByTagName("INPUT").length-10;
    for(var i = 0;i<len;i++)
    {
        kontener_inputow[i+4]=["zgloszenie"+i];
    }
    //alert(kontener_inputow);
}
if(gdzie_jestem=="/php/zarzadzaj.php")
{  
    kontener_inputow[4]=[];
    kontener_inputow[4][0]="guzik_wybierz";
    kontener_inputow[4][1]="wybrano";      
    if(document.getElementById("wybrana_tabela"))
    {
        if($("#wybrana_tabela").html()[17]=="U")
        {
            kontener_inputow[5]=[];
            for(var i=0;i<9;i++)
            {
                pojemnik_inputow.push("guzik_wybierz");
                kontener_inputow[5][i]="sort"+i;
            }
            var len = (document.getElementsByTagName("INPUT").length-20)/11;
            var str;//alert(len);     
            var j=0;
            for(var i = 6;i<=len+6;i++)
            {
                kontener_inputow[i]=[];
                kontener_inputow[i][0]="login_"+j;
                pojemnik_inputow.push("login_"+j);
                kontener_inputow[i][1]="haslo_"+j;
                pojemnik_inputow.push("haslo_"+j);
                kontener_inputow[i][2]="zapisz_to_"+j;
                pojemnik_inputow.push("zapisz_to_"+j);
                kontener_inputow[i][3]="zablokuj_to_"+j;
                pojemnik_inputow.push("zablokuj_to_"+j);
                kontener_inputow[i][4]="uprawnij_to_"+j;
                pojemnik_inputow.push("uprawnij_to_"+j);
                kontener_inputow[i][5]="skasuj_to_"+j;  
                pojemnik_inputow.push("skasuj_to_"+j);
                j++;              
            }
        }
        if($("#wybrana_tabela").html()[17]=="Z")
        {
            kontener_inputow[5]=[];
            for(var i=0;i<11;i++)
            {
                pojemnik_inputow.push("guzik_wybierz");
                kontener_inputow[5][i]="sort"+i;
            }
            var len = (document.getElementsByTagName("INPUT").length-22)/9;
            var str;    
            var j=0;
            for(var i = 6;i<len+6;i++)
            {
                kontener_inputow[i]=[];
                kontener_inputow[i][0]="nazwa_zdjecia"+j;
                pojemnik_inputow.push("nazwa_zdjecia"+j);
                kontener_inputow[i][1]="opis_zdjecia"+j;
                pojemnik_inputow.push("opis_zdjecia"+j);
                kontener_inputow[i][2]="data_wykonania"+j;
                pojemnik_inputow.push("data_wykonania"+j);
                kontener_inputow[i][3]="lokalizacja__"+j;
                pojemnik_inputow.push("lokalizacja__"+j);
                kontener_inputow[i][4]="urzadzenie__"+j;
                pojemnik_inputow.push("urzadzenie__"+j);
                kontener_inputow[i][5]="tag_"+j; 
                pojemnik_inputow.push("tag_"+j);
                kontener_inputow[i][6]="zapisz_to_"+j;  
                pojemnik_inputow.push("zapisz_to_"+j);
                kontener_inputow[i][7]="usun_to_"+j;  
                pojemnik_inputow.push("usun_to_"+j);
                j++;              
            }
        }
    }
    //alert(kontener_inputow);
}
if(gdzie_jestem=="/zarejestruj.php")
{
    pojemnik_inputow.unshift("regulamin");
    kontener_inputow[0].unshift("regulamin");
}

function skonstruuj_kontener(msg) {
        if(msg.search("Nie znaleziono zdjęć spełniających wprowadzone kryteria")=="-1" || msg.search("Wszystkie zdjęcia zostały zweryfikowane")=="-1")
        {//alert(msg);
            kontener_inputow = null;
            pojemnik_inputow = null;
            kontener_inputow = [];
            pojemnik_inputow = [];
            kontener_inputow.length = 0;//alert(kontener_inputow); 
            pojemnik_inputow.length = 0;
            for(var i=0;i<document.getElementsByTagName("INPUT").length;i++)
            {
                pojemnik_inputow[i] = document.getElementsByTagName("INPUT")[i].id;                
            }
            if($("#lab_id").html()[0]=="W")
            {    
                if($("#lab_id").html()[6]!="g")
                {
                    pojemnik_inputow.unshift(document.getElementsByTagName("A")[0].id);
                }
                pojemnik_ = [1,7,2];
            }
            if($("#lab_id").html()[0]=="D")
            {
                pojemnik_inputow.unshift(document.getElementsByTagName("A")[0].id);
                pojemnik_ = [1,6,3,2];
            }
            if($("#lab_id").html()[6]=="g")
            {
                pojemnik_ = [5,2,5];
            } 
            if(gdzie_jestem=="/php/przeszukaj.php")
            {
               // alert(pojemnik_inputow);
                pojemnik_.push(2);
                pojemnik_.push(2);              
            }    
            if(gdzie_jestem=="/php/profil.php")
            {
                pojemnik_ = [2,1,1];                
            }   
            pojemnik_.push(1);
            var aaa = document.getElementsByTagName("INPUT").length;
           // liczba_przyciskow = pojemnik_inputow.length;
            
            var len = aaa-liczba_przyciskow;
            var eee = aaa-len-1;        //  alert(aaa+" "+eee+" "+len);        
            for(var i=0;i<=len;i++)
            {
                if(i==len)
                {
                    pojemnik_.push(1);
                }
                else
                {
                    pojemnik_.push(2);
                }
                i++;
            }
            pojemnik_.push(1);
            wielkosc_tablicy = pojemnik_.length;
            var tab = new Array();
            for (var i = 0; i < wielkosc_tablicy; i++) {
                tab[i] = [];
            }
            var a=0;
            for (var i = 0; i < wielkosc_tablicy; i++) {
                for (var j = 0; j < pojemnik_[i]; j++) {                                
                    tab[i][j] = pojemnik_inputow[a++];                                     
                }
            }
            liczba_przyciskow = pojemnik_inputow.length;
          //  alert(len+" "+aaa+" "+eee+"  "+liczba_przyciskow);
            kontener_inputow = tab;
           // alert(kontener_inputow);            
            if(gdzie_jestem=="/php/zdjecie.php")
            {
                //alert(kontener_inputow);
                var j;
                for(var i=0;i<len;i++)
                {
                    j = kontener_inputow[i].indexOf("popd_0"); 
                    if(j==0)
                    {
                        kontener_inputow.splice(i+4,100);//+1
                        var b,c,d,e;
                        b = pojemnik_inputow.indexOf("popd_");                
                        c = pojemnik_inputow.indexOf("odtw_g");                
                        d = pojemnik_inputow.indexOf("obraz_");                
                        e = pojemnik_inputow.indexOf("nast_");
                        var t=1;
                        if(document.getElementById('przycisk07') != null) 
                        {
                            t=0;
                        }
                        kontener_inputow[i]   = [document.getElementsByTagName("INPUT")[b-t].id],
                        kontener_inputow[i+1] = [document.getElementsByTagName("INPUT")[c-t].id];
                        kontener_inputow[i+2] = [document.getElementsByTagName("INPUT")[d-t].id];
                        kontener_inputow[i+3] = [document.getElementsByTagName("INPUT")[e-t].id];
                        break;
                    }
                }//alert(kontener_inputow);alert(pojemnik_inputow);
            }
            if(gdzie_jestem=="/php/profil.php")
            {                
                kontener_inputow.splice(3,4);
                kontener_inputow.push(["akcept"]);//alert(kontener_inputow);
            }  
            $("input").on({
                mouseenter: function(){//gdy się najecha myszką na
                    najechanie_myszka(this.id)
                },  
                mouseleave: function(){//gdy sie odjecha myszką z
                    odjechanie_myszka(this.id);
                }, 
                click: function(){
                    klikniecie_myszka(this.id);
                }  
            });
            //test_wyświetl();  
        }
       // alert(kontener_inputow);alert(pojemnik_inputow);
    }    
    /*
    if($("#lab_id").html()[0]=="W")
    {    
        kontener_inputow[kontener_inputow.length] = [];
        kontener_inputow[kontener_inputow.length-1][0] = "profil";
    }
    if($("#lab_id").html()[0]=="D")
    {
        kontener_inputow[kontener_inputow.length] = [];
        kontener_inputow[kontener_inputow.length-1][0] = "profil";
    }
*/
        function najechanie_myszka(idek)
        {
                podmien_kolory_inputow(2,8,3,true,9,idek);
                //$("#"+idek).focus();                
                nawigator = obecny(pojemnik_inputow,idek);//tu skończyłem
                //	popd_przycisk = document.getElementById(nawigator[0]);
                obec_przycisk = document.getElementById(nawigator);
                 //	nast_przycisk = document.getElementById(nawigator[2]);
                komunikat();
                podmien_kolory_inputow(4,7,1,false,9,idek);
        }
        function odjechanie_myszka(idek)
        {
                podmien_kolory_inputow(2,8,3,true,9,idek);
               // $("input").blur();		
                document.getElementById("obsluga").innerHTML = " ";
                podmien_kolory_inputow(2,8,3,false,9,idek);
                $(".przycisk-ikona-usun").css("background-color", kolory[9]);
        }
        function klikniecie_myszka(idek)
        {
                podmien_kolory_inputow(2,8,3,true,9,idek);
                nawigator = obecny(pojemnik_inputow,idek);//tu skończyłem
                    //	popd_przycisk = document.getElementById(nawigator[0]);
                obec_przycisk = document.getElementById(nawigator);
                    //	nast_przycisk = document.getElementById(nawigator[2]);
                komunikat();
                 //podmien_kolory_inputow(4,5,6,false);
                if($("#"+idek).attr('type')=="text" || $("#"+idek).attr('type')=="password")//przycisk
                {
                    $("#"+idek).css("background-color", kolory[4]);							
                }
                if($("#"+idek).attr('type')=="button" || $("#"+idek).attr('type')=="submit")//przycisk
                {
                    $("#"+idek).css("background-color", kolory[5]);
                    $(".przycisk-ikona-usun").css("background-color", kolory[10]);
                }
                if($("#"+idek).attr('class') == "obraz_kategori")//obrazek 
                {
                    $("#"+idek).css("background-color", kolory[6]);
                }   
                if($(this).attr('type')=="checkbox")//checkbox
                {
                    $(this).css("background-color", kolory[6]);							
                }
                if($("#"+idek).attr('class') == "plik-standard-przeslij" && !wybrano_plik)
                {
                    wybrano_plik=true;
                    $("#upload-file-container").css('background-image', 'url("/grafika/upload_b_2.gif")');
                }
        }
        
        function podmien_kolory_inputow(a,b,c,d,e,idek){            
            if(d)
            {
                $(".wiadomosc").css("background-color", kolory[a]);
                $("#tresc_wiadomości").css("background-color", kolory[a]);
                $("a").css("background-color", kolory[b]);
                $(".img").css("background-color", kolory[c]);
                $(".przycisk-ikona").css("background-color", kolory[b]);
                $(".przycisk-ikona-usun").css("background-color", kolory[e]);
                $(".przycisk-ikona-zmien").css("background-color", kolory[b]);
                $(".przycisk-dluzszy").css("background-color", kolory[b]);
		$(".text-standard").css("background-color", kolory[a]);
                $(".przycisk-standard").css("background-color", kolory[b]);	
                $(".obraz_kategori").css("background-color", kolory[c]);
                $(".checkbox-standard").css("background-color", kolory[c]);
                $(".plik-standard-przeslij").css("background-color", kolory[c]);
                $(".obszar_tekstowy-przeslij").css("background-color", kolory[a]);
                $(".text-standard-przeslij").css("background-color", kolory[a]);
                $(".text-standard-mini").css("background-color", kolory[a]);
                $(".data-standard-przeslij").css("background-color", kolory[a]);   
                if(!wybrano_plik)
                {
                    $("#upload-file-container").css('background-image', 'url("/grafika/upload_a_2.gif")');    
                }
                else
                {
                    $("#upload-file-container").css('background-image', 'url("/grafika/upload_b_2.gif")');   
                }
            }
            else
            {
		if($("#"+idek).attr('type')=="text" || $("#"+idek).attr('type')=="password")//pole tekstowe
		{
                    $("#"+idek).css("background-color", kolory[a]);
		}
		if($("#"+idek).attr('type')=="button" || $("#"+idek).attr('type')=="submit")//przycisk
		{
                    $("#"+idek).css("background-color", kolory[b]);
                    $(".przycisk-ikona-usun").css("background-color", kolory[10]);
		}                
		if($("#"+idek).attr('class') == "obraz_kategori")//obrazek 
		{
                    $("#"+idek).css("background-color", kolory[c]);
                }
                if($("#"+idek).attr('class') == "plik-standard-przeslij")//textarea 
		{
                    $("#"+idek).css("background-color", kolory[c]);
		}
		if($("#"+idek).attr('class') == "obszar_tekstowy-przeslij")//textarea 
		{
                    $("#"+idek).css("background-color", kolory[a]);
		}
		if($("#"+idek).attr('class') == "text-standardy-przeslij")//textarea 
		{
                    $("#"+idek).css("background-color", kolory[a]);
		}
                if($("#"+idek).attr('class') == "data-standard-przeslij")//textarea 
		{
                    $("#"+idek).css("background-color", kolory[a]);
		}  
                if($("#"+idek).attr('class') == "img")//textarea 
		{
                    $("#"+idek).css("background-color", kolory[c]);
		}  
                if($("#"+idek).attr('class') == "checkbox-standard")//textarea 
		{
                    $("#"+idek).css("background-color", kolory[c]);
		}  
            }
	}
//odtwórz     
function playSound(str) {
    if(glos_){
    $("#odtwarzacz").html("<embed id='odtwarzacz' src='/sound/"+str+".mp3' width='0' height='0' border='0' volume='1' > </embed >");   
    }
}
document.body.innerHTML += "<div id='odtwarzacz' ></div>"

function test_wyświetl(){
    for (var j = 0; j < wielkosc_tablicy; j++) {alert(kontener_inputow[j]);}
}
