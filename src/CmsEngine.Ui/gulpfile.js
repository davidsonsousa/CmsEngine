/// <binding />
'use strict';

var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var del = require('del');
var sass = require('gulp-sass')(require('sass'));
var autoprefixer = require('gulp-autoprefixer');
var cssmin = require('gulp-cssmin');
var rename = require('gulp-rename');

require('require-dir')('./gulp-tasks');

gulp.paths = {
    webroot: './wwwroot/',
    temp: './wwwroot/temp/',
    css: 'css/',
    js: 'js/',
    media: 'media/',
    uploadedFiles: 'UploadedFiles/',
    fonts: 'fonts/',
    vendors: 'vendors/'
};

gulp.paths.concatJsDest = gulp.paths.webroot + 'js/site.min.js';
gulp.paths.concatCssDest = gulp.paths.webroot + 'css/site.min.css';

//gulp.task('build:admin', function () {
//    return gulp.src('./assets/scss/admin/admin.scss')
//        .pipe(sass())
//        .pipe(autoprefixer())
//        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css))
//        .pipe(cssmin())
//        .pipe(rename({ suffix: '.min' }))
//        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css));
//});

gulp.task('build:admin-custom-scripts', function () {
    return gulp.src('./assets/admin/js/custom/*.js')
        .pipe(gulp.dest(gulp.paths.temp + gulp.paths.js))
        .pipe(concat('admin.js'))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js))
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js));
});

gulp.task('build:site-styles', function () {
    return gulp.src('./assets/site/scss/site.scss')
        .pipe(sass())
        .pipe(autoprefixer())
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css))
        .pipe(cssmin())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css));
});

gulp.task('build:site-scripts', function () {
    return gulp.src('./assets/site/js/*.js')
        .pipe(gulp.dest(gulp.paths.temp + gulp.paths.js))
        .pipe(concat('site.js'))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js))
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js));
});

gulp.task('copy:admin-template-styles', function () {
    return gulp.src('./assets/admin/css/*.css')
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css + '/admin'));
});

gulp.task('copy:admin-template-vendor-styles', function () {
    return gulp.src('./assets/admin/css/vendors/*.css')
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css + '/admin/vendors'));
});

gulp.task('copy:admin-template-vendor-scripts', function () {
    return gulp.src('./assets/admin/js/*.js')
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js + '/admin'));
});

gulp.task('copy:admin-media', function () {
    return gulp.src('./assets/admin/media/**/*')
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.media));
});

gulp.task('copy:site-media', function () {
    return gulp.src('./assets/site/media/**/*')
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.media));
});

gulp.task('clean:dist', function () {
    return del([
        gulp.paths.webroot + gulp.paths.css + '**',
        gulp.paths.webroot + gulp.paths.js + '**',
        gulp.paths.webroot + gulp.paths.media + '**',
        '!' + gulp.paths.webroot + gulp.paths.uploadedFiles
    ]);
});

gulp.task('default',
    gulp.series(
        'clean:dist',
        gulp.parallel('build:admin-custom-scripts', 'build:site-scripts', 'build:site-styles',
            'copy:admin-template-styles', 'copy:admin-template-vendor-styles', 'copy:admin-template-vendor-scripts', 'copy:admin-media', 'copy:site-media'),
        'build:vendors', 'clean:min-min'
    )
);
