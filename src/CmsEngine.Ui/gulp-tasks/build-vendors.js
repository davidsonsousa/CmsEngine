'use strict';

var gulp = require('gulp');
var uglify = require('gulp-uglify');
var del = require('del');
var rename = require("gulp-rename");
var sass = require('gulp-sass')(require('sass'));
var autoprefixer = require('gulp-autoprefixer');
var cssmin = require('gulp-cssmin');

gulp.task('clean:temp', function () {
    return del(gulp.paths.temp);
});

gulp.task('clean:min-min', function () {
    return del(gulp.paths.webroot + gulp.paths.js + gulp.paths.vendors + '*.min.min.js');
});

/** CSS **/
var vendorsCSS = [
    //'node_modules/codemirror/lib/codemirror.css',
    'node_modules/datatables.net-bs4/css/dataTables.bootstrap4.css',
    'node_modules/datatables.net-buttons-bs4/css/buttons.bootstrap4.css',
    'node_modules/@fortawesome/fontawesome-free/css/all.css',
    //'node_modules/ladda/dist/ladda-themeless.min.css',
    //'node_modules/quill/dist/quill.snow.css',
    'node_modules/simple-line-icons/css/simple-line-icons.css',
    //'node_modules/spinkit/css/spinkit.css',
    'node_modules/fancybox/dist/css/jquery.fancybox.css',
    'node_modules/select2/dist/css/select2.css',
    'node_modules/blueimp-file-upload/css/jquery.fileupload.css'
];

gulp.task('node-vendors:copyCSS', function () {
    return gulp.src(vendorsCSS)
        .pipe(gulp.dest(gulp.paths.temp + gulp.paths.css + gulp.paths.vendors));
});

gulp.task('node-vendors:minifyCSS', function () {
    return gulp.src([
        gulp.paths.temp + gulp.paths.css + gulp.paths.vendors + '*.css',
        '!' + gulp.paths.temp + gulp.paths.css + gulp.paths.vendors + '*.min.css'
    ])
        .pipe(cssmin())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css + gulp.paths.vendors));
});

gulp.task('vendors:cleanCSS', function () {
    return del(gulp.paths.webroot + gulp.paths.css + gulp.paths.vendors);
});

gulp.task('compile-vendors-admin:sass', function () {
    return gulp.src('./assets/scss/admin/vendors/**/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(autoprefixer())
        .pipe(rename({ dirname: '' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css + gulp.paths.vendors))
        .pipe(cssmin())
        .pipe(rename({ suffix: '.min' }))
        .pipe(rename({ dirname: '' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css + gulp.paths.vendors));
});

/** JavaScript **/
var vendorsJS = [
    'node_modules/bootstrap/dist/js/bootstrap.min.js',
    'node_modules/bootstrap/dist/js/bootstrap.min.js.map',
    //'node_modules/bootstrap-daterangepicker/daterangepicker.js',
    'node_modules/chart.js/dist/Chart.min.js',
    //'node_modules/codemirror/lib/codemirror.js',
    //'node_modules/codemirror/mode/markdown/markdown.js',
    //'node_modules/codemirror/mode/xml/xml.js',
    'node_modules/datatables.net/js/jquery.dataTables.js',
    'node_modules/datatables.net-bs4/js/dataTables.bootstrap4.js',
    'node_modules/datatables.net-buttons-bs4/js/buttons.bootstrap4.js',
    //'node_modules/fullcalendar/dist/fullcalendar.min.js',
    //'node_modules/fullcalendar/dist/gcal.min.js',
    //'node_modules/gaugeJS/dist/gauge.min.js',
    //'node_modules/ion-rangeslider/js/ion.rangeSlider.min.js',
    'node_modules/jquery/dist/jquery.min.js',
    'node_modules/jquery/dist/jquery.min.map',
    //'node_modules/jquery-ui-dist/jquery-ui.min.js',
    'node_modules/jquery-validation/dist/jquery.validate.min.js',
    'node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js',
    //'node_modules/jquery.maskedinput/src/jquery.maskedinput.js',
    //'node_modules/ladda/dist/ladda.min.js',
    //'node_modules/ladda/dist/spin.min.js',
    'node_modules/moment/min/moment.min.js',
    //'node_modules/quill/dist/quill.min.js',
    //'node_modules/quill/dist/quill.min.js.map',
    'node_modules/pace-progress/pace.min.js',
    'node_modules/popper.js/dist/umd/popper.min.js',
    'node_modules/popper.js/dist/umd/popper.min.js.map',
    'node_modules/select2/dist/js/select2.min.js',
    //'node_modules/toastr/toastr.js',
    'node_modules/fancybox/dist/js/jquery.fancybox.js',
    'node_modules/jquery.cookie/jquery.cookie.js',
    //'node_modules/select2/js/select2.js',
    'node_modules/blueimp-file-upload/js/vendor/jquery.ui.widget.js',
    'node_modules/blueimp-file-upload/js/jquery.fileupload.js'
];

gulp.task('node-vendors:copyJS', function () {
    return gulp.src(vendorsJS)
        .pipe(gulp.dest(gulp.paths.temp + gulp.paths.js + gulp.paths.vendors));
});

gulp.task('node-vendors:copyMAP', function () {
    return gulp.src([
        gulp.paths.temp + gulp.paths.js + gulp.paths.vendors + '*.map'
    ])
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js + gulp.paths.vendors));
});

gulp.task('node-vendors:minifyJS', function () {
    return gulp.src([
        gulp.paths.temp + gulp.paths.js + gulp.paths.vendors + '*.js'
    ])
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js + gulp.paths.vendors))
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js + gulp.paths.vendors));
});

