import moment from 'moment';

function isBetween(startDate, endDate) {
  const now = moment();
  return now.isSameOrAfter(startDate) && now.isSameOrBefore(endDate);
}

/* @ngInject */
class CampController {
  constructor(CampsService, $rootScope, $stateParams) {
    this.campId = $stateParams.campId;
    this.campsService = CampsService;
    this.isClosed = false;
    this.rootScope = $rootScope;
    this.stateParams = $stateParams;
    this.viewReady = false;
  }

  $onInit() {
    this.campsService.campTitle = this.campsService.campInfo.eventTitle;
    this.isClosed = !isBetween(this.campsService.campInfo.registrationStartDate,
                               this.campsService.campInfo.registrationEndDate);
    this.viewReady = true;
  }

}
export default CampController;
