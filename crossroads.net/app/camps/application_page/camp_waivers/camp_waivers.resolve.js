/* @ngInject */
function getCampWaivers(CampsService, $state) {
  const campId = $state.toParams.campId;
  const contactId = $state.toParams.contactId;
  return CampsService.getCampWaivers(campId, contactId);
}

export default getCampWaivers;
