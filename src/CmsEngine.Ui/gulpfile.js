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
    img: 'img/',
    uploadedFiles: 'UploadedFiles/',
    fonts: 'fonts/',
    vendors: 'vendors/'
};

gulp.paths.concatJsDest = gulp.paths.webroot + 'js/site.min.js';
gulp.paths.concatCssDest = gulp.paths.webroot + 'css/site.min.css';

gulp.task('build:admin', function () {
    return gulp.src('./assets/scss/admin/admin.scss')
        .pipe(sass())
        .pipe(autoprefixer())
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css))
        .pipe(cssmin())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css));
});

gulp.task('build:admin-scripts', function () {
    return gulp.src('./assets/js/admin/*.js')
        .pipe(gulp.dest(gulp.paths.temp + gulp.paths.js))
        .pipe(concat('admin.js'))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js))
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js));
});

gulp.task('build:site', function () {
    return gulp.src('./assets/scss/site/site.scss')
        .pipe(sass())
        .pipe(autoprefixer())
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css))
        .pipe(cssmin())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.css));
});

gulp.task('build:site-scripts', function () {
    return gulp.src('./assets/js/site/*.js')
        .pipe(gulp.dest(gulp.paths.temp + gulp.paths.js))
        .pipe(concat('site.js'))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js))
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(gulp.paths.webroot + gulp.paths.js));
});

gulp.task('copy:images', function () {
    return gulp.src('./assets/img/**/*')
        .pipe(gulp.dest(gulp.paths.webroot + '/img'));
});

gulp.task('clean:dist', function () {
    return del([
        gulp.paths.webroot + gulp.paths.css + '**',
        gulp.paths.webroot + gulp.paths.img + '**',
        gulp.paths.webroot + gulp.paths.js + '**',
        '!' + gulp.paths.webroot + gulp.paths.uploadedFiles
    ]);
});

gulp.task('default',
    gulp.series(
        'clean:dist',
        gulp.parallel('copy:images', 'build:admin', 'build:admin-scripts', 'build:site', 'build:site-scripts'),
        'build:vendors', 'clean:min-min'
    )
);
