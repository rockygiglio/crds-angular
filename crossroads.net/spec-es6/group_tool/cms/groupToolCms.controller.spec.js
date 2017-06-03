import constants from 'crds-constants';
import GroupToolCmsController from '../../../app/group_tool/cms/groupToolCms.controller';

describe('GroupToolCmsController', () => {
  let fixture,
    page,
    participantService,
    state,
    window,
    groupUrlService;

  beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

  beforeEach(inject(function ($injector) {
    page = $injector.get('Page');
    participantService = $injector.get('ParticipantService');
    state = $injector.get('$state');
    window = $injector.get('$state');
    groupUrlService = jasmine.createSpyObj('groupUrlService', ['groupLeaderUrl']);

    fixture = new GroupToolCmsController(page, participantService, state, window, groupUrlService);

    window.location = {
      origin: 'origin',
      href: 'href'
    };
  }));

  describe('$onInit() function', () => {
    it('is an approved group leader', () => {
      spyOn(participantService, 'get').and.callFake(
        function () {
          return {
            then: function (callback) {
              return callback({
                'ApprovedSmallGroupLeader': true
              });
            }
          }
        }
      )


      fixture.$onInit();

      //had to go this route as I could never get mocking a spy on this.page to work
      expect(fixture.url).toEqual('/groups/leader/resources/');
    });

    fit('is not an approved group leader', () => {
      let url = 'test';
      spyOn(participantService, 'get').and.callFake(
        function () {
          return {
            then: function (callback) {
              return callback({
                ApprovedSmallGroupLeader: false
              });
            }
          }
        }
      )

      groupUrlService.groupLeaderUrl.and.callFake(
        function () {
          return {
            then: function (callback) {
              return callback(url);
            }
          }
        }
      );

      fixture.$onInit();

      expect(window.location.href).toEqual(window.location.origin + url);
    });
  });
});