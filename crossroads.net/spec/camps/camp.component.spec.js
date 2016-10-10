import constants from 'crds-constants';

/* jshint unused: false */
import campsModule from '../../app/camps/camps.module';

describe('Camp Component', () => {

  let campService,
      $componentController,
      campController,
      httpBackend;

  const endpoint = window.__env__['CRDS_API_ENDPOINT'] + 'api';

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  beforeEach(inject((_$componentController_, _$httpBackend_) => {
    $componentController = _$componentController_;
    httpBackend = _$httpBackend_;
    httpBackend.whenGET(`${endpoint}/lookup/sites`).respond(200, {});
    var bindings = {};
    campController = $componentController('crossroadsCamp', null, bindings);
    campController.$onInit();
  }));

  it('should set the view as ready', () => {
    expect(campController.viewReady).toBe(true);
  });
});
