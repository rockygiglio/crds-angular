
export default class StreamingController {
  /*@ngInject*/
  constructor(CMSService, StreamspotService, $rootScope) {
    this.cmsService        = CMSService;
    this.streamspotService = StreamspotService;
    this.rootScope = $rootScope;

    this.inProgress     = false;
    this.numberOfPeople = 2;
    this.displayCounter = true;
    this.countSubmit    = false;
    this.dontMiss       = [];
    this.beTheChurch    = []
    this.redirectText   = 'Go Back';

    this.rootScope.$on('isBroadcasting', (e, inProgress) => {
      this.inProgress = inProgress;
      window.location.href = '/live';
    });


  }

  goBack() {
    window.location.href = '/live-v1';
  }
}