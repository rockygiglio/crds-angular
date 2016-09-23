
import {SearchFilterValue} from '../../../../app/group_tool/group_search_filter/filter_impl/searchFilter';
import MeetingDayFilter from '../../../../app/group_tool/group_search_filter/filter_impl/meetingDay.filter';

describe('MeetingDayFilter', () => {
  describe('matches() function', () => {
    it('should return true if no meeting day currently filtered', () => {
      let meetingDays = [
        new SearchFilterValue('Sunday', 123, false)
      ];
      let searchResult = {
        meetingDay: 'Saturday'
      };

      let filter = new MeetingDayFilter('Days', meetingDays);
      expect(filter.matches(searchResult)).toBeTruthy();
    });

    it('should return true if the search result contains a filtered meeting day', () => {
      let meetingDays = [
        new SearchFilterValue('Sunday', 123, false),
        new SearchFilterValue('Monday', 456, true)
      ];
      let searchResult = {
        meetingDay: 'Monday'
      };

      let filter = new MeetingDayFilter('Days', meetingDays);
      expect(filter.matches(searchResult)).toBeTruthy();
    });

    it('should return false if the search result does not contain a filtered meeting day', () => {
      let meetingDays = [
        new SearchFilterValue('Sunday', 123, false),
        new SearchFilterValue('Monday', 456, true)
      ];
      let searchResult = {
        meetingDay: 'Tuesday'
      };

      let filter = new MeetingDayFilter('Days', meetingDays);
      expect(filter.matches(searchResult)).toBeFalsy();
    });

    it('should return false if the search result does not contain an meeting day', () => {
      let meetingDays = [
        new SearchFilterValue('Sunday', 123, false),
        new SearchFilterValue('Monday', 456, true)
      ];
      let searchResult = { };

      let filter = new MeetingDayFilter('Days', meetingDays);
      expect(filter.matches(searchResult)).toBeFalsy();
    });
  });
});
