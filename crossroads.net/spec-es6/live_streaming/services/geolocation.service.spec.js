import CONSTANTS from 'crds-constants';
import Geolocation from '../../../app/live_stream/models/geolocation'
import GeolocationService from '../../../app/live_stream/services/geolocation.service';

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