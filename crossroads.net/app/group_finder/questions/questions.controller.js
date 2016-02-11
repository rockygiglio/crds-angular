(function(){
  'use strict';

  module.exports = QuestionsCtrl;

  QuestionsCtrl.$inject = ['$rootScope', '$scope', '$state', '$stateParams', '$window', 'SERIES'];

  function QuestionsCtrl($rootScope, $scope, $state, $stateParams, $window, SERIES) {

    // ------------------------ Properties

    $scope.totalQuestions = _.size($scope.questions);
    $scope.currentIteration = parseInt($stateParams.step) || 1;

    Object.defineProperty($scope, 'nextBtn', {
      get: function() {
        return $scope.isPrivateGroup() ? 'Skip' : ($scope.currentQuestion().next || 'Next');
      }
    });

    // ------------------------ Methods

    $scope.previousQuestion = function() {
      $scope.currentIteration--;
      $state.go(SERIES.permalink + '.' + $scope.mode, { step: $scope.currentIteration });
    };

    $scope.nextQuestion = function() {
      if($scope.isRequired() && !_.isEmpty($scope.currentErrorFields())) {
        $scope.applyErrors();
      } else {
        $scope.go();
      }
    };

    $scope.go = function() {
      if($scope.mode === 'host' && $scope.isPrivateGroup()) {
        // TODO if private group skip review, save and show confirmation page.
        $state.go(SERIES.permalink + '.' + $scope.mode + '_review');
      } else if($scope.currentIteration === $scope.totalQuestions) {
        $state.go(SERIES.permalink + '.' + $scope.mode + '_review');
      } else {
        $scope.currentIteration++;
        $state.go(SERIES.permalink + '.' + $scope.mode, { step: $scope.currentIteration });
      }
    };

    $scope.isPrivateGroup = function() {
      return ($scope.currentKey() === 'open_spots' && $scope.currentResponse() === 0);
    };

    $scope.currentResponse = function() {
      return $scope.responses[$scope.currentQuestion().model][$scope.currentKey()];
    };

    $scope.currentKey = function() {
      return _.pluck($scope.questions, 'key')[$scope.currentIteration - 1];
    };

    $scope.currentQuestion = function() {
      return _.find($scope.questions, function(obj){
        return obj['key'] === $scope.currentKey();
      });
    };

    $scope.renderBtn = function(dir) {
      return dir;
    };

    $scope.startOver = function() {
      $scope.currentIteration = 1;
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

    $rootScope.$on('$viewContentLoaded',function(event){
      setTimeout(function() {
        var el = $('input[type=text], input[type=number]').filter('[name*=' + $scope.currentKey() + ']').first();
            el.focus();
      }, 100);
    });

  }

})();
