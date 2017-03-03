export default class AnywhereLeaderController {
  /* @ngInject */
  constructor(GoVolunteerService) {
    this.viewReady = false;
    this.project = GoVolunteerService.project;

    this.participants = [
      { name: 'Jenny Shultz', email: 'jshultz@hotmail.com', phone: '205-333-5962', adults: 1, children: 2 },
      { name: 'Jamie Hanks', email: 'jaha95@gmail.com', phone: '205-333-5962', adults: 0, children: 2 },
      { name: 'Jennie Jones', email: 'jjgirl@yahoo.com', phone: '205-334-5988', adults: 2, children: 0 },
      { name: 'Jimmy Hatfield', email: 'jhattyhat@yahoo.com', phone: '205-425-5772', adults: 0, children: 2 },
      { name: 'Elisha Underwood', email: 'eu@yahoo.com', phone: '205-259-2777', adults: 0, children: 2 },
      { name: 'Terry Washington', email: 'tdub777@yahoo.com', phone: '205-259-8954', adults: 1, children: 1 },
      { name: 'Jim Wolf', email: 'dwolf@yahoo.com', phone: '205-334-9584', adults: 1, children: 2 }
    ];

    this.totalParticipants = 24;
  }

  $onInit() {
    this.viewReady = true;
  }
}
