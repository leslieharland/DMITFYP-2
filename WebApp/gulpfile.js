var gulp = require('gulp');
var cssmin = require('gulp-cssmin');
var rename = require('gulp-rename');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var rimraf = require("rimraf");
var cleanCSS = require('gulp-clean-css');
var merge = require('merge-stream');

gulp.task("minify", function() {

    var streams = [
        gulp.src(["wwwroot/js/*.js", "wwwroot/js/**/*.js",
            'wwwroot/js/!bootstrap/*',
            'wwwroot/js/!jquery/*',
            'wwwroot/js/!tinymce/*'
        ])
        .pipe(uglify())
        .pipe(concat("app.min.js"))
        .pipe(gulp.dest("wwwroot/lib/site")),

    ];

    return merge(streams);
});

gulp.task('mincss', function() {
    var streams = [
        gulp.src(["wwwroot/css/*.css", "wwwroot/css/**/*.css"])
        .pipe(cssmin())
        .pipe(concat("app.min.css"))
        .pipe(cleanCSS({
            compatibility: 'ie8'
        }))
        .pipe(gulp.dest("wwwroot/lib/site"))
    ];

    return merge(streams);
});

// Dependency Dirs
var deps = {
    "jquery": {
        "dist/*": ""
    },
    "bootstrap": {
        "dist/**/*": ""
    },
    "@fortawesome/fontawesome-free-webfonts": {
        "**/*": ""
    }
};

gulp.task("clean", function(cb) {
    return rimraf("wwwroot/lib/", cb);
});

gulp.task("scripts", function() {

    var streams = [];

    for (var prop in deps) {
        console.log("Prepping Scripts for: " + prop);
        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/lib/" + prop + "/" + deps[prop][itemProp])));
        }
    }

    return merge(streams);

});


gulp.task('default',
    gulp.series('clean', gulp.parallel('scripts', 'minify', 'mincss')));