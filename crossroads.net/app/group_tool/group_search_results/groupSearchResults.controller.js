
export default class GroupSearchResultsController {
  /*@ngInject*/
  constructor(NgTableParams, GroupService, $state, $modal) {
    this.groupService = GroupService;
    this.$modal = $modal;

    this.search = null;
    this.processing = false;
    this.state = $state;
    this.ready = false;
    this.results = [];

    this.showLocationInput = false;
    this.searchedWithLocation = false;

    this.tableParams = new NgTableParams({}, {});
  }

  $onInit() {
    this.search = {
      query: this.state.params.query || 'tallent',
      location: this.state.params.location
    };
    this.doSearch(this.state.params.query || 'tallent', this.state.params.location);
  }

  doSearch(query, location) {
    this.showLocationInput = false;
    this.searchedWithLocation = location && location.length > 0;
    this.ready = false;
    this.results = [];
    this.groupService.search(query, location).then(
      (data) => {
        this.results = data;
      },
      (err) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }
    ).finally(
      () => {
        // TODO Need to figure out pagination, etc

        // This resets the ngTable count so we see all the results and sets sorting appropriately
        let parms = {
          count: this.results.length
        };
        parms.sorting = this.searchedWithLocation ? { proximity: 'asc' } : { meetingDay: 'asc' };

        // This resets the dataset so ngTable properly renders the new search results
        let settings = {
          dataset: this.results
        };
        this.tableParams.settings(settings);
        this.tableParams.parameters(parms);
        this.ready = true;
      }
    );
  }

  submit() {
    this.doSearch(this.search.query, this.search.location);
  }

  requestToJoin(group) {
    console.debug("Request to Join", group);
    var modalInstance = this.$modal.open({
      template: '<confirm-request group="confirmRequestModal.group" modal-instance="confirmRequestModal.modalInstance"></confirm-request>',
      controller: function(group, $modalInstance) {
        this.group = group;
        this.modalInstance = $modalInstance;
      },
      controllerAs: 'confirmRequestModal',
      size: 'lg',
      resolve: {
        group: function () {
          return group;
        }
      }
    });
  }
}
