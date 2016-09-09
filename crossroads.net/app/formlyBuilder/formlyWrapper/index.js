import WrapperComponent from './formlyWrapper.component';

export default ngModule => {
  ngModule.component('formlyWrapper', WrapperComponent());
  //ngModule.directive('formlyWrapper', () => new WrapperDirective);
}