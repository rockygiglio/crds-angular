let WOW = require('wow.js/dist/wow.min.js');

export default class LandingController {
  constructor($rootScope, $filter, CMSService, StreamStatusService) {

    this.rootScope = $rootScope;
    this.streamStatus = StreamStatusService.getStatus();

    this.rootScope.$on('streamStatusChanged', (e, streamStatus) => {
      this.streamStatus = streamStatus;
    });

    this.cmsService = CMSService;
    this.filter = $filter;

    new WOW({
      offset: 100,
      mobile: false
    }).init();

    var maxPastWeekends = 4;

    this.cmsService
      .getRecentMessages(maxPastWeekends)
      .then((response) => {
        this.pastWeekends = this.parseWeekends(response,maxPastWeekends)
      })
  }

  parseWeekends(response,maxPastWeekends) {

    var pastWeekendTotal = 0;
    var queriedPastWeekends = response.map((event, i, pastWeekends) => {

      pastWeekendTotal++;
      if ( pastWeekendTotal > maxPastWeekends ) {
        return false;
      }

      if (typeof event.series !== "undefined") {
        let title = this.filter('replaceNonAlphaNumeric')(event.title);

        event.delay = i * 100;
        event.subtitle = event.title;
        
        if (event.number === 0) {
          event.number++;
        }
        event.title = `${event.series.title} #${event.number}`;

        event.url = `/message/${event.id}/${title}`;
        event.image = 'https://crds-cms-uploads.imgix.net/content/images/register-bg.jpg';

        if (typeof event.messageVideo !== "undefined" && typeof event.messageVideo.still !== 'undefined') {
          event.image = event.messageVideo.still.filename
        } 
      }
      return event;
    });

    return queriedPastWeekends.slice(0,maxPastWeekends);
  }
}
