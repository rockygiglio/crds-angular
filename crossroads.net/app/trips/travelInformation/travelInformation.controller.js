var attributeTypes = require('crds-constants').ATTRIBUTE_TYPE_IDS;
var attributeIds = require('crds-constants').ATTRIBUTE_IDS;

export default class TravelInformationController {
  /* @ngInject() */
  constructor() {
    this.destination = null;
    this.frequentFlyers = null;
  }

  $onInit() {
    this.frequentFlyers = [
      {
        "attributeId": 3958,
        "name": "Delta Airlines",
        "description": null,
        "selected": false,
        "startDate": "0001-01-01T00:00:00",
        "endDate": null,
        "notes": null,
        "sortOrder": 0,
        "category": null,
        "categoryId": null,
        "categoryDescription": null,
        "attributeTypeId": null
      },
      {
        "attributeId": 3959,
        "name": "South Africa Airlines",
        "description": null,
        "selected": false,
        "startDate": "0001-01-01T00:00:00",
        "endDate": null,
        "notes": null,
        "sortOrder": 0,
        "category": null,
        "categoryId": null,
        "categoryDescription": null,
        "attributeTypeId": null
      },
      {
        "attributeId": 4623,
        "name": "Southwest",
        "description": null,
        "selected": false,
        "startDate": "0001-01-01T00:00:00",
        "endDate": null,
        "notes": null,
        "sortOrder": 0,
        "category": null,
        "categoryId": null,
        "categoryDescription": null,
        "attributeTypeId": null
      },
      {
        "attributeId": 3960,
        "name": "United Airlines",
        "description": null,
        "selected": false,
        "startDate": "0001-01-01T00:00:00",
        "endDate": null,
        "notes": null,
        "sortOrder": 0,
        "category": null,
        "categoryId": null,
        "categoryDescription": null,
        "attributeTypeId": null
      },
      {
        "attributeId": 3980,
        "name": "US Airways",
        "description": null,
        "selected": false,
        "startDate": "0001-01-01T00:00:00",
        "endDate": null,
        "notes": null,
        "sortOrder": 0,
        "category": null,
        "categoryId": null,
        "categoryDescription": null,
        "attributeTypeId": null
      }
    ];
  }

  showFrequentFlyer(airline) {
    console.log('SHOW FREQUENT FLYER', airline.attributeId);
    if (airline.attributeId === attributeIds.SOUTHAFRICA_FREQUENT_FLYER) {
      if (this.isSouthAfrica()) {
        return true;
      }
      return false;
    }

    if (airline.attributeId === attributeIds.US_FREQUENT_FLYER) {
      if (this.isNica()) {
        return true;
      }

      return false;
    }

    return true;
  }

  isNica() {
    if (this.destination === 'Nicaragua') {
      return true;
    }

    return false;
  }

  isSouthAfrica() {
    if (this.destination === 'South Africa') {
      return true;
    }

    return false;
  }
}
