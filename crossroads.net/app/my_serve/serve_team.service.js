<<<<<<< HEAD
import CONSTANTS from '../constants';

export default class ServeTeamService {
    /*@ngInject*/
    constructor($log, $resource, $q) {
        this.log = $log;
        this.resource = $resource;
        this.qApi = $q;
    }

    getAllTeamMembersByLeader() {
        return [
            {
                id: 1001,
                name: 'Genie Simmons',
                email: 'gsimmons@gmail.com',
                role: 'Leader'
            },
            {
                id: 1002,
                name: 'Holly Gennaro',
                email: 'hgennaro@excite.com',
                role: null
            },
        ]
    }

    getTeamDetailsByLeader() {
        return this.resource(`${__API_ENDPOINT__}api/serve/GetLoggedInLeadersGroups`).query().$promise;
    }

//TODO: THIS METHOD IS BASICALLY THE SAME METHOD IN GROUP TOUL SERVICES/MESSAGE SERVICE.  THAT SERVICE SHOULD BE REFACTORED
//AND PULLED UP TO A HIGHER LEVEL TO BE USED MORE BROADLY
    sendGroupMessage(groupId, message) {
        debugger;
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