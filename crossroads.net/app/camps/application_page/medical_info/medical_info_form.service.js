/* ngInject */
class MedicalInfoForm {

  constructor($resource) {
    this.formModel = {
      insuranceCompanyName: undefined,
      policyHolderName: undefined,
      physicianName: undefined,
      physicianNumber: undefined,
      showAllergies: undefined || true,
      medicineAllergies: undefined,
      foodAllergies: undefined,
      environmentAllergies: undefined,
      otherAllergies: undefined
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
          key: 'insuranceCompanyName',
          type: 'crdsInput',
          templateOptions: {
            label: 'Insurance Company Name',
            required: false
          }
        }, {
          className: 'form-group col-xs-6',
          key: 'policyHolderName',
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
            options: [{
              label: 'Yes',
              id: true
            }, {
              label: 'No',
              id: false
            }]
          }
        }]
      },
      {
        className: 'row',
        hideExpression: () => !this.formModel.showAllergies,
        fieldGroup: [{
          className: 'col-xs-12',
          template: '<p>List all allergies, reactions and treatments to allergies.</p>'
        }, {
          className: 'form-group col-xs-12',
          key: 'medicineAllergies',
          type: 'crdsTextArea',
          templateOptions: {
            label: 'Medicine Allergies',
            required: false
          }
        }, {
          className: 'form-group col-xs-12',
          key: 'foodAllergies',
          type: 'crdsTextArea',
          templateOptions: {
            label: 'Food Allergies',
            required: false
          }
        }, {
          className: 'form-group col-xs-12',
          key: 'environmentAllergies',
          type: 'crdsTextArea',
          templateOptions: {
            label: 'Environmental Allergies',
            required: false
          }
        }, {
          className: 'form-group col-xs-12',
          key: 'otherAllergies',
          type: 'crdsTextArea',
          templateOptions: {
            label: 'Other Allergies',
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
