
import Address from '../model/address';

export default class GroupSearchResultsController {
  /*@ngInject*/
  constructor(NgTableParams, GroupService, $state, AuthModalService, $rootScope, AddressValidationService, $location) {
    this.groupService = GroupService;
    this.authModalService = AuthModalService;
    this.rootScope = $rootScope;
    this.addressValidationService = AddressValidationService;
    this.locationService = $location;

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
    this.searchedWithLocation = location && location.length > 0;

    let queryString = {};
    if(this.searchedWithLocation) {
      queryString.location = location;
    }
    if(query && query.length > 0) {
      queryString.query = query;
    }
    this.locationService.search(queryString);

    this.showLocationInput = false;
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

  showLocationForm(form) {
    form.location.$rollbackViewValue();
    this.showLocationInput = true;
  }

  hideLocationForm(form) {
    if(form.location.$invalid) {
      this.search.location = '';
      form.location.$setValidity('pattern', true);
    }
    form.location.$rollbackViewValue();
    this.showLocationInput = false;
  }

  submit(form) {
    if(form && this.search.location && this.search.location.length > 0) {
      this.processing = true;
      let valid = false;
      this.addressValidationService.validateAddressString(this.search.location).then((data) => {
        let address = new Address(data);
        this.search.location = address.toSearchString();
        valid = true;
      }, (err) => {
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

  requestToJoinOrEmailGroupLeader(group, email=false) {
    let modalOptions = {
      template: '<confirm-request email-leader="confirmRequestModal.emailLeader" group="confirmRequestModal.group" modal-instance="confirmRequestModal.modalInstance"></confirm-request>',
      controller: function(group, emailLeader, $modalInstance) {
        this.group = group;
        this.emailLeader = emailLeader;
        this.modalInstance = $modalInstance;
      },
      controllerAs: 'confirmRequestModal',
      size: 'lg',
      resolve: {
        emailLeader: function () {
          return email;
        },
        group: function () {
          return group;
        }
      }
    };

    var modalInstance = this.authModalService.open({
      loginTitle: 'Sign In',
      loginContentBlockId: 'groupToolAuthModalLoginText',
      registerTitle: 'Register',
      registerContentBlockId: 'groupToolAuthModalRegisterText',
      cancelButton: 'Back to Search Results',
      modal: modalOptions
    });
  }
}
