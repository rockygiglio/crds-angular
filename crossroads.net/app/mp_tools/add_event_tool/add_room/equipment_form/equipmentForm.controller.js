import CONSTANTS from 'crds-constants';

export default class EquipmentController {
    /* @ngInject */
    constructor(AddEvent, Validation) {
      this.addEvent = AddEvent;
      this.validation = Validation;
    }

    addEquipment() {
      this.currentEquipment.push({equipment: {name: null, quantity: 0 }});
    }

    existing(equipment) {
      return _.has(equipment, 'cancelled');
    }

    fieldName(name, idx) {
      return name + '-' + idx;
    }

    isCancelled(equipment) {
      return existing(equipment) && equipment.cancelled;
    }

    remove(idx) {
      if (this.currentEquipment[idx] !== undefined) {
        if (existing(this.currentEquipment[idx].equipment)) {
          this.currentEquipment[idx].equipment.cancelled = true;
        } else {
          this.currentEquipment.splice(idx, 1);
        }
      }
    }

    showError(form) {
      return Validation.showErrors(form, 'equipmentChooser') ||
        Validation.showErrors(form, 'equip.quantity');
    }

    showFieldError(form, name) {
      return Validation.showErrors(form, name);
    }

    undo(idx) {
      if (this.currentEquipment[idx] !== undefined) {
        if (existing(this.currentEquipment[idx].equipment)) {
          this.currentEquipment[idx].equipment.cancelled = false;
        }
      }
    }
};