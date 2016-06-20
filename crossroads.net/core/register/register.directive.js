(function(){
  'use strict';
  
  module.exports = RegisterForm;

  RegisterForm.$inject = ['$log', 'AUTH_EVENTS'];

  function RegisterForm($log, AUTH_EVENTS){
    return {
      restrict: 'EA',
      templateUrl: 'register/register_form.html',
      controller: 'RegisterController as register',
      bindToController: true,
    };
  }
})();
