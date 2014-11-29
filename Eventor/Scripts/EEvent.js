var EventApp = (function (EventApp) {
    EventApp.EventViewModel = function (EventID) {

        var self = this;
        self.Pending = ko.observable('<span class="ajax-loader"><img src="/Content/img/ajax-loader.gif" />Loading ...</span>');

        self.CurrentEvent = ko.observable({
            EventID: ko.observable(EventID != null ? EventID : ""),
            Name: ko.observable(""),
            Description : ko.observable(""),
            Content: ko.observable("")
        });

        self.ConfirmDialog = ko.observable({
            Email: ko.observable(""),
            iAgree: ko.observable(false)
        });

        self.SubEvents = ko.observableArray();

        self.RedirectToMainPage = function () {
            alert("You havent agreed with terms or didnt entered email adress, you are beeing redirected to login page!")
            location.href = "/Account/Login";
        };

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

        if (EventID != null)
            self.GetEvent();
    }

    return EventApp;

} (EventApp || {}));

