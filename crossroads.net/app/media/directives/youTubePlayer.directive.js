(function() {
  'use strict';

  module.exports = YouTubePlayer;

  YouTubePlayer.$inject = ['$window', 'YT_EVENT', 'YouTubePlayerFactory', '$analytics'];

  function YouTubePlayer($window, YT_EVENT, YouTubePlayerFactory, $analytics) {
    return {
      restrict: 'E',

      scope: {
        height: '@',
        width: '@',
        videoid: '@'
      },

      template: '<div></div>',

      link: function(scope, element) {
        var tag = document.createElement('script');
        tag.src = '//www.youtube.com/iframe_api';
        var firstScriptTag = document.getElementsByTagName('script')[0];
        firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

        var player;

        YouTubePlayerFactory.onReady(function() {
          player = setupPlayer(scope, element);
        });

        function setupPlayer(scope, element) {

          return new YT.Player(element.children()[0], {
            playerVars: {
              autoplay: 0,
              html5: 1,
              theme: 'light',
              modesbranding: 0,
              color: 'white',
              iv_load_policy: 3,
              showinfo: 1,
              controls: 1
            },

            height: scope.height,
            width: scope.width,
            videoId: scope.videoid,

            events: {
              'onReady': function(){
                  player.addEventListener('onStateChange', function(e) {
                  console.log('State is:', e.data);
                   });                
              },

              'onStateChange': function(event) {

                var message = {
                  event: YT_EVENT.STATUS_CHANGE,
                  data: ''
                };

                switch (event.data) {
                  case YT.PlayerState.PLAYING:
                    message.data = 'PLAYING';
                    $analytics.eventTrack('Play', {  category: 'video', label: scope.videoid });
                    break;
                  case YT.PlayerState.ENDED:
                    message.data = 'ENDED';
                    $analytics.eventTrack('Ended', {  category: 'video', label: scope.videoid });
                    break;
                  case YT.PlayerState.UNSTARTED:
                    message.data = 'NOT PLAYING';
                    $analytics.eventTrack('Loaded', {  category: 'video', label: scope.videoid });
                    break;
                  case YT.PlayerState.PAUSED:
                    message.data = 'PAUSED';
                    $analytics.eventTrack('Pause', {  category: 'video', label: scope.videoid });
                    break;
                }

                scope.$apply(function() {
                  scope.$emit(message.event, message.data);
                });
              }
            }
          });
        }

        scope.$watch('height + width', function(newValue, oldValue) {
          if (newValue == oldValue) {
            return;
          }

          player.setSize(scope.width, scope.height);

        });

        scope.$watch('videoid', function(newValue, oldValue) {
          if (newValue == oldValue) {
            return;
          }

          player.cueVideoById(scope.videoid);

        });

        scope.$on(YT_EVENT.STOP, function() {
          if (!player) {
            return;
          }

          player.seekTo(0);
          player.stopVideo();
        });

        scope.$on(YT_EVENT.PLAY, function() {
          player.playVideo();
        });

        scope.$on(YT_EVENT.PAUSE, function() {
          player.pauseVideo();
        });

      }
    };
  }
})();
