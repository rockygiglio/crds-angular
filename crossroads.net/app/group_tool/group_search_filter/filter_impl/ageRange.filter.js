
import {SearchFilter} from './searchFilter';

export default class AgeRangeFilter extends SearchFilter {
  constructor(filterName, filterValues) {
    super(filterName, filterValues, this._matchingFunction);
  }

  _matchingFunction(result) {
    // Guard against errors if group has no age ranges.  Shouldn't happen, but just in case...
    if(!result.ageRange || !Array.isArray(result.ageRange)) {
      return false;
    }
    
    let selectedAgeRanges = this.getSelectedValues();

    let filtered = result.ageRange.filter((a) => {
      return selectedAgeRanges.find((s) => { return s.getValue() === a.attributeId; }) !== undefined;
    });

    return filtered !== undefined && filtered.length > 0;
  }
}