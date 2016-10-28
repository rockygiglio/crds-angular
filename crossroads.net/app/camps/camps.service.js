/* ngInject */
class CampService {
  constructor($resource, $stateParams, $log) {
    this.log = $log;
    this.stateParams = $stateParams;
    this.resource = $resource;
    // eslint-disable-next-line prefer-template
    this.campResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId');
    // eslint-disable-next-line prefer-template
    this.camperResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId/:camperId');
    // eslint-disable-next-line prefer-template
    this.camperResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId/:camperId');
    // eslint-disable-next-line prefer-template
    this.campDashboard = $resource(__API_ENDPOINT__ + 'api/my-camp');
    // eslint-disable-next-line prefer-template
    this.campFamily = $resource(__API_ENDPOINT__ + 'api/camps/:campId/family');
    // eslint-disable-next-line prefer-template
    this.campWaiversResource = $resource(__API_ENDPOINT__ + 'api/camps/waivers/:campId');
    this.campInfo = {};
    this.camperInfo = {};
    this.campTitle = '';
    this.waivers = [];
  }

  getCampInfo(campId) {
    return this.campResource.get({ campId }, (campInfo) => {
      this.campInfo = campInfo;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }

  getCamperInfo(campId, camperId) {
    return this.camperResource.get({ campId, camperId }, (camperInfo) => {
      this.camperInfo = camperInfo;
    },

    (err) => {
      console.log(err);
    }).$promise;
  }

  getCampDashboard() {
    return this.campDashboard.query((myCamps) => {
      this.dashboard = myCamps;
    },

   (err) => {
      this.log.error(err);
    }).$promise;
  }

  getCampFamily(campId) {
    return this.campFamily.query({ campId }, (family) => {
      this.family = family;
    }, (err) => {
      this.log.error(err);
    }).$promise;
  }

  getCampWaivers(campId) {
    return this.campWaiversResource.query({ campId }, (waivers) => {
      this.waivers = waivers;
    },

    (err) => {
      console.log(err);
    }).$promise;
  }

  submitWaivers(campId, contactId, waivers) {
    console.debug(`Camp: ${campId}, Contact: ${contactId}, Waivers:`, waivers);
  }

}

export default CampService;
