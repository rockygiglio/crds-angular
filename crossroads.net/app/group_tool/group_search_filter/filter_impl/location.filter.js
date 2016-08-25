
import {SearchFilter, SearchFilterValue} from './searchFilter'; 

export default class LocationFilter extends SearchFilter {
  constructor(filterName) {
    let filterValues = [
      new SearchFilterValue('In Person', true, false),
      new SearchFilterValue('Online', false, false)
    ];

    super(filterName, filterValues, this._matchingFunction);
  }

  _matchingFunction(result) {
    let selectedOnline = this.getSelectedValues();
    
    let filtered = selectedOnline.filter((v) => {
      return result.hasAddress() === v.getValue(); 
    });

    return filtered !== undefined && filtered.length > 0;
  }
}