<%@ Page Title="" Language="C#" MasterPageFile="~/DevSpace.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DevSpace.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
	<link href="styles/Login.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="js/Login.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
	<h4 id="ErrorMessage" style="display: none;"></h4>
	<div id="Login">
		<h1 class="LoginField">Login</h1>
		<h1 class="RegistrationField">Register</h1>
		<h2 class="RegistrationField">Display Name</h2>
		<input type="text" id="Name" class="RegistrationField" tabindex="1" />
		<h2>Email Address</h2>
		<input type="text" id="Email" tabindex="2" />
		<h2><a onclick="GetToken();" tabindex="9" style="float: right; cursor: pointer;">Forgot</a>Password</h2>
		<input type="password" id="Password" onkeypress="ActionOnEnter( event, Login );" tabindex="3" />
		<h2 class="RegistrationField">Verify Password</h2>
		<input type="password" id="Verify" onblur="VerifyPassword();" onkeypress="ActionOnEnter( event, Register );" class="RegistrationField" tabindex="4" />
		<input type="button" onclick="Login();" value="Login" class="LoginField" tabindex="5" />
		<input type="button" onclick="Register();" value="Register" class="RegistrationField" tabindex="6" />
		<h3>- OR -</h3>
		<input type="button" onclick="ShowRegister();" value="Register" class="LoginField" tabindex="7" />
		<input type="button" onclick="ShowLogin();" value="Cancel" class="RegistrationField" tabindex="8" />
	</div>
</asp:Content>
