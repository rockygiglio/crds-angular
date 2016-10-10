import controller from './closableTag.controller';

export default function closableTagComponent() {
  return {
    bindings: {
      name: '<',
      onClick: '&',
      onClose: '&'
    },
    templateUrl: 'closable-tag/closableTag.html',
    controller: controller,
    controllerAs: 'closableTag'
  }
}