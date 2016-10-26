function getCampWaivers(CampsService, $stateParams) {
  const campId = $stateParams.campId;
  return CampsService.getCampWaivers(campId);
}

const resolve = {
  loggedin: crds_utilities.checkLoggedin,
  campsService: 'CampsService',
  getCampWaivers,
  $stateParams: '$stateParams'
};

export default resolve;
