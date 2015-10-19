﻿var allowedExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png"];
var uploaderTemplate = '<div class="uploader field">\
<div class="ui segment">\
    <img style="max-width:300px;" src="{0}" class="preview">\
</div>\
</div>\
<div class="uploader">\
<label for="file{1}" class="ui pink icon button">\
    <i class="file icon"></i>\
    Upload</label>\
    <input id="file{1}" class="file" data-target="{1}" style="display: none" type="file">\
</div>';

var scrudShowCompact = function () {
    var url = updateQueryString('show', 'compact');
    url = updateQueryString('Page', '1', url);
    window.location = url;
};

var scrudShowAll = function () {
    var url = updateQueryString('show', 'all');
    url = updateQueryString('Page', '1', url);
    window.location = url;
};

var scrudConfirmAction = function () {
    var retVal = false;
    var selectedItemValue;

    var confirmed = confirm(Resources.Questions.AreYouSure());

    if (confirmed) {
        selectedItemValue = scrudGetSelectedRadioValue();

        if (selectedItemValue == undefined) {
            alert(Resources.Titles.NothingSelected());
            retVal = false;
        } else {
            retVal = true;
            if (customFormUrl && keyColumn) {
                window.location = customFormUrl + "?" + keyColumn + "=" + selectedItemValue;
            };

            $("#GridPanel").addClass("ui segment loading");
        };
    };


    return retVal;
};

var scrudSelectAndClose = function () {
    var lastValueHidden = $("#LastValueHidden");
    lastValueHidden.val(scrudGetSelectedRadioValue());
    scrudSaveAndClose();
};

var scrudGetSelectedRadioValue = function () {
    return $('[id^="SelectRadio"]:checked').val();
};

var scrudSelectRadioById = function (id) {
    $('[id^="SelectRadio"]').prop("checked", false);
    $("#" + id).prop("checked", true);
};

var scrudPrintGridView = function () {
    var user = $("#" + userIdHiddenId).val();
    var office = $("#" + officeCodeHiddenId).val();
    var title = $("#" + titleLabelId).html();

    printGridView(reportTemplatePath, reportHeaderPath, title, formGridViewId, date, user, office, 'ScrudReport', 1, 0);
};


var scrudUpdateMarkup = function (triggerControlId) {
    var user = $("#" + userIdHiddenId).val();
    var office = $("#" + officeCodeHiddenId).val();
    var title = $("#" + titleLabelId).html();

    printGridView(reportExportTemplatePath, reportHeaderPath, title, formGridViewId, date, user, office, 'ScrudReport', 1, 0, $("#MarkupHidden"), triggerControlId);
};


Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
    //Fired on each ASP.net AJAX request.
    scrudInitialize();
    initializeUploader();
    scrudLoadDatepicker();
    $(".activating.element").popup();
});

$(document).ready(function () {
    scrudInitialize();
    initializeUploader();
    scrudLoadDatepicker();
});

var scrudInitialize = function () {
    //Registering grid row click event to automatically select the radio.
    $('#' + formGridViewId + ' tr').click(function () {
        //Grid row was clicked. Now, searching the radio button.
        var radio = $(this).find('td input:radio');

        //The radio button was found.
        scrudSelectRadioById(radio.attr("id"));
    });

    scrudSaveAndClose();
    scrudLayout();
    scrudOnServerError();
};

function scrudOnServerError() {
    var errorField = $("#ScrudError");
    if (errorField.length) {
        displayForm();
    };
};

function scrudLayout() {
    var gridPanel = $("#GridPanel");
    var scrudUpdatePanel = $("#ScrudUpdatePanel");

    if (gridPanel && scrudUpdatePanel) {
        gridPanel.css("width", scrudUpdatePanel.width() + "px");
    };
};

function scrudGetQueryStringByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function getParent() {
    var parent;

    if (window.opener && window.opener.document) {
        parent = window.opener;
    };

    if (parent == undefined) {
        parent = window.parent;
    };

    return parent;
};

function scrudSaveAndClose() {
    var parent = getParent();

    if (parent) {
        var lastValue = parseFloat2($("#LastValueHidden").val());
        var ctl = scrudGetQueryStringByName('AssociatedControlId');
        var associatedControl = parent.$('#' + ctl);
        var callBackFunctionName = scrudGetQueryStringByName('CallBackFunctionName');

        if (lastValue > 0) {
            associatedControl.val(lastValue);

            if (callBackFunctionName) {
                parent[callBackFunctionName]();
            };

            closeWindow();
        };
    };
};

function closeWindow() {
    if (window.opener && window.opener.document) {
        top.close();
    } else {
        parent.jQuery.colorbox.close();
    };
};

function scrudDispalyLoading() {
    $("#FormPanel").find("div.segment").addClass("loading");
};
function scrudRemoveLoading() {
    $("#FormPanel").find("div.segment").removeClass("loading");
};

function scrudUpdateTableHeaders() {
    $("div.floating-header").each(function () {
        var originalHeaderRow = $(".tableFloatingHeaderOriginal", this);
        var floatingHeaderRow = $(".tableFloatingHeader", this);
        var offset = $(this).offset();
        var scrollTop = $(window).scrollTop();
        if ((scrollTop > offset.top) && (scrollTop < offset.top + $(this).height())) {
            floatingHeaderRow.css("visibility", "visible");
            floatingHeaderRow.css("z-index", "1");
            floatingHeaderRow.css("top", Math.min(scrollTop - offset.top, $(this).height() - floatingHeaderRow.height()) + "px");

            // Copy cell widths from original header
            $("th", floatingHeaderRow).each(function (index) {
                var cellWidth = $("th", originalHeaderRow).eq(index).css('width');
                $(this).css('width', cellWidth);
            });

            // Copy row width from whole table
            floatingHeaderRow.css("width", $("table.grid").css("width"));
        } else {
            floatingHeaderRow.css("visibility", "hidden");
            floatingHeaderRow.css("top", "0");
        }
    });
};

