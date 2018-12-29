// File upload
CmsEngine.FileUpload.Upload = function (fileInput, urlAction, preview, inputPath, inputPathThumb) {
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

CmsEngine.FileUpload.Remove = function (button, preview, inputPath, inputPathThumb) {
  $(button).on('click', function (e) {
    e.preventDefault();
    $(preview).html('');
    $(inputPath).val('');
    $(inputPathThumb).val('');

    // TODO: Delete image if exists
  });
};
