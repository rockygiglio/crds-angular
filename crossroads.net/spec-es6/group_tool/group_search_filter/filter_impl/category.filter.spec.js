
import {SearchFilterValue} from '../../../../app/group_tool/group_search_filter/filter_impl/searchFilter';
import CategoryFilter from '../../../../app/group_tool/group_search_filter/filter_impl/category.filter';

describe('CategoryFilter', () => {
  describe('matches() function', () => {
    it('should return true if no category currently filtered', () => {
      let categories = [
        new SearchFilterValue('Interest', 123, false)
      ];
      let searchResult = {
        categories: [
          { categoryId: 456 }
        ]
      };

      let filter = new CategoryFilter('Categories', categories);
      expect(filter.matches(searchResult)).toBeTruthy();
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

      let filter = new CategoryFilter('Categories', categories);
      expect(filter.matches(searchResult)).toBeTruthy();
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

      let filter = new CategoryFilter('Categories', categories);
      expect(filter.matches(searchResult)).toBeFalsy();
    });

    it('should return false if the search result does not contain a category', () => {
      let categories = [
        new SearchFilterValue('Interest', 123, false),
        new SearchFilterValue('Neighborhoods', 456, true)
      ];
      let searchResult = {
        categories: [ ]
      };

      let filter = new CategoryFilter('Categories', categories);
      expect(filter.matches(searchResult)).toBeFalsy();
    });
  });
});
