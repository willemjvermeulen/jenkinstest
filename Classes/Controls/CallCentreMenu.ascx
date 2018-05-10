<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CallCentreMenu.ascx.vb" Inherits="Serenity.CallCentreMenu" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Namespace="Intuitive.WebControls" TagPrefix="int" Assembly="Intuitive"%>
<int:frameholder id="hldCallBookNav" title="Booking Menu" runat="server">
	<div id="divBookingMenu">
		<ul class="menu">
			<li class="menusection" id="mnuBooking" runat="server">
				<div class="menusectionheader">
					<a onclick="javascript:menusection(this, '#BaseUrl#/Secure/CallCentre/BookingDetails.aspx?BookingID=12');">
						Booking
					</a>
					<ul>
						<li><strong>Reference</strong>12345</li>
						<li><strong>Status</strong>Booking</li>
						<li><strong>Source</strong>Virgin Holidays</li>
						<li><strong>Total</strong>£1,780.60</li>
					</ul>
				</div>
				<ul class="menusectionbody">
					<li id="BookingDetails" runat="server"><a href="#BaseUrl#/Secure/CallCentre/BookingDetails.aspx?BookingID=12">Booking Details</a></li>
					<li id="LeadGuest" runat="server"><a href="#BaseUrl#/Secure/CallCentre/LeadGuest.aspx">Lead Guest</a></li>
					<li id="BookingComment" runat="server"><a href="#BaseUrl#/Secure/CallCentre/BookingComments.aspx">Booking Comments</a></li>
					<li id="Payment" runat="server"><a href="#BaseUrl#/Secure/CallCentre/Payment.aspx">Payment</a></li>
					<li id="AddLink" class="add" runat="server"><a href="#BaseUrl#/Secure/CallCentre/PropertyBooking/BookingDetails.aspx?BookingDetailID=-1">Add Hotel Booking</a></li>
					<li id="AddPackageLink" class="add" runat="server"><a href="#BaseUrl#/Secure/CallCentre/PackageBooking/BookingDetails.aspx?BookingDetailID=-1">Add Package Booking</a></li>
				</ul>
			</li>
			<li class="menusection" id="mnuProperty" runat="server">
				<div class="menusectionheader">
				<a onclick="javascript:menusection(this, '#BaseUrl#/Secure/CallCentre/PropertyBooking/BookingDetails.aspx');">
						Hotel Booking
					</a>
					<ul>
						<li><strong>Reference</strong>12345A</li>
						<li><strong>Property</strong>Windjammer</li>
						<li><strong>Arrival Date</strong>07 Jul '05</li>
						<li><strong>Duration</strong>14 Nights</li>
					</ul>
				</div>
				<ul class="menusectionbody">
					<li id="PropertyBookingDetails" runat="server" class="complete"><a href="#BaseUrl#/Secure/CallCentre/PropertyBooking/BookingDetails.aspx">Booking Details</a></li>
					<li id="PropertyContractSelection" runat="server"><a href="#BaseUrl#/Secure/CallCentre/PropertyBooking/ContractSelection.aspx">Contract Selection</a></li>
					<li id="PropertyRates" runat="server"  class="error"><a href="#BaseUrl#/Secure/CallCentre/PropertyBooking/Rates.aspx">Rates</a></li>
					<li id="PropertyGuestDetails" runat="server"><a href="#BaseUrl#/Secure/CallCentre/PropertyBooking/GuestDetails.aspx">Guest Details</a></li>
					<li id="PropertyPackages" runat="server" ><a href="#BaseUrl#/Secure/CallCentre/PropertyBooking/Packages.aspx">Packages</a></li>
					<li id="PropertyErrata" runat="server" class="error"><a href="#BaseUrl#/Secure/CallCentre/PropertyBooking/Errata.aspx">Errata</a></li>
					<li id="PropertyBookingComments" runat="server"><a href="#BaseUrl#/Secure/CallCentre/PropertyBooking/BookingComments.aspx">Booking Comments</a></li>
					<li id="PropertyLinkedBookings" runat="server"><a href="#BaseUrl#/Secure/CallCentre/PropertyBooking/LinkedBookings.aspx">Linked Bookings</a></li>
					<li id="PropertyConfirmation" runat="server"><a href="#BaseUrl#/Secure/CallCentre/PropertyBooking/Confirmation.aspx">Confirmation</a></li>
				</ul> 
			</li>
			
			<li class="menusection" id="mnuPackage" runat="server">
				<div class="menusectionheader">
				<a onclick="javascript:menusection(this, '#BaseUrl#/Secure/CallCentre/PackageBooking/BookingDetails.aspx');">
						Package Booking
					</a>
					<ul>
						<li><strong>Reference</strong>12345B</li>
						<li><strong>Package</strong>Wind Surfing</li>
					</ul>
				</div>
				<ul class="menusectionbody">
					<li id="PackageBookingDetails" runat="server"><a href="#BaseUrl#/Secure/CallCentre/PackageBooking/BookingDetails.aspx">Booking Details</a></li>
					<li id="PackageBookingComments" runat="server"><a href="#BaseUrl#/Secure/CallCentre/PackageBooking/BookingComments.aspx">Booking Comments</a></li>
					<li id="PackageLinkedBookings" runat="server"><a href="#BaseUrl#/Secure/CallCentre/PackageBooking/LinkedBookings.aspx">Linked Bookings</a></li>
				</ul>
			</li>
			
		</ul>	
	</div>
</int:frameholder>