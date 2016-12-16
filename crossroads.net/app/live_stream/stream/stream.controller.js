export default class StreamingController {
  constructor(CMSService, StreamspotService, GeolocationService, $rootScope, $modal, $location, $timeout, $sce, $document, $interval) {
    this.cmsService = CMSService;
    this.streamspotService = StreamspotService;
    this.geolocationService = GeolocationService;
    this.rootScope = $rootScope;
    this.timeout = $timeout;
    this.document = $document;
    this.modal = $modal;
    this.inProgress = false;
    this.numberOfPeople = 2;
    this.displayCounter = true;
    this.countSubmit = false;
    this.dontMiss = [];
    this.beTheChurch = [];
    this.inlineGiving = [];
    this.interval = $interval;

    this.sce = $sce;
    let debug = false;

    if ($location !== undefined) {
      const params = $location.search();
      debug = params.debug;
    }

    if (debug === 'true') {
      this.inProgress = true;
    } else {
      this.rootScope.$on('isBroadcasting', (e, inProgress) => {
        this.inProgress = inProgress;
        if (this.inProgress === false) {
          window.location.href = '/live';
        }
      });
    }

    this.cmsService
        .getDigitalProgram()
        .then((data) => {
          this.sortDigitalProgram(data);
        });

    this.openGeolocationModal();
    this.timeout(this.afterViewInit.bind(this), 500);
  }

  afterViewInit() {
    this.carouselCard = document.querySelector('content-card');
    this.carouselCardTotal = document.querySelectorAll('content-card').length;
    this.carouselWrapper = document.querySelector('.crds-carousel__content-wrap');
    this.carousel = document.querySelector('.crds-card-carousel');
    this.carouselElement = angular.element(document.querySelector('.crds-card-carousel'));
  }

  sortDigitalProgram(data) {
    data.forEach((feature, i) => {
      // null status indicates a published feature
      if (feature.status === null || feature.status.toLowerCase() !== 'draft') {
        feature.delay = i * 100;
        feature.url = 'javascript:;';

        if (feature.link !== null) {
          feature.url = feature.link;
        }

        feature.target = '_blank';

        if (typeof feature.image !== 'undefined' && typeof feature.image.filename !== 'undefined') {
          feature.image = feature.image.filename;
        } else {
          feature.image = 'https://crds-cms-uploads.imgix.net/content/images/register-bg.jpg';
        }
        if (feature.section === 1) {
          this.dontMiss.push(feature);
        } else if (feature.section === 2) {
          this.beTheChurch.push(feature);
        }
      }
    });
  }

  showGeolocationBanner() {
    return this.geolocationService.showBanner();
  }

  openGeolocationModal() {
    if (this.geolocationService.showModal()) {
      this.modalInstance = this.modal.open({
        templateUrl: 'geolocation_modal/geolocationModal.html',
        controller: 'GeolocationModalController',
        controllerAs: 'geolocationModal',
        openedClass: 'geolocation-modal',
        backdrop: 'static',
        size: 'lg'
      });
    }
  }

  getCarouselCardWidth() {
    let marginRight = parseInt(window.getComputedStyle(this.carouselCard).marginRight, 0); // eslint-disable-line prefer-const
    return this.carouselCard.offsetWidth + marginRight;
  }

  getCurrentScrollPosition() {
    return this.carousel.scrollLeft;
  }

  carouselNext() {
    /* eslint-disable prefer-const */
    let cardWidth = this.getCarouselCardWidth();
    let n = Math.floor(this.getCurrentScrollPosition() / cardWidth);
    let scrollLeft = (n + 1) * cardWidth;
    /* eslint-enable prefer-const */
    this.scrollTo(scrollLeft);
  }

  carouselPrev() {
    let cardWidth = this.getCarouselCardWidth(); // eslint-disable-line prefer-const
    let scrollPos = this.getCurrentScrollPosition(); // eslint-disable-line prefer-const
    let n = 0;
    if (scrollPos > cardWidth) {
      n = Math.round(scrollPos / cardWidth) - 1;
    }
    let scrollLeft = n * cardWidth; // eslint-disable-line prefer-const
    this.scrollTo(scrollLeft);
  }

  scrollTo(x, duration = 250) {
    this.carouselElement.scrollLeftAnimated(x, duration);
  }

  static getMargins(el) {
    return {
      marginRight: parseInt(window.getComputedStyle(el).marginRight, 0),
      marginLeft: parseInt(window.getComputedStyle(el).marginLeft, 0)
    };
  }

}
