export default class AnalyticsService {
  /* @ngInject */
  constructor($analytics) {
    // need to get this from constant file based on env var
    this.analytics = $analytics;
  }

  trackForgotPassword() {
    this.analytics.eventTrack('Forgot Password');
  }
}