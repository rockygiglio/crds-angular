import { Component, AfterViewInit } from '@angular/core';

var videojs = require('video.js/dist/video');
declare var window: any;
window.videojs = videojs;
require('videojs-contrib-hls/dist/videojs-contrib-hls');

@Component({
  selector: 'videojs',
  templateUrl: './videojs.ng2component.html'
})

export class VideoJSComponent implements AfterViewInit {

  player: any;

  // non-public
  //stream: string = "https://limelight1.streamspot.com/dvr/smil:crossr30e3.smil/playlist.m3u8";

  // production
  //stream: string = "https://limelight1.streamspot.com/url/smil:crossr4915.smil/playlist.m3u8";

  // test link
  stream: string = "http://qthttp.apple.com.edgesuite.net/1010qwoeiuryfg/sl.m3u8";

  id: string = "videojs-player";
  width: number = 640;
  height: number = 380;
  poster: string = "https://d2i0qcc2ysg3s9.cloudfront.net/crossr4915_0333c740_spark_titlepng_resized.png";
  visible: boolean = true;

  ngAfterViewInit() {

    // set up video
    this.player = window.videojs(this.id, {
      "techOrder": ["flash", "html5"]
    });

    // set player source
    this.player.src({
      "type": "application/x-mpegURL", 
      "src": this.stream
    });
    
    // callback for pre-data
    this.player.on("loadstart", function(t: any) {});

    // callback for successful data load
    this.player.on("loadeddata", function(t: any) {});

    // callback for error handling
    this.player.on("error", function(t: any) {});

    /*

    All videojs callbacks:

    loadstart
    waiting
    canplay
    canplaythrough
    playing
    ended
    seeking
    seeked
    play
    firstplay
    pause
    progress
    durationchange
    fullscreenchange
    error
    suspend
    abort
    emptied
    stalled
    loadedmetadata
    loadeddata
    timeupdate
    ratechange
    volumechange
    texttrackchange
    loadedmetadata
    posterchange
    textdata

    */

  }

}