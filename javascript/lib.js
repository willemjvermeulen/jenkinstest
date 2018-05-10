/* 23/08/10 12:10 DM */

var a = new ArrayFunctions();
var d = new DateFunctions();
var f = new FormFunctions();
var n = new NumberFunctions();
var s = new StringFunctions();
var dd = new DropdownFunctions();
var cb = new CheckBoxFunctions();
var e = new Effects();
var dl = new DataListFunctions();
var c = new CookieFunctions();
var b = new BrowserFunctions();


/* array functions */
function ArrayFunctions() {

	this.IsArray = function(o) {
		return o && typeof o === 'object' && typeof o.length === 'number' &&
	          !(o.propertyIsEnumerable('length'));
    }

    this.ArrayContains = function(oArray, oValue) {
        if (a.IsArray(oArray)) {
            for (var i = 0; i < oArray.length; i++) {
                if (oArray[i] == oValue) return true;
            }
        }

        return false;
    }
}



//date functions
function DateFunctions() {

	this.New = function(iDay, iMonth, iYear) {
		return new Date(iYear, iMonth - 1, iDay);
    }

    this.Today = function() {
        var dToday = new Date();
        return d.New(d.Day(dToday), d.Month(dToday), d.Year(dToday));
    }

	this.GetDateOnly = function(dDate) {
		return d.New(d.Day(dDate), d.Month(dDate), d.Year(dDate));
	}

	this.AddDays = function(dDate, iDays) {
		dDate.setDate(dDate.getDate() + iDays);
		return dDate;
	}

	this.Year = function(dDate) {
		return dDate.getFullYear();
	}

	this.Month = function(dDate) {
		return dDate.getMonth() + 1;
	}

	this.Day = function(dDate) {
		return dDate.getDate();
	}

	this.DayName = function(dDate) {
		return s.Left(dDate + '', 3);
	}

	this.MonthName = function(dDate) {
		var aMonths = new Array('Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec');
		return aMonths[d.Month(dDate) - 1];
	}

	this.MonthEnd = function(dDate) {

		//create new date 01/month+1/year
		return d.AddDays(d.New(1, (d.Month(dDate) == 12) ? 1 : d.Month(dDate) + 1,
			(d.Month(dDate) == 12) ? d.Year(dDate) + 1 : d.Year(dDate)), -1);
	}

	this.Weekend = function(dDate) {
		return (s.Left(dDate + '', 1) == 'S');
	}

	this.IsDate = function(oDate) {
		return !isNaN(new Date(oDate));
	}

	this.SafeDate = function(oDate) {
		if (this.IsDate(oDate)) {
			return new Date(oDate);
		}
	}

	this.FromSQLDate = function(sDate) {
		return d.New(sDate.substring(8, 10), sDate.substring(5, 7), sDate.substring(0, 4));
	}
	this.ToSQLDate = function(dDate) {
		var sYear = dDate.getFullYear().toString();
		var sMonth = s.PadWithZeros((dDate.getMonth() + 1).toString(), 2);
		var sDay = s.PadWithZeros(dDate.getDate().toString(), 2);

		var sHours = s.PadWithZeros(dDate.getHours().toString(), 2);
		var sMinutes = s.PadWithZeros(dDate.getMinutes().toString(), 2);
		var sSeconds = s.PadWithZeros(dDate.getSeconds().toString(), 2);

		return sYear + '-' + sMonth + '-' + sDay + 'T' + sHours + ':' + sMinutes + ':' + sSeconds;
	}

	this.DisplayDate = function(dDate) {
		dDate = new Date(dDate)

		var aMonths = new Array('Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec')

		var sDay = dDate.getDate().toString()
		if (sDay.length == 1) {
			sDay = '0' + sDay;
		}
		return sDay + ' ' + aMonths[dDate.getMonth()] + ' ' + dDate.getFullYear();
	}

	this.GetAge = function(dDateOfBirth) {

		var dNow = new Date();
		var iAge = -1;

		while (dNow >= dDateOfBirth) {
			iAge++;
			dDateOfBirth.setFullYear(dDateOfBirth.getFullYear() + 1);
		}

		return iAge;
	}

	this.DateDiff = function(sStartDate, sEndDate) {

		var dStartDate = new Date(sStartDate);
		var dEndDate = new Date(sEndDate);
		var iStartYear;
		var iEndYear;
		var iStartDayOfYear;
		var iEndDayOfYear;
		var iDiff;

		//get the years and day of years, if end date is before start date then swap them round
		if (dStartDate <= dEndDate) {
			iStartYear = dStartDate.getYear();
			iEndYear = dEndDate.getYear();
			iStartDayOfYear = this.DayOfYear(dStartDate);
			iEndDayOfYear = this.DayOfYear(dEndDate);
		} else {
			iStartYear = dEndDate.getYear();
			iEndYear = dStartDate.getYear();
			iStartDayOfYear = this.DayOfYear(dEndDate);
			iEndDayOfYear = this.DayOfYear(dStartDate);
		}


		//2 possibilities, same year, different years
		if (iStartYear == iEndYear) {

			iDiff = iEndDayOfYear - iStartDayOfYear;

		} else {

			//one or more years apart starts with same calculation
			iDiff = iEndDayOfYear + (365 - iStartDayOfYear);

			//if it's a leap year and next year is different then add
			if (this.CheckLeapYear(iStartYear) == 1 && iEndYear != iStartYear) {
				iDiff += 1;
			}

			//now loop through all (if any years inbetween)
			for (var iLoop = iStartYear + 1; iLoop < iEndYear; iLoop++) {

				//add 365 for a normal year, 366 for a leap year
				if (this.CheckLeapYear(iLoop) == 1) {
					iDiff += 366;
				} else {
					iDiff += 365;
				}
			}
		}

		// if start date > end date invert the difference
		if (dStartDate > dEndDate) {
			iDiff = iDiff * (-1);
		}

		return iDiff;
	}

	this.CheckLeapYear = function(iYear) {
		return (((iYear % 4 == 0) && (iYear % 100 != 0)) || (iYear % 400 == 0)) ? 1 : 0;
	}

	this.DayOfYear = function(dDate) {

		//start with current day of month and then add on preivous mointh days
		var iDayOfYear = dDate.getDate();
		var iMonth = dDate.getMonth();
		var iYear = dDate.getYear();

		//if it's a leap year and we are past Februrary then add 1
		if ((this.CheckLeapYear(iYear) == 1) && (iMonth >= 2)) {
			iDayOfYear++;
		}

		//now do a huge ugly if statement adding the rest on for the months
		if (iMonth == 1) {
			iDayOfYear += 31;
		} else if (iMonth == 2) {
			iDayOfYear += 59;
		} else if (iMonth == 3) {
			iDayOfYear += 90;
		} else if (iMonth == 4) {
			iDayOfYear += 120;
		} else if (iMonth == 5) {
			iDayOfYear += 151;
		} else if (iMonth == 6) {
			iDayOfYear += 181;
		} else if (iMonth == 7) {
			iDayOfYear += 212;
		} else if (iMonth == 8) {
			iDayOfYear += 243;
		} else if (iMonth == 9) {
			iDayOfYear += 273;
		} else if (iMonth == 10) {
			iDayOfYear += 304;
		} else if (iMonth == 11) {
			iDayOfYear += 334;
		}

		return iDayOfYear;
	}

}

