var OverviewApp = (function (OverviewApp) {
    OverviewApp.OverviewViewModel = function () {

        var self = this;
        self.Pending = ko.observable('<span class="ajax-loader"><img src="/Content/img/ajax-loader.gif" />Loading ...</span>');

        self.CreatedEvent = ko.observable({
            EventID: ko.observable(""),
            Name: ko.observable(""),
            Description: ko.observable(""),
            Content: ko.observable("")
        });

        self.EditEvent = ko.observable({
            EventID: ko.observable(""),
            Name: ko.observable(""),
            Description: ko.observable(""),
            Content: ko.observable("")
        });

        self.Events = ko.observableArray();

        self.GetAll = function () {
            $.ajax({
                url: '/Event/GetAllEvents',
                cache: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: {},
                success: function (data) {
                    self.Events(data);
                    self.Pending("");
                },
                error: function () {
                    alert("Fail");
                },
            });
        }

        self.Add = function () {
            if (self.CreatedEvent().Name() != "" && self.CreatedEvent().Description() != "") {
                $.ajax({
                    url: '/Event/AddEvent',
                    cache: false,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON(self.CreatedEvent()),
                    success: function (data, status) {
                        if (data != null) {
                            self.Events.push(data);
                            self.CreatedEvent().Name("");
                            self.CreatedEvent().Description("")
                        }
                        else {
                            alert("No data added");
                        }
                    },
                    error: function () {
                        alert("Fail");
                    },
                });
            }
            else {
                alert("Add more info");
            };
        }

        self.Remove = function (Event) {
            if (confirm('Are you sure to remove #"' + Event.Name + '" event')) {
                $.ajax({
                    url: '/Event/RemoveEvent',
                    cache: false,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: ko.toJSON(Event),
                    success: function (data) {
                        if (data.Status == true) {
                            alert("Event removed successfuly!");
                            self.Events.remove(Event);
                        }
                        else {
                            alert("Event hasnt deleted successfully. An error has occured!")
                        }
                    },
                    error: function () {
                        alert("Error");
                    }
                });
            }
        };

        self.FillEditModal = function (Event) {
            self.EditEvent(Event);
        };

        self.Edit = function () {
            $.ajax({
                url: '/Event/EditEvent',
                cache: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(self.EditEvent()),
                success: function (data) {
                    if (data.Status == true) {
                        self.GetAll();
                        alert("Record Updated Successfully");
                    }
                    else {
                        alert("Record Updated Unsuccessfully");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("Ajax mechanism failed" + textStatus + " " + errorThrown);
                }
            })
        };

        self.GetAll();        
    }

    return OverviewApp;

} (OverviewApp || {}));
