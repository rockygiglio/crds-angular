let WOW = require('wow.js/dist/wow.min.js');

export default class LandingController {
  constructor(CMSService, $filter, StreamStatusService) {

    this.streamStatus = StreamStatusService.getStatus();
    console.log(this.streamStatus);

    this.cmsService = CMSService;
    this.filter = $filter;

    new WOW({
      offset: 100,
      mobile: false
    }).init();

    this.cmsService
      .getRecentMessages(4)
      .then((response) => {
        this.pastWeekends = this.parseWeekends(response)
      })
  }

  parseWeekends(response) {
    return response.map((event, i, pastWeekends) => {
      if (typeof event.series !== "undefined") {
        let title = this.filter('replaceNonAlphaNumeric')(event.title);

        event.delay = i * 100;
        event.subtitle = event.title;
        
        if (event.number === 0) {
          event.number++;
        }
        event.title = `${event.series.title} #${event.number}`;

        event.url = `/message/${event.id}/${title}`
        event.image = 'https://crds-cms-uploads.imgix.net/content/images/register-bg.jpg'

        if (typeof event.messageVideo !== "undefined" && typeof event.messageVideo.still !== 'undefined') {
          event.image = event.messageVideo.still.filename
        } 
      }
      return event;
    })
  }
}
