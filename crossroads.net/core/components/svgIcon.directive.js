(function(){
  
  angular.module('crossroads.core').directive('svgIcon', SvgIcon);

  SvgIcon.$inject = [];

  function SvgIcon(){
    return {
      restrict: 'E',
      scope: {
        'icon': '@?icon',
        'classes': '@?classes'
      },
      link: function link(scope, el, attr){
        el.replaceWith('<svg viewBox=\'0 0 32 32\' class=\'icon icon-' + 
            scope.icon + (scope.classes ? ' ' + scope.classes : '') +  
            '\'><use xlink:href=\'#' + 
            scope.icon + '\'></use> </svg>'); 
      }
    };
  }
})();
