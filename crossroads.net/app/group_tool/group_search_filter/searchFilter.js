
export default class SearchFilter {
  constructor(filterName, matchingFunction, resetFunction, activeFunction) {
    this.filterName = filterName;
    this.matchingFunction = matchingFunction;
    this.resetFunction = resetFunction;
    this.activeFunction = activeFunction;
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
    return this.activeFunction();
  }

  clear() {
    this.resetFunction();
  }

  compareTo(other) {
    return this.getName().localeCompare(other.getName());
  }
}

export class SearchFilterBuilder {
  constructor() {
    this.filterName = undefined;
    this.matchingFunction = () => { return false; };
    this.resetFunction = () => { return; };
    this.activeFunction = () => { return false; };
  }

  withFilterName(filterName) {
    this.filterName = filterName;
    return this;
  }

  withMatchingFunction(f) {
    this.matchingFunction = f;
    return this;
  }

  withResetFunction(f) {
    this.resetFunction = f;
    return this;
  }

  withActiveFunction(f) {
    this.activeFunction = f;
    return this;
  }

  getSearchFilter() {
    return new SearchFilter(this.filterName, this.matchingFunction, this.resetFunction, this.activeFunction);
  }
}