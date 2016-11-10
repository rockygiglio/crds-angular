
import {SearchFilterValue} from '../../../../app/group_tool/group_search_filter/filter_impl/searchFilter';
import CategoryFilter from '../../../../app/group_tool/group_search_filter/filter_impl/category.filter';

describe('CategoryFilter', () => {
  let qApi, rootScope;

  beforeEach(inject(function ($injector) {
    qApi = $injector.get('$q');
    rootScope = $injector.get('$rootScope');
  }));
  
  describe('the constructor', () => {
    it('should load categories', () => {
      let categories = [
        {
        categoryId: 9987878,
        name: 'Journey',
        desc: 'The current Journey',
        exampleText: 'Journey Group',
        requiresActiveAtribute: true,
        attribute: {
          id: '1',
          name: 'I Am ______',
          startDate: '',
          endDate: ''
        }
      }, {
        categoryId: 2,
        name: 'Interest',
        desc: 'desc',
        exampleText: 'Ex. Boxing, XBox',
        requiresActiveAtribute: false,
        attribute: {
          id: '',
          name: '',
          startDate: '',
          endDate: ''
        }
      }
      ];

      let deferred = qApi.defer();
      deferred.resolve(categories);
      let groupService = jasmine.createSpyObj('groupServiceMock', ['getGroupTypeCategories']);
      groupService.getGroupTypeCategories.and.returnValue(deferred.promise);

      let filter = new CategoryFilter('Category', groupService);
      rootScope.$apply();
      expect(groupService.getGroupTypeCategories).toHaveBeenCalled();
      expect(filter.getValues()).toBeDefined();
      expect(filter.getValues().length).toEqual(categories.length);
      for(let i = 0; i < categories.length; i++) {
        expect(filter.getValues()[i].getName()).toEqual(categories[i].name);
        expect(filter.getValues()[i].getValue()).toEqual(categories[i].categoryId);
        expect(filter.getValues()[i].getHelpText()).toEqual(categories[i].desc);
        expect(filter.getValues()[i].isSelected()).toBeFalsy();
      }
    });
  });

  describe('matches() function', () => {
    let fixture;

    beforeEach(() => {
      let deferred = qApi.defer();
      deferred.resolve([]);

      let groupService = jasmine.createSpyObj('groupServiceMock', ['getGroupTypeCategories']);
      groupService.getGroupTypeCategories.and.returnValue(deferred.promise);
      fixture = new CategoryFilter('Category', groupService);
      rootScope.$apply();
    });
    
    it('should return true if no category currently filtered', () => {
      let categories = [
        new SearchFilterValue('Interest', 123, false)
      ];
      let searchResult = {
        categories: [
          { categoryId: 456 }
        ]
      };
      fixture.filterValues = categories;

      expect(fixture.matches(searchResult)).toBeTruthy();
    });

    it('should return true if the search result contains a filtered category', () => {
      let categories = [
        new SearchFilterValue('Interest', 123, false),
        new SearchFilterValue('Neighborhoods', 456, true)
      ];
      let searchResult = {
        categories: [
          { categoryId: 456 }
        ]
      };
      fixture.filterValues = categories;

      expect(fixture.matches(searchResult)).toBeTruthy();
    });

    it('should return false if the search result does not contain a filtered category', () => {
      let categories = [
        new SearchFilterValue('Interest', 123, false),
        new SearchFilterValue('Neighborhoods', 456, true)
      ];
      let searchResult = {
        categories: [
          { categoryId: 789 }
        ]
      };
      fixture.filterValues = categories;

      expect(fixture.matches(searchResult)).toBeFalsy();
    });

    it('should return false if the search result does not contain a category', () => {
      let categories = [
        new SearchFilterValue('Interest', 123, false),
        new SearchFilterValue('Neighborhoods', 456, true)
      ];
      let searchResult = {
        categories: [ ]
      };
      fixture.filterValues = categories;

      expect(fixture.matches(searchResult)).toBeFalsy();
    });
  });
});
