/* ngInject */
class EmergencyContactForm {

  constructor($resource) {
    this.formModel = {
      firstName: null,
      lastName: null,
      mobileNumber: null,
      email: null,
      relationship: null
    };

    this.emergencyContactResource = $resource(`${__API_ENDPOINT__}api/camps/:campId/emergencycontact/:contactId`);
  }

  save(campId, contactId) {
    return this.emergencyContactResource.save({ campId, contactId }, this.formModel).$promise;
  }

  // eslint-disable-next-line class-methods-use-this
  getFields() {
    return [
      {
        className: '',
        wrapper: 'campBootstrapRow',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'firstName',
          type: 'crdsInput',
          templateOptions: {
            label: 'First Name',
            required: true
          }
        }, {
          className: 'form-group col-xs-6',
          key: 'lastName',
          type: 'crdsInput',
          templateOptions: {
            label: 'Last Name',
            required: true
          }
        }]
      },
      {
        className: '',
        wrapper: 'campBootstrapRow',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'mobileNumber',
          type: 'crdsInput',
          optionsTypes: ['phoneNumber'],
          templateOptions: {
            label: 'Mobile Number',
            required: true
          }
        }, {
          className: 'form-group col-xs-6',
          key: 'email',
          type: 'crdsInput',
          templateOptions: {
            label: 'Email',
            type: 'email',
            required: true
          }
        }]
      },
      {
        className: '',
        wrapper: 'campBootstrapRow',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'relationship',
          type: 'crdsInput',
          templateOptions: {
            label: 'Relationship to Student',
            required: true
          }
        }]
      }
    ];
  }

  getModel() {
    return this.formModel;
  }
}

export default EmergencyContactForm;
