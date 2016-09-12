export default class FBComposerController {
  /*@ngInject*/
  constructor(fbMapperConfig, fbMapperService, $log, $rootScope) {
    this.fbMapperConfig = fbMapperConfig;
    this.rootScope = $rootScope;
    this.fbMapperService = fbMapperService;
    this.log = $log;
    this.invokedFields = _.isFunction(this.fields) ? this.fields() : this.fields;
    this.log.debug(this);
  }

  $onInit() {
    this.preparedFields = this.prepareFields(this.invokedFields);
  }

  prepareFields(builderFields) {
    //prepopulate
    ////find compositions used
    let compositions = [];
    //Composition Validation (check that it is valid)
    let fields = [];
    _.forEach(builderFields, (builderField) => {
      let field = builderField.formlyConfig;
      let keyArray = builderField.formlyConfig.key.split('.');
      let mapperConfigElement = this.fbMapperConfig.getElement(keyArray[keyArray.length - 1]);
      this.log.debug(mapperConfigElement);
      fields.push(field);
      compositions.push(keyArray[0])
    });

    debugger;
    compositions = _.uniq(compositions);

    //this is not finished, stopped here for the day
    this.model = this.fbMapperService.prepopulateCompositions(compositions);
    let unPopulate = _.where(builderFields, (field) => {
      return field.prepopulate === false;
    });
    debugger;
    _.forEach(unPopulate, (model) => {

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