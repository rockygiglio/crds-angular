import constants from 'crds-constants';
import ChildcareDashboardService from '../../app/childcare_dashboard/childcareDashboard.service';

/* jshint unused: false */
import childcareModule from '../../app/childcare_dashboard/childcareDashboard.module';

describe('Childcare Dashboard Service', () => {
  
  const endpoint = window.__env__['CRDS_API_ENDPOINT'] + 'api';
  const uid = 1234567890;

  let childcareService,
      resource,
      httpBackend,
      session;

  beforeEach(angular.mock.module(constants.MODULES.CHILDCARE_DASHBOARD));

  beforeEach(inject(function($injector) {
    resource = $injector.get('$resource');
    session = $injector.get('Session');
    httpBackend = $injector.get('$httpBackend');
    console.log(ChildcareDashboardService);
    childcareService = new ChildcareDashboardService(resource, session);

    spyOn(session, 'exists').and.returnValue(uid);
  }));

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });

  it('should get and set a list of childcare dates', () => {
    expect(childcareService.childcareDates).toBeUndefined();
    httpBackend.expectGET(`${endpoint}/childcare/dashboard/${uid}`)
      .respond(200, {'childcareDates': []});

     childcareService.fetchChildcareDates();
     httpBackend.flush();
  });

});
