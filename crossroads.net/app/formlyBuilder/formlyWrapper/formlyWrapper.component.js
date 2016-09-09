import controller from './formlyWrapper.controller';
import html from './formlyWrapper.html';

FormlyWrapperComponent.$inject = [];

export default function FormlyWrapperComponent() {

//why doesn't binding work with < binding - cycles through in a loop??
//what is difference between template and templateURL (omit import line at top)
  let formlyWrapperComponent = {
    bindings: {
      fields: '&'
    },
    restrict: 'E',
    template: html,
    controller: controller,
    controllerAs: 'ctrl',
    bindToController: true
  };

  return formlyWrapperComponent;

}