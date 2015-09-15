function Session(data) {
	var Self = this;
	Self.Id = ko.observable();
	Self.Title = ko.observable();
	Self.Link = ko.observable();

	if (data) {
		Self.Id(data.Id);
		Self.Title(data.Title);

		Self.Link('Sessions.aspx?Id=' + data.Id);
	}
}

function Profile(data) {
	var Self = this;
	Self.Id = ko.observable();
	Self.DisplayName = ko.observable();
	Self.EmailAddress = ko.observable();
	Self.Password = ko.observable();
	Self.Bio = ko.observable();
	Self.Twitter = ko.observable();
	Self.Website = ko.observable();
	Self.Sessions = ko.observableArray([]);

	if (data) {
		Self.Id(data.Id);
		Self.DisplayName(data.DisplayName);
		Self.EmailAddress(data.EmailAddress);
		Self.Bio(data.Bio);

		if (data.Twitter)
			if (data.Twitter[0] != '@')
				data.Twitter = '@' + data.Twitter;

		Self.Twitter(data.Twitter);

		if( data.Website )
			if (data.Website.search('http') == -1)
				data.Website = 'http://' + data.Website;

		Self.Website(data.Website);

		var SessionsRequest = new XMLHttpRequest();
		//SessionsRequest.withCredentials = true;
		SessionsRequest.open('GET', ApiUrl + 'Speakers/' + data.Id + '/Sessions/Accepted', true);
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
	}

	Self.TwitterLink = ko.pureComputed(function () {
		var Raw = Self.Twitter();

		if (Raw)
			return 'https://twitter.com/' + Raw.substring( 1 );
		else
			return null;
	});
}

function ViewModel() {
	var Self = this;
	Self.Profiles = ko.observableArray([]);

	var qd = null;
	if (location.search) {
		qd = {};
		location.search.substr(1).split("&").forEach(function (item) { var s = item.split("="), k = s[0], v = s[1] && decodeURIComponent(s[1]); (k in qd) ? qd[k].push(v) : qd[k] = [v] });
	}

	var ProfileRequest = new XMLHttpRequest();
	if (qd && qd.Id)
		ProfileRequest.open('GET', ApiUrl + 'Speakers/' + qd.Id, true);
	else
		ProfileRequest.open('GET', ApiUrl + 'Speakers', true);
	ProfileRequest.send();

	ProfileRequest.onreadystatechange = function () {
		if (ProfileRequest.readyState == ProfileRequest.DONE) {
			switch (ProfileRequest.status) {
				case 200:
					var ProfileList = JSON.parse(ProfileRequest.responseText);
					if (ProfileList.length)
						for (var index = 0; index < ProfileList.length; ++index)
							Self.Profiles.push(new Profile(ProfileList[index]));
					else
						Self.Profiles.push(new Profile(ProfileList));
					break;

				case 401:
					// Login failed

				default:
					break;
			}
		}
	};
}

window.onload = function () {
	ko.applyBindings(new ViewModel());
}
