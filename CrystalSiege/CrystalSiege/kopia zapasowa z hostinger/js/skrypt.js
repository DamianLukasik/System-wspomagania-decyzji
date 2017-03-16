var siła_hasła_kolory = new Array("red","orange","yellow","greenyellow","green","#4682B4");
var siła_hasła_tekst = new Array("banalne","słabe","średnie","mocne","bezpieczne","niedozłamania");
var prawidłowe_hasło = false;
var powtórzone_hasło = true;
var prawidłowy_email = false;
var prawidłowy_nick  = false;
var zaakceptowano_regulamin  = false;

function wprowadz_hasło() {
    var a = document.getElementById("pole1").value;
    var siła_hasła = 0;
    if(a.length>=8){ siła_hasła++; } 
    if(a.length>=20){ siła_hasła++; }
    var wzor = /\d+/
    if (wzor.test(a)) { siła_hasła++; }
    var wzor = /[a-z]+/
    if (wzor.test(a)) { siła_hasła++; }
    var wzor = /[A-Z]+/
    if (wzor.test(a)) { siła_hasła++; }
    var wzor = /[\~\`\!\@\#\$\%\^\&\*\(\)\_\+\|\\\=\-\[\]\}\{\'\;\:\"\?\.\,\<\>]+/
    if (wzor.test(a)) { siła_hasła++; }
    
    if (siła_hasła <= 0) { siła_hasła = 0 };
   
    for (var i = 1; i < 6; i++){
        document.getElementById("siła" + i).style.backgroundColor = null; 
        document.getElementById("hasło").innerHTML = "";  
    }

    if (siła_hasła >= 0){
        for (var i = 0; i<siła_hasła; i++){
            document.getElementById("siła" + (i + 1)).style.backgroundColor = siła_hasła_kolory[siła_hasła-1];
            document.getElementById("hasło").innerHTML = siła_hasła_tekst[siła_hasła-1];
        }
    }
    porównaj_hasło();
}

function porównaj_hasło() {
    var a = document.getElementById("pole1").value;
    var b = document.getElementById("pole2").value;
    poinformuj_o_braku("1", "#F5F9FB", "Z");
    poinformuj_o_braku("2", "#F5F9FB", "Z"); 
    if(a.length < 8 || a.length > 20){
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

function poinformuj_o_braku(i, kolor, grafika) {
    if (kolor != null)
    {
        document.getElementById("pole" + i).style.backgroundColor = kolor;
    }
    zmień_grafikę(i, grafika);
}

function zmień_grafikę(i,grafika) {
    document.getElementById("znak"+i).setAttribute("src", "/grafika/"+grafika+".gif");
}

function weryfikacja_maila() {
    var m = document.getElementById("pole4").value;
    var wzorMaila = /^[0-9a-zA-Z_.-]+@[0-9a-zA-Z.-]+\.[a-zA-Z]{2,3}$/
    poinformuj_o_braku("4", "#F5F9FB","Z");
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
    poinformuj_o_braku("0", "#F5F9FB","Z");
    if (m.length < 3)
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

function zaakceptuj_regulamin() {
    poinformuj_o_braku("6", null,"Z");
    if(document.getElementById("pole6").checked)
    {
        zaakceptowano_regulamin = true;
        zmień_grafikę("6", "V");
    }
    else
    {
        zaakceptowano_regulamin = false;
        zmień_grafikę("6", "Z");
    }
}