import campsModule from '../../../../app/camps/application_page/application_page.module';
import campHelpers from '../../campHelpers';

describe('Camps Emergency Contact Component', () => {
  let $componentController;
  let emergencyContactController;
//  let log;
  let rootScope;

  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_$componentController_, _$log_, _$rootScope_, _EmergencyContactForm_) => {
    $componentController = _$componentController_;
//    log = _$log_;
    rootScope = _$rootScope_;

    rootScope.MESSAGES = campHelpers.messages;
    const bindings = {};
    emergencyContactController = $componentController('emergencyContact', null, bindings);
    emergencyContactController.$onInit();
  }));

  it('should set the view as ready', () => {
    expect(emergencyContactController.viewReady).toBeTruthy();
  });
});
