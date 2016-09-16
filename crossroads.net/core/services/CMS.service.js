(function() {
  'use strict';

  module.exports = CMSService;

  CMSService.$inject = ['$http'];

  function CMSService($http) {

    var CMSService = {};

    CMSService.url = `${__CMS_ENDPOINT__}api`;
    
    CMSService.getCurrentSeries = function() {

      let todaysDate = moment().format('YYYY-MM-DD');

      let currentSeriesAPIAddress = `${this.url}/series?endDate__GreaterThanOrEqual=${todaysDate}&endDate__sort=ASC`
      return $http.get(encodeURI(currentSeriesAPIAddress))
        .then((response) => {
          let currentSeries;
          let allActiveSeries = response.data.series;

          allActiveSeries.some(series => {
            if (new Date(series.startDate).getTime() <= todaysDate.getTime()) {
              currentSeries = series;
              return true;
            }
          })

          if ( currentSeries === undefined ) {
            allActiveSeries.sort(this.dateSortMethod);
            currentSeries = allActiveSeries[0];
          }
          
          return currentSeries;

      });
    }

    CMSService.getNearestSeries = function() {
      let todaysDate = moment().format('YYYY-MM-DD');
      let nearestSeriesAPIAddress = `${this.url}/series?startDate__GreaterThanOrEqual=${todaysDate}&startDate__sort=ASC&__limit[]=1`
      return $http.get(encodeURI(nearestSeriesAPIAddress))
              .then((response) => {
                console.log('getNearestSeries', response);
              })
    }

    CMSService.getLastSeries = function() {
      let todaysDate = moment().format('YYYY-MM-DD');
      let nearestSeriesAPIAddress = `${this.url}/series?endDate__LessThanOrEqual=${todaysDate}&endDate__sort=DESC&__limit[]=1`
      return $http.get(encodeURI(nearestSeriesAPIAddress))
              .then((response) => {
                return _.first(response.data.series);
              })
    }

    CMSService.getSeries = function(query) {
      return $http.get(encodeURI(`${this.url}/series?${queryString}`))
              .then(rsp => {return rsp.data.series})
    }

    CMSService.getDigitalProgram = function() {
      return $http.get(encodeURI(`${this.url}/features`))
              .then(rsp => {return rsp.json().features});
    }

    CMSService.getContentBlock = function(query) {
        return $http.get(encodeURI(`${this.url}/contentblock?${query}`))
                .then(rsp => {return _.first(rsp.data.contentblocks)});
    }


    return CMSService;
  }
})();
