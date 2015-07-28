function Profile(data) {
	var Self = this;
	Self.Id = ko.observable();
	Self.DisplayName = ko.observable();
	Self.EmailAddress = ko.observable();
	Self.Password = ko.observable();
	Self.Bio = ko.observable();
	Self.Twitter = ko.observable();
	Self.Website = ko.observable();

	if (data) {
		Self.Id(data.Id);
		Self.DisplayName(data.DisplayName);
		Self.EmailAddress(data.EmailAddress);
		Self.Bio(data.Bio);
		Self.Twitter(data.Twitter);
		Self.Website(data.Website);
	}
}

function Session(data) {
	var Self = this;
	Self.Id = ko.observable();
	Self.Speaker = ko.observable();
	Self.Title = ko.observable();
	Self.Abstract = ko.observable();
	Self.Notes = ko.observable();
	Self.Tags = ko.observableArray([]);

	Self.TagList = ko.pureComputed( function () {
		var TagList = '';
		for (var index = 0; index < this.Tags().length; ++index)
			TagList += this.Tags()[index].Text() + '; ';
		return TagList;
	}, Self );

	if (data) {
		Self.Id(data.Id);
		Self.Speaker(data.Speaker);
		Self.Title(data.Title);
		Self.Abstract(data.Abstract);
		Self.Notes(data.Notes);

		if (data.Tags)
			for (var index = 0; index < data.Tags.length; ++index)
				if (ko.isObservable(data.Tags[index]))
					Self.Tags.push(data.Tags[index]);
				else
					Self.Tags.push(new Tag(data.Tags[index]));
	}
}

function Tag(data) {
	var Self = this;
	Self.Id = ko.observable(data.Id);
	Self.Text = ko.observable(data.Text);
}

