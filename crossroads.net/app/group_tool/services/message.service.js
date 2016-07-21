import CONSTANTS from '../../constants';

export default class MessageService {
    /*@ngInject*/
    constructor($log, $resource, $q, AuthService, ImageService) {
        this.log = $log;
        this.resource = $resource;
        this.deferred = $q;
        this.auth = AuthService;
        this.imgService = ImageService;
    }

    sendGroupMessage(groupId, message) {
        return this.resource(__API_ENDPOINT__ + 'api/grouptool/:groupId/:groupTypeId/groupmessage').save({groupId: groupId,
            groupTypeId: CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS}, message).$promise;
    }
}