﻿var EventApp = (function (EventApp) {

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
        Content = new EditableText(data.Content, false);
    };

    EventApp.EventViewModel = function (EventID) {

        var self = this;
        self.Pending = ko.observable('<span class="ajax-loader"><img src="/Content/img/ajax-loader.gif" />Loading ...</span>');
        self.CurrentEvent = ko.observable();        

        self.edit = function (model) {
            console.log(model)
            model.editing(true);
        };

        self.CreatedSubEvent = ko.observable({
            EventId: ko.observable(EventID),
            SubEventId: ko.observable(""),
            ParentId: ko.observable(""),
            Name: ko.observable(""),
            Description: ko.observable(""),
            Content: ko.observable("")
        });

        self.EditedSubEvent = ko.observable({
            EventId: ko.observable(EventID),
            SubEventId: ko.observable(""),
            ParentId: ko.observable(""),
            Name: ko.observable(""),
            Description: ko.observable(""),
            Content: ko.observable("")
        });

        self.AddSubEvent = function () {
            if (self.CreatedSubEvent().Name() != "" && self.CreatedSubEvent().Description() != "") {
                $.ajax({
                    url: '/Event/AddSubEvent',
                    cache: false,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON(self.CreatedSubEvent()),
                    success: function (data, status) {
                        if (data != null) {
                            self.SubEvents.push(data);
                            self.CreatedSubEvent().Name("");
                            self.CreatedSubEvent().Description("")
                            Logger.log(arguments.callee.toString(), Logger.success);
                        }
                        else {
                            Logger.log(arguments.callee.toString(), Logger.fail);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        Logger.log(arguments.callee.toString(), textStatus + " " + errorThrown);
                        alert("Error - Modal Window to do");
                    },
                });
            }
            else {
                alert("Add more info");
            };
        }

        self.FillEditModal = function (SubEvent) {
            self.EditedSubEvent(SubEvent);
        };

        self.EditSubEvent = function () {
            $.ajax({
                url: '/Event/EditSubEvent',
                cache: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(self.EditedSubEvent()),
                success: function (data) {
                    if (data.Status == true) {
                        self.GetAll();
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

        self.Remove = function (SubEvent) {
            if (confirm('Are you sure to remove #"' + SubEvent.Name + '" event')) {
                $.ajax({
                    url: '/Event/RemoveEvent',
                    cache: false,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: ko.toJSON(SubEvent),
                    success: function (data) {
                        if (data.Status == true) {
                            alert("Event removed successfuly!");
                            self.SubEvents.remove(SubEvent);
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
