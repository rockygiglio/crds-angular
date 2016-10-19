/* ngInject */
class CampService {
  constructor($resource) {
    this.resource = $resource;
    // eslint-disable-next-line prefer-template
    this.campResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId');
    // eslint-disable-next-line prefer-template
    this.campDashboard = $resource(__API_ENDPOINT__ + 'api/my-camp');
    this.campFamily = $resource(`${__API_ENDPOINT__}api/camps/family`);
    this.campInfo = {};
    this.campTitle = '';
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
    return this.campDashboard.query((myCamps) => {
      this.dashboard = myCamps;
    },

   (err) => {
     console.error(err);
   }).$promise;
  }

  getSummerCampFamily() {
    return this.campFamily.query({ summerCamp: true }, (family) => {
      this.family = family;
    }, (err) => {
      console.error(err);
    }).$promise;
  }

}

export default CampService;
