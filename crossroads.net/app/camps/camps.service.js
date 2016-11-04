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
    this.campWaiversResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId/waivers/:contactId', { campId: '@campId', contactId: '@contactId' });
    // eslint-disable-next-line prefer-template
    this.campMedicalResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId/medical/:contactId', { campId: '@campId', contactId: '@contactId' });

    this.campInfo = {};
    this.camperInfo = {};
    this.campTitle = '';
    this.waivers = [];
    this.campMedical = {};
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

  getCampWaivers(campId, contactId) {
    return this.campWaiversResource.query({ campId, contactId }, (waivers) => {
      this.waivers = waivers;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }

  getCampMedical(campId, contactId) {
    return this.campMedicalResource.get({ campId, contactId }, (medical) => {
      this.medical = medical;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }

  submitWaivers(campId, contactId, waivers) {
    return this.campWaiversResource.save({ campId, contactId }, waivers).$promise;
  }

}

export default CampService;
