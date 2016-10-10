LiveStreamRouter.$inject = ['$httpProvider', '$stateProvider'];

export default function LiveStreamRouter($httpProvider, $stateProvider) {
  $httpProvider.defaults.useXDomain = true;

  $stateProvider
    .state('live', {
      parent: 'screenWidth',
      url: '/live',
      template: '<landing></landing>',
      data: {
        meta: {
          title: 'Live',
          description: ''
        }
      }
    })
    .state('livestream', {
      parent: 'noHeaderOrFooter',
      url: '/live/stream',
      template: '<stream></stream>',
      data: {
        meta: {
          title: 'Live',
          description: ''
        }
      }
    })
    .state('livestream-videojs', {
      parent: 'noHeaderOrFooter',
      url: '/live-v1/videojs',
      template: '<stream-videojs></stream-videojs>',
      data: {
        meta: {
          title: 'Live',
          description: ''
        }
      }
    })
  ;
}
