var gulp = require('gulp');
var nugetRestore = require('gulp-nuget-restore');
var msbuild = require('gulp-msbuild');

gulp.task('nuget-restore', function () {
  return gulp.src('../IssueTracker.sln')
    .pipe(nugetRestore());
});

gulp.task('default', ['nuget-restore'], function () {
  return gulp.src('../../project.msbuild')
    .pipe(msbuild({
      stdout: true,
      targets: ['Start'],
      toolsVersion: 15,
      verbosity: 'minimal'
    }));
});
