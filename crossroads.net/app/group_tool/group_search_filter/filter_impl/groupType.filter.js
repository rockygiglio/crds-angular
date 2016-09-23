
import {SearchFilter} from './searchFilter';

export default class GroupTypeFilter extends SearchFilter {
  constructor(filterName, filterValues) {
    super(filterName, filterValues, this._matchingFunction);
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