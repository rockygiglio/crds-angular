export default class GroupURLService {
  /*@ngInject*/
  constructor($location) {
    this.location = $location;
  }

  shareUrl(groupId) {
    return $location.protocol() + '://' + $location.host() + '/trips/giving/' + groupId;
  }

  groupLeaderUrl(){
    return this.resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/group-leader/url-segment').get().$promise;
  }
}
