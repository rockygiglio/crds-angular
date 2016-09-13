export default class FBComposerController {
  /*@ngInject*/
  constructor(fbMapperConfig, fbMapperService, $log, $rootScope, $q) {
    this.fbMapperConfig = fbMapperConfig;
    this.rootScope = $rootScope;
    this.fbMapperService = fbMapperService;
    this.log = $log;
    this.qApi = $q;
    this.invokedFields = _.isFunction(this.fields) ? this.fields() : this.fields;
    this.log.debug(this);
    this.prePopulationComplete = false;
  }

  $onInit() {
    debugger;
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

    compositions = _.uniq(compositions);

    //this is not finished, stopped here for the day
    this.fbMapperService.prepopulateCompositions(compositions)
      .then((data) => {
        debugger;
        this.model = data;
        // let unPopulate = _.where(builderFields, { prePopulate: false });
        // _.forEach(unPopulate, (field) => {
        //   this.model[field.formlyConfig.key] = undefined;
        // });      
        this.prePopulationComplete = true;
        return fields;
      });
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