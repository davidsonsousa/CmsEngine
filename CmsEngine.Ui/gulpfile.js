'use strict';

var gulp = require('gulp');
var browserSync = require('browser-sync').create();
var sass = require('gulp-sass');
var autoprefixer = require('gulp-autoprefixer');
var cssmin = require('gulp-cssmin');
var rename = require('gulp-rename');
var runSequence = require('run-sequence');

require('require-dir')('./gulp-tasks');

gulp.paths = {
  webroot: './wwwroot/'
};

//gulp.paths.js = gulp.paths.webroot + 'js/**/*.js';
//gulp.paths.css = gulp.paths.webroot + 'css/**/*.css';

//gulp.paths.minJs = gulp.paths.webroot + 'js/**/*.min.js';
//gulp.paths.minCss = gulp.paths.webroot + 'css/**/*.min.css';

gulp.paths.jsDest = gulp.paths.webroot + 'js/';
gulp.paths.cssDest = gulp.paths.webroot + 'css/';

gulp.paths.jsVendorDest = gulp.paths.webroot + 'js/vendor/';
gulp.paths.cssVendorDest = gulp.paths.webroot + 'css/vendor/';

gulp.paths.concatJsDest = gulp.paths.webroot + 'js/site.min.js';
gulp.paths.concatCssDest = gulp.paths.webroot + 'css/site.min.css';

gulp.task('sass', ['compile-vendors'], function () {
  return gulp.src('./assets/scss/style.scss')
    .pipe(sass())
    .pipe(autoprefixer())
    .pipe(gulp.dest(gulp.paths.cssDest))
    .pipe(cssmin())
    .pipe(rename({ suffix: '.min' }))
    .pipe(gulp.dest(gulp.paths.cssDest))
    .pipe(browserSync.stream());
});
