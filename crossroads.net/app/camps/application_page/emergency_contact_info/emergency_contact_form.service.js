/* ngInject */
class EmergencyContactForm {

  constructor($resource) {
    this.formModel = {};

    this.emergencyContactResource = $resource(`${__API_ENDPOINT__}api/camps/:campId/emergencycontact/:contactId`);
  }

  save(campId, contactId) {
    // Create param array from model
    const contacts = [];
    contacts[0] = this.formModel.contacts['0'];
    contacts[0].primaryContact = true;

    if (this.formModel.additionalContact) {
      contacts[1] = this.formModel.contacts['1'];
    }

    return this.emergencyContactResource.save({ campId, contactId }, contacts).$promise;
  }

  // eslint-disable-next-line class-methods-use-this
  getFields() {
    return [
      {
        className: '',
        wrapper: 'campBootstrapRow',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'contacts[0].firstName',
          type: 'crdsInput',
          templateOptions: {
            label: 'First Name',
            required: true
          }
        }, {
          className: 'form-group col-xs-6',
          key: 'contacts[0].lastName',
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
          key: 'contacts[0].mobileNumber',
          type: 'crdsInput',
          optionsTypes: ['phoneNumber'],
          templateOptions: {
            label: 'Mobile Number',
            required: true
          }
        }, {
          className: 'form-group col-xs-6',
          key: 'contacts[0].email',
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
          key: 'contacts[0].relationship',
          type: 'crdsInput',
          templateOptions: {
            label: 'Relationship to Student',
            required: true
          }
        }]
      },
      {
        key: 'additionalContact',
        type: 'crdsRadio',
        templateOptions: {
          label: 'Is there an additional emergency contact?',
          required: true,
          labelProp: 'label',
          valueProp: 'additionalContact',
          inline: true,
          options: [{
            label: 'Yes',
            additionalContact: true
          }, {
            label: 'No',
            additionalContact: false
          }]
        }
      },
      {
        className: '',
        wrapper: 'campBootstrapRow',
        hideExpression: '!model.additionalContact',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'contacts[1].firstName',
          type: 'crdsInput',
          expressionProperties: {
            'templateOptions.required': 'model.additionalContact'
          },
          templateOptions: {
            label: 'First Name',
          }
        }, {
          className: 'form-group col-xs-6',
          key: 'contacts[1].lastName',
          type: 'crdsInput',
          expressionProperties: {
            'templateOptions.required': 'model.additionalContact'
          },
          templateOptions: {
            label: 'Last Name',
          }
        }]
      },
      {
        className: '',
        wrapper: 'campBootstrapRow',
        hideExpression: '!model.additionalContact',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'contacts[1].mobileNumber',
          type: 'crdsInput',
          optionsTypes: ['phoneNumber'],
          expressionProperties: {
            'templateOptions.required': 'model.additionalContact'
          },
          templateOptions: {
            label: 'Mobile Number',
          }
        }, {
          className: 'form-group col-xs-6',
          key: 'contacts[1].email',
          type: 'crdsInput',
          expressionProperties: {
            'templateOptions.required': 'model.additionalContact'
          },
          templateOptions: {
            label: 'Email',
            type: 'email',
          }
        }]
      },
      {
        className: '',
        wrapper: 'campBootstrapRow',
        hideExpression: '!model.additionalContact',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'contacts[1].relationship',
          type: 'crdsInput',
          expressionProperties: {
            'templateOptions.required': 'model.additionalContact'
          },
          templateOptions: {
            label: 'Relationship to Student',
          }
        }]
      },
    ];
  }

  getModel() {
    return this.formModel;
  }
}

export default EmergencyContactForm;
