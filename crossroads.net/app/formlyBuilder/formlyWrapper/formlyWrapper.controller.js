export default class FormlyWrapperController {
  /*@ngInject*/
  constructor(formlyMapperConfig, $log) {
    this.formlyMapperConfig = formlyMapperConfig;
    this.log = $log;
    this.invokedFields = this.fields();
    this.log.debug(this);
  }

  $onInit() {
  }
}