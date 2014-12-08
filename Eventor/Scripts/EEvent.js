var EventApp = (function (EventApp) {

    function EditableText(text, editable) {
        var self = this;
        self.text = ko.observable(text);
        self.editing = ko.observable(editable);
    };

    function Event(data) {
        var self = this;
        EventID = new EditableText(data.EventID, false);
        Name = new EditableText(data.Name, false);
        Description = new EditableText(data.Description, false);
        Content = new EditableText("test", false);
    };

    EventApp.EventViewModel = function (EventID) {

        var self = this;
        self.Pending = ko.observable('<span class="ajax-loader"><img src="/Content/img/ajax-loader.gif" />Loading ...</span>');

        self.CurrentEvent = ko.observable();        

        self.edit = function (model) {
            console.log(model)
            model.editing(true);
        };

        self.SubEvents = ko.observableArray();

        self.GetEvent = function () {
            $.ajax({
                url: '/Event/GetEvent',
                cache: false,
                type: "POST",
                data: { "EventId" : EventID },
                success: function (data) {
                    self.CurrentEvent(new Event(data));
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

        self.ConfirmDialog = ko.observable({
            Email: ko.observable(""),
            iAgree: ko.observable(false),
        });

        self.ProcessConfirmDialogClose = function () {
            alert("You havent agreed with terms or didnt entered email adress, you are beeing redirected to login page!")
            location.href = "/Account/Login";
        };

        self.ProcessConfirmDialogConfirm = function () {
            alert("You have successfully entered email and agreed with terms and conditions!");
            $('#confirmModal').modal('hide');
        }

        if (EventID != null)
            self.GetEvent();
    }

    return EventApp;

}(EventApp || {}));
