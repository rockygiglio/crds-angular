require('../../app/routes.content');

(() => {
  'use strict';
  describe('RoutesContent', () => {
    xit('#removeTrailingSlashIfNecessary(link) should remove slash if necessary', () => {
      let link = "https://www.crossroads.net/care/";
      let result = removeTrailingSlashIfNecessary(link);

      expect(result).toEqual("https://www.crossroads.net/foobar");
    }); 
  });
});

