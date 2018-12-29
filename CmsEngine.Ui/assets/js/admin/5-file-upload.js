// File upload
CmsEngine.FileUpload.UploadHeader = function (fileInput, urlAction, preview, inputPath, inputPathThumb) {
  $(fileInput).fileupload({
    dataType: 'json',
    url: urlAction,
    autoUpload: true,
    done: function (e, data) {
      $(preview).html('<img src="' + data.result[0].path + data.result[0].thumbnailname + '" />');
      $(inputPath).val(data.result[0].path + data.result[0].filename);
      $(inputPathThumb).val(data.result[0].path + data.result[0].thumbnailname);
    },
    progressall: function (e, data) {
      var progress = parseInt(data.loaded / data.total * 100, 10);
      $('#progress .progress-bar').css(
        'width',
        progress + '%'
      );
    }
  });
};

CmsEngine.FileUpload.UploadImagesToEditor = function (fileInput, urlAction) {
  $(fileInput).fileupload({
    dataType: 'json',
    url: urlAction,
    autoUpload: true,
    done: function (e, data) {
      $.each(data.result, function (index, file) {
        tinymce.activeEditor.execCommand('mceInsertContent', false, '<img src="' + file.path + file.filename + '" alt="' + file.filename + '" />');
      });
    },
    progressall: function (e, data) {
      var progress = parseInt(data.loaded / data.total * 100, 10);
      $('#progress .progress-bar').css(
        'width',
        progress + '%'
      );
    }
  });
};

CmsEngine.FileUpload.UploadFilesToEditor = function (fileInput, urlAction) {
  $(fileInput).fileupload({
    dataType: 'json',
    url: urlAction,
    autoUpload: true,
    done: function (e, data) {
      $.each(data.result, function (index, file) {
        tinymce.activeEditor.execCommand('mceInsertContent', false, '<span class="download-link"><a href="' + file.path + file.filename + '">' + file.filename + ' (' + file.size + ')</a></span>');
      });
    },
    progressall: function (e, data) {
      var progress = parseInt(data.loaded / data.total * 100, 10);
      $('#progress .progress-bar').css(
        'width',
        progress + '%'
      );
    }
  });
};

CmsEngine.FileUpload.Remove = function (button, preview, inputPath, inputPathThumb) {
  $(button).on('click', function (e) {
    e.preventDefault();
    $(preview).html('');
    $(inputPath).val('');
    $(inputPathThumb).val('');

    // TODO: Delete image if exists
  });
};
