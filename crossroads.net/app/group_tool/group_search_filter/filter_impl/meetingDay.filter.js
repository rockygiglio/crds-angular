
import {SearchFilter, SearchFilterValue} from './searchFilter';

export default class MeetingDayFilter extends SearchFilter {
  constructor(filterName, groupService) {
    super(filterName, [], this._matchingFunction);
    
    groupService.getDaysOfTheWeek().then(
      (data) => {
        data = _.sortBy( data, 'dp_RecordID' );
        this.getValues().push.apply(this.getValues(), data.map((a) => {
          return new SearchFilterValue(a.dp_RecordName, a.dp_RecordName, false);
        }));
        this.getValues().push(new SearchFilterValue('Flexible', 'Flexible Meeting Time', false, undefined, 'groupToolFlexibleMeetingFilterHelpText'));
      },
      (err) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }
    ).finally(
      () => {
      });    
  }

  _matchingFunction(result) {
    // Guard against errors if group has no days.  Shouldn't happen, but just in case...
    if(!result.meetingDay && !result.meetingTimeFrequency) {
      return false;
    }

    let selectedMeetingDays = this.getSelectedValues();

    let filtered = selectedMeetingDays.filter((a) => {
      return a.getValue() === result.meetingDay || a.getValue() === result.meetingTimeFrequency;
    });

    return filtered !== undefined && filtered.length > 0;
  }
}
