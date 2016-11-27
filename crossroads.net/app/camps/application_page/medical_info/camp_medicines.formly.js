import CampMedicineController from './camp_medicines.controller';

/* @ngInject */
export default function campsMedicinesFormlyConfig(formlyConfigProvider) {
  formlyConfigProvider.setType({
    name: 'campMedicines',
    templateUrl: 'medical_info/camp_medicines.html',
    controller: CampMedicineController
  });
}
