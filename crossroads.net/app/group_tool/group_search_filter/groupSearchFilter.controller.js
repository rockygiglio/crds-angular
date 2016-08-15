
export default class GroupSearchResultsController {
  /*@ngInject*/
  constructor(GroupService) {
    this.groupService = GroupService;
    this.ageRanges = [];
    this.expanded = false;
    this.currentFilters = {};
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

  clearFilters() {
    // TODO When additional filters are added, call their clear functions here
    this.clearAgeRangeFilter();

    this.applyFilters();
  }

  openFilters() {
    this.expanded = true;
  }

  closeFilters() {
    this.expanded = false;
  }

  hasFilters() {
    return Object.keys(this.currentFilters).length > 0;
  }

  // TODO - This is probably not very efficient, might need to optimize with large result sets
  ageRangeFilter(searchResult) {
    delete this.currentFilters['Age Range'];
    let selectedAgeRanges = this.ageRanges.filter((a) => {
      return a.selected === true;
    }).map((a) => {
      return a.attributeId;
    });

    if(selectedAgeRanges.length === 0) {
      return true;
    }

    this.currentFilters['Age Range'] = () => {
      this.clearAgeRangeFilter();
      this.applyFilters();
    };

    // Guard against errors if group has no age ranges.  Shouldn't happen, but just in case...
    if(!searchResult.ageRange || !Array.isArray(searchResult.ageRange)) {
      return false;
    }
    
    let filteredResults = searchResult.ageRange.filter((a) => {
      return selectedAgeRanges.find((s) => { return s === a.attributeId; }) !== undefined;
    });

    return filteredResults !== undefined && filteredResults.length > 0;
  }

  clearAgeRangeFilter() {
    for(let i = 0; i < this.ageRanges.length; i++)
    {
      this.ageRanges[i].selected = false;
    }
    delete this.currentFilters['Age Range'];
  }

  loadAgeRanges() {
    this.groupService.getAgeRanges().then(
      (data) => {
        this.ageRanges = data.attributes;

        this.clearAgeRangeFilter();
      },
      (err) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }
    ).finally(
      () => {
      });
  }
}