'use strict';

var gulp = require('gulp');
var del = require('del');
var rename = require("gulp-rename");
var runSequence = require('run-sequence');
var sass = require('gulp-sass');
var autoprefixer = require('gulp-autoprefixer');
var cssmin = require('gulp-cssmin');

var vendors = './scss/vendors/';

gulp.task('compile-vendors:clean', function () {
  return del(gulp.paths.cssVendorDest);
});

gulp.task('compile-vendors:sass', function () {
  return gulp.src('./assets/scss/vendors/**/*.scss')
    .pipe(sass().on('error', sass.logError))
    .pipe(autoprefixer())
    .pipe(rename({ dirname: '' }))
    .pipe(gulp.dest(gulp.paths.cssVendorDest))
    .pipe(cssmin())
    .pipe(rename({ suffix: '.min' }))
    .pipe(rename({ dirname: '' }))
    .pipe(gulp.dest(gulp.paths.cssVendorDest));
});

gulp.task('compile-vendors', function (callback) {
  runSequence('compile-vendors:clean', 'compile-vendors:sass', callback);
});
