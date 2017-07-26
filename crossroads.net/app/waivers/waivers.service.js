

/* @ngInject */
export default class WaiversService {
  constructor($resource) {
    this.waiverResource = $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/v1.0.0/waivers/:waiverId`);
  }

  getWaiver(waiverId) {
    return this.waiverResource.get({ waiverId }).$promise;
  }
}
