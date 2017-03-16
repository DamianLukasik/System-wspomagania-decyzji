<%@ Page Language="C#" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML><HEAD><TITLE>Crystal</TITLE>
<META http-equiv=Content-Type content="text/html; charset=iso-8859-2">
<META http-equiv=Refresh content="15; url=http://hostedwindows.pl">

<STYLE type=text/css>P {
	FONT-SIZE: 9pt; FONT-FAMILY: Verdana, Tahoma, Arial, sans-serif
}
TD {
	FONT-SIZE: 9pt; FONT-FAMILY: Verdana, Tahoma, Arial, sans-serif
}
A {
	TEXT-DECORATION: none
}
A:hover {
	COLOR: #b007ab; TEXT-DECORATION: underline
}
A.link {
	COLOR: #002300
}
</STYLE>

</HEAD>

<script runat="server" language="C#">

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Random r = new Random(System.DateTime.Now.Millisecond);
       int numrow = r.Next(1, 150);
       int numcol = r.Next(1, 150);
       int rowCnt;
       int rowCtr;
       int cellCtr;
       int cellCount;
      
       rowCnt = numrow;
       cellCount = numcol;
        for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
        {
            HtmlTableRow tRow = new HtmlTableRow();
            Table2.Rows.Add(tRow);
            for (cellCtr = 1; cellCtr <= cellCount; cellCtr++)
            {
                HtmlTableCell tCell = new HtmlTableCell();
                tRow.Cells.Add(tCell);
            }
        }
  
    }
    
</script>
<BODY text=#333333 vLink=##0067ab link=#0067ab bgColor=#ff00ff scroll=no>
<TABLE id="Table1" runat="server" height="100%" cellSpacing=0 cellPadding=0 width="100%" border=0 align="center">
  
  <TBODY>
  
  <TR>
    <TD align=center >
        <br />
        <A href="http://hostedwindows.pl/"><IMG alt="" 
      src="http://hostedwindows.pl/static/hostedwindows/image/logo.png" border=0></A><BR><BR><BR>Strona jest zarejestrowana w serwisie <A class=link href="http://hostedwindows.pl/"><B><SPAN style="COLOR: #0067ab">HostedWindows.pl</SPAN></B></A><BR>oferuj&#261;cym profesjonalny hosting stron i aplikacji internetowych<BR>wykorzystuj&#261;cych technologie serwerowe firmy Microsoft: IIS, ASP.NET oraz SQL Server.<BR><BR>Zach&#281;camy do zapoznania si&#281; z <A class=link href="http://hostedexchange.pl/"><B><SPAN style="COLOR: #0067ab">HostedExchange.pl</SPAN></B></A> - profesjonaln&#261; poczt&#261; dla Twojego biznesu. 

      <P><FONT size=1>Copyright 1995-2016 <A class=link 
      href="http://www.dcs.pl/">dcs.pl Sp. z o.o.</A> <A class=link 
      href="http://www.dcs.pl/copyright.asp">Wszelkie prawa zastrze&#380;one.</A><BR><BR><A class=link href="http://hostedwindows.pl/"><B><SPAN style="COLOR: #0067ab">HostedWindows.pl</SPAN></B></A> | <A class=link href="http://hostedexchange.pl/"><B><SPAN style="COLOR: #0067ab">HostedExchange.pl</SPAN></B></A>
<BR></FONT> </P></TD></TR></TBODY>
	  
	  
	  
	  </TABLE>
    

 <table id="Table2" border=0
             runat="server" visible=true>


      </table>
  
    
	  </BODY></HTML>