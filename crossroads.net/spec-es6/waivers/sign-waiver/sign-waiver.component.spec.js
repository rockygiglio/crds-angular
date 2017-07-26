import WaiversModule from '../../../app/waivers/waivers.module';

fdescribe('Waivers', () => {
  let httpBackend;
  let log;
  let state;
  let rootScope;
  let ctrl;
  let waiversService;

  beforeEach(() => {
    angular.mock.module(WaiversModule);
  });

  beforeEach(inject((_$httpBackend_, _$log_, _$state_, _$rootScope_, _WaiversService_) => {
    httpBackend = _$httpBackend_;
    log = _$log_;
    state = _$state_;
    rootScope = _$rootScope_;
    waiversService = _WaiversService_;
  }));

  describe('Sign Waiver Component', () => {
    let waiversServiceSpy;

    beforeEach(() => {
      spyOn(log, 'error').and.callThrough();
      spyOn(rootScope, '$emit').and.callThrough();
      spyOn(waiversService, 'getWaiver');
    });

    beforeEach(inject((_$componentController_) => {
      ctrl = _$componentController_('signWaiver', null, {});
    }));

    it('Should get the waiver to be signed', () => {

    });

    it('Should display an error on failure to get waiver', () => {

    });

    it('Should log an error on failure to get waiver', () => {

    });

    it('Should navigate back on Cancel', () => {

    });

    it('Should send a confirmation email on Send', () => {

    });

    it('Should display a modal on successful Send', () => {

    });

    it('Should display an error on failed Send', () => {

    });

    it('Should log an error on failed Send', () => {

    });
  });
});
