
describe('InterceptorFactory', function() {
  var fixture;

  beforeEach(function() {
    angular.mock.module('crossroads.core');
  });

  beforeEach(inject(function(_InterceptorFactory_) {
    fixture = _InterceptorFactory_;
  }));

  describe('#request', function() {
    it('should not alter headers if none are specified', function() {
      var config = {
      };
      config = fixture.request(config);
      expect(config.headers).toBeUndefined();
    });

    it('should not alter headers if Crds-Api-Key is already specified', function() {
      var config = {
        headers: {
          'Crds-Api-Key': 'this is my api key'
        }
      };
      config = fixture.request(config);
      expect(config.headers['Crds-Api-Key']).toBe('this is my api key');
    });

    it('should add the default Crds-Api-Key to the headers if the request specifies an empty one', function() {
      var config = {
        headers: {
          'Crds-Api-Key': ''
        }
      };
      config = fixture.request(config);
      expect(config.headers['Crds-Api-Key']).toBe(__CROSSROADS_API_TOKEN__);
    });

    it('should add the default Crds-Api-Key to the headers if the request does not specify one', function() {
      var config = {
        headers: {
        }
      };
      config = fixture.request(config);
      expect(config.headers['Crds-Api-Key']).toBe(__CROSSROADS_API_TOKEN__);
    });
  });

});