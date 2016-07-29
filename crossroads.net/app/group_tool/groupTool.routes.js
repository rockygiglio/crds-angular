GroupToolRouter.$inject = ['$httpProvider', '$stateProvider'];

export default function GroupToolRouter($httpProvider, $stateProvider) {
  $httpProvider.defaults.useXDomain = true;

  //TODO: I think this is done globally, not needed here, I think the above needs to be done globally
  $httpProvider.defaults.headers.common['X-Use-The-Force'] = true;

  $stateProvider
    .state('grouptool', {
      parent: 'noSideBar',
      abstract: true,
    })
    .state('grouptool.mygroups', {
      parent: 'noSideBar',
      url: '/groups/mygroups',
      template: '<my-groups></my-groups>',
      data: {
        isProtected: true,
        meta: {
          title: 'My Groups',
          description: ''
        }
      }
    })
    .state('grouptool.create', {
      parent: 'noSideBar',
      url: '/groups/create',
      template: '<create-group></create-group>',
      resolve:{
        stateList: (CreateGroupService, GroupService) =>{
          return GroupService.getStates().then((data) => {
            CreateGroupService.statesLookup = data;
          })
        },
        profile: (CreateGroupService, GroupService) => {
          if(!CreateGroupService.resolved) {
            return GroupService.getProfileData().then((data) => {
              //CreateGroupService.model.profile = data;
              CreateGroupService.setCreateModel(data);
            })
          }
        },
        countryList: (CreateGroupService, Lookup) => {
          return Lookup.query({table: 'countries'}, (data) => {
            CreateGroupService.countryLookup = data;
          })
        }
      },
      data: {
        isProtected: true,
        meta: {
          title: 'Create a Group',
          description: ''
        }
      }
    })
    .state('grouptool.create.preview', {
      url: '/groups/create/preview',
      parent: 'noSideBar',
      template: '<create-group-preview> </create-group-preview>',
      data: {
        isProtected: true,
        meta: {
          title: 'Preview a Group',
          description: ''
        }
      }
    })
    .state('grouptool.edit', {
      parent: 'noSideBar',
      url: '/groups/edit/{groupId:int}',
      template: '<edit-group> </edit-group>',
      resolve:{
        // we are not using any of these resolves in the controller.
        // we are using these resolves to prepare the CreateGroupService
        // before the controller is initialized
        stateList: (CreateGroupService, GroupService) =>{
          return GroupService.getStates().then((data) => {
            CreateGroupService.statesLookup = data;
          })
        },
        profile: (CreateGroupService, GroupService) => {
          if(!CreateGroupService.resolved) {
            return GroupService.getProfileData().then((profile) => {
              return GroupService.getGroupData().then((group) => {
                CreateGroupService.setEditModel(group, profile);
              })
            })
          }
        },
        countryList: (CreateGroupService, Lookup) => {
          return Lookup.query({table: 'countries'}, (data) => {
            CreateGroupService.countryLookup = data;
          })
        },
      },
      data: {
        isProtected: true,
        meta: {
          title: 'Edit Your Group',
          description: ''
        }
      }
    })
    .state('grouptool.detail', {
      parent: 'noSideBar',
      url: '/groups/mygroups/detail/{groupId:int}',
      params: {
        groupId: {
          value: null,
          squash: true
        }
      },
      template: '<group-detail></group-detail>',
      data: {
        isProtected: true,
        meta: {
          title: 'Group Detail',
          description: ''
        }
      }
    })
    .state('grouptool.detail.about', {
      url: '/about',
      template: '<group-detail-about></group-detail-about>'
    })
    .state('grouptool.detail.participants', {
      url: '/participants',
      template: '<group-detail-participants></group-detail-participants>'
    })
    .state('grouptool.detail.requests', {
      url: '/requests',
      template: '<group-detail-requests></group-detail-requests>'
    })
    .state('grouptool.search', {
      parent: 'noSideBar',
      url: '/groups/search',
      template: '<group-search></group-search>',
      data: {
        isProtected: true,
        meta: {
          title: 'Find a Group',
          description: ''
        }
      }
    })
  ;
}
