(function(){
  'use strict';

  module.exports = QuestionsCtrl;

  QuestionsCtrl.$inject = ['$scope', '$state', '$stateParams', '$window', 'SERIES'];

  function QuestionsCtrl($scope, $state, $stateParams, $window, SERIES) {

    // ------------------------ Properties

    $scope.totalQuestions = _.size($scope.questions);
    $scope.currentIteration = parseInt($stateParams.step) || 1;

    // ------------------------ Methods

    $scope.previousQuestion = function() {
      $scope.currentIteration--;
      $state.go(SERIES.permalink + '.' + $scope.mode, { step: $scope.currentIteration });
    };

    $scope.nextQuestion = function() {
      if($scope.isRequired() && !_.isEmpty($scope.currentErrorFields())) {
        $scope.applyErrors();
      } else {
        $scope.currentIteration++;
        $state.go(SERIES.permalink + '.' + $scope.mode, { step: $scope.currentIteration });
      }
    };

    $scope.currentKey = function() {
      return _.pluck($scope.questions, 'key')[$scope.currentIteration - 1];
    };

    $scope.currentQuestion = function() {
      return $scope.questions[$scope.currentIteration - 1];
    };

    $scope.startOver = function() {
      $scope.currentIteration = 1;
    };

    $scope.reviewResponses = function() {
      $state.go(SERIES.permalink + '.' + $scope.mode + '_review');
    };

    $scope.isRequired = function() {
      return $scope.currentQuestion().required === true;
    };

    $scope.requiredFields = function() {
      var visibleFields = $('input:visible, select:visible, textarea:visible');
      return _.map(visibleFields, function(el,i) {
        return $(el).attr('name');
      });
    };

    $scope.currentErrorFields = function() {
      return _.chain($scope.requiredFields())
              .map(function(name) {
                var el = $('[name=' + name + ']');
                return el.val() === '' || el.val().indexOf('undefined') > -1 ? el : false;
              })
              .compact()
              .value();
    };

    $scope.applyErrors = function() {
      $('div.has-error:visible').removeClass('has-error');
      _.each($scope.currentErrorFields(), function(el){
        if(el.val() === '' || el.val().indexOf('undefined') > -1) {
          el.closest('div').addClass('has-error');
        }
      });
    };

  }

})();
