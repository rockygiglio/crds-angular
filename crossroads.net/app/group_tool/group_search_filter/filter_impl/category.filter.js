
import {SearchFilter} from './searchFilter';

export default class CategoryFilter extends SearchFilter {
  constructor(filterName, filterValues) {
    super(filterName, filterValues, this._matchingFunction);
  }

  _matchingFunction(result) {
    // Guard against errors if group has no categories.  Shouldn't happen, but just in case...
    if(!result.categories || !Array.isArray(result.categories)) {
      return false;
    }
    
    let selectedCategories = this.getSelectedValues();

    let filtered = result.categories.filter((c) => {
      return selectedCategories.find((s) => { return s.getValue() === c.categoryId; }) !== undefined;
    });

    return filtered !== undefined && filtered.length > 0;   
  }
}