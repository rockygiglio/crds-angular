import { Component, AfterViewInit, OnDestroy } from '@angular/core';
import { StreamspotService } from './streamspot.service';

@Component({
  selector: 'streamspot-iframe',
  templateUrl: './streamspot-iframe.ng2component.html',
  providers: [StreamspotService]
})

export class StreamspotIframeComponent {

  debug: boolean = false;
  markup: string = '';

  constructor( private streamspot: StreamspotService) {}

  ngOnDestroy() {
    this.markup = '';
  }

  ngAfterViewInit() {

    this.streamspot.getBroadcaster().subscribe( response => {

      if ( response.success === true && response.data.broadcaster !== undefined ) {

        var broadcaster = response.data.broadcaster;

        if ( broadcaster.players === undefined || broadcaster.players.length === 0 ) {
          console.log('Error getting player from broadcast.');
          return;
        }

        var defaultPlayer;
        broadcaster.players.forEach(element => {
          if ( element.default === true ) {
            defaultPlayer = element;
            return false;
          }
        });

        if ( defaultPlayer === undefined ) {
          defaultPlayer = broadcaster.players[0];
        }

        console.log(defaultPlayer);

        if ( broadcaster.isBroadcasting === true || this.debug ) {

          this.markup = '<iframe width="640" height="360" src="https://player2.streamspot.com/?playerId=' + defaultPlayer.id + '" frameborder="0" allowfullscreen></iframe>';

        }
        else {
          console.log('No broadcast available.');
          window.location.href = '/live';
        }
      }
      else {
        console.log('StreamSpot API Failure!');
      }

    });
  }
}
