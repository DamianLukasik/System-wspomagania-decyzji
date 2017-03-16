var prawidłowe_hasło = false;
var powtórzone_hasło = true;
var prawidłowy_email = false;
var prawidłowy_nick  = false;
var prawidłowy_kod   = false;
var zaakceptowano_regulamin  = false;
var wykryto_cyfry = false;
var wykryto_małe_litery = false;
var wykryto_duże_litery = false;
var wykryto_znaki_specjalne = false;
var wykryto_min_8_znakow = false;
var nie_jest_botem = false;
var siła_hasła_kolory;
if($("link[rel=stylesheet]").attr("href")=="/css/styl_2.css")
{
	siła_hasła_kolory = ["yellow","yellow","yellow","yellow","yellow"];
}
else
{
	siła_hasła_kolory = ["red","orange","yellow","greenyellow","green"];
}
var rodzaje_pól = new Array("dane","pole","znak");
var tryb_rejestracji = false;
var tryb_logowania   = false;
alert("nikt nie używa");
function twórz_kontrolkę(typ,nr,funkcja) {    
    var pole_ = document.getElementById("pole-"+nr);
    var input = document.createElement("input");
    input.setAttribute("type", typ);
    input.setAttribute("value", "");
    input.setAttribute("id", "pole"+nr);
    input.setAttribute("class", "text-standard");
    input.addEventListener("keyup", funkcja);
    pole_.appendChild(input);    
    twórz_label("label",nr,"","opis");    
}

function twórz_label(typ,nr,tekst,dod) {    
    var pole_ = document.getElementById("pole-"+nr);
    var input = document.createElement(typ);
    input.textContent = tekst;
    input.setAttribute("id", "pole"+nr+dod);
    input.setAttribute("class", "text-standard");
    pole_.appendChild(input);
}

function twórz_checkoxa(typ,nr,funkcja) {
    var pole_ = document.getElementById("pole-"+nr);
    var input = document.createElement("input");
    input.setAttribute("type", typ);
    input.setAttribute("value", "");
    input.setAttribute("id", "pole"+nr);
    input.setAttribute("class", "checkbox-standard");
    input.addEventListener("click", funkcja);
    pole_.appendChild(input);
    twórz_label("label",nr,"","opis");  
}

function twórz_przycisk(typ,nr,funkcja,tekst) {
    var pole_guzik = document.getElementById("pole-"+nr);
    var input = document.createElement("input");
    input.setAttribute("type", typ);
    input.setAttribute("value", tekst);
    input.setAttribute("id", "pole"+nr);
    input.setAttribute("class", "przycisk-standard");
    input.addEventListener("click", funkcja);
    pole_guzik.appendChild(input);     
}

function zaloguj() {
    tryb_logowania = !tryb_logowania;
    if(tryb_logowania)
    {
        document.getElementById("pole-9").style.visibility = "visible";
		document.getElementById("obsluga").innerHTML = "X panel logowania do systemu";
    }
    else
    {
        document.getElementById("pole-9").style.visibility = "hidden";
		document.getElementById("obsluga").innerHTML = "A panel logowania do systemu";
    }    
}

function zarejestruj() {
    tryb_rejestracji = !tryb_rejestracji;   
    if(tryb_rejestracji)
    {
        PokażPanelRejestracji();
    }
    else
    {
        UkryjPanelRejestracji();
    }     
}

function UkryjPanelRejestracji() {
    odhacz_warunki();
    wyczyść();
    for (var i = 1; i < 6; i++){
        document.getElementById("siła" + i).style.backgroundColor = null;
    }
    document.getElementById("zakladanie_konta").disabled = false;   
}

function PokażPanelRejestracji() {  

    var pola_tekst = new Array("", "", "",
        "",
        "", "", "", "");
    var ilosc = pola_tekst.length;
    
    for (i = 0 ; i < ilosc ; i++) {
        if (i != 5)
        {
            var pole_;
            for (j = 2 ; j >= 0 ; j--) {
                pole_ = document.getElementById(rodzaje_pól[j]+"-" + i);
                pole_.style.visibility = "visible";                
            }
            pole_.innerText = pola_tekst[i];
        }
    }
    
    document.getElementById("pole-5").style.visibility = "visible";
    var pole_ = document.getElementById("pole-8");
    pole_.style.visibility = "visible";
    
    twórz_kontrolkę("text","0",weryfikacja_nick);
    twórz_kontrolkę("password","1",wprowadz_hasło);
    twórz_kontrolkę("password","2",porównaj_hasło);       
    twórz_label("label","3","Siła hasła","");  
    twórz_kontrolkę("text","4",weryfikacja_maila);
    
    //Captcha
    var pole_kod = document.getElementById("pole-5");    
    $("#captcha").wygeneruj_obrazek();  
    var input = document.createElement("img");
    input.setAttribute("id", "kod_z_obrazka");
    pole_kod.appendChild(input);
    var input = document.createElement("input");
    input.setAttribute("type", "button");
    input.setAttribute("value", "Odśwież");
    input.setAttribute("id", "captcha_odśwież");
    input.setAttribute("class", "przycisk-standard");    
    input.addEventListener("click", $("#captcha_odśwież").odswiez); 
    pole_kod.appendChild(input);

    twórz_kontrolkę("text","6",weryfikacja_kodu);   
    twórz_checkoxa("checkbox","7",zaakceptuj_regulamin);
    twórz_przycisk("button","8",załóż_konto,"Załóż konto"); 
}

