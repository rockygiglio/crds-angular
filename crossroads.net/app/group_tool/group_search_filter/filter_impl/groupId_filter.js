import CONSTANTS from 'crds-constants';
import {SearchFilter, SearchFilterValue} from './searchFilter'; 

export default class GroupIdFilter extends SearchFilter {
  constructor(filterName, selectedFilters) {
    
    if (selectedFilters == null || selectedFilters == undefined)
      selectedFilters="";
    let selectedArray = selectedFilters.split(',');
    let yesSelected = _.findIndex(selectedArray, (s) => { return s.toUpperCase() == 'YES'}) != -1;
    let noSelected = _.findIndex(selectedArray, (s) => { return s.toUpperCase() == 'NO'}) != -1

    let filterValues = [
      new SearchFilterValue('Yes', true, yesSelected),
      new SearchFilterValue('No', false, noSelected)
    ];

    super(filterName, filterValues, this._matchingFunction, CONSTANTS.GROUP.SEARCH_FILTERS_QUERY_PARAM_NAMES.GROUP_ID);
  }

  _matchingFunction(result) {
    // Guard against errors if group has no kids welcome flag set.  Shouldn't happen, but just in case...
    if(result.groupId === undefined) {
      return false;
    }

    let selectedGroupId = this.getSelectedValues();
    
    let filtered = selectedGroupId.filter((v) => {
      return result.groupId === v.getValue(); 
    });

    return filtered !== undefined && filtered.length > 0;
  }
}