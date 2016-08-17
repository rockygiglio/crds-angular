import { Component, AfterViewInit } from '@angular/core';
import { StreamspotService } from './streamspot.service';

declare var window: any;
declare var chrome: any;

window.videojs = require('video.js/dist/video');

require('./vendor/streamspotAnalytics');
require('./vendor/videojs5-hlsjs-source-handler.min');
require('videojs-chromecast/dist/videojs-chromecast');

@Component({
  selector: 'videojs',
  templateUrl: './videojs.ng2component.html'
})

export class VideoJSComponent implements AfterViewInit {

  id: string = "videojs-player";
  player: any;
  visible: boolean = false;

  constructor(private streamspot: StreamspotService) {}

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

        this.player = window.videojs(this.id, {
          "techOrder": ["html5"],
          "fluid": true,
          "poster" : defaultPlayer.bgLink,
          "preload": 'auto',
          "html5": {
            "hlsjsConfig": {
              "debug": false
            }
          }
        });

        // create play handler (analytics)
        this.player.on('play', () => {
          window.SSTracker = window.SSTracker ? window.SSTracker : new window.Tracker(this.streamspot.ssid);
          window.SSTracker.start(broadcaster.live_src.cdn_hls, true, this.streamspot.ssid);
        });

        // create stop handler (analytics)
        this.player.on('pause', () => {
          if(window.SSTracker){
            window.SSTracker.stop();
            window.SSTracker = null;
          }
        });
            
        if ( broadcaster.isBroadcasting === true ) {
    
          this.player.src([
            {
              "type": "application/x-mpegURL",
              "src": broadcaster.live_src.cdn_hls
            }
          ]);

          this.visible = true;

          this.player.ready(() => {

            this.player.play();

          });
          
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
