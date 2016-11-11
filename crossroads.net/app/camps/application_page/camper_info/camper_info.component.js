import CamperInfoController from './camper_info.controller';
import InfoTemplate from './camper_info.html';
import { getCamperInfo, getCampInfo } from '../../camps.resolves';

const CamperInfo = {
  bindings: {},
  template: InfoTemplate,
  controller: CamperInfoController,
  controllerAs: 'camperInfo',
  resolve: [
    getCamperInfo,
    getCampInfo
  ]
};

export default CamperInfo;
