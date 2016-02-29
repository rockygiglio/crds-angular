(function(){
  'use strict';

  module.exports = ResponseService;

  ResponseService.$inject = ['$rootScope'];

  function ResponseService($rootScope) {
    this.data = {};

    this.clear = function(){
      this.data = {};
    };

    this.getResponse = function(definition) {
      return this.data[definition.key];
    };

    this.SaveState = function () {
      sessionStorage.userService = angular.toJson(this.data);
    };

    this.RestoreState = function () {
      var data = angular.fromJson(sessionStorage.userService);
      if (data) {
        this.data = data;
      }
    };

  }

})();
