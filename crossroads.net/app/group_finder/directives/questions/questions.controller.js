(function(){
  'use strict';

  module.exports = QuestionsCtrl;

  QuestionsCtrl.$inject = ['$timeout', '$rootScope', '$scope', '$state', '$stateParams', '$window', 'Responses'];

  function QuestionsCtrl($timeout, $rootScope, $scope, $state, $stateParams, $window, Responses) {


    // ------------------------ Properties

    $scope.responses = Responses.data;
    $scope.totalQuestions = _.size($scope.questions);

    Object.defineProperty($scope, 'nextBtn', {
      get: function() {
        return $scope.isPrivateGroup() ? 'Skip' : ($scope.currentQuestion().next || 'Next');
      }
    });

    // ------------------------ Methods

    $scope.previousQuestion = function() {
      $scope.step--;
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
        $state.go('group_finder.' + $scope.mode + '.review');
      } else if($scope.step === $scope.totalQuestions) {
        $state.go('group_finder.' + $scope.mode + '.review');
      } else {
        $scope.step++;
      }
    };

    $scope.isPrivateGroup = function() {
      return ($scope.currentKey() === 'open_spots' && $scope.currentResponse() === 0);
    };

    $scope.currentResponse = function() {
      return $scope.responses[$scope.currentKey()];
    };

    $scope.currentKey = function() {
      return _.pluck($scope.questions, 'key')[$scope.step - 1];
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
      $scope.step = 1;
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
