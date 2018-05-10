	var iOpacity=100;
	var iOpacity=100;
	var IE4 = (document.all);
	var NS4 = (document.layers);
	var dLastGridDate;

	function GetDateFromWeek(iWeek) {
	
		var iFirstDayOfYear=new Date(new Date().getYear(),0,1).getDay();
		var iFirstWeekEnding;
		if (iFirstDayOfYear<5) {
			iFirstWeekEnding=8-iFirstDayOfYear;
		} else {
			iFirstWeekEnding=15-iFirstDayOfYear;
		}
		var dDate=new Date(new Date().getYear(),0,(iWeek-1)*7+iFirstWeekEnding-2);
		
		return GetDisplayDate(dDate);
	}

	function GetProductionDateFromWeek(iWeek) {
	
		var iFirstDayOfYear=new Date(new Date().getYear(),0,1).getDay();
		var iFirstWeekEnding;
		if (iFirstDayOfYear<5) {
			iFirstWeekEnding=8-iFirstDayOfYear;
		} else {
			iFirstWeekEnding=15-iFirstDayOfYear;
		}		
		var dDate=new Date(new Date().getYear(),0,(iWeek-1)*7+iFirstWeekEnding-3);
		
		return GetDisplayDate(dDate);
	}	
	
	function ButtonPostBack(oButton) {      
        oButton.disabled=true;
        Postback(oButton.id,'');
    }
	
	function Postback(sCommand, sArgument, bCheckDelete) {

		//confirm delete if required	
		if (bCheckDelete==true) {
			if (confirm('Are you sure that you want to delete this record?')==false) {
				return;
			}
		}
		
		if (typeof(oWYSIWYG)!='undefined') {
			TidyXHTML();
			
		}
		
		document.forms[0].Command.value=sCommand;
		document.forms[0].Argument.value=sArgument;
		document.forms[0].submit();
	}

	function ShowPopup(sURL, iHeight, iWidth) {
		var iTop=(screen.availHeight-iHeight)/2;
		var iLeft=(screen.availWidth-iWidth)/2;
		
		var sFeatures='scrollbars=yes, menubar=no, resizable=yes, status=no,' + 
					'titlebar=no, toolbar=no, height=' + iHeight  + 
					', width=' + iWidth + ',top=' + iTop + ', Left=' + iLeft;
		
		window.open(sURL,'_blank',sFeatures,true);
	}
	
	
	function ShowDocument(sURL, iHeight, iWidth, sCommand, sArgument) {

		var iTop=(screen.availHeight-iHeight)/2;
		var iLeft=(screen.availWidth-iWidth)/2;
		
		var sFeatures='scrollbars=no, menubar=yes, resizable=yes, status=no,' + 
					'titlebar=no, toolbar=no, height=' + iHeight  + 
					', width=' + iWidth + ',top=' + iTop + ', Left=' + iLeft;
		
		sURL=sURL + '?Command=' + sCommand + '&Argument=' + sArgument;	
		window.open(sURL,'_blank',sFeatures,true);
	}
	
	function ShowPDF(sDocumentViewerURL, sDocumentURL) {
		var iTop=(screen.availHeight-600)/2;
		var iLeft=(screen.availWidth-850)/2;
	
		
		var sFeatures='scrollbars=no, menubar=yes, resizable=yes, status=no,' + 
					'titlebar=no, toolbar=no, height=600, width=850,top=' 
					+ iTop + ', Left=' + iLeft;
		var sURL=sDocumentViewerURL+'?exacturl='+sDocumentURL;
		window.open(sURL,'_blank',sFeatures,true)		
	}
	
	
	function ShowCalendar(sURL, sDateField, sJavascriptFunction, bFutureDate) {
		var iHeight=191;
		var iWidth=212;
		var iTop=(screen.availHeight-iHeight)/2;
		var iLeft=(screen.availWidth-iWidth)/2;
		var sDate;
		
		//set popup window features
		var sFeatures='scrollbars=no, menubar=no, resizable=no, status=no,' + 
					'titlebar=no, toolbar=no, height=' + iHeight  + 
					', width=' + iWidth + ',top=' + iTop + ', Left=' + iLeft;
		
		//retrive the date from the date field - may have containerid appended so check
		if(document.getElementById(sDateField + ':txtDate')){
			sDate=document.getElementById(sDateField + ':txtDate').value;			
		}else if(document.getElementById(sDateField + '_txtDate')){
			sDate=document.getElementById(sDateField + '_txtDate').value;			
		}else{sDate=document.getElementById(sDateField).value;}
		
		//create the url
		sURL=sURL + '?Argument=' + sDateField + '&StartDate=' + sDate;		
		
		//add the function if needed
		if (sJavascriptFunction!=undefined) {
			sURL=sURL+'&JavascriptFunction='+sJavascriptFunction;
		}
		
		sURL=sURL+'&FutureDate='+bFutureDate;

		//tada
		window.open(sURL,'_blank',sFeatures,true);
	}
	
	function ShowHelp (sHelpURL) {
	
		var sURL = sHelpURL + '?page=' + location.pathname
	
		var iTop=(screen.availHeight-500)/2;
		var iLeft=(screen.availWidth-600)/2;
		
		var sFeatures='scrollbars=yes, menubar=no, resizable=yes, status=no,' + 
					'titlebar=no, toolbar=no, height=500, width=600,top=' 
					+ iTop + ', Left=' + iLeft;
		
		window.open(sURL,'_blank',sFeatures,true);

	}
	
	function GetDisplayDate(dDate) {
		dDate=new Date(dDate)
		
		var aMonths=new Array('Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec')
		
		var sDay=dDate.getDate().toString()
		if (sDay.length==1) {
			sDay='0'+sDay;
		}
		return sDay + ' ' +aMonths[dDate.getMonth()] + ' ' +dDate.getFullYear();
	}
	
	function ProperCasing(oTextbox) {
	
		//get the value, if it's not empty then do the bizzo
		var sValue=oTextbox.value;		
		if (sValue.length>0) {
		
			//replace double spaces
			var aValues=sValue.split(' ');
			var sValueCase;
			var sNewValue='';
			for (var iLoop=0;iLoop<=aValues.length-1;iLoop++) {
			
				sNewValue+=aValues[iLoop].substring(0,1).toUpperCase() + aValues[iLoop].substring(1,aValues[iLoop].length);
				if (iLoop<aValues.length) sNewValue+=' ';
			}
			
			oTextbox.value=sNewValue;
		}
	
	}
		
	function ParseDate(oTextBox,bFutureDate,sStartDateTextBox) {
		var sDate=oTextBox.value;	
		var now=new Date();
		var dDate;
		var sDay;
		var sMonth;
		var sYear;
		var aMaxDays=new Array(31,29,31,30,31,30,31,31,30,31,30,31);
		oUSDateFormat = document.getElementById("USDateFormat");
		
		//bomb out if no characters
		if (sDate.length==0) return;		
		
		//if the date is in the format wnn then 
		var weekformat = /^w([1-5][0-9]|[1-9])$/i;
		if (weekformat.test(sDate)) {
			sDate=GetDateFromWeek(sDate.substring(1));
			oTextBox.value=sDate;
			return;
		}
		
		//if the date is in the format wnn then 
		var prodweekformat = /^p([1-5][0-9]|[1-9])$/i;
		if (prodweekformat.test(sDate)) {
			sDate=GetProductionDateFromWeek(sDate.substring(1));
			oTextBox.value=sDate;
			return;
		}
		
		//today
		var todayformat=/^t$/i;
		if (todayformat.test(sDate)) {
			var dNewDate=new Date();
			oTextBox.value=GetDisplayDate(dNewDate);
			return;
		}
		
		//tomorrow
		var tomorrowformat=/^tm$/i;
		if (tomorrowformat.test(sDate)) {
			var dNewDate=new Date(new Date().getTime()+86400000);
			oTextBox.value=GetDisplayDate(dNewDate);
			return;
		}
		
		//next week
		var nextweekformat=/^nw$/i;
		if (nextweekformat.test(sDate)) {
			var dNewDate=new Date(new Date().getTime()+7*86400000);
			oTextBox.value=GetDisplayDate(dNewDate);
			return;
		}
		
		//last week
		var lastweekformat=/^lw$/i;
		if (lastweekformat.test(sDate)) {
			var dNewDate=new Date(new Date().getTime()-7*86400000);
			oTextBox.value=GetDisplayDate(dNewDate);
			return;
		}
		
		//check for relative date
		var relativeformat=/^[\-|+]\d+$/i;
		if (relativeformat.test(sDate)) {
			var iOffset=parseInt(sDate);
			var dNewDate=new Date(new Date().getTime()+iOffset*86400000);
			oTextBox.value=GetDisplayDate(dNewDate);
			return;
		}

		//strip out separators
		sDate=Replace(sDate,'.','');
		sDate=Replace(sDate,'/','');
		sDate=Replace(sDate,' ','');
		sDate=Replace(sDate,'-','');
		
		//check for text month functionality
		sDate=sDate.toLowerCase();
		var textdate1=/^(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)(19|20)?(\d\d)?$/;
		var textdate2=/^\d(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)(19|20)?(\d\d)?$/;
		var textdate3=/^[0-3]\d(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)(19|20)?(\d\d)?$/;
		var bTextDate=false;
		if (textdate1.test(sDate)) {
			sDate='01'+sDate;
			bTextDate=true;
		} else if (textdate2.test(sDate)) {
			sDate='0'+sDate;
			bTextDate=true;
		} else if (textdate3.test(sDate)) {
			bTextDate=true;
		}
		
		if (bTextDate==true) {
			sDate=sDate.replace('jan','01').replace('feb','02').replace('mar','03').replace('apr','04').replace('may','05').replace('jun','06').replace('jul','07').replace('aug','08').replace('sep','09').replace('oct','10').replace('nov','11').replace('dec','12')
			
			//switch for US Date format if necessary
			if (oUSDateFormat.value=='true'){			
				sDate=sDate.substr(2,2)+sDate.substr(0,2)+sDate.substr(4,4);
			}
		}
		
			
		
		//make sure only numbers, if not leave values bomb out
		var nonDigit = /\D/g;
		if (nonDigit.test(sDate)) return;
		
		//do the bizzo
		if (sDate.length==4 || sDate.length==6 || sDate.length==8) {
		
			if (oUSDateFormat.value != 'true'){
				sDay=sDate.substr(0,2);
				sMonth=sDate.substr(2,2)-1;
			} else {
				sDay=sDate.substr(2,2);
				sMonth=sDate.substr(0,2)-1;
			}
			
			
			var sStartDate='';
			if (sDate.length==4 && sStartDateTextBox!=undefined && sStartDateTextBox!='') {
				sStartDate=document.getElementById(sStartDateTextBox).value;
			}
			
			if (sStartDate!='') {
				
				var iStartDay=parseInt(sStartDate.substring(0,3));
				var iStartMonth=parseInt(sStartDate.substring(3,6).replace('Jan','01').replace('Feb','02').replace('Mar','03').replace('Apr','04').replace('May','05').replace('Jun','06').replace('Jul','07').replace('Aug','08').replace('Sep','09').replace('Oct','10').replace('Nov','11').replace('Dec','12'));
				var iStartYear=parseInt(sStartDate.substring(7,12));
				if (iStartMonth>parseInt(sMonth) || (iStartMonth==parseInt(sMonth) && iStartDay>parseInt(sDay))) {
					sYear=iStartYear+1;
				} else {
					sYear=iStartYear;
				}			
					
				
			} else if (sDate.length==4 && bFutureDate=='True') {				
				
				//check, if date entered is before today then pick next year
				var dNow=new Date();
				var iNowDay=dNow.getDate();
				var iNowMonth=dNow.getMonth();
				var iNowYear=dNow.getFullYear();		
				if (iNowMonth>parseInt(sMonth) || (iNowMonth==parseInt(sMonth) && iNowDay>parseInt(sDay))) {
					sYear=iNowYear+1;
				} else {
					sYear=iNowYear;
				}								
			} else if (sDate.length==4) {
				oDefaultYear=document.getElementById('DefaultYear');
				sYear=oDefaultYear.value;
			} else if (sDate.length==6) {
				sYear=sDate.substr(4,2);
				if (sYear>50) {
					sYear='19'+sYear;
				} else {
					sYear='20'+sYear;
				}
			} else {
				sYear=sDate.substr(4,4);
			}

			//if invalid number of days bomb out
			if (sDay>aMaxDays[sMonth]) return;
			
			//create new date
			dDate=new Date(sYear, sMonth, sDay);
		} else return;
		
		oTextBox.value=GetDisplayDate(dDate);
				
		//now set the row below with the nxt date (if its a grid input)
		if (oTextBox.id.substring(0,1)=='c' && oTextBox.id.indexOf('_')>0){	
		
			if (Date.parse(dDate)<Date.parse(dLastGridDate)){
				SetNextRowDate(oTextBox,dDate);
			}
		}
	}
	
	function SetNextRowDate(oTextBox,dDate) {
		var sID =oTextBox.name;
		var iSplitIndex=sID.indexOf('_');
		var iLastSplitIndex=sID.lastIndexOf('_');
		
		var iRow=parseInt(sID.substring(1,iSplitIndex));
		var iCol=parseInt(sID.substring(iSplitIndex+1,iLastSplitIndex));
		var iHash=parseInt(sID.substring(iLastSplitIndex+1));
		
		//check if the grid is using a date range and textbox was the end date box
		//date box?
		if (oTextBox.className.indexOf('grddate') > -1 && iCol==1){
			
			//set the id for the previous datebox (startdate)
			sID='c' + iRow + '_' + parseInt(iCol-1)+'_'+iHash;	
			
			//get back the datebox (startdate)		
			var oStartBox=document.getElementById(sID);
	
			if (oStartBox!=null){				
				//previous textbox is a date too?
				if (oStartBox.className.indexOf('grddate') > -1) {
					sID='c' + parseInt(iRow+1) + '_' + parseInt(iCol-1)+'_'+iHash;	
					
					//get the start date box for the next row
					var oNextBox=document.getElementById(sID);
					
					//if we got the box then set its date
					if (oNextBox!=null){
						oNextBox.value=GetDisplayDate(dDate.setDate((dDate.getDate()+1)));
						sID='c' + parseInt(iRow+1) + '_' + parseInt(iCol)+'_'+iHash;
						document.getElementById(sID).focus();
					}
				}	
			}		
		}
	}
	
    
    function Replace(sString, sStringToReplace, sReplacement) {
		while (sString.indexOf(sStringToReplace) != -1) {
			sString=sString.replace(sStringToReplace, sReplacement);
		}
		return sString;
	}

	function HideSelects() {
		var o=document.getElementById("HideSelect")
		if (o==null) return;
		var aSelects=o.value.split('~')
		for (var i in aSelects) {
			var oSelect=document.getElementById(aSelects[i]);
			oSelect.style.visibility='hidden';
		}
	}

	/*
	function ShowSelects() {
		var o=document.getElementById("HideSelect")
		if (o==null) return;
		var aSelects=o.value.split('~')
		for (var i in aSelects) {
			var oSelect=document.getElementById(aSelects[i]);
			oSelect.style.visibility='visible';
		}
	}*/	
	
	function TextboxSubmit(oEvent, sCommand, sArgument) {
		if (oEvent.keyCode == 13) {
			document.forms[0].Command.value=sCommand;
			document.forms[0].Argument.value=sArgument;
			document.forms[0].submit();
			oEvent.cancelBubble = true;
			oEvent.returnValue = false;
			
		}
	}
	
	function TextboxOnEnter(oEvent, oFunction) {

		if (oEvent.keyCode==13) {
			oFunction(); 
			oEvent.cancelBubble = true;
			oEvent.returnValue = false; 
		}

	}

	function SetFocus(sControlID) {
		var oControl=window.document.getElementById(sControlID);
		if (oControl != null){oControl.focus()};
	}	

	function IntegerOnly(oEvent) {
		iKeyPress=iif(oEvent.keyCode,oEvent.keyCode, oEvent.which);
		return iKeyPress>47 && iKeyPress<58;
	}

	function IntegerMinusOnly(oEvent) {
		iKeyPress=iif(oEvent.keyCode,oEvent.keyCode, oEvent.which);
		return iKeyPress>47 && iKeyPress<58 || iKeyPress==45;
	}
	
	function NumberOnly(oEvent) {
		iKeyPress=iif(oEvent.keyCode,oEvent.keyCode, oEvent.which);
		return (iKeyPress>47 && iKeyPress<58) || (iKeyPress==46 || iKeyPress==45);
	}	
	
	function PostiveNumberOnly(oEvent) {
		iKeyPress=iif(oEvent.keyCode,oEvent.keyCode, oEvent.which);
		return (iKeyPress>47 && iKeyPress<58) || (iKeyPress==46);
	}
	

	
	function PreLoadImage(sImageSource) {
		oImage = new Image();
		oImage.src = sImageSource;
	}
	
	//Functions to set Css styles
	setStyle = function(obj, style, value) {
		getRef(obj).style[style] = value;
	}      
	getRef = function (obj){
		return (typeof obj == "string") ?
				document.getElementById(obj) : obj;
	}
	
	//calendar set opener page date
	function SetDate(sDate,sID){
		if (window.opener.document.getElementById(sID + ':txtDate')){
			window.opener.document.getElementById(sID+':txtDate').value=sDate;
		}else if (window.opener.document.getElementById(sID + '_txtDate')){
			window.opener.document.getElementById(sID + '_txtDate').value=sDate;
		}else if (window.opener.document.getElementById(sID)){
			window.opener.document.getElementById(sID).value=sDate;
		}
	}	

	function ToggleCheckbox(oCheckbox, sControl) {
	
		var oControl=document.getElementById(sControl);
		
		if (oControl!=null) {
			if (oCheckbox.checked==true) {
				oControl.style.display='';	
			} else {
				oControl.style.display='none';
			}
		}
	}

	function ToggleCheckboxInvert(oCheckbox, sControl) {
	
		var oControl=document.getElementById(sControl);
		
		if (oControl!=null) {
			if (oCheckbox.checked==true) {
				oControl.style.display='none';
			} else {
				oControl.style.display='';
			}
		}
	}	
	
	function ToggleSection(sSection) {
		//oSection=document.getElementById(sSection);
		oSection=GetObjectById(sSection);
		oHeader=GetObjectById(sSection+'link');
		
		if (oSection.style.display=='none') {
			oSection.style.display='';
			oHeader.className='section toggled';
		} else {
			oSection.style.display='none';
			oHeader.className='section';
		}
	}
	
	function ToggleDrillerPlus(sDrillerID) {
		var oDriller=GetObjectById(sDrillerID);
		var oDrillerState=GetObjectById(sDrillerID+'State');
		var oDrillerImage=GetObjectById(sDrillerID+'Image');
				
		if (oDriller.className=='driller') {
			oDriller.className='driller hide';
			oDrillerState.value='hide';
			oDrillerImage.src='../../images/expandbox.gif';
		} else {
			oDriller.className='driller';
			oDrillerState.value='show';
			oDrillerImage.src='../../images/collapsebox.gif';
		}
	
	}
	
	function ToggleDriller() {
	
		//oSection=document.getElementById(sSection);
		oSection=GetObjectById('divDriller');
		oImg=GetObjectById('imgDriller');
		oHidden=GetObjectById('hidDriller');
		
		if (oSection.className=='hide') {
			oSection.className='';
			oImg.src="../../images/collapsebox.gif";
			oHidden.value="show";
		} else {
			oSection.className='hide';
			oImg.src="../../images/expandbox.gif";
			oHidden.value="hide";
		}
	}
	
	function SetSection(bShow) {
		oSection=GetObjectById('divDriller');
		oImg=GetObjectById('imgDriller');
		oHidden=GetObjectById('hidDriller');
		
		if ((oSection!=null)&&(oImg!=null)) {
			if (bShow=='True') {
				oSection.className='';
				oImg.src="../../images/collapsebox.gif";
				oHidden.value="show";
			} else {
				oSection.className='hide';
				oImg.src="../../images/expandbox.gif";
				oHidden.value="hide";
			}
		}	
	}
	
	function GetObjectById(sID) {
		if (document.getElementById) {
			return document.getElementById(sID);	
		} else {
			return document.layers[sID];
		}
	}
	
	function ConfirmReassign() {
		var sNL=String.fromCharCode(13)
		//get user to confirm the reassign
		if (confirm('Are you sure that you want to continue with reassigning these Users?'+
			sNL+sNL+'This will'+sNL+sNL+'Assign the New User to any customers currently assigned to the Old User'+
			sNL+'Set the Action User to the New User for any actions that are currently Pending and assigned to the Old User'+
			sNL+sNL+'Click on OK to continue or Cancel to abort this operation')==false) {
			return ;
		}
		document.forms[0].Command.value='ReAssign';
		document.forms[0].Argument.value='';
		document.forms[0].submit();
	}	
	
	function CheckPrint(){
		if (confirm('Did the documents print successfully?')==false) {
			return;
		}
	}
	function selectAll(ID) 
	{
	var oForm = document.forms[0];
	var oControl; 
	var sSelectAllID = ID + 'SelectAll';
	var bChecked = document.getElementById(sSelectAllID).checked;
	var iLen = ID.length;
	 
	//look for status of the 'check all' checkbox
	//and set the other checkbox' with ID accordingly 
	for (var i=0;i < oForm.length;i++) 
	{
		oControl = oForm.elements[i];
		//if its a checkbox, and its not a 'Select All' checkbox
		//and its id begins with the id we passed in
		if (oControl.type == 'checkbox' && 
		oControl.id != sSelectAllID && 
		oControl.id.substring(0,iLen) == ID)
		{   
			oControl.checked = bChecked;         
		}
	}
	} 




