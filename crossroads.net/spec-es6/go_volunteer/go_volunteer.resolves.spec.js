import goVolunteerModule from '../../app/go_volunteer/goVolunteer.module';
import { GetProject, GetCities, GetOrganizations } from '../../app/go_volunteer/goVolunteer.resolves';

describe('GO Local Resolves', () => {
  let state;
  let q;
  let logger;
  let GoVolunteerDataService;
  let GoVolunteerService;
  let Organizations;

  const projectId = 3;
  const initiativeId = 90;
  const contactId = 998;
  const project = {
    projectId,
    contactId
  };

  const cities = [
    'Chicago, IL',
    'Cleveland, OH',
    'Phoenix, AZ'
  ];


  beforeEach(angular.mock.module(goVolunteerModule));

  beforeEach(inject((_$log_, _$state_, _$q_, _GoVolunteerDataService_, _GoVolunteerService_, _Organizations_) => {
    state = _$state_;
    q = _$q_;
    logger = _$log_;
    GoVolunteerService = _GoVolunteerService_;
    GoVolunteerDataService = _GoVolunteerDataService_;
    Organizations = _Organizations_;
    state.toParams = { projectId, initiativeId };
  }));

  it('should resolve the project', () => {
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

  it('should resolve the initiative cities', () => {
    spyOn(GoVolunteerDataService, 'getInitiativeCities').and.callFake(() => {
      const deferred = q.defer();
      deferred.resolve(cities);
      return deferred.promise;
    });

    const result = GetCities(logger, state, GoVolunteerDataService, GoVolunteerService, q);
    expect(GoVolunteerDataService.getInitiativeCities).toHaveBeenCalledWith(initiativeId);
    result.then(() => {
      expect(GoVolunteerService.cities).toBe(cities);
    });
  });

  it('should resolve the initiative cities even on err', () => {
    spyOn(GoVolunteerDataService, 'getInitiativeCities').and.callFake(() => {
      const deferred = q.defer();
      deferred.reject();
      return deferred.promise;
    });

    const result = GetCities(logger, state, GoVolunteerDataService, GoVolunteerService, q);
    expect(GoVolunteerDataService.getInitiativeCities).toHaveBeenCalledWith(initiativeId);
    result.then(() => {
      expect(GoVolunteerService.cities).toBe([]);
    });
  });

  it('should resolve organizations', () => {
    const fake = { orgId: 1, orgName: 'test' };
    spyOn(Organizations, 'getCurrentOrgs').and.callFake(() => {
      const deferred = q.defer();
      deferred.resolve(fake);
      return deferred.promise;
    });

    const result = GetOrganizations(Organizations, GoVolunteerService, q, logger);
    expect(Organizations.getCurrentOrgs).toHaveBeenCalled();
    result.then(() => {
      expect(GoVolunteerService.organizations).toBe(fake);
    });
  });
});
