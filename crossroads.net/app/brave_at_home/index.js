var app = angular.module('crossroads');
require('./braveAtHome.html');
//require('./braveLogin.html');
require('./braveIntro1.html');
require('./braveIntro2.html');
require('./braveIntro3.html');

app.controller('BraveHomeController', require('./braveHome.controller'));
