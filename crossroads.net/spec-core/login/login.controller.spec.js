describe('Login Controller', function () {

  var $controller, $q, LoginController, $rootScope, MESSAGES, $state, $log, AuthService, Session, $timeout, MockSession, $location, redirectUrl, params;

  beforeEach(function () {
    angular.mock.module('crossroads.core', function ($provide) {

      // mocked service
      $provide.value('AuthService', {
        login: function () {
          return {
            then: function (callback) {
              return callback('username');
            }
          };
        },
      });

      return null;
    });
  });

  beforeEach(inject(function (_$controller_, _$rootScope_, _MESSAGES_, _$state_, _$log_, _AuthService_, _$timeout_, _Session_, _$location_) {
    $controller = _$controller_;
    $rootScope = _$rootScope_;
    MESSAGES = _MESSAGES_;
    $state = _$state_;
    $log = _$log_;
    $scope = $rootScope.$new();
    AuthService = _AuthService_;
    $timeout = _$timeout_;
    Session = _Session_;
    $location = _$location_;

    MockSession = {
      exists: function (value) {
        if (value == 'redirectUrl') {
          return redirectUrl;
        } else if (value == 'params') {
          return params;
        }
      }
    }

    LoginController = $controller('LoginController', {
      $scope: $scope,
      $rootScope: $rootScope,
      MESSAGES: MESSAGES,
      AuthService: AuthService,
      Session: Session
    });

  }));

  it('should route to homepage when navigateToHome is called', function() {
    spyOn($state, 'go');
    LoginController.navigateToHome();
    expect($state.go).toHaveBeenCalledWith('content', { link: '/' });
  });

  it('should route to homepage after signing in and has no redirect parameters', function() {

    $scope.navlogin = {
      $setPristine: function () {
        return null;
      }
    };

    $scope.credentials = {
      username: 'username',
      password: 'password'
    };

    $state.current.name = 'login';

    spyOn($state, 'go');
    spyOn($rootScope, '$emit');

    spyOn(Session, 'hasRedirectionInfo').and.returnValue(false);

    $scope.login();
    $timeout.flush();

    expect($rootScope.$emit).toHaveBeenCalledTimes(0);
    expect($state.go).toHaveBeenCalledWith('content', { link: '/' });
  });

  it('should route to known state after signing in and has redirectUrl', function() {

    $scope.navlogin = {
      $setPristine: function () {
        return null;
      }
    };

    $scope.credentials = {
      username: 'username',
      password: 'password'
    };

    $state.current.name = 'login';
    redirectUrl = 'logout';

    spyOn($state, 'go');
    spyOn($state, 'get').and.returnValue([{ url: '/signout', controller: 'LogoutController', data: { isProtected: false, meta: { title: 'Sign out', description: '' } }, name: 'logout' }]);
    spyOn($rootScope, '$emit');

    spyOn(Session, 'hasRedirectionInfo').and.returnValue(true);
    spyOn(Session, 'exists').and.callFake(MockSession.exists);

    $scope.login();
    $timeout.flush();

    expect($rootScope.$emit).toHaveBeenCalledTimes(0);
    expect($state.go).toHaveBeenCalledWith(redirectUrl);
  });

  it('should route to known state after signing in and has redirectUrl and params', function() {

    $scope.navlogin = {
      $setPristine: function () {
        return null;
      }
    };

    $scope.credentials = {
      username: 'username',
      password: 'password'
    };

    $state.current.name = 'login';
    redirectUrl = 'logout';
    const paramsObj = { bye: 'Felicia' };
    params = JSON.stringify(paramsObj);

    spyOn($state, 'go');
    spyOn($state, 'get').and.returnValue([{ url: '/signout', controller: 'LogoutController', data: { isProtected: false, meta: { title: 'Sign out', description: '' } }, name: 'logout' }]);
    spyOn($rootScope, '$emit');

    spyOn(Session, 'hasRedirectionInfo').and.returnValue(true);
    spyOn(Session, 'exists').and.callFake(MockSession.exists);

    $scope.login();
    $timeout.flush();

    expect($rootScope.$emit).toHaveBeenCalledTimes(0);
    expect($state.go).toHaveBeenCalledWith(redirectUrl, paramsObj);
  });

  it('should route to external url after signing in and has redirectUrl', function() {

    $scope.navlogin = {
      $setPristine: function () {
        return null;
      }
    };

    $scope.credentials = {
      username: 'username',
      password: 'password'
    };

    $state.current.name = 'login';
    redirectUrl = '/connect';

    spyOn($location, 'url');
    spyOn($state, 'get').and.returnValue([{ url: '/signout', controller: 'LogoutController', data: { isProtected: false, meta: { title: 'Sign out', description: '' } }, name: 'logout' }]);
    spyOn($rootScope, '$emit');

    spyOn(Session, 'hasRedirectionInfo').and.returnValue(true);
    spyOn(Session, 'exists').and.callFake(MockSession.exists);

    $scope.login();
    $timeout.flush();

    expect($rootScope.$emit).toHaveBeenCalledTimes(0);
    expect($location.url).toHaveBeenCalledWith(redirectUrl);
  });

  it('should route to external url after signing in and has redirectUrl and params', function() {

    $scope.navlogin = {
      $setPristine: function () {
        return null;
      }
    };

    $scope.credentials = {
      username: 'username',
      password: 'password'
    };

    $state.current.name = 'login';
    redirectUrl = '/connect';
    const paramsObj = { resolve: true };
    params = JSON.stringify(paramsObj);

    spyOn($location, 'url').and.returnValue($location);
    spyOn($location, 'search');
    spyOn($state, 'get').and.returnValue([{ url: '/signout', controller: 'LogoutController', data: { isProtected: false, meta: { title: 'Sign out', description: '' } }, name: 'logout' }]);
    spyOn($rootScope, '$emit');

    spyOn(Session, 'hasRedirectionInfo').and.returnValue(true);
    spyOn(Session, 'exists').and.callFake(MockSession.exists);

    $scope.login();
    $timeout.flush();

    expect($rootScope.$emit).toHaveBeenCalledTimes(0);
    expect($location.url).toHaveBeenCalledWith(redirectUrl);
    expect($location.search).toHaveBeenCalledWith(paramsObj);

  });

});