var app = angular.module('crossroads');
require('./braveAtHome.html');
//require('./braveLogin.html');
require('./braveIntro1.html');

app.controller('BraveHomeController', require('./braveHome.controller'));
