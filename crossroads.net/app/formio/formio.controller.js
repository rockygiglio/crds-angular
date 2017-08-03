

/* @ngInject */
export default class FormioController {
  constructor($state) {
    this.state = $state;
    this.stateParams = $state.params;

    this.formPath = this.stateParams.formPath;
  }

  $onInit() {
    console.log(this.stateParams);
  }
}
