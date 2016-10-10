import constants from 'crds-constants';

/* jshint unused: false */
import campsModule from '../../app/camps/camps.module';

describe('Camp Service', () => {
  let campService;

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  beforeEach(inject((_CampService_) => {
    campService = _CampService_;
  }));

});
