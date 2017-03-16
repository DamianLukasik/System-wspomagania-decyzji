var div = document.createElement('div');
div.id = "ciastko";
var button = "<input id='akcept' type='button' onclick='zjedz()' class='przycisk-standard' value='Akceptuje'/>";
div.innerHTML = "<label style='font-size: 115%;' >Na niniejszym serwisie używamy cookies i podobnych technologii. Cookies będą zapisywane w pamięci Twojego urządzenia.</label>"+button;
div.className = 'cookies';
div.addEventListener('click', zjedz);        
document.body.appendChild(div);

var str = document.cookie;
var n = str.search("akceptacja");//cookie są kłopotliwe
if(n==-1)//jeśli nie istnieje tworzymy cookie
{
    var m = str.substr(n+11,1);
    var data = new Date();
    data.setTime(data.getTime()+(365*24*60*1000));
    var expires = '; expires='+data.toGMTString();
    document.cookie = 'akceptacja=1' + expires + '; path=/';
}
else
{
    var m = str.substr(n+11,1);
    if(m!="1")
    {
        div.style.visibility = 'hidden';
        div.style.height = '0px';  
    }
}
function zjedz()
    {        
        var str = document.cookie;
        var n = str.search("akceptacja");//cookie są kłopotliwe
        var m = str.substr(n+11,1);
        if(m=="1")
        {
            var ciastko = document.getElementById("ciastko");
            ciastko.style.visibility = 'hidden';
            ciastko.style.height = '0px';  
            var data = new Date();
            data.setTime(data.getTime()+(365*24*60*1000));
            var expires = '; expires='+data.toGMTString();
            document.cookie = 'akceptacja=0' + expires + '; path=/';
			wielkosc_tablicy--;
			nastepny_input(kontener_inputow);
        }
    }
    
document.cookie = 'resolution=' + screen.width + 'x' + screen.height;
