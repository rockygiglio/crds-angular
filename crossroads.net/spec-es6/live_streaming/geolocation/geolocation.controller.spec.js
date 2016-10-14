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
    fixture.add();

    expect(fixture.location.count).toBe(1);
    expect(fixture.subject).toBe('person');
    expect(fixture.verb).toBe('is');

    fixture.add();

    expect(fixture.location.count).toBe(2);
    expect(fixture.subject).toBe('people');
    expect(fixture.verb).toBe('are');
  });

  it('should subtract from number of watchers', () => {
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

  it('should not enable submit w/o count or zipcode', () => {
    expect(fixture.submitEnabled()).toBeFalsy();

    fixture.add();
    expect(fixture.submitEnabled()).toBeTruthy();

    fixture.subtract();
    expect(fixture.submitEnabled()).toBeFalsy();

    fixture.location.zipcode = '45202';
    expect(fixture.submitEnabled()).toBeTruthy();
  });

  it('should not submit an invalid zipcode', () => {
    fixture.location.zipcode = '1234asdf';
    fixture.submit();
    expect(fixture.invalidZipcode).toBe(true);
  })


})
