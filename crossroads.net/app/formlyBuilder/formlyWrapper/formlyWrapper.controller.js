export default class FormlyWrapperController {
  /*@ngInject*/
  constructor(formlyMapperConfig, $log) {
    this.formlyMapperConfig = formlyMapperConfig;
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

  submit()
  {
    this.log.debug(this.model);
  }
}