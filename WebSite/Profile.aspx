<%@ Page Title="" Language="C#" MasterPageFile="~/DevSpace.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="DevSpace.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
	<link href="styles/Profile.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="js/knockout-3.3.0.js"></script>
	<script type="text/javascript" src="js/Profile.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
	<div id="Profile">
		<h1>Profile</h1>
		<h2>Display Name</h2>
		<input type="text" data-bind="value: Profile().DisplayName" />
		<h2>Email Address</h2>
		<input type="text" readonly="readonly" data-bind="value: Profile().EmailAddress" />
		<h2>Twitter</h2>
		<input type="text" data-bind="value: Profile().Twitter" />
		<h2>Website</h2>
		<input type="text" data-bind="value: Profile().Website" />
		<h2>Bio</h2>
		<textarea data-bind="value: Profile().Bio"></textarea>
		<input type="button" data-bind="click: SaveProfile" value="SaveProfile" />
		<h1>Sessions</h1>
		<table>
			<tbody id="SessionList" data-bind="foreach: Sessions">
				<tr>
					<td class="Title" data-bind="text: Title"></td>
					<td><img src="images/Edit.png" alt="Edit" data-bind="click: $root.ShowSession" /></td>
					<td><img src="images/Trash.png" alt="Delete" data-bind="click: $root.DeleteSession" /></td>
				</tr>
			</tbody>
		</table>
		<input type="button" data-bind="click: function () { ShowSession(null) }" value="New Session" />
	</div>
	<div id="Session" style="display: none;">
		<h1>Session</h1>
		<h2>Title</h2>
		<input type="text" data-bind="value: SelectedSession().Title" />
		<h2>Abstract</h2>
		<textarea data-bind="value: SelectedSession().Abstract"></textarea>
		<h2>Notes to Reviewer</h2>
		<textarea data-bind="value: SelectedSession().Notes"></textarea>
		<h2>Tags</h2>
		<input type="text" readonly="readonly" data-bind="value: SelectedSession().TagList" />
		<h2>Tag List (Click to Add or Remove)</h2>
		<ul data-bind="foreach: Tags">
			<li data-bind="text: Text, click: $root.AddOrRemoveTagToSession" />
		</ul>
		<h3>- OR -</h3>
		<h2>New Tag</h2>
		<input type="text" id="NewTagText" style="width: 50%; display: inline-block;" />
		<input type="button" value="Add New" style="width: 35%; display: inline-block;" data-bind="click: SaveTag" />
		<input type="button" data-bind="click: SaveSession" value ="Save" />
		<input type="button" data-bind="click: ShowProfile" value="Cancel" />
	</div>
</asp:Content>
