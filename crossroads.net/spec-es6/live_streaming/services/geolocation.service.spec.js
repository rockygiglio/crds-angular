import CONSTANTS from 'crds-constants';
import Geolocation from '../../../app/live_stream/models/geolocation'
import GeolocationService from '../../../app/live_stream/services/geolocation.service';
// import GoogleMapsService from '../../../app/services/google_maps.service';
// import MockGoogleMapsService from '../mocks/google_maps.service';

describe('Geolocation Service', () => {
  let service,
      q,
      location,
      rootScope,
      mapService;

  beforeEach(angular.mock.module(CONSTANTS.MODULES.LIVE_STREAM));

  beforeEach(inject(function($injector) {
    q          = $injector.get('$q');
    rootScope  = $injector.get('$rootScope');
    mapService = $injector.get('GoogleMapsService');

    service = new GeolocationService(q, rootScope, mapService);

    location = Geolocation.build({
      lat: '39.1603615',
      lng: '-84.4230026',
      count: 1,
      zipcode: '45209'
    });

    localStorage.removeItem('crds-geolocation');
  }))

  afterEach(() => {
    localStorage.removeItem('crds-geolocation');
  });

  describe('Modal and Banner Logic', () => {
    it('should show modal if not answered before', () => {
      expect(service.showModal()).toBe(true);
    })

    it('should not show modal if answered before', () => {
      localStorage.setItem('crds-geolocation', JSON.stringify(location));

      expect(service.showModal()).toBe(false);
    })

    it('should show the banner if answered before', () => {
      localStorage.setItem('crds-geolocation', JSON.stringify(location));
      
      expect(service.showBanner()).toBe(true);
    })

    it('should show the banner if modal dismissed', () => {
      service.modalDismissed = true;

      expect(service.showBanner()).toBe(true);

    })

    it('should hide banner if answered in modal', () => {
      expect(service.showModal()).toBe(true);
      service.saveLocation(location);
      expect(service.showBanner()).toBe(false);
    })

    it('should show banner but not modal if previously answered', () => {
      localStorage.setItem('crds-geolocation', JSON.stringify(location));
      expect(service.showModal()).toBe(false);
      expect(service.showBanner()).toBe(true);
    })
  });

  describe('Retrieve and Store location', () => {
    it('should retrieve location data from local storage', () => {
      service.saveLocation(location);

      expect(service.getLocation()).toEqual(location);
    })

    it('should null if no location stored', () => {
      expect(service.getLocation()).toBe(null);
    });

    it('should return zipcode for lat/long', () => {
      spyOn(mapService, 'reverseGeocode').and.callFake((lat, lng) => {
        let deferred = q.defer();
        let result = [
          {
            "address_components": [
              {
                "long_name": "316",
                "short_name": "316",
                "types": [
                  "street_number"
                ]
              },
              {
                "long_name": "Ridgewood Place",
                "short_name": "Ridgewood Pl",
                "types": [
                  "route"
                ]
              },
              {
                "long_name": "Fort Thomas",
                "short_name": "Fort Thomas",
                "types": [
                  "locality",
                  "political"
                ]
              },
              {
                "long_name": "Campbell County",
                "short_name": "Campbell County",
                "types": [
                  "administrative_area_level_2",
                  "political"
                ]
              },
              {
                "long_name": "Kentucky",
                "short_name": "KY",
                "types": [
                  "administrative_area_level_1",
                  "political"
                ]
              },
              {
                "long_name": "United States",
                "short_name": "US",
                "types": [
                  "country",
                  "political"
                ]
              },
              {
                "long_name": "45209",
                "short_name": "45209",
                "types": [
                  "postal_code"
                ]
              }
            ],
            "formatted_address": "316 Ridgewood Pl, Fort Thomas, KY 41075, USA",
            "geometry": {
              "location": {
                "lat": 39.07458570000001,
                "lng": -84.45914979999998
              },
              "location_type": "ROOFTOP",
              "viewport": {
                "south": 39.07323671970851,
                "west": -84.46049878029152,
                "north": 39.07593468029151,
                "east": -84.4578008197085
              }
            },
            "place_id": "ChIJOcVHAsixQYgRehO6cWRCxxU",
            "types": [
              "street_address"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "Fort Thomas",
                "short_name": "Fort Thomas",
                "types": [
                  "locality",
                  "political"
                ]
              },
              {
                "long_name": "Campbell County",
                "short_name": "Campbell County",
                "types": [
                  "administrative_area_level_2",
                  "political"
                ]
              },
              {
                "long_name": "Kentucky",
                "short_name": "KY",
                "types": [
                  "administrative_area_level_1",
                  "political"
                ]
              },
              {
                "long_name": "United States",
                "short_name": "US",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Fort Thomas, KY, USA",
            "geometry": {
              "bounds": {
                "south": 39.0508049,
                "west": -84.4748444,
                "north": 39.1113341,
                "east": -84.42853400000001
              },
              "location": {
                "lat": 39.07506070000001,
                "lng": -84.4471633
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 39.0508049,
                "west": -84.4748444,
                "north": 39.1113341,
                "east": -84.42853400000001
              }
            },
            "place_id": "ChIJM_Dc4cKxQYgRd__8oMYrUU0",
            "types": [
              "locality",
              "political"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "41075",
                "short_name": "41075",
                "types": [
                  "postal_code"
                ]
              },
              {
                "long_name": "Fort Thomas",
                "short_name": "Fort Thomas",
                "types": [
                  "locality",
                  "political"
                ]
              },
              {
                "long_name": "Campbell County",
                "short_name": "Campbell County",
                "types": [
                  "administrative_area_level_2",
                  "political"
                ]
              },
              {
                "long_name": "Kentucky",
                "short_name": "KY",
                "types": [
                  "administrative_area_level_1",
                  "political"
                ]
              },
              {
                "long_name": "United States",
                "short_name": "US",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Fort Thomas, KY 41075, USA",
            "geometry": {
              "bounds": {
                "south": 39.046681,
                "west": -84.47467110000002,
                "north": 39.111334,
                "east": -84.42955799999999
              },
              "location": {
                "lat": 39.0799481,
                "lng": -84.45402130000002
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 39.046681,
                "west": -84.47467110000002,
                "north": 39.111334,
                "east": -84.42955799999999
              }
            },
            "place_id": "ChIJCXb1hsOxQYgRxDRBpigwM7g",
            "postcode_localities": [
              "Fort Thomas",
              "Kenton Vale",
              "Newport"
            ],
            "types": [
              "postal_code"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "Campbell County",
                "short_name": "Campbell County",
                "types": [
                  "administrative_area_level_2",
                  "political"
                ]
              },
              {
                "long_name": "Kentucky",
                "short_name": "KY",
                "types": [
                  "administrative_area_level_1",
                  "political"
                ]
              },
              {
                "long_name": "United States",
                "short_name": "US",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Campbell County, KY, USA",
            "geometry": {
              "bounds": {
                "south": 38.805667,
                "west": -84.50610599999999,
                "north": 39.1217619,
                "east": -84.23191500000001
              },
              "location": {
                "lat": 38.8951891,
                "lng": -84.3962535
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 38.805667,
                "west": -84.50610599999999,
                "north": 39.1217619,
                "east": -84.23191500000001
              }
            },
            "place_id": "ChIJvTJkb5WkQYgR2Z8MxRfWzgU",
            "types": [
              "administrative_area_level_2",
              "political"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "Cincinnati-Middletown, OH-KY-IN",
                "short_name": "Cincinnati-Middletown, OH-KY-IN",
                "types": [
                  "political"
                ]
              },
              {
                "long_name": "United States",
                "short_name": "US",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Cincinnati-Middletown, OH-KY-IN, USA",
            "geometry": {
              "bounds": {
                "south": 38.472983,
                "west": -85.298699,
                "north": 39.5900575,
                "east": -83.67301800000001
              },
              "location": {
                "lat": 39.0220536,
                "lng": -84.4382721
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 38.472983,
                "west": -85.298699,
                "north": 39.5900575,
                "east": -83.67301800000001
              }
            },
            "place_id": "ChIJiXZ_sr6xQYgRr578Yjq4WJo",
            "types": [
              "political"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "Kentucky",
                "short_name": "KY",
                "types": [
                  "administrative_area_level_1",
                  "political"
                ]
              },
              {
                "long_name": "United States",
                "short_name": "US",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Kentucky, USA",
            "geometry": {
              "bounds": {
                "south": 36.497129,
                "west": -89.57150890000003,
                "north": 39.1474581,
                "east": -81.96497090000003
              },
              "location": {
                "lat": 37.8393332,
                "lng": -84.27001789999997
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 36.497129,
                "west": -89.57150890000003,
                "north": 39.1474581,
                "east": -81.96497090000003
              }
            },
            "place_id": "ChIJyVMZi0xzQogR_N_MxU5vH3c",
            "types": [
              "administrative_area_level_1",
              "political"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "United States",
                "short_name": "US",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "United States",
            "geometry": {
              "bounds": {
                "south": 18.910677,
                "west": 172.4458955,
                "north": 71.3867745,
                "east": -66.95028609999997
              },
              "location": {
                "lat": 37.09024,
                "lng": -95.71289100000001
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 25.82,
                "west": -124.38999999999999,
                "north": 49.38,
                "east": -66.94
              }
            },
            "place_id": "ChIJCzYy5IS16lQRQrfeQ5K5Oxw",
            "types": [
              "country",
              "political"
            ]
          }
        ];
        deferred.resolve(result);
        return deferred.promise;
      });

      let foundLocation = Geolocation.blank();
      service.retrieveZipcode(location).then((result) => {
        foundLocation = result;
      })
      rootScope.$digest();
      expect(foundLocation.zipcode).toEqual(location.zipcode);
    });

    it('should return "outside US" for lat/long outside US', () => {
      spyOn(mapService, 'reverseGeocode').and.callFake((lat, lng) => {
        let deferred = q.defer();
        let result = [
          {
            "address_components": [
              {
                "long_name": "316",
                "short_name": "316",
                "types": [
                  "street_number"
                ]
              },
              {
                "long_name": "Ridgewood Place",
                "short_name": "Ridgewood Pl",
                "types": [
                  "route"
                ]
              },
              {
                "long_name": "Fort Thomas",
                "short_name": "Fort Thomas",
                "types": [
                  "locality",
                  "political"
                ]
              },
              {
                "long_name": "Campbell County",
                "short_name": "Campbell County",
                "types": [
                  "administrative_area_level_2",
                  "political"
                ]
              },
              {
                "long_name": "Kentucky",
                "short_name": "KY",
                "types": [
                  "administrative_area_level_1",
                  "political"
                ]
              },
              {
                "long_name": "Germany",
                "short_name": "DE",
                "types": [
                  "country",
                  "political"
                ]
              },
              {
                "long_name": "45209",
                "short_name": "45209",
                "types": [
                  "postal_code"
                ]
              }
            ],
            "formatted_address": "316 Ridgewood Pl, Fort Thomas, KY 41075, USA",
            "geometry": {
              "location": {
                "lat": 39.07458570000001,
                "lng": -84.45914979999998
              },
              "location_type": "ROOFTOP",
              "viewport": {
                "south": 39.07323671970851,
                "west": -84.46049878029152,
                "north": 39.07593468029151,
                "east": -84.4578008197085
              }
            },
            "place_id": "ChIJOcVHAsixQYgRehO6cWRCxxU",
            "types": [
              "street_address"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "Fort Thomas",
                "short_name": "Fort Thomas",
                "types": [
                  "locality",
                  "political"
                ]
              },
              {
                "long_name": "Campbell County",
                "short_name": "Campbell County",
                "types": [
                  "administrative_area_level_2",
                  "political"
                ]
              },
              {
                "long_name": "Kentucky",
                "short_name": "KY",
                "types": [
                  "administrative_area_level_1",
                  "political"
                ]
              },
              {
                "long_name": "Germany",
                "short_name": "DE",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Fort Thomas, KY, USA",
            "geometry": {
              "bounds": {
                "south": 39.0508049,
                "west": -84.4748444,
                "north": 39.1113341,
                "east": -84.42853400000001
              },
              "location": {
                "lat": 39.07506070000001,
                "lng": -84.4471633
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 39.0508049,
                "west": -84.4748444,
                "north": 39.1113341,
                "east": -84.42853400000001
              }
            },
            "place_id": "ChIJM_Dc4cKxQYgRd__8oMYrUU0",
            "types": [
              "locality",
              "political"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "41075",
                "short_name": "41075",
                "types": [
                  "postal_code"
                ]
              },
              {
                "long_name": "Fort Thomas",
                "short_name": "Fort Thomas",
                "types": [
                  "locality",
                  "political"
                ]
              },
              {
                "long_name": "Campbell County",
                "short_name": "Campbell County",
                "types": [
                  "administrative_area_level_2",
                  "political"
                ]
              },
              {
                "long_name": "Kentucky",
                "short_name": "KY",
                "types": [
                  "administrative_area_level_1",
                  "political"
                ]
              },
              {
                "long_name": "Germany",
                "short_name": "DE",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Fort Thomas, KY 41075, USA",
            "geometry": {
              "bounds": {
                "south": 39.046681,
                "west": -84.47467110000002,
                "north": 39.111334,
                "east": -84.42955799999999
              },
              "location": {
                "lat": 39.0799481,
                "lng": -84.45402130000002
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 39.046681,
                "west": -84.47467110000002,
                "north": 39.111334,
                "east": -84.42955799999999
              }
            },
            "place_id": "ChIJCXb1hsOxQYgRxDRBpigwM7g",
            "postcode_localities": [
              "Fort Thomas",
              "Kenton Vale",
              "Newport"
            ],
            "types": [
              "postal_code"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "Campbell County",
                "short_name": "Campbell County",
                "types": [
                  "administrative_area_level_2",
                  "political"
                ]
              },
              {
                "long_name": "Kentucky",
                "short_name": "KY",
                "types": [
                  "administrative_area_level_1",
                  "political"
                ]
              },
              {
                "long_name": "Germany",
                "short_name": "DE",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Campbell County, KY, USA",
            "geometry": {
              "bounds": {
                "south": 38.805667,
                "west": -84.50610599999999,
                "north": 39.1217619,
                "east": -84.23191500000001
              },
              "location": {
                "lat": 38.8951891,
                "lng": -84.3962535
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 38.805667,
                "west": -84.50610599999999,
                "north": 39.1217619,
                "east": -84.23191500000001
              }
            },
            "place_id": "ChIJvTJkb5WkQYgR2Z8MxRfWzgU",
            "types": [
              "administrative_area_level_2",
              "political"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "Cincinnati-Middletown, OH-KY-IN",
                "short_name": "Cincinnati-Middletown, OH-KY-IN",
                "types": [
                  "political"
                ]
              },
              {
                "long_name": "Germany",
                "short_name": "DE",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Cincinnati-Middletown, OH-KY-IN, USA",
            "geometry": {
              "bounds": {
                "south": 38.472983,
                "west": -85.298699,
                "north": 39.5900575,
                "east": -83.67301800000001
              },
              "location": {
                "lat": 39.0220536,
                "lng": -84.4382721
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 38.472983,
                "west": -85.298699,
                "north": 39.5900575,
                "east": -83.67301800000001
              }
            },
            "place_id": "ChIJiXZ_sr6xQYgRr578Yjq4WJo",
            "types": [
              "political"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "Kentucky",
                "short_name": "KY",
                "types": [
                  "administrative_area_level_1",
                  "political"
                ]
              },
              {
                "long_name": "Germany",
                "short_name": "DE",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Kentucky, USA",
            "geometry": {
              "bounds": {
                "south": 36.497129,
                "west": -89.57150890000003,
                "north": 39.1474581,
                "east": -81.96497090000003
              },
              "location": {
                "lat": 37.8393332,
                "lng": -84.27001789999997
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 36.497129,
                "west": -89.57150890000003,
                "north": 39.1474581,
                "east": -81.96497090000003
              }
            },
            "place_id": "ChIJyVMZi0xzQogR_N_MxU5vH3c",
            "types": [
              "administrative_area_level_1",
              "political"
            ]
          },
          {
            "address_components": [
              {
                "long_name": "Germany",
                "short_name": "US",
                "types": [
                  "country",
                  "political"
                ]
              }
            ],
            "formatted_address": "Germany",
            "geometry": {
              "bounds": {
                "south": 18.910677,
                "west": 172.4458955,
                "north": 71.3867745,
                "east": -66.95028609999997
              },
              "location": {
                "lat": 37.09024,
                "lng": -95.71289100000001
              },
              "location_type": "APPROXIMATE",
              "viewport": {
                "south": 25.82,
                "west": -124.38999999999999,
                "north": 49.38,
                "east": -66.94
              }
            },
            "place_id": "ChIJCzYy5IS16lQRQrfeQ5K5Oxw",
            "types": [
              "country",
              "political"
            ]
          }
        ];
        deferred.resolve(result);
        return deferred.promise;
      });

      let foundLocation = Geolocation.blank();
      service.retrieveZipcode(location).then((result) => {
        foundLocation = result;
      })
      rootScope.$digest();
      expect(foundLocation.zipcode).toEqual('outside US');
    });
  });


});