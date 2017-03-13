import CONSTANTS from '../../constants';
export default class GroupDetailService {
    /*@ngInject*/
    constructor($log) {
        this.log = $log;
    }

    //From groupDetail.participants.controller.js loadGroupParticipants()
    //participants array stored on this service

    canAccessParticipants(isLeader, groupTypeID) {
        let onsiteGroup = groupTypeID == CONSTANTS.GROUP.GROUP_TYPE_ID.ONSITE_GROUPS;
        return ((onsiteGroup && isLeader) || (!onsiteGroup));
    }

}