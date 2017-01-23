(function() {
  module.exports = Search;

  Search.$inject = ['$resource'];
 
  function Search($resource) {
    return $resource(__AWS_SEARCH_ENDPOINT__+'search',{},{
      execute: {
        method : 'POST',
        transformRequest: function(data, headers){
          console.log(headers);
          var str = [];
          for(var p in data) {
            if(data[p]){
              str.push(encodeURIComponent(p) + '=' + encodeURIComponent(data[p]));
            }
          }
          return str.join('&');
        },
        headers : {
          'Content-Type': 'application/x-www-form-urlencoded',
          'ImpersonateUserId': undefined,
          'Authorization': undefined,
          'RefreshToken': undefined
        }
      }
    });
  }
})();
