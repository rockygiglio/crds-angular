import CONSTANTS from '../../constants';
import SmallGroup from '../model/smallGroup';

export default class CreateGroupService {
  /*@ngInject*/
  constructor($log, $resource, $q, AuthService, ImageService) {
    this.log = $log;
    this.resource = $resource;
    this.deferred = $q;
    this.auth = AuthService;
    this.imgService = ImageService;

  }



  


}
