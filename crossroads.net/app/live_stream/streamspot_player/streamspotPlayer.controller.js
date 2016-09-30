
export default class StreamspotPlayerController {
  /*@ngInject*/
  constructor(StreamspotService, $rootScope) {
    this.streamspotService = StreamspotService;
    this.rootScope = $rootScope;

    this.debug     = false;
    this.markup = '';
  }

  $onInit() {
    this.streamspotService.getBroadcaster().then((response) => {
      if ( response.success === true && response.data.broadcaster !== undefined ) {

        let broadcaster = response.data.broadcaster,
            id = '1adb55de'
            defaultPlayer;

        if ( broadcaster.players === undefined || broadcaster.players.length === 0 ) {
          console.log('Error getting player from broadcast.');
          return;
        }

        broadcaster.players.forEach((element) => {
          if ( element.default === true ) {
            defaultPlayer = element;
            return false;
          }
        });

        if ( defaultPlayer === undefined ) {
          defaultPlayer = broadcaster.players[0];
        }

        if ( broadcaster.isBroadcasting === true || this.debug ) {

          if ( this.streamspotService.ssid === 'crossr30e3' ) {
            id = '2887fba1';
          }
          this.src = `https://player2.streamspot.com/?playerId=${id}`;
          document.getElementById('streamspot-iframe').src = this.src;
        }
        else {
          console.log('No broadcast available.');
        }
      }
      else {
        console.error('StreamSpot API Failure!');
      }
    })
  }
}