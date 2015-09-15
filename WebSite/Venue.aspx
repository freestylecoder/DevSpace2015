<%@ Page Title="" Language="C#" MasterPageFile="~/DevSpace.Master" AutoEventWireup="true" CodeBehind="Venue.aspx.cs" Inherits="DevSpace.Venue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
	<style>
		#Map {
			top: 100px;
			right: 350px;
			width: 300px;
			height: 300px;
			overflow: hidden;
			margin-left: 50px;
			position: absolute;
		}

		section article {
			margin-right: 400px;
		}

		@media only screen and (max-width: 1120px) {
			#Map {
				top: auto;
				right: auto;
				width: 300px;
				position: relative;
				margin-left: auto;
				margin-right: auto;
			}

			section article {
				margin-right: 50px;
			}
		}
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
	<section>
		<article>
			<h1>Our Venue</h1>
			<h2>
				Von Braun Center<br />
				700 Monroe Street<br />
				Huntsville, AL 35801
			</h2>
			<p>DevSpace is taking place at The Von Braun Center. Details on the room block can be found on the Travel page.</p>
			<div id="Map"><div id="gmap_canvas" style="height:300px;width:300px;"><iframe style="height:300px;width:300px;border:0;" frameborder="0" src="https://www.google.com/maps/embed/v1/place?q=Von+Braun+Center,+Huntsville,+AL,+United+States&key=AIzaSyAN0om9mFmy1QN6Wf54tXAowK4eT0ZUPrU"></iframe></div><a class="google-map-code" href="http://track-chat.com" id="get-map-data">http://track-chat.com</a><style>#gmap_canvas img{max-width:none!important;background:none!important}</style><script src="https://www.embed-map.com/google-maps-authorization.js?id=95155482-bb71-5c10-8351-151358ee3b8f" defer="defer" async="async"></script></div>
		</article>
	</section>
</asp:Content>
