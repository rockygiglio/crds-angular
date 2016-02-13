(function(){
  'use strict';

  module.exports = ResponseService;

  ResponseService.$inject = ['$log'];

  function ResponseService($log) {

    this.data = {
      group: [],
      member: []
    };

    this.clear = function(){
      this.data = {};
    };

    this.getResponse = function(definition) {
      return this.data[definition.model][definition.key];
    };

  }

})();
