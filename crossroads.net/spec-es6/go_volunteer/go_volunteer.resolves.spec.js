import goVolunteerModule from '../../app/go_volunteer/goVolunteer.module';
import { GetDashboard, GetProject, GetCities, GetOrganizations } from '../../app/go_volunteer/goVolunteer.resolves';

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

  it('should fail to resolve the dashboard', () => {
    spyOn(GoVolunteerDataService, 'getDashboard').and.callFake(() => {
      const deferred = q.defer();
      deferred.reject({ stausCode: 500 });
      return deferred.promise;
    });

    spyOn(logger, 'error');

    const result = GetDashboard(state, GoVolunteerDataService, GoVolunteerService, q, logger);
    expect(GoVolunteerDataService.getDashboard).toHaveBeenCalledWith(projectId);
    result.then(() => {
    }, () => {
      expect(GoVolunteerService.dashboard).toBe(undefined);
      expect(logger.error).toHaveBeenCalled();
    });
  });

  it('should resovle the dashboard', () => {
    const participants = [
      { name: 'Jenny Shultz', email: 'jshultz@hotmail.com', phone: '205-333-5962', adults: 1, children: 2 },
      { name: 'Jamie Hanks', email: 'jaha95@gmail.com', phone: '205-333-5962', adults: 0, children: 2 },
      { name: 'Jennie Jones', email: 'jjgirl@yahoo.com', phone: '205-334-5988', adults: 2, children: 0 },
      { name: 'Jimmy Hatfield', email: 'jhattyhat@yahoo.com', phone: '205-425-5772', adults: 0, children: 2 },
      { name: 'Elisha Underwood', email: 'eu@yahoo.com', phone: '205-259-2777', adults: 0, children: 2 },
      { name: 'Terry Washington', email: 'tdub777@yahoo.com', phone: '205-259-8954', adults: 1, children: 1 },
      { name: 'Jim Wolf', email: 'dwolf@yahoo.com', phone: '205-334-9584', adults: 1, children: 2 }
    ];
    spyOn(GoVolunteerDataService, 'getDashboard').and.callFake(() => {
      const deferred = q.defer();
      deferred.resolve(participants);
      return deferred.promise;
    });

    const result = GetDashboard(state, GoVolunteerDataService, GoVolunteerService, q);
    expect(GoVolunteerDataService.getDashboard).toHaveBeenCalledWith(projectId);
    result.then(() => {
      expect(GoVolunteerService.dashboard).toBe(participants);
    });
  });
});
