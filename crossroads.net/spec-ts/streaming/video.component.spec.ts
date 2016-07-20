/* tslint:disable:no-unused-variable */

import { VideoComponent } from '../../app/streaming/video.component';

describe('Component: Video', () => {

  var testComponent: VideoComponent;

  beforeEach(() => {
    testComponent = new VideoComponent();
  });

  it('should create an instance', () => {
    expect(testComponent).toBeTruthy();
  });

  it('should increment the number of people', () => {

    var currentNum = testComponent.number_of_people;
    testComponent.increaseCount();

    expect(testComponent.number_of_people).toBeGreaterThan(currentNum);

  });

  it('should decrement the number of people', () => {

    var currentNum = testComponent.number_of_people;
    testComponent.decreaseCount();

    expect(testComponent.number_of_people).toBeLessThan(currentNum);

  });

  it('should submit the count of people', () => {

    testComponent.submitCount();
    expect(testComponent.countSubmit).toBe(true);

  });

});