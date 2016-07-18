import { Component, OnInit } from '@angular/core';

declare var videojs: any;

@Component({
  selector: 'videojs',
  templateUrl: './videojs.component.html'
})

export class VideoJSComponent implements OnInit {

  player: any;

  //stream: string = "https://limelight1.streamspot.com/dvr/smil:crossr30e3.smil/playlist.m3u8";
  stream: string = "https://limelight1.streamspot.com/url/smil:crossr4915.smil/playlist.m3u8";

  id: string = "videojs-player";
  width: number = 640;
  height: number = 380;
  poster: string = "https://d2i0qcc2ysg3s9.cloudfront.net/crossr4915_0333c740_spark_titlepng_resized.png";

  ngOnInit() {
    
    this.player = videojs({
      "autoplay": false,
      "preload": "none",
      "techOrder": ["flash", "html5"]
    });

    this.player.src({
      "type": "application/x-mpegURL", 
      "src": this.stream
    });

    this.player.on("error", function(t: any) {
      console.log(t);
    });

  }

}