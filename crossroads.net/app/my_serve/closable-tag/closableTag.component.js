import controller from './closableTag.controller';

export default function closableTagComponent() {
  return {
    bindings: {
      name: '<',
      age: '<',
      onClick: '&',
      onClose: '&',
      isLeader: '='
    },
    templateUrl: 'closable-tag/closableTag.html',
    controller: controller,
    controllerAs: 'closableTag'
  }
}