function UpdateMenuState(sID) {            
    var oHidden=window.document.getElementById('hidMenuNodeState');
    var iStart = oHidden.value.indexOf(sID);
    var iEnd = iStart + sID.length;
    
    //f the ID is already in there, remove it!
    if (iStart>0) {
		//remove the ID from the hidden field and update it
		oHidden.value = oHidden.value.substring(0,iStart) + 
						oHidden.value.substring(iEnd+1, oHidden.value.length);	
		
    } else {        
		//Add the ID to our Hidden Control, need to convert ID to a string first
		sID = "" + sID;
		oHidden.value = oHidden.value + sID + '#';
    }    
}



function GridMoveKey() {

	//if (IE4) 
    //{
    var event = window.event || arguments.callee.caller.arguments[0];
	var keyCode = iif(event.keyCode,event.keyCode,event.which);
	var sID = event.target.id;
	var bArrowKey = false;
    var sControl;
    
		//check if source element is a grid input field
		if (sID.substring(0,1)=='c' && sID.indexOf('_')>0) {
			var iSplitIndex=sID.indexOf('_');
			var iLastSplitIndex=sID.lastIndexOf('_');
			
			var iRow=parseInt(sID.substring(1,iSplitIndex));
			var iCol=parseInt(sID.substring(iSplitIndex+1,iLastSplitIndex));
			var iHash=parseInt(sID.substring(iLastSplitIndex+1));
						
			var r = '';
			if (keyCode == 39 || keyCode == 9){
				//r += 'arrow right';
			    iCol++;

			    sControl = 'c' + iRow + '_' + iCol + '_' + iHash;

			    if (document.getElementById(sControl) == null) {
			        iRow++;
			        iCol = 0;
			    }

			    bArrowKey=true;
			}
			else if (keyCode == 40 || keyCode==13){
				//r += 'arrow down';
			    iRow++;
				bArrowKey=true;
			}
			else if (keyCode == 38){
				//r += 'arrow up';
				iRow--;
				bArrowKey=true;
			}
			else if (keyCode == 37){
				//r += 'arrow left';
			    iCol--;

			    sControl = 'c' + iRow + '_' + iCol + '_' + iHash;

			    if (document.getElementById(sControl) == null) {
			        iRow--;
			        i = 0;

			        for (var i = 0; i < 50; i++) {

			            sControl = 'c' + iRow + '_' + i + '_' + iHash;

			            if (document.getElementById(sControl) == null) {
			                iCol = --i;
                            break;
			            }
			        }
			        
			    }

				bArrowKey=true;
			}
            
			//get focus if an arrow key was pressed
			if (bArrowKey==true) {
				sControl='c' + iRow + '_' + iCol+'_'+iHash;

			    SetFocus(sControl);

				if (document.getElementById(sControl)!=null) {
					document.getElementById(sControl).select();
				}
				
				return false;
			}
		}
	//}
	return true;
}

