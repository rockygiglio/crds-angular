
import constants from 'crds-constants';
import StreamController from '../../../app/live_stream/stream/stream.controller';
import StreamspotService from '../../../app/live_stream/services/streamspot.service';
import CMSService from '../../../core/services/CMS.service';

describe('Stream Controller', () => {
  let fixture,
      rootScope,
      cmsService,
      location,
      streamspotService,
      httpBackend,
      results;

  beforeEach(angular.mock.module(constants.MODULES.LIVE_STREAM));

  beforeEach(inject(function ($injector) {
    StreamspotService = $injector.get('StreamspotService');
    rootScope = $injector.get('$rootScope');
    cmsService = $injector.get('CMSService');
    location = $injector.get('$location');
    fixture = new StreamController(cmsService, StreamspotService, rootScope, location);

    results = [
      {
        "id": 7,
        "title": "Be a Giver",
        "description": "<p><span>Everything we're experiencing today is funded by a team of people just </span><span>like you who believe in investing in your growth and changing the world</span><span>. You can join the team and</span><span> start funding today. </span></p><p><a class=\"btn btn-primary\" href=\"http://crossroads.net/give\" target=\"_blank\">Give</a></p>",
        "status": null,
        "link": null,
        "version": "8",
        "image": {
          "id": 18287,
          "name": "Screen-Shot-2016-08-26-at-11.30.39-AM.png",
          "title": "Screen Shot 2016 08 26 at 11.30.39 AM",
          "filename": "https://s3.amazonaws.com/crds-cms-uploads/Uploads/Screen-Shot-2016-08-26-at-11.30.39-AM.png",
          "content": null,
          "showInSearch": "1",
          "cloudStatus": "Live",
          "cloudSize": "696743",
          "cloudMetaJson": "{\"Dimensions\":\"1428x778\",\"LastPut\":1472225426}",
          "parent": 602,
          "owner": 72,
          "derivedImages": [
            2236,
            2238,
            2269
          ],
          "created": "2016-08-26T11:30:24-04:00",
          "className": "CloudImage"
        },
        "section": 2,
        "created": "2016-08-26T13:20:03-04:00",
        "className": "Feature"
      },
      {
        "id": 9,
        "title": "Join a Group",
        "description": "<p>From dude groups on a back porch, Bible studies in the home, family groups on playgrounds, business groups in a conference room or adventure groups out on a trail, God is just waiting to move in our lives wherever we are.</p>",
        "status": null,
        "link": "https://www.crossroads.net/groups",
        "version": "5",
        "image": {
          "id": 18282,
          "name": "Group-TitleSlide-4x3.jpg",
          "title": "Group TitleSlide 4x3",
          "filename": "https://s3.amazonaws.com/crds-cms-uploads/Uploads/Group-TitleSlide-4x3.jpg",
          "content": null,
          "showInSearch": "1",
          "cloudStatus": "Live",
          "cloudSize": "2025104",
          "cloudMetaJson": "{\"Dimensions\":\"3714x2786\",\"LastPut\":1472222514}",
          "parent": 602,
          "owner": 72,
          "derivedImages": [
            2221,
            2223,
            2265
          ],
          "created": "2016-08-26T10:41:52-04:00",
          "className": "CloudImage"
        },
        "section": 1,
        "created": "2016-08-26T13:31:48-04:00",
        "className": "Feature"
      },
      {
        "id": 11,
        "title": "Go Somewhere",
        "description": "<p><span>GO trip signups are live! You can now view all the trips and sign up to GO Somewhere in 2017.</span></p>",
        "status": null,
        "link": "http://crossroadsgo.net/",
        "version": "4",
        "image": {
          "id": 18277,
          "name": "gosomehwere.jpg",
          "title": "gosomehwere",
          "filename": "https://s3.amazonaws.com/crds-cms-uploads/Uploads/gosomehwere.jpg",
          "content": null,
          "showInSearch": "1",
          "cloudStatus": "Live",
          "cloudSize": "31908",
          "cloudMetaJson": "{\"Dimensions\":\"480x480\",\"LastPut\":1472221570}",
          "parent": 602,
          "owner": 72,
          "derivedImages": [
            2217,
            2222,
            2264
          ],
          "created": "2016-08-26T10:26:09-04:00",
          "className": "CloudImage"
        },
        "section": 1,
        "created": "2016-08-26T13:34:20-04:00",
        "className": "Feature"
      },
      {
        "id": 13,
        "title": "Business of Success ",
        "description": "<p>We spend more time working than doing anything else, but we think God doesn't care about the results. That's crazy. God wants us to be successful. He wants us to thrive every day. But we're stuck feeling guilty for success or worried that our work isn't important.</p>",
        "status": null,
        "link": null,
        "version": "6",
        "image": {
          "id": 18332,
          "name": "Screen-Shot-2016-09-03-at-4.29.22-PM.png",
          "title": "Screen Shot 2016 09 03 at 4.29.22 PM",
          "filename": "https://s3.amazonaws.com/crds-cms-uploads/Uploads/Screen-Shot-2016-09-03-at-4.29.22-PM.png",
          "content": null,
          "showInSearch": "1",
          "cloudStatus": "Live",
          "cloudSize": "95876",
          "cloudMetaJson": "{\"Dimensions\":\"440x294\",\"LastPut\":1472934524}",
          "parent": 602,
          "owner": 72,
          "derivedImages": [
            2320
          ],
          "created": "2016-09-03T16:28:42-04:00",
          "className": "CloudImage"
        },
        "created": "2016-09-03T16:20:12-04:00",
        "className": "Feature"
      }
    ]

  }));

  it('process Digital Program in Dont Miss and Be The Church', () => {
    fixture.sortDigitalProgram(results);
    expect(fixture.dontMiss.length).toBe(2);
    expect(_.first(fixture.dontMiss).title).toBe('Join a Group')

    expect(fixture.beTheChurch.length).toBe(1);
    expect(_.first(fixture.beTheChurch).title).toBe('Be a Giver');
  });
})