import WrapperDirective from './directives/formlyWrapper.directive';

export default ngModule => {
  ngModule.directive('formlyWrapper', () => new WrapperDirective);
}