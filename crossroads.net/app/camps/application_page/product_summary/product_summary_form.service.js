/* ngInject */
class ProductSummaryForm {

  constructor($resource) {
    this.formModel = {
      financialAssistance: false
    };

    this.productSummaryResource = $resource(`${__API_ENDPOINT__}api/camps/product`);
  }

  save(eventId, contactId) {
    const params = {
      eventId,
      contactId,
      financialAssistance: this.formModel.financialAssistance
    };

    return this.productSummaryResource.save({ }, params).$promise;
  }

  getModel() {
    return this.formModel;
  }
}

export default ProductSummaryForm;
