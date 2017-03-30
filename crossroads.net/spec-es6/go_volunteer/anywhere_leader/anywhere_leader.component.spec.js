import goVolunteerModule from '../../../app/go_volunteer/goVolunteer.module';
import helpers from '../goVolunteer.helpers';

describe('Go Local Leader Dashboard', () => {
  let GoVolunteerService;
  let componentController;
  let logger;
  let fixture;
  let cookie;

  const bindings = {};
  const validUserId = '2186211';
  const invalidUserId = '123456';

  beforeEach(angular.mock.module(goVolunteerModule));

  beforeEach(inject((_$componentController_, _$log_, _GoVolunteerService_, _$cookies_) => {
    logger = _$log_;
    componentController = _$componentController_;
    GoVolunteerService = _GoVolunteerService_;
    cookie = _$cookies_;

  }));

  fdescribe('User is project leader', () => {
    beforeEach(() => {
      GoVolunteerService.project = helpers.project;
      GoVolunteerService.dashboard = helpers.participants;
      fixture = componentController('anywhereLeader', null, bindings);
      spyOn(cookie, 'get').and.returnValue(validUserId);
    });

    it('should allow user to view page', () => {
      fixture.$onInit();
      expect(cookie.get).toHaveBeenCalledWith('userId');
      expect(fixture.unauthorized).toBe(false);
      expect(fixture.viewReady).toBe(true);
      expect(fixture.showDashboard()).toBe(true);
    });
  });

  fdescribe('User is not project leader', () => {
    beforeEach(() => {
      GoVolunteerService.project = helpers.project;
      GoVolunteerService.dashboard = helpers.participants;
      fixture = componentController('anywhereLeader', null, bindings);
      spyOn(cookie, 'get').and.returnValue(invalidUserId);
    });
    it('should not allow user to view page', () => {
      fixture.$onInit();
      expect(cookie.get).toHaveBeenCalledWith('userId');
      expect(fixture.unauthorized).toBe(true);
      expect(fixture.viewReady).toBe(true);
      expect(fixture.showDashboard()).toBe(false);
    });
  });

  describe('When data is accurate', () => {
    beforeEach(() => {
      GoVolunteerService.project = helpers.project;
      GoVolunteerService.dashboard = helpers.participants;
      fixture = componentController('anywhereLeader', null, bindings);
    });

    it('should create the component', () => {
      expect(fixture.viewReady).toBeFalsy();
    });

    it('should initialize the component', () => {
      fixture.$onInit();
      expect(fixture.viewReady).toBeTruthy();
    });

    it('should get the project from the service', () => {
      expect(fixture.project).toBe(helpers.project);
    });

    it('should get the participants from the service', () => {
      expect(fixture.participants).toBe(helpers.participants);
    });

    it('should calculate total participants for this dashboard', () => {
      expect(fixture.totalParticipants()).toBe(17);
    });
  });

  describe('When data is inaccurate', () => {
    beforeEach(() => {
      GoVolunteerService.project = helpers.project;
      GoVolunteerService.dashboard = undefined;
      fixture = componentController('anywhereLeader', null, bindings);
    });

    it('should handle the dashboard being undefined gracefully', () => {
      expect(fixture.totalParticipants()).toBe(1);
    });
  });
});
