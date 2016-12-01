import MedicalInfoController from './medical_info.controller';
import MedicalInfoTemplate from './medical_info.html';
import { getCampMedical }  from '../../camps.resolves';

const MedicalInfo = {
  bindings: {},
  template: MedicalInfoTemplate,
  controller: MedicalInfoController,
  resolve: [getCampMedical]
};

export default MedicalInfo;
