(function(){
  'use strict';

  module.exports = ResponseService;

  ResponseService.$inject = [];

  function ResponseService() {

    this.data = {};
    this.data = {
      "date_and_time": {
        "ampm": "PM",
        "day": "Wednesday",
        "time": "07:30"
      },
      "description": "asdf asdasdfa asdasdf asdfasdf",
      "gender": "0",
      "goals": "1",
      "group_type": "0",
      "kids": "1",
      "location": {
        "city": "Verona",
        "state": "KY",
        "street": "10031 Manor Ln",
        "zip": "41092"
      },
      "marital_status": "2",
      "open_spots": 5,
      "pets": {
        "0": true,
        "1": true,
        "2": true
      },
      "total_capacity": 7
    }
    ;

    this.clear = function(){
      this.data = {};
    };

    this.getResponse = function(definition) {
      return this.data[definition.key];
    };

  }

})();
