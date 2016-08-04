
import constants from 'crds-constants';
import GroupSearchResultsController from '../../../app/group_tool/group_search_results/groupSearchResults.controller';

describe('GroupSearchResultsController', () => {
  let fixture,
    ngTableParams,
    groupService,
    state,
    qApi,
    rootScope;

  beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

  beforeEach(inject(function ($injector) {
    ngTableParams = $injector.get('NgTableParams');
    groupService = {
      search: function() {}
    };
    state = $injector.get('$state');
    state.params = {
      query: 'kw1 kw2 kw3',
      location: 'oakley'
    };
    qApi = $injector.get('$q');
    rootScope = $injector.get('$rootScope');
    fixture = new GroupSearchResultsController(ngTableParams, groupService, state);
  }));

  describe('the constructor', () => {
    it('should initialize properties', () => {
      expect(fixture.search).toBe(null);
      expect(fixture.processing).toBeFalsy();
      expect(fixture.ready).toBeFalsy();
      expect(fixture.results).toEqual([]);
      expect(fixture.showLocationInput).toBeFalsy();
      expect(fixture.searchedWithLocation).toBeFalsy();
      expect(fixture.tableParams instanceof ngTableParams).toBeTruthy();
      expect(fixture.tableParams.settings().dataset).toBe(null);
    });
  });

  describe('$onInit() function', () => {
    it('should set search model and call doSearch', () => {
      spyOn(fixture, 'doSearch').and.callFake(() => {});
      fixture.$onInit();
      expect(fixture.search.query).toEqual(state.params.query);
      expect(fixture.search.location).toEqual(state.params.location);
      expect(fixture.doSearch).toHaveBeenCalledWith(state.params.query, state.params.location);
    });
  });

  describe('submit() function', () => {
    it('should call doSearch with search model', () => {
      spyOn(fixture, 'doSearch').and.callFake(() => {});
      fixture.search = {
        query: '123',
        location: '456'
      };
      fixture.submit();
      expect(fixture.doSearch).toHaveBeenCalledWith('123', '456');
    });
  });

  describe('doSearch() function', () => {
    it('should replace the results on successful service call', () => {
      let originalResults = fixture.results;
      fixture.results.length = 0;
      fixture.results.push({groupName: 'name'});
      fixture.showLocationInput = true;
      fixture.ready = false;
      fixture.tableParams.parameters().count = 1;

      let groups = [{groupName: 'group1'},{groupName: 'group2'}];
      let deferred = qApi.defer();
      deferred.resolve(groups);
      spyOn(groupService, 'search').and.callFake(() => {
        return deferred.promise;
      });
      fixture.doSearch('123', '456');
      rootScope.$apply();

      expect(groupService.search).toHaveBeenCalledWith('123', '456');
      expect(fixture.showLocationInput).toBeFalsy();
      expect(fixture.searchedWithLocation).toBeTruthy();
      expect(fixture.ready).toBeTruthy();
      expect(fixture.results).not.toBe(originalResults);
      expect(fixture.results.length).toEqual(groups.length);
      expect(fixture.tableParams.settings().dataset).toBe(fixture.results);
      expect(fixture.tableParams.parameters().count).toEqual(groups.length);
      expect(fixture.tableParams.parameters().sorting.proximity).toEqual('asc');
    });

    it('should reset results if error calling service', () => {
      let originalResults = fixture.results;
      fixture.results.length = 0;
      fixture.results.push({groupName: 'name'});
      fixture.showLocationInput = true;
      fixture.ready = false;
      fixture.tableParams.parameters().count = 1;

      let deferred = qApi.defer();
      deferred.reject({});
      spyOn(groupService, 'search').and.callFake(() => {
        return deferred.promise;
      });
      fixture.doSearch('123', '');
      rootScope.$apply();

      expect(groupService.search).toHaveBeenCalledWith('123', '');
      expect(fixture.showLocationInput).toBeFalsy();
      expect(fixture.searchedWithLocation).toBeFalsy();
      expect(fixture.ready).toBeTruthy();
      expect(fixture.results).not.toBe(originalResults);
      expect(fixture.results.length).toEqual(0);
      expect(fixture.tableParams.settings().dataset).toBe(fixture.results);
      expect(fixture.tableParams.parameters().count).toEqual(0);
      expect(fixture.tableParams.parameters().sorting.groupName).toEqual('asc');
    });
  });
});