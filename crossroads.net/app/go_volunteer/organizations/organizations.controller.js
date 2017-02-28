function getOrgSlug(name) {
  return name.split(' ')[0].toLowerCase();
}

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
    this.organizations = this.buildOrganizations().sort((a, b) => a.order - b.order);
    this.viewReady = true;
  }

  buildOrganizations() {
    return this.organizations.map((org) => {
      if (org.name.startsWith('Archdiocese of Cincinnati')) {
        return {
          name: 'Archdiocese',
          subtitle: 'of Cincinnati',
          order: 2,
          imageUrl: org.imageURL
        };
      } else if (org.name.startsWith('Other')) {
        return {
          name: 'Other',
          subtitle: 'Affiliate or Organization',
          order: 3,
          imageUrl: org.imageURL
        };
      } else if (org.name.startsWith('Crossroads')) {
        return {
          name: 'Crossroads Community Church',
          imageUrl: org.imageURL,
          order: 1,
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

  selectOrg({ name, }) {
    if (!name.startsWith('Crossroads')) {
      this.state.go('go-local.page', { organization: getOrgSlug(name), page: 'profile', initiativeId: this.state.toParams.initiativeId });
    }
  }
}
