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
        vm.dynMarkers = [];

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

        vm.projects = [
          {id:'foo', name: 'FOO project', position:[41,-87]},
          {id:'bar', name: 'BAR project', position:[42,-86]}
        ];
        vm.project = vm.projects[0];

        vm.showDetail = function(e, project) {
          vm.project = project;
          vm.map.showInfoWindow('project-iw', project.id);
        };

        vm.hideDetail = function() {
          vm.map.hideInfoWindow('project-iw');
        };

        NgMap.getMap().then(function(map) {
          vm.map = map;
          vm.showCustomMarker= function(evt) {
            map.customMarkers.foo.setVisible(true);
            map.customMarkers.foo.setPosition(this.getPosition());
          };
          vm.closeCustomMarker= function(evt) {
            this.style.display = 'none';
          };
        });

        vm.currentIndex = 0;
        vm.selectNextCustomMarker = function() {
          vm.map.customMarkers[vm.currentIndex].removeClass('selected');
          vm.currentIndex = (vm.currentIndex+1) % vm.positions.length;
          vm.map.customMarkers[vm.currentIndex].addClass('selected');
          vm.currentPosition = vm.positions[vm.currentIndex];
        }

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
