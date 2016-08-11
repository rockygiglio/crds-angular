
export default class GroupSearchResultsController {
    /*@ngInject*/
    constructor(NgTableParams, GroupService, $state) {
        this.groupService = GroupService;

        //this.search = null;
        //this.processing = false;
        //this.state = $state;
        //this.ready = false;
        //this.results = [];
        //
        //this.showLocationInput = false;
        //this.searchedWithLocation = false;
        //
        //this.ageRangeFilter = null;
        //
        //this.tableParams = new NgTableParams({}, {});
        this.ageRanges = [];
    }

    $onInit() {


        this.loadAgeRanges();


        //this.search = {
        //    query: this.state.params.query,
        //    location: this.state.params.location
        //};
        //this.doSearch(this.state.params.query, this.state.params.location);
    }

    clickedButton() {
        var x = this.filterParams;
        var y = this.ageRanges;

        //this.filterParams.filter =

        debugger;
    }

    loadAgeRanges() {
        this.groupService.getAgeRanges().then(
            (data) => {
                this.ageRanges = data;

                for(var i = 0; i < this.ageRanges.attributes.length; i++)
                {
                    this.ageRanges.attributes[i].selected = false;
                }

                debugger;
            },
            (err) => {
                // TODO what happens on error? (could be 404/no results, or other error)
            }
        ).finally(
            () => {

        })
    }

    //doSearch(query, location) {
    //    this.showLocationInput = false;
    //    this.searchedWithLocation = location && location.length > 0;
    //    this.ready = false;
    //    this.results = [];
    //    this.groupService.search(query, location).then(
    //        (data) => {
    //            this.results = data;
    //        },
    //        (err) => {
    //            // TODO what happens on error? (could be 404/no results, or other error)
    //        }
    //    ).finally(
    //        () => {
    //            // TODO Need to figure out pagination, etc
    //
    //            // This resets the ngTable count so we see all the results and sets sorting appropriately
    //            let parms = {
    //                count: this.results.length
    //            };
    //            parms.sorting = this.searchedWithLocation ? { proximity: 'asc' } : { groupName: 'asc' };
    //
    //            // This resets the dataset so ngTable properly renders the new search results
    //            let settings = {
    //                dataset: this.results
    //            };
    //            this.tableParams.settings(settings);
    //            this.tableParams.parameters(parms);
    //            this.ready = true;
    //        }
    //    );
    //}
    //
    //submit() {
    //    this.doSearch(this.search.query, this.search.location);
    //}
    //
    //openMap(group) {
    //    console.log('Open Map');
    //}
}