function ViewModel() {
	var Self = this;
	Self.Profile = ko.observable(new Profile());
	Self.Sessions = ko.observableArray([]);
	Self.Tags = ko.observableArray([]);
	Self.SelectedSession = ko.observable(new Session());
	Self.Verify = ko.observable();

	var ProfileRequest = new XMLHttpRequest();
	ProfileRequest.withCredentials = true;
	ProfileRequest.open('GET', ApiUrl + 'Speakers/' + sessionStorage.getItem('Id'), true);
	ProfileRequest.send();

	ProfileRequest.onreadystatechange = function () {
		if (ProfileRequest.readyState == ProfileRequest.DONE) {
			switch (ProfileRequest.status) {
				case 200:
					Self.Profile(new Profile(JSON.parse(ProfileRequest.responseText)));
					break;

				case 401:
					// Login failed

				default:
					break;
			}
		}
	};

	var SessionsRequest = new XMLHttpRequest();
	SessionsRequest.withCredentials = true;
	SessionsRequest.open('GET', ApiUrl + 'Speakers/' + sessionStorage.getItem('Id') + '/Sessions', true);
	SessionsRequest.send();

	SessionsRequest.onreadystatechange = function () {
		if (SessionsRequest.readyState == SessionsRequest.DONE) {
			switch (SessionsRequest.status) {
				case 200:
					var SessionList = JSON.parse(SessionsRequest.responseText);
					for (var index = 0; index < SessionList.length; ++index)
						Self.Sessions.push(new Session(SessionList[index]));
					break;

				case 401:
					// Login failed

				default:
					break;
			}
		}
	};

	var TagsRequest = new XMLHttpRequest();
	TagsRequest.withCredentials = true;
	TagsRequest.open('GET', ApiUrl + 'Tags', true);
	TagsRequest.send();

	TagsRequest.onreadystatechange = function () {
		if (TagsRequest.readyState == TagsRequest.DONE) {
			switch (TagsRequest.status) {
				case 200:
					var TagList = JSON.parse(TagsRequest.responseText);
					for (var index = 0; index < TagList.length; ++index)
						Self.Tags.push(new Tag(TagList[index]));
					break;

				case 401:
					// Login failed

				default:
					break;
			}
		}
	};

	Self.ShowProfile = function () {
		document.getElementById('Profile').style.display = 'block';
		document.getElementById('Session').style.display = 'none';
		document.getElementById('Credentials').style.display = 'none';
	}

	Self.SaveProfile = function () {
		var Request = new XMLHttpRequest();
		Request.withCredentials = true;
		Request.open('POST', ApiUrl + 'Speakers', true);
		Request.setRequestHeader('Content-Type', 'application/json');
		Request.send(ko.toJSON(Self.Profile));

		Request.onreadystatechange = function () {
			if (Request.readyState == Request.DONE) {
				switch (Request.status) {
					case 200:
						break;

					case 401:
						// Login failed

					default:
						break;
				}
			}
		};
	}

	Self.ShowCredentials = function () {
		document.getElementById('Profile').style.display = 'none';
		document.getElementById('Session').style.display = 'none';
		document.getElementById('Credentials').style.display = 'block';
	}

	Self.SaveCredentials = function () {
		if (Self.Verify() != Self.Profile().Password()) {
			alert('Password and Verify did not match');
			return;
		}

		var Request = new XMLHttpRequest();
		Request.withCredentials = true;
		Request.open('POST', ApiUrl + 'Speakers/' + sessionStorage.getItem('Id'), true);
		Request.setRequestHeader('Content-Type', 'application/json');
		Request.send(ko.toJSON(Self.Profile));

		Request.onreadystatechange = function () {
			if (Request.readyState == Request.DONE) {
				switch (Request.status) {
					case 200:
						Self.ShowProfile();
						break;

					case 400:
						// Login failed

					default:
						break;
				}
			}
		};
	}

	Self.ShowSession = function (data) {
		document.getElementById('Profile').style.display = 'none';
		document.getElementById('Session').style.display = 'block';
		document.getElementById('Credentials').style.display = 'none';

		if (data) {
			Self.SelectedSession(data);
		} else {
			Self.SelectedSession(new Session());
			Self.SelectedSession().Id(-1);
			Self.SelectedSession().Speaker(Self.Profile);
		}
	}

	Self.SaveSession = function () {
		var Request = new XMLHttpRequest();
		Request.withCredentials = true;
		Request.open('POST', ApiUrl + 'Sessions', true);
		Request.setRequestHeader('Accept', 'application/json');
		Request.setRequestHeader('Content-Type', 'application/json');
		Request.send( ko.toJSON(Self.SelectedSession()) );

		Request.onreadystatechange = function () {
			if (Request.readyState == Request.DONE) {
				switch (Request.status) {
					case 201:
						Self.Sessions.push(new Session(JSON.parse(Request.responseText)));

					case 200:
						Self.ShowProfile();
						break;

					default:
						break;
				}
			}
		};
	}

	Self.DeleteSession = function (data) {
		var Request = new XMLHttpRequest();
		Request.withCredentials = true;
		Request.open('DELETE', ApiUrl + 'Sessions/' + data.Id(), true);
		Request.send();

		Request.onreadystatechange = function () {
			if (Request.readyState == Request.DONE) {
				switch (Request.status) {
					case 204:
						Self.Sessions.remove(data);
						Self.Sessions.valueHasMutated();
						break;

					default:
						break;
				}
			}
		};
	}

	Self.AddOrRemoveTagToSession = function (data) {
		for (var index = 0; index < Self.SelectedSession().Tags().length; ++index) {
			if (data.Text().toUpperCase() == Self.SelectedSession().Tags()[index].Text().toUpperCase()) {
				Self.SelectedSession().Tags.remove(Self.SelectedSession().Tags()[index]);
				return;
			}
		}

		Self.SelectedSession().Tags.push(data);
	}

	Self.SaveTag = function () {
		var TagText = document.getElementById('NewTagText').value.trim();
		if (!TagText) return;

		for (var index = 0; index < Self.Tags().length; ++index) {
			if (TagText.toUpperCase() == Self.Tags()[index].Text().toUpperCase()) {
				Self.AddOrRemoveTagToSession(Self.Tags()[index]);
				document.getElementById('NewTagText').value = '';
				return;
			}
		}

		var TagRequest = new XMLHttpRequest();
		TagRequest.withCredentials = true;
		TagRequest.open('POST', ApiUrl + 'Tags', true);
		TagRequest.setRequestHeader('Content-Type', 'application/json');
		TagRequest.send( '{ Id: -1, Text: "' + TagText + '" }' );

		TagRequest.onreadystatechange = function () {
			if (TagRequest.readyState == TagRequest.DONE) {
				switch (TagRequest.status) {
					case 201:
						var NewTag = new Tag(JSON.parse(TagRequest.responseText));
						Self.Tags.push(NewTag);
						Self.AddOrRemoveTagToSession(NewTag);
						document.getElementById('NewTagText').value = '';
						break;

					case 401:
						// Login failed

					default:
						break;
				}
			}
		}
	}
}

window.onload = function () {
	ko.applyBindings(new ViewModel());
}
