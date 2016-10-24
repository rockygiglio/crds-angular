class EmergencyContactForm {

  constructor() {
    this.formModel = {
      firstName: null,
      lastName: null,
      mobileNumber: null,
      email: null,
      relationship: null
    };
  }

  // eslint-disable-next-line class-methods-use-this
  getFields() {
    return [
      {
        className: 'row',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'firstName',
          type: 'crdsInput',
          templateOptions: {
            label: 'First Name',
            requrired: true
          }
        }, {
          className: 'form-group col-xs-6',
          key: 'lastName',
          type: 'crdsInput',
          templateOptions: {
            label: 'Last Name',
            requrired: true
          }
        }]
      },
      {
        className: 'row',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'mobileNumber',
          type: 'crdsInput',
          optionsTypes: ['phoneNumber'],
          templateOptions: {
            label: 'Mobile Number',
            required: true,
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
    ];
  }

  getModel() {
    return this.formModel;
  }
}

export default EmergencyContactForm;
