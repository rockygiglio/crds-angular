import crdsConstants from 'crds-constants';
import filter from 'lodash/collection/filter';

/* ngInject */
class CampsService {
  constructor($resource, $stateParams, $log, AttributeTypeService) {
    this.log = $log;
    this.stateParams = $stateParams;
    this.resource = $resource;
    this.attributeTypeService = AttributeTypeService;

    // eslint-disable-next-line prefer-template
    this.campResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId');
    // eslint-disable-next-line prefer-template
    this.camperResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId/campers/:camperId');
    // eslint-disable-next-line prefer-template
    this.campDashboard = $resource(__API_ENDPOINT__ + 'api/my-camp');
    // eslint-disable-next-line prefer-template
    this.campFamily = $resource(__API_ENDPOINT__ + 'api/v1.0.0/camps/:campId/family');
    // eslint-disable-next-line prefer-template
    this.campMedicalResource = $resource(__API_ENDPOINT__ + 'api/v1.0.0/camps/:campId/medical/:contactId', { campId: '@campId', contactId: '@contactId' });
    // eslint-disable-next-line prefer-template
    this.campWaiversResource = $resource(__API_ENDPOINT__ + 'api/v1.0.0/camps/:campId/waivers/:contactId', { campId: '@campId', contactId: '@contactId' });
    this.medicalInfoResource = $resource(`${__API_ENDPOINT__}api/camps/medical/:contactId`);
    // eslint-disable-next-line prefer-template
    this.emergencyContactResource = $resource(__API_ENDPOINT__ + 'api/v1.0.0/camps/:campId/emergencycontact/:contactId', { campId: '@campId', contactId: '@contactId' });
    // eslint-disable-next-line prefer-template
    this.productSummaryResource = $resource(__API_ENDPOINT__ + 'api/camps/:campId/product/:camperId', { campId: '@campId', camperId: '@camperId' });
    // eslint-disable-next-line prefer-template
    this.paymentResource = $resource(__API_ENDPOINT__ + 'api/v1.0.0/invoice/:invoiceId/payment/:paymentId');
    this.confirmationResource = $resource(`${__API_ENDPOINT__}api/camps/:campId/confirmation/:contactId`);
    this.hasPaymentsResource = $resource(`${__API_ENDPOINT__}api/v1.0.0/invoice/:invoiceId/has-payment`);

    this.campInfo = null;
    this.campTitle = null;
    this.shirtSizes = null;
    this.camperInfo = null;
    this.waivers = null;
    this.productInfo = null;
    this.payment = null;
    this.campMedical = null;

    this.initializeCampData();
    this.initializeCamperData();
  }

  initializeCampData() {
    this.campInfo = {};
    this.campTitle = '';
    this.shirtSizes = [];
  }

  initializeCamperData() {
    this.camperInfo = {};
    this.waivers = [];
    this.productInfo = {};
    this.payment = {};
    this.campMedical = {};
  }

  getCampInfo(campId) {
    return this.campResource.get({ campId }, (campInfo) => {
      this.campInfo = campInfo;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }

  getCamperInfo(campId, camperId) {
    return this.camperResource.get({ campId, camperId }, (camperInfo) => {
      this.camperInfo = camperInfo;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }

  getCampDashboard() {
    return this.campDashboard.query((myCamps) => {
      this.dashboard = myCamps;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }

  getCampFamily(campId) {
    return this.campFamily.query({ campId }, (family) => {
      this.family = family;
    }, (err) => {
      this.log.error(err);
    }).$promise;
  }

  getCampWaivers(campId, contactId) {
    return this.campWaiversResource.query({ campId, contactId }, (waivers) => {
      this.waivers = waivers;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }

  getCampMedical(campId, contactId) {
    return this.campMedicalResource.get({ campId, contactId }, (medical) => {
      this.campMedical = medical;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }

  getCampProductInfo(campId, camperId) {
    return this.productSummaryResource.get({ campId, camperId }, (productInfo) => {
      this.productInfo = productInfo;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }

  submitWaivers(campId, contactId, waivers) {
    return this.campWaiversResource.save({ campId, contactId }, waivers).$promise;
  }

  getEmergencyContacts(campId, contactId) {
    return this.emergencyContactResource.query({ campId, contactId }).$promise;
  }

  saveEmergencyContacts(campId, contactId, contacts) {
    return this.emergencyContactResource.save({ campId, contactId }, contacts).$promise;
  }

  getCampPayment(invoiceId, paymentId) {
    return this.paymentResource.get({ paymentId, invoiceId }, (payment) => {
      this.payment = payment;
    },
    (err) => {
      this.log.error(err);
    }).$promise;
  }

  sendConfirmation(invoiceId, paymentId, campId, contactId) {
    this.confirmationResource.save({ contactId, campId, invoiceId, paymentId }, {});
  }

  getShirtSizes() {
    return this.attributeTypeService.AttributeTypes().get({ id: crdsConstants.ATTRIBUTE_TYPE_IDS.TSHIRT_SIZES }).$promise
      .then((shirtSizes) => {
        this.shirtSizes = filter(shirtSizes.attributes, attribute => attribute.category === 'Adult');
        return shirtSizes;
      });
  }

  invoiceHasPayment(invoiceId) {
    return this.hasPaymentsResource.get({ invoiceId }).$promise;
  }
}

export default CampsService;
