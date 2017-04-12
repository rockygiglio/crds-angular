
describe('Session Service', function() {

  var $cookies;
  var Backend;
  var Session;
  var Injector;
  var rootScope;
  var modal;
  var state;
  var cookieNames = require('crds-constants').COOKIES;
  var family = [0, 1, 2, 3, 4];
  var timeout;

  beforeEach(angular.mock.module('crossroads.core'));

  beforeEach(inject(function($injector, _$cookies_, _$rootScope_, _Session_, _$modal_, _$timeout_) {
    $cookies = _$cookies_;
    state = $injector.get('$state');
    spyOn(state, 'go');
    Backend = $injector.get('$httpBackend');
    Session = _Session_;
    Injector = $injector;
    rootScope = _$rootScope_;
    modal = _$modal_;
    timeout = _$timeout_;
  }));

  afterEach(function() {
    Backend.verifyNoOutstandingExpectation();
    Backend.verifyNoOutstandingRequest();
  });

  it('should save an array of family members', function(){
    Session.addFamilyMembers(family);
    expect($cookies.get('family')).toBe(family.join(','));
  });

  it('should return an array of family members', function(){
    Session.addFamilyMembers(family);
    expect(Session.getFamilyMembers()[0]).toBe(family[0]);
    expect(Session.getFamilyMembers()[1]).toBe(family[1]);
    expect(Session.getFamilyMembers()[4]).toBe(family[4]);
  });

  it('should add a family member to an existing family', function(){
    Session.addFamilyMembers(family);
    expect(Session.getFamilyMembers()[0]).toBe(family[0]);
    expect(Session.getFamilyMembers()[1]).toBe(family[1]);
    expect(Session.getFamilyMembers()[4]).toBe(family[4]);
    family.push(5);
    Session.addFamilyMembers(family);
    expect(Session.getFamilyMembers()[5]).toBe(family[5]);
  });

  describe('function enableReactiveSso', function() {

    var mockResponse = {
      age: 0,
      canImpersonate: true,
      refreshToken: null,
      roles: [],
      userEmail: 'test@tester.com',
      userId: 1234567,
      userPhone: null,
      userToken: 'Aadfwedrwererre',
      userTokenExp: null,
      username: 'Test'
    };

    it('should create a setInterval to monitor cookie changes', function() {
      Session.enableReactiveSso();
      expect(Session.reactiveSsoInterval).toBeDefined();
    });

    it('should set credentials when login is detected', function() {
      $cookies.put(cookieNames.SESSION_ID, mockResponse.userToken);
      Backend.expectGET(window.__env__['CRDS_GATEWAY_CLIENT_ENDPOINT'] + 'api/authenticated').respond(200, mockResponse);
      Session.performReactiveSso();
      Backend.flush();
      expect(rootScope.userid).toBe(mockResponse.userId);
      expect(rootScope.username).toBe(mockResponse.username);
      expect(rootScope.email).toBe(mockResponse.userEmail);
      expect(rootScope.phone).toBe(mockResponse.userPhone);
    });

    it('should clear credentials when logout is detected', function() {
      Session.wasLoggedIn = true;
      $cookies.remove(cookieNames.SESSION_ID);
      Session.performReactiveSso();
      expect(rootScope.userid).toBe(null);
      expect(rootScope.username).toBe(null);
      expect(rootScope.email).toBe(null);
      expect(rootScope.phone).toBe(null);
    });

    it('should open login model when logout is detected on protected route', function() {
      Session.wasLoggedIn = true;
      spyOn(modal, 'open').and.callThrough();
      state.current.data = {
        isProtected: true
      };
      $cookies.remove(cookieNames.SESSION_ID);
      Session.performReactiveSso(undefined, state.current.name, state.current.data, state.current.params);
      expect(modal.open).toHaveBeenCalledWith({
        templateUrl: 'stayLoggedInModal/stayLoggedInModal.html',
        controller: 'StayLoggedInController as StayLoggedIn',
        backdrop: 'static',
        keyboard: false,
        show: false,
      });
    });

  });

  describe('function addRedirectRoute', function() {
    it('should add redirectUrl and params cookies', function() {
      var params = {
        p1: 2,
        p3: '4'
      };
      Session.addRedirectRoute('new.state', params);
      expect($cookies.get('redirectUrl')).toBe('new.state');
      expect($cookies.get('params')).toBe(JSON.stringify(params));
    });
  });

  describe('function verifyAuthentication', function() {

    describe('if user session exists', function() {

      var mockResponse = {
        age: 0,
        canImpersonate: true,
        refreshToken: null,
        roles: [],
        userEmail: 'test@tester.com',
        userId: 1234567,
        userPhone: null,
        userToken: 'Aadfwedrwererre',
        userTokenExp: null,
        username: 'Test'
      };

      beforeEach(function() {
        $cookies.put(cookieNames.SESSION_ID, mockResponse.userToken);
      });

      it('should handle successful login', function() {
        Backend.expectGET(window.__env__['CRDS_GATEWAY_CLIENT_ENDPOINT'] + 'api/authenticated').respond(200, mockResponse);
        Session.verifyAuthentication(undefined, state.current.name, state.current.data, state.current.params);
        Backend.flush();
        expect(rootScope.userid).toBe(mockResponse.userId);
        expect(rootScope.username).toBe(mockResponse.username);
        expect(rootScope.email).toBe(mockResponse.userEmail);
        expect(rootScope.phone).toBe(mockResponse.userPhone);
      });

    });

    describe('if user session does not exist', function() {

      var mockResponse = {};

      beforeEach(function() {
        $cookies.remove(cookieNames.SESSION_ID);
      });

      it('should make you login when visiting protected pages', function() {
        state.current.data = {
          isProtected: true
        };
        Session.verifyAuthentication(undefined, state.current.name, state.current.data, state.current.params);
        expect(state.go).toHaveBeenCalledWith('login');
      });

      it('should stay on non protected pages when not logged in', function() {
        state.current.data = {
          isProtected: false
        };
        Session.verifyAuthentication(undefined, state.current.name, state.current.data, state.current.params);
        expect(state.go).not.toHaveBeenCalled();
      });
    });
  });

  describe('function redirectIfNeeded', function() {

    it('should redirect to url', function() {
        var existsMock = spyOn(Session,'exists');
        existsMock.and.callFake(function(args){
          switch(args)
          {
            case 'redirectUrl':
              return 'content';
            case 'params':
              return null;
            default:
              return null;
          }
        });
        Session.redirectIfNeeded(state);
        timeout.flush();
        expect(state.go).toHaveBeenCalledWith('content');
    });

    it('should redirect to url with params', function() {
        var existsMock = spyOn(Session,'exists');
        existsMock.and.callFake(function(args){
          switch(args)
          {
            case 'redirectUrl':
              return 'content';
            case 'params':
              return JSON.stringify({'link':'/'});
            default:
              return null;
          }
        });
        Session.redirectIfNeeded(state);
        timeout.flush();
        expect(state.go).toHaveBeenCalledWith('content', { link: '/' });    
    });

    it('should not redirect', function() {
        Session.redirectIfNeeded(state);
        expect(state.go).toHaveBeenCalledTimes(0);
    });

  })

});
