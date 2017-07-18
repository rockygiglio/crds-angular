import Analytics from 'astronomer';
import CONSTANTS from '../../constants';

export default class AnalyticsService {
  /* @ngInject */
  constructor() {
    this.analytics = new Analytics('cJvH9S7atrCgZM74m');
  }

  trackForgotPassword() {
    debugger;
    const analyticsPayload = {
      anonymousId: '123',
      event: 'ForgotPassword'
    };
    this.analytics.track(analyticsPayload);
  }
}