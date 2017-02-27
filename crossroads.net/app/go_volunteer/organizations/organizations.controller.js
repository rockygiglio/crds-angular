export default class OrganizationsController {
  /* @ngInject */
  constructor($state, GoVolunteerService) {
    this.viewReady = false;
    this.selectedCity = null;
    this.cities = GoVolunteerService.cities;
    this.organizations = GoVolunteerService.organizations;
    this.state = $state;
    this.cincinnati = [{
      name: 'Cincinnati or Central Kentucky Crossroads Sites',
      projectId: -1
    }];
  }

  $onInit() {
    this.organizations = this.buildOrganizations();
    this.viewReady = true;
  }

  buildOrganizations() {
    return this.organizations.map((org) => {
      if (org.name === 'Archdiocese of Cincinnati') {
        return {
          name: 'Archdiocese',
          subtitle: 'of Cincinnati',
          imageUrl: org.imageURL
        };
      } else if (org.name === 'Other') {
        return {
          name: 'Other',
          subtitle: 'Affiliate or Organization',
          imageUrl: org.imageURL
        };
      } else if (org.name === 'Crossroads') {
        return {
          name: 'Crossroads Community Church',
          imageUrl: org.imageURL,
          cities: [...this.cincinnati, ...this.buildCities()]
        };
      }
      return {
        name: org.name,
        imageUrl: org.imageURL
      };
    });
  }

  buildCities() {
    return this.cities.map((city) => {
      const name = { name: `${city.city}, ${city.state}` };
      return Object.assign(name, city);
    });
  }

  selectCity({ projectId, city }) {
    if (projectId === -1) {
      this.state.go('go-local.cincinnatipage', { initiativeId: this.state.toParams.initiativeId, organization: 'crossroads', page: 'profile' });
    } else {
      this.state.go('go-local.anywherepage', { initiativeId: this.state.toParams.initiativeId, city, projectId });
    }
  }
}

