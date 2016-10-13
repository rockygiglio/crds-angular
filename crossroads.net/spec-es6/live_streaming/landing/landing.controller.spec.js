
import constants from 'crds-constants';
import LandingController from '../../../app/live_stream/landing/landing.controller';
import CMSService from '../../../core/services/CMS.service';

describe('Landing Controller', () => {
  let fixture,
      cmsService,
      filter,
      pastWeekends,
      results;

  beforeEach(angular.mock.module('crossroads'));

  beforeEach(inject(function ($injector, $filter) {
    filter = $filter;
    cmsService = $injector.get('CMSService');
    fixture = new LandingController(cmsService, filter);

    pastWeekends = [
      {
        "id": 3814,
        "title": "Know Your Role",
        "description": "<p><span id=\"docs-internal-guid-482c9298-6731-8d52-52ed-721c2d9a4ddf\"><span>God has given us unique skills and expects us to use them in community.</span></span></p>",
        "date": "2016-09-24",
        "version": "5",
        "series": {
          "id": "237",
          "title": "Business of Success",
          "description": "<p dir=\"ltr\"><span>God cares about your success. </span></p><p dir=\"ltr\"><span></span>We spend more time working than doing anything else, but we think God doesn't care about the results. That's crazy. God wants us to be successful. He wants us to thrive every day.  But we're stuck feeling guilty for success or worried that our work isn't important.</p><p dir=\"ltr\">Come join us as we talk about business from God's perspective—the business of success.</p><p><span> </span></p>",
          "startDate": "2016-09-03",
          "endDate": "2016-10-02",
          "trailerLink": "https://www.youtube.com/watch?v=Rv6mKI5aHAM"
        },
        "messageVideo": {
          "id": 5977,
          "containsAdultContent": "0",
          "serviceId": "SYfPXR0MxeA",
          "sourcePath": null,
          "source": {
            "id": 18471,
            "name": "bos-wk-04.mp4",
            "title": "bos wk 04",
            "filename": "https://s3.amazonaws.com/crds-cms-uploads/media/messages/video/bos-wk-04.mp4",
            "content": null,
            "showInSearch": "1",
            "cloudStatus": "Live",
            "cloudSize": "2011672794",
            "cloudMetaJson": "{\"LastPut\":1474904869}",
            "parent": 827,
            "owner": 68,
            "created": "2016-09-26T11:36:21-04:00",
            "className": "CloudFile"
          },
          "still": {
            "id": 18470,
            "name": "bos-wk-04-still.jpg",
            "title": "bos wk 04 still",
            "filename": "https://crds-cms-uploads.imgix.net/media/messages/stills/bos-wk-04-still.jpg",
            "content": null,
            "showInSearch": "1",
            "cloudStatus": "Live",
            "cloudSize": "418412",
            "cloudMetaJson": "{\"Dimensions\":\"1920x1080\",\"LastPut\":1474904038}",
            "parent": 817,
            "owner": 68,
            "derivedImages": [
              2490,
              2491
            ],
            "created": "2016-09-26T11:33:56-04:00",
            "className": "CloudImage"
          },
          "created": "2016-09-26T11:53:15-04:00",
          "className": "MessageVideo"
        },
        "number": 4
      },
      {
        "id": 3812,
        "title": "The Innovator God",
        "description": "<p><span id=\"docs-internal-guid-482c9298-4343-6c0a-5e3b-7ce5332518ba\"><span>God is constantly innovating and building new things.</span></span></p>",
        "date": "2016-09-17",
        "version": "2",
        "series": {
          "id": "237",
          "title": "Business of Success",
          "description": "<p dir=\"ltr\"><span>God cares about your success. </span></p><p dir=\"ltr\"><span></span>We spend more time working than doing anything else, but we think God doesn't care about the results. That's crazy. God wants us to be successful. He wants us to thrive every day.  But we're stuck feeling guilty for success or worried that our work isn't important.</p><p dir=\"ltr\">Come join us as we talk about business from God's perspective—the business of success.</p><p><span> </span></p>",
          "startDate": "2016-09-03",
          "endDate": "2016-10-02",
          "trailerLink": "https://www.youtube.com/watch?v=Rv6mKI5aHAM"
        },
        "messageVideo": {
          "id": 5974,
          "containsAdultContent": "0",
          "serviceId": "lUR1916Kvc8",
          "sourcePath": null,
          "source": {
            "id": 18437,
            "name": "bos-wk-03.mp4",
            "title": "bos wk 03",
            "filename": "https://s3.amazonaws.com/crds-cms-uploads/media/messages/video/bos-wk-03.mp4",
            "content": null,
            "showInSearch": "1",
            "cloudStatus": "Live",
            "cloudSize": "2025337262",
            "cloudMetaJson": "{\"LastPut\":1474303168}",
            "parent": 827,
            "owner": 68,
            "created": "2016-09-19T12:34:41-04:00",
            "className": "CloudFile"
          },
          "still": {
            "id": 18436,
            "name": "bos-wk-03-still.jpg",
            "title": "bos wk 03 still",
            "filename": "https://crds-cms-uploads.imgix.net/media/messages/stills/bos-wk-03-still.jpg",
            "content": null,
            "showInSearch": "1",
            "cloudStatus": "Live",
            "cloudSize": "691831",
            "cloudMetaJson": "{\"Dimensions\":\"1920x1080\",\"LastPut\":1474302768}",
            "parent": 817,
            "owner": 68,
            "derivedImages": [
              2428,
              2429
            ],
            "created": "2016-09-19T12:32:47-04:00",
            "className": "CloudImage"
          },
          "created": "2016-09-19T12:48:32-04:00",
          "className": "MessageVideo"
        },
        "number": 3
      },
      {
        "id": 3811,
        "title": "God is Pro-Business",
        "description": "<p><span id=\"docs-internal-guid-18577695-1f0b-26c8-dd7c-e6f47013f955\"><span>All good work honors and serves God, not just certain types of it.</span></span></p>",
        "date": "2016-09-10",
        "version": "2",
        "series": {
          "id": "237",
          "title": "Business of Success",
          "description": "<p dir=\"ltr\"><span>God cares about your success. </span></p><p dir=\"ltr\"><span></span>We spend more time working than doing anything else, but we think God doesn't care about the results. That's crazy. God wants us to be successful. He wants us to thrive every day.  But we're stuck feeling guilty for success or worried that our work isn't important.</p><p dir=\"ltr\">Come join us as we talk about business from God's perspective—the business of success.</p><p><span> </span></p>",
          "startDate": "2016-09-03",
          "endDate": "2016-10-02",
          "trailerLink": "https://www.youtube.com/watch?v=Rv6mKI5aHAM"
        },
        "messageVideo": {
          "id": 5972,
          "containsAdultContent": "0",
          "serviceId": "z1DAJKEnkPg",
          "sourcePath": null,
          "source": {
            "id": 18431,
            "name": "bos-wk-02.mp4",
            "title": "bos wk 02",
            "filename": "https://s3.amazonaws.com/crds-cms-uploads/media/messages/video/bos-wk-02.mp4",
            "content": null,
            "showInSearch": "1",
            "cloudStatus": "Live",
            "cloudSize": "1513319569",
            "cloudMetaJson": "{\"LastPut\":1473695067}",
            "parent": 827,
            "owner": 68,
            "created": "2016-09-12T11:38:32-04:00",
            "className": "CloudFile"
          },
          "still": {
            "id": 18430,
            "name": "bos-wk-02-still.jpg",
            "title": "bos wk 02 still",
            "filename": "https://crds-cms-uploads.imgix.net/media/messages/stills/bos-wk-02-still.jpg",
            "content": null,
            "showInSearch": "1",
            "cloudStatus": "Live",
            "cloudSize": "592941",
            "cloudMetaJson": "{\"Dimensions\":\"1920x1080\",\"LastPut\":1473694590}",
            "parent": 817,
            "owner": 68,
            "derivedImages": [
              2414,
              2415
            ],
            "created": "2016-09-12T11:36:28-04:00",
            "className": "CloudImage"
          },
          "created": "2016-09-12T11:36:34-04:00",
          "className": "MessageVideo"
        },
        "number": 2
      },
      {
        "id": 3810,
        "title": "The Business of Success",
        "description": "<p>Chuck Mingo talks about the definition of success.</p>",
        "date": "2016-09-03",
        "version": "2",
        "series": {
          "id": "237",
          "title": "Business of Success",
          "description": "<p dir=\"ltr\"><span>God cares about your success. </span></p><p dir=\"ltr\"><span></span>We spend more time working than doing anything else, but we think God doesn't care about the results. That's crazy. God wants us to be successful. He wants us to thrive every day.  But we're stuck feeling guilty for success or worried that our work isn't important.</p><p dir=\"ltr\">Come join us as we talk about business from God's perspective—the business of success.</p><p><span> </span></p>",
          "startDate": "2016-09-03",
          "endDate": "2016-10-02",
          "trailerLink": "https://www.youtube.com/watch?v=Rv6mKI5aHAM"
        },
        "messageVideo": {
          "id": 5970,
          "containsAdultContent": "0",
          "serviceId": "pCVkj2YvaaM",
          "sourcePath": null,
          "source": {
            "id": 18335,
            "name": "bos-wk-01.mp4",
            "title": "bos wk 01",
            "filename": "https://s3.amazonaws.com/crds-cms-uploads/media/messages/video/bos-wk-01.mp4",
            "content": null,
            "showInSearch": "1",
            "cloudStatus": "Live",
            "cloudSize": "1870190588",
            "cloudMetaJson": "{\"LastPut\":1473187436}",
            "parent": 827,
            "owner": 68,
            "created": "2016-09-06T14:38:35-04:00",
            "className": "CloudFile"
          },
          "still": {
            "id": 18334,
            "name": "bos-wk-01-still.jpg",
            "title": "bos wk 01 still",
            "filename": "https://crds-cms-uploads.imgix.net/media/messages/stills/bos-wk-01-still.jpg",
            "content": null,
            "showInSearch": "1",
            "cloudStatus": "Live",
            "cloudSize": "600475",
            "cloudMetaJson": "{\"Dimensions\":\"1920x1080\",\"LastPut\":1473186960}",
            "parent": 817,
            "owner": 68,
            "derivedImages": [
              2321,
              2322
            ],
            "created": "2016-09-06T14:35:59-04:00",
            "className": "CloudImage"
          },
          "created": "2016-09-06T14:45:51-04:00",
          "className": "MessageVideo"
        },
        "number": 1
      }
    ]

  }));

  it('should process 4 pastWeekends', () => {
    let messages = fixture.parseWeekends(pastWeekends);
    expect(messages.length).toBe(4);
  });

})