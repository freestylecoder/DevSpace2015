<%@ Page Title="" Language="C#" MasterPageFile="~/DevSpace.Master" AutoEventWireup="true" CodeBehind="Speakers.aspx.cs" Inherits="DevSpace.Speakers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
	<link href="styles/Speakers.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
	<section>
		<article>
			<h1>Call for Speakers Now Open</h1>
			<p>Our call for speakers is officially open. We will be accepting submissions until July 29th. Speakers will be notified on August 5th. Speakers will be announced on August 10th.</p>
			<p>To submit your sessions, click the giant, gawdy button below.</p>
			<input type="button" onclick="window.location = './Login.aspx';" value="Call For Speakers" />
			<h3>FAQ</h3>
			<h2>What topics are you looking for?</h2>
			<p>
				We want DevSpace to be about community.
				The conversation is almost as important as the content.
				As such, we want speakers and attendees from every area.
				This shouldn't be limited to development technologies.
				Talks on design, management, planning, personal improvement, and anything else you can think up will be accepted.
			</p>
			<h2>What incentives will I get as a speaker?</h2>
			<p>
				As a first year non-profit, there is not much I can offer.
				I will promise you a free ticket to the event.
				I will also offer you free breakfast, lunch, and snacks.
				Unfortunately, that is all I can promise.
				If a sponsor steps up, I would like to have one dinner, as well.
				Anything else will be decided by the remaining budget as we approach the event.
			</p>
		</article>
	</section>
</asp:Content>
