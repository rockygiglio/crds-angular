
export default class GroupSearchResultsController {
  /*@ngInject*/
  constructor(NgTableParams, GroupService, $state) {
    this.groupService = GroupService;

    this.search = null;
    this.processing = false;
    this.state = $state;
    this.ready = false;
    this.results = [];

    this.showLocationInput = false;
    this.searchedWithLocation = false;

    this.tableParams = new NgTableParams({}, { dataset: this.results, sorting: { proximity: 'asc' }});
    //this.tableParams = new NgTableParams({}, { dataset: this.results });
  }

  $onInit() {
    this.search = {
      query: this.state.params.query,
      location: this.state.params.location
    };
    this.doSearch(this.state.params.query, this.state.params.location);
  }

  doSearch(query, location) {
    this.showLocationInput = false;
    this.searchedWithLocation = location && location.length > 0;
    this.ready = false;
    this.results.length = 0;
    this.groupService.search(query, location).then(
      (data) => {
        debugger;
        this.results.push(...data);
      },
      (err) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }
    ).finally(
      () => {
        // TODO Need to figure out pagination, etc
        // This resets the ngTable count so we see all the results
        this.tableParams.parameters({count: this.results.length});
        this.ready = true;
      }
    );
  }

  submit() {
    this.doSearch(this.search.query, this.search.location);
  }

  openMap(group) {
    console.log('Open Map');
  }
}