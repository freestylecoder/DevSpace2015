<%@ Page Title="" Language="C#" MasterPageFile="~/DevSpace.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="DevSpace.Tickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
	<section>
		<article>
			<h1>Tickets on sale</h1>
			<p>
				Tickets for DevSpace are now on sale.
				The Super Early Bird tickets will be on sale until we either sell all 50 or announce the keynote speaker.
				The Early Bird tickets will be on sale until we either sell all 100 or announce the full speaker and session lineup.
				Standard tickets will be on sale until the Wednesday before the event.
			</p>
			<p>
				After you purchase your tickets, you will have an option to include lunches.
				Lunches are free as long as Super Early Bird tickets are available.
				Once we transistion into Early Bird tickets, lunches will be $5 for both days.
				Finally, lunches will be $10 for both days during Standard ticket sales.
			</p>
			<p>
				Tickets are being processed through EventBrite.
				To view the full event details and tickets FAQ (or, if the widget is hidden do to screen size,) please visit our EventBrite page using the link below.
				Please note that the link will open in a new tab / window.
			</p>
			<h1><a href="https://www.eventbrite.com/e/devspace-2015-registration-17584925987" target="EventBrite">Event Page on EventBrite.</a></h1>
		</article>

		<div id="EventBriteTickets">
			<iframe src="//eventbrite.com/tickets-external?eid=17584925987&ref=etckt" frameborder="0" height="340" width="100%" vspace="0" hspace="0" marginheight="5" marginwidth="5" scrolling="auto" allowtransparency="true"></iframe>
			<div style="font-family: Helvetica, Arial; font-size: 10px; padding: 5px 0 5px; margin: 2px; width: 100%; text-align: left;">
				<a class="powered-by-eb" style="color: #dddddd; text-decoration: none;" target="_blank" href="http://www.eventbrite.com/r/etckt">Powered by Eventbrite</a>
			</div>
		</div>
	</section>
</asp:Content>
