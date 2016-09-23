
import {SearchFilterValue} from '../../../../app/group_tool/group_search_filter/filter_impl/searchFilter';
import FrequencyFilter from '../../../../app/group_tool/group_search_filter/filter_impl/frequency.filter';

describe('FrequencyFilter', () => {
    describe('matches() function', () => {
        it('should return true if no frequency currently filtered', () => {
            let frequencies = [
                new SearchFilterValue('Weekly', 1, false)
            ];
            let searchResult = {
                meetingFrequencyID: 1
            };

            let filter = new FrequencyFilter('Frequency', frequencies);
            expect(filter.matches(searchResult)).toBeTruthy();
        });

        it('should return true if the search result contains a filtered frequency', () => {
            let frequencies = [
                new SearchFilterValue('Weekly', 1, true),
                new SearchFilterValue('Monthly', 3, true)
            ];
            let searchResult = {
                meetingFrequencyID: 1
            };

            let filter = new FrequencyFilter('Frequency', frequencies);
            expect(filter.matches(searchResult)).toBeTruthy();
        });

        it('should return false if the search result does not contain a filtered frequency', () => {
            let frequencies = [
                new SearchFilterValue('Weekly', 1, false),
                new SearchFilterValue('Monthly', 3, true)
            ];
            let searchResult = {
                meetingFrequencyID: 1
            };

            let filter = new FrequencyFilter('Frequency', frequencies);
            expect(filter.matches(searchResult)).toBeFalsy();
        });

        it('should return false if the search result does not contain an meeting day', () => {
            let frequencies = [
                new SearchFilterValue('Weekly', 1, false),
                new SearchFilterValue('Monthly', 3, true)
            ];
            let searchResult = { };

            let filter = new FrequencyFilter('Frequency', frequencies);
            expect(filter.matches(searchResult)).toBeFalsy();
        });
    });
});
