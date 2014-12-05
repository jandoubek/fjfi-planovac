var AccountManagerViewModel = function () {

    var self = this;
    self.Pending = ko.observable('<span class="ajax-loader"><img src="/Content/img/ajax-loader.gif" />Loading ...</span>');

    self.UserAccountInfo = ko.observable({
        UserName: ko.observable(""),
        Name: ko.observable(""),
        Surname: ko.observable(""),
        Email: ko.observable("")
    });

    self.GetUserAccountInfo = function () {
        $.ajax({
            url: '/Account/GetUserInfo',
            cache: false,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: {},
            success: function (data) {
                self.UserAccountInfo.Name = data.Name;
                self.UserAccountInfo.Surname = data.Surname;
                self.UserAccountInfo.UserName = data.UserName;
                self.UserAccountInfo.Email = data.Email;
                self.Pending("");
                Logger.log(arguments.callee.toString(), Logger.success);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                Logger.log(arguments.callee.toString(), textStatus + " " + errorThrown);
                alert("Error - Modal Window to do");
            },
        });
    };

    self.GetUserAccountInfo();
};

var AccountEditViewModel = function () {

    var self = this;
    self.Pending = ko.observable('<span class="ajax-loader"><img src="/Content/img/ajax-loader.gif" />Loading ...</span>');

    self.EditAccountInfo = ko.observable({
        UserName: ko.observable(""),
        Name: ko.observable(""),
        Surname: ko.observable(""),
        Email: ko.observable("")
    });

    self.EditAccount = function () {
            $.ajax({
                url: '/Account/EditUserInfo',
                cache: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(self.EditAccountInfo()),
                success: function (data) {
                    if (data.Status == true) {
                        AccountManagerViewModel.GetUserAccountInfo();
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
            });
    };

};