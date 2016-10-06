
import constants from 'crds-constants';
import StreamspotPlayerController from '../../../app/live_stream/streamspot_player/streamspotPlayer.controller';
import StreamspotService from '../../../app/live_stream/services/streamspot.service';

describe('Streampost player', () => {
  let fixture,
      response,
      rootScope,
      StreamspotService;

  beforeEach(angular.mock.module(constants.MODULES.LIVE_STREAM));

  beforeEach(inject(function ($injector) {
    StreamspotService = $injector.get('StreamspotService');
    rootScope = $injector.get('$rootScope');
    fixture = new StreamspotPlayerController(StreamspotService, rootScope);

    response = {
      "success": true,
      "data": {
        "broadcaster": {
          "churchname": "crossr30e3",
          "ssId": "crossr30e3",
          "apiKey": "a0cb38cb-8146-47c2-b11f-6d93f4647389",
          "name": "Crossroads Non-public",
          "origin": 10,
          "isHighGround": 0,
          "idHash": "db32d723",
          "isAutoStreamer": 1,
          "isSuspended": 0,
          "isBroadcasting": true,
          "timezone": "Eastern",
          "streamPlan": "pro1",
          "billing_start": "2015-01-29",
          "nextBill": "2016-10-24",
          "stream_pw": "golive1",
          "origin_url": "origin1.streamspot.com",
          "origin_rtmp": "rtmp://origin1.streamspot.com/live",
          "stream_name": "mp4:crossr30e3",
          "is_chat": 0,
          "isDonate": 0,
          "isRoku": 0,
          "edge": 0,
          "donate": null,
          "description": "",
          "isMBR": 1,
          "is247": 0,
          "playerLimit": 1500,
          "maxConnections": 499,
          "timezoneSet": "America/New_York",
          "venueDescription": "",
          "live_src": {
            "cdn_hls": "https://limelight1.streamspot.com/dvr/smil:crossr30e3.smil/playlist.m3u8",
            "hls": "http://edge.streamspot.com:1935/live10/smil:crossr30e3.smil/playlist.m3u8",
            "cdn_mpd": "https://limelight1.streamspot.com/dvr/smil:crossr30e3.smil/manifest.mpd",
            "mpd": "http://edge.streamspot.com:1935/live10/smil:crossr30e3.smil/manifest.mpd",
            "smil": "http://edge.streamspot.com:1935/live10/smil:crossr30e3.smil/medialist.smil",
            "rtmp_url": "rtmp://edge.streamspot.com/live10",
            "rtmp": "crossr30e3"
          },
          "bitrates": [
            {
              "id": 27,
              "ssId": "crossr30e3",
              "bitrate": "300000",
              "width": "320",
              "file": "crossr30e3_mbr1",
              "created": "2016-01-29 17:16:36",
              "height": "180",
              "resolution": "180p",
              "src": "http://edge.streamspot.com:1935/live10/crossr30e3_mbr1/playlist.m3u8",
              "cdn_src": "https://limelight1.streamspot.com/dvr/crossr30e3_mbr1/playlist.m3u8"
            },
            {
              "id": 60,
              "ssId": "crossr30e3",
              "bitrate": "700000",
              "width": "640",
              "file": "crossr30e3_mbr4",
              "created": "2016-01-29 17:16:36",
              "height": "360",
              "resolution": "360p",
              "src": "http://edge.streamspot.com:1935/live10/crossr30e3_mbr4/playlist.m3u8",
              "cdn_src": "https://limelight1.streamspot.com/dvr/crossr30e3_mbr4/playlist.m3u8"
            },
            {
              "id": 87,
              "ssId": "crossr30e3",
              "bitrate": "1300000",
              "width": "854",
              "file": "crossr30e3_mbr3",
              "created": "2016-01-29 17:16:36",
              "height": "480",
              "resolution": "480p",
              "src": "http://edge.streamspot.com:1935/live10/crossr30e3_mbr3/playlist.m3u8",
              "cdn_src": "https://limelight1.streamspot.com/dvr/crossr30e3_mbr3/playlist.m3u8"
            },
            {
              "id": 90,
              "ssId": "crossr30e3",
              "bitrate": "3000000",
              "width": "1920",
              "file": "crossr30e3_mbr2",
              "created": "2016-01-29 17:16:36",
              "height": "1080",
              "resolution": "1080p",
              "src": "http://edge.streamspot.com:1935/live10/crossr30e3_mbr2/playlist.m3u8",
              "cdn_src": "https://limelight1.streamspot.com/dvr/crossr30e3_mbr2/playlist.m3u8"
            }
          ],
          "players": [
            {
              "thumbImage": null,
              "bgImage": "crossr30e3_1f2886e6_thelanding_logojpg_resized.jpg",
              "ssId": "crossr30e3",
              "playerName": "The Landing",
              "playerDescription": "Default Player for Crossroads Oakley: Non-Public Account",
              "playerWidth": 640,
              "playerHeight": 360,
              "default": true,
              "thumbLink": null,
              "bgLink": "https://d2i0qcc2ysg3s9.cloudfront.net/crossr30e3_1f2886e6_thelanding_logojpg_resized.jpg"
            },
            {
              "thumbImage": "default-thumb-player.png",
              "bgImage": "streamspot_splash_player.jpg",
              "ssId": "crossr30e3",
              "playerName": "Weekly Connect",
              "playerDescription": "Custom Player for Crossroads Non-public",
              "playerWidth": 640,
              "playerHeight": 360,
              "default": false,
              "thumbLink": "https://d2i0qcc2ysg3s9.cloudfront.net/default-thumb-player.png",
              "bgLink": "https://d2i0qcc2ysg3s9.cloudfront.net/streamspot_splash_player.jpg"
            }
          ]
        }
      }
    }

  }));

  it('should set the iframe src', () => {
    let id = '1adb55de';
    if ( fixture.streamspotService.ssid === 'crossr30e3' ) {
      id = '2887fba1';
    }

    let src = fixture.setPlayerSrc(response.data.broadcaster);

    expect(src).toBe(`https://player2.streamspot.com/?playerId=${id}`)
  });
})