function SetLastGridDate(iYear,iMonth,iDay){
	dLastGridDate=new Date(iYear,iMonth,iDay);
}

function SetGridDateRange(sDateRange, sGridID)
{
	//wanna get all input items within the GridID param (which is a DIV)
	var aInputs=document.forms(0).tags("Input");
	var oGrid=document.getElementById(sGridID);
	
	var oChild = oGrid.firstChild;
	while ( oChild != oGrid.lastChild )
	{
		alert(oChild.id);		
		oChild = oChild.nextSibling;
	}
	//then we want to insert our daterange param into the next avail
	//start, end date
}

function ShowPDF(sDocumentViewerURL, sDocumentURL) {
	var iTop=(screen.availHeight-600)/2;
	var iLeft=(screen.availWidth-850)/2;

	
	var sFeatures='scrollbars=no, menubar=yes, resizable=yes, status=no,' + 
				'titlebar=no, toolbar=no, height=600, width=850,top=' 
				+ iTop + ', Left=' + iLeft;
	var sURL=sDocumentViewerURL+'?exacturl='+sDocumentURL;
	window.open(sURL,'_blank',sFeatures,true)
}
	
function SetPage(iOffset)
	{
		var iFooterHeight = 60;
		var iScrollHeight=document.documentElement.scrollHeight;
		var iClientHeight=document.documentElement.clientHeight;
		var iHeight=iScrollHeight>iClientHeight ? iScrollHeight : iClientHeight;
				
		var oFooter=document.getElementById('divFooter');
		var oForm=document.getElementById('frm');
		
		if (iOffset==undefined){iOffset=0;}
		
		oFooter.style.top= (iHeight-iFooterHeight+iOffset)+'px';
		oForm.style.height=(iHeight-10+iOffset)+'px';	
		
	}
	
