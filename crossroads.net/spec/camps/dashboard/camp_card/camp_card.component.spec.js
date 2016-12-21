/* eslint-disable import/no-extraneous-dependencies, import/no-unresolved, import/extensions */
import constants from 'crds-constants';
/* eslint-enable */

describe('Camp Card Directive', () => {
  let cardComponent;
  let campsService;
  let state;

  const bindings = {
    attendee: 'Matt Silbernagel',
    startDate: '2017-06-20 00:00:00',
    endDate: '2017-06-27 00:00:00',
    campTitle: 'awesome Camp',
    paymentRemaining: 300,
    campPrimaryContact: 'studentministry@gmail.com',
    campId: 3452,
    camperId: 1233
  };

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  beforeEach(inject((_$componentController_, _CampsService_, _$state_) => {
    campsService = _CampsService_;
    state = _$state_;
    spyOn(campsService, 'initializeCamperData').and.callThrough();
    spyOn(state, 'go').and.returnValue(true);
    cardComponent = _$componentController_('campCard', null, bindings);
  }));

  it('should bind to bindings', () => {
    expect(cardComponent.attendee).toBe(bindings.attendee);
    expect(cardComponent.startDate).toBe(bindings.startDate);
    expect(cardComponent.endDate).toBe(bindings.endDate);
    expect(cardComponent.paymentRemaining).toBe(bindings.paymentRemaining);
    expect(cardComponent.campTitle).toBe(bindings.campTitle);
    expect(cardComponent.campPrimaryContact).toBe(bindings.campPrimaryContact);
  });

  it('should format the dates for display', () => {
    expect(cardComponent.formatDate()).toBe('June 20th - June 27th, 2017');
  });

  it('should direct to the payment page with the update and redirect flags', () => {
    cardComponent.makePayment();
    expect(campsService.initializeCamperData).toHaveBeenCalled();
    expect(state.go).toHaveBeenCalledWith('campsignup.application', {
      page: 'camps-payment',
      contactId: bindings.camperId,
      campId: bindings.campId,
      update: true,
      redirectTo: 'payment-confirmation'
    });
  });

  it('should format remaining amount for display', () => {
    expect(cardComponent.formatAmountDue()).toBe('$300.00');
  });

  it('should format remaining amount when zero', () => {
    cardComponent.paymentRemaining = 0;
    expect(cardComponent.formatAmountDue()).toBe('$0.00');
  });

  it('should format remaining amount when undefined', () => {
    cardComponent.paymentRemaining = undefined;
    expect(cardComponent.formatAmountDue()).toBe('Error getting payments. Please contact studentministry@gmail.com');
  });

  it('should format remaining amount when undefined', () => {
    cardComponent.paymentRemaining = null;
    expect(cardComponent.formatAmountDue()).toBe('Error getting payments. Please contact studentministry@gmail.com');
  });

  it('should format remaining amount when negative', () => {
    cardComponent.paymentRemaining = -10;
    expect(cardComponent.formatAmountDue()).toBe('Error getting payments. Please contact studentministry@gmail.com');
  });
});