gulp.task('vendors:cleanJS', function () {
    return del(gulp.paths.webroot + gulp.paths.js + gulp.paths.vendors);
});

/** Fonts **/
var fonts = [
    'node_modules/simple-line-icons/fonts/**',
    'assets/fonts/blog.*'
];

gulp.task('vendors:copyFonts', function () {
    return gulp.src(fonts)
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css + gulp.paths.fonts));
});

var fontawesome = [
    'node_modules/@fortawesome/fontawesome-free/webfonts/*.eot',
    'node_modules/@fortawesome/fontawesome-free/webfonts/*.ttf',
    'node_modules/@fortawesome/fontawesome-free/webfonts/*.woff',
    'node_modules/@fortawesome/fontawesome-free/webfonts/*.woff2'
];

gulp.task('vendors:copyFontAwesome', function () {
    return gulp.src(fontawesome)
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css + 'web' + gulp.paths.fonts));
});

/** TinyMCE **/
gulp.task('node-vendors:copyTinyMCE', done => {
    gulp.src([
        'node_modules/tinymce/tinymce.min.js',
        'node_modules/tinymce/plugins/**',
        'node_modules/tinymce/themes/silver/theme.min.js',
        'node_modules/tinymce/skins/ui/oxide/skin.min.css',
        'node_modules/tinymce/skins/ui/oxide/content.min.css',
        'node_modules/tinymce/skins/ui/oxide/fonts/tinymce-mobile.woff'
    ], { base: './node_modules' })
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js + gulp.paths.vendors));
    done();
});

/** Datetime picker **/
gulp.task('node-vendors:copyDatetimePicker', done => {
    gulp.src([
        'node_modules/pc-bootstrap4-datetimepicker/build/js/bootstrap-datetimepicker.min.js',
        'node_modules/pc-bootstrap4-datetimepicker/build/css/bootstrap-datetimepicker.min.css'
    ], { base: './node_modules' })
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js + gulp.paths.vendors));
    done();
});

/** Build all **/
gulp.task('build:vendors',
    gulp.series(
        gulp.parallel('vendors:cleanCSS', 'vendors:cleanJS'),
        gulp.parallel('node-vendors:copyCSS', 'node-vendors:copyJS', 'compile-vendors-admin:sass', 'node-vendors:copyMAP'),
        gulp.parallel('node-vendors:minifyCSS', 'node-vendors:minifyJS'),
        gulp.parallel('node-vendors:copyTinyMCE', 'node-vendors:copyDatetimePicker', 'vendors:copyFonts', 'vendors:copyFontAwesome'),
        'clean:temp'
    )
);
