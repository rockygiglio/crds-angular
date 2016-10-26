import Countdown from '../models/countdown';

export default class CountdownService {
  constructor($rootScope, StreamspotService) {
    this.rootScope = $rootScope;
    this.streamspotService = StreamspotService;
    this.countdown = new Countdown();

    this.rootScope.$on('nextEvent', (e, event) => {
      this.event = event;
      this.parseEvent();
    })
  }

  parseEvent() {
    this.isCountdown    = this.event.isUpcoming();
    this.isBroadcasting = this.event.isBroadcasting();

    let duration = moment.duration(
      +this.event.start - +moment(),
      'milliseconds'
    );
    this.countdown.days    = this.pad(duration.days());
    this.countdown.hours   = this.pad(duration.hours());
    this.countdown.minutes = this.pad(duration.minutes());
    this.countdown.seconds = this.pad(duration.seconds());
    this.displayCountdown  = true;
  }

  pad(value) {
    return value < 10 ? `0${value}`: `${value}`;
  }
}