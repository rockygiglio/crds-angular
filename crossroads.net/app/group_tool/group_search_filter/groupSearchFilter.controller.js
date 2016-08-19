
import {SearchFilter, SearchFilterBuilder} from './searchFilter'; 

export default class GroupSearchResultsController {
  /*@ngInject*/
  constructor(GroupService) {
    this.groupService = GroupService;
    this.ageRanges = [];
    this.kidsWelcome = undefined;
    this.expanded = false;
    this.allFilters = [];
  }

  $onInit() {
    this.allFilters = [
      // TODO - When new filters are implemented, add them here
      this.buildAgeRangeFilter(),
      this.buildKidsWelcomeFilter()
    ];

    this.loadAgeRanges();
  }

  $onChanges(allChanges) {
    this.searchResults = allChanges.searchResults.currentValue;
    this.applyFilters();
  }

  applyFilters() {
    let settings = {
      dataset: this.searchResults.filter((r) => {
        for (let i = 0; i < this.allFilters.length; i++) {
          if(!this.allFilters[i].matches(r)) {
            return false;
          }
        }
        return true;
      })
    };

    this.expanded = false;
    angular.extend(this.tableParams.settings(), settings);
    this.tableParams.reload();
  }

  clearFilters() {
    this.allFilters.forEach(function(f) {
      f.clear();
    }, this);

    this.applyFilters();
  }

  openFilters() {
    this.expanded = true;
  }

  closeFilters(filterForm) {
    // Reset all filters that are not in sync with the model. This handles the case 
    // where someone changes filter values but does not click "Update Filters". 
    filterForm.$rollbackViewValue();

    this.expanded = false;
  }

  clearFilter(filter) {
    filter.clear();
    this.applyFilters();
  }

  clearFilterByName(filterName) {
    let filter = this.allFilters.find((f) => { return f.getName() === filterName; });
    if(filter === undefined) {
      return;
    }
    this.clearFilter(filter);
  }

  getCurrentFilters() {
    return this.allFilters.filter((f) => f.isActive());
  }

  hasFilters() {
    return this.allFilters.find((f) => f.isActive()) !== undefined;
  }

  buildAgeRangeFilter() {
    return new SearchFilterBuilder()
      .withFilterName('Age Range')
      .withMatchingFunction((result) => {
        let selectedAgeRanges = this.ageRanges.filter((a) => {
          return a.selected === true;
        }).map((a) => {
          return a.attributeId;
        });

        if(selectedAgeRanges.length === 0) {
          return true;
        }

        // Guard against errors if group has no age ranges.  Shouldn't happen, but just in case...
        if(!result.ageRange || !Array.isArray(result.ageRange)) {
          return false;
        }
        
        let filteredResults = result.ageRange.filter((a) => {
          return selectedAgeRanges.find((s) => { return s === a.attributeId; }) !== undefined;
        });

        return filteredResults !== undefined && filteredResults.length > 0;
      })
      .withResetFunction(() => {
        for(let i = 0; i < this.ageRanges.length; i++)
        {
          this.ageRanges[i].selected = false;
        }
      })
      .withActiveFunction(() => {
        return this.ageRanges.find((i) => { return i.selected === true; }) !== undefined;
      })
      .getSearchFilter();
  }

  buildKidsWelcomeFilter() {
    return new SearchFilterBuilder()
      .withFilterName('Kids Welcome')
      .withMatchingFunction((result) => {
        return result.kidsWelcome !== undefined && result.kidsWelcome === this.kidsWelcome;
      })
      .withResetFunction(() => {
        this.kidsWelcome = undefined;
      })
      .withActiveFunction(() => {
        return this.kidsWelcome !== undefined;
      })
      .getSearchFilter();
  }

  loadAgeRanges() {
    this.groupService.getAgeRanges().then(
      (data) => {
        this.ageRanges = data.attributes;
        this.clearFilterByName('Age Range');
      },
      (err) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }
    ).finally(
      () => {
      });
  }
}