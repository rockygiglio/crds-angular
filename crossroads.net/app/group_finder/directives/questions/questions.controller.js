(function(){
  'use strict';

  module.exports = QuestionsCtrl;

  QuestionsCtrl.$inject = [
    '$location',
    '$timeout',
    '$scope',
    '$state',
    '$window',
    'Responses'
  ];

  function QuestionsCtrl( $location,
                          $timeout,
                          $scope,
                          $state,
                          $window,
                          Responses) {

    $scope.initialize = function() {
      $scope.step = $location.hash() || $scope.step;
      $scope.responses = Responses.data;
      $scope.totalQuestions = _.size($scope.questions);

      Object.defineProperty($scope, 'nextBtn', {
        get: function() {
          return $scope.isPrivateGroup() ? 'Make My Group Private' : ($scope.currentQuestion().next || 'Next');
        }
      });
    };

    $scope.previousQuestion = function() {
      $scope.step--;
      $scope.provideFocus();
    };

    $scope.nextQuestion = function() {
      if(_.any($scope.currentErrorFields())) {
        $scope.applyErrors();
        $scope.provideFocus();
        $scope.$broadcast('groupFinderShowError');
      } else {
        $scope.$broadcast('groupFinderClearError');
        $scope.go();
      }
    };

    $scope.go = function() {
      Responses.data.completed_flow = true;
      if($scope.mode === 'host' && $scope.isPrivateGroup()) {
        // TODO if private group skip review, save and show confirmation page.
        $state.go('group_finder.' + $scope.mode + '.review');
      } else if($scope.step === $scope.totalQuestions) {
        $state.go('group_finder.' + $scope.mode + '.review');
      } else {
        Responses.data.completed_flow = false;
        $scope.step++;
        $scope.provideFocus();
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

    $scope.requiredFields = function() {
      var visibleFields = $('input:visible, select:visible, textarea:visible');
      return _.map(visibleFields, function(el,i) {
        return $(el).attr('name');
      });
    };

    $scope.currentErrorFields = function() {

      return _.chain($scope.requiredFields())
              .uniq()
              .map(function(name) {

                var pattern = /([^\[\]]*)(\[(.*)\])?/;
                var matches = name.match(pattern);
                var cleanedName = matches[1];
                var controlName = matches[3];

                var el = $('[name="' + name + '"]');
                var response = $scope.responses[cleanedName];

                if(typeof response === 'object') {
                  if(controlName === undefined) {
                    // multi-select value, ie. checkbox
                    response = Object.keys(response);
                  } else {
                    // compound group of fields, ie. address, date/time, etc.
                    response = response[controlName];
                  }
                }

                var hasError = (response === undefined || response === '');

                return hasError ? el : false;
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


    $scope.provideFocus = function() {
      $timeout(function() {
        var el = $('input, select').filter('[name*=' + $scope.currentKey() + ']').first();
            el.focus();
      },100);
    };

    // ----------------------------------- //

    $scope.$watch('step', function(step) {
      $location.hash(step);
      $scope.$broadcast('stepChange', step);
    });

    $window.onhashchange = function(e) {
      var hash = $location.hash();
      if( hash &&
          $.isNumeric(hash) &&
          parseInt(hash) !== $scope.step) {
        $scope.step = parseInt(hash);
      }
    };

    // ----------------------------------- //

    $scope.initialize();
  }

})();
