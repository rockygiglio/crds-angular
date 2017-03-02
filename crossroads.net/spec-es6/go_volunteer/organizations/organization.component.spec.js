import goVolunteerModule from '../../../app/go_volunteer/goVolunteer.module';
import helpers from '../goVolunteer.helpers';

describe('Go Volunteer Organizations Page', () => {
  let componentController;
  let organizationController;
  let goVolunteerService;
  let state;

  const bindings = {};
  const initiativeId = 3;

  beforeEach(angular.mock.module(goVolunteerModule));

  beforeEach(inject((_$componentController_, _$httpBackend_, _GoVolunteerService_, _$state_) => {
    goVolunteerService = _GoVolunteerService_;
    componentController = _$componentController_;
    state = _$state_;
    state.toParams = {
      initiativeId
    };
    goVolunteerService.organizations = helpers.organizations;
  }));

  describe('Build organizations', () => {
    beforeEach(() => {
      organizationController = componentController('goVolunteerOrganizations', null, bindings);
      organizationController.$onInit();
    });

    it('should transform the imageURL to imageUrl', () => {
      organizationController.organizations.forEach((org) => {
        expect(org.imageUrl).toBeDefined();
        expect(org.imageURL).toBeUndefined();
      });
    });

    it('should include cities for crossroads', () => {
      const crossroads = organizationController.organizations.find(org => org.name === 'Crossroads Community Church');
      expect(crossroads.cities).toBeDefined();
      expect(crossroads.cities.length).toBe(1);
    });
  });

  describe('No Cities returned', () => {
    beforeEach(() => {
      goVolunteerService.cities = [];
      organizationController = componentController('goVolunteerOrganizations', null, bindings);
      organizationController.$onInit();
    });

    it('should still display Cincinnati and Central KY', () => {
      const crossroads = organizationController.organizations.filter(org => org.name === 'Crossroads Community Church');
      expect(crossroads.length).toBe(1);
      expect(crossroads[0].cities.length).toBe(1);
      expect(crossroads[0].cities[0].name).toBe('Cincinnati or Central Kentucky Crossroads Sites');
    });
  });

  describe('Multiple Cities Returned', () => {
    beforeEach(() => {
      goVolunteerService.cities = helpers.cities;
      organizationController = componentController('goVolunteerOrganizations', null, bindings);
      organizationController.$onInit();
    });

    it('should include all the cities for the crossroads org', () => {
      const crossroads = organizationController.organizations.filter(org => org.name === 'Crossroads Community Church');
      expect(crossroads.length).toBe(1);
      expect(crossroads[0].cities.length).toBe(1 + helpers.cities.length);
    });

    it('should include a name attribute for cities for the crossroads org', () => {
      const crossroads = organizationController.organizations.filter(org => org.name === 'Crossroads Community Church');
      expect(crossroads.length).toBe(1);
      expect(crossroads[0].cities.length).toBe(1 + helpers.cities.length);
      expect(crossroads[0].cities[1].name).toBeDefined();
      expect(crossroads[0].cities[1].name).toBe('Cleveland, OH');
    });
  });

  describe('City Selected', () => {
    beforeEach(() => {
      goVolunteerService.cities = helpers.cities;

      spyOn(state, 'go');

      organizationController = componentController('goVolunteerOrganizations', null, bindings);
      organizationController.$onInit();
    });

    it('should navigate to the anywhere page when a city has been selected', () => {
      const selectedCity = helpers.cities[0];
      organizationController.selectCity(selectedCity);
      expect(state.go).toHaveBeenCalledWith('go-local.anywherepage', {
        initiativeId,
        city: selectedCity.city,
        projectId: selectedCity.projectId
      });
    });

    it('should navigate to the cincinnatipage when cincinnati/cky has been selected', () => {
      const selectedCity = organizationController.cincinnati[0];
      organizationController.selectCity(selectedCity);
      expect(state.go).toHaveBeenCalledWith('go-local.cincinnatipage', {
        initiativeId,
        organization: 'crossroads',
        page: 'profile'
      });
    });
  });
});
