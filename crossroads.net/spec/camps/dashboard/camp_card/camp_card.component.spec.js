import constants from 'crds-constants';

/* jshint unused: false */
import campsModule from '../../../../app/camps/camps.module';
import campHelpers from '../../campHelpers';

describe('Camp Card Directive', () => {

  let cardComponent;

  const bindings = {
    attendee: 'Matt Silbernagel',
    startDate: '2017-06-20 00:00:00',
    endDate: '2017-06-27 00:00:00',
    campTitle: 'awesome Camp',
    paymentRemaining: 300
  };

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  beforeEach(inject((_$componentController_) => {
    cardComponent = _$componentController_('campCard', null, bindings);
  }));

  it('should bind to bindings', () => {
    expect(cardComponent.attendee).toBe(bindings.attendee);
    expect(cardComponent.startDate).toBe(bindings.startDate);
    expect(cardComponent.endDate).toBe(bindings.endDate);
    expect(cardComponent.paymentRemaining).toBe(bindings.paymentRemaining);
    expect(cardComponent.campTitle).toBe(bindings.campTitle);
  });

  it('should format the dates for display', () => {
    expect(cardComponent.formatDate()).toBe('June 20th - June 27th, 2017');
  });

});