$(document).ready(function () {
    $("table.grid").each(function () {
        $(this).wrap("<div class=\"floating-header\" style=\"position:relative\"></div>");

        var originalHeaderRow = $("tr:first", this);
        originalHeaderRow.before(originalHeaderRow.clone());
        var clonedHeaderRow = $("tr:first", this);

        clonedHeaderRow.addClass("tableFloatingHeader");
        clonedHeaderRow.css("position", "absolute");
        clonedHeaderRow.css("top", "0");
        clonedHeaderRow.css("left", $(this).css("margin-left"));
        clonedHeaderRow.css("visibility", "hidden");

        originalHeaderRow.addClass("tableFloatingHeaderOriginal");
    });
    scrudUpdateTableHeaders();
    $(window).scroll(scrudUpdateTableHeaders);
    $(window).resize(scrudUpdateTableHeaders);
});

var scrudAddNew = function () {
    if (customFormUrl) {
        top.location = customFormUrl;
    }

    $('#' + formGridViewId + 'tr').find('td input:radio').prop('checked', false);
    $('#form1').each(function () {
        this.reset();
    });

    displayForm();

    scrudRepaint();

    if (typeof scrudAddNewCallBack === "function") {
        scrudAddNewCallBack();
    };
    //Prevent postback
    return false;
};

function displayForm() {
    $('#' + gridPanelId).hide(500);
    $('#' + formPanelId).show(500);

    if (typeof scrudFormDisplayedCallBack === "function") {
        scrudFormDisplayedCallBack();
    };
};

var scrudRepaint = function () {
    setTimeout(function () {
        $(document).trigger('resize');
    }, 1000);
};

$(document).ready(function () {
    shortcut.add("ESC", function () {
        if ($('#' + formPanelId).is(':hidden')) {
            return;
        };

        if ($("#colorbox").css("display") === "block") {
            return;
        };

        if ($("#colorbox").siblings().hasClass('active')) {
            return;
        }

        if ($("body").attr("class") === "modal-open") {
            return;
        };

        var result = confirm(Resources.Questions.AreYouSure());
        if (result) {
            $('#' + cancelButtonId).click();
        }
    });

    shortcut.add("RETURN", function () {
        scrudSelectAndClose();
    });

    shortcut.add("CTRL+SHIFT+C", function () {
        scrudShowCompact();
    });

    shortcut.add("CTRL+SHIFT+S", function () {
        scrudShowAll();
    });

    shortcut.add("CTRL+SHIFT+A", function () {
        return (scrudAddNew());
    });

    shortcut.add("CTRL+SHIFT+E", function () {
        $('#EditButtontop').click();
    });

    shortcut.add("CTRL+SHIFT+D", function () {
        $('#DeleteButtontop').click();
    });

    shortcut.add("CTRL+SHIFT+P", function () {
        scrudPrintGridView();
    });
});

function scrudClientValidation() {
    if (Page_ClientValidate("")) {
        scrudDispalyLoading();

        if (typeof scrudCustomValidator === "function") {
            var isValid = scrudCustomValidator();

            if (!isValid) {
                scrudRemoveLoading();
            };

            return isValid;
        };

        return true;
    };
};


//Scrud Uploader Begin
function initializeUploader() {
    var instances = $("input.image");
    instances.each(function () {
        var el = $(this);
        el.parent().find(".uploader").remove();
        var val = el.val();
        var id = el.attr("id");
        var imagePath = "/Static/images/mixerp-logo-light.png";

        if (val) {
            imagePath = "/Resource/Static/Attachments/" + val;
        }

        el.attr("style", "display:none;");
        el.parent().append(stringFormat(uploaderTemplate, imagePath, id));
    });


    var file = $(".file");

    file.change(function () {
        if (isValidExtension(this)) {
            readURL(this);
            var el = $(this);
            var segment = el.closest(".segment");
            var target = $("#" + el.attr("data-target"));

            segment.addClass("loading");

            el.upload("/FileUploadHanlder.ashx", function (uploadedFileName) {
                target.val(uploadedFileName);
                segment.removeClass("loading");
            }, function (progress, value) {
                //not implemented yet.
            });
        };
    });

};

function isValidExtension(el) {

    if (el.type === "file") {
        var fileName = el.value;

        if (fileName.length > 0) {

            var valid = false;

            for (var i = 0; i < allowedExtensions.length; i++) {
                var extension = allowedExtensions[i];

                if (fileName.substr(fileName.length - extension.length, extension.length).toLowerCase() === extension.toLowerCase()) {
                    valid = true;
                    break;
                };
            };

            if (!valid) {
                alert("Invalid file extension.");
                el.value = "";
                return false;
            };
        };
    };

    return true;
};



function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var image = $(input).parent().parent().parent().find("img.preview");
            image.attr('src', e.target.result);
            image.attr('style', "max-height:160px;max-width:250;");
        };

        reader.readAsDataURL(input.files[0]);
    };
};

//Scrud Uploader End
function scrudLoadDatepicker() {
    $(".date").datepicker(
    {
        dateFormat: datepickerFormat,
        showWeek: datepickerShowWeekNumber,
        firstDay: datepickerWeekStartDay,
        constrainInput: false,
        numberOfMonths: eval(datepickerNumberOfMonths)
    },
    $.datepicker.regional[language]);
};