function ToggleStyleSection(Section, sSectionID) {
	var oSection=document.getElementById('div'+sSectionID);
	var oSectionHeader=document.getElementById('spn'+sSectionID+'Header');
	var oImage=document.getElementById('img'+sSectionID);


	if (oSection.className=='hide') {
	
		//show it
		oSection.className='';
		document.getElementById('hidSecState'+sSectionID).value='TRUE';
		oImage.src='../../images/collapsebox.gif';
		
		
	} else {
	
		//hide it
		oSection.className='hide';
		document.getElementById('hidSecState'+sSectionID).value='FALSE';
		oImage.src='../../images/expandbox.gif';
	}
}			


function SelectTab(sTabStripID, sSelectedTab) {

	/* if a disabled link has been clicked ignore it */
	var oLink=document.getElementById('tablink'+sSelectedTab);
	if (oLink.className=='disabled') return;
	

	/* get the hidden input that stores the currently selected tab */
	var oHidden=document.getElementById(sTabStripID+'storeselected');
	
	/*get the class of the currently selected tab */
	var sCurrent=oHidden.value;
	var oCurrent=document.getElementById('tablink'+sCurrent);

	/* if the current tab is error set as normal + error */
	var sError=oCurrent.className.indexOf('error')>0 ? ' error' : '';
	oCurrent.className='normal'+sError;

	
	/*set the selected tab as selected */
	var oSelected=document.getElementById('tablink'+sSelectedTab);
	sError=oSelected.className.indexOf('error')>0 ? ' error' : '';
	oSelected.className='selected'+sError;
	

	/* tabs */			
	var oTab=document.getElementById(sTabStripID+'tabs');
	for (var i=0; i<oTab.childNodes.length;i++) {
		var oNode=oTab.childNodes[i];
		if (oNode.nodeName=='DIV') {
			oNode.className= (oNode.id=='tab'+sSelectedTab ? '':'hide');
		}
	}
	
	/* set selected tab */
	oHidden.value=sSelectedTab;
}


function CopyColumn(iHash,iColumn) {

	//find max row with a value in the first column
	var iCurrentRow=0;
	var iMaxRow=0;
	while (document.getElementById('c'+iCurrentRow+'_0_'+iHash)!=null) {
		if (document.getElementById('c'+iCurrentRow+'_0_'+iHash).value!='') {
			iMaxRow=iCurrentRow;
		}
		iCurrentRow++;
	}
	
	//scan through and copy the values
	var sCopyValue='';
	var oControl;
	for (var iRow=0;iRow<=iMaxRow;iRow++) {
		oControl=document.getElementById('c'+iRow+'_'+iColumn+'_'+iHash);
		if (oControl.value!='' && parseFloat(oControl.value)!=0) {
			sCopyValue=oControl.value;
		} else {
			oControl.value=sCopyValue;
		}
	}
	
}

function ListboxPostback(sCommand, sArgument, bCheckDelete, sConfirmVerb, sConfirmNoun) {

	//if confirm verb and confirm noun are undefined then set them to delete and record respectively
	if ((sConfirmVerb==undefined)||(sConfirmVerb=='')) {
        sConfirmVerb='delete';
	}
	
	if ((sConfirmNoun==undefined)||(sConfirmNoun=='')) {
        sConfirmNoun='record';
	}

	//confirm delete if required
	if (bCheckDelete==true) {
        if (confirm('Are you sure that you want to '+sConfirmVerb+' this '+sConfirmNoun+'?')==false) {
            return;
        }
	}
	
	Postback(sCommand, sArgument);
}

function DataListSetScroll(sListID, sRowID) {
		
		var oList=window.document.getElementById(sListID+'Scroll');
		var oRow=window.document.getElementById(sRowID);
		
		if (oRow!=null) {
			if (oRow.offsetTop > (oList.offsetHeight-oRow.offsetHeight)) {	
				oList.scrollTop=oRow.offsetTop-30;
			}
		}
	}
	
	function DropDownSelect(oList, oEvent, sAutoPostBack) {
				
		var oStoreKeyPress=document.getElementById('hid'+oList.name);
		
		if ((oEvent.keyCode==13)&&(sAutoPostBack!='true')) {
			return;
		} else if ((oEvent.keyCode==13)&&(sAutoPostBack=='true')) {
			Postback(oList.name,'')
		} else if (oEvent.keyCode!=27) {
			oStoreKeyPress.value=oStoreKeyPress.value+String.fromCharCode(oEvent.keyCode);
		} else {
			oStoreKeyPress.value='';
		}
		
		//find it
		SelectItemFromSearch(oList, oStoreKeyPress.value);
		return false;

	}
	
	function SelectItemFromSearch(oList, sSearch) {
		for (var i=0; i<oList.options.length; i++) {
			if (oList.options[i].text.toLowerCase().indexOf(sSearch.toLowerCase())==0) {
				oList.selectedIndex=i;
				if (oList.onchange!=null) {
					oList.onchange();
				}
				return;
			}
		}
		oList.selectedIndex=0;
	}
	
	function ClearSelection(oList) {
		
		document.getElementById('hid'+oList.name).value='';
	}

	function iif(bCondition, oTrue, oFalse) {

		return bCondition ? oTrue : oFalse;

	}


