
import constants from 'crds-constants';
import GroupSearchFilter from '../../../app/group_tool/group_search_filter/groupSearchFilter.controller';

describe('GroupSearchFilter', () => {
  let fixture, groupService;

  beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

  beforeEach(inject(function (/* $injector */) {
    groupService = {
      getAgeRanges: function () { }
    };
    fixture = new GroupSearchFilter(groupService);
  }));

  describe('the constructor', () => {
    it('should initialize properties', () => {
      expect(fixture.ageRanges).toEqual([]);
      expect(fixture.expanded).toBeFalsy();
      expect(fixture.currentFilters).toEqual({});
    });
  });

  describe('$onInit function', () => {
    it('should load age ranges', () => {
      spyOn(fixture, 'loadAgeRanges').and.callFake(() => {});
      fixture.$onInit();
      expect(fixture.loadAgeRanges).toHaveBeenCalled();
    });
  });

  describe('$onChanges function', () => {
    it('should reset search results and re-apply filters', () => {
      let changes = {
        searchResults: {
          currentValue: {
            p1: '1',
            p2: '2'
          }
        }
      };
      spyOn(fixture, 'applyFilters').and.callFake(() => {});
      fixture.$onChanges(changes);
      expect(fixture.applyFilters).toHaveBeenCalled();
      expect(fixture.searchResults).toBe(changes.searchResults.currentValue);
    });
  });

  describe('applyFilters function', () => {
    it('should apply filters and reload ngTable', () => {
      fixture.searchResults = [
        { 'age': 1 },
        { 'age': 2 },
        { 'age': 3 },
      ];

      let settings = {
            dataset: [{ 'age': 5 }],
            someOtherSettingThatShouldNotChange: true
      }; 

      fixture.tableParams = {
        settings: function() {
          return settings;
        },
        reload: function() {}
      };

      spyOn(fixture, 'ageRangeFilter').and.callFake((r) => {
        return r.age === 2;
      });

      spyOn(fixture.tableParams, 'reload').and.callFake(() => {});

      fixture.applyFilters();

      expect(fixture.ageRangeFilter.calls.count()).toEqual(fixture.searchResults.length);
      for(let i = 0; i < fixture.searchResults.length; i++) {
        expect(fixture.ageRangeFilter).toHaveBeenCalledWith(fixture.searchResults[i]);
      }
      expect(fixture.tableParams.reload).toHaveBeenCalled();
      expect(fixture.tableParams.settings().dataset).toEqual([{ 'age': 2 }]);
      expect(fixture.tableParams.settings().someOtherSettingThatShouldNotChange).toBeTruthy();
    });
  });

  describe('filter manipulation function', () => {
    it('clearFilters should clear and re-apply all filters', () => {
      spyOn(fixture, 'clearAgeRangeFilter').and.callFake(() => {});
      spyOn(fixture, 'applyFilters').and.callFake(() => {});

      fixture.clearFilters();
      expect(fixture.clearAgeRangeFilter).toHaveBeenCalled();
      expect(fixture.applyFilters).toHaveBeenCalled();
    });

    it('openFilters should set expanded to true', () => {
      fixture.expanded = false;
      fixture.openFilters();

      expect(fixture.expanded).toBeTruthy();
    });
    
    it('closeFilters should set expanded to false', () => {
      fixture.expanded = true;
      let form = {
        $rollbackViewValue: jasmine.createSpy('$rollbackViewValue')
      };
      fixture.closeFilters(form);

      expect(form.$rollbackViewValue).toHaveBeenCalled();
      expect(fixture.expanded).toBeFalsy();
    });

    it('hasFilters should return false if no current filters', () => {
      fixture.currentFilters = {};

      expect(fixture.hasFilters()).toBeFalsy();
    });    

    it('hasFilters should return true if one or more current filters', () => {
      fixture.currentFilters = { 'Age Range': function() {}};

      expect(fixture.hasFilters()).toBeTruthy();
    });    
  });

  describe('age range filter function', () => {
    describe('ageRangeFilter', () => {
      beforeEach(() => {
        fixture.currentFilters['Age Range'] = function() {};
      });

      it('should return true if no age range currently filtered', () => {
        fixture.ageRanges = [
          { selected: false}
        ];
        expect(fixture.ageRangeFilter({})).toBeTruthy();
        expect(fixture.currentFilters['Age Range']).not.toBeDefined();
      });

      it('should return true if the search result contains a filtered age range', () => {
        fixture.ageRanges = [
          { selected: false, attributeId: 123 },
          { selected: true, attributeId: 456 }
        ];
        let searchResult = {
          ageRange: [
            { attributeId: 456 },
            { attributeId: 789 }
          ]
        };
        expect(fixture.ageRangeFilter(searchResult)).toBeTruthy();

        // Make sure the currentFilters['Age Range'] is defined and calls the right functions
        expect(fixture.currentFilters['Age Range']).toBeDefined();
        spyOn(fixture, 'clearAgeRangeFilter').and.callFake(() => {});
        spyOn(fixture, 'applyFilters').and.callFake(() => {});
        let clearAgeFilter = fixture.currentFilters['Age Range'];
        clearAgeFilter();
        expect(fixture.clearAgeRangeFilter).toHaveBeenCalled();
        expect(fixture.applyFilters).toHaveBeenCalled();
      });

      it('should return false if the search result does not contain a filtered age range', () => {
        fixture.ageRanges = [
          { selected: false, attributeId: 123 },
          { selected: true, attributeId: 456 }
        ];
        let searchResult = {
          ageRange: [
            { attributeId: 234 },
            { attributeId: 567 }
          ]
        };
        expect(fixture.ageRangeFilter(searchResult)).toBeFalsy();
        expect(fixture.currentFilters['Age Range']).toBeDefined();
      });

      it('should return false if the search result does not contain an age range', () => {
        fixture.ageRanges = [
          { selected: false, attributeId: 123 },
          { selected: true, attributeId: 456 }
        ];
        let searchResult = { };
        expect(fixture.ageRangeFilter(searchResult)).toBeFalsy();
        expect(fixture.currentFilters['Age Range']).toBeDefined();
      });
    });

    describe('clearAgeRangeFilter', () => {
      it('should unselect age ranges and remove currentFilter', () => {
        fixture.currentFilters['Age Range'] = function() {};
        fixture.ageRanges = [
          { selected: true, attributeId: 123 },
          { selected: true, attributeId: 456 }
        ];

        fixture.clearAgeRangeFilter();
        expect(fixture.currentFilters['Age Range']).not.toBeDefined();
        expect(fixture.ageRanges[0].selected).toBeFalsy();
        expect(fixture.ageRanges[1].selected).toBeFalsy();
      });
    });
  });
});