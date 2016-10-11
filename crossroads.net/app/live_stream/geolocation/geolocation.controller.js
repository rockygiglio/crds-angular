import Geolocation from '../models/geolocation';
import CONSTANTS from '../../constants';

export default class GeolocationController {
  constructor(GeolocationService) {
    this.locationService = GeolocationService;
    
    this.count   = 0;
    this.subject = 'people';
    this.verb    = 'are';

    this.isLocating     = false;
    this.locationError  = false;
    this.dismiss        = false;

    this.location = this.locationService.getLocation() || Geolocation.blank();
  }

  add() {
    let count = this.location.count;
    count += 1;
    this.setCount(count);
  }

  subtract() {
    let count = this.location.count;
    if (count > 0) {
      count -= 1;
    }

    this.setCount(count);
  }

  submitEnabled() {
    return this.location.count > 0 || this.location.zipcode !== '';
  }

  setCount(count) {
    this.location.count = count;

    if (this.location.count === 1) {
      this.subject = 'person';
      this.verb    = 'is'
    } else {
      this.subject = 'people';
      this.verb    = 'are';
    }
  }

  /*************************
   * LOCATION FUNCTIONALITY
   *************************/
  requestLocation() {
    if (this.isLocating === false) {
      this.location.zipcode = '';
      this.isLocating = true;
      let options = {
        enableHighAccuracy: true,
        timeout: 50000,
        maximumAge: 0
      }
      navigator.geolocation.getCurrentPosition(this.getCoords.bind(this), this.setLocationError.bind(this), options);

    }
  }

  getCoords(position) {
    this.location.lat = position.coords.latitude;
    this.location.lng = position.coords.longitude;

    // set zip code
    this.locationService.retrieveZipcode(this.location.lat, this.location.lng).then((result) => {
      this.isLocating = false;
      this.location.zipcode = result;
    }, (error) => {
      this.setLocationError(error);
    })
  }

  setLocationError(error) {
    this.isLocating    = false;
    this.locationError = true;
  }

  /***********************
   * FORM FUNCTIONALITY
   ***********************/
  submit() {
    this.locationService.saveLocation(this.location);
    this.success = true; 
    setTimeout(() => {
      this.dismiss = true;
      this.locationService.success();
    }, CONSTANTS.GEOLOCATION.MODAL_TIMEOUT);
  }

  dismissed() {
    this.locationService.dismissed();
  }
}