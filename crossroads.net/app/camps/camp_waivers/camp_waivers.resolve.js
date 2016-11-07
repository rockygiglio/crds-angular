function getCampWaivers(CampsService, $stateParams) {
  const campId = $stateParams.campId;
  const contactId = $stateParams.contactId;
  return CampsService.getCampWaivers(campId, contactId);
}

export default getCampWaivers;
