
import {SearchFilter, SearchFilterValue} from './searchFilter'; 

class TimeRange {
  constructor(beginTime, endTime) {
    this.beginTime = moment(beginTime, 'HH:mm:ss');
    this.endTime = moment(endTime, 'HH:mm:ss');
  }

  isWithinTimeRange(meetingTime) {
    let m = moment(meetingTime, 'HH:mm:ss');
    // inclusive match on begin and end time
    return m.isSameOrAfter(this.beginTime) && m.isSameOrBefore(this.endTime);
  }
}

export default class MeetingTimeFilter extends SearchFilter {
  constructor(filterName) {
    let filterValues = [
      new SearchFilterValue('Mornings (before noon)', new TimeRange('00:00:00', '11:59:59'), false),
      new SearchFilterValue('Afternoons (12 - 5pm)', new TimeRange('12:00:00', '17:00:00'), false),
      new SearchFilterValue('Evenings (after 5pm)', new TimeRange('17:00:01', '23:59:59'), false)
    ];

    super(filterName, filterValues, this._matchingFunction);
  }

  _matchingFunction(result) {
    // Guard against errors if group has no meeting time set.  Shouldn't happen, but just in case...
    if(result.meetingTime === undefined) {
      return false;
    }

    let selectedMeetingTime = this.getSelectedValues();
    
    let filtered = selectedMeetingTime.filter((v) => {
      return v.getValue().isWithinTimeRange(result.meetingTime);
    });

    return filtered !== undefined && filtered.length > 0;
  }
}