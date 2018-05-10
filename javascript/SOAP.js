//soap stuff
var oRequest;

function SOAP() {

	this.Send=function(sWebServiceURL, sNamespace, sFunction, aParameters) {
		
		var sRequest=
			'<?xml version="1.0" encoding="utf-8"?>'+
			'<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" '+
			'xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
			'<soap:Body>'+
			'	<'+sFunction+' xmlns="'+sNamespace+'">'

		for (var i=0;i<aParameters.length;i++) {
			sRequest=sRequest+'<'+aParameters[i][0]+'>'+
				aParameters[i][1]+'</'+aParameters[i][0]+'>';
		}
		
		sRequest=sRequest+'	</'+sFunction+'>'+
			'</soap:Body>'+
			'</soap:Envelope>';

		
		this.SendXMLHTTPRequest(sWebServiceURL, sNamespace, sFunction, sRequest);
		
	}

	this.ReceiveSOAPXMLHTTPRequest= function() {
		
		if (oRequest.readyState == 4) {
			if (oRequest.status == 200) {
				ProcessResponse(oRequest.responseXML);
			} else {
				alert("There was a problem retrieving the XML data:\n" +oRequest.statusText);
			}
		}
	}
	
	
	this.SendXMLHTTPRequest=function (sUrl, sNamespace, sFunction, sSOAP) {
		
		// branch for native XMLHttpRequest object
		if (window.XMLHttpRequest) {
			netscape.security.PrivilegeManager.enablePrivilege("UniversalPreferencesRead");
			oRequest = new XMLHttpRequest();
			oRequest.onreadystatechange = this.ReceiveSOAPXMLHTTPRequest;
			oRequest.open("POST", sUrl, true);
			oRequest.setRequestHeader("Content-Type", "text/xml")
			oRequest.setRequestHeader("MessageType", "CALL")
			oRequest.setRequestHeader('SOAPAction',sNamespace)
			oRequest.send(sSOAP);
		// branch for IE/Windows ActiveX version
		} else if (window.ActiveXObject) {
			//isIE = true;
			oRequest = new ActiveXObject("Microsoft.XMLHTTP");
			if (oRequest) {
				oRequest.onreadystatechange = this.ReceiveSOAPXMLHTTPRequest;			
				oRequest.open("POST", sUrl, true);
				oRequest.setRequestHeader("Content-Type", "text/xml")
				oRequest.setRequestHeader("MessageType", "CALL")
				oRequest.setRequestHeader('SOAPAction',sNamespace+'/'+sFunction)
				oRequest.send(sSOAP);
			}
		}
	}
}


function GetTagValue(oLocalXML, sTag) {

	
	var aItems=oLocalXML.getElementsByTagName(sTag);
	if (aItems.length==1 && aItems[0].childNodes[0]) {
		return aItems[0].childNodes[0].data;
	} else {
		return '';
	}
}
