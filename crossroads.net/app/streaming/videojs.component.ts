import { Component, AfterViewInit } from '@angular/core';
import { StreamspotService } from './streamspot.service';

var videojs = require('video.js/dist/video');
//var sstracker = require('https://d2i0qcc2ysg3s9.cloudfront.net/analytics-pre-release.js');

declare var window: any;
window.videojs = videojs;
//window.Tracker = sstracker;

require('videojs-contrib-hls/dist/videojs-contrib-hls');

@Component({
  selector: 'videojs',
  templateUrl: './videojs.ng2component.html'
})

export class VideoJSComponent implements AfterViewInit {

  player: any;

  nonPublicUrl: string = "//limelight1.streamspot.com/dvr/smil:crossr30e3.smil/playlist.m3u8";
  productionUrl: string = "//limelight1.streamspot.com/url/smil:crossr4915.smil/playlist.m3u8";
  testUrl: string = "//qthttp.apple.com.edgesuite.net/1010qwoeiuryfg/sl.m3u8";

  ssid: string = "crossr30e3";
  //ssid: string = "crossr4915";

  url: string = "";
  id: string = "videojs-player";
  width: number = 640;
  height: number = 380;
  poster: string = "//d2i0qcc2ysg3s9.cloudfront.net/crossr4915_0333c740_spark_titlepng_resized.png";
  visible: boolean = true;

  constructor(private streamspot: StreamspotService) {}

  ngAfterViewInit() {
    this.url = this.productionUrl;

    // set up video
    this.player = window.videojs(this.id, {
      "techOrder": ["flash", "html5"],
      "fluid": true
    });

    // create play handler (analytics)
    this.player.on('play', () => {
      var streamName = 'crossr30e3_mbr2'
//      window.SSTracker = window.SSTracker ? window.SSTracker : new window.Tracker(this.ssid);
 //     window.SSTracker.start(this.url, true, this.ssid);
    })

    // create stop handler (analytics)
    this.player.on('pause', () => {
//      if(window.SSTracker){
//        window.SSTracker.stop();
//        window.SSTracker = null;
//      }
    })

    // do callback for broadcasting event
    this.streamspot.getBroadcasting((data: any) => {

      var isBroadcasting: boolean = data.isBroadcasting;
      if ( isBroadcasting === false ) {
        
        window.location.href = '/live';
      }
      else {

        // set player source
        this.player.src({
          "type": "application/x-mpegURL",
          "src": this.url
        });

        this.player.play();

      }

    });

  }

}
