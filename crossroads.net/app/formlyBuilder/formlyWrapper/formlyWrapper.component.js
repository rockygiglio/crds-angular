import controller from './formlyWrapper.controller';
import html from './formlyWrapper.html';

FormlyWrapperComponent.$inject = [];

export default function FormlyWrapperComponent() {

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