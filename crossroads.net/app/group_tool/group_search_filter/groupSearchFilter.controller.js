import debounce from 'lodash/function/debounce';

import {SearchFilterValue} from './filter_impl/searchFilter';
import AgeRangeFilter from './filter_impl/ageRange.filter'; 
import CategoryFilter from './filter_impl/category.filter'; 
import KidsWelcomeFilter from './filter_impl/kidsWelcome.filter'; 
import LocationFilter from './filter_impl/location.filter'; 
import GroupTypeFilter from './filter_impl/groupType.filter'; 
import MeetingDayFilter from './filter_impl/meetingDay.filter';
import MeetingTimeFilter from './filter_impl/meetingTime.filter';
import FrequencyFilter from './filter_impl/frequency.filter'; 
import LeadersSiteFilter from './filter_impl/leadersSite.filter'; 

const APPLY_FILTER_DEBOUNCE = 750;

export default class GroupSearchResultsController {
  /*@ngInject*/
  constructor(GroupService, CreateGroupService) {
    this.groupService = GroupService;
    this.createGroupService = CreateGroupService;
    this.ageRanges = [];
    this.groupTypes = [];
    this.days = [];
    this.categories = [];
    this.frequencies = [];
    this.leadersSite = [];
    this.expanded = false;
    this.allFilters = [];
    this.expandedFilter = null;

    this.applyFilters = debounce(this._internalApplyFilters, APPLY_FILTER_DEBOUNCE);
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
      new CategoryFilter('Category', this.categories),
      new GroupTypeFilter('Group Type', this.groupTypes),
      new KidsWelcomeFilter('Kids Welcome'),
      new LocationFilter('Location'),
      new MeetingDayFilter('Day', this.days),
      new MeetingTimeFilter('Time'),
      new FrequencyFilter('Frequency', this.frequencies),
      new LeadersSiteFilter('Leaders Site', this.leadersSite)
    ];

    this.loadAgeRanges();
    this.loadGroupTypes();
    this.loadDays();
    this.loadCategories();
    this.loadFrequencies();
    this.loadLeadersSite();
  }

  _internalApplyFilters() {
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
    this.expandedFilter = null;
  }

  openFilter(filter) {
    this.expanded = true;
    this.expandedFilter = filter;
  }

  closeFilters() {
    this.expanded = false;
  }

  toggleFilter(filter) {
    if (this.expandedFilter === filter) {
      this.expandedFilter = null;
    } else {
      this.expandedFilter = filter;
    }
  }

  isOpenFilter(filter) {
    return this.expandedFilter === filter;
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

  loadCategories() {
    this.groupService.getGroupCategories().then(
      (data) => {
        this.categories.push.apply(this.categories, data.map((c) => {
          return new SearchFilterValue(c.label, c.categoryId, false, c.labelDesc);
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

  loadFrequencies() {
    let frequencies = this.createGroupService.getMeetingFrequencies();
    frequencies = _.sortBy( frequencies, 'meetingFrequencyId' );

    this.frequencies.push.apply(this.frequencies, frequencies.map((a) => {
      return new SearchFilterValue(a.meetingFrequencyDesc, a.meetingFrequencyId, false);
    }));
  }

  loadLeadersSite() {
    this.groupService.getSites().then(
      (data) => {
        data = _.sortBy( data, 'dp_RecordID' );
        this.leadersSite.push.apply(this.leadersSite, data.map((a) => {
          return new SearchFilterValue(a.dp_RecordName, a.dp_RecordID, false);
        }));
      },
      (/*err*/) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }).finally(
        () => {
      });
  }
}