//number functions
function NumberFunctions() {

	this.SafeInt = function(sInteger) {
		if ((sInteger == null) || (sInteger == '') || (sInteger == '0') || isNaN(parseFloat(sInteger))) {
			return 0;
		} else {

			//remove any commas
			sInteger += '';
			var aInt = sInteger.split(",");
			var sTotal = '';
			for (var loop = 0; loop < aInt.length; loop++) {
				sTotal += aInt[loop];
			}
			return parseInt(parseFloat(sTotal));
		}
	}


	this.SafeNumeric = function(sNumber) {
		if (sNumber == null || sNumber == '' || sNumber == '0' || isNaN(parseFloat(sNumber))) {
			return 0;
		} else {

			//remove any commas
			sNumber += '';
			return parseFloat(sNumber.replace(',', ''));
		}
	}

	this.Cent = function(nNumber) {

		// returns the amount in the .99 format
		return (nNumber == Math.floor(nNumber)) ? nNumber + '.00' : ((nNumber * 10 == Math.floor(nNumber * 10)) ? nNumber + '0' : nNumber);

	}

	this.Round = function(nNumber, X) {

		// rounds number to X decimal places, defaults to 2
		X = (!X ? 2 : X);
		return Math.round(nNumber * Math.pow(10, X)) / Math.pow(10, X);

	}

	this.FormatMoney = function(nNumber, sCurrency) {

		//get the rounded figure
		var nRounded = n.Cent(n.Round(nNumber));
		if (sCurrency != undefined) {

			if (nRounded < 0) {
				nRounded = n.Cent(nRounded * (-1));
				return '-' + sCurrency + nRounded;
			} else {
				return sCurrency + nRounded;
			}
		} else {
			return nRounded;
		}

	}

	this.FormatNumber = function(o, iDecimalPlaces) {

		o = n.SafeNumeric(o);
		return o.toFixed(iDecimalPlaces == undefined ? 2 : iDecimalPlaces);
    }

    this.FormatCommas = function(nStr) {
        nStr += '';
        x = nStr.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    }

}

//string functions
function StringFunctions() {

	this.Left = function(s, i) {
		return s.substring(0, i);
	}

	this.Right = function(s, i) {
		return s.substring(s.length - i);
	}

	this.Chop = function(sString, i) {

		if (i == undefined) {
			i = 1;
		}

		return s.Substring(sString, 0, sString.length - i);
	}

	this.Substring = function(s, iStart, iLength) {

		if (iLength == undefined) {
			return s.substring(iStart);
		} else {
			return s.substring(iStart, iLength);
		}
	}

	this.Slice = function(s, iStart, iEnd) {
		if (iEnd == undefined) {
			iEnd = iStart;
		}
		return s.substring(iStart, iStart + (iEnd - iStart) + 1);
	}

	this.StartsWith = function(sBase, sCompare) {
		return sBase.indexOf(sCompare) == 0;
	}

	this.Replace = function(sString, sStringToReplace, sReplacement) {
		while (sString.indexOf(sStringToReplace) != -1) {
			sString = sString.replace(sStringToReplace, sReplacement);
		}
		return sString;
	}

	this.Trim = function(sBase) {
		return sBase.replace(/^\s*|\s*$/g, '');
	}

	this.ArrayToCSV = function(aArray) {
		var sReturn = '';
		if (aArray.constructor.toString().indexOf('Array') > 0) {
			for (var i = 0; i < aArray.length; i++) {
				sReturn += aArray[i].toString() + (i < aArray.length - 1 ? ',' : '');
			}
		}

		return sReturn;
	}

	this.PadWithZeros = function(sBase, iLength) {
		if (sBase.length < iLength) {
			return '0' * (iLength - sBase.length) + sBase;
		} else {
			return sBase;
		}
	}

	this.EncodeString = function(sString) {
		return encodeURIComponent(sString);
	}

	this.Format = function(sString) {

	    if (arguments.length <= 1) return sString;

	    var tokenCount = arguments.length - 2;
	    for (var token = 0; token <= tokenCount; token++) {
	        sString = sString.replace(new RegExp('\\{' + token + '\\}', 'gi'), arguments[token + 1]);
	    }
	    return sString;
	}
}


