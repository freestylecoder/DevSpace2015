﻿function Profile(data) {
	var Self = this;
	Self.Id = ko.observable();
	Self.DisplayName = ko.observable();
	Self.Link = ko.observable();

	if (data) {
		Self.Id(data.Id);
		Self.DisplayName(data.DisplayName);

		Self.Link('Speakers.aspx?Id=' + data.Id);
	}
}

function Session(data) {
	var Self = this;
	Self.Id = ko.observable();
	Self.Speaker = ko.observable();
	Self.Title = ko.observable();
	Self.Abstract = ko.observable();
	Self.TimeSlot = ko.observable();
	Self.Tags = ko.observableArray([]);

	Self.TagList = ko.pureComputed(function () {
		var TagList = '';
		for (var index = 0; index < this.Tags().length; ++index)
			TagList += this.Tags()[index].Text() + '; ';
		return TagList;
	}, Self);

	if (data) {
		Self.Id(data.Id);
		Self.Speaker(new Profile(data.Speaker));
		Self.Title(data.Title);
		Self.Abstract('<p>' + data.Abstract.trim().replace('\n', '</p><p>') + '</p>');
		Self.TimeSlot(data.TimeSlot);

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
	Self.Link = ko.observable('Sessions.aspx?TagId=' + data.Id);
}

function ViewModel() {
	var Self = this;
	Self.Sessions = ko.observableArray([]);

	var qd = null;
	if (location.search) {
		qd = {};
		location.search.substr(1).split("&").forEach(function (item) { var s = item.split("="), k = s[0], v = s[1] && decodeURIComponent(s[1]); (k in qd) ? qd[k].push(v) : qd[k] = [v] });
	}

	var SessionsRequest = new XMLHttpRequest();
	if (qd)
		if (qd.Id)
			SessionsRequest.open('GET', ApiUrl + 'Sessions/' + qd.Id, true);
		else if (qd.TagId)
			SessionsRequest.open('GET', ApiUrl + 'Sessions/Tags/' + qd.TagId, true);
		else
			SessionsRequest.open('GET', ApiUrl + 'Sessions', true);
	else
		SessionsRequest.open('GET', ApiUrl + 'Sessions', true);
	SessionsRequest.send();

	SessionsRequest.onreadystatechange = function () {
		if (SessionsRequest.readyState == SessionsRequest.DONE) {
			switch (SessionsRequest.status) {
				case 200:
					var SessionList = JSON.parse(SessionsRequest.responseText);
					if (SessionList.length)
						for (var index = 0; index < SessionList.length; ++index)
							Self.Sessions.push(new Session(SessionList[index]));
					else
						Self.Sessions.push(new Session(SessionList));
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
