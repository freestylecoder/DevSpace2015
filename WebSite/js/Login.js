function Login() {
	var Request = new XMLHttpRequest();
	Request.withCredentials = true;
	Request.open('GET', ApiUrl + 'Login', true);
	Request.setRequestHeader('Accept', 'text/plain');
	Request.setRequestHeader('Authorization', 'Basic ' + btoa(document.getElementById('Email').value + ':' + document.getElementById('Password').value));
	Request.send();

	Request.onreadystatechange = function () {
		if (Request.readyState == Request.DONE) {
			document.body.style.cursor = '';
			switch (Request.status) {
				case 200:
					sessionStorage.setItem('Id', Request.responseText);
					window.location = 'Profile.aspx';
					break;

				case 401:
					var ErrorMessage = document.getElementById('ErrorMessage');
					ErrorMessage.innerText = "Bad Email or password";
					ErrorMessage.style.display = '';
					break;

				default:
					var ErrorMessage = document.getElementById('ErrorMessage');
					ErrorMessage.innerText = "Unknown Error";
					ErrorMessage.style.display = '';
					break;
			}
		}
	};

	document.body.style.cursor = 'wait';
}

var Forced = false;
function Force() {
	if (Forced)
		return;

	Forced = true;

	var Token = window.location.search.substring(7);

	var Request = new XMLHttpRequest();
	Request.withCredentials = true;
	Request.open('GET', ApiUrl + 'Login', true);
	Request.setRequestHeader('Accept', 'text/plain');
	Request.setRequestHeader('Authorization', 'Force ' + Token);
	Request.send();

	Request.onreadystatechange = function () {
		if (Request.readyState == Request.DONE) {
			document.body.style.cursor = '';
			switch (Request.status) {
				case 200:
					sessionStorage.setItem('Id', Request.responseText);
					window.location = 'Profile.aspx';
					break;

				default:
					var ErrorMessage = document.getElementById('ErrorMessage');
					ErrorMessage.innerText = "Invalid Token";
					ErrorMessage.style.display = '';
					break;
			}
		}
	};

	document.body.style.cursor = 'wait';
}

function GetToken() {
	if ('' == document.getElementById('Email').value.trim()) {
		alert('You must enter an email address.');
		return;
	}

	var Request = new XMLHttpRequest();
	Request.withCredentials = true;
	Request.open('GET', ApiUrl + 'Login/?Email=' + document.getElementById('Email').value, true);
	Request.setRequestHeader('Accept', 'text/plain');
	Request.send();

	Request.onreadystatechange = function () {
		if (Request.readyState == Request.DONE) {
			document.body.style.cursor = '';
			var ErrorMessage = document.getElementById('ErrorMessage');
			ErrorMessage.innerText = Request.responseText.trim( '"' );
			ErrorMessage.style.display = '';
		}
	};

	document.body.style.cursor = 'wait';
}

function Register() {
	if (VerifyPassword()) {
		var RequestJson = {
			DisplayName: document.getElementById('Name').value,
			EmailAddress: document.getElementById('Email').value,
			Password: document.getElementById('Password').value
		};

		var Request = new XMLHttpRequest();
		Request.withCredentials = true;
		Request.open('POST', ApiUrl + 'Speakers', true);
		Request.setRequestHeader( 'Content-Type', 'application/json')
		Request.send(JSON.stringify(RequestJson));

		Request.onreadystatechange = function () {
			if (Request.readyState == Request.DONE) {
				document.body.style.cursor = '';
				switch (Request.status) {
					case 201:
						Login();
						break;

					case 401:
						var ErrorMessage = document.getElementById('ErrorMessage');
						ErrorMessage.innerText = "Registration Failed. Email Already Registered.";
						ErrorMessage.style.display = '';
						break;

					default:
						var ErrorMessage = document.getElementById('ErrorMessage');
						ErrorMessage.innerText = "Unknown Error";
						ErrorMessage.style.display = '';
						break;
				}
			}
		};

		document.body.style.cursor = 'wait';
	}
}

function VerifyPassword() {
	var Verify = document.getElementById('Verify');
	var Password = document.getElementById('Password');

	if (Password.value && (document.getElementById('Password').value == Verify.value)) {
		document.getElementById('ErrorMessage').style.display = 'none';
		Verify.style.borderColor = '#000000';
		return true;
	} else {
		document.getElementById('ErrorMessage').style.display = '';
		document.getElementById('ErrorMessage').innerText = 'Password and Verification did not match';
		Verify.style.borderColor = '#FF0000';
		return false;
	}
}

function ShowRegister() {
	var LoginFields = document.getElementsByClassName('LoginField');
	var RegistrationFields = document.getElementsByClassName('RegistrationField');

	var index;
	for (index = 0; index < LoginFields.length; ++index) {
		LoginFields[index].style.display = 'none';
	}

	for (index = 0; index < RegistrationFields.length; ++index) {
		RegistrationFields[index].style.display = '';
	}

	document.getElementById('Name').focus();
}

function ShowLogin() {
	if (window.location.search.toUpperCase().indexOf("FORCE") > -1) {
		Force();
	}

	var LoginFields = document.getElementsByClassName('LoginField');
	var RegistrationFields = document.getElementsByClassName('RegistrationField');

	var index;
	for (index = 0; index < LoginFields.length; ++index) {
		LoginFields[index].style.display = '';
	}

	for (index = 0; index < RegistrationFields.length; ++index) {
		RegistrationFields[index].style.display = 'none';
	}

	document.getElementById('Email').focus();
}

function ActionOnEnter(e, f) {
	if (e.char == '\n') {
		f();
	}
}

window.onload = ShowLogin;