export default class CampMedicineController {
  /* @ngInject */
  constructor($scope) {
    this.$scope = $scope;
    $scope.addMedicineFields = () => {
      this.addMedicineFields();
    };
  }

  addMedicineFields() {
    this.$scope.fields.push(this.getMedicineFields());
  }

  getMedicineFields() {
    return {
      className: '',
      wrapper: 'campBootstrapRow',
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
    };
  }
}
