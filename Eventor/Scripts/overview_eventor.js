function eventCreation(name,description){
    this.name = name;
    this.description = description;
}

function AppViewModel() {

    this.a = ko.observable("asdf");
    this.b = ko.observable("fdaf");

    this.events = ko.observableArray([
        new eventCreation("Bla", "blalba"),
        new eventCreation("fadfa","fasfda")
    ]);

    this.addEvent = function (a,b) {
        this.events.push(new eventCreation(a,b));
    }

    this.moje = ko.observable();
    this.mojes = ko.observableArray();

    $.ajax({
        url: '@Url.Action("GetAllEvents", "Event")',
        cache: false,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            self.mojes(data);
        }
    });



    
}

ko.applyBindings(new AppViewModel());