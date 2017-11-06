// Dialog events
CmsEngine.Dialog.Events.Delete = function () {
  // Should cover all delete scenarios coming from tables
  $.post(CmsEngine.ClickedElement[0].href, function (data) {
    if (data.isError == false) {
      $("#general-dialog").modal("hide");
      alert(data.message);
      CmsEngine.ClickedElement.closest("tr").hide("fast");
    }
    else {
      alert("Error: " + data.message);
    }
  });
};

CmsEngine.Dialog.Events.BulkDelete = function () {
  // Should cover all bulk delete scenarios coming from tables

  var itemsToDelete = [];
  CmsEngine.SelectedElements.each(function () {
    itemsToDelete.push($(this).val());
  });

  $.post(CmsEngine.ClickedElement[0].href, { 'id[]': itemsToDelete }, function (data) {
    if (data.isError == false) {
      $("#general-dialog").modal("hide");
      alert(data.message);
      CmsEngine.SelectedElements.closest("tr").hide("fast");
    }
    else {
      alert("Error: " + data.message);
    }
  });
};
