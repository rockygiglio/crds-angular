
import {SearchFilter} from './searchFilter';

export default class LeadersSiteFilter extends SearchFilter {
  constructor(filterName, filterValues) {
    super(filterName, filterValues, this._matchingFunction);
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