function wprowadz_hasło() {
    var a = document.getElementById("pole1").value + "";
    var siła_hasła = 0;
    if(a.length>=8){ wykryto_min_8_znakow = true; }else{ wykryto_min_8_znakow = false; }
    var wzor = /\d+/
    if (wzor.test(a)) { wykryto_cyfry = true; } else { wykryto_cyfry = false; }
    var wzor = /[a-z]+/
    if (wzor.test(a)) { wykryto_małe_litery = true; } else { wykryto_małe_litery = false; }
    var wzor = /[A-Z]+/
    if (wzor.test(a)) { wykryto_duże_litery = true; } else { wykryto_duże_litery = false; }
    var wzor = /[\~\`\!\@\#\$\%\^\&\*\(\)\_\+\|\\\=\-\[\]\}\{\'\;\:\"\?\.\,\<\>]+/
    if (wzor.test(a)) { wykryto_znaki_specjalne = true; } else { wykryto_znaki_specjalne = false; }

    if (wykryto_min_8_znakow){siła_hasła++;}
    if (wykryto_znaki_specjalne) { siła_hasła++; }
    if (wykryto_duże_litery) { siła_hasła++; }
    if (wykryto_małe_litery) { siła_hasła++; }
    if (wykryto_cyfry) { siła_hasła++; }
    if (siła_hasła <= 0) { siła_hasła = 0 };
   
    for (var i = 1; i < 6; i++){
        document.getElementById("siła" + i).style.backgroundColor = null;
    }

    if (siła_hasła >= 0){
        for (var i = 0; i<siła_hasła; i++){
            document.getElementById("siła" + (i + 1)).style.backgroundColor = siła_hasła_kolory[siła_hasła-1];
        }
    }
    porównaj_hasło();
}

function porównaj_hasło() {
    var a = document.getElementById("pole1").value;
    var b = document.getElementById("pole2").value;
    poinformuj_o_braku("1", "", "#F5F9FB", "Z");
    poinformuj_o_braku("2", "", "#F5F9FB", "Z"); 
    if(a.length == 0){
        prawidłowe_hasło = false;  
        powtórzone_hasło = true; 
        zmień_grafikę("1", "X");
    }
    else{
        prawidłowe_hasło = true;
        zmień_grafikę("1", "V");   
        if (a != b){
            powtórzone_hasło = false;
            zmień_grafikę("2", "X");
        }
        else{
            powtórzone_hasło = true;                
            zmień_grafikę("2", "V");
        }
    } 
    wyczyść_pola_haseł(a,b);
}

function wyczyść_pola_haseł(a,b) {
    if (a.length == 0 || b.length == 0)
    {
        zmień_grafikę("1", "Z");
        zmień_grafikę("2", "Z");
    }
}

function zmień_grafikę(i,grafika) {
    document.getElementById("znak"+i).setAttribute("src", "grafika/"+grafika+".gif");
}

//skrypty są niepodobne do modelu UML

function weryfikacja_maila() {
    var m = document.getElementById("pole4").value;
    var wzorMaila = /^[0-9a-zA-Z_.-]+@[0-9a-zA-Z.-]+\.[a-zA-Z]{2,3}$/
    poinformuj_o_braku("4", "", "#F5F9FB","Z");
    if (m.length == 0)
    {
        prawidłowy_email = false;
        zmień_grafikę("4", "Z");
    }
    else
    {
        if (wzorMaila.test(m)) {
            prawidłowy_email = true;
            zmień_grafikę("4", "V");
        }
        else {
            prawidłowy_email = false;
            zmień_grafikę("4", "X");
        }
    }
}

function weryfikacja_nick() {
    var m = document.getElementById("pole0").value;
    poinformuj_o_braku("0", "", "#F5F9FB","Z");
    if (m.length == 0)
    {
        prawidłowy_nick = false;
        zmień_grafikę("0", "Z");
    }
    else
    {
        prawidłowy_nick = true;
        zmień_grafikę("0", "V");
    }
}

function weryfikacja_kodu(){
    var k = document.getElementById("pole6").value;
    poinformuj_o_braku("6", "", "#F5F9FB", "Z");
    if(k == $.fn.zwróć_kod()){
        prawidłowy_kod = true;
        zmień_grafikę("6", "V");
    }    
    else   
    {
        prawidłowy_kod = false;
        zmień_grafikę("6", "X");
    }
}

function zaakceptuj_regulamin() {
    poinformuj_o_braku("7", "", null,"Z");
    if(document.getElementById("pole7").checked)
    {
        zaakceptowano_regulamin = true;
        zmień_grafikę("7", "V");
    }
    else
    {
        zaakceptowano_regulamin = false;
        zmień_grafikę("7", "Z");
    }
}

function załóż_konto() {
    weryfikacja_kodu();
    var nick = document.getElementById("pole0").value;
    var pass = document.getElementById("pole2").value;
    var mail = document.getElementById("pole4").value;
    if (prawidłowy_nick && prawidłowe_hasło && powtórzone_hasło && 
        prawidłowy_email && prawidłowy_kod && zaakceptowano_regulamin) {
        //alert("ok"+nick+" "+pass+" "+mail);
        dodaj_użytkownika(nick,pass,mail);
   //     wyślij_wiadomość_weryfikacyjną(nick,pass,mail);//nie działa
    }
    else
    {
        if (!prawidłowy_nick)
        {
            poinformuj_o_braku("0","nie podałeś nicku","#F1C6C7","X");                      
        }
        if (!prawidłowe_hasło) {
            poinformuj_o_braku("1", "nie podałeś hasła", "#F1C6C7","X");
        }
        if (!powtórzone_hasło) {
            poinformuj_o_braku("2", "hasła nie są takie same", "#F1C6C7","X");
        }
        if (!prawidłowy_email) {
            poinformuj_o_braku("4", "nie podałeś adresu", "#F1C6C7","X");
        }
        if (!prawidłowy_kod) {
            poinformuj_o_braku("6", "zły kod", "#F1C6C7","X");
            $("#captcha_odśwież").odswiez();
        }
        if (!zaakceptowano_regulamin) {
            poinformuj_o_braku("7", "nie zaakceptowałeś", null,"X");
        }
    }
}

function poinformuj_o_braku(i, tekst, kolor, grafika) {
    if (kolor != null)
    {
        document.getElementById("pole" + i).style.backgroundColor = kolor;
    }
    document.getElementById("pole" + i + "opis").textContent = tekst;
    zmień_grafikę(i, grafika);
}

function dodaj_użytkownika(nick, pass, mail) {
    var http = new XMLHttpRequest();
    var url = "php/dodanie_uzytkownika.php";
    var params = "nick="+nick+"&pass="+pass+"&mail="+mail+"";
    http.open("POST", url, true);
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

    http.onreadystatechange = function() {
        if(http.readyState == 4 && http.status == 200) {
            //alert(http.responseText);
            tryb_rejestracji = !tryb_rejestracji;
            UkryjPanelRejestracji();         
        }
    }
    http.send(params);
}

function odhacz_warunki() {
    prawidłowy_nick=false;
    poinformuj_o_braku("0", "", "#F5F9FB","Z");
    prawidłowe_hasło=false;
    poinformuj_o_braku("1", "", "#F5F9FB","Z");
    poinformuj_o_braku("2", "", "#F5F9FB","Z");
    prawidłowy_email=false;
    poinformuj_o_braku("4", "", "#F5F9FB","Z");
    prawidłowy_kod=false;
    poinformuj_o_braku("6", "", "#F5F9FB","Z");
    zaakceptowano_regulamin=false;
    poinformuj_o_braku("7", "", null,"Z");
}

function wyczyść() { 
    for(var j=0;j<9;j++)
    {       
        if(j==5 || j==8)
        {
            document.getElementById(rodzaje_pól[1]+"-"+j).style.visibility = "hidden";
            if(j==5)
            {
                usuń_element("captcha_odśwież");
            }
            else
            {
                usuń_element("pole8");
            }            
        }
        else
        { 
            for(var i=0;i<3;i++)
            {
                document.getElementById(rodzaje_pól[i]+"-"+j).style.visibility = "hidden";      
            } 
            if(j!=3)
            {
                usuń_element("pole"+j);          
                usuń_element("pole"+j+"opis");
            }
            else
            {
                usuń_element("pole"+j);
            }
        }         
    }
}

function usuń_element(tekst) {
    var doUsuniecia = document.getElementById(tekst);
    doUsuniecia.parentNode.removeChild(doUsuniecia);
}

function wyślij_wiadomość_weryfikacyjną(nick, pass, mail) {
    var http = new XMLHttpRequest();
    var url = "php/wyslij_mail.php";
    var params = "nick="+nick+"&pass="+pass+"&mail="+mail+"";
    http.open("POST", url, true);
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

    http.onreadystatechange = function() {
        if(http.readyState == 4 && http.status == 200) {
            alert("wysłano "+http.responseText);                    
        }
    }
    http.send(params);      
}