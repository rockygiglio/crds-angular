import controller from './equipmentForm.controller';
import template from './equipment_form.html';

export default function EquipmentFormComponent() {
  return {
    restrict: 'E',
    bindings: {
        currentEquipment: '=',
        equipmentLookup: '='
    },
    template,
    controller,
    controllerAs: 'equipment'
  };
};
