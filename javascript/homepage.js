var sSelected;

function FloatItem(sName) {

	document.getElementById('div'+sSelected).style.display='none';
	document.getElementById('div'+sName).style.display='block';		
}

function HideItem(sName) {
	document.getElementById('div'+sName).style.display='none';
	document.getElementById('div'+sSelected).style.display='block';
}

function SelectItem(sName) {
	document.getElementById('div'+sSelected).style.display='none';
	document.getElementById('div'+sName).style.display='block';
	
	document.getElementById('li'+sSelected).className='';
	document.getElementById('li'+sName).className='selected';
	sSelected=sName;
	
	oStoreMenuSection.Go(sName);
}


var oStoreMenuSection=new WebService();
oStoreMenuSection.Go=function(AccessRightGroupID) { 
	aParams=new Array(['AccessRightGroupID',AccessRightGroupID]);
	this.RunWebService('webservices/menu.asmx','http://intuitivesystems', 'StoreMenuSection', aParams, this, false);
}

oStoreMenuSection.Done=function(oXML) {
	var oReturn=this.GetTagValue(oXML, 'StoreMenuSectionResult');
}
