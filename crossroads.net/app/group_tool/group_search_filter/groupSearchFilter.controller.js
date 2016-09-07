
import {SearchFilterValue} from './filter_impl/searchFilter';
import AgeRangeFilter from './filter_impl/ageRange.filter'; 
import KidsWelcomeFilter from './filter_impl/kidsWelcome.filter'; 
import LocationFilter from './filter_impl/location.filter'; 
import GroupTypeFilter from './filter_impl/groupType.filter'; 
import MeetingDayFilter from './filter_impl/meetingDay.filter'; 

export default class GroupSearchResultsController {
  /*@ngInject*/
  constructor(GroupService) {
    this.groupService = GroupService;
    this.ageRanges = [];
    this.groupTypes = [];
    this.days = [];
    this.expanded = false;
    this.allFilters = [];
  }

  $onInit() {
    this.initializeFilters();
  }

  $onChanges(allChanges) {
    this.searchResults = allChanges.searchResults.currentValue;
    this.applyFilters();
  }

  initializeFilters() {
    this.allFilters = [
      // TODO - When new filters are implemented, add them here - they will display in the order specified in this array
      new AgeRangeFilter('Age Range', this.ageRanges),
      new GroupTypeFilter('Group Type', this.groupTypes),
      new KidsWelcomeFilter('Kids Welcome'),
      new LocationFilter('Location'),
      new MeetingDayFilter('Day', this.days)
    ];

    this.loadAgeRanges();
    this.loadGroupTypes();
    this.loadDays();
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

  clearFilter(filter) {
    filter.clear();
    this.applyFilters();
  }

  getCurrentFilters() {
    return this.allFilters.filter((f) => f.isActive());
  }

  hasFilters() {
    return this.allFilters.find((f) => f.isActive()) !== undefined;
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

  loadAgeRanges() {
    this.groupService.getAgeRanges().then(
      (data) => {
        this.ageRanges.push.apply(this.ageRanges, data.attributes.map((a) => {
          return new SearchFilterValue(a.name, a.attributeId, false);
        }));
      },
      (err) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }
    ).finally(
      () => {
      });
  }

  loadGroupTypes() {
    this.groupService.getGroupGenderMixType().then(
      (data) => {
        this.groupTypes.push.apply(this.groupTypes, data.attributes.map((a) => {
          return new SearchFilterValue(a.name, a.attributeId, false);
        }));
      },
      (/*err*/) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }).finally(
        () => {
      });
  }  

  loadDays() {
    this.groupService.getDaysOfTheWeek().then(
      (data) => {
        data = _.sortBy( data, 'dp_RecordID' );
        data.push({dp_RecordID: 0, dp_RecordName: 'Flexible Meeting Time'});
        this.days.push.apply(this.days, data.map((a) => {
          return new SearchFilterValue(a.dp_RecordName, a.dp_RecordID, false);
        }));
      },
      (err) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }
    ).finally(
      () => {
      });
  }
}
