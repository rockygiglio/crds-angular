import CONSTANTS from '../constants';

export default class ServeTeamService {
    /*@ngInject*/
    constructor($log, $resource, $q) {
        this.log = $log;
        this.resource = $resource;
        this.qApi = $q;
    }

    getAllTeamMembersForLoggedInLeader() {
        return this.resource(`${__API_ENDPOINT__}api/serve/GetLoggedInLeadersGroupsParticipants`).query().$promise;
    }

    getIsLeader() {
        return this.resource(`${__API_ENDPOINT__}api/serve/GetIsLeader`).get().$promise;
    }

    getTeamDetailsByLeader() {
        return this.resource(`${__API_ENDPOINT__}api/serve/GetLoggedInLeadersGroups`).query().$promise;
    }

//TODO: THIS METHOD IS BASICALLY THE SAME METHOD IN GROUP TOUL SERVICES/MESSAGE SERVICE.  THAT SERVICE SHOULD BE REFACTORED
//AND PULLED UP TO A HIGHER LEVEL TO BE USED MORE BROADLY
    sendGroupMessage(groupId, message) {
        return this.resource(__API_ENDPOINT__ + 'api/grouptool/:groupId/:groupTypeId/groupmessage').save({
            groupId: groupId,
            groupTypeId: CONSTANTS.GROUP.GROUP_TYPE_ID.MY_SERVE
        }, message).$promise;
    }

    getTeamRsvps(team) {
        return this.resource(__API_ENDPOINT__ +'api/serve/getTeamRsvps')
        .save(team).$promise;
    }
}