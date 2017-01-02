import CONSTANTS from 'crds-constants';

export default class GroupCardController {
  /*@ngInject*/
  constructor($state, $rootScope) { 
    this.state = $state;
    this.rootScope = $rootScope;
    this.emailOptions = [];
    this.emailOptions.push(
      { name: CONSTANTS.GROUP.EMAIL.COMPOSE_EMAIL_NAME,
        descriptionLine1: CONSTANTS.GROUP.EMAIL.COMPOSE_EMAIL_DESCRIPTION_LINE1,
        descriptionLine2: CONSTANTS.GROUP.EMAIL.COMPOSE_EMAIL_DESCRIPTION_LINE2,
        icon: CONSTANTS.GROUP.EMAIL.COMPOSE_EMAIL_ICON
      });
    this.emailOptions.push(
      { name: CONSTANTS.GROUP.EMAIL.COPY_EMAIL_NAME,
        descriptionLine1: CONSTANTS.GROUP.EMAIL.COPY_EMAIL_DESCRIPTION_LINE1,
        descriptionLine2: CONSTANTS.GROUP.EMAIL.COPY_EMAIL_DESCRIPTION_LINE2,
        icon: CONSTANTS.GROUP.EMAIL.COPY_EMAIL_ICON
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

  onCopySuccess() {
    this.rootScope.$emit('notify', this.rootScope.MESSAGES.copiedToClipBoard);
  }

  onCopyError() {
    this.location.path('/groups/search/results').search('id', this.groupId);
    this.rootScope.$emit('notify', this.rootScope.MESSAGES.copiedToClipBoardError);
  }
}
