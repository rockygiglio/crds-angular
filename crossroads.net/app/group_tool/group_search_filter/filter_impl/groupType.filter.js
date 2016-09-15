
import {SearchFilter, SearchFilterValue} from './searchFilter';

export default class GroupTypeFilter extends SearchFilter {
  constructor(filterName, groupService) {
    super(filterName, [], this._matchingFunction);
    groupService.getGroupGenderMixType().then(
      (data) => {
        this.getValues().push.apply(this.getValues(), data.attributes.map((a) => {
          return new SearchFilterValue(a.name, a.attributeId, false);
        }));
      },
      (/*err*/) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }).finally(
        () => {
      });    
  }

  _matchingFunction(result) {
    // Guard against errors if group has no group type.  Shouldn't happen, but just in case...
    if(!result.groupType) {
      return false;
    }
    
    let filtered = this.getSelectedValues().find((v) => {
      return v.getValue() === result.groupType.attributeId; 
    });

    return filtered !== undefined;    
  }
}