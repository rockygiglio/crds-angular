(function(){

  var app = angular.module('crossroads.core');
  app.filter('time', require('./time.filter'));
  app.filter('htmlToPlainText', require('./html_to_plain_text.filter'));
  
  require('./html.filter');
  require('./truncate.filter');
})()
