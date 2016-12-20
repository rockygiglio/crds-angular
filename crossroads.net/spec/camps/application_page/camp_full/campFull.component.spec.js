import applicationModule from '../../../../app/camps/application_page/application_page.module';
import campsModule from '../../../../app/camps/camps.module';

describe('Camps full component', () => {
  let $componentController;
  let campsFullController;
  // let rootScope;
  // let state;

  // const campId = 1234;
  // const contactId = 9861;

  beforeEach(angular.mock.module(campsModule));
  beforeEach(angular.mock.module(applicationModule));

  beforeEach(inject((_$componentController_ /* ,_$rootScope_, _$state_ */) => {
    $componentController = _$componentController_;
    // state = _$state_;
    // rootScope = _$rootScope_;
    campsFullController = $componentController('campsFull', null, {});
  }));

  it('should instantiate the component', () => {
    expect(campsFullController.viewReady).toBeTruthy();
  });
});
