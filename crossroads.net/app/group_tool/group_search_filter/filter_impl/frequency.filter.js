
import {SearchFilter, SearchFilterValue} from './searchFilter';

export default class FrequencyFilter extends SearchFilter {
    constructor(filterName, createGroupService) {
      super(filterName, [], this._matchingFunction);

      let frequencies = createGroupService.getMeetingFrequencies();
      frequencies = _.sortBy(frequencies, 'meetingFrequencyId' );

      this.getValues().push.apply(this.getValues(), frequencies.map((a) => {
        return new SearchFilterValue(a.meetingFrequencyDesc, a.meetingFrequencyId, false);
      }));        
    }

    _matchingFunction(result) {
        // Guard against errors if group has no frequency.  Shouldn't happen, but just in case...
        if(!result.meetingFrequencyID) {
            return false;
        }

        let selectedMeetingFrequencies = this.getSelectedValues();

        let filtered = selectedMeetingFrequencies.filter((a) => {
            return a.value === result.meetingFrequencyID;
        });

        return filtered !== undefined && filtered.length > 0;
    }
}
