
import Geolocation from '../../../app/live_stream/models/geolocation';

describe('Geolocation Model', () => {
  it('should build a blank location', () => {

    let location = Geolocation.blank();

    expect(location.lat).not.toBeTruthy();
    expect(location.lng).not.toBeTruthy();
    expect(location.count).toBe(1);
    expect(location.zipcode).not.toBeTruthy();
  })

  it('should build a location', () => {
    
    let location = Geolocation.build({
      lat: '0.0',
      lng: '0.0',
      count: 2,
      zipcode: '45202'
    });

    expect(location.lat).toBe('0.0');
    expect(location.lng).toBe('0.0');
    expect(location.count).toBe(2);
    expect(location.zipcode).toBe('45202');
  })


})