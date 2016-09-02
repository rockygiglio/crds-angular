export default ngModule => {
  require('./multiCheckBoxCombo')(ngModule);
  require('./zipCodeValidator')(ngModule);
  require('./boldCheckBox')(ngModule);
  require('./profilePicture')(ngModule);
  require('./timePicker')(ngModule);
  require('./datePicker')(ngModule);
  require('./formlyBuilderInput')(ngModule);
  require('./formlyBuilderCheckbox')(ngModule);
  require('./formlyBuilderMultiCheckbox')(ngModule);
  require('./formlyBuilderRadio')(ngModule);
  require('./formlyBuilderRadioDesc')(ngModule);
  require('./formlyBuilderSelect')(ngModule);
  require('./formlyBuilderTextarea')(ngModule);
}