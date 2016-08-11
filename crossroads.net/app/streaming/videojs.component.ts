import { Component, AfterViewInit, ElementRef } from '@angular/core';
import { StreamspotService } from './streamspot.service';

var videojs = require('video.js/dist/video');

declare var window: any;
window.videojs = videojs;

require('./vendor/streamspotAnalytics');
require('videojs-contrib-hls/dist/videojs-contrib-hls');

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

  constructor(private streamspot: StreamspotService, private _el:ElementRef) {}

  ngAfterViewInit() {

    this.streamspot.getBroadcaster((data: any) => {

      if ( data.broadcaster !== undefined ) {

        var broadcaster = data.broadcaster;
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
          "techOrder": ["html5", "flash"],
          "fluid": true,
          "poster" : this.poster,
          "preload": 'auto'
        });

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

          this.player.ready(() => {
            this.visible = true;
            this.player.play();
          });
        }
        else {
          console.log('No broadcast available.');
          window.location.href = '/live';
        }

      }
      else {
        console.log('Failed to get broadcaster from API');
      }

    });

  }

}
