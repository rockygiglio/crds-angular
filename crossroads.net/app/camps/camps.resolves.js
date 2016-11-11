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
