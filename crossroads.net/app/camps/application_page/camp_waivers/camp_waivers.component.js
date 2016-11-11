import CampWaiversController from './camp_waivers.controller';
import template from './camp_waivers.html';
import getCampWaivers from './camp_waivers.resolve';

const CampWaiversComponent = {
  bindings: {},
  template,
  controller: CampWaiversController,
  controllerAs: 'campWaivers',
  resolve: [getCampWaivers]
};

export default CampWaiversComponent;
