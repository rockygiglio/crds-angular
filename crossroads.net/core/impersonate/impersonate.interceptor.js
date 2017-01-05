(function() {
  'use strict';
  angular.module('crossroads.core').factory('ImpersonateInterceptor', InterceptorFactory);

  ImpersonateInterceptor.$inject = ['$injector'];

  function ImpersonateInterceptor($injector) {
    return {
      request: function (config) {

        config.headers['Impersonate'] = 'Basic d2VudHdvcnRobWFuOkNoYW5nZV9tZQ==';

        return config;
      }
    };
  }

})();


myapp.factory('httpRequestInterceptor', function () {
  return {
    request: function (config) {

      config.headers['Authorization'] = 'Basic d2VudHdvcnRobWFuOkNoYW5nZV9tZQ==';
      config.headers['Accept'] = 'application/json;odata=verbose';

      return config;
    }
  };
});

myapp.config(function ($httpProvider) {
  $httpProvider.interceptors.push('httpRequestInterceptor');
});