/// <binding />
'use strict';

var gulp = require('gulp');
var browserSync = require('browser-sync').create();
var del = require('del');
var sass = require('gulp-sass');
var autoprefixer = require('gulp-autoprefixer');
var cssmin = require('gulp-cssmin');
var rename = require('gulp-rename');
var runSequence = require('run-sequence');

require('require-dir')('./gulp-tasks');

gulp.paths = {
  webroot: './wwwroot/',
  temp: './wwwroot/temp/',
  css: 'css/',
  js: 'js/',
  fonts: 'fonts/',
  vendors: 'vendors/'
};

//gulp.paths.js = gulp.paths.webroot + 'js/**/*.js';
//gulp.paths.css = gulp.paths.webroot + 'css/**/*.css';

//gulp.paths.minJs = gulp.paths.webroot + 'js/**/*.min.js';
//gulp.paths.minCss = gulp.paths.webroot + 'css/**/*.min.css';

//gulp.paths.jsDest = gulp.paths.webroot + 'js/';
//gulp.paths.cssDest = gulp.paths.webroot + 'css/';

//gulp.paths.jsVendorDest = gulp.paths.webroot + 'js/vendor/';
//gulp.paths.cssVendorDest = gulp.paths.webroot + 'css/vendor/';

gulp.paths.concatJsDest = gulp.paths.webroot + 'js/site.min.js';
gulp.paths.concatCssDest = gulp.paths.webroot + 'css/site.min.css';

gulp.task('build:styles', function () {
  return gulp.src('./assets/scss/style.scss')
    .pipe(sass())
    .pipe(autoprefixer())
    .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css))
    .pipe(cssmin())
    .pipe(rename({ suffix: '.min' }))
    .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css));
    //.pipe(browserSync.stream());
});

gulp.task('build:scripts', function () {
  return gulp.src('./assets/js/**/*')
    .pipe(gulp.dest(gulp.paths.webroot + '/js'));
});

gulp.task('copy:images', function () {
  return gulp.src('./assets/img/**/*')
    .pipe(gulp.dest(gulp.paths.webroot + '/img'));
});

gulp.task('clean:dist', function () {
  return del(gulp.paths.webroot + './*');
});

gulp.task('default', function (callback) {
  runSequence(
    'clean:dist', 'copy:images',
    'build:styles', 'build:scripts', 'build:vendors',
    callback);
});
