export default class GeolocationController {
  constructor(GoogleMapsService) {
    this.locationService = GoogleMapsService;
    // this.service         = GeolocationService;
    // this.modalInstance   = $modalInstance;
    
    this.count = 0;
    this.subject = 'people';
    this.verb    = 'are';
    
    this.options = {
      enableHighAccuracy: true,
      timeout: 50000,
      maximumAge: 0
    }
  }

  close() {
    this.modalInstance.close();
  }

  add() {
    this.count += 1;
    this.setContent();
  }

  subtract() {
    if (this.count > 0) {
      this.count -= 1;
    }

    this.setContent();
  }

  setContent() {
    if (this.count === 1) {
      this.subject = 'person';
      this.verb    = 'is'
    } else {
      this.subject = 'people';
      this.verb    = 'are';
    }
  }

  requestLocation() {
    navigator.geolocation.getCurrentPosition(this.getCoords.bind(this), this.coordError, this.options);
  }

  getCoords(position) {
    let lat = position.coords.latitude,
        lng = position.coords.longitude;

    // set zip code
    this.locationService.retrieveZipcode(lat, lng).then((result) => {
      console.log(result);
    })
  }

  locationError(error) {
    console.error(error)
  }
}