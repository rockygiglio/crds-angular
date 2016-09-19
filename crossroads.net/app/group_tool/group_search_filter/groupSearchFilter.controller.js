
import debounce from 'lodash/function/debounce';

import AgeRangeFilter from './filter_impl/ageRange.filter'; 
import CategoryFilter from './filter_impl/category.filter'; 
import KidsWelcomeFilter from './filter_impl/kidsWelcome.filter'; 
import LocationFilter from './filter_impl/location.filter'; 
import GroupTypeFilter from './filter_impl/groupType.filter'; 
import MeetingDayFilter from './filter_impl/meetingDay.filter';
import MeetingTimeFilter from './filter_impl/meetingTime.filter';
import FrequencyFilter from './filter_impl/frequency.filter'; 
import LeadersSiteFilter from './filter_impl/leadersSite.filter';
import FilterGroup from './filter_impl/filterGroup'; 

const APPLY_FILTER_DEBOUNCE = 750;

export default class GroupSearchResultsController {
  /*@ngInject*/
  constructor(GroupService, CreateGroupService) {
    this.groupService = GroupService;
    this.createGroupService = CreateGroupService;
    this.expanded = false;
    this.allFilters = undefined;
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
    this.allFilters = new FilterGroup('All Filters', [
      new AgeRangeFilter('Age Range', this.groupService),
      new CategoryFilter('Category', this.groupService),
      new GroupTypeFilter('Group Type', this.groupService),
      new KidsWelcomeFilter('Kids Welcome'),
      new LocationFilter('Location'),
      new FilterGroup('Day / Time / Frequency', [
        new MeetingDayFilter('Day', this.groupService),
        new MeetingTimeFilter('Time'),
        new FrequencyFilter('Frequency', this.createGroupService),
      ]),
      new LeadersSiteFilter('Leaders Site', this.groupService)
    ], true);
  }

  _internalApplyFilters() {
    let settings = {
      dataset: this.searchResults.filter((r) => {
        for (let i = 0; i < this.allFilters.getValues().length; i++) {
          if(!this.allFilters.getValues()[i].matches(r)) {
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
    this.allFilters.clear();
    this.applyFilters();
  }

  clearFilter(filter) {
    filter.clear();
    this.applyFilters();
  }

  getCurrentFilters() {
    return this.allFilters.getCurrentFilters();
  }

  hasFilters() {
    return this.allFilters.hasFilters();
  }

  openFilters() {
    this.expanded = true;
    this.expandedFilter = null;
  }

  openFilter(filter) {
    this.expanded = true;
    this.expandedFilter = this._getFilter(filter);
  }

  closeFilters() {
    this.expanded = false;
  }

  toggleFilter(filter) {
    if (this.expandedFilter === this._getFilter(filter)) {
      this.expandedFilter = null;
    } else {
      this.expandedFilter = this._getFilter(filter);
    }
  }

  isOpenFilter(filter) {
    return this.expandedFilter === this._getFilter(filter);
  }

  _getFilter(filter) {
    return filter.belongsToFilterGroup() ? filter.getFilterGroup() : filter;
  }
}