function GetBaseURL() {

	return window.location.href.substring(0,window.location.href.indexOf('/Secure')+1)
}

function Cent(nNumber) {
	
	// returns the amount in the .99 format
	return (nNumber == Math.floor(nNumber)) ? nNumber + '.00' : (  (nNumber*10
	== Math.floor(nNumber*10)) ? nNumber + '0' : nNumber);

}

function Round(nNumber,X) {

	// rounds number to X decimal places, defaults to 2
	X = (!X ? 2 : X);
	return Math.round(nNumber*Math.pow(10,X))/Math.pow(10,X);

}

function SafeInt(sInteger) {		
		if ((sInteger==null)||(sInteger=='')||(sInteger=='0')) {
			return 0;
		} else {
		
			//remove any commas
			var aInt = sInteger.split(",");
			var sTotal='';
			for (var loop=0; loop<aInt.length; loop++) {
				sTotal+=aInt[loop];
			}
			return parseInt(parseFloat(sTotal));
		}
	}
	
function SafeNumeric(sNumber) {		
	if ((sNumber==null)||(sNumber=='')||(sNumber=='0')) {
		return 0;
	} else {
	
		//remove any commas
		var aInt = sNumber.split(",");
		var sTotal='';
		for (var loop=0; loop<aInt.length; loop++) {
			sTotal+=aInt[loop];
		}
		return parseFloat(sTotal);
	}
}


//new exciting dropdown functions
function GetDropdownText(oDropdown) {
	return iif(oDropdown!=null,oDropdown.options[oDropdown.selectedIndex].text,'');
}

function GetDropdownValue(oDropdown) {
	return iif(oDropdown!=null,oDropdown.options[oDropdown.selectedIndex].value,0);
}


/* *************     Validation Functions ********** */
function ClientValidation(oButton, sTable, sWarn, bShowWarnings) {

	if (bShowWarnings==undefined) {
		bShowWarnings=true;
	}

	var sControlID, sNiceName, sValidation,
		sControlPrefix, oControl, sControlValue, sWarnings='',
		iSelectedValue, sSelectedValue, sFocusControl='',bIsValid;
		
	if (sWarn!=undefined) {
		sWarnings=sWarn;
	}
	
	//postback if we haven't got the validation array
	if (typeof(aValidation)=='undefined') {
		ButtonPostBack(oButton);
		return;
	}
	
	for (var i=0;i<aValidation.length;i++) {
	
		if (sTable=='' || sTable==aValidation[i][0]) {
			sControlID=aValidation[i][1];
			sNiceName=aValidation[i][2];
			sValidation=aValidation[i][3];

			
			oControl=document.getElementById(sControlID);
			bIsValid=true;
			
			//if the control is null then don't do nowt
			if (oControl!=null) {
				sControlPrefix=sControlID.substring(0,3);

				// textbox
				if (sControlPrefix=='txt') {

					sControlValue=oControl.value;

					// not empty
					if (sValidation.indexOf('NotEmpty')>-1 && oControl.className.indexOf('number')==-1
								 && sControlValue=='') {
						bIsValid=false;
						sWarnings+='The '+sNiceName+' must be specified|';
					}	
					
					/* not empty is numeric */
					if (sValidation.indexOf('NotEmpty')>-1 && oControl.className.indexOf('number')>-1
								 && SafeNumeric(sControlValue)==0) {
						bIsValid=false;
						sWarnings+='The '+sNiceName+' must be specified|';
					}			
					
					//email
					if (sValidation.indexOf('IsEmail')>-1 && IsEmail(sControlValue)==false && sControlValue!='') {
						bIsValid=false;
						sWarnings+='The '+sNiceName+' must be a Valid Email|';
					}
					
					//time
					if (sValidation.indexOf('IsTime')>-1 && IsTime(sControlValue)==false && sControlValue!='') {
						bIsValid=false;
						sWarnings+='The '+sNiceName+' must be a Valid Time (hh:mm)|';
					}				
					
				}
				
				// autocomplete
				if (sControlPrefix=='acp') {
				
					var oAutoControlValue=document.getElementById(sControlID+'Hidden');
					sControlValue=oAutoControlValue.value;
					
					/* not empty */
					if (sValidation.indexOf('NotEmpty')>-1 && SafeNumeric(sControlValue)==0) {
						bIsValid=false;
						sWarnings+='The '+sNiceName+' must be specified|';
					}			
				}
								

				// date textbox
				if (sControlPrefix=='dtb') {
					sControlValue=oControl.value;
					
					// not empty
					if (sValidation.indexOf('NotEmpty')>-1 && sControlValue=='') {
						bIsValid=false;
						sWarnings+='The '+sNiceName+' must be specified|';
					} else if (sControlValue!='' && IsDate(sControlValue)==false) {
						bIsValid=false;
						sWarnings+='The '+sNiceName+' must be a Valid Date|';
					}
				}

				//plopdown
				if (sControlPrefix=='ddl' || sControlPrefix=='add' || sControlPrefix=='sdd') {

					iValue=SafeInt(oControl.options[oControl.selectedIndex].value);
					sValue=oControl.options[oControl.selectedIndex].text;
					
					//not empty
					if (sValidation.indexOf('NotEmpty')>-1 && 						
							((iValue==0 && sControlPrefix=='ddl' && sValue=='') ||
							(iValue==0 && sControlPrefix!='ddl') || (sControlPrefix=='add' && iValue<1))) {
						bIsValid=false;
						sWarnings+='The '+sNiceName+' must be specified|';
					}
				}
				
				//custom validation
				if (sValidation=='CustomValidation') {
				    if (!aValidation[i][4]()) {
						sWarnings+=aValidation[i][2]+'|';
						bIsValid=false;
					}
				}
				
				//set control valid class
				SetControlValidClass(oControl,bIsValid);

				//if it's the first warning then set the focus control
				if (sWarnings!='' && sFocusControl=='') {
					sFocusControl=sControlID;
				}
			}
		}
	}

	//postback if all's ok or show warnings instead
	if (sWarnings.length==0) {			
		ButtonPostBack(oButton);
	} else if (bShowWarnings) {
		ShowInfoBox(sWarnings,sFocusControl);	
	}	
}	

function CheckedDataListHasSelected(sCheckedDataList) {

	var oInput=document.getElementsByTagName('input');
	for (var i=0;i<oInput.length;i++) {

		if (oInput[i].id.substring(0,sCheckedDataList.length)==sCheckedDataList && oInput[i].id.indexOf('SelectAll')<0) {
			if (oInput[i].checked) {
				return true;
				break;
			}
		}
	}
	
	return false;    
}
		
function SetControlValidClass(oControl,bIsValid) {	
	if (bIsValid==true) {
		oControl.className=oControl.className.replace('error','');
	} else if (oControl.className.indexOf('error')== -1) {
		oControl.className=oControl.className+' error';
	}
}

function IsEmail(sEmail) {
     var sEmailRegEx = /^[a-zA-Z0-9._-]+@([a-zA-Z0-9.-]+\.)+[a-zA-Z0-9.-]{2,4}$/;
     var o = new RegExp(sEmailRegEx);
     return o.test(sEmail);
}

