import CONSTANTS from 'crds-constants';

export default class ServeTeamService {
    /*@ngInject*/
    constructor($log, $resource, $q) {
        this.log = $log;
        this.resource = $resource;
        this.qApi = $q;
    }

    getTeamRsvps(team)
    {
        return this.resource(__API_ENDPOINT__ +'api/serve/getTeamRsvps')
        .save(team).$promise;
    }
}