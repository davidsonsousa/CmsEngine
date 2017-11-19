
// Main
CmsEngine.SetupListeners = function () {
  $('#main-menu-toggle').click(function () {
    var t = $('.main').attr('class'),
      n = parseInt(t.replace(/^\D+/g, '')),
      r = n - 2,
      i = 'span' + r;
    if ($(this).hasClass('open')) {
      $(this).removeClass('open').addClass('close');
      $('.main').addClass('full');
      $('.navbar-brand').addClass('noBg');
      $('.sidebar').hide();
    }
    else {
      $(this).removeClass('close').addClass('open');
      $('.main').removeClass('full');
      $('.navbar-brand').removeClass('noBg');
      $('.sidebar').show();
    }
  });

  $('#main-menu-min').click(function () {
    if ($(this).hasClass('full')) {
      $(this).removeClass('full').addClass('minified').find('i').removeClass('fa-angle-double-left').addClass('fa-angle-double-right');
      $('body').addClass('sidebar-minified');
      $('.main').addClass('sidebar-minified');
      $('.sidebar').addClass('minified');
      $('.dropmenu > .chevron').removeClass('opened').addClass('closed');
      $('.dropmenu').parent().find('ul').hide();
      $('.sidebar > div > ul > li > a > .chevron').removeClass('closed').addClass('opened');
      $('.sidebar > div > ul > li > a').addClass('open');
    }
    else {
      $(this).removeClass('minified').addClass('full').find('i').removeClass('fa-angle-double-right').addClass('fa-angle-double-left');
      $('body').removeClass('sidebar-minified');
      $('.main').removeClass('sidebar-minified');
      $('.sidebar').removeClass('minified');
      $('.sidebar > div > ul > li > a > .chevron').removeClass('opened').addClass('closed');
      $('.sidebar > div > ul > li > a').removeClass('open');
    }
  });

  $('#table-list').on('click', '.bulk-delete-select-all', function (e) {
    $(':checkbox[name=items-to-delete]').prop('checked', $(this).is(':checked'));
    $('#bulk-delete').attr('disabled', $(this).is(':checked') === false);
  });

  $('#table-list').on('change', ':checkbox[name=items-to-delete]', function (e) {
    $('#bulk-delete').attr('disabled', $(this).is(':checked') === false);
  });

  // Modal Delete - Generic, should cover all delete actions
  $('#table-list').on('click', '.remove-link, .delete-link', function (e) {
    e.preventDefault();
    CmsEngine.ClickedElement = $(this);
    $('#general-dialog').modal('show');
    CmsEngine.Configure.Dialog('#general-dialog', 'generic-delete', 'Delete', CmsEngine.ClickedElement.attr('data-message'), 'No', 'Yes');
  });

  // Modal Bulk Delete - Generic, should cover all bulk delete actions
  $('#bulk-delete').on('click', function (e) {
    e.preventDefault();
    CmsEngine.ClickedElement = $(this);
    CmsEngine.SelectedElements = $('input:checkbox[name=items-to-delete]:checked');

    if (CmsEngine.SelectedElements.length === 0) {
      return;
    }

    var items = "";
    CmsEngine.SelectedElements.closest('tr').find('td:nth-child(2)').each(function () {
      items += "<li>" + $(this).html() + "</li>";
    });
    var message = 'Do you want to <span class="text-danger">delete</span> the items below?<ul>' + items + '</ul>';

    $('#general-dialog').modal('show');
    CmsEngine.Configure.Dialog('#general-dialog', 'generic-bulk-delete', 'Delete', message, 'No', 'Yes');
  });

  // Modal primary button action
  $('#general-dialog .btn-primary').click(function () {
    var dialogType = $('#general-dialog').attr('data-dialog-type');

    switch (dialogType) {
      //case 'new-ticket':
      //case 'edit-ticket':
      //	CmsEngine.Dialog.Events.SaveTicket();
      //	break;
      //case 'approve-ticket':
      //	CmsEngine.Dialog.Events.ChangeTicketStatus(true);
      //	break;
      //case 'deny-ticket':
      //	CmsEngine.Dialog.Events.ChangeTicketStatus(false);
      //	break;
      //case 'delete-ticket':
      //	CmsEngine.Dialog.Events.DeleteTicket();
      //	break;
      case 'generic-delete':
        CmsEngine.Dialog.Events.Delete();
        break;
      case 'generic-bulk-delete':
        CmsEngine.Dialog.Events.BulkDelete();
        break;
      //case 'adjust-compensation':
      //	CmsEngine.Dialog.Events.SaveCompensation();
      //	break;
      //case 'forgot-pswd':
      //	CmsEngine.Dialog.Events.ForgotPassword();
      //	break;
      default:
        alert('Oh... You shouldn\'t have clicked here...');
        break;
    }
  });

  // Create slug field
  $('.slugify').keyup(function () {
    $('.slug').val(CmsEngine.Utils.ConvertToSlug($(this).val()));
  });

  /* ---------- Main Menu Open/Close, Min/Full ---------- */
  $('.sidebar-toggler').click(function () {
    $('body').toggleClass('sidebar-hidden');
    CmsEngine.Navigation.ResizeBroadcast();
  });

  $('.sidebar-minimizer').click(function () {
    $('body').toggleClass('sidebar-minimized');
    CmsEngine.Navigation.ResizeBroadcast();
  });

  $('.brand-minimizer').click(function () {
    $('body').toggleClass('brand-minimized');
  });

  $('.aside-menu-toggler').click(function () {
    $('body').toggleClass('aside-menu-hidden');
    CmsEngine.Navigation.ResizeBroadcast();
  });

  $('.mobile-sidebar-toggler').click(function () {
    $('body').toggleClass('sidebar-mobile-show');
    CmsEngine.Navigation.ResizeBroadcast();
  });

  $('.sidebar-close').click(function () {
    $('body').toggleClass('sidebar-opened').parent().toggleClass('sidebar-opened');
  });

  /* ---------- Disable moving to top ---------- */
  $('a[href="#"][data-top!=true]').click(function (e) {
    e.preventDefault();
  });

  // Cards actions
  $(document).on('click', '.card-actions a', function (e) {
    e.preventDefault();

    if ($(this).hasClass('btn-close')) {
      $(this).parent().parent().parent().fadeOut();
    } else if ($(this).hasClass('btn-minimize')) {
      var $target = $(this).parent().parent().next('.card-block');
      if (!$(this).hasClass('collapsed')) {
        $('i', $(this)).removeClass($.panelIconOpened).addClass($.panelIconClosed);
      } else {
        $('i', $(this)).removeClass($.panelIconClosed).addClass($.panelIconOpened);
      }

    } else if ($(this).hasClass('btn-setting')) {
      $('#myModal').modal('show');
    }

  });
};

// Script initialization
CmsEngine.Init = function () {
  CmsEngine.Configure.ToolTips('[rel="tooltip"],[data-rel="tooltip"]');
  CmsEngine.Configure.PopOver('[rel="popover"],[data-rel="popover"],[data-toggle="popover"]');
  this.SetupListeners();
};

$(function () {
  CmsEngine.Init();
});
