import campsModule from '../../../../app/camps/camps.module';
import campHelpers from '../../campHelpers';

describe('Camps Payment Component', () => {
  let fixture;
  let campsService;
  let state;

  const campId = 123;
  const contactId = 456;
  const invoiceId = 476543;

  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_$componentController_, _CampsService_, _$state_) => {
    campsService = _CampsService_;
    state = _$state_;
    campsService.productInfo = campHelpers.productInfo;
    fixture = _$componentController_('campsPayment', null, {});
  }));
});
