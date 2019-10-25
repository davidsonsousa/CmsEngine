var CmsEngine = CmsEngine || {};
CmsEngine.Events = CmsEngine.Events || {};
CmsEngine.Configure = CmsEngine.Configure || {};
CmsEngine.Dialog = CmsEngine.Dialog || {};
CmsEngine.Dialog.Events = CmsEngine.Dialog.Events || {};
CmsEngine.FileUpload = CmsEngine.FileUpload || {};
CmsEngine.Utils = CmsEngine.Utils || {};
CmsEngine.Navigation = CmsEngine.Navigation || {};

'use strict';

CmsEngine.Configure.AjaxStart = function (time) {
    $(".loading").fadeIn(time);
};

CmsEngine.Configure.AjaxComplete = function (time) {
    $(".loading").fadeOut(time);
};

// Example: CmsEngine.Configure.DatePicker('#input-text', 'mm/dd/yyyy', 0, '', []);
CmsEngine.Configure.DatePicker = function (selector, dateFormat, firstWeekDay, daysWeekDisabled, dtDisabled) {
    if (dateFormat === "") {
        dateFormat = "mm/dd/yyyy";
    }

    $(selector).datepicker({
        format: dateFormat,
        weekStart: firstWeekDay,
        maxViewMode: 2,
        daysOfWeekDisabled: daysWeekDisabled,
        daysOfWeekHighlighted: "0,6",
        autoclose: true,
        todayHighlight: true,
        datesDisabled: dtDisabled
    });
};

CmsEngine.Configure.DateTimePicker = function (selector) {
    $(selector).datetimepicker({ locale: 'en' });
};

CmsEngine.Configure.ToolTips = function (selector) {
    $(selector).tooltip({ "placement": "bottom", delay: { show: 400, hide: 200 } });
};

CmsEngine.Configure.PopOver = function (selector) {
    $(selector).popover();
};

CmsEngine.Configure.DataTable = function (route) {
    $("#table-list").DataTable({
        "processing": true,
        "serverSide": true,
        "columnDefs": [
            {
                "targets": 0, // Targeting the first column
                "searchable": false,
                "orderable": false,
                "width": "15px",
                "data": null,
                "render": function (data, type, row, meta) {
                    return '<input type="checkbox" name="items-to-delete" value="' + row[row.length - 1] + '">';
                }
            },
            {
                "targets": -1, // Targeting the last column
                "searchable": false,
                "orderable": false,
                "width": "85px",
                "data": null,
                "render": function (data, type, row, meta) {
                    return '<a href="' + route + '/edit/' + row[row.length - 1] + '" role="button" class="btn btn-secondary" data-toggle="tooltip" data-placement="top" title="Edit"><span class="far fa-edit"></span></a>'
                        + '<a href="' + route + '/delete/' + row[row.length - 1] + '" role="button" class="btn btn-danger delete-link" data-toggle="tooltip" data-placement="top" data-message="Do you really want to <span class=&quot;text-danger&quot;>delete</span> <strong>' + row[1] + '</strong>?" title="Delete"><span class="far fa-trash-alt"></span></a>';
                }
            }],
        "ajax": {
            type: "POST",
            //contentType: "application/json",
            url: route + "/getdata",
            //data: function (d) {
            //	return JSON.stringify({ parameters: d });
            //}
        }
    });
};

CmsEngine.Configure.Dialog = function (dialogId, dialogType, title, body, cancelButton, confirmButton) {
    $(dialogId + " .modal-title").html(title);
    $(dialogId).attr("data-dialog-type", dialogType);
    $(dialogId + " .btn-primary").html(confirmButton);

    if (body != undefined) {
        $(dialogId + " .modal-body").html(body);
    }
    else {
        $(dialogId + " .modal-body").html("Do you really want to delete this item?");
    }

    if (cancelButton.length > 0) {
        $(dialogId + " .btn-secondary").show();
        $(dialogId + " .btn-secondary").html(cancelButton);
    }
    else {
        $(dialogId + " .btn-secondary").hide();
    }

    // Selectize
    //$('.selectize').selectize({
    //	create: false,
    //	sortField: 'text'
    //});
};

CmsEngine.Configure.TinyMCE = function (selector, height) {
    tinymce.init({
        selector: selector,
        height: height,
        content_css: "",
        relative_urls: false,
        remove_script_host: true,
        document_base_url: "/",
        convert_urls: true,
        plugins: 'print preview fullpage paste importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons',
        //plugins: [
            //"advlist autolink lists link image imagetools charmap print preview anchor",
            //"searchreplace visualblocks code fullscreen",
            //"insertdatetime media table contextmenu paste"
        //],
        imagetools_cors_hosts: ['picsum.photos'],
        //toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | code",
        toolbar: 'undo redo | bold italic underline | formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | removeformat | fullscreen  preview | insertfile link codesample code',
        menubar: 'file edit view insert format tools table',
        extended_valid_elements: 'script[language|type|src]',
        autosave_ask_before_unload: true,
        autosave_interval: "30s",
        autosave_prefix: "{path}{query}-{id}-",
        autosave_restore_when_empty: false,
        autosave_retention: "2m",
        image_advtab: true,
        contextmenu: "link image imagetools table",
        setup: function (editor) {
            editor.on('SaveContent', function (e) {
                e.content = e.content.replace(/&#39/g, "&apos");
            });
        }
    });
};
