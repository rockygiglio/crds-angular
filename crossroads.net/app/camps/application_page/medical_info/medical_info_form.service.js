/* ngInject */
class MedicalInfoForm {

  constructor($resource) {
    this.formModel = {
      insuranceCompanyName: null,
      policyHolderName: null,
      physicianName: null,
      physicianNumber: null,
      showAllergies: null,
      medicineAllergies: null
    };

    this.medicalInfoResource = $resource(`${__API_ENDPOINT__}api/camps/medical/:contactId`);
  }

  save(contactId) {
    return this.medicalInfoResource.save({ contactId }, this.formModel).$promise;
  }

  // eslint-disable-next-line class-methods-use-this
  getFields() {
    return [
      {
        className: 'row',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'insuranceCompany',
          type: 'crdsInput',
          templateOptions: {
            label: 'Insurance Company Name',
            required: false
          }
        }, {
          className: 'form-group col-xs-6',
          key: 'policyHolder',
          type: 'crdsInput',
          templateOptions: {
            label: 'Policy Holder Name',
            required: false
          }
        }]
      },
      {
        className: 'row',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'physicianName',
          type: 'crdsInput',
          templateOptions: {
            label: 'Physician Name',
            required: false
          }
        }, {
          className: 'form-group col-xs-6',
          key: 'physicianPhone',
          type: 'crdsInput',
          optionsTypes: ['phoneNumber'],
          templateOptions: {
            label: 'Physician Phone Number',
            required: false
          }
        }]
      },
      {
        className: 'row',
        fieldGroup: [{
          className: 'form-group col-xs-6',
          key: 'showAllergies',
          type: 'crdsRadio',
          templateOptions: {
            label: 'Are there any Allergy/Dietary Needs?',
            required: true,
            inline: true,
            labelProp: 'label',
            valueProp: 'id',
            options: [
              {
                label: 'Yes',
                id: true
              },
              {
                label: 'No',
                id: false
              }
            ]
          }
        }]
      },
      {
        className: 'row',
        hideExpression: () => !this.formModel.showAllergies,
        fieldGroup: [{
          className: 'form-group col-xs-12',
          key: 'medicineAllergies',
          type: 'crdsTextArea',
          templateOptions: {
            label: 'Medicine Allergies',
            required: false
          }
        }]
      }
    ];
  }

  getModel() {
    return this.formModel;
  }
}

export default MedicalInfoForm;
