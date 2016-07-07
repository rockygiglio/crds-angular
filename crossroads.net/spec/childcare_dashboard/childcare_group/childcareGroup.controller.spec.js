import constants from 'crds-constants';
import ChildcareDashboardService from '../../../app/childcare_dashboard/childcareDashboard.service';
import ChildcareDashboardGroupController from '../../../app/childcare_dashboard/childcare_group/childcareDashboardGroup.controller';

/* jshint unused: false */
import childcareModule from '../../../app/childcare_dashboard/childcareDashboard.module';

describe('Childcare Group Component Controller', () => {

  let rootScope,
      log,
      childcareDashboardService,
      resource,
      cookies,
      controller
      ;

  beforeEach(angular.mock.module(constants.MODULES.CHILDCARE_DASHBOARD));

  beforeEach(inject(function(_$rootScope_, $injector) {
    rootScope = _$rootScope_;
    rootScope.MESSAGES = {
      childcareEventClosed: { content: 'test' },
      noEligibleChildren: { content: 'test2' },
      childcareEventCancelled: { content: 'test3'}
    };
    log = $injector.get('$log');
    cookies = $injector.get('$cookies');
    resource = $injector.get('$resource');
    childcareDashboardService = new ChildcareDashboardService(resource,cookies);
    controller = new ChildcareDashboardGroupController(rootScope, childcareDashboardService);
    controller.communityGroup = {eligibleChildren: [] };
  }));

  it('should not have eligibile children', () => {
    expect(controller.hasEligibleChildren()).toBe(false);
  });

  it('should display message if message is set', () => {
    controller.message = 'some message';
    controller.communityGroup = fakeCG();
    expect(controller.showMessage()).toBe(true);
  });

  it('event should be closed if it is happening within 7 days of today', () => {
    const today = new Date();
    var nextDate = new Date();
    nextDate.setDate(nextDate.getDate() + 7);
    controller.eventDate = nextDate.toISOString();
    controller.communityGroup = fakeCG();

    expect(controller.isEventClosed()).toBe(true);
    expect(controller.showMessage()).toBe(true);
  });

  it('event should not be closed if the happeing more than 7 days from today', () => {
    const today = new Date();
    var nextDate = new Date();
    nextDate.setDate(nextDate.getDate() + 9);
    controller.eventDate = nextDate.toISOString();
    controller.communityGroup = fakeCG();
    expect(controller.isEventClosed()).toBe(false);
  });

  it('should indicate if any children are currently rsvpd', () => {
    controller.communityGroup = fakeCG();
    expect(controller.hasSignedUpChild()).toBe(true);
  });

  it('should indicate if there are no children signed up', () => {
    controller.communityGroup = fakeCG(false);
    expect(controller.hasSignedUpChild()).toBe(false);
  });

  it('should indicate that the event has been cancelled', () => {
    controller.cancelled = true;
    expect(controller.isEventCancelled()).toBe(true);
    expect(controller.message).toBe(rootScope.MESSAGES.childcareEventCancelled.content);
  });

  function fakeCG(signedUp = true) {
    return {
     congregationId: 1,
     eligibleChildren:
       [
        {
          contactId: 100030266, 
          childName: 'Miles Silbernagel', 
          eligible: true, 
          rsvpness: signedUp 
        }
     ],
     eventEndTime: '2016-07-12T19:00:00',
     eventStartTime: '2016-07-12T18:00:00',
     groupMemberNamel:'Matthew Silbernagel',
     groupName: '(t) Fathers Oakley CG',
     maxAge: 0,
     remainingCapacity: 0,
    };
  }

});

