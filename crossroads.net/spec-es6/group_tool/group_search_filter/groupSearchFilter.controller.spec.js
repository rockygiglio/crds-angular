
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
      expect(fixture.kidsWelcome).toEqual([]);
      expect(fixture.expanded).toBeFalsy();
      expect(fixture.allFilters).toEqual([]);
    });
  });

  describe('$onInit function', () => {
    it('should load age ranges', () => {
      spyOn(fixture, 'initializeFilters').and.callFake(() => {});
      spyOn(fixture, 'loadAgeRanges').and.callFake(() => {});
      fixture.$onInit();
      expect(fixture.initializeFilters).toHaveBeenCalled();
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

  describe('initializeFilters function', () => {
    it('should initialize all filters', () => {
      let ageRangeFilter = { age: 1 };
      let kidsWelcomeFilter = { kids: 1 };
      spyOn(fixture, 'buildAgeRangeFilter').and.returnValue(ageRangeFilter);
      spyOn(fixture, 'buildKidsWelcomeFilter').and.returnValue(kidsWelcomeFilter);
      spyOn(fixture, 'loadAgeRanges').and.callFake(() => {});
      spyOn(fixture, 'loadKidsWelcome').and.callFake(() => {});

      fixture.allFilters = [];
      fixture.initializeFilters();

      expect(fixture.buildAgeRangeFilter).toHaveBeenCalled();
      expect(fixture.buildKidsWelcomeFilter).toHaveBeenCalled();
      expect(fixture.loadAgeRanges).toHaveBeenCalled();
      expect(fixture.loadKidsWelcome).toHaveBeenCalled();
      expect(fixture.allFilters).toEqual([ageRangeFilter, kidsWelcomeFilter]);
    });
  });

  describe('applyFilters function', () => {
    it('should apply filters and reload ngTable', () => {
      fixture.expanded = true;

      fixture.searchResults = [
        { 'age': 1, 'kids': false },
        { 'age': 2, 'kids': true },
        { 'age': 3, 'kids': true },
      ];

      fixture.allFilters = [
        {
          matches: function(r) {
            return r.age === 2;
          }
        },
        {
          matches: function(r) {
            return r.kids === true;
          }
        },
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

      spyOn(fixture.tableParams, 'reload').and.callFake(() => {});

      fixture.applyFilters();

      expect(fixture.tableParams.reload).toHaveBeenCalled();
      expect(fixture.expanded).toBeFalsy();
      expect(fixture.tableParams.settings().dataset).toEqual([{ 'age': 2, 'kids': true }]);
      expect(fixture.tableParams.settings().someOtherSettingThatShouldNotChange).toBeTruthy();
    });
  });

  describe('filter manipulation function', () => {
    it('clearFilters should clear and re-apply all filters', () => {
      let filters = [
        {
          clear: jasmine.createSpy('clear')
        },
        {
          clear: jasmine.createSpy('clear')
        }
      ];
      fixture.allFilters = filters;

      spyOn(fixture, 'applyFilters').and.callFake(() => {});

      fixture.clearFilters();
      filters.forEach(function(f) {
        expect(f.clear).toHaveBeenCalled();
      }, this);
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

    it('hasFilters should return false if no active filters', () => {
      let filters = [
        {
          isActive: jasmine.createSpy('isActive').and.returnValue(false)
        },
        {
          isActive: jasmine.createSpy('isActive').and.returnValue(false)
        }
      ];
      fixture.allFilters = filters;

      expect(fixture.hasFilters()).toBeFalsy();
      filters.forEach(function(f) {
        expect(f.isActive).toHaveBeenCalled();
      }, this);
    });    

    it('hasFilters should return true if one or more active filters', () => {
      let filters = [
        {
          isActive: jasmine.createSpy('isActive').and.returnValue(false)
        },
        {
          isActive: jasmine.createSpy('isActive').and.returnValue(true)
        }
      ];
      fixture.allFilters = filters;

      expect(fixture.hasFilters()).toBeTruthy();
      filters.forEach(function(f) {
        expect(f.isActive).toHaveBeenCalled();
      }, this);
    });    
  });

  describe('buildAgeRangeFilter function', () => {
    it('should return true if no age range currently filtered', () => {
      fixture.ageRanges = [
        { selected: false }
      ];
      let searchResult = {
        ageRange: [
          { attributeId: 456 },
          { attributeId: 789 }
        ]
      };

      let filter = fixture.buildAgeRangeFilter();
      expect(filter.matches(searchResult)).toBeTruthy();
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
      let filter = fixture.buildAgeRangeFilter();
      expect(filter.matches(searchResult)).toBeTruthy();
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
      let filter = fixture.buildAgeRangeFilter();
      expect(filter.matches(searchResult)).toBeFalsy();
    });

    it('should return false if the search result does not contain an age range', () => {
      fixture.ageRanges = [
        { selected: false, attributeId: 123 },
        { selected: true, attributeId: 456 }
      ];
      let searchResult = { };
      
      let filter = fixture.buildAgeRangeFilter();
      expect(filter.matches(searchResult)).toBeFalsy();
    });
  });

  describe('buildKidsWelcomeFilter function', () => {
    it('should return true if no kids welcome currently filtered', () => {
      fixture.kidsWelcome = [
        { selected: false }
      ];
      let searchResult = {
        kidsWelcome: false
      };

      let filter = fixture.buildKidsWelcomeFilter();
      expect(filter.matches(searchResult)).toBeTruthy();
    });

    it('should return true if the search result contains a filtered kids welcome', () => {
      fixture.kidsWelcome = [
        { selected: false, value: true },
        { selected: true, value: false }
      ];
      let searchResult = {
        kidsWelcome: false
      };
      let filter = fixture.buildKidsWelcomeFilter();
      expect(filter.matches(searchResult)).toBeTruthy();
    });

    it('should return false if the search result does not contain a filtered kids welcome', () => {
      fixture.kidsWelcome = [
        { selected: false, value: true },
        { selected: true, value: false }
      ];
      let searchResult = {
        kidsWelcome: true
      };

      let filter = fixture.buildKidsWelcomeFilter();
      expect(filter.matches(searchResult)).toBeFalsy();
    });

    it('should return false if the search result does not contain a kids welcome', () => {
      fixture.kidsWelcome = [
        { selected: false, value: true },
        { selected: true, value: false }
      ];
      let searchResult = { };

      let filter = fixture.buildKidsWelcomeFilter();
      expect(filter.matches(searchResult)).toBeFalsy();
    });
  });  
});