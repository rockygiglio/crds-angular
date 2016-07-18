export default class CreateGroupController {
    /*@ngInject*/
    constructor( ParticipantService, Group, $location, $state, $log) {
        this.log = $log;
        this.log.debug("CreateGroupController constructor");

        this.location = $location;
        this.group = Group;
        this.participantService = ParticipantService;
        this.state = $state;
        this.ready = false;
        this.approvedLeader = false;
    }

    $onInit() {
        this.log.debug('CreateGroupController onInit');
        this.participantService.get().then((data) => {
            if (_.get(data, 'ApprovedSmallGroupLeader', false)) {
                this.approvedLeader = true;
                this.ready = true;
            } else {
                this.location.path('/grouptool/leader');
            }
        },

            (err) => {
                this.log.error(`Unable to get Participant for logged-in user: ${err.status} - ${err.statusText}`);
                this.location.path('/grouptool/leader');
            });
        var profileAboutFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'Tell us about yourself.'
            },
            fieldGroup: [{
                key: 'profile.congregationId',
                type: 'select',
                templateOptions: {
                    label: 'At what site do you regularly attend service?',
                    valueProp: 'congregationId',
                    labelProp: 'congregationName',
                    options: [{
                            congregationId: '1',
                            congregationName: 'site1',
                        }, {
                            congregationId: '2',
                            congregationName: 'site2',
                        }, {
                            congregationId: '3',
                            congregationName: 'site3',
                        }, {
                            congregationId: '4',
                            congregationName: 'site4',
                        },
                    ]
                }
            }, {
                key: 'profile.birthDate',
                type: 'input',
                templateOptions: {
                    label: 'Birth Date'
                }
            }, {
                key: 'profile.genderId',
                type: 'radio',
                templateOptions: {
                    label: 'Gender',
                    inline: false,
                    valueProp: 'genderId',
                    labelProp: 'genderLabel',
                    options: [{
                        genderId: 0,
                        genderLabel: 'Female'
                    },{
                        genderId: 1,
                        genderLabel: 'Male'
                    }]
                }
            }]};
        var profileAddressFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'What’s your address?'
            },
            fieldGroup: [{
                key: 'profile.address.street',
                type: 'input',
                templateOptions: {
                    label: 'Street'
                }
            }, {
                key: 'profile.address.city',
                type: 'input',
                templateOptions: {
                    label: 'City'
                }
            }, {
                key: 'profile.address.state',
                type: 'input',
                templateOptions: {
                    label: 'State'
                }
            }, {
                key: 'profile.address.zip',
                type: 'input',
                templateOptions: {
                    label: 'Zip'
                }
            }, {
                key: 'profile.address.country',
                type: 'input',
                templateOptions: {
                    label: 'Country'
                }
            }]};
        var groupMeetingDateTimeFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'When will your group meet?',
                sectionHelp: 'To get the most out of your group, you’ll want to meet on a regular basis. We recommend weekly, but we want you to choose what’s best for your group.'
            },
            fieldGroup: [{
                type: 'radio',
                templateOptions: {
                    labelProp: 'label',
                    inline: false,
                    options: [{
                        label: 'Specific Day and Time',
                    },{
                        label: 'Flexible Meeting Times/Not Sure Yet',
                    }]
                }
            }, {
                key: 'group.meeting.day',
                type: 'input',
                templateOptions: {
                    label: 'Day'
                }
            }, {
                key: 'group.meeting.time',
                type: 'input',
                templateOptions: {
                    label: 'Time'
                }
            }, {
                key: 'group.meeting.frequency',
                type: 'input',
                templateOptions: {
                    label: 'Frequency'
                }
            }]};
        var groupMeetingLocationFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'Tell us about your meeting place.',
                sectionHelp: 'We’re not asking for the blueprint of your home, just a few details about where you’ll meet, if you have pets and if your group is kid-friendly.'
            },
            fieldGroup: [{
                key: 'group.meeting.online',
                type: 'radio',
                templateOptions: {
                    label: 'Where will your group meet?',
                    labelProp: 'label',
                    valueProp: 'online',
                    inline: false,
                    options: [{
                        label: 'Location',
                        online: false
                    },{
                        label: 'Online',
                        online: true
                    }]
                }
            }, {
                key: 'group.meeting.address.street',
                type: 'input',
                templateOptions: {
                    label: 'Street'
                }
            }, {
                key: 'group.meeting.address.city',
                type: 'input',
                templateOptions: {
                    label: 'City'
                }
            }, {
                key: 'group.meeting.address.state',
                type: 'input',
                templateOptions: {
                    label: 'State'
                }
            }, {
                key: 'group.meeting.address.zip',
                type: 'input',
                templateOptions: {
                    label: 'Zip'
                }
            }, {
                key: 'group.meeting.childcare',
                type: 'radio',
                templateOptions: {
                    label: 'Are you interested in leading a group with chilcare options?',
                    labelProp: 'label',
                    valueProp: 'childcare',
                    inline: false,
                    options: [{
                        label: 'Kids welcome at the group.',
                        childcare: true
                    },{
                        label: 'No. (Parents are welcome, but make your own kid plans.)',
                        childcare: false
                    }]
                }
            }]};
        var groupStartFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'Tell us the start date of your group.'
            },
            fieldGroup: [{
                key: 'group.startDate',
                type: 'input',
                templateOptions: {
                    label: 'Start Date'
                }
            }]};
        var groupTypeFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'What kind of group would you like to lead?',
                sectionHelp: 'We’re not trying to recreate a scene from your school lunchroom. Some groups like to roll with just guys or strictly married couples.'
            },
            fieldGroup: [{
                key: 'group.typeId',
                type: 'radio',
                templateOptions: {
                    labelProp: 'label',
                    valueProp: 'typeId',
                    options: [{
                        label: 'Men and women together (like God intended).',
                        typeId: 0
                    },{
                        label: 'Men only (no girls allowed).',
                        typeId: 1
                    },{
                        label: 'Women only (don\'t be a creeper, dude).',
                        typeId: 2
                    },{
                        label: 'Married couples (because you put a ring on it).',
                        typeId: 3
                    }]
                }
            }]};
        var groupAgeFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'What age range is your group going to be?',
                sectionHelp: 'Select as many as you like. If you want to lead middle and high school students, you must be approved by Student Ministry and complete a background check.'
            },
            fieldGroup: [{
                key: 'groupAgeRangeIds',
                type: 'multiCheckbox',
                templateOptions: {
                    valueProp: 'groupAgeRangeId',
                    labelProp: 'ageRangeName',
                    options: [{
                            groupAgeRangeId: 1,
                            ageRangeName: 'Middle School Students (Grades 6-8)'
                        }, {
                            groupAgeRangeId: 2,
                            ageRangeName: 'High School Students (Grades 9-12)'
                        }, {
                            groupAgeRangeId: 3,
                            ageRangeName: 'College Students'
                        }, {
                            groupAgeRangeId: 4,
                            ageRangeName: '20\'s'
                        }, {
                            groupAgeRangeId: 5,
                            ageRangeName: '30\'s'
                        }, {
                            groupAgeRangeId: 6,
                            ageRangeName: '40\'s'
                        }, {
                            groupAgeRangeId: 7,
                            ageRangeName: '50\'s'
                        }, {
                            groupAgeRangeId: 8,
                            ageRangeName: '60\'s+'
                        }
                    ]
                }
            }]};
        var groupAboutFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'Tell us what your group is all about.',
                sectionHelp: 'Now’s the time to add some personality, and tell us all about your group. Keep in mind, this is the description people will see when they search for groups to join.'
            },
            fieldGroup: [{
                key: 'group.groupName',
                type: 'input',
                templateOptions: {
                    label: 'Group Name'
                }
            },{
                key: 'group.groupDescription',
                type: 'textarea',
                templateOptions: {
                    label: 'Group Description',
                    rows: 6
                }
            }]};
        var groupVisibilityFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'Set your group to public or private.',
                sectionHelp: 'Choose whether your group will be viewable to everyone or only the people in your group.'
            },
            fieldGroup: [{
                key: 'group.availableOnline',
                type: 'radio',
                templateOptions: {
                    valueProp: 'accessId',
                    labelProp: 'accessLabel',
                    options: [{
                        accessId: 0,
                        accessLabel: 'Public (Your group will be viewable in search results for everyone.)'
                    },{
                        accessId: 1,
                        accessLabel: 'Private (Your group will NOT be viewable in search results for everyone.)'
                    }]
                }
            }]};
        this.model = {};
        this.fields = [profileAboutFields, profileAddressFields, groupTypeFields,
                        groupAgeFields, groupStartFields, groupMeetingDateTimeFields,
                        groupMeetingLocationFields, groupAboutFields, groupVisibilityFields];


}

  previewGroup() {
    this.state.go('grouptool.create.preview');
  }

  submit() {
    this.saving = true;
    if (this.childcareRequestForm.$invalid) {
      this.saving = false;
      return false;
    } else {
      //TODO map object(s)
      const dto = {
        groupName: 'Hard Coded Name',
        groupDescription: 'Hard Coded Description',
        groupTypeId: '1',
        ministryId: '8',
        startDate: moment(this.startDate).utc(),
        congregationId: this.choosenCongregation.dp_RecordID
      };

      //TODO save objects

      //const save = this.groupService.saveRequest(dto);
      //save.$promise.then(() => {
      //  this.log.debug('saved!');
      //  this.saving = false;
      //}, () => {
      //  this.saving = false;
      //  this.log.error('error!');
     //   this.saving = false;
    //  });
  }

  }

  savePersonal() {
      // set oldName to existing email address to work around password change dialog issue
      this.data.profileData.person.oldEmail = this.data.profileData.person.emailAddress;
      return this.data.profileData.person.$save();
  }

  createGroup() {

  }

  addParticipant() {
    var participants = [this.data.groupParticipant];

    return group.Participant.save({
        groupId: this.data.group.groupId,
      },
      participants).$promise;
  }

}
