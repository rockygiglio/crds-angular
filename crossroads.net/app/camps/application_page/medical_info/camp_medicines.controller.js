// Based on http://angular-formly.com/#/example/advanced/repeating-section
export default class CampMedicineController {
  /* @ngInject */
  constructor($scope) {
    this.unique = 1;
    this.$scope = $scope;
    this.$scope.formOptions = { formState: $scope.formState };
    this.$scope.copyFields = () => this.copyFields;
    this.$scope.addNew = () => this.addNew();
    this.$scope.remove = ($index) => this.remove($index);
    this.$scope.showTrash = ($index) => this.showTrash($index);

    this.$scope.addMedicineFields = () => {
      this.addMedicineFields();
    };

    // Init
    this.$scope.fields = this.copyFields(this.$scope.to.fields);
  }

  copyFields(fields) {
    fields = angular.copy(fields);
    this.addRandomIds(fields);
    return fields;
  }

  addNew() {
    this.$scope.model[this.$scope.options.key] = this.$scope.model[this.$scope.options.key] || [];
    var repeatsection = this.$scope.model[this.$scope.options.key];
    var lastSection = repeatsection[repeatsection.length - 1];
    var newsection = {};
    if (lastSection) {
      newsection = angular.copy(lastSection);
    }
    repeatsection.push(newsection);
  }

  remove($index) {
    if (this.$scope.model[this.$scope.options.key].length > 1) {
      this.$scope.model[this.$scope.options.key].splice($index, 1);
    }
  }

  showTrash($index) {
    return $index > 0 || this.$scope.model[this.$scope.options.key].length > 1;
  }

  addRandomIds(fields) {
    this.unique++;
    angular.forEach(fields, (field, index) => {
      if (field.fieldGroup) {
        this.addRandomIds(field.fieldGroup);
        return; // fieldGroups don't need an ID
      }

      if (field.templateOptions && field.templateOptions.fields) {
        this.addRandomIds(field.templateOptions.fields);
      }

      field.id = field.id || (field.key + '_' + index + '_' + this.unique + this.getRandomInt(0, 9999));
    });
  }

  getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
  }

}
