
import {SearchFilter, SearchFilterValue} from './searchFilter';

export default class CategoryFilter extends SearchFilter {
  constructor(filterName, groupService) {
    super(filterName, [], this._matchingFunction);

    groupService.getGroupCategories().then(
      (data) => {
        this.getValues().push.apply(this.getValues(), data.map((c) => {
          return new SearchFilterValue(c.label, c.categoryId, false, c.labelDesc);
        }));
      },
      (/*err*/) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }).finally(
        () => {
      });
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