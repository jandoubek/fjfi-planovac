
// Namespace
var chatR = {};

// Models

chatR.chatMessage = function (User, Content, EventId, Date) {
    var self = this;

    self.User = User;
    self.Content = Content;
    self.EventId = EventId;

    if (Date != null) {
        self.Timestamp = Date;
    }
}

chatR.user = function (data) {
    var self = this;

    self.UserId = data.UserId;
    self.UserName = data.UserName;
    self.FirstName = data.FirstName;
    self.LastName = data.LastName;
    self.Writing = ko.observable(false);
    
    self.FullName = ko.computed(function () {
        return self.FirstName + " " + self.LastName;
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
    self.customRemove = function (UserToRemove) {;
        self.contacts.remove(function (item) {
            return item.UserId == UserToRemove.UserId;
        });
    }
    self.startWriting = function (user, started) {
        ko.utils.arrayForEach(self.contacts(), function (item) {
            if (item.UserId == user.UserId) {
                item.Writing(started);
            }
        });
    }
}

