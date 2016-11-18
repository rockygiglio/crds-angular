import campsModule from '../../../app/camps/camps.module';
import campHelpers from '../campHelpers';

describe('Camps Thank You Component', () => {
  let $componentController;
  let campThankYouController;
  let campsService;
  // let log;
  let rootScope;
  let stateParams;
  let q;

  const campId = 123;
  const contactId = 456;

  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_$componentController_, _CampsService_, _$log_, _$rootScope_, _$stateParams_, _$q_) => {
    $componentController = _$componentController_;
    // log = _$log_;
    stateParams = _$stateParams_;
    campsService = _CampsService_;
    rootScope = _$rootScope_;
    q = _$q_;

    stateParams.campId = campId;
    stateParams.contactId = contactId;

    campsService.payment = new campHelpers().payment;

    const bindings = {};
    campThankYouController = $componentController('campThankYou', null, bindings);
    campThankYouController.$onInit();
  }));

  it('should set the view as ready', () => {
    expect(campThankYouController.viewReady).toBeTruthy();
  });

  it('should get the model', () => {
    expect(campThankYouController.payment).toEqual(campsService.payment);
  });
});
