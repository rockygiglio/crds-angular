﻿(function(){
    angular.module("crdsServices", ['ngResource']);
    angular.module('crdsServices').factory('Profile', ['$resource', ProfileService]);

    function ProfileService($resource) {
        //TODO This file seems like a near duplicate, should this be removed?
        return {
            Personal: $resource( __GATEWAY_CLIENT_ENDPOINT__ +  'api/profile'),
            Person: $resource( __GATEWAY_CLIENT_ENDPOINT__ +  'api/profile/:contactId'),
            Account: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/account'),
            Password: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/account/password')
        }
    }


})()
