export default class FormlyWrapperController {
  /*@ngInject*/
  constructor(formlyMapperConfig, formlyMapperService, $log, $rootScope ) {
    this.formlyMapperConfig = formlyMapperConfig;
    this.rootScope = $rootScope;
    this.formlyMapperService = formlyMapperService;
    this.log = $log;
    this.invokedFields = this.fields();
    this.log.debug(this);
  }

  $onInit() {
    this.preparedFields = this.prepareFields(this.invokedFields);
  }

  prepareFields(builderFields) {
    //Composition Validation (check that it is valid)
    let fields = [];
    _.forEach(builderFields, (builderField) => {
      let field = builderField;
      let mapperConfigElement = this.formlyMapperConfig.getElement(builderField.key.substring(builderField.key.lastIndexOf('.') + 1));
      this.log.debug(mapperConfigElement);
      fields.push(field);
    });
    return fields;
  }

  prepareValidation(field, validations) {

  }

  submit() {
    try {
      var promise = this.formlyMapperService.saveFormlyFormData(this.model)
        .then((data) => {
          this.rootScope.$emit('notify', this.rootScope.MESSAGES.successfulSubmission);
          this.log.debug(this.model);
        })
    }
    catch (error) {
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      throw (error);
    }
  }


}