const iFrameResizer = require('iframe-resizer/js/iframeResizer.min.js');

export default class StreamingController {
  constructor(CMSService, StreamspotService, GeolocationService, $rootScope, $modal, $location, $timeout, $sce) {
    this.cmsService = CMSService;
    this.streamspotService = StreamspotService;
    this.geolocationService = GeolocationService;
    this.rootScope = $rootScope;
    this.timeout = $timeout;
    this.modal = $modal;
    this.inProgress = false;
    this.numberOfPeople = 2;
    this.displayCounter = true;
    this.countSubmit = false;
    this.dontMiss = [];
    this.beTheChurch = [];

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

    switch (__CRDS_ENV__) {
      case 'int':
        this.baseUrl = 'https://embedint.crossroads.net';
        break;
      case 'demo':
        this.baseUrl = 'https://embeddemo.crossroads.net';
        break;
      default:
        this.baseUrl = 'https://embed.crossroads.net';
        break;
    }

    this.timeout(this.afterViewInit.bind(this), 500);
  }

  afterViewInit() {
    // Carousel variables
    this.wrapper = document.querySelector('.crds-carousel__content-wrap');
    this.content = document.querySelector('.crds-carousel__list');
    this.content.style.marginLeft = '0px';
    this.pos = 0;

    this.resizeIframe();
  }

  resizeIframe() {
    const el = document.querySelector('.digital-program__giving iframe');
    el.removeAttribute('height');
    this.inlineGiving = iFrameResizer({
      heightCalculationMethod: 'taggedElement',
      minHeight: 350,
      checkOrigin: false,
      interval: -16
    }, el);
  }

  buildUrl() {
    const params = this.queryStringParams || 'type=donation&theme=dark';
    return this.sce.trustAsResourceUrl(`${this.baseUrl}?${params}`);
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

  carouselCardWidth() {
    this.article = document.querySelector('.crds-carousel__item');

    return this.article.offsetWidth;
  }

  carouselPrev() {
    if (this.pos < 0) {
      this.pos += this.carouselCardWidth();
      this.content.style.marginLeft = `${this.pos}px`;
    }
  }

  carouselNext() {
    if (this.pos > ((this.content.scrollWidth * -1) + (this.wrapper.offsetWidth))) {
      this.pos -= this.carouselCardWidth();
      this.content.style.marginLeft = `${this.pos}px`;
    }
  }
}