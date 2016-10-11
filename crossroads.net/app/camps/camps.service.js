/* ngInject */
class CampService {
  constructor($resource) {
    this.resource = $resource;
    this.camp = $resource(__API_ENDPOINT__ + 'api/camps/:eventId');
    this.campInfo = {};
  }

  getCampInfo(eventId) {
    return this.camp.get({ eventId });
  }
}

export default CampService;
