import CONSTANTS from '../../constants';

export default class CreateGroupService {
    /*@ngInject*/
    constructor($log, Profile, GroupService) {
        this.log = $log;
        this.profile = Profile;
        this.groupService = GroupService;
        this.model = {};

        this.groupService.getProfileData().then((data) => {
            this.model = {
                profile: {
                    birthDate: data.dateOfBirth,
                    genderId: data.genderId,
                    address: {
                        street: data.addressLine1,
                        city: data.city,
                        state: data.state,
                        zip: data.postalCode,
                        country: data.foreignCountry
                    }
                },
                group: {
                    meeting: {
                        time: '1983-01-16T22:00:00.007Z'
                    }
                }
            }
        });
    }

    getFields() {
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
                    valueProp: 'dp_RecordID',
                    labelProp: 'dp_RecordName',
                    options: []
                },
                controller: /* @ngInject */ function ($scope, GroupService) {
                    $scope.to.loading = GroupService.getSites().then(function (response) {
                        $scope.to.options = response;
                        return response;
                    });
                }
            }, {
                    key: 'profile.birthDate',
                    type: 'datepicker',
                    templateOptions: {
                        label: 'Birth Date',
                        type: 'text',
                        datepickerPopup: 'MM/dd/yyyy'
                    }
                }, {
                    key: 'profile.genderId',
                    type: 'radio',
                    templateOptions: {
                        label: 'Gender',
                        inline: false,
                        valueProp: 'dp_RecordID',
                        labelProp: 'dp_RecordName',
                        options: []
                    },
                    controller: /* @ngInject */ function ($scope, GroupService) {
                        $scope.to.loading = GroupService.getGenders().then(function (response) {
                            $scope.to.options = response;
                            return response;
                        });
                    }
                }]
        };
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
                }]
        };
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
                    }, {
                        label: 'Flexible Meeting Times/Not Sure Yet',
                    }]
                }
            }, {
                    key: 'group.meeting.day',
                    type: 'select',
                    templateOptions: {
                        label: 'Day',
                        valueProp: 'dp_RecordID',
                        labelProp: 'dp_RecordName',
                        options: []
                    },
                    controller: /* @ngInject */ function ($scope, GroupService) {
                        $scope.to.loading = GroupService.getDaysOfTheWeek().then(function (response) {
                            $scope.to.options = response;
                            return response;
                        });
                    }
                }, {
                    key: 'group.meeting.time',
                    type: 'timepicker',
                    templateOptions: {
                        label: 'Time',
                        minuteStep: 15
                    }
                }, {
                    key: 'group.meeting.frequency',
                    type: 'select',
                    templateOptions: {
                        label: 'Frequency',
                        valueProp: 'meetingFrequencyId',
                        labelProp: 'meetingFrequencyDesc',
                        options: [{
                            meetingFrequencyId: 1,
                            meetingFrequencyDesc: 'Every week'
                        }, {
                                meetingFrequencyId: 2,
                                meetingFrequencyDesc: 'Every other week'
                            }]
                    }
                }]
        };
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
                    }, {
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
                        }, {
                                label: 'No. (Parents are welcome, but make your own kid plans.)',
                                childcare: false
                            }]
                    }
                }]
        };
        var groupStartFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'Tell us the start date of your group.'
            },
            fieldGroup: [{
                key: 'group.startDate',
                type: 'datepicker',
                templateOptions: {
                    label: 'Start Date',
                    type: 'text',
                    datepickerPopup: 'MM/dd/yyyy'
                }
            }]
        };
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
                    labelProp: 'name',
                    valueProp: 'attributeId',
                    options: []
                },
                controller: /* @ngInject */ function ($scope, GroupService) {
                    $scope.to.loading = GroupService.getGroupGenderMixType().then(function (response) {
                        $scope.to.options = response.attributes;
                        return response;
                    });
                }
            }]
        };
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
                    valueProp: 'attributeId',
                    labelProp: 'name',
                    options: []
                },
                controller: /* @ngInject */ function ($scope, GroupService) {
                    $scope.to.loading = GroupService.getAgeRanges().then(function (response) {
                        $scope.to.options = response.attributes;
                        // note, the line above is shorthand for:
                        // $scope.options.templateOptions.options = data;
                        return response;
                    });
                }
            }]
        };
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
            }, {
                    key: 'group.groupDescription',
                    type: 'textarea',
                    templateOptions: {
                        label: 'Group Description',
                        rows: 6
                    }
                }]
        };
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
                    }, {
                            accessId: 1,
                            accessLabel: 'Private (Your group will NOT be viewable in search results for everyone.)'
                        }]
                }
            }]
        };

        return [profileAboutFields, profileAddressFields, groupTypeFields,
            groupAgeFields, groupStartFields, groupMeetingDateTimeFields,
            groupMeetingLocationFields, groupAboutFields, groupVisibilityFields];
    }
}