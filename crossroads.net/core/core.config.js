'use strict()';
(function(){

  var app = angular.module("crossroads.core");
  app.config(AppConfig);
  
  AppConfig.$inject = ['$httpProvider', '$locationProvider', '$cookies', 'datepickerConfig', 'datepickerPopupConfig'];
  
  function AppConfig($httpProvider, $locationProvider, $cookies, datepickerConfig, datepickerPopupConfig){
    $httpProvider.defaults.useXDomain = true;
    $httpProvider.defaults.headers.common['Authorization']= $cookies.get('sessionId');
    // This is a dummy header that will always be returned in any 'Allow-Header' from any CORS request. This needs to be here because of IE.
    $httpProvider.defaults.headers.common["X-Use-The-Force"] = true;

    configureDatePickersDefaults(datepickerConfig, datepickerPopupConfig);
  }

  var configureDatePickersDefaults = function (datepickerConfig, datepickerPopupConfig) {
    datepickerConfig.showWeeks = false;
    datepickerPopupConfig.showWeeks = false;
  }

})();
