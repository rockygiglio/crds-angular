(function(){
  'use strict';

  module.exports = ResponseService;

  ResponseService.$inject = ['ParticipantQuestionService'];

  function ResponseService(ParticipantQuestionService) {
    this.data = {};

    this.clear = function(){
      this.data = {};
    };

    this.lookup = function() {
      if (ParticipantQuestionService.lookup.loaded) {
        sessionStorage.setItem('lookup', angular.toJson(ParticipantQuestionService.lookup));
      } else {
        ParticipantQuestionService.lookup = angular.fromJson(sessionStorage.getItem('lookup'));
      }

      return ParticipantQuestionService.lookup;
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
      }, {responses: this.data, lookup: this.lookup()});

      return results;
    };

    this.getMultiAttributes = function(attributes) {
      var results = {};
      _.each(attributes, function(index) {
        if (_.has(this.responses, index)) {
          var answer = this.responses[index];
          _.each(answer, function(value, answerId) {
            if (value) {
              var attributeTypeId = this.lookup[answerId].attributeTypeId;
              if (!_.has(results, attributeTypeId)) {
                results[attributeTypeId] = {attributeTypeId: attributeTypeId, attributes: []};
              }

              results[attributeTypeId].attributes.push({attributeId: answerId, selected: true});
            }
          }, {lookup: this.lookup});
        }
      }, {responses: this.data, lookup: this.lookup()});

      return results;
    };

  }

})();
