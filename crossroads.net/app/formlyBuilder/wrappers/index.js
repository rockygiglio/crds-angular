export default ngModule => {
  require('./formlyBuilder-show-error')(ngModule);
  require('./formlyBuilder-label')(ngModule);
  require('./formlyBuilder-has-error')(ngModule);
}