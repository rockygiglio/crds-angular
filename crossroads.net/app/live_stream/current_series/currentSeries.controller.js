
export default class CurrentSeriesController {
  constructor(CMSService, $modal) {
    this.title        = '';
    this.picture      = '';
    this.description  = '';
    this.startDate    = '';
    this.endDate      = '';
    this.runningDates = '';
    this.trailer      = '';
    this.embed        = '';
    this.tags         = [];
    this.visible      = false;
    this.response;
    this.cmsService = CMSService;
    this.modal = $modal;
  }

  $onInit() {
    this.cmsService.getCurrentSeries().then((response) => {
      console.log(response);
      this.parseData(response);
      if (response === undefined) {
        this.cmsService.getLastSeries().then((response) => {
          this.parseData(response);
        })
      }
    })
  }

  open() {
    let options = { src: this.embed };
    let instance = this.modal.open({
      size: 'lg',
      templateUrl: 'current_series_modal/currentSeriesModal.html',
      controller: 'CurrentSeriesModalController',
      controllerAs: 'modal',
      openedClass: 'crds-modal',
      resolve: {
        options: function() {
          return options;
        }
      }
    })
    instance.rendered.then(() => {
      document.getElementById('youtube-player').src = this.embed;
    })
  }

  parseData(response) {
    this.response = response;

    if (this.response !== undefined) {
      this.title = response.title;
      this.description = response.description;
      this.startDate   = response.startDate;
      this.endDate     = response.endDate;

      if ( response.trailerLink !== null ) {
        this.trailer = response.trailerLink;
        let embed = this.trailer.split(/https*:\/\/www.youtube.com\/watch\?v=/);
        if (embed[1]) {
          this.embed = `https://www.youtube.com/embed/${embed[1]}`;
        }
      }

      if ( response.image !== undefined ) {
        this.picture = response.image.filename;
      }

      this.setRunningDates(response);
      this.setTagsArray(response);
      
      this.visible = true;

    }
  }

  setRunningDates(response) {
    let formatString = 'MMMM Do';
    let mStartDate = moment(response.startDate);
    let mEndDate = moment(response.endDate);

    if ( mStartDate.isValid() && mEndDate.isValid() ) {
      this.runningDates = `RUNS: ${mStartDate.format(formatString)} - ${mEndDate.format(formatString)}`;
    }
  }

  setTagsArray(response) {
    if ( response.tags !== undefined && response.tags.length > 0 ) {
      this.tags = response.tags.map(tag => tag.title);
    }
  }
}