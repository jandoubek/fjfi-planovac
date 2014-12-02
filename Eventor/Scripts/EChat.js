
// Namespace
var chatR = {};

// Models

chatR.chatMessage = function (sender, eventId, content, dateSent) {
    var self = this;
    self.user = sender;
    self.eventid = eventId;
    self.content = content;
    if (dateSent != null) {
        self.timestamp = dateSent;
    }
}

chatR.user = function (userId, userName, firstName, lastName) {
    var self = this;
    self.userid = userId;
    self.username = userName;
    self.firstname = firstName;
    self.lastname = lastName;

    self.fullName = ko.computed(function () {
        return self.firstname + " " + self.lastname;
    });
}

// ViewModels

chatR.chatViewModel = function () {
    var self = this;
    self.messages = ko.observableArray();
}

chatR.connectedUsersViewModel = function () {
    var self = this;
    self.contacts = ko.observableArray();
    self.customRemove = function (userToRemove) {
        var userIdToRemove = userToRemove.userid;
        self.contacts.remove(function (item) {
            return item.userid === userIdToRemove;
        });
    }
}

