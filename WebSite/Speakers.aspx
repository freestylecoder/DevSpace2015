<%@ Page Title="" Language="C#" MasterPageFile="~/DevSpace.Master" AutoEventWireup="true" CodeBehind="Speakers.aspx.cs" Inherits="DevSpace.Speakers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
	<link href="styles/Speakers.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="js/knockout-3.3.0.js"></script>
	<script type="text/javascript" src="js/Speakers.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
	<section data-bind="foreach: Profiles">
		<article>
			<h1 data-bind="text: DisplayName"></h1>
			<p data-bind="text: Bio"></p>
			<!-- ko if: Twitter || Website -->
			<p>
				<!-- ko if: Twitter -->
				<a target="_blank" data-bind="text: Twitter, attr: { href: TwitterLink }"></a>
				<!-- /ko -->
				<!-- ko if: Website -->
				<!-- ko if: Twitter -->
				<br />
				<!-- /ko -->
				<a target="_blank" data-bind="text: Website, attr: { href: Website }"></a>
				<!-- /ko -->
			</p>
			<!-- /ko -->
			<h1>Sessions</h1>
			<p data-bind="foreach: Sessions">
				<span><a data-bind="text: Title, attr: { href: Link }"></a></span><br />
			</p>
		</article>
	</section>
</asp:Content>
