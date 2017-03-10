import * as helpers from './goVolunteer.servicehelpers';

/* ngInject */
export default class GoVolunteerService {
  constructor($state) {
    this.state = $state;
    this.cities = [];
    this.initiativeCities = [];
    this.cmsInfo = {};
    this.childrenAttending = {
      childTwoSeven: 0,
      childEightTwelve: 0,
      childThirteenEighteen: 0
    };
    this.equipment = [];
    this.otherEquipment = [];
    this.launchSites = {};
    this.person = {
      nickName: '',
      lastName: '',
      emailAddress: '',
      dateOfBirth: null,
      mobilePhone: null
    };
    this.preferredLaunchSite = null;
    this.project = {};
    this.projectPrefOne = null;
    this.projectPrefTwo = null;
    this.projectPrefThree = null;
    this.privateGroup = false;
    this.skills = [];
    this.spouse = {
      fromDb: false
    };
    this.spouseAttending = false;
    this.myPrepTime = false;
    this.spousePrepTime = false;
    this.organization = {};
    this.otherOrgName = null;
    this.prepWork = [];
  }

  getRegistrationDto() {
    return this.registrationDto();
  }

  registrationDto() {
    return {
      additionalInformation: this.additionalInformation,
      children: helpers.children(this.childrenOptions),
      createGroupConnector: helpers.createGroupConnector(this.groupConnector),
      equipment: helpers.equipment(this.equipment, this.otherEquipment),
      groupConnector: this.groupConnector,
      initiativeId: this.state.toParams.initiativeId,
      organizationId: this.organization.organizationId,
      otherOrganizationName: this.otherOrgName ? this.otherOrgName : null,
      preferredLaunchSite: helpers.preferredLaunchSite(this.preferredLaunchSite, this.groupConnector),
      prepWork: helpers.prepWork(this.myPrepTime, this.spousePrepTime),
      privateGroup: this.privateGroup,
      projectPreferences: helpers.projectPreferences(this.projectPrefOne,
                                                     this.projectPrefTwo,
                                                     this.projectPrefThree),
      self: helpers.personDto(this.person),
      skills: this.skills.filter(skill => skill.checked),
      spouse: helpers.personDto(this.spouse),
      spouseParticipation: this.spouseAttending,
      waiverSigned: true
    };
  }
}
