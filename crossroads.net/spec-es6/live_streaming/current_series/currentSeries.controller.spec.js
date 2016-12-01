
import constants from '../../../app/constants';
import CurrentSeriesController from '../../../app/live_stream/current_series/currentSeries.controller';

describe('Current Series Controller', () => {
  let fixture;
  let modal;
  let cmsService;
  let responsiveImageService;

  const series = {
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

  beforeEach(inject(($injector) => {
    modal = $injector.get('$modal');
    cmsService = $injector.get('CMSService');
    responsiveImageService = $injector.get('ResponsiveImageService');
    fixture = new CurrentSeriesController(cmsService, modal, responsiveImageService);
  }));

  it('should correctly parse results', () => {
    fixture.parseData(series);

    expect(fixture.title).toBe('Hello World');
    expect(fixture.runningDates).toBe('RUNS: August 1st - August 30th');
    expect(fixture.tags).toEqual(['some tag title 1', 'some tag title 2']);
    expect(fixture.embed).toEqual('https://www.youtube.com/embed/h1Lfd1aB9YI?rel=0');
  });
})
;