//form functions
function FormFunctions() {

	this.GetObject = function(sID) {
		return document.getElementById(sID);
	}

	this.GetObjectsByIDPrefix = function(sPrefix, sTagName, oContainer) {

		if (oContainer == undefined) {
			oContainer = document;
		} else {
			oContainer = f.GetObject(oContainer);
		}

		var aObjects = new Array();

		if (sTagName == undefined) {
			sTagName = 'input';
		}

		var aElements = oContainer.getElementsByTagName(sTagName);
		for (var i = 0; i < aElements.length; i++) {
			if (s.StartsWith(aElements[i].id, sPrefix)) {
				aObjects.push(aElements[i]);
			}
		}

		return aObjects;
	}



	this.GetValue = function(o) {
		var oControl = this.SafeObject(o);
		if (oControl != null) {
			return oControl.value;
		} else {
			return '';
		}
	}

	this.GetIntValue = function(o) {
		var oControl = this.SafeObject(o);
		if (oControl != null) {
			return n.SafeInt(oControl.value);
		} else {
			return 0;
		}
	}

	this.GetNumericValue = function(o) {
		var oControl = this.SafeObject(o);
		if (oControl != null) {
			return n.SafeNumeric(oControl.value);
		} else {
			return 0;
		}
	}

	this.SetValue = function(o, sValue) {
		var oControl = this.SafeObject(o);
		if (oControl != null) {
			oControl.value = sValue;
		}
	}

	this.GetHTML = function(o) {
		var oControl = this.SafeObject(o);
		if (oControl != null) {
			return oControl.innerHTML;
		}
		return '';
	}

	this.SetHTML = function(o, sValue, bRunInlineScripts) {
	    var oControl = this.SafeObject(o);
	    if (oControl != null) {
	        oControl.innerHTML = sValue;
	    }

	    // run any inline scripts included in the HTML if the bRunScriptTags parameter was specified.
	    if (bRunInlineScripts != undefined && bRunInlineScripts) f.RunScriptsWithinHTML(sValue);
	}

    this.RunScriptsWithinHTML = function(sHTML) {
        var scriptregex = /\<script(?:\stype=(?:"text\/javascript"|text\/javascript))?\s?\>([\s\S]*?)\<\/script\>/gim;
        var match;
        while (match = scriptregex.exec(sHTML)) {
            eval(match[1]);
        }
    }

	this.SafeObject = function(o) {
		if (typeof (o) == 'object') {
			return o;
		} else if (typeof (o) == 'string') {
			return this.GetObject(o);
		} else {
			return null;
		}
	}

	this.Toggle = function(o) {
		var oControl = this.SafeObject(o);
		if (oControl.tagName.toUpperCase() == 'TR' && !b.IE())
		{ oControl.style.display = oControl.style.display == 'none' ? 'table-row' : 'none' }
		else
		{ oControl.style.display = oControl.style.display == 'none' ? 'block' : 'none'; }
	}

	this.Show = function(o) {
		var oControl = this.SafeObject(o);
		if (oControl != null) {
			oControl.style.display = oControl.style.display = '';
		}
	}

	this.Hide = function(o) {
		var oControl = this.SafeObject(o);
		if (oControl != null) {
			oControl.style.display = oControl.style.display = 'none';
		}
	}

	this.Visible = function(o) {
		var oControl = this.SafeObject(o);
		if (oControl != null) {
			return oControl.style.display != 'none';
		} else {
			return false;
		}
	}

	this.SetFocus = function(o) { // will also accept a pipe seperated list of objects to try in turn.

		if (typeof (o) == 'string') {
			var aObjects = o.split('|');

			for (var i = 0; i < aObjects.length; i++) {
				if (f.SetFocus(f.SafeObject(aObjects[i]))) return true;
			}

		} else {
			var oControl = f.SafeObject(o);
			if (oControl && oControl.focus != undefined) {
				try {
					oControl.focus();
					return true;
				} catch (exception) { /* stops the browser throwing up a silly error */ }
			}
		}

		return false;
	}

	this.SetClass = function(o, s) {
		var oControl = this.SafeObject(o);
		oControl.className = s;
	}

	this.GetClass = function(o) {
		var oControl = this.SafeObject(o);
		return oControl.className;
	}

	this.SetClassIf = function(o, ClassName, bCondition) {

		if (bCondition) {
			f.AddClass(o, ClassName);
		} else {
			f.RemoveClass(o, ClassName);
		}
	}

    this.ToggleClassIf = function(o, TrueClass, FalseClass, bCondition) {

        if (bCondition) {
            f.AddClass(o, TrueClass);
            f.RemoveClass(o, FalseClass);
        } else {
            f.RemoveClass(o, TrueClass);
            f.AddClass(o, FalseClass);
        }
    }


	this.AddClass = function(o, s) {

		var aClassNames = f.GetClass(o).split(' ');

		// add class if it doesn't exist already
		if (!f.HasClass(o, s)) {
			f.SetClass(o, f.GetClass(o) + ' ' + s)
		}
	}


	this.RemoveClass = function(o, s) {

		var sClassName = '';
		var aClassNames = f.GetClass(o).split(' ');

		// 
		for (var i = 0; i < aClassNames.length; i++) {
			if (aClassNames[i] != s) {
				sClassName = sClassName + aClassNames[i] + ' ';
			}
		}

		f.SetClass(o, sClassName);
	}


	this.ToggleClass = function(o, s) {

		if (f.HasClass(o, s)) {
			f.RemoveClass(o, s);
		} else {
			f.AddClass(o, s);
		}

	}


	this.HasClass = function(o, s) {

		var sClass = f.GetClass(o);
		var aClassNames = sClass == undefined ? [] : sClass.split(' ');

		for (var i = 0; i < aClassNames.length; i++) {
			if (aClassNames[i] == s) {
				return true;
			}
		}

		return false;

	}


	this.GetElementsByClassName = function(sElement, sClassName, oContainer) {

		if (oContainer == undefined) {
			oContainer = document;
		} else {
			oContainer = f.SafeObject(oContainer);
		}

		if (sElement == '' || sElement == null || sElement == undefined) sElement = '*';

		var aElements = oContainer.getElementsByTagName(sElement);
		var aReturn = new Array();
		for (var i = 0; i < aElements.length; i++) {

			if (aElements[i].className.indexOf(sClassName) > -1) {
				aReturn[aReturn.length] = aElements[i];
			}
		}

		return aReturn;
    }


    this.ToggleFocus = function(o) {
        var oControl = this.SafeObject(o);
        oControl.style.display = oControl.style.display == 'none' ? 'block' : 'none';
        if (oControl.style.display == 'block') {
            SetFocus(oControl);
        }
    }


	this.ShowIf = function(o, bCondition) {

		if (o.constructor != Array) {
			var oControl = this.SafeObject(o);
			if (bCondition) {
				this.Show(o);
			} else {
				this.Hide(o);
			}
		} else {
			for (var i = 0; i < o.length; i++) {
				f.ShowIf(o[i], bCondition);
			}
		}
	}


	this.BuildList = function(aListItems) {

		var sList = '<ul>';
		for (var i = 0; i < aListItems.length; i++) {
			sList += '<li>' + aListItems[i] + '</li>';
		}
		sList += '</ul>';
		return sList;
	}

	this.Disable = function(o) {

		var oControl = this.SafeObject(o);
		if (oControl != null) {
			oControl.readOnly = true;
		}
	}

	this.Enable = function(o) {

		var oControl = this.SafeObject(o);
		if (oControl != null) {
			oControl.readOnly = false;
		}
	}

	this.ClearFileUpload = function(o) {

		var oControl = this.SafeObject(o);
		if (oControl != null) {
			oControl.outerHTML = oControl.outerHTML;
		}
	}


	/* event handling */
	this.AttachEvent = function(oObject, sEventName, oFunction) {

		oObject = this.SafeObject(oObject);

		var oListenerFunction = oFunction;

		if (oObject.addEventListener) {
			oObject.addEventListener(sEventName, oListenerFunction, false);
		} else if (oObject.attachEvent) {
			oListenerFunction = function() {
				oFunction(window.event);
			}
			oObject.attachEvent("on" + sEventName, oListenerFunction);
		} else {
			throw new Error("Event registration not supported");
		}


		var oEvent = { Instance: oObject, EventName: sEventName, Listener: oListenerFunction };
		return oEvent;
	}


	this.DetachEvent = function(oEvent) {

		var oObject = oEvent.Instance;

		if (oObject.removeEventListener) {
			oObject.removeEventListener(oEvent.EventName, oEvent.Listener, false);
		} else if (oObject.detachEvent) {
			oObject.detachEvent("on" + oEvent.EventName, oEvent.Listener);
		}
	}

	this.GetObjectFromEvent = function(oEvent) {
		return oEvent.srcElement ? oEvent.srcElement : oEvent.target;
	}

	this.GetKeyCodeFromEvent = function(oEvent) {
		return oEvent.keyCode ? oEvent.keyCode : oEvent.which;
	}

	this.FireEvent = function(oObject, oEvent) {

		var o = f.SafeObject(oObject);

		if (o.dispatchEvent) {
			o.dispatchEvent(oEvent);
		} else if (o.fireEvent) {
			o.fireEvent('on' + oEvent.type, oEvent);
		} else {
			throw new Error("Event firing not supported");
		}

	}

	this.ShowPopup = function(oObject, sClassName, sHTML, sSourceObjectID, bRightAlign, iYOffset, iXOffset) {

		if (iYOffset == undefined) { iYOffset = 0; }
		if (iXOffset == undefined) { iXOffset = 0; }

		if (sSourceObjectID != undefined && f.GetObject(sSourceObjectID)) {
			sHTML = f.GetObject(sSourceObjectID).innerHTML;
		}

		//create container			
		var oHelp = document.createElement('div');
		oHelp.setAttribute('id', 'divPopup');
		f.SetClass(oHelp, sClassName);
		oHelp.style.position = 'absolute';
		oHelp.innerHTML = sHTML;

		//set position
		var oDimensions = new e.BrowserDimensions();
		var oLinkPosition;
		if (!oObject.Left) {
			oLinkPosition = e.GetPosition(oObject);
		} else {
			oLinkPosition = new e.Position();
			oLinkPosition.Left = oObject.Left;
			oLinkPosition.Top = oObject.Top;
		}

		oHelp.style.top = n.SafeInt(oLinkPosition.Top + 20 + iYOffset) + 'px';
		oHelp.style.left = n.SafeInt(oLinkPosition.Left + iXOffset) + 'px';



		//create mask
		if (b.IE6()) {
			var oMask = document.createElement('iframe');
			oMask.setAttribute('id', 'iMask');
			oMask.src = '';
			e.SetPosition(oMask, e.GetPosition(oHelp));
			f.GetObject('frm').appendChild(oMask);
		}

		f.GetObject('frm').appendChild(oHelp);

		//move it if it's too low
		oHelpPosition = e.GetPosition(oHelp);
		if (oHelpPosition.Top + oHelpPosition.Height > oDimensions.ViewportHeight + f.ScrollPosition()) {
			oHelp.style.top = oDimensions.ViewportHeight + f.ScrollPosition() - oHelp.offsetHeight - 10 + 'px';
		}


		//if we're right aligning then shift over now
		if (bRightAlign != undefined && bRightAlign) {
			oHelp.style.left = oLinkPosition.Left - oHelp.clientWidth + iXOffset + 'px';
		}
	}


	this.HidePopup = function() {
		if (b.IE6() && f.GetObject('iMask')) {
			f.GetObject('frm').removeChild(f.GetObject('iMask'));
		}

		if (f.GetObject('divPopup')) {
			f.GetObject('frm').removeChild(f.GetObject('divPopup'));
		}
	}

	this.ScrollPosition = function() {
		return (window.pageYOffset) ?
					window.pageYOffset
					: (document.documentElement && document.documentElement.scrollTop)
						? document.documentElement.scrollTop : document.body.scrollTop;
	}



	this.GetContainerQueryString = function(oContainer) {

	    var aElements = f.SafeObject(oContainer).getElementsByTagName('*');

	    var sQueryString = '';
	    for (var i = 0; i < aElements.length; i++) {
	        if (aElements[i].name) {
	            var bRadio = aElements[i].type && aElements[i].type.toUpperCase() == 'RADIO'

	            if (!bRadio) {
	                sQueryString += (sQueryString == '' ? '' : '&') + aElements[i].name + '='
	            }

	            if (aElements[i].nodeName == 'INPUT' && s.StartsWith(aElements[i].id, 'chk')) {
	                sQueryString += aElements[i].checked;

	            } else if (aElements[i].nodeName == 'INPUT' && bRadio) {
	                if (aElements[i].checked) {
	                    sQueryString += aElements[i].name + '=' + f.GetValue(aElements[i]);
	                }
	            } else if (aElements[i].nodeName == 'INPUT') {
	                sQueryString += s.EncodeString(f.GetValue(aElements[i]));

	            } else if (aElements[i].nodeName == 'SELECT') {
	                sQueryString += s.EncodeString(f.GetValue(aElements[i]) != '' ? dd.GetValue(aElements[i]) : dd.GetText(aElements[i]));
	            
	            } else if (aElements[i].nodeName == 'TEXTAREA') {
	                sQueryString += s.EncodeString(f.GetValue(aElements[i]));
	            }
	        }
	    }

	    return sQueryString;

	}

	this.GetRadioButtonValue = function(sName) {

		var aElements = document.body.getElementsByTagName('INPUT');
		var sReturn = '';

		for (var i = 0; i < aElements.length; i++) {
			if (aElements[i].name == sName && aElements[i].checked) {
				sReturn = aElements[i].value;
				break;
			}
		}

		return sReturn

	}

	this.SetRadioButtonValue = function(sName, sValue) {

		var aElements = document.body.getElementsByTagName('INPUT');

		for (var i = 0; i < aElements[i].length; i++) {
			if (aElements[i].name == sName && aElements[i].value == sValue) {
				aElements[i].checked = true;
				break;
			}
		}

	}



}

