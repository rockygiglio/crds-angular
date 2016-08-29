
import {SearchFilter, SearchFilterValue} from './searchFilter'; 

export default class KidsWelcomeFilter extends SearchFilter {
  constructor(filterName) {
    let filterValues = [
      new SearchFilterValue('Yes', true, false),
      new SearchFilterValue('No', false, false)
    ];

    super(filterName, filterValues, this._matchingFunction);
  }

  _matchingFunction(result) {
    // Guard against errors if group has no kids welcome flag set.  Shouldn't happen, but just in case...
    if(result.kidsWelcome === undefined) {
      return false;
    }

    let selectedKidsWelcome = this.getSelectedValues();
    
    let filtered = selectedKidsWelcome.filter((v) => {
      return result.kidsWelcome === v.getValue(); 
    });

    return filtered !== undefined && filtered.length > 0;
  }
}