function IsURL(sURL) {
	var sURLRegEx=/(ht|f)tp(s?)\:\/\/[a-zA-Z0-9\-\._]+(\.[a-zA-Z0-9\-\._]+){2,}(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?/;
	var o=new RegExp(sURLRegEx);
	return o.test(sURL);
}

function IsTime(sTime) {
	var sTimeRegEx=/[0-2][0-9]:[0-6][0-9]/;
	var o=new RegExp(sTimeRegEx);
	return o.test(sTime);
}

function IsDate(sDate) {
	var nonDigit = /\D/g;
	var displaydateformat=/^[0-3][0-9]\s(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s(19|20)\d\d$/;
	var now=new Date();
	var dDate;
	var iDay;
	var sMonth;
	var iYear;

	//bomb out if no characters
	if (sDate.length==0) return false;			

	//display date
	if (displaydateformat.test(sDate)) {
		
		//test for max days
		iDay=parseInt(sDate.substring(0,2));
		sMonth=sDate.substring(3,6);
		iYear=parseInt(sDate.substring(7,11));				
		if (iDay<=31 && (sMonth=='Jan' || sMonth=='Mar' || sMonth=='May' || sMonth=='Jul' 
					|| sMonth=='Aug' || sMonth=='Oct' || sMonth=='Dec')) {
			return true;
		} else if (iDay<=30 && (sMonth=='Apr' || sMonth=='Jun' || sMonth=='Sep' || sMonth=='Nov')) {
			return true;
		} else if (sMonth=='Feb' && ((iDay<=29 && CheckLeapYear(iYear)==true)
						|| (iDay<=28 && CheckLeapYear(iYear)==false))) {
			return true;
		} else {
			return false;
		}
		
	} else {
		return false;
	}

}

function CheckLeapYear(iYear) {
	return (((iYear % 4 == 0) && (iYear % 100 != 0)) || (iYear % 400 == 0)) ? 1 : 0;
}

function HideInfoBox(e){
	var Key='';				
		
	if (document.layers) 
		Key=''+ e.which ;
	else 
		Key = window.event.keyCode;
			
	//if info box is displayed + Key is the return key
	if (iOpacity>0 && (Key==13 || Key==27) && document.getElementById('divInfobox').style.display!='none') {
		CloseInfoBox();
		Key=0;
		return false;
	}	
}

function CloseInfoBox(){
	var DivRef = document.getElementById('divInfobox');
	var oMask = document.getElementById('infomask');			
	
	//hide frame mask
	oMask.style.display="none";
	
	DivRef.style.display="none";
	iOpacity=100;

	//set focus control if necessary
	var oHidden=document.getElementById('FocusControl');
	if (oHidden!=null) {
		if (oHidden.value!='') {
			SetFocus(oHidden.value);
		}
	}
}

function ShowInfoBox(sWarnings, sFocusControl){

	//get the ul
	var oInfoBox=document.getElementById('divInfoBox');
	var ulInfoBox=document.getElementById('ulInfoItems');
	var oMask=document.getElementById('infomask');
	var oLi;				
	
	//chop off the last manpipe
	if (sWarnings.substr(-2,2)=='\n') {
		sWarnings=sWarnings.substring(0,sWarnings.length-1);				
	}
	
	//remove any client ones (don't want to add them twice)
	for (iLoop=ulInfoBox.childNodes.length;iLoop>0;iLoop=iLoop-1) {
		oLi=ulInfoBox.childNodes[iLoop-1];
		ulInfoBox.removeChild(oLi);
	}
	
	//add any client warnings
	var aWarnings=sWarnings.split('|');
	var msg = '';
	for (iLoop=0;iLoop<aWarnings.length;iLoop++) {
			oLi=document.createElement("li");
			oLi.innerText=aWarnings[iLoop];
			msg += aWarnings[iLoop]+"\n";
			ulInfoBox.appendChild(oLi);
	}			
	alert(msg);return;
	
	//make sure it's all on display
	oMask.style.display='block';
        if(oInfoBox) {
            oInfoBox.style.display='block';
	    oInfoBox.className='warning';
		//fix height if needed
            oInfoBox.style.top = document.documentElement.scrollTop+200+'px';		
        }
	
	var oHidden=document.getElementById('FocusControl');
	if (oHidden!=null && sFocusControl!=undefined) {
		oHidden.value=sFocusControl;	
	}


}

//Get cookie
function getCookie(name) {
	var oCookie = document.cookie;
	name += "="; 
	var i = 0; 

	while (i < oCookie.length) {
		var offset = i + name.length; // end of section to compare with name string
		if (oCookie.substring(i, offset) == name) { // if string matches
		var endstr = oCookie.indexOf(";", offset); // locate end of name=value pair
		if (endstr == -1) endstr = oCookie.length;
		return unescape(oCookie.substring(offset, endstr)); // return cookie value section
		}
		i = oCookie.indexOf(" ", i) + 1; // move i to next name=value pair
		if (i == 0) break; // no more values in cookie string
	}
		return null; // cookie not found
}
  
//Set cookie
function setCookie (name, value) {
  var argv = setCookie.arguments;
  var argc = setCookie.arguments.length;
  var expires = (argc > 2) ? argv[2] : null;
  var path = (argc > 3) ? argv[3] : null;
  var domain = (argc > 4) ? argv[4] : null;
  var secure = (argc > 5) ? argv[5] : false;
  
  document.cookie = name + "=" + escape (value) +
    ((expires == null) ? "" : ("; expires=" + expires.toGMTString())) +
    ((path == null) ? "" : ("; path=" + path)) +
    ((domain == null) ? "" : ("; domain=" + domain)) +
    ((secure == true) ? "; secure" : "");
}  



/* auto complete/suggest */
var dEarliestTimeToFire;
var oASTextbox;
var sASBaseURL;
function AutoSuggestKeyUp(oEvent, oTextbox, sBaseURL) {

	iKeyCode=iif(oEvent.keyCode,oEvent.keyCode, oEvent.which);
		
	if (iKeyCode<41 && iKeyCode!=32 && iKeyCode!=8) {return;}
	
	var oDiv=document.getElementById(oTextbox.id+'Container');
	
	if (oTextbox.value.length>1) {
		dEarliestTimeToFire=new Date()-0+500;
		oASTextbox=oTextbox;
		sASBaseURL=sBaseURL;
		setTimeout('AutoSuggest_Try()',600);
	} else {
		
		//clear the value
		AutoSuggestClear(oTextbox);
	}
}


function AutoSuggest_Try() {
	
	if (new Date()>dEarliestTimeToFire && oASTextbox.value.length>1) {
		var aParams=document.getElementById(oASTextbox.id+'Params').value.split('|');	
		oASTextbox.className+=' autocompleteworking';
		AutoSuggestSendRequest(sASBaseURL,oASTextbox.id,aParams[0],aParams[1],aParams[4],aParams[5],aParams[6],oASTextbox.value)
	}

}


function AutoSuggestClear(oTextbox) {
	var oHidden=document.getElementById(oTextbox.id+'Hidden');
	oHidden.value='0';
}


function AutoSuggestKeyDown(oEvent, oTextbox) {
	
	iKeyCode=iif(oEvent.keyCode,oEvent.keyCode, oEvent.which);
	var oDiv=document.getElementById(oTextbox.id+'Container');
	var oHidden=document.getElementById(oTextbox.id+'Hidden');
	
		
	if (oDiv.style.display != 'none') {

		switch(iKeyCode) {
			case 38: //up arrow
				AutoSuggestMove(oDiv,-1);
				return;
			case 40: //down arrow
				AutoSuggestMove(oDiv,1);
				return;
			case 33: //page up
				AutoSuggestMove(oDiv,-5);
				return;
			case 34: //page down
				AutoSuggestMove(oDiv,5);
				return;	
			case 27: //escape
				AutoSuggestHideContainer(oDiv);
				return;
			case 9: //tab
				//if there's only one item in the box then select it
				var iSelected=AutoSuggestGetCurrentSelectedIndex(oDiv);	
				if (iSelected==-1 && oDiv.childNodes.length==1) {
					iSelected=0;
				}
				AutoSuggestSelect(oDiv,oTextbox,oHidden,iSelected);
				return;
			case 13: //enter
				
				//if there's only one item in the box then select it
				var iSelected=AutoSuggestGetCurrentSelectedIndex(oDiv);	
				if (iSelected==-1 && oDiv.childNodes.length==1) {
					iSelected=0;
				}
				AutoSuggestSelect(oDiv,oTextbox,oHidden,iSelected);
				
				//get autopostback param, if true (and something is selected) then postback(oTextbox.id)
				var bAutoPostback=document.getElementById(oTextbox.id+'Params').value.split('|')[7];	
				if (bAutoPostback=='True' && iSelected>-1) {
					Postback(oTextbox.id,0);
				}
				oEvent.cancelBubble = true;
				oEvent.returnValue = false;				
				return false;				
			}
			
		}
	}


function AutoSuggestMove(oDiv, iNumberToMove) {

	var iItemCount=oDiv.childNodes.length-1;
	var iCurrentSelected=AutoSuggestGetCurrentSelectedIndex(oDiv);
	var iTarget=iCurrentSelected+iNumberToMove;
	
	if (iTarget<0) {
		iTarget=0;
	} else if (iTarget>iItemCount) {
		iTarget=iItemCount;
	}
	
	AutoSuggestSetSelected(oDiv,iTarget,iCurrentSelected);
}


function AutoSuggestSelect(oDiv, oTextbox, oHidden, iSelected, bMouseClick) {
	
	oDiv=f.SafeObject(oDiv);
	oTextbox=f.SafeObject(oTextbox);
	oHidden=f.SafeObject(oHidden);
	
	if (iSelected>-1) {
		var sDisplay=Replace(oDiv.childNodes[iSelected].innerHTML,'<span>','')
		sDisplay=Replace(sDisplay,'</span>','')
		sDisplay=Replace(sDisplay,'<SPAN>','')
		sDisplay=Replace(sDisplay,'</SPAN>','')
		sDisplay=Replace(sDisplay,'&amp;','&')
		
		if (sDisplay.indexOf('<')>-1) {
			sDisplay=sDisplay.substring(0,sDisplay.indexOf('<'));
		}	

		oTextbox.value=sDisplay;
		
		oHidden.value=oDiv.childNodes[iSelected].id;
		
		//if it's been selected via a mouse click then check for postback
		if (bMouseClick) {
			
			//get autopostback param, if true (and something is selected) then postback(oTextbox.id)
			var bAutoPostback=document.getElementById(oTextbox.id+'Params').value.split('|')[7];	
			if (bAutoPostback=='True' && iSelected>-1) {
				Postback(oTextbox.id,0);
			}
					
		}
		
	}
	AutoSuggestHideContainer(oDiv);
	
}

function AutoSuggestShowContainer(oDiv) {

	var oTextbox=document.getElementById(oDiv.id.replace(/Container/,''));

	oDiv.style.display='block';

	oTextbox.style.position='relative';
	
	var aParams=document.getElementById(oTextbox.id+'Params').value.split('|');	
	if (SafeInt(aParams[2])!=0 || SafeInt(aParams[3])) {
		oDiv.style.top=aParams[3];
		oDiv.style.left=aParams[2];
	} else {
		oDiv.style.top=oTextbox.offsetTop+18+'px';
		oDiv.style.left=oTextbox.offsetLeft+'px';
	}
	
	oDiv.style.width=(oTextbox.offsetWidth-2)+'px';
	
	AutoSuggestManageHiddenControls(oDiv,'Show');
	
}

function AutoSuggestHideContainer(oDiv) {
	oDiv.style.display='none';
	AutoSuggestManageHiddenControls(oDiv,'Hide');
}

function AutoSuggestManageHiddenControls(oDiv,sType) {

	//see if there's any hidden controls to sort
	var oHidden=document.getElementById(oDiv.id.replace(/Container/,'HiddenControls'));
	if (oHidden!=null) {
		
		var aHiddenControls=oHidden.value.split(';');
		var oHiddenControl;
		for (var iLoop=0;iLoop<=aHiddenControls.length-1;iLoop++) {
			
			oHiddenControl=document.getElementById(aHiddenControls[iLoop]);
			if (oHiddenControl!=null) {
			
				if (sType=='Show') {
					oHiddenControl.style.visibility='hidden';
				} else {
					oHiddenControl.style.visibility='';
				}
			}
		}
	}
}

function AutoSuggestSetSelected(oDiv,iSelectIndex,iDeselectIndex) {

	if (iDeselectIndex>-1) {
		oDiv.childNodes[iDeselectIndex].className='';
	}
	oDiv.childNodes[iSelectIndex].className='selected';
	
	oDiv.scrollTop=oDiv.childNodes[iSelectIndex].offsetTop-30;
	
}

function AutoSuggestGetCurrentSelectedIndex(oDiv) {
	for (var i=0;i<oDiv.childNodes.length;i++) {
		if (oDiv.childNodes[i].className=='selected') {
			return i;
		}
	}
	return -1;
}



/*  auto suggest clever stuff */
var oASRequest;

function AutoSuggestSendRequest(sPage,sControl,sTable,sExpression,sSecondExpression,sFilter,sOrder,sSearch) {
	
	var sUrl=sPage+'?Control='+sControl+'&Table='+sTable+'&Expression='+sExpression
		+'&SecondExpression='+sSecondExpression+'&Filter='+sFilter+'&Order='+sOrder+'&Search='+sSearch;
	

	// branch for native XMLHttpRequest object
   if (window.XMLHttpRequest) {
      oASRequest = new XMLHttpRequest();
      oASRequest.onreadystatechange = AutoSuggestReceiveResponse;
      oASRequest.open("GET", sUrl, true);
      oASRequest.send(null);

   // branch for IE/Windows ActiveX version
   } else if (window.ActiveXObject) {
		//isIE = true;
		oASRequest = new ActiveXObject("Microsoft.XMLHTTP");
		if (oASRequest) {
				oASRequest.onreadystatechange = AutoSuggestReceiveResponse;
				oASRequest.open("GET", sUrl, true);
				oASRequest.send();
		}
   }
}


function AutoSuggestReceiveResponse() {

	if (oASRequest.readyState == 4) {
		if (oASRequest.status == 200) {
			AutoSuggestDisplayResults(oASRequest.responseText);
			
		} else {
			// alert("There was a problem retrieving the XML data:\n" +oRequest.statusText);
		}
	}
}

function AutoSuggestDisplayResults(sResults) {

	var aResults=sResults.split('|');
	var oContainer=document.getElementById(aResults[0]+'Container');
	var sResult='';
	var aSplit;
	
	//work out ids
	var sDiv=oContainer.id;
	var sTextbox=aResults[0];
	var sHidden=aResults[0]+'Hidden';

	for (var i=1; i<aResults.length;i++) {
	
		aSplit=aResults[i].split('^');
		if (aSplit.length==2) {
			//sResult+='<div id="'+aSplit[1]+'">'+aSplit[0]+'</div>'
			sResult+='<div id="'+aSplit[1]+'" onclick="AutoSuggestSelect(\''+oContainer.id+'\',\''+sTextbox+'\',\''+sHidden+'\','+(i-1)+',true);">'+aSplit[0]+'</div>'
		} else {
			//sResult+='<div id="'+aSplit[2]+'">'+aSplit[0]+'<div>'+aSplit[1]+'</div>'+'</div>'
			sResult+='<div id="'+aSplit[2]+'" onclick="AutoSuggestSelect(\''+oContainer.id+'\',\''+sTextbox+'\',\''+sHidden+'\','+(i-1)+',true);">'+aSplit[0]+'<div>'+aSplit[1]+'</div>'+'</div>'
		}
	}
	
	var oTextbox=f.GetObject(sTextbox);
	oTextbox.className=s.Replace(oTextbox.className,' autocompleteworking','');
	
	oContainer.innerHTML=sResult;
	AutoSuggestShowContainer(oContainer);
}



/* ##########################  Datalist Excel Export Stuff #################################### */
function ExcelExport(sWebServicePage, sCallingPage, sDatalist, iColumns) {			
	ExcelExportSendRequest(sWebServicePage, sCallingPage, sDatalist, iColumns);		
}

function ExcelExportPopUp(sReturn) {

	if (sReturn.substring(0,7)=='Warning') {
		alert(sReturn);
	} else {
		window.open(sReturn,'_blank');
	}
}

function ExcelExportSendRequest(sWebServicePage,sCallingPage,sDatalist,iColumns) {

	var sUrl=sWebServicePage+'?CallingPage='+sCallingPage+'&Datalist='+sDatalist+'&Columns='+iColumns;

		
	// branch for native XMLHttpRequest object
	if (window.XMLHttpRequest) {
		oASRequest = new XMLHttpRequest();
		oASRequest.onreadystatechange = ExcelExportReceiveResponse;
		oASRequest.open("GET", sUrl, true);
		oASRequest.send(null);

	// branch for IE/Windows ActiveX version
	} else if (window.ActiveXObject) {
			//isIE = true;
		oASRequest = new ActiveXObject("Microsoft.XMLHTTP");
		if (oASRequest) {
				oASRequest.onreadystatechange = ExcelExportReceiveResponse;
				oASRequest.open("GET", sUrl, true);
				oASRequest.send();
		}
	}
}


function ExcelExportReceiveResponse() {

	if (oASRequest.readyState == 4) {
		if (oASRequest.status == 200) {
			ExcelExportPopUp(oASRequest.responseText);					
		} else {
			alert("There was a problem retrieving the data:\n" +oRequest.statusText);
		}
	}
}





var Datalist = new function() {

    this.ClickHandler = function(o) {
        var oRow = null;

        while (o.parentNode && o.tagName != 'IMG') {
            if (o.tagName == 'TR') {
                oRow = o;
                break;
            }
            o = o.parentNode;
        }

        if (oRow != null) {
            var aLinks = oRow.getElementsByTagName('A');
            if (aLinks.length > 0) {
                aLinks[0].click();
            }
        }
    }


    
    this.ToggleSearch = function(ListID) {
        var sTextBox = 'txt' + ListID + 'clientfilter';
        if (!f.Visible(sTextBox))
            {
            f.Show(sTextBox);
            f.SafeObject(sTextBox).focus();
            }
        else
            {f.SetValue(sTextBox,'');
            f.Hide(sTextBox);
            this.ClientFilter(null,ListID)
            }

    }

    this.ClientFilter = function(oEvent, ListID) {
        //see if they have pressed Escape
        if (oEvent) {
            var iKeyCode = f.GetKeyCodeFromEvent(oEvent);
            if (iKeyCode == 27) {
                this.ToggleSearch(ListID);
                return false;}
            }

        //get the search string
        var sTextBox = 'txt' + ListID + 'clientfilter';
        var sFilterString = f.GetValue(sTextBox).toUpperCase();

        //get the datalist table string
        var aListTable = f.GetElementsByClassName('table', 'dl', ListID + 'Scroll')
        if (aListTable.length == 1) {
            var tblList = aListTable[0];
                        
            //loop through the rows
            if (tblList.rows && tblList.rows.length > 0) {
                var oFirstRow = tblList.rows[0];
                var oFirstDisplayRow = null;

                for (i = 0; i < tblList.rows.length; i++) {
                    var oRow = tblList.rows[i];
                    var bShow = false;
                    
                    if (sFilterString == '')
                        bShow=true;
                    else {
                        //loop through the cells
                        var aCells = oRow.cells;
                        for (j = 0; j < aCells.length-1; j++) {
                            if (j==0 && aCells[j].className.indexOf('grouprow')>0)
                                {break;
                                }
                            else if (aCells[j].innerHTML.toUpperCase().indexOf(sFilterString) > -1) {
                                bShow = true;
                                break;
                            }
                        }
                        
                    }
                    
                    f.ShowIf(oRow, bShow)                    
                    
                    if (!oFirstDisplayRow && bShow) {
                        oFirstDisplayRow = oRow
                    }
                }
                
                //copy the classes from the first row if necessary
                if (oFirstDisplayRow && oFirstDisplayRow != oFirstRow) {
                    var aCells = oFirstRow.cells;
                    for (j = 0; j < aCells.length-1; j++) {
                        f.SetClass(oFirstDisplayRow.cells[j], f.GetClass(oFirstRow.cells[j]));
                    }
                }
            }
        }
    }
};

(function () {
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = '//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js';
    document.head.appendChild(script);
})();

function _bindReady(handler) {

	var called = false

	function ready() { 
		if (called) return
		called = true
		handler()
	}

	if ( document.addEventListener ) { // native event
		document.addEventListener( "DOMContentLoaded", ready, false )
	} else if ( document.attachEvent ) {  // IE

		try {
			var isFrame = window.frameElement != null
		} catch(e) {}

		// IE, the document is not inside a frame
		if ( document.documentElement.doScroll && !isFrame ) {
			function tryScroll(){
				if (called) return
				try {
					document.documentElement.doScroll("left")
					ready()
				} catch(e) {
					setTimeout(tryScroll, 10)
				}
			}
			tryScroll()
		}

		// IE, the document is inside a frame
		document.attachEvent("onreadystatechange", function(){
			if ( document.readyState === "complete" ) {
				ready()
			}
		})
	}

	// Old browsers
    if (window.addEventListener)
        window.addEventListener('load', ready, false)
    else if (window.attachEvent)
        window.attachEvent('onload', ready)
    else {
		var fn = window.onload // very old browser, copy old onload
		window.onload = function() { // replace by new onload and call the old one
			fn && fn()
			ready()
		}
    }
}

_bindReady(function() {
	tryJqueryBind();
});
function tryJqueryBind() {
	if(typeof(jQuery) == "undefined") {
		setTimeout(function() {
			tryJqueryBind();
		}, 100);
		return;
	}
	window.$j = jQuery.noConflict();
	pageReRender();
}

function pageReRender() {
    $j("#divIntuitiveHeader").append('<div class="headerLogo"></div>');
    var linkHTML = '\
        <div class="headerLinks">\
            <a class="icon reports" href="http://rmireports.centriumres.com/" target="_blank"><div>R<span>reports</span></div></a>\
            <a class="icon xml" href="http://xml.centriumres.com/inputFormXml.php" target="_blank"><div>X<span>xml</span></div></a>\
            <a class="icon mail" href="https://rm-mail.resort-marketing.co.uk/owa/" target="_blank"><div>M<span>mail</span></div></a>\
            <a class="icon rmi" href="http://www.resort-marketing.co.uk" target="_blank"><div>RMI</div></a>\
            <a class="icon weddings" href="http://www.calendarwiz.com/eliteweddings" target="_blank"><div>W<span>weddings</span></div></a>\
            <a class="icon eir" href="http://www.eliteislandresorts.co.uk" target="_blank"><div>EIR</div></a>\
            <a class="icon packages" href="http://www.eliteislandresorts.co.uk/pages/packages.php" target="_blank"><div>P<span>packages</span></div></a>\
        </div>\
    ';
    $j("#divIntuitiveHeader").append(linkHTML);

    /* var searchHTML = '\
        <div class="googleSearch">\
            <img src="http://www.google.co.uk/images/srpr/logo3w.png" />\
            <input type="text" class="text-search" />\
            <div class="btn">Google Search</div>\
        </div>\
    ';
    $j("#divIntuitiveHeader").append(searchHTML);

    $j("#divIntuitiveHeader .googleSearch .text-search").bind('keypress', function(e) {
        if(e.keyCode == 13) {
            openSearch() ;
        }
    });

    $j("#divIntuitiveHeader .googleSearch .btn").bind('click', function(e) {
        e.preventDefault();
        openSearch() ;
    });

    function openSearch() {
        var el = $j("#divIntuitiveHeader .googleSearch .text-search");
        url = 'https://www.google.co.uk/search?q='+encodeURIComponent(el.val());
        window.open(url,'_blank');
        el.val('').blur();
    } */
}

function safeJ(cb) {
	if(typeof(jQuery) == "undefined" || typeof($j) == "undefined") {
		setTimeout(function() {
			safeJ(cb);
		}, 100);
		return;
	}
	cb();
}
