import applicationModule from '../../../../app/camps/application_page/application_page.module';
import campsModule from '../../../../app/camps/camps.module';
import campHelpers from '../../campHelpers';

import moment from 'moment';

describe('Camp Product Summary Component', () => {
  let $componentController;
  let productSummaryController;
  let productSummaryForm;
  let campsService;
  let rootScope;
  let stateParams;
  let q;

  const campId = 123;
  const contactId = 456;

  beforeEach(angular.mock.module(campsModule));
  beforeEach(angular.mock.module(applicationModule));

  beforeEach(inject((_$componentController_, _ProductSummaryForm_, _CampsService_, _$rootScope_, _$stateParams_, _$q_) => {
    $componentController = _$componentController_;
    campsService = _CampsService_;
    stateParams = _$stateParams_;
    rootScope = _$rootScope_;
    q = _$q_;

    campsService.productInfo = campHelpers().productInfo;

    stateParams.campId = campId;
    stateParams.contactId = contactId;

    // Set up the form instance
    productSummaryForm = _ProductSummaryForm_.createForm();
    spyOn(_ProductSummaryForm_, 'createForm').and.callFake(() => productSummaryForm);
    spyOn(productSummaryForm, 'getModel').and.callThrough();

    // Create the component to test
    const bindings = {};
    productSummaryController = $componentController('productSummary', null, bindings);
    productSummaryController.$onInit();
  }));

  it('should set the view as ready', () => {
    expect(productSummaryController.viewReady).toBeTruthy();
  });

  it('should get the model', () => {
    expect(productSummaryForm.getModel).toHaveBeenCalled();
    expect(productSummaryController.model).toBeDefined();
  });

  it('should successfully save the form', () => {
    productSummaryController.productSummary = { $valid: true };

    spyOn(productSummaryForm, 'save').and.callFake(() => {
      const deferred = q.defer();
      deferred.resolve('success!');
      return deferred.promise;
    });

    productSummaryController.submit();
    rootScope.$apply();
    expect(productSummaryForm.save).toHaveBeenCalledWith(campId, contactId);
  });

  it('should reject saving the form', () => {
    productSummaryController.productSummary = { $valid: true };

    spyOn(productSummaryForm, 'save').and.callFake(() => {
      const deferred = q.defer();
      deferred.reject('error!');
      return deferred.promise;
    });

    productSummaryController.submit();
    rootScope.$apply();
    expect(productSummaryForm.save).toHaveBeenCalledWith(campId, contactId);
  });

  it('should disable the submit button while saving', () => {
    expect(productSummaryController.submitting).toBe(false);

    productSummaryController.productSummary = { $valid: true };

    spyOn(productSummaryForm, 'save').and.callFake(() => {
      const deferred = q.defer();
      deferred.resolve('success!');
      return deferred.promise;
    });

    productSummaryController.submit();
    expect(productSummaryController.submitting).toBe(true);

    rootScope.$apply();
    expect(productSummaryController.submitting).toBe(false);
  });

  describe('Minimum deposit', () => {
    it('should be standard amount', () => {
      expect(productSummaryController.model.financialAssistance).toBeFalsy();
      expect(productSummaryController.getMinimumDeposit()).toBe(200);
    });

    it('should be reduced for financial assistance', () => {
      productSummaryController.model.financialAssistance = true;

      expect(productSummaryController.model.financialAssistance).toBeTruthy();
      expect(productSummaryController.getMinimumDeposit()).toBe(50);
    });
  });

  describe('Registration Fee', () => {
    it('should have first discount highlighted', () => {
      const today = moment('2016-11-01').toDate();
      jasmine.clock().mockDate(today);

      productSummaryController.$onInit();
      expect(productSummaryController.productInfo.options[0].isCurrent).toBeTruthy();
    });

    it('should have second discount highlighted', () => {
      const today = moment('2016-11-30').toDate();
      jasmine.clock().mockDate(today);

      productSummaryController.$onInit();
      expect(productSummaryController.productInfo.options[1].isCurrent).toBeTruthy();
    });

    it('should have base price highlighted', () => {
      const today = moment('2016-12-02').toDate();
      jasmine.clock().mockDate(today);

      productSummaryController.$onInit();
      expect(productSummaryController.isBasePriceCurrent).toBeTruthy();
    });
  });

  it('Shows error message when data not set up', () => {
    productSummaryController.productInfo = {};

    expect(productSummaryController.hasProductInfo()).toBeFalsy();
  });
});
