var EventApp = (function (EventApp) {
    EventApp.EventViewModel = function (EventID) {

        var self = this;
        self.Pending = ko.observable('<span class="ajax-loader"><img src="/Content/img/ajax-loader.gif" />Loading ...</span>');

        self.CurrentEvent = ko.observable({
            EventID: ko.observable(EventID),
            Name: ko.observable(""),
            Description : ko.observable(""),
            Content: ko.observable("")
        });

        self.SubEvents = ko.observableArray();

        self.GetEvent = function () {
            $.ajax({
                url: '/Event/GetEvent',
                cache: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON(self.CurrentEvent()),
                success: function (data) {
                    self.CurrentEvent(data);
                    self.GetSubEvents(data);
                    self.Pending("");
                },
                error: function () {
                    alert("Fail");
                },
            });
        }

        self.GetSubEvents = function (event) {
            $.ajax({
                url: '/Event/GetSubEvents',
                cache: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON(event),
                success: function (data) {
                    self.SubEvents(data);
                    self.Pending("");
                },
                error: function () {
                    alert("Fail2");
                },
            });
        }

        self.GetEvent();
    }

    return EventApp;

} (EventApp || {}));

