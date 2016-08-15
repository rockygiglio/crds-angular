
import Address from '../model/address';

export default class GroupSearchResultsController {
  /*@ngInject*/
  constructor(NgTableParams, GroupService, $state, $modal, $rootScope, AddressValidationService) {
    this.groupService = GroupService;
    this.$modal = $modal;
    this.rootScope = $rootScope;
    this.addressValidationService = AddressValidationService;

    this.search = null;
    this.processing = false;
    this.state = $state;
    this.ready = false;
    this.results = [];

    this.showLocationInput = false;
    this.searchedWithLocation = false;

    this.tableParams = new NgTableParams({}, {});
  }

  $onInit() {
    this.search = {
      query: this.state.params.query,
      location: this.state.params.location
    };
    this.doSearch(this.state.params.query, this.state.params.location);
  }

  doSearch(query, location) {
    this.showLocationInput = false;
    this.searchedWithLocation = location && location.length > 0;
    this.ready = false;
    this.results = [];
    this.groupService.search(query, location).then(
      (data) => {
        this.results = data;
      },
      (err) => {
        // TODO what happens on error? (could be 404/no results, or other error)
      }
    ).finally(
      () => {
        // TODO Need to figure out pagination, etc

        // This resets the ngTable count so we see all the results and sets sorting appropriately
        let parms = {
          count: this.results.length
        };
        parms.sorting = this.searchedWithLocation ? { proximity: 'asc' } : { meetingDay: 'asc' };

        // This resets the dataset so ngTable properly renders the new search results
        let settings = {
          dataset: this.results
        };
        this.tableParams.settings(settings);
        this.tableParams.parameters(parms);
        this.ready = true;
      }
    );
  }

  submit(form) {
    let valid = true;
    if(form) {
      this.processing = true;
      valid = false;
      this.addressValidationService.validateAddressString(this.search.location).then((data) => {
        let address = new Address(data);
        this.search.location = address.toSearchString();
        valid = true;
      }, (err) => {
        valid = false;
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.groupToolSearchInvalidAddressGrowler);
      }).finally(() => {
        this.processing = false;
        form.location.$setValidity('pattern', valid);
        if(valid) {
          this.doSearch(this.search.query, this.search.location);
        }
      });
    } else {
      this.doSearch(this.search.query, this.search.location);
    }
  }

  requestToJoin(group) {
    var modalInstance = this.$modal.open({
      template: '<confirm-request group="confirmRequestModal.group" modal-instance="confirmRequestModal.modalInstance"></confirm-request>',
      controller: function(group, $modalInstance) {
        this.group = group;
        this.modalInstance = $modalInstance;
      },
      controllerAs: 'confirmRequestModal',
      size: 'lg',
      resolve: {
        group: function () {
          return group;
        }
      }
    });
  }
}
