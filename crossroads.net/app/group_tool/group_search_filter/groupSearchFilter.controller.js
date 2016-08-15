
export default class GroupSearchResultsController {
  /*@ngInject*/
  constructor(GroupService) {
    this.groupService = GroupService;
    this.ageRanges = [];

    this.showFilters = true;
  }

  $onInit() {
    this.loadAgeRanges();
  }

  $onChanges(allChanges) {
    this.searchResults = allChanges.searchResults.currentValue;
    this.applyFilters();
  }

  applyFilters() {
    let settings = {
      dataset: this.searchResults.filter((r) => {
        // TODO When additional filters are added, call their functions here
        return this.ageRangeFilter(r);
      })
    };
    angular.extend(this.tableParams.settings(), settings);
    this.tableParams.reload();
  }

  // TODO - This is probably not very efficient, might need to optimize with large result sets
  ageRangeFilter(searchResult) {    
    let selectedAgeRanges = this.ageRanges.filter((a) => {
      return a.selected === true;
    }).map((a) => {
      return a.attributeId;
    });

    if(selectedAgeRanges.length === 0) {
      return true;
    }

    let filteredResults = searchResult.ageRange.filter((a) => {
      return selectedAgeRanges.find((s) => { return s === a.attributeId; }) !== undefined;
    });

    return filteredResults !== undefined && filteredResults.length > 0;
  }

  loadAgeRanges() {
    this.groupService.getAgeRanges().then(
      (data) => {
        this.ageRanges = data.attributes;

        for(let i = 0; i < this.ageRanges.length; i++)
        {
          this.ageRanges[i].selected = false;
        }
      },
      (err) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }
    ).finally(
      () => {
    });
  }
}