//checkbox functions 
function CheckBoxFunctions() {

	this.Checked = function(o) {
		o = f.SafeObject(o);
		if (o != null) {
			return o.checked;
		} else {
			return false;
		}
	}

	this.SetValue = function(o, sBoolean) {
		o = f.SafeObject(o);
		if (o != null) {
			if ((sBoolean + '').toLowerCase() == 'false') {
				o.checked = false;
			} else {
				o.checked = true;
			}
		} else {
			return false;
		}
	}

    this.Toggle = function(o) {
        o = f.SafeObject(o);
        if (o != null) {
            o.checked = !o.checked;
        }
    }
}



//dropdown functions
function DropdownFunctions() {

	this.GetText = function(o) {
		o = f.SafeObject(o);
		if (o != null && o.options.length > 0 && o.selectedIndex > -1) {
			return o.options[o.selectedIndex].text;
		} else {
			return '';
		}
	}

	this.GetIntText = function(o) {
		return n.SafeInt(dd.GetText(o));
	}


	this.GetValue = function(o) {
		o = f.SafeObject(o);
		if (o != null && o.selectedIndex >= 0) {
			return o.options[o.selectedIndex].value;
		} else {
			return '';
		}
	}

	this.GetIntValue = function(o) {
		return n.SafeInt(dd.GetValue(0));
	}

	this.GetIndex = function(o) {
		o = f.SafeObject(o);
		if (o != null) { return o.selectedIndex; }
	}

	this.ListCount = function(o) {
		o = f.SafeObject(o);
		if (o != null) { return o.options.length; }
	}

	this.SetIndex = function(o, iIndex) {
		o = f.SafeObject(o);
		if (o != null) { o.selectedIndex = iIndex; }
	}

	this.SetValue = function(o, iValue) {
		o = f.SafeObject(o);
		if (o != null && o.options) {
			for (var i = 0; i <= o.options.length - 1; i++) {
				if (o.options[i].value == iValue) {
					o.selectedIndex = i;
					break;
				}
			}
		}
	}

	this.SetText = function(o, sText) {
		var o = f.SafeObject(o);
		if (o != null) {
			for (var i = 0; i <= o.options.length - 1; i++) {
				if (o.options[i].text != null) {
					if (o.options[i].text == sText) {
						o.selectedIndex = i;
						break;
					}
				}
			}
		}
	}

	this.Clear = function(o, sText) {
		var o = f.SafeObject(o);
		if (o != null) {
			o.options.length = 0;
		}
	}

	this.AddOption = function(o, sText, iValue, sClass) {
		var o = f.SafeObject(o);
		if (o != null) {
			o.options[o.length] = new Option(sText, iValue);
			if (sClass != undefined) {
				o.options[o.length - 1].className = sClass;
			}
		}
	}

	this.SetOptions = function(o, sOptions) {
	    var o = f.SafeObject(o);
	    if (o != null) {
	        this.Clear(o);

	        var i = 0;
	        while (sOptions.split('#')[i] != undefined) {
	            var sOption = sOptions.split('#')[i];
	            if (sOption.indexOf('|') >= 0) {
	                o.options[i] = new Option(sOption.split('|')[0], sOption.split('|')[1]);
	            } else {
	                o.options[i] = new Option(sOptions.split('#')[i]);
	            }
	            i += 1;
	        }
	    }
	}
}

// checked data list
function CheckedDatalist(o) {

	this.List = f.SafeObject(o);

	this.HasCheckedItems = function() {

		var aCheckboxes = f.GetObjectsByIDPrefix(this.List.id + 'chk');

		for (var i = 0; i < aCheckboxes.length; i++) {
			if (f.SafeObject(aCheckboxes[i]).checked == true) {
				return true;
			}
		}

		return false;

	}

}

// validator
function Validator(oButton) {

	this.Validations = new Array();
	this.Button = f.SafeObject(oButton);


	this.AddValidation = function(Control, FieldName, ValidationType) {
		this.Validations[this.Validations.length] = ['Custom', Control, FieldName, ValidationType];
	}

	this.AddCustomValidation = function(Condition, Control, Message) {
		this.Validations[this.Validations.length] = ['Custom', Control, Message, 'CustomValidation', Condition]
	}

	this.Validate = function(bShowWarnings) {
		aValidation = this.Validations;
		if (bShowWarnings == undefined) {
			bShowWarnings = true;
		}

		if (this.Button) {
			ClientValidation(this.Button, 'Custom', undefined, bShowWarnings);
		} else {
			return ClientValidation(null, 'Custom', undefined, bShowWarnings);
		}
	}
}



