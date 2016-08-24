
export default class SearchFilter {
  constructor(filterName, filterValues, matchingFunction) {
    this.filterName = filterName;
    this.filterValues = filterValues;
    this.matchingFunction = matchingFunction;
  }

  getName() {
    return this.filterName;
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

export class SearchFilterBuilder {
  constructor() {
    this.filterName = undefined;
    this.filterValues = undefined;
    this.matchingFunction = () => { return false; };
  }

  withFilterName(filterName) {
    this.filterName = filterName;
    return this;
  }

  withFilterValues(filterValues) {
    this.filterValues = filterValues;
    return this;
  }

  withMatchingFunction(f) {
    this.matchingFunction = f;
    return this;
  }

  getSearchFilter() {
    return new SearchFilter(this.filterName, this.filterValues, this.matchingFunction);
  }
}