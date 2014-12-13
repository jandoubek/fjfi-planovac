var OverviewApp = (function (OverviewApp) {
    OverviewApp.OverviewViewModel = function () {

        var self = this;

        // Async load animation
        self.Pending = ko.observable('<span class="ajax-loader"><img src="/Content/img/ajax-loader.gif" />Loading ...</span>');

        // Create Event dialog
        self.CreatedEvent = ko.observable({
            EventId: ko.observable(""),
            Name: ko.observable(""),
            Description: ko.observable(""),
            Content: ko.observable("")
        });

        // Edit Event dialog
        self.EditEvent = ko.observable({
            EventId: ko.observable(""),
            Name: ko.observable(""),
            Description: ko.observable(""),
            Content: ko.observable("")
        });

        // Event list
        self.Events = ko.observableArray();

        // Method for filling Edit Event dialog
        self.FillEditModal = function (Event) {
            self.EditEvent(Event);
        };

        // Redirection on Event Detail page on click
        self.RedirectToEvent = function (Event) {
            $(location).attr('href', '/Event/Detail/' + truncateString(Event.Name) + '/' + Event.EventId);
        };        

        // Method for duping all user events
        self.GetEvents = function () {
            $.ajax({
                url: '/Event/GetEvents',
                cache: false,
                type: "POST",
                success: function (data) {
                    self.Events(data);
                    self.Pending("");

                    Logger.log(arguments.callee.toString(), Logger.success);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Logger.log(arguments.callee.toString(), textStatus + " " + errorThrown);
                    alert("Ajax Error - Need to implement Modal Window");
                },
            });
        };











        self.Add = function () {
            if (self.CreatedEvent().Name() != "" && self.CreatedEvent().Description() != "") {
                $.ajax({
                    url: '/Event/AddEvent',
                    cache: false,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON(self.CreatedEvent()),
                    success: function (data, status) {
                        if (data) {
                            self.CreatedEvent().Name("");
                            self.CreatedEvent().Description("")
                            self.Events.push(data);

                            Logger.log(arguments.callee.toString(), Logger.success);
                        }
                        else {
                            Logger.log(arguments.callee.toString(), Logger.fail);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        Logger.log(arguments.callee.toString(), textStatus + " " + errorThrown);
                        alert("Ajax Error - Need to implement Modal Window");
                    },
                });
            }
            else {
                alert("Need more information");
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
                            Logger.log(arguments.callee.toString(), Logger.success);
                        }
                        else {
                            Logger.log(arguments.callee.toString(), Logger.fail);
                            alert("Event hasnt deleted successfully. An error has occured!")
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        Logger.log(arguments.callee.toString(), textStatus + " " + errorThrown);
                        alert("Error - Modal Window to do");
                    }
                });
            }
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
                        self.GetEvents();
                        Logger.log(arguments.callee.toString(), Logger.success);
                    }
                    else {
                        Logger.log(arguments.callee.toString(), Logger.fail);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Logger.log(arguments.callee.toString(), textStatus + " " + errorThrown);
                    alert("Error - Modal Window to do");
                }
            })
        };

        self.GetEvents();        
    }

    return OverviewApp;

}(OverviewApp || {}));

