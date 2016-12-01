import constants from 'crds-constants';
import Geolocation from '../../../app/live_stream/models/geolocation'
import GeolocationController from '../../../app/live_stream/geolocation/geolocation.controller';
import GeolocationService from '../../../app/live_stream/services/geolocation.service';

describe('Geolocation Controller', () => {
  let fixture,
      locationService;

  beforeEach(angular.mock.module(constants.MODULES.LIVE_STREAM));

  beforeEach(inject(function ($injector) {
    locationService  = $injector.get('GeolocationService');
    fixture = new GeolocationController(locationService);

  }));

  it('should add to number of watchers', () => {

    expect(fixture.location.count).toBe(0);
    expect(fixture.subject).toBe('people');
    expect(fixture.verb).toBe('are');

    fixture.add();

    expect(fixture.location.count).toBe(1);
    expect(fixture.subject).toBe('person');
    expect(fixture.verb).toBe('is');

    fixture.add();

    expect(fixture.location.count).toBe(2);
    expect(fixture.subject).toBe('people');
    expect(fixture.verb).toBe('are');
  });

  it('should increment / decrement appropriately', () => {
    fixture.add();
    fixture.add();

    expect(fixture.location.count).toBe(2);

    fixture.subtract();
    expect(fixture.location.count).toBe(1);
    expect(fixture.subject).toBe('person');
    expect(fixture.verb).toBe('is');

    fixture.subtract();
    expect(fixture.location.count).toBe(0);
    expect(fixture.subject).toBe('people');
    expect(fixture.verb).toBe('are');

    fixture.subtract();
    expect(fixture.location.count).toBe(0);
  });

  it('should dismiss w/o count or zipcode', () => {
    fixture.submit();
    expect(fixture.locationService.answered).toBe(true);
    expect(fixture.success).toBe(false);
  });

  it('should not submit an invalid zipcode', () => {
    fixture.location.zipcode = '1234asdf';
    fixture.submit();
    expect(fixture.invalidZipcode).toBe(true);
  })


})
