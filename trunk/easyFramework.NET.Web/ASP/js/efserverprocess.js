//Promain Software-Betreuung GmbH, 2004


function gsXmlString(sString) {
	//makes a string safe to be an xml-data-string:
	
	var s = new String(sString);
	
	s = s.replace("&", "&amp;");
	
	return s;
}


// Zum auslesen eines Tags per Javascript anstatt document.all.item(“tag“)

function getObj(name) 

{              

                var NS  = (document.layers) ? 1:0;

                var IE  = (document.all) ? 1:0;

                var DOM = (document.getElementById) ? 1:0;

                if (IE && document.all[name])  

                {

                               return document.all.item(name);

                } 

                if (DOM && document.getElementById(eval("'"+name+"'"))) 

                {

                               return document.getElementById(name);

                }  

                if (NS && document.layers[name]) 

                {

                               return document.layers[name];

                }

                return 0;

}

// Allgemeine gsCallServerMethod gibt String zurück

function gsCallServerMethod(sURL,sParams,lFlags)
{

	//calls the given url and retrieves data; should be datapage or something like that
	
	var sXml = '<Call><Params><ClientID>' + 
	msClientID + 
	'</ClientID>' + sParams + '</Params></Call>';

	if (sURL.search(/\?/i)==-1)
		sURL = sURL + '?ClientID=' + msClientID;
	else
		sURL = sURL + '&ClientID=' + msClientID;
	
	
	var IE  = (document.all) ? 1:0;

    var oSMthHttp;

    var sResponse;   

    
    try

        {

                        oSMthHttp = new ActiveXObject("Microsoft.XMLHTTP");

        }

        catch (e)

        {

                        oSMthHttp = new XMLHttpRequest();

        }

        
		if (sParams != "" && sParams != null && sParams!="undefined")

        {

                        oSMthHttp.open("POST",sURL,false);

                        oSMthHttp.send(sXml);

        }

        else

        {		
        
        
        		try {
                        oSMthHttp.open("GET",	sURL,	false);
                }
                catch (e) {
		
					alert(e);
                }
			
                oSMthHttp.send(null);

        }              
	
	        

    sResponse= oSMthHttp.responseText;
	
		
    oSMthHttp = null;

	
	
	return(sResponse);

}