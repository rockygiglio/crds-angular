import WaiversModule from '../../../app/waivers/waivers.module';

fdescribe('Waivers', () => {
  let httpBackend;
  let rootScope;
  let state;
  let ctrl;
  let waiversService;

  beforeEach(() => {
    angular.mock.module(WaiversModule);
  });

  beforeEach(inject((_$httpBackend_, _$rootScope_, _$state_, _WaiversService_) => {
    httpBackend = _$httpBackend_;
    rootScope = _$rootScope_;
    state = _$state_;
    waiversService = _WaiversService_;
  }));

  describe('Sign Waiver Component', () => {
    beforeEach(inject((_$componentController_) => {
      ctrl = _$componentController_('signWaiver', null, {});
    }));

    it('Should get the waiver to be signed', () => {

    });

    it('Should navigate back on Cancel', () => {

    });

    it('Should send a confirmation email on Send', () => {

    });

    it('Should display a modal on successful Send', () => {

    });

    it('Should display an error on failed Send', () => {

    });
  });
});
