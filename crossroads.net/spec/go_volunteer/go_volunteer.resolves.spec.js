import goVolunteerModule from '../../app/go_volunteer/goVolunteer.module';
import { GetProject } from '../../app/go_volunteer/goVolunteer.resolves';

describe('GO Local Resolves', () => {
  let state;
  let q;
  let GoVolunteerDataService;
  let GoVolunteerService;

  const projectId = 3;
  const contactId = 998;
  const project = {
    projectId,
    contactId
  };


  beforeEach(angular.mock.module(goVolunteerModule));

  beforeEach(inject((_$rootScope_, _$state_, _$q_, _GoVolunteerDataService_, _GoVolunteerService_) => {
    state = _$state_;
    q = _$q_;
    GoVolunteerService = _GoVolunteerService_;
    GoVolunteerDataService = _GoVolunteerDataService_;
    state.toParams = { projectId };
  }));

  it('should resolve the project when navigating to the anywhere page', () => {
    spyOn(GoVolunteerDataService, 'getProject').and.callFake(() => {
      const deferred = q.defer();
      deferred.resolve(project);
      return deferred.promise;
    });

    const result = GetProject(state, GoVolunteerDataService, GoVolunteerService, q);
    expect(GoVolunteerDataService.getProject).toHaveBeenCalledWith(projectId);
    result.then(() => {
      expect(GoVolunteerService.project).toBe(project);
    });
  });
});
