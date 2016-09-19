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
    this.prepareForm(this.invokedFields)
      .then((form) => {
        this.preparedFields = form.fields;
        this.model = form.model;
      });
  }
//making change so can push and change archive name 
  prepareForm(builderFields) {
    let form = {
      model: {},
      fields: []
    };
    let compositions = _.uniq(_.map(builderFields, (field) => { return field.mapperSuperPath.split('.')[0] }));
    let formBuildingOperations = [
      this._prepareFields(builderFields),
      this._prepareModel(builderFields, compositions)
    ]
    return this.qApi.all(formBuildingOperations)
            .then((dataArray) => {
              form.fields = dataArray[0];
              form.model = dataArray[1];
              return form;
            })
            .catch((err) => {
              this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
              throw (err);
            });
  }

  _prepareModel(builderFields, compositions) {
    let prepareModel = {};
    return this.fbMapperService.prepopulateCompositions(compositions)
      .then((data) => {
        prepareModel = data;
        let unPopulate = _.where(builderFields, { prePopulate: false });
        _.forEach(unPopulate, (field) => {
          _.set(prepareModel, field.formlyConfig.key, undefined);
        });
        return prepareModel;
      });
  }

  _prepareFields(builderFields) {
    let fields = [];
    _.forEach(builderFields, (builderField) => {
      let field = builderField.formlyConfig;
      field.key = builderField.mapperSuperPath;
      // Generate formly fields object
      let fieldPromise = this.fbMapperConfig.getElement(field.key.split('.').pop())
        .then((mapperConfigElement) => {
          if (_.has(mapperConfigElement, 'lookupData')) {
            field.templateOptions.options = mapperConfigElement.lookupData;
            field.templateOptions.labelProp = mapperConfigElement.model.lookup.labelProp;
            field.templateOptions.valueProp = mapperConfigElement.model.lookup.valueProp;
          }
          return field;
        });
      fields.push(fieldPromise);
    });
    return this.qApi.all(fields);
  }

  _prepareValidation(field, validations) {

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