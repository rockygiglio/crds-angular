/* ngInject */
class CampService {
  constructor($resource) {
    this.resource = $resource;
    this.campResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId');
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

}

export default CampService;
