(function(){
  'use strict()';

  var app = angular.module('crossroads.core')
    .controller('ImpersonateController', require('./impersonate.controller'));

  require('./impersonate.html');

})();