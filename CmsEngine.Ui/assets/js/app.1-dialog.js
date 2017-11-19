// Dialog
CmsEngine.Dialog.Open = function (urlPart, dialogId, dialogType, title, cancelButton, confirmButton) {
  var body = "Unable to load the content";
  var urlPartial = '';

  if (CmsEngine.ClickedElement.href == undefined) {
    urlPartial = CmsEngine.ClickedElement[0].href;
  }
  else {
    urlPartial = CmsEngine.ClickedElement.href;
  }

  if (urlPartial.indexOf(urlPart) !== -1) {
    urlPartial = urlPartial.replace(urlPart, urlPart + 'Partial');
  }

  $(dialogId).modal("show");

  $.get(urlPartial, { "_": $.now() }, function (data) {
    if (data) {
      CmsEngine.Configure.Dialog(dialogId, dialogType, title, data, cancelButton, confirmButton);
      if ($('.selectpicker') != undefined) {
        $('.selectpicker').selectpicker('render');
      }

      if (dialogType == 'new-ticket' || dialogType == 'edit-ticket') {
        CmsEngine.Configure.TicketDateFields();
      }
    }
  }).fail(function () {
    alert('Error: Could not load the dialog');
    $(dialogId).modal("hide");
  });
};

CmsEngine.Dialog.Restore = function () {
  $(this).find(".modal-body").html("Loading...");
};
