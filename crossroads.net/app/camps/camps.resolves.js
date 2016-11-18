export function getCamperInfo(CampsService, $state) {
  const camperId = $state.toParams.contactId;
  const campId = $state.toParams.campId;
  return CampsService.getCamperInfo(campId, camperId);
}

export function getCampInfo(CampsService, $state) {
  const id = $state.toParams.campId;
  return CampsService.getCampInfo(id);
}

export function getCampProductInfo(CampsService, $state) {
  const campId = $state.toParams.campId;
  const camperId = $state.toParams.contactId;
  return CampsService.getCampProductInfo(campId, camperId);
}

export function getCamperPayment(CampsService, $state) {
  const invoiceId = $state.toParams.invoiceId;
  const paymentId = $state.toParams.paymentId;
  return CampsService.getCampPayment(invoiceId, paymentId);
}

export function getCampMedical(CampsService, $state) {
  const campId = $state.toParams.campId;
  const contactId = $state.toParams.contactId;
  return CampsService.getCampMedical(campId, contactId);
}

export function getCamperFamily(CampsService, $state) {
  const id = $state.toParams.campId;
  return CampsService.getCampFamily(id);
}

export function getCampWaivers(CampsService, $state) {
  const campId = $state.toParams.campId;
  const contactId = $state.toParams.contactId;
  return CampsService.getCampWaivers(campId, contactId);
}
