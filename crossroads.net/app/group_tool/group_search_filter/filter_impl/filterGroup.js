
export default class FilterGroup {
  constructor(filterName, filters, topLevel) {
    this.filterName = filterName;
    this.filters = filters;

    // Only set parent group if this is not the top-level group
    if(topLevel !== true) {
      this.filters.forEach((f) => {
        f.setFilterGroup(this);
      });
    }
  }

  getName() {
    return this.filterName;
  }

  getFilterName() {
    return _.camelCase(this.getName());
  }

  getValues() {
    return this.filters;
  }

  getSelectedValues() {
    let selected = [];
    this.filters.forEach((f) => { selected.push.apply(selected, f.getSelectedValues()); });
    return selected;
  }

  getCurrentFilters() {
    let current = [];
    this.filters.forEach((f) => {
      if(f.isActive()) {
        if(f.constructor.name !== 'FilterGroup') {
          current.push(f);
        } else {
          current.push.apply(current, f.getCurrentFilters());
        }
      }
    });
    return current;
  }

  matches(result) {
    // A filter group matches a result only if all contained filters match the result
    return this.filters.find((f) => { return f.matches(result) === false; }) === undefined;
  }

  hasFilters() {
    return this.isActive();
  }

  isActive() {
    // A filter group is active if any of the contained filters are active
    return this.filters.find((f) => { return f.isActive() === true; }) !== undefined;
  }  

  clear() {
    this.filters.forEach((f) => { f.clear(); });
  }

  belongsToFilterGroup() {
    return this.filterGroup !== undefined;
  }

  setFilterGroup(filterGroup) {
    this.filterGroup = filterGroup;
  }

  getFilterGroup() {
    return this.filterGroup;
  }

  compareTo(other) {
    return this.getName().localeCompare(other.getName());
  }
}