
import {SearchFilterValue} from '../../../../app/group_tool/group_search_filter/filter_impl/searchFilter';
import GroupTypeFilter from '../../../../app/group_tool/group_search_filter/filter_impl/groupType.filter';

describe('GroupTypeFilter', () => {
  describe('matches() function', () => {
    it('should return true if no group type currently filtered', () => {
      let groupTypes = [
        new SearchFilterValue('Couples', 123, false)
      ];
      let searchResult = {
        groupType: { attributeId: 456 }
      };

      let filter = new GroupTypeFilter('Group Type', groupTypes);
      expect(filter.matches(searchResult)).toBeTruthy();
    });

    it('should return true if the search result contains a filtered group type', () => {
      let groupTypes = [
        new SearchFilterValue('Couples', 123, false),
        new SearchFilterValue('Men', 456, true)
      ];
      let searchResult = {
        groupType: { attributeId: 456 }
      };

      let filter = new GroupTypeFilter('Group Type', groupTypes);
      expect(filter.matches(searchResult)).toBeTruthy();
    });

    it('should return false if the search result does not contain a filtered group type', () => {
      let groupTypes = [
        new SearchFilterValue('Couples', 123, false),
        new SearchFilterValue('Men', 456, true)
      ];
      let searchResult = {
        groupType: { attributeId: 234 }
      };

      let filter = new GroupTypeFilter('Group Type', groupTypes);
      expect(filter.matches(searchResult)).toBeFalsy();
    });

    it('should return false if the search result does not contain a group type', () => {
      let groupTypes = [
        new SearchFilterValue('Couples', 123, false),
        new SearchFilterValue('Men', 456, true)
      ];
      let searchResult = { };
      
      let filter = new GroupTypeFilter('Group Type', groupTypes);
      expect(filter.matches(searchResult)).toBeFalsy();
    });
  });
});