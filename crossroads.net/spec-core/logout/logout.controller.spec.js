describe('Logout Controller', function () {

  var $controller, LogoutController, $rootScope, $state, $log, AuthService, Session;

  beforeEach(function () {
    angular.mock.module('crossroads.core', function ($provide) {

      // mocked service
      $provide
        .value('AuthService', {
          login: function () {
            return {
              then: function (callback) {
                return callback("username");
              }
            };
          },
          logout: function () {
            return {
              then: function (callback) {
                return true;
              }
            }
          }
        });
      return null;
    });
  });

  beforeEach(inject(function (_$controller_, _$rootScope_, _$state_, _$log_, _AuthService_, _Session_) {
    $controller = _$controller_;
    $rootScope = _$rootScope_;
    $state = _$state_;
    $log = _$log_;
    $scope = $rootScope.$new();
    AuthService = _AuthService_;
    Session = _Session_;

    // Session = {
    //   hasRedirectionInfo: function () {
    //     return false;
    //   },
    //   redirectIfNeeded: function($state) {
    //     return true;
    //   }
    // }

  }));

  it('should route to homepage when empty \'processExternalRedirect\'', function () {
    spyOn($state, 'go');
    spyOn(Session, 'redirectIfNeeded');

    LogoutController = $controller('LogoutController', {
      $scope: $scope,
      $rootScope: $rootScope,
      $log: $log,
      AuthService: AuthService,
      $state: $state,
      Session: Session,
      processExternalRedirect: undefined
    });

    expect(Session.redirectIfNeeded).toHaveBeenCalledTimes(1);
  });

  it('should invoke \'processExternalRedirect\' when not empty and a function', function () {
    spyOn($state, 'go');
    spyOn(Session, 'redirectIfNeeded');

    var externalCalled = false;
    processExternalRedirect = function () {
      externalCalled = true;
    }

    LogoutController = $controller('LogoutController', {
      $scope: $scope,
      $rootScope: $rootScope,
      $log: $log,
      AuthService: AuthService,
      $state: $state,
      Session: Session,
      processExternalRedirect: processExternalRedirect
    });

    expect(externalCalled).toBe(true);
    expect($state.go).toHaveBeenCalledTimes(0);
  });

});