//formfunctions
var ff = new function() {

    var me = this;

    /* safe param */
    this.SafeParam = function(sParam) {
        if (sParam.replace) {
            return sParam.replace(/\|/g, '/\\pipe\\/');
        } else if (sParam.getDate && sParam.getUTCFullYear && sParam.toDateString) { // attempt to find out whether sParam is a Date object
            return d.ToSQLDate(sParam);
        } else {
            return sParam;
        }
    }

    /* call */
    this.Call = function(FunctionName, CallBack) {

        //work out the params
        var sParams = '';
        for (var i = 2; i <= this.Call.arguments.length - 1; i++) {
            sParams += this.SafeParam(this.Call.arguments[i]) + '|'
        }
        if (sParams != '') { sParams = s.Chop(sParams); }

        //build up the url
        var sURL = window.location.href;
        if (s.Right(sURL, 1) == '#') {
            sURL = s.Chop(sURL);
        }
        sURL += '?executeformfunction';
        sURL += '&function=' + FunctionName;
        sURL += '&params=' + encodeURIComponent(sParams);

        //request
        var oRequest;
        if (window.XMLHttpRequest) {
            oRequest = new XMLHttpRequest();
            oRequest.open("POST", sURL, true);
        } else {
            oRequest = new ActiveXObject("Microsoft.XMLHTTP");
            oRequest.open("POST", sURL, true);
        }
        oRequest.onreadystatechange = function() {
            if (oRequest.readyState == 4) {
                if (oRequest.status != 200 && window.location.toString().indexOf('localhost') > -1) {
                    alert(oRequest.responseText);
                    return;
                }
                ff.Response(oRequest.responseText, CallBack);
            }
        }

        /* send parameter to stop firefox error on older versions */
        var sParam = '';
        oRequest.send(sParam);
    }



    /* response */
    this.Response = function(sResponse, oCallBack) {

        if (typeof (oCallBack) == 'string') {
            eval(oCallBack + '(\'' + s.Replace(sResponse, '\'', '\\\'') + '\')');
        } else if (typeof (oCallBack == 'function')) {
            oCallBack(sResponse);
        }
    }

}





//webservice
var oWebService = new WebService();
function WebService() {

	var oRequest, oResponseObject, bResponseTextOnly, oPopulateList, oHideDiv, SelectedID;

	//getlist
	this.PopulateList = function(URL, sNamespace, oList, SourceSQL, oDiv, iSelectedID) {

		oHideDiv = oDiv;
		SelectedID = iSelectedID;

		
		// get the data
		aParams = new Array(['SourceSQL', SourceSQL]);
		oPopulateList = f.SafeObject(oList);
		this.RunWebService(URL, sNamespace, 'GetList', aParams, oPopulateList);
	}

	this.FillList = function(oXML) {

		
		var bHasAll = (oPopulateList.options.length > 0 && oPopulateList.options[0].text == 'All');
		var bHasBlank = (oPopulateList.options.length > 0 && oPopulateList.options[0].text == '');
		var iOffset = 1;

		// clear the list
		dd.Clear(oPopulateList);

		//add all if required
		if (bHasAll) {
			oPopulateList[0] = new Option('All', -1);
		} else if (bHasBlank) {
			oPopulateList[0] = new Option('', 0);
		} else {
			iOffset = 0;
		}

		var oListItems = oXML.getElementsByTagName('ListItem');
		var sLastGroup = 'alanpartridge';
		var sValue;
		var iID;
		for (var i = 0; i < oListItems.length; i++) {

			sValue = this.GetNodeText(oListItems[i].childNodes[0]);
			iID = this.GetNodeText(oListItems[i].childNodes[1]);


			//if we're grouping 
			if (sValue.indexOf('~') > -1) {
				if (sValue.split('~')[0] != sLastGroup) {
					oPopulateList[i + iOffset] = new Option(sValue.split('~')[0], 0);
					oPopulateList.options[i + iOffset].className = 'dropdowngroup';
					iOffset += 1;
				}

				oPopulateList[i + iOffset] = new Option(sValue.split('~')[1], iID);
				sLastGroup = sValue.split('~')[0];
			} else {

				oPopulateList[i + iOffset] = new Option(sValue, iID);
			}
		}

		if (oHideDiv != undefined) {
			f.ShowIf(oHideDiv, oListItems.length > 0);
		}

		if (SelectedID != null && SelectedID != undefined) {
			dd.SetValue(oPopulateList, SelectedID);
		}
	}


	//support functions
	this.GetTagValue = function(oLocalXML, sTag) {
		var aItems = oLocalXML.getElementsByTagName(sTag);
		if (aItems.length == 1 && aItems[0].childNodes[0]) {
			if (aItems[0].textContent) {
				return aItems[0].textContent;
			} else {
				return aItems[0].childNodes[0].data;
			}
		} else {
			return '';
		}
	}


	this.GetNodeText = function(oNode) {
		return oNode.text ? oNode.text : oNode.textContent;
	}

	this.SafeParam = function(sParam) {
		if (sParam.replace) {
			return sParam.replace(/&/g, '&amp;');
		} else if (sParam.getDate && sParam.getUTCFullYear && sParam.toDateString) { // attempt to find out whether sParam is a Date object
			return d.ToSQLDate(sParam);
		} else {
			return sParam;
		}
	}


	this.CallServer = function(sURL, oCallback) {

		if (window.XMLHttpRequest) {
			oRequest = new XMLHttpRequest();
			oRequest.open("POST", sURL, true);
		} else {
			oRequest = new ActiveXObject("Microsoft.XMLHTTP");
			oRequest.open("POST", sURL, true);
		}

		oRequest.onreadystatechange = function() {
			if (oRequest.readyState == 4) {
				if (oRequest.status != 200 && window.location.toString().indexOf('localhost') > -1) {
					alert(oRequest.responseText);
					return;
				} else {
					oCallback(oRequest.responseText);
				}
			}
		}

		oRequest.send();

	}


	this.RunWebService = function(sUrl, sNamespace, sFunction, aParameters, oCallingObject, bTextOnly) {

		var sRequest =
			'<?xml version="1.0" encoding="utf-8"?>' +
			'<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' +
			'xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">' +
			'<soap:Body>' +
			'	<' + sFunction + ' xmlns="' + sNamespace + '">'

		for (var i = 0; i < aParameters.length; i++) {
			sRequest = sRequest + '<' + aParameters[i][0] + '>' +
				this.SafeParam(aParameters[i][1]) + '</' + aParameters[i][0] + '>';
		}

		sRequest = sRequest + '	</' + sFunction + '>' + '</soap:Body>' + '</soap:Envelope>';

		// branch for native XMLHttpRequest object
		if (window.XMLHttpRequest) {
			oRequest = new XMLHttpRequest();
			oRequest.open("POST", sUrl, true);
		} else {
			oRequest = new ActiveXObject("Microsoft.XMLHTTP");
			oRequest.open("POST", sUrl, true);
		}

		oResponseObject = oCallingObject;
		bResponseTextOnly = bTextOnly == undefined ? false : bTextOnly;

		oRequest.onreadystatechange = function() {
			if (oRequest.readyState == 4) {
				if (oRequest.status != 200 && window.location.toString().indexOf('localhost') > -1) {
					alert(oRequest.responseText);
					return;
				}

				if (oResponseObject == oPopulateList) {
					oWebService.FillList(oRequest.responseXML);
				} else if (bResponseTextOnly == false && oResponseObject.Done != undefined) {
					oResponseObject.Done(oRequest.responseXML);
				} else if (oResponseObject.Done != undefined) {
					oResponseObject.Done(oRequest.responseText);
				}
			}
		}

		oRequest.setRequestHeader("Content-Type", "text/xml;charset=UTF-8")
		oRequest.setRequestHeader("MessageType", "CALL")
		oRequest.setRequestHeader('SOAPAction', sNamespace + '/' + sFunction)
		oRequest.send(sRequest);

	}

    
}


