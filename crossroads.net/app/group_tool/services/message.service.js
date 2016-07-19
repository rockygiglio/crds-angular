import GroupInvitation from '../model/groupInvitation';
import CONSTANTS from '../../constants';
import SmallGroup from '../model/smallGroup';
import Participant from '../model/participant';
import GroupInquiry from '../model/groupInquiry';

export default class GroupService {
    /*@ngInject*/
    constructor($log, $resource, $q, AuthService, ImageService) {
        this.log = $log;
        this.resource = $resource;
        this.deferred = $q;
        this.auth = AuthService;
        this.imgService = ImageService;
    }

    sendGroupInvitation(message) {
        // can pass params here, if needed
        return this.resource(__API_ENDPOINT__ + 'api/invitation').save(message).$promise;
    }
}