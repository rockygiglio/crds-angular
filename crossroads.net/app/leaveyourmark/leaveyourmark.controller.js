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
        vm.showInfoWindow = false;
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
          {
            id:'uptown',
            name: 'Crossroads Uptown',
            position:[39.1281492,-84.5147683],
            description: "A Right Here site in Cincinnati, Ohio near the University of Cincinnati. Opened in August 2016.",
            status: "Complete",
            img: "https://crossroads-media.s3.amazonaws.com/images/lym-uptown.jpg"
          },
          {
            id:'florence',
            name: 'Crossroads Florence',
            position:[38.9885678,-84.6508777],
            description: "A Right Here site in Florence, Kentucky. New chapel space opened in December 2016.",
            status: "Complete",
            img: "https://crossroads-media.s3.amazonaws.com/images/lym-florence.jpg"
          },
          {
            id:'eastside',
            name: 'Crossroads East Side',
            position:[39.0938111,-84.2772213],
            description: "A right here site in Eastgate, Ohio. Property is being renovated for a January 2018 launch.",
            status: "Underway",
            img: "https://crossroads-media.s3.amazonaws.com/images/lym-eastside.jpg"
          },
          {
            id:'mason',
            name: 'Mason Awaited',
            position:[39.339405,-84.3387407],
            description: "A Right Here site in Mason, Ohio. The Awaited Christmas Experience welcomed 97,000 people in 2016.",
            status: "Complete",
            img: "https://crossroads-media.s3.amazonaws.com/images/masonawaited.jpg"
          },
          {
            id:'westside',
            name: 'West Side Expansion',
            position:[39.1604547,-84.7270626],
            description: "A Right Here site in Cleves, Ohio. In 2017, we doubled the size of the parking lot, auditorium and atrium to welcome more friends.",
            status: "Complete",
            img: "https://crossroads-media.s3.amazonaws.com/images/lym-westside.jpg"
          },
          {
            id:'oakley',
            name: 'Crossroads Oakley',
            position:[39.1594616,-84.4255526],
            description: "A right here site in Cincinnati, Ohio. In 2016, video and technical upgrades done on a 10-year-old system are now furthering our reach digitally.",
            status: "Complete",
            img: "https://crossroads-media.s3.amazonaws.com/images/lym-oakley.jpg"
          },
          {
            id:'oxford',
            name: 'Crossroads Oxford',
            position:[39.5106957,-84.736382],
            description: "Outpost",
            status: "Underway",
            img: ""
          },
          {
            id:'basecamp',
            name: 'Base Camp',
            position:[38.8121841,-84.2202297],
            description: "435 acres of land purchased in Neville, Ohio. Property has hosted more than 4,000 men, women and couples for camps.",
            status: "Complete",
            img: "https://crossroads-media.s3.amazonaws.com/images/lym-basecamp.jpg"
          },
          {
            id:'citylink',
            name: 'CityLink',
            position:[39.1197136,-84.5291963],
            description: "With I’M IN contributions, CityLink has signed a deal with Findlay Market to open a restaurant as a culinary training program, preparing individuals for careers in the industry.",
            status: "Underway",
            img: ""
          },
          // {
          //   id:'anywhere',
          //   name: 'Anywhere',
          //   position:[39.1197136,-84.5291963],
          //   description: "With I’M IN contributions, CityLink has signed a deal with Findlay Market to open a restaurant as a culinary training program, preparing individuals for careers in the industry.",
          //   status: "Underway"
          // },
          {
            id:'dayton',
            name: 'Crossroads Dayton',
            position:[39.8112442,-84.3422414],
            description: "A Right Here site in Dayton, Ohio. Properties are being considered for a January 2018 launch.",
            status: "Underway",
            img: ""
          },
          {
            id:'columbus',
            name: 'Columbus',
            position:[39.9828671,-83.130913],
            description: "A Right Here site in Columbus, Ohio. Groups are gathering in homes across the city and a site leader has been identified as momentum is growing.",
            status: "Underway",
            img: ""
          },
          {
            id:'nicaragua',
            name: 'Nicaragua',
            position:[12.859621,-87.2621271],
            description: "Through our partnerships with Compassion International, Crossroads is sponsoring xxx kids, feeding and educating them until age 18. In addition, funds are going toward renovations to their schools.",
            status: "",
            img: "https://crossroads-media.s3.amazonaws.com/images/lym-nica.jpg"
          },
          {
            id:'bolivia',
            name: 'Bolivia',
            position:[-16.2362265,-68.0461268],
            description: "",
            status: "",
            img: ""
          },
          {
            id:'india',
            name: 'India',
            position:[20.7478654,73.7222848],
            description: "",
            status: "",
            img: ""
          },
          {
            id:'southafrica',
            name: 'South Africa',
            position:[-29.7138554,20.355064],
            description: "",
            status: "",
            img: ""
          },
          {
            id:'centralky',
            name: 'Central Kentucky',
            position:[37.9937646,-84.3967061],
            description: "",
            status: "",
            img: ""
          },
        ];
        vm.project = vm.projects[0];

        vm.showDetail = function(e, project) {
          vm.project = project;
          vm.showInfoWindow = true;
          // vm.map.showInfoWindow('project-iw', project.id);
        };

        vm.hideDetail = function() {
          vm.showInfoWindow = false;
          // vm.map.hideInfoWindow('project-iw');
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
