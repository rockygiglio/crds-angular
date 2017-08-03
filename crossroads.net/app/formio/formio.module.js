import { MODULES as modules } from '../constants';
import routes from './formio.routes';
import FormioService from './formio.service';
import formioComponent from './formio.component';

const {
  FORMIO: formio,
  CORE: core,
  COMMON: common
} = modules;

export default angular.module(formio, [core, common, 'formio'])
  .config(routes)
  .component('crdsFormio', formioComponent)
  .service('FormioService', FormioService)
  .name;
