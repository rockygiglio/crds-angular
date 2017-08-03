import FormioController from './formio.controller';
import formioTemplate from './formio.html';

const formioComponent = {
  bindings: {},
  template: formioTemplate,
  controller: FormioController,
  controllerAs: 'ctrl'
};

export default formioComponent;
