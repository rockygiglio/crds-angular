var env = require("../environment");

describe('Streaming landing', function() {

  beforeEach(function () {
    browser.get(env.baseUrl + '/#/live');
    browser.ignoreSynchronization = true;
  });

  it('should render link to see schedule', function() {
    var scheduleBtn = element.all(by.css('streaming')).all(by.css('.see-schedule'));
    expect(scheduleBtn.isDisplayed()).toBeTruthy()
  });

  it('should scroll to schedule content', function() {
    browser.executeScript('return document.body.scrollTop').then(function(scrollTop){
      expect(scrollTop).toBe(0);
      var scheduleBtn = element.all(by.css('streaming')).all(by.css('.see-schedule'));
          scheduleBtn.click();

      browser.sleep(2000); // wait for animation
      browser.executeScript('return document.body.scrollTop').then(function(scrollTop){
        expect(scrollTop > 0).toBeTruthy();
      });
    });
  });

});
