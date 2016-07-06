GroupToolRouter.$inject = ['$httpProvider', '$stateProvider'];

export default function GroupToolRouter($httpProvider, $stateProvider) {

  $httpProvider.defaults.useXDomain = true;

  //TODO: I think this is done globally, not needed here, I think the above needs to be done globally
  $httpProvider.defaults.headers.common['X-Use-The-Force'] = true;

  $stateProvider
    .state('grouptool', {
      parent: 'noSideBar',
      url: '/groups',
      template: '<ui-view></ui-view>',
      data: {
        meta: {
          title: 'Groups',
          description: ''
        }
      }
    })
    .state('grouptool.mygroups', {
      parent: 'noSideBar',
      url: '/groups/mine',
      template: '<my-groups></my-groups>',
      data: {
        meta: {
          title: 'My Groups',
          description: ''
        }
      }
    })
    .state('grouptool.create', {
      parent: 'noSideBar',
      url: '/grouptool/create',
      template: '<create-group></create-group>',
      data: {
        isProtected: true,
        meta: {
          title: 'Create a Group',
          description: ''
        }
      }
    });
}
