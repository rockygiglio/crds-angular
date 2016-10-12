
import constants from 'crds-constants';
import CurrentSeriesController from '../../../app/live_stream/current_series/currentSeries.controller';
import CMSService from '../../../core/services/CMS.service';

describe('Current Series Controller', () => {
  let fixture,
      series,
      modal,
      cmsService,
      httpBackend;

  const reminderEndpoint = `${__API_ENDPOINT__}`;

  series = {
    title: 'Hello World',
    description: 'This is my hello world test',
    picture: 'hello_world.jpg',
    startDate: '2016-08-01',
    endDate: '2016-08-30',
    trailerLink: 'https://www.youtube.com/watch?v=h1Lfd1aB9YI',
    image: {
      filename: 'c:/stupidfile.jpg'
    },
    tags: [
      {
          title: 'some tag title 1'
      },
      {
          title: 'some tag title 2'
      }
    ]
  };

  beforeEach(angular.mock.module(constants.MODULES.LIVE_STREAM));

  beforeEach(inject(function ($injector) {
    modal       = $injector.get('$modal');
    httpBackend = $injector.get('$httpBackend');
    cmsService  = $injector.get('CMSService');
    fixture = new CurrentSeriesController(cmsService, modal);

  }));

  it('should correctly parse results', () => {
    fixture.parseData(series);

    expect(fixture.title).toBe('Hello World');
    expect(fixture.runningDates).toBe('RUNS: August 1st - August 30th');
    expect(fixture.tags).toEqual(['some tag title 1','some tag title 2']);
    expect(fixture.embed).toEqual('https://www.youtube.com/embed/h1Lfd1aB9YI');
  })

})