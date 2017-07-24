require('../../../app/common/common.module');
require('../../../app/app');

describe('Common Giving Donation Service', () => {
  let fixture;

  let analytics;

  beforeEach(() => {
    angular.mock.module('crossroads.common', ($provide) => {
      analytics = jasmine.createSpyObj('$analytics',
        ['eventTrack']);
      $provide.value('$analytics', analytics);
    });
  });

  beforeEach(
      inject((AnalyticsService) => {
        fixture = AnalyticsService;
      })
  );

  it('should call eventTrack with "Forgot Password"', () => {
    fixture.trackForgotPassword();
    expect(analytics.eventTrack).toHaveBeenCalledWith('Forgot Password');
  });
});
