export default class OrganizationService {
  /* ngInject */
  constructor($resource) {
    this.ByName = $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/organization/:name`);
    this.LocationsForOrg = $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/organizations/:orgId/locations`);
    this.Others = $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/organizations/other`);
    this.Current = $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/v1.0.0/go-volunteer/organizations`);
  }

  getCurrentOrgs() {
    return this.Current.query().$promise;
  }
}
