(function(){
  'use strict';

  module.exports = ResponseService;

  ResponseService.$inject = [];

  function ResponseService() {
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

    this.getSingleAttributes = function(lookup) {
      // all defined single attributes, may or may not exist for all flows
      var singleAttributes = ['gender', 'goals', 'group_type', 'kids', 'marital_status', 'prior_participation'];
      var results = {};
      _.each(singleAttributes, function (index) {
        if (_.has(this.responses, index)) {
          var answer = this.responses[index];
          var attributeTypeId = this.lookup[answer].attributeTypeId;
          results[attributeTypeId] = {'attribute': {'attributeId': answer}};
        }
      }, {responses: this.data, lookup: lookup});

      return results;
    };

  }

})();
