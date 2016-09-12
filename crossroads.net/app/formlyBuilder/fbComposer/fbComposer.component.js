import controller from './fbComposer.controller';
import html from './fbComposer.html';

FBComposerComponent.$inject = [];

export default function FBComposerComponent() {

//why doesn't binding work with < binding - cycles through in a loop??
//what is difference between template and templateURL (omit import line at top)
  let fbComposerComponent = {
    bindings: {
      fields: '&'
    },
    restrict: 'E',
    template: html,
    controller: controller,
    controllerAs: 'ctrl',
    bindToController: true
  };

  return fbComposerComponent;

}