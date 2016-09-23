
import {SearchFilterValue} from '../../../../app/group_tool/group_search_filter/filter_impl/searchFilter';
import LeadersSiteFilter from '../../../../app/group_tool/group_search_filter/filter_impl/leadersSite.filter';
import SmallGroup from '../../../../app/group_tool/model/smallGroup';

describe('LeadersSiteFilter', () => {
  describe('matches() function', () => {
    it('should return true if no leaders site range currently filtered', () => {
      let leadersSite = [
        new SearchFilterValue('Oakley', 123, false)
      ];
      let searchResult = {};

      let filter = new LeadersSiteFilter('Leaders Site', leadersSite);
      expect(filter.matches(searchResult)).toBeTruthy();
    });

    it('should return true if the search result contains a filtered leaders site range', () => {
      let leadersSite = [
        new SearchFilterValue('Oakley', 123, false),
        new SearchFilterValue('Anywhere', 456, true)
      ];
      let searchResult = new SmallGroup({});

      spyOn(searchResult, 'leaders').and.callFake(() => {
        return [{ congregation: 'Oakley' }, { congregation: 'Anywhere' }];
      });

      let filter = new LeadersSiteFilter('Leaders Site', leadersSite);
      expect(filter.matches(searchResult)).toBeTruthy();
    });

    it('should return false if the search result does not contain a filtered leaders site', () => {
      let leadersSite = [
        new SearchFilterValue('Oakley', 123, false),
        new SearchFilterValue('Anywhere', 456, true)
      ];
      let searchResult = new SmallGroup({});

      spyOn(searchResult, 'leaders').and.callFake(() => {
        return [{ congregation: 'Florence' }, { congregation: 'Uptown' }];
      });

      let filter = new LeadersSiteFilter('Leaders Site', leadersSite);
      expect(filter.matches(searchResult)).toBeFalsy();
    });

    it('should return false if the search result does not contain an leaders site', () => {
      let leadersSite = [
        new SearchFilterValue('Oakley', 123, false),
        new SearchFilterValue('Anywhere', 456, true)
      ];
      let searchResult = new SmallGroup({});

      let filter = new LeadersSiteFilter('Leaders Site', leadersSite);
      expect(filter.matches(searchResult)).toBeFalsy();
    });
  });
});
