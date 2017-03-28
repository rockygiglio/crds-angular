import goVolunteerModule from '../../../app/go_volunteer/goVolunteer.module';
import helpers from '../goVolunteer.helpers';

describe('Go Local Leader Dashboard', () => {
  let GoVolunteerService;
  let componentController;
  let logger;
  let fixture;

  const bindings = {};

  beforeEach(angular.mock.module(goVolunteerModule));

  beforeEach(inject((_$componentController_, _$log_, _GoVolunteerService_) => {
    logger = _$log_;
    componentController = _$componentController_;
    GoVolunteerService = _GoVolunteerService_;
  }));

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
