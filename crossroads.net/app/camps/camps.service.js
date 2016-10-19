/* ngInject */
class CampService {
  constructor($resource) {
    this.resource = $resource;
    this.campResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId');
    this.campDashboard = $resource(__API_ENDPOINT__ + 'api/my-camp');
    this.campInfo = {};
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

}

export default CampService;
