
import {SearchFilter} from './searchFilter';

export default class MeetingDayFilter extends SearchFilter {
  constructor(filterName, filterValues) {
    super(filterName, filterValues, this._matchingFunction);
  }

  _matchingFunction(result) {
    // Guard against errors if group has no days.  Shouldn't happen, but just in case...
    if(!result.meetingDay && !result.meetingTimeFrequency) {
      return false;
    }

    let selectedMeetingDays = this.getSelectedValues();

    let filtered = selectedMeetingDays.filter((a) => {
      return a.getName() === result.meetingDay || a.getName() === result.meetingTimeFrequency;
    });

    return filtered !== undefined && filtered.length > 0;
  }
}
