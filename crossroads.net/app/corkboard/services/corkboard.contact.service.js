(function () {
    'use strict';

    module.exports = ContactAboutPost;
    var emailEndpoint = __API_ENDPOINT__ + 'api/sendemail/';

    ContactAboutPost.$inject = ['$resource', '$http'];

    function ContactAboutPost($resource, $http) {

        return {
            post: function(post) {
                return $resource(emailEndpoint,
                    post,
                    {
                        'post': { method:'POST' }
                    });
            }
        }
    };

})();
