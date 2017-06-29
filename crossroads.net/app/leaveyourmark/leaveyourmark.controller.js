(function() {
    'use strict';

    module.exports = LeaveYourMarkController;

    LeaveYourMarkController.$inject = [
        '$rootScope',
        '$filter',
        '$log',
        '$state',
        'LeaveYourMark',
        'NgMap'
    ];

    function LeaveYourMarkController(
        $rootScope,
        $filter,
        $log,
        $state,
        LeaveYourMark,
        NgMap
    ) {
        var vm = this;

        vm.currentDay = undefined;
        vm.totalDays = undefined;
        vm.given = undefined;
        vm.committed = undefined;
        vm.givenGercentage = undefined
        vm.notStartedPercent = undefined
        vm.behindPercent = undefined
        vm.onPacePercent = undefined
        vm.aheadPercent = undefined
        vm.completedPercent = undefined
        vm.viewReady = false;
        vm.googleMapsUrl="https://maps.googleapis.com/maps/api/js?key=AIzaSyArKsBK97N0Wi-69x10OL7Sx57Fwlmu6Cs";

        vm.cities = {
          chicago: {population:2714856, position: [41.878113, -87.629798]},
          newyork: {population:8405837, position: [40.714352, -74.005973]},
          losangeles: {population:3857799, position: [34.052234, -118.243684]},
          vancouver: {population:603502, position: [49.25, -123.1]},
        }

        vm.mapStyles =  [{
          'elementType': 'geometry',
          'stylers': [
            {
              'color': '#f5f5f5'
            }
          ]
        },
        {
          'elementType': 'labels.icon',
          'stylers': [
            {
              'visibility': 'off'
            }
          ]
        },
        {
          'elementType': 'labels.text.fill',
          'stylers': [
            {
              'color': '#616161'
            }
          ]
        },
        {
          'elementType': 'labels.text.stroke',
          'stylers': [
            {
              'color': '#f5f5f5'
            }
          ]
        },
        {
          'featureType': 'administrative.land_parcel',
          'elementType': 'labels.text.fill',
          'stylers': [
            {
              'color': '#bdbdbd'
            }
          ]
        },
        {
          'featureType': 'administrative.province',
          'elementType': 'geometry.stroke',
          'stylers': [
            {
              'color': '#979797'
            },
            {
              'weight': 1
            }
          ]
        },
        {
          'featureType': 'poi',
          'elementType': 'geometry',
          'stylers': [
            {
              'color': '#eeeeee'
            }
          ]
        },
        {
          'featureType': 'poi',
          'elementType': 'labels.text.fill',
          'stylers': [
            {
              'color': '#757575'
            }
          ]
        },
        {
          'featureType': 'poi.park',
          'elementType': 'geometry',
          'stylers': [
            {
              'color': '#e5e5e5'
            }
          ]
        },
        {
          'featureType': 'poi.park',
          'elementType': 'labels.text.fill',
          'stylers': [
            {
              'color': '#9e9e9e'
            }
          ]
        },
        {
          'featureType': 'road',
          'elementType': 'geometry',
          'stylers': [
            {
              'color': '#ffffff'
            }
          ]
        },
        {
          'featureType': 'road.arterial',
          'elementType': 'labels.text.fill',
          'stylers': [
            {
              'color': '#757575'
            }
          ]
        },
        {
          'featureType': 'road.highway',
          'elementType': 'geometry',
          'stylers': [
            {
              'color': '#dadada'
            }
          ]
        },
        {
          'featureType': 'road.highway',
          'elementType': 'labels.text.fill',
          'stylers': [
            {
              'color': '#616161'
            }
          ]
        },
        {
          'featureType': 'road.local',
          'elementType': 'labels.text.fill',
          'stylers': [
            {
              'color': '#9e9e9e'
            }
          ]
        },
        {
          'featureType': 'transit.line',
          'elementType': 'geometry',
          'stylers': [
            {
              'color': '#e5e5e5'
            }
          ]
        },
        {
          'featureType': 'transit.station',
          'elementType': 'geometry',
          'stylers': [
            {
              'color': '#eeeeee'
            }
          ]
        },
        {
          'featureType': 'water',
          'elementType': 'geometry',
          'stylers': [
            {
              'color': '#c9c9c9'
            }
          ]
        },
        {
          'featureType': 'water',
          'elementType': 'labels.text.fill',
          'stylers': [
            {
              'color': '#9e9e9e'
            }
          ]
        }];

        vm.iconStyle = {
          path: 'M-2.4,0a2.4,2.4 0 1,0 4.8,0a2.4,2.4 0 1,0 -4.8,0',
          fillColor: '#2C5A80',
          fillOpacity: 1,
          scale: 3,
          strokeColor: 'white',
          strokeWeight: 2
        }

        vm.getRadius = function(num) {
          return Math.sqrt(num) * 100;
        }

        NgMap.getMap().then(function(map) {
          vm.showCustomMarker= function(evt) {
            map.customMarkers.foo.setVisible(true);
            map.customMarkers.foo.setPosition(this.getPosition());
          };
          vm.closeCustomMarker= function(evt) {
            this.style.display = 'none';
          };
        });

        activate();

        function activate() {
            LeaveYourMark.campaignSummary
                         .get({pledgeCampaignId: 1103})
                         .$promise
                         .then((data) => {
                            vm.viewReady = true;
                            vm.currentDay = data.currentDay;
                            vm.totalDays = data.totalDays;
                            vm.given = data.totalGiven;
                            vm.committed = data.totalCommitted;
                            vm.givenPercentage = $filter('number')(vm.given / vm.committed * 100, 0);
                            vm.notStartedPercent = data.notStartedPercent;
                            vm.behindPercent = data.behindPercent;
                            vm.onPacePercent = data.onPacePercent;
                            vm.aheadPercent = data.aheadPercent;
                            vm.completedPercent = data.completedPercent;
                         })
                         .catch((err) => {
                            vm.viewReady = true;
                            console.error(err);
                         });
        }
    }
})();
