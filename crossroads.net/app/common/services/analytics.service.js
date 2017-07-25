export default class AnalyticsService {
  /* @ngInject */
  constructor($analytics) {
    this.analytics = $analytics;
  }

  trackForgotPassword() {
    this.analytics.eventTrack('ForgotPassword');
  }
}
