import constants from 'crds-constants';
import ImpersonateController from '../../../core/impersonate/impersonate.controller';

describe('Impersonate Controller', () => {
  let rootScope;
  let http;
  let httpBackend;
  let cookies;
  let fixture;
  let Session;

  const mockUser = {
    userId: 1,
    username: 'Sam',
    userEmail: 'sam@schmoe.com',
    canImpersonate: true
  }

  const mockResponse = {
    userId: 2,
    username: 'Joe',
    userEmail: 'joe@schmoe.com',
    canImpersonate: false
  };

  beforeEach(angular.mock.module('crossroads.core'));
  beforeEach(inject(function ($injector, _$cookies_, _$rootScope_, _Session_) {
    rootScope = _$rootScope_;
    rootScope.canImpersonate = true;
    http = $injector.get('$http');
    httpBackend = $injector.get('$httpBackend');
    cookies = _$cookies_;
    Session = _Session_;

    fixture = new ImpersonateController(rootScope, http, cookies);
    
    cookies.put(constants.COOKIES.SESSION_ID, 'testsession');
    httpBackend.expectGET(`${window.__env__['CRDS_API_ENDPOINT']}api/authenticated`).respond(200, mockUser);
    Session.verifyAuthentication(undefined, undefined, undefined, undefined);
    httpBackend.flush();
  }));

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });

  it('should send a request to the MP API', () => {
    spyOn(fixture, 'storeCurrentUser');
    spyOn(fixture, 'storeImpersonateDetails');
    spyOn(fixture, 'setCurrentUser');

    httpBackend.expectGET(`${window.__env__['CRDS_API_ENDPOINT']}api/user?username=${fixture.username}`).respond(200, mockResponse);
    fixture.startImpersonating();
    httpBackend.flush();

    expect(fixture.storeCurrentUser).toHaveBeenCalled();
    expect(fixture.storeImpersonateDetails).toHaveBeenCalled();
    expect(fixture.setCurrentUser).toHaveBeenCalled();
  });

  it('should set user details on successful API call', () => {
    httpBackend.expectGET(`${window.__env__['CRDS_API_ENDPOINT']}api/user?username=${fixture.username}`).respond(200, mockResponse);
    fixture.startImpersonating();
    httpBackend.flush();

    expect(fixture.error).toBe(false);
    expect(rootScope.userid).toBe(mockResponse.userId);
    expect(rootScope.username).toBe(mockResponse.username);
    expect(rootScope.email).toBe(mockResponse.userEmail);

  });

  it('should set fail and keep user credentials on unsuccessful API call', () => {
    httpBackend.expectGET(`${window.__env__['CRDS_API_ENDPOINT']}api/user?username=${fixture.username}`).respond(404);
    fixture.startImpersonating();
    httpBackend.flush();

    expect(fixture.error).toBe(true);
    expect(rootScope.userid).toBe(mockUser.userId);
    expect(rootScope.username).toBe(mockUser.username);
    expect(rootScope.email).toBe(mockUser.userEmail);
  });

  it('should NOT overwrite canImpersonate flag when impersonating', () => {
    httpBackend.expectGET(`${window.__env__['CRDS_API_ENDPOINT']}api/user?username=${fixture.username}`).respond(200, mockResponse);
    fixture.startImpersonating();
    httpBackend.flush();

    httpBackend.expectGET(`${window.__env__['CRDS_API_ENDPOINT']}api/authenticated`).respond(200, mockResponse);
    Session.verifyAuthentication(undefined, undefined, undefined, undefined);
    httpBackend.flush();

    expect(mockResponse.canImpersonate).toBe(false);
    expect(rootScope.canImpersonate).toBe(true);
  });

  it('should not send impersonate headers after stopping impersonate', () => {
    httpBackend.expectGET(`${window.__env__['CRDS_API_ENDPOINT']}api/user?username=${fixture.username}`).respond(200, mockResponse);
    fixture.startImpersonating();
    httpBackend.flush();

    fixture.stopImpersonating();
    expect(http.defaults.headers.common.ImpersonateUserId).toBeUndefined();
  });
});
