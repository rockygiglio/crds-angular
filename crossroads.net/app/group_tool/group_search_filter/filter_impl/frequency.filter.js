import {SearchFilter} from './searchFilter';

export default class FrequencyFilter extends SearchFilter {
    constructor(filterName, filterValues) {
        super(filterName, filterValues, this._matchingFunction);
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
