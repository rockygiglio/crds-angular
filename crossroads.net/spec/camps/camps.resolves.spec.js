import campsModule from '../../app/camps/camps.module';
import { getCampProductInfo } from '../../app/camps/camps.resolves';

describe('Camp Resolvers', () => {
  let campsService;
  let rootScope;
  let state;
  let q;

  const campId = 3344;
  const camperId = 9876;
  const invoiceId = 981274;

  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_$rootScope_, _CampsService_, _$state_, _$q_) => {
    rootScope = _$rootScope_;
    campsService = _CampsService_;
    state = _$state_;
    q = _$q_;

    spyOn(state, 'go').and.returnValue(true);
  }));

  describe('already made payment', () => {
    beforeEach(() => {
      state.toParams = {
        campId,
        contactId: camperId,
        page: 'camps-payment'
      };
      spyOn(campsService, 'getCampProductInfo').and.callFake(() => {
        const deferred = q.defer();
        deferred.reject({ status: 302 });
        return deferred.promise;
      });
    });

    it('Should redirect to the camps payment page if I already paid', () => {
      getCampProductInfo(campsService, state, q);
      rootScope.$apply();
      expect(state.go).toHaveBeenCalledWith('campsignup.application', { page: 'camps-payment', contactId: camperId, campId, update: true, redirectTo: 'payment-confirmation' });
    });
  });

  describe('have not made deposit yet', () => {
    beforeEach(() => {
      state.toParams = {
        campId,
        contactId: camperId,
        page: 'camps-payment'
      };
      spyOn(campsService, 'getCampProductInfo').and.callFake(() => {
        const deferred = q.defer();
        deferred.resolve({ invoiceId });
        return deferred.promise;
      });
    });

    it('should not call state.go', () => {
      const result = getCampProductInfo(campsService, state, q);
      expect(campsService.getCampProductInfo).toHaveBeenCalledWith(campId, camperId, true);
      expect(state.go).not.toHaveBeenCalled();
      result.then((data) => {
        expect(data.invoiceId).toBe(invoiceId);
      });
    });
  });

  describe('Naviate to product summary', () => {
    beforeEach(() => {
      state.toParams = {
        campId,
        contactId: camperId,
        page: 'product-summary'
      };
      spyOn(campsService, 'getCampProductInfo').and.callFake(() => {
        const deferred = q.defer();
        deferred.resolve({ invoiceId });
        return deferred.promise;
      });
    });

    it('should not get payment status if not going to camp-payments', () => {
      const result = getCampProductInfo(campsService, state, q);
      expect(state.go).not.toHaveBeenCalled();
      expect(campsService.getCampProductInfo).toHaveBeenCalledWith(campId, camperId);
      result.then((data) => {
        expect(data.invoiceId).toBe(invoiceId);
      });
    });
  });
});
