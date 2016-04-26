(function() {
  'use strict()';

  var MODULE = 'crossroads.mptools';

  require('./adminTool.html');
  require('./siteFilter.html');

  var app = angular.module(MODULE);
  app.controller('siteFilter', require('./siteFilter.directive'));
  app.controller('AdminToolController', require('./adminTool.controller'));
  app.config(require('./adminTool.routes'));

})();
