require('../../app/corkboard');

describe('CorkboardEditController', function() {
    beforeEach(angular.mock.module('crossroads.corkboard'));

    beforeEach(angular.mock.module(function($provide){
      $provide.value('selectedItem', getSelectedItem());
    }));

    var $controller, $log, $httpBackend, $rootScope, $scope, selectedItem, CorkboardPostTypes, $state, Session, $window, $stateParams;
    beforeEach(inject(function(_$controller_, _$rootScope_, _$log_, _$state_, _$stateParams_, _Session_, _$window_, $injector){
        $log = _$log_;
        $httpBackend = $injector.get('$httpBackend');
        $rootScope = _$rootScope_.$new();
        $scope = _$rootScope_.$new();
        selectedItem = $injector.get('selectedItem');
        CorkboardPostTypes = $injector.get('CorkboardPostTypes');
        $state = _$state_;
        $stateParams = _$stateParams_;
        $stateParams.type = "ITEM";
        Session = _Session_;
        $window = _$window_;
        $controller = _$controller_('CorkboardEditController', { $scope:$scope });
    }));



    function getSelectedItem(){
        return {"_id":{"$oid":"559bdd6c966686116894e13c"},"Time":"2015-06-25T00:00:00Z","Title":"Super Ultra Mega Party","Location":"SPACE","Date":"2015-07-01T04:00:00Z","Description":"This event is in the past! YOU GOTTA GO BACK IN TIME AND BLOW UP THE OCEAN","PostType":"EVENT","UserId":"2562379","Removed":false,"DatePosted":{"$date":1436278124112},FlagCount:2};
    }
});
