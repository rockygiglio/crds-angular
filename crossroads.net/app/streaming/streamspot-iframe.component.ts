import { Component, AfterViewInit, OnDestroy } from '@angular/core';
import { StreamspotService } from './streamspot.service';
import { SafeResourceUrl, DomSanitizationService } from '@angular/platform-browser';

@Component({
  selector: 'streamspot-iframe',
  templateUrl: './streamspot-iframe.ng2component.html',
  providers: [StreamspotService]
})

export class StreamspotIframeComponent {

  debug: boolean = false;
  markup: string = '';
  sanitizer: any;

  constructor( private streamspot: StreamspotService, sanitizer: DomSanitizationService) {
    this.sanitizer = sanitizer;
  }

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

        if ( broadcaster.isBroadcasting === true || this.debug ) {

          let id = '1adb55de';
          if ( this.streamspot.ssid == 'crossr30e3' ) {
            id = '2887fba1';
          }
          this.markup = this.sanitizer.bypassSecurityTrustHtml('<iframe width="640" height="360" src="https://player2.streamspot.com/?playerId=' + id + '" frameborder="0" allowfullscreen></iframe>');

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
