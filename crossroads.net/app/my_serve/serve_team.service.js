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

    getCapacity(opp, eventId) {
          return this.resource(__API_ENDPOINT__ + 'api/serve/opp-capacity').get({
            id: opp.Opportunity_ID,
            eventId: eventId,
            min: opp.minimum,
            max: opp.maximum
          });
      }
}