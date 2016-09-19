
import {SearchFilter, SearchFilterValue} from './searchFilter';

export default class LeadersSiteFilter extends SearchFilter {
  constructor(filterName, groupService) {
    super(filterName, [], this._matchingFunction);

    groupService.getSites().then(
      (data) => {
        data = _.sortBy( data, 'dp_RecordID' );
        this.getValues().push.apply(this.getValues(), data.map((a) => {
          return new SearchFilterValue(a.dp_RecordName, a.dp_RecordID, false);
        }));
      },
      (/*err*/) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }).finally(
        () => {
      });    
  }

  _matchingFunction(result) {
    // Guard against errors if group has no age ranges.  Shouldn't happen, but just in case...
    if(!result.leaders() || !Array.isArray(result.leaders())) {
      return false;
    }

    let selectedSites = this.getSelectedValues();

    let filtered = result.leaders().filter((a) => {
      return selectedSites.find((s) => { return s.getName() === a.congregation; }) !== undefined;
    });

    return filtered !== undefined && filtered.length > 0;
  }
}
