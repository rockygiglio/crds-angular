export default class MyGroupsController {

  /*@ngInject*/
  constructor() {
    this.groups = [
      {
        id: 123,
        leader: true,
        name: 'John and Jenny\'s Married Couples New Testament Study Group',
        focus: '1 John',
        time: 'Friday\'s at 9:30am, Every Other Week',
        location: '8115 Montgomery Road, Cincinnati OH, 45243'
      },
      {
        id: 456,
        leader: false,
        name: 'Financial Help',
        focus: 'Budgeting',
        time: 'Thursday\'s at 10:30am, Every Week',
        location: '8115 Montgomery Road, Cincinnati OH, 45243'
      },
      {
        id: 789,
        leader: false,
        name: 'Bible Study',
        focus: 'Reaching Jesus',
        time: 'Friday\'s at 9:30am, Every Three Week',
        location: '8115 Montgomery Road, Cincinnati OH, 45243'
      },
    ];
  }
}
