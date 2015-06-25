"use strict";
(function () {

  angular.module("crossroads.core").service("Session",SessionService);

  SessionService.$inject = ['$log','$cookies'];

  function SessionService($log, $cookies) {
    var self = this;
    this.create = function (sessionId, userId, username) {
      console.log("creating cookies!");
      $cookies.put('sessionId', sessionId);
      $cookies.put('userId', userId);
      $cookies.put('username', username);
    };

    /*
     * This formats the family as a comma seperated string before storing in the
     * cookie called 'family'
     *
     * @param family - an array of participant ids
     */
    this.addFamilyMembers = function (family) {
      $log.debug("Adding " + family + " to family cookie");
      $cookies.put('family', family.join(","));
    };

    /*
     * @returns an array of participant ids
     */
    this.getFamilyMembers = function () {
      if(this.exists('family')){
        return _.map($cookies.get('family').split(","), function(strFam){
          return Number(strFam);
        });
      }
      return [];
    };

    this.isActive = function () {
      var ex = this.exists("sessionId");
      if (ex === undefined || ex === null ) {
          return false;
      }
      return true;
    };

    this.exists = function (cookieId) {
      return $cookies.get(cookieId);
    };

    this.clear = function () {
      // TODO Added to debug/research US1403 - should remove after issue is resolved
      console.log("US1403: clearing session in session_service");

      $cookies.remove("sessionId");
      $cookies.remove("userId");
      $cookies.remove("username");
      $cookies.remove('family');
      return true;
    };

    this.getUserRole = function () {
        return "";
    };

    //TODO: Get this working to DRY up login_controller and register_controller
    this.redirectIfNeeded = function($state){
      // TODO Added to debug/research US1403 - should remove after issue is resolved
      console.log("US1403: redirectIfNeeded session_service");

      if (self.hasRedirectionInfo()) {
        // TODO Added to debug/research US1403 - should remove after issue is resolved
        console.log("US1403: redirectIfNeeded session_service - has redirect info");

        var url = self.exists("redirectUrl");
        var link = self.exists("link");
        self.removeRedirectRoute();
        if(link === undefined){
          $state.go(url);
        } else {
          $state.go(url,{link:link});
        }
      }
    };

    this.addRedirectRoute = function(redirectUrl, link) {
        $cookies.put('redirectUrl', redirectUrl);
        $cookies.put('link', link);
    };

    this.removeRedirectRoute = function() {
        $cookies.remove("redirectUrl");
        $cookies.remove("link");
    };

    this.hasRedirectionInfo = function() {
        if (this.exists("redirectUrl") !== undefined) {
            return true;
        }
        return false;
    };

    return this;
  }

})()
