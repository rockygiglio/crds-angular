(function () {
  'use strict';
  module.exports = BraveHomeController;

  BraveHomeController.$inject = ['$scope', '$window', '$stateParams', '$log', '$location', '$anchorScroll', 'Email'];

function BraveHomeController($scope, $window, $stateParams, $log, $location, $anchorScroll, Email) {
		var vm = this;


    

    console.log('test');
    //
    // email = {
    //   groupId          : group.groupId,
    //   replyToContact   : CONTACT_ID.JOURNEY,
    //   fromContactId    : cid,
    //   toContactId      : cid,
    //   mergeData : {
    //     HostName         : group.contact ? group.contact.firstName : null,
    //     HostPreferredName: group.contact ? group.contact.firstName : null
    //   }
    // };

    // email.templateId = EMAIL_TEMPLATES.PARTICIPANT_PUBLIC_CONFIRM_EMAIL_ID;
    //
    // Email.Mail.save(email).$promise.catch(function emailError(error) {
    //   $log.error('Email confirmation failed', error);
    // });


	};
})()
