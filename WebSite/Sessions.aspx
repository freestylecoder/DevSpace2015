<%@ Page Title="" Language="C#" MasterPageFile="~/DevSpace.Master" AutoEventWireup="true" CodeBehind="Sessions.aspx.cs" Inherits="DevSpace.Sessions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
	<script type="text/javascript" src="js/knockout-3.3.0.js"></script>
	<script type="text/javascript" src="js/Sessions.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
	<section data-bind="foreach: Sessions">
		<article>
			<h1 data-bind="text: Title"></h1>
			<p data-bind="html: Abstract"></p>

			<h2 data-bind="text: TimeSlot"></h2>
			<h2>Speaker: <a data-bind="text: Speaker().DisplayName, attr: { href: Speaker().Link }"></a></h2>
			<h2>Tags: <!-- ko foreach: Tags --><a data-bind="text: Text, attr: { href: Link }"></a>; <!-- /ko --></h2>
		</article>
	</section>
</asp:Content>
