
import {SearchFilterValue} from '../../../../app/group_tool/group_search_filter/filter_impl/searchFilter';
import AgeRangeFilter from '../../../../app/group_tool/group_search_filter/filter_impl/ageRange.filter';

describe('AgeRangeFilter', () => {
  describe('matches() function', () => {
    it('should return true if no age range currently filtered', () => {
      let ageRanges = [
        new SearchFilterValue('40s', 123, false)
      ];
      let searchResult = {
        ageRange: [
          { attributeId: 456 },
          { attributeId: 789 }
        ]
      };

      let filter = new AgeRangeFilter('Age Range', ageRanges);
      expect(filter.matches(searchResult)).toBeTruthy();
    });

    it('should return true if the search result contains a filtered age range', () => {
      let ageRanges = [
        new SearchFilterValue('40s', 123, false),
        new SearchFilterValue('50s', 456, true)
      ];      
      let searchResult = {
        ageRange: [
          { attributeId: 456 },
          { attributeId: 789 }
        ]
      };
      let filter = new AgeRangeFilter('Age Range', ageRanges);
      expect(filter.matches(searchResult)).toBeTruthy();
    });

    it('should return false if the search result does not contain a filtered age range', () => {
      let ageRanges = [
        new SearchFilterValue('40s', 123, false),
        new SearchFilterValue('50s', 456, true)
      ];  
      let searchResult = {
        ageRange: [
          { attributeId: 234 },
          { attributeId: 567 }
        ]
      };
      let filter = new AgeRangeFilter('Age Range', ageRanges);
      expect(filter.matches(searchResult)).toBeFalsy();
    });

    it('should return false if the search result does not contain an age range', () => {
      let ageRanges = [
        new SearchFilterValue('40s', 123, false),
        new SearchFilterValue('50s', 456, true)
      ];  
      let searchResult = { };
      
      let filter = new AgeRangeFilter('Age Range', ageRanges);
      expect(filter.matches(searchResult)).toBeFalsy();
    });
  });
});