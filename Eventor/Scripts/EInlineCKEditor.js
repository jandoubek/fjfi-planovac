ko.bindingHandlers.ckEditor = {

    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {

        var txtBoxID = $(element).attr("id");

        // handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            if (CKEDITOR.instances[txtBoxID]) {
                CKEDITOR.remove(CKEDITOR.instances[txtBoxID]);
            }
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {

        var editing = valueAccessor().editing;
        var observable = valueAccessor().observable;

        var txtBoxID = $(element).attr("id");

        var options = allBindingsAccessor().richTextOptions || {};
        options.toolbar_Full = [
            ['Source', '-', 'Format', 'Font', 'FontSize', 'TextColor', 'BGColor', '-', 'Bold', 'Italic', 'Underline', 'SpellChecker'],
            ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl'],
            ['Link', 'Unlink', 'Image', 'Table']
        ];

        if (editing()) {

            $(element).ckeditor(options);
            var val = ko.utils.unwrapObservable(observable());
            $(element).val(val);

            // wire up the blur event to ensure our observable is properly updated
            CKEDITOR.instances[txtBoxID].focusManager.blur = function () {
                observable($(element).val());
            };
        }
        else
        {
            if (CKEDITOR.instances[txtBoxID]) {
                CKEDITOR.instances[txtBoxID].destroy(true);
                $(element).hide();
            }
        }
    }
}