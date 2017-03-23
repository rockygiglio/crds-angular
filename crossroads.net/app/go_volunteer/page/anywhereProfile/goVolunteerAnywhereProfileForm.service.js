import moment from 'moment';

export default class GoVolunteerAnywhereProfileForm {
  /* @ngInject */
  constructor(GoVolunteerService, GoVolunteerDataService, $log) {
    this.goVolunteerService = GoVolunteerService;
    this.goVolunteerDataService = GoVolunteerDataService;
    this.log = $log;
    const person = this.goVolunteerService.person;

    this.model = {
      firstName: person.nickName || person.firstName || undefined,
      lastName: person.lastName || undefined,
      email: person.emailAddress || undefined,
      mobilePhone: person.mobilePhone || undefined,
      birthDate: person.dateOfBirth || undefined,
      bringSpouse: undefined,
      numberKids: undefined,
      serveOutsideChurch: undefined,
      serveOptions: undefined,
      serveOtherName: undefined
    };
  }

  save(initiativeId, projectId) {
    const {
      firstName,
      lastName,
      email: emailAddress,
      birthDate: dob,
      mobilePhone: mobile,
      bringSpouse: spouseParticipation,
      numberKids: numberOfChildren
    } = this.model;
    const { contactId } = this.goVolunteerService.person;

    const registrationData = {
      initiativeId,
      spouseParticipation,
      numberOfChildren,
      self: {
        contactId,
        dob,
        emailAddress,
        firstName,
        lastName,
        mobile
      }
    };

    // eslint-disable-next-line new-cap
    return this.goVolunteerDataService.createAnywhereRegistration(projectId, registrationData);
  }

  getModel() {
    return this.model;
  }

  getFields() {
    return [
      {
        className: '',
        wrapper: 'campBootstrapRow',
        fieldGroup: [
          {
            className: 'form-group col-xs-6',
            key: 'firstName',
            type: 'crdsInput',
            templateOptions: {
              label: 'First Name',
              required: true
            }
          },
          {
            className: 'form-group col-xs-6',
            key: 'lastName',
            type: 'crdsInput',
            templateOptions: {
              label: 'Last Name',
              required: true
            }
          }
        ]
      },
      {
        key: 'email',
        type: 'crdsInput',
        templateOptions: {
          label: 'Email',
          required: true,
        }
      },
      {
        className: '',
        wrapper: 'campBootstrapRow',
        fieldGroup: [
          {
            className: 'form-group col-sm-6',
            key: 'birthDate',
            type: 'crdsDatepicker',
            templateOptions: {
              label: 'Birth Date (mm/dd/yyyy)',
              required: true,
              type: 'text',
              datepickerPopup: 'MM/dd/yyyy'
            },
            validation: {
              messages: {
                tooYoung: () => 'Must be 18 years old or older to sign up'
              }
            },
            asyncValidators: {
              tooYoung: {
                expression: modelValue => new Promise((resolve, reject) => {
                  const bday = moment(modelValue, 'MM-DD-YYYY');
                  const cutoff18 = moment().subtract(18, 'years');

                  if (bday.isAfter(cutoff18)) {
                    this.log.error('You must be 18 to sign up!');
                    reject();
                  }

                  resolve();
                })
              }
            }
          },
          {
            className: 'form-group col-sm-6',
            key: 'mobilePhone',
            type: 'crdsInput',
            optionsTypes: ['phoneNumber'],
            templateOptions: {
              label: 'Mobile Phone',
              required: true
            }
          },
        ]
      },
      {
        key: 'bringSpouse',
        type: 'crdsRadio',
        templateOptions: {
          label: 'Are you bringing your spouse?',
          required: true,
          labelProp: 'label',
          valueProp: 'value',
          inline: false,
          options: [{
            label: 'Yes',
            value: true
          }, {
            label: 'No',
            value: false
          }]
        }
      },
      {
        key: 'numberKids',
        type: 'crdsSelect',
        templateOptions: {
          label: 'If you are bringing your children, please specify how many.',
          helpBlock: '(17 years old or under)',
          required: true,
          valueProp: 'value',
          labelProp: 'label',
          options: [{
            label: '0',
            value: 0
          }, {
            label: '1',
            value: 1
          }, {
            label: '2',
            value: 2
          }, {
            label: '3',
            value: 3
          }, {
            label: '4',
            value: 4
          }, {
            label: '5',
            value: 5
          }, {
            label: '6',
            value: 6
          }]
        }
      }
    ];
  }
}
