import { Component, AfterViewInit } from '@angular/core';
import { StreamspotService } from './streamspot.service';

var videojs = require('video.js/dist/video');

declare var window: any;
declare var Hls: any;
window.videojs = videojs;

require('./vendor/streamspotAnalytics');
var HlsTest = require('./vendor/videojs5-hlsjs-source-handler.min');

@Component({
  selector: 'videojs',
  templateUrl: './videojs.ng2component.html'
})

export class VideoJSComponent implements AfterViewInit {

  player: any;
  url: string;
  id: string = "videojs-player";
  width: number = 320;
  height: number = 190;
  poster: string;
  visible: boolean = false;

  constructor(private streamspot: StreamspotService) {}

  ngAfterViewInit() {

    this.streamspot.getBroadcaster().subscribe( response => {

      if ( response.success === true && response.data.broadcaster !== undefined ) {

        var broadcaster = response.data.broadcaster;
        this.url = broadcaster.live_src.cdn_hls;

        var defaultPlayer;
        for (var i = 0; i < broadcaster.players.length; i++) {
          if ( broadcaster.players[i].default === true ) {
            defaultPlayer = broadcaster.players[i];
            break;
          }
        }

        if ( defaultPlayer === undefined ) {
          console.log('Error getting player from broadcast.');
          return;
        }

        this.poster = defaultPlayer.bgLink;
        this.player = window.videojs(this.id, {
          "techOrder": ["html5"],
          "fluid": true,
          "poster" : this.poster,
          "preload": 'auto',
          "html5": {
            "hlsjsConfig": {
              "debug": true
            }
          }
        });

        console.log(HlsTest);

        // create play handler (analytics)
        this.player.on('play', () => {
          window.SSTracker = window.SSTracker ? window.SSTracker : new window.Tracker(this.streamspot.ssid);
          window.SSTracker.start(this.url, true, this.streamspot.ssid);
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
              "src": this.url
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
