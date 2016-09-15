
import {SearchFilter, SearchFilterValue} from './searchFilter';

export default class AgeRangeFilter extends SearchFilter {
  constructor(filterName, groupService) {
    super(filterName, [], this._matchingFunction);

    groupService.getAgeRanges().then(
      (data) => {
        this.getValues().push.apply(this.getValues(), data.attributes.map((a) => {
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