export default ngModule => {
  require('./formlyConfig/types')(ngModule);
  require('./formlyConfig/wrappers')(ngModule);
}