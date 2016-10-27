/* ngInject */
class CampService {
  constructor($resource) {
    this.resource = $resource;
    this.campResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId');
    this.campDashboard = $resource(__API_ENDPOINT__ + 'api/my-camp');
    this.campWaiversResource = $resource(__API_ENDPOINT__ + 'api/camps/waivers/:campId');

    this.campInfo = {};
    this.waivers = [];
  }

  getCampInfo(campId) {
    return this.campResource.get({ campId }, (campInfo) => {
      this.campInfo = campInfo;
    },

    (err) => {
      console.log(err);
    }).$promise;
  }

  getCampDashboard() {
    return this.campDashboard.query( (myCamps) => {
      this.dashboard = myCamps;
    },

   (err) => {
      console.error(err);
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
