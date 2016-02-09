(function() {
  'use strict';

  module.exports = AddEventComponent;

  AddEventComponent.$inject = [
    '$log',
    '$rootScope',
    'AddEvent',
    'Lookup',
    'Programs',
    'StaffContact',
    'Validation'
  ];

  function AddEventComponent() {
    return {
      restrict: 'E',
      scope: {
        eventData: '='
      },
      templateUrl: 'add_event/add_event.html',
      controller: AddEventController,
      controllerAs: 'evt',
      bindToController: true
    };
  }

  function AddEventController($log, $rootScope, AddEvent, Lookup, Programs, StaffContact, Validation) {
    var vm = this;

    vm.crossroadsLocations = [];
    vm.addEvent = AddEvent;
    vm.endDateOpen = endDateOpen;
    vm.endDateOpened = false;
    vm.eventTypes = Lookup.query({ table: 'eventtypes' });
    vm.formatContact = formatContact;
    vm.programs = Programs.AllPrograms.query();
    vm.reminderDays = Lookup.query({ table: 'reminderdays' });
    vm.resetRooms = resetRooms;
    vm.staffContacts = StaffContact.query();
    vm.startDateOpen = startDateOpen;
    vm.startDateOpened = false;
    vm.validation = Validation;
    vm.validDateRange = validDateRange;

    activate();

    ///////
    function activate() {

      // Get the congregations
      Lookup.query({ table: 'crossroadslocations' }, function(locations) {
        vm.crossroadsLocations = locations;

        // does the current location need to be updated with the name?
        // if (AddEvent.editMode) {
        //   vm.eventData.event.congregation = _.find(locations, function(l) {
        //     return l.dp_RecordID === vm.eventData.event.congregation.dp_RecordID;
        //   });
        // }
      });

      if (_.isEmpty(vm.eventData)) {
        var startDate = new Date();
        startDate.setMinutes(0);
        startDate.setSeconds(0);
        var endDate = new Date(startDate);
        endDate.setHours(startDate.getHours() + 1);
        vm.eventData = {
          donationBatch: 0,
          sendReminder: 0,
          minutesSetup: 0,
          minutesCleanup: 0,
          startDate: new Date(),
          endDate: new Date(),
          startTime: startDate,
          endTime: endDate
        };
      }
    }

    function dateTime(dateForDate, dateForTime) {
      return new Date(
          dateForDate.getFullYear(),
          dateForDate.getMonth(),
          dateForDate.getDate(),
          dateForTime.getHours(),
          dateForTime.getMinutes(),
          dateForTime.getSeconds(),
          dateForTime.getMilliseconds());
    }

    function endDateOpen($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.endDateOpened = true;
    }

    function formatContact(contact) {
      var displayName = contact.displayName;
      var email = contact.email;
      return displayName + ' - ' + email;
    }

    function resetRooms() {
      vm.addEvent.eventData.rooms.length = 0;
    }

    function startDateOpen($event) {
      $event.preventDefault();
      $event.stopPropagation();
      vm.startDateOpened = true;
    }

    function validDateRange(form) {
      if (form === undefined) {
        return false;
      }

      var start =  dateTime(vm.eventData.startDate, vm.eventData.startTime);
      var end = dateTime(vm.eventData.endDate, vm.eventData.endTime);

      if (moment(start) <= moment(end)) {
        form.endDate.$error.endDate = false;
        form.endDate.$valid = true;
        form.endDate.$invalid = false;
        return false;
      }

      // set the endDate Invalid...
      form.endDate.$error.endDate = true;
      form.endDate.$valid = false;
      form.endDate.$invalid = true;
      form.endDate.$dirty = true;
      form.$valid = false;
      form.$invalid = true;
      return true;
    }

  }

})();
