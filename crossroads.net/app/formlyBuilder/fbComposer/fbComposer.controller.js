export default class FBComposerController {
  /*@ngInject*/
  constructor(fbMapperConfig, fbMapperService, $log, $rootScope ) {
    this.fbMapperConfig = fbMapperConfig;
    this.rootScope = $rootScope;
    this.fbMapperService = fbMapperService;
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
      let mapperConfigElement = this.fbMapperConfig.getElement(builderField.key.substring(builderField.key.lastIndexOf('.') + 1));
      this.log.debug(mapperConfigElement);
      fields.push(field);
    });
    return fields;
  }

  prepareValidation(field, validations) {

  }

  submit() {
    try {
      var promise = this.fbMapperService.saveFormlyFormData(this.model)
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