function ListPopulater(oList, bSelectSingleOption) {

	var me = this;
	this.List = f.SafeObject(oList);
	this.Query = new WebService();
	this.SelectedID = 0;
	this.AddBlank = true;
	this.HideDiv = null;
	this.SelectSingleOption = (bSelectSingleOption == undefined ? false : bSelectSingleOption);

	this.Populate = function(sURL, sNamespace, sFunctionName, sSourceSQL, iSelectedID) {
		dd.Clear(me.List);
		aParams = new Array(['SourceSQL', sSourceSQL]);


		if (iSelectedID != undefined) {
			me.SelectedID = iSelectedID;
		} else {
			me.SelectedID = 0;
		}

		me.Query.RunWebService(sURL, sNamespace, sFunctionName, aParams, this.Query, false);
	}


	this.Query.Done = function(oXML) {
		var sOptions = me.Query.GetTagValue(oXML, 'BuildOptionsResult');

		if (me.AddBlank) {
			dd.AddOption(me.List, '', 0);
		}

		if (sOptions.length > 0) {
			var sLastGroup = 'Partridge';
			var aOptions = sOptions.split('#');
			for (var i = 0; i < aOptions.length; i++) {

				var aOption = aOptions[i].split('|');
				if (aOption[0].indexOf('~') > -1) {
					if (aOption[0].split('~')[0] != sLastGroup) {
						dd.AddOption(me.List, aOption[0].split('~')[0], 0, 'dropdowngroup');
					}
					dd.AddOption(me.List, aOption[0].split('~')[1], aOption[1]);
					sLastGroup = aOption[0].split('~')[0];
				} else {
					dd.AddOption(me.List, aOption[0], aOption[1]);
				}

			}
		}

		if (me.SelectedID > 0) {
			dd.SetValue(me.List, me.SelectedID);
		}

		if (me.SelectSingleOption && me.SelectedID == 0 && sOptions.split('#').length == 1) {

			//if grouping then index 2 else 1
			if (sLastGroup != 'Partridge') {
				dd.SetIndex(me.List, 2);
			} else {
				dd.SetIndex(me.List, 1);
			}
		}

		if (me.List.onchange) {
			me.List.onchange();
		}


		if (me.Populated) {
			me.Populated();
		}

		if (me.HideDiv != null) {
			f.ShowIf(me.HideDiv, sOptions.split('#').length > 1);
		}
	}

    this.BuildList = function(oList, sOptions) {

        var aOptions = sOptions.split('#');
        var iSelected = 0;
        for (var i = 0; i < aOptions.length; i++) {
            var aOption = aOptions[i].split('|');

            if (s.StartsWith(aOption[0], '**')) {
                aOption[0] = s.Substring(aOption[0], 2);
                iSelected = aOption[1];
            }
            dd.AddOption(oList, aOption[0], aOption[1]);
        }

        if (iSelected > 0) { dd.SetValue(oList, iSelected); }
    }

}



