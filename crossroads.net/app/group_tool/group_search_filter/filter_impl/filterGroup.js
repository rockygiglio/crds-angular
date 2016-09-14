
export default class FilterGroup {
  constructor(filterName, filters) {
    this.filterName = filterName;
    this.filters = filters;
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
    for(let i = 0; i < this.filters.length; i++) {
      selected.push.apply(selected, this.filters[i].getSelectedValues());
    }
    return selected;
  }

  getCurrentFilters() {
    let current = [];
    for(let i = 0; i < this.filters.length; i++) {
      if(!this.filters[i].isActive()) {
        continue;
      }
      if(this.filters[i].constructor.name !== 'FilterGroup') {
        current.push.apply(current, [this.filters[i]]);
      } else {
        current.push.apply(current, this.filters[i].getCurrentFilters());
      }
    }
    return current;
  }

  matches(result) {
    for(let i = 0; i < this.filters.length; i++) {
      if(!this.filters[i].matches(result)) {
        return false;
      }
    }
    return true;
  }

  isActive() {
    for(let i = 0; i < this.filters.length; i++)
    {
      if(this.filters[i].isActive()) {
        return true;
      }
    }
    return false;
  }  

  clear() {
    for(let i = 0; i < this.filters.length; i++)
    {
      this.filters[i].clear();
    }
  }

  compareTo(other) {
    return this.getName().localeCompare(other.getName());
  }
}