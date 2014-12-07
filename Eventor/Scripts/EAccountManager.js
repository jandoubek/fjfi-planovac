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
                self.UserAccountInfo(data);
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
};