/* effects */
function Effects() {

	this.SetOpacity = function(o, iOpacity) {
		var oControl = f.SafeObject(o);
		oControl.style.opacity = iOpacity / 100;
		oControl.style.filter = 'alpha(opacity=' + iOpacity + ')';
	}


	this.SlideOpen = function(oObject, SlideTime, FinalHeight, EndTime) {

		oObject = f.SafeObject(oObject);
		SlideTime = SlideTime == undefined ? 0.75 : SlideTime;

		if (EndTime == undefined) {
			oObject.style.overflow = 'hidden';
			oObject.style.display = 'block';
			oObject.style.height = '1px';
			oObject.style.height = 'auto';
			FinalHeight = oObject.scrollHeight;
			var dStart = new Date();
			EndTime = new Date(dStart.getTime() + (SlideTime * 1000));
		} else {
			EndTime = new Date(EndTime);
		}

		if (new Date() < EndTime) {
			oObject.style.height = Math.round(Math.sin(Math.PI / 2 * (1 - (EndTime - new Date()) / 1000 / SlideTime)) * FinalHeight) + 'px'
			setTimeout('e.SlideOpen(\'' + oObject.id + '\',' + SlideTime + ',' + FinalHeight + ',\'' + EndTime + '\')', 10);
		} else {
			oObject.style.height = FinalHeight + 'px';
		}
	}

	this.SlideClose = function(oObject, SlideTime, FinalHeight, EndTime) {

		oObject = f.SafeObject(oObject);
		SlideTime = SlideTime == undefined ? 0.75 : SlideTime;

		if (EndTime == undefined) {
			FinalHeight = oObject.scrollHeight;
			var dStart = new Date();
			EndTime = new Date(dStart.getTime() + (SlideTime * 1000));
		} else {
			EndTime = new Date(EndTime);
		}

		if (new Date() < EndTime) {
			oObject.style.height = Math.round(Math.sin(Math.PI / 2 * ((EndTime - new Date()) / 1000 / SlideTime)) * FinalHeight) + 'px'
			setTimeout('e.SlideClose(\'' + oObject.id + '\',' + SlideTime + ',' + FinalHeight + ',\'' + EndTime + '\')', 10);
		} else {
			oObject.style.height = 0;
			oObject.style.display = 'none';
		}
	}


	this.ScrollIntoView = function(Object, Padding, ScrollTime) {

	    oObject = f.SafeObject(Object);
	    Padding = Padding == undefined ? 0 : Padding;
		ScrollTime = ScrollTime == undefined ? 2 : ScrollTime;

		var oBrowserDimensions = new e.BrowserDimensions();
		var oObjectPosition = e.GetPosition(oObject);


		var iObjectTop = oObjectPosition.Top;
		var iObjectBottom = oObjectPosition.Top + oObjectPosition.Height;
		var iObjectHeight = oObjectPosition.Height;
		var iViewportTop = oBrowserDimensions.ScrollYPos;
		var iViewportBottom = oBrowserDimensions.ScrollYPos + oBrowserDimensions.ViewportHeight;
		var iViewportHeight = oBrowserDimensions.ViewportHeight;


		var dStart = new Date();
		var dEndTime = new Date(dStart.getTime() + (ScrollTime * 1000));


		if (iObjectHeight > iViewportHeight && iObjectTop < iViewportTop && iObjectBottom > iViewportBottom) {
			return;

		} else if (iObjectTop < iViewportTop) {
		    e.ScrollToObject(oObject, ScrollTime, iViewportTop, iObjectTop - Padding, dEndTime);

		} else if (iObjectBottom > iViewportBottom) {
			e.ScrollToObject(oObject, ScrollTime, iViewportTop, iObjectBottom - iViewportHeight + Padding, dEndTime);
		}

	}


	this.ScrollToObject = function(Object, ScrollTime, StartPosition, FinalPosition, EndTime) {

		oObject = f.SafeObject(Object);
		ScrollTime = ScrollTime == undefined ? 2 : ScrollTime;

		if (EndTime == undefined) {
			var oBrowserDimensions = new e.BrowserDimensions();
			StartPosition = oBrowserDimensions.ScrollYPos;
			FinalPosition = e.GetPosition(oObject).Top;

			//            if (FinalPosition + oBrowserDimensions.ViewportHeight > oBrowserDimensions.PageHeight) {
			//                FinalPosition = oBrowserDimensions.PageHeight - oBrowserDimensions.ViewportHeight;
			//            }

			var dStart = new Date();
			EndTime = new Date(dStart.getTime() + (ScrollTime * 1000));
		} else {
			EndTime = new Date(EndTime);
		}

		if (new Date() < EndTime) {
			var nFractionNotDone = (EndTime - new Date()) / (ScrollTime * 1000);
			// uses a combination of cosine and power for the multiplier so it starts smooth and ends smooth, but goes faster when it starts than when it ends...
			var nMultiplier = 0.5 + Math.cos(Math.PI * Math.pow(nFractionNotDone, 2)) / 2;
			var iScrollPosition = Math.round(StartPosition + nMultiplier * (FinalPosition - StartPosition));

			if (window.pageYOffset) {
				window.scrollTo(0, iScrollPosition);
			} else if (document.documentElement) {
				document.documentElement.scrollTop = iScrollPosition;
			} else {
				document.body.scrollTop = iScrollPosition;
			}

			setTimeout('e.ScrollToObject(\'' + oObject.id + '\',' + ScrollTime + ',' + StartPosition + ',' + FinalPosition + ',\'' + EndTime + '\')', 10);
		} else if (window.pageYOffset) {
			window.scrollTo(0, FinalPosition);
		} else if (document.documentElement) {
			document.documentElement.scrollTop = FinalPosition;
		} else {
			document.body.scrollTop = FinalPosition;
		}

	}


	this.FadeOut = function(oObject, FadeTime, Opacity) {
		this.FadeOutObject = f.SafeObject(oObject);
		FadeTime = FadeTime == undefined ? 1 : FadeTime;
		this.FadeInterval = FadeTime / 20 * 800;
		this.Opacity = Opacity == undefined ? 100 : Opacity;

		this.Opacity -= 5;

		if (this.Opacity < 0) {
			e.SetOpacity(this.FadeOutObject, 0);
		} else {
			e.SetOpacity(this.FadeOutObject, this.Opacity);
			setTimeout('e.FadeOut(\'' + this.FadeOutObject.id + '\',' + FadeTime + ',' + this.Opacity + ')',
				this.FadeInterval);
		}
	}

	this.FadeIn = function(oObject, FadeTime, Opacity) {
		this.FadeInObject = f.SafeObject(oObject);
		FadeTime = FadeTime == undefined ? 1 : FadeTime;
		this.FadeInterval = FadeTime / 20 * 800;
		this.Opacity = Opacity == undefined ? 0 : Opacity;

		this.Opacity += 5;

		if (this.Opacity > 100) {
			e.SetOpacity(this.FadeInObject, 100);
		} else {
			e.SetOpacity(this.FadeInObject, this.Opacity);
			setTimeout('e.FadeIn(\'' + this.FadeInObject.id + '\',' + FadeTime + ',' + this.Opacity + ')',
				this.FadeInterval);
		}
	}



    this.ImageRotator = function(IDBase, ItemCount, RotateTime, CurrentIndex) {

        if (ItemCount > 1) {
              RotateTime = RotateTime == undefined ? 2 : parseInt(RotateTime);
              CurrentIndex = CurrentIndex == undefined ? 0 : CurrentIndex;

              if (CurrentIndex == 0) {

                    CurrentIndex = 1;

                    var oFirst = f.GetObject(IDBase + CurrentIndex);
                    oFirst.style.zIndex = 100;
                    
              } else {

                    var oFadeOut = f.GetObject(IDBase + CurrentIndex);
                    if (oFadeOut == null) {
                          return false;
                    }

                    CurrentIndex += 1;
                    CurrentIndex = CurrentIndex > ItemCount ? 1 : CurrentIndex;

                    var oFadeIn = f.GetObject(IDBase + CurrentIndex);

                    e.FadeOut(oFadeOut);
                    e.FadeIn(oFadeIn);
                    
                    oFadeOut.style.zIndex = 0;
                    oFadeIn.style.zIndex = 100;
                    
              }

              setTimeout('e.ImageRotator(\'' + IDBase + '\',' + ItemCount + ',' + RotateTime + ',' + CurrentIndex + ')', RotateTime * 1000);
        }

    }

    this.GetPosition = function(o) {
      var oControl = f.SafeObject(o);

      var s = oControl.id;

      var iLeft = 0, iTop = 0, iWidth, iHeight, iScrollLeft, iScrollTop;
      iWidth = oControl.offsetWidth;
      iHeight = oControl.offsetHeight;

      if (oControl.offsetParent) {
          iLeft = oControl.offsetLeft;
          iTop = oControl.offsetTop;
          while (oControl = oControl.offsetParent) {
              iScrollLeft = (oControl.offsetParent && oControl.scrollLeft > 0 ? oControl.scrollLeft : 0)
              iScrollTop = (oControl.offsetParent && oControl.scrollTop > 0 ? oControl.scrollTop : 0)
              iLeft += oControl.offsetLeft - iScrollLeft;
              iTop += oControl.offsetTop - iScrollTop;
          }
      }

      return new this.Position(iLeft, iTop, iWidth, iHeight);
    }

	this.SetPosition = function(o, oPosition) {
		oControl = f.SafeObject(o);
		oControl.style.top = oPosition.Top + 'px';
		oControl.style.left = oPosition.Left + 'px';
		oControl.style.width = oPosition.Width + 'px';
		if (oPosition.Height > 0) {
			oControl.style.height = oPosition.Height + 'px';
		}
	}


	this.SetTopLeft = function(o, iTop, iLeft) {
		oControl = f.SafeObject(o);
		oControl.style.top = iTop + 'px';
		oControl.style.left = iLeft + 'px';
	}

	this.Position = function(iLeft, iTop, iWidth, iHeight) {
		this.Left = iLeft;
		this.Top = iTop;
		this.Width = iWidth;
		this.Height = iHeight;
	}


	this.BrowserDimensions = function() {


		var iXScroll, iYScroll;

		if (document.documentElement != undefined && document.documentElement.scrollHeight) {
		    iXScroll = document.documentElement.scrollWidth;
		    iYScroll = document.documentElement.scrollHeight;
		} else if (window.innerHeight && window.scrollMaxY) {
			iXScroll = window.innerWidth + window.scrollMaxX;
			iYScroll = window.innerHeight + window.scrollMaxY;
		} else if (document.body.scrollHeight > document.body.offsetHeight) {
			iXScroll = document.body.scrollWidth;
			iYScroll = document.body.scrollHeight;
		} else {
			iXScroll = document.body.offsetWidth;
			iYScroll = document.body.offsetHeight;
		}


		var iWindowWidth, iWindowHeight;

		if (self.innerHeight) {
			if (document.documentElement.clientWidth) {
				iWindowWidth = document.documentElement.clientWidth;
			} else {
				iWindowWidth = self.innerWidth;
			}
			iWindowHeight = self.innerHeight;
		} else if (document.documentElement && document.documentElement.clientHeight) {
			iWindowWidth = document.documentElement.clientWidth;
			iWindowHeight = document.documentElement.clientHeight;
		} else if (document.body) {
			iWindowWidth = document.body.clientWidth;
			iWindowHeight = document.body.clientHeight;
		}


		var iXScrollPos, iYScrollPos;

		if (window.pageYOffset && window.pageXOffset) {
			iXScrollPos = window.pageXOffset
			iYScrollPos = window.pageYOffset
		} else if (document.documentElement) {
			iXScrollPos = document.documentElement.scrollLeft;
			iYScrollPos = document.documentElement.scrollTop;
		} else {
			iXScrollPos = document.body.scrollLeft;
			iYScrollPos = document.body.scrollTop;
		}

		if (window.pageYOffset) {
			iYScrollPos = window.pageYOffset
		} else if (document.documentElement) {
			iYScrollPos = document.documentElement.scrollTop;
		} else {
			iYScrollPos = document.body.scrollTop;
		}


		var iPageHeight, iPageWidth
		var iPageHeight = iYScroll < iWindowHeight ? iWindowHeight : iYScroll;
		var iPageWidth = iXScroll < iWindowWidth ? iXScroll : iWindowWidth;


		this.ViewportWidth = iWindowWidth;
		this.ViewportHeight = iWindowHeight;
		this.PageWidth = iPageWidth;
		this.PageHeight = iPageHeight;
		this.PageScrollTop = iYScroll;
		this.ScrollYPos = iYScrollPos;
		this.ScrollXPos = iXScrollPos;

	}



	this.CreateOverlay = function(oOpacity, oContainer) {

		var oOverlay = document.createElement('div');
		oOverlay.setAttribute('id', 'divOverlay');

		if (oContainer == undefined) {
			document.body.appendChild(oOverlay);
		} else {
			oContainer.appendChild(oOverlay);
		}

		
		var oDimensions = new e.BrowserDimensions();
		oOverlay.style.height = oDimensions.PageHeight + 'px';
		oOverlay.style.left = oDimensions.ScrollXPos + 'px';
		oOverlay.style.width = '100%';

		if (oOpacity == undefined) {
			e.SetOpacity(oOverlay, 60);
		} else {
			e.SetOpacity(oOverlay, n.SafeInt(oOpacity));
		}

		return oOverlay;
	}



	//if ie6, hide/show the dropdowns!
	var ToggleDropdownVisibility = new function() {

		//hide
		this.Hide = function() {
			if (b.IE6()) {
				var aSelect = document.getElementsByTagName('select');
				for (var i = 0; i < aSelect.length; i++) {
					aSelect[i].style.visibility = 'hidden';

				}
			}
		}


		//show
		this.Show = function() {
			if (b.IE6()) {
				var aSelect = document.getElementsByTagName('select');
				for (var i = 0; i < aSelect.length; i++) {
					aSelect[i].style.visibility = 'visible';

				}
			}
		}
	}



	/* modal popup */
	this.ModalPopup = new function() {

		this.PopupDiv;
		this.ContainerDiv;
		this.EscapeEvent;

		this.Show = function(oDiv, oParent, sTitle) {
			e.ModalPopup.Close();

			ToggleDropdownVisibility.Hide();

			this.PopupDiv = f.SafeObject(oDiv);
			
			//if we've been drawn from the modal popup control we will have a container div
			this.ContainerDiv = f.SafeObject(this.PopupDiv.id + '_Container');

			if (this.ContainerDiv != null) {

				//ensure we are outside of div all
				document.forms[0].appendChild(this.ContainerDiv);
				e.CreateOverlay(60, this.ContainerDiv);
				f.Show(this.ContainerDiv);
			} else {

				e.CreateOverlay();
				f.Show(this.PopupDiv);
			}

			this.UpdateScreenPosition(oParent);

			if (sTitle != undefined) {
				f.SetHTML('h3' + this.PopupDiv.id, sTitle);
			}

			e.ModalPopup.EscapeEvent = f.AttachEvent(document, 'keypress',
				function(oEvent) {
					if (f.GetKeyCodeFromEvent(oEvent) == 27) {
						e.ModalPopup.Close();
					}
				});
		}


		this.Close = function() {
			if (f.Visible(e.ModalPopup.PopupDiv)) {

				if (this.ContainerDiv != null) {

					f.Hide(e.ModalPopup.ContainerDiv);
					if (f.GetObject('divOverlay') != null) {
						this.ContainerDiv.removeChild(f.GetObject('divOverlay'));
					}
					
				} else {

					f.Hide(e.ModalPopup.PopupDiv);
					if (f.GetObject('divOverlay') != null) {
						document.body.removeChild(f.GetObject('divOverlay'));
					}					
				}
				
				f.DetachEvent(e.ModalPopup.EscapeEvent);
				ToggleDropdownVisibility.Show();


			}
		}


		this.UpdateScreenPosition = function(oParent) {

			var iControlWidth = this.PopupDiv.offsetWidth;
			var iControlHeight = this.PopupDiv.offsetHeight;

			var oDimensions = new e.BrowserDimensions();
			var iLeft;
			if (oParent == undefined) {
				iLeft = (oDimensions.ViewportWidth - iControlWidth) / 2 + oDimensions.ScrollXPos;
			} else {
				oParentPosition = e.GetPosition(oParent);
				iLeft = oParentPosition.Left + (oParentPosition.Width - iControlWidth) / 2;
			}

			var iTop = (oDimensions.ViewportHeight / 3) - (iControlHeight / 2);

			//set min top
			iTop = iTop < 50 ? 50 : iTop;
			iTop += oDimensions.ScrollYPos;


			this.PopupDiv.style.left = iLeft + 'px';
			this.PopupDiv.style.top = iTop + 'px';


			this.CheckOverlayIsTallEnough();
		}


		this.CheckOverlayIsTallEnough = function() {

			var oOverlay = f.GetObject('divOverlay');
			var oDimensions = new e.BrowserDimensions();
			var iPopupBottom = this.PopupDiv.offsetTop + this.PopupDiv.offsetHeight;

			if (iPopupBottom + 20 > oDimensions.PageHeight) {
				oOverlay.style.height = iPopupBottom + 20 + 'px';
			}

		}
	}
}



/* datalist functions */
function DataListFunctions() {

	this.SetSelected = function(ListID, ID) {
		//work out the id prefix
		var oHolder = f.GetObject(ListID + 'Scroll');
		var sBaseID = oHolder.childNodes[0].nodeName == '#text' ? oHolder.childNodes[1].id : oHolder.childNodes[0].id;

		var oRows = f.GetObjectsByIDPrefix(sBaseID, 'tr');
		for (var i = 0; i < oRows.length; i++) {
			if (oRows[i].id == sBaseID + '_' + ID) {
				f.AddClass(oRows[i], 'selected');
			} else {
				f.RemoveClass(oRows[i], 'selected');
			}
			//f.AddClass(oRows[i], oRows[i].id == sBaseID + '_' + ID ? 'selected' : '');
		}
	}

}





/* cookie functions */
function CookieFunctions() {

	this.Set = function(sName, sValue, iDays) {

		var sExpires;
		if (iDays) {
			var dDate = new Date();
			dDate.setTime(dDate.getTime() + (iDays * 24 * 60 * 60 * 1000));
			sExpires = '; expires=' + dDate.toGMTString();
		} else {
			sExpires = '';
		}
		document.cookie = sName + '=' + sValue + sExpires + '; path=/';
	}

	this.Get = function(sName) {
		var sNameEQ = sName + '=';
		var aCookies = document.cookie.split(';');
		for (var i = 0; i < aCookies.length; i++) {
			var sCookie = aCookies[i];
			while (sCookie.charAt(0) == ' ') sCookie = sCookie.substring(1, sCookie.length);
			if (sCookie.indexOf(sNameEQ) == 0) {
				return sCookie.substring(sNameEQ.length, sCookie.length);
			}
		}
		return '';
	}

	this.Delete = function(sName) {
		c.Set(sName, '', -1);
	}

}


/* browser functions */
function BrowserFunctions() {

	this.BrowserName = function() {
		return navigator.appName
	}

	this.BrowserVersion = function() {
		return navigator.appVersion
	}

	this.IE = function() {
		return this.BrowserName() == 'Microsoft Internet Explorer'
	}

	this.IE6 = function() {
	    return this.IE() && this.BrowserVersion().indexOf('MSIE 6') > 0;
	}

}