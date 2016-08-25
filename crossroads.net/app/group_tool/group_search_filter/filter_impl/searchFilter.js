
export class SearchFilter {
  // Don't instantiate this class directly, subclass it for a specific filter
  constructor(filterName, filterValues, matchingFunction) {
    this.filterName = filterName;
    this.filterValues = filterValues;
    this.matchingFunction = matchingFunction;
  }

  getName() {
    return this.filterName;
  }

  getFilterName() {
    return _.camelCase(this.getName());
  }

  getSelectedValues() {
    return this.filterValues.filter((i) => { return i.selected === true; })
  }

  getValues() {
    return this.filterValues;
  }

  matches(result) {
    if(!this.isActive()) {
      return true;
    }
    return this.matchingFunction(result);
  }

  isActive() {
    return this.filterValues.find((i) => { return i.selected === true; }) !== undefined;
  }

  clear() {
    for(let i = 0; i < this.filterValues.length; i++)
    {
      this.filterValues[i].selected = false;
    }
  }

  compareTo(other) {
    return this.getName().localeCompare(other.getName());
  }
}

export class SearchFilterValue {
  constructor(name, value, selected) {
    this.name = name;
    this.value = value;
    this.selected = selected;
  }

  getName() {
    return this.name;
  }

  getValue() {
    return this.value;
  }

  isSelected() {
    return this.selected !== undefined && this.selected === true;
  }
}