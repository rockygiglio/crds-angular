﻿'use strict';
(function () {
    angular.module('crossroads').controller('RegisterCtrl', ['$scope', '$rootScope', 'AUTH_EVENTS','AuthService', 'MESSAGES', 'User', '$log', RegisterController]);
   
    function RegisterController($scope, $rootScope, AUTH_EVENTS, AuthService, MESSAGES, User, $log) {
        $log.debug("Inside register controller");
        $scope.newuser = User;
        $scope.passwordPrefix = "registration";

        $scope.pwprocess = function(){
            if ($scope.pwprocessing =="SHOW") {
                $scope.pwprocessing = "HIDE";
                $scope.inputType = 'text';
            }
            else {
                $scope.pwprocessing = "SHOW";
                $scope.inputType = 'password';
            }
        }

        var _this = this;
        $scope.register = function () {


            if ($scope.newuser == null || $scope.newuser.email == null || $scope.newuser.password == null || $scope.newuser.email == "" || $scope.newuser.password == "" || $scope.newuser.firstname == null || $scope.newuser.firstname == '' || $scope.newuser.lastname == null || $scope.newuser.lastname =='') {
                $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                return;
            }
                
            $scope.credentials = {};
            $scope.credentials.username = $scope.newuser.email;
            $scope.credentials.password = $scope.newuser.password;

            User.$save().then(function () {
                AuthService.login($scope.credentials).then(function (user) { // TODO Refactor this to a shared location for use here and in login_controller
                    $log.debug("got a 200 from the server ");
                    $log.debug(user);
                    $scope.registerShow = !$scope.registerShow;
                    $rootScope.showLoginButton = false; //TODO use emit or an event here, avoid using rootscope
                    $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
                    $scope.registerForm.$setPristine();
                    $scope.newuser = {};
                }, function () {
                    $log.debug("Bad password");
                    $scope.pending = false;
                    $scope.loginFailed = true;
                }).then(function () {
                    $scope.processing = false;
                })
            });

        }


        $scope.showRegisterButton = true;
        $scope.registerShow = false;

        $scope.toggleDesktopRegister = function () {
            $scope.registerShow = !$scope.registerShow;
            if ($scope.loginShow)
                $scope.loginShow = !$scope.loginShow;
        }

        $scope.openLogin = function (data) {
            $scope.passwordPrefix = "login-dropdown";
            $scope.credentials.username = $scope.newuser.email;
            $scope.credentials.password = $scope.newuser.password;
            $scope.registerShow = !$scope.registerShow;
            if (!$scope.loginShow)
                $scope.loginShow = !$scope.loginShow;
        }
    }
})()