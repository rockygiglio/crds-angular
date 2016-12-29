
export default class GroupCardController {
  /*@ngInject*/
  constructor($state) { 
    this.state = $state;
    this.emailOptions = [];
    this.emailOptions.push(
      { name: "Compose Email",
        description: "Opens the email application on your computer/phone.",
        icon: "mail5"
      });
    this.emailOptions.push(
      { name: "Copy Email Addresses",
        description: "Copies participant email addresses to you clipboard",
        icon: "file-text-o"
      });
  }

  $OnInit() {
  }

  goToInvite() {
    this.state.go('grouptool.detail.requests', {groupId: this.group.groupId});
  }

  goToEdit() {
    this.state.go('grouptool.edit', {groupId: this.group.groupId});
  }
}
