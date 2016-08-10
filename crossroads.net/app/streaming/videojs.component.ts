import { Component, AfterViewInit } from '@angular/core';
import { StreamspotService } from './streamspot.service';

var videojs = require('video.js/dist/video');

declare var window: any;
window.videojs = videojs;

require('./vendor/streamspotAnalytics');
require('videojs-contrib-hls/dist/videojs-contrib-hls');

@Component({
  selector: 'videojs',
  templateUrl: './videojs.ng2component.html',
  providers: [StreamspotService]
})

export class VideoJSComponent implements AfterViewInit {

  player: any;
  url: string;
  id: string = "videojs-player";
  width: number = 320;
  height: number = 190;
  poster: string;
  visible: boolean = true;

  constructor(private streamspot: StreamspotService) {}

  ngAfterViewInit() {

    // toggeling dev
    this.streamspot.toggleDev(true);

    this.streamspot.getBroadcaster((data: any) => {

      if ( data.broadcaster != undefined ) {

        var broadcaster = data.broadcaster;

        this.url = broadcaster.live_src.cdn_hls;
        this.streamspot.getPlayers((data: any) => {

          if ( data.players != undefined ) {

            // this needs to change later to get the default player
            var defaultPlayer = data.players[0];

            this.poster = defaultPlayer.bgLink;
            this.player = window.videojs(this.id, {
              "techOrder": ["html5", "flash"],
              "fluid": true,
              "poster" : this.poster,
              "preload": 'auto'
            });

            // create play handler (analytics)
            this.player.on('play', () => {
              window.SSTracker = window.SSTracker ? window.SSTracker : new window.Tracker(this.streamspot.id);
              window.SSTracker.start(this.url, true, this.streamspot.id);
            });

            // create stop handler (analytics)
            this.player.on('pause', () => {
              if(window.SSTracker){
                window.SSTracker.stop();
                window.SSTracker = null;
              }
            });

            broadcaster.isBroadcasting = true;
            if ( broadcaster.isBroadcasting === true ) {
        
              this.player.src([
                {
                  "type": "application/x-mpegURL",
                  "src": this.url
                }
              ]);

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
            console.log('Failed to get players for video stream.');
          }

        });

      }
      else {
        console.log('Failed to get broadcaster from API');
      }

    });

  }

}
