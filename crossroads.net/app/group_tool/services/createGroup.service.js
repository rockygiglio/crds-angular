
import CONSTANTS from 'crds-constants';
import SmallGroup from '../model/smallGroup';
import Participant from '../model/participant';
import AgeRange from '../model/ageRange';
import Address from '../model/address';
import Category from '../model/category';
import GroupType from '../model/groupType';
import Profile from '../model/profile';

export default class CreateGroupService {
    /*@ngInject*/
    constructor($log, Profile, GroupService, Session, $rootScope, ImageService) {
        this.log = $log;
        this.profile = Profile;
        this.groupService = GroupService;
        this.session = Session;
        this.rootScope = $rootScope;
        this.model = {};
        this.resolved = false;
        this.imageService = ImageService;
        this.meetingFrequencyLookup = [{
            meetingFrequencyId: 1,
            meetingFrequencyDesc: 'Every week'
        }, {
                meetingFrequencyId: 2,
                meetingFrequencyDesc: 'Every other week'
            }];
        //this.statesLookup is added by the route resolve of the createGroupController.
        //this.profileData is added by the route resolve of the createGroupController.
        //this.countryLookup is added by the route resolve of the createGroupController.
    }

    preloadModel() {
        if (!this.resolved) {
            this.model.profile.oldEmail = this.model.profile.emailAddress;
            delete this.model.profile.householdMembers;
            delete this.model.profile.congregationId;
            this.model.group = {
                meeting: {
                    time: "1983-07-16T21:00:00.000Z"
                }
            };
            this.model.specificDay = true;
            this.resolved = true;
        }
    }

    getFields() {
        this.preloadModel();
        var profileAboutFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'Tell us about yourself.'
            },
            fieldGroup: [{
                key: 'profile.congregationId',
                type: 'formlyBuilderSelect',
                templateOptions: {
                    label: 'At what site do you regularly attend service?',
                    required: true,
                    valueProp: 'dp_RecordID',
                    labelProp: 'dp_RecordName',
                    options: []
                },
                controller: /* @ngInject */ function ($scope, GroupService, CreateGroupService) {
                    $scope.to.loading = GroupService.getSites().then(function (response) {
                        $scope.to.options = response;
                        CreateGroupService.sitesLookup = response;
                        return response;
                    });
                }
            }, {
                    key: 'profile.dateOfBirth',
                    type: 'datepicker',
                    templateOptions: {
                        label: 'Birth Date',
                        required: true,
                        type: 'text',
                        datepickerPopup: 'MM/dd/yyyy'
                    }
                }, {
                    key: 'profile.genderId',
                    type: 'formlyBuilderRadio',
                    templateOptions: {
                        label: 'Gender',
                        required: true,
                        inline: false,
                        valueProp: 'dp_RecordID',
                        labelProp: 'dp_RecordName',
                        options: []
                    },
                    controller: /* @ngInject */ function ($scope, GroupService) {
                        $scope.to.loading = GroupService.getGenders().then(function (response) {
                            $scope.to.options = response;
                            CreateGroupService.genderLookup = response;
                            return response;
                        });
                    }
                }, {
                    type: 'profilePicture',
                    wrapper: 'createGroupProfilePicture',
                    templateOptions: {
                        contactId: this.model.profile.contactId,
                        title: 'Update/Add Profile Picture',
                        desc: 'This will display on your group page. (Optional)'
                    }
                }]
        };
        var profileAddressFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'What’s your address?'
            },
            fieldGroup: [{
                key: 'profile.addressLine1',
                type: 'formlyBuilderInput',
                templateOptions: {
                    label: 'Street',
                    required: true,
                }
            }, {
                    key: 'profile.city',
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'City',
                        required: true,
                    }
                }, {
                    key: 'profile.state',
                    type: 'formlyBuilderSelect',
                    templateOptions: {
                        label: 'State',
                        required: true,
                        valueProp: 'dp_RecordName',
                        labelProp: 'dp_RecordName',
                        options: this.statesLookup
                    }
                }, {
                    key: 'profile.postalCode',
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'Zip',
                        required: true,
                    }
                }, {
                    key: 'profile.foreignCountry',
                    type: 'formlyBuilderSelect',
                    templateOptions: {
                        label: 'Country',
                        valueProp: 'dp_RecordName',
                        labelProp: 'dp_RecordName',
                        options: this.countryLookup
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
                key: 'specificDay',
                type: 'formlyBuilderRadio',
                templateOptions: {
                    labelProp: 'label',
                    required: true,
                    inline: false,
                    options: [{
                        label: 'Specific Day and Time',
                        value: true
                    }, {
                            label: 'Flexible Meeting Times/Not Sure Yet',
                            value: false
                        }]
                }
            }, {
                    key: 'group.meeting.day',
                    type: 'formlyBuilderSelect',
                    hideExpression: '!model.specificDay',
                    templateOptions: {
                        label: 'Day',
                        required: true,
                        valueProp: 'dp_RecordID',
                        labelProp: 'dp_RecordName',
                        options: []
                    },
                    expressionProperties: {
                        'templateOptions.required': 'model.specificDay'
                    },
                    controller: /* @ngInject */ function ($scope, GroupService, CreateGroupService) {
                        $scope.to.loading = GroupService.getDaysOfTheWeek().then(function (response) {
                            let sortedResponse = _.sortBy(response, function (day) { return day.dp_RecordID; });
                            $scope.to.options = sortedResponse;
                            CreateGroupService.meetingDaysLookup = response;
                            return response;
                        });
                    }
                }, {
                    key: 'group.meeting.time',
                    type: 'timepicker',
                    hideExpression: '!model.specificDay',
                    expressionProperties: {
                        'templateOptions.required': 'model.specificDay'
                    },
                    templateOptions: {
                        label: 'Time',
                        required: true,
                        minuteStep: 15
                    }
                }, {
                    key: 'group.meeting.frequency',
                    type: 'formlyBuilderSelect',
                    templateOptions: {
                        label: 'Frequency',
                        required: true,
                        valueProp: 'meetingFrequencyId',
                        labelProp: 'meetingFrequencyDesc',
                        options: this.meetingFrequencyLookup
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
                type: 'formlyBuilderRadio',
                templateOptions: {
                    label: 'Where will your group meet?',
                    required: true,
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
                    type: 'formlyBuilderInput',
                    hideExpression: 'model.group.meeting.online',
                    templateOptions: {
                        label: 'Street',
                        required: true,
                    },
                    expressionProperties: {
                        'templateOptions.required': 'model.group.meeting.online'
                    }
                }, {
                    key: 'group.meeting.address.city',
                    type: 'formlyBuilderInput',
                    hideExpression: 'model.group.meeting.online',
                    templateOptions: {
                        label: 'City',
                        required: true,
                    },
                    expressionProperties: {
                        'templateOptions.required': 'model.group.meeting.online'
                    }
                }, {
                    key: 'group.meeting.address.state',
                    type: 'formlyBuilderSelect',
                    hideExpression: 'model.group.meeting.online',
                    templateOptions: {
                        label: 'State',
                        required: true,
                        valueProp: 'dp_RecordName',
                        labelProp: 'dp_RecordName',
                        options: this.statesLookup
                    },
                    expressionProperties: {
                        'templateOptions.required': 'model.group.meeting.online'
                    }
                }, {
                    key: 'group.meeting.address.zip',
                    type: 'formlyBuilderInput',
                    optionsTypes: ['zipcode'],
                    hideExpression: 'model.group.meeting.online',
                    templateOptions: {
                        label: 'Zip',
                        required: true
                    },
                    expressionProperties: {
                        'templateOptions.required': 'model.group.meeting.online'
                    }
                }, {
                    key: 'group.kidFriendly',
                    type: 'formlyBuilderRadio',
                    hideExpression: 'model.group.meeting.online',
                    templateOptions: {
                        required: true,
                        label: 'Will your group have childcare?',
                        labelProp: 'label',
                        valueProp: 'kidFriendly',
                        inline: false,
                        options: [{
                            label: 'Yep. Kids are welcome and as a group we’ll make plans.',
                            kidFriendly: true
                        }, {
                                label: 'No. Adults only please.',
                                kidFriendly: false
                            }]
                    },
                    expressionProperties: {
                        'templateOptions.required': 'model.group.meeting.online'
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
                    required: true,
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
                type: 'formlyBuilderRadio',
                templateOptions: {
                    labelProp: 'name',
                    required: true,
                    valueProp: 'attributeId',
                    options: []
                },
                controller: /* @ngInject */ function ($scope, $log, GroupService, CreateGroupService) {
                    $scope.to.loading = GroupService.getGroupGenderMixType().then(function (response) {
                        $scope.to.options = response.attributes;
                        CreateGroupService.typeIdLookup = response.attributes;
                        $log.debug(CreateGroupService.model)
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
                type: 'formlyBuilderMultiCheckbox',
                templateOptions: {
                    valueProp: 'attributeId',
                    required: true,
                    labelProp: 'name',
                    options: []
                },
                controller: /* @ngInject */ function ($scope, GroupService, CreateGroupService) {
                    $scope.to.loading = GroupService.getAgeRanges().then(function (response) {
                        $scope.to.options = response.attributes;
                        // note, the line above is shorthand for:
                        // $scope.options.templateOptions.options = data;
                        CreateGroupService.ageRangeLookup = response.attributes;
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
                type: 'formlyBuilderInput',
                templateOptions: {
                    label: 'Group Name',
                    required: true,
                    maxlength: 75
                }
            }, {
                    key: 'group.groupDescription',
                    type: 'formlyBuilderTextarea',
                    templateOptions: {
                        label: 'Group Description',
                        required: true,
                        rows: 6,
                        maxlength: 2000
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
                type: 'formlyBuilderRadio',
                templateOptions: {
                    valueProp: 'accessId',
                    labelProp: 'accessLabel',
                    required: true,
                    options: [{
                        accessId: true,
                        accessLabel: 'Public (Your group will be viewable in search results for everyone.)'
                    }, {
                            accessId: false,
                            accessLabel: 'Private (Your group will NOT be viewable in search results for everyone.)'
                        }]
                }
            }]
        };

        var groupCategoryFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'What kind of group would you like to lead?',
                sectionHelp: ''
            },
            fieldGroup: [{
                key: 'categories',
                type: 'multiCheckBoxCombo',
                templateOptions: {
                    required: true,
                    valueProp: 'categoryId',
                    labelProp: 'label',
                    descProp: 'labelDesc',
                    placeholder: 'placeholder',
                    options: [{
                            categoryId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.LIFE_STAGES,
                            label: 'Life Stage',
                            labelDesc: 'For people in a similar life stage like empty nesters, singles, foster parents, moms, young married couples, etc.',
                            placeholder: 'Life Stages detail...'
                        }, {
                            categoryId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.NEIGHBORHOODS,
                            label: 'Neighborhoods',
                            labelDesc: 'Your group is primarily focused on building community with the people who live closest together in your town, zip code or on your street.',
                            placeholder: 'Neighborhood detail...'
                        }, {
                            categoryId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.SPIRITUAL_GROWTH,
                            label: 'Spirtual Growth',
                            labelDesc: 'Grow together through Huddle, reading a book or studying the Bible and applying what you learn to your everyday life.',
                            placeholder: 'Spritual Growth detail...'
                        }, {
                            categoryId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.INTEREST,
                            label: 'Interest',
                            labelDesc: 'For people who share a common activity. From cooking to karate, motorcycles to frisbee golf, veterans or entrepreneurs, whatever your interest, we bet there’s a group looking for it.',
                            placeholder: 'Interest detail...'
                        }, {
                            categoryId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.HEALING,
                            label: 'Healing',
                            labelDesc: 'For people looking for healing and recovery in an area of life like grief, infertility, addiction, divorce, crisis, etc.',
                            placeholder: 'Healing detail...'
                    }],
                }
            }]
        }

        return [profileAboutFields, profileAddressFields, groupTypeFields,
            groupAgeFields, groupStartFields, groupMeetingDateTimeFields,
            groupMeetingLocationFields, groupCategoryFields, groupAboutFields, groupVisibilityFields];
    }

    getMeetingLocation() {
        let meetingDay = 'Flexible Meeting Time';
        let meetingFreq = _.find(this.meetingFrequencyLookup, (freq) => { return freq.meetingFrequencyId == this.model.group.meeting.frequency });
        if (this.model.group.meeting.day != 'undefined' && this.model.group.meeting.day != null) {
            meetingDay = _.find(this.meetingDaysLookup, (day) => { return day.dp_RecordID == this.model.group.meeting.day });
            return meetingDay.dp_RecordName + '\'s at ' + moment(this.model.group.meeting.time).format('LT') + ', ' + meetingFreq.meetingFrequencyDesc;
        }
        else {
            return meetingDay + ", " + meetingFreq.meetingFrequencyDesc;
        }
    }


    //This is ugly and needs to be refactored
    mapSmallGroup() {
        let groupType = _.find(this.typeIdLookup, (groupType) => {
            return groupType.attributeId == this.model.group.typeId
        });

        let ageRangeNames = [];
        _.forEach(this.model.groupAgeRangeIds, (selectedRange) => {
            ageRangeNames.push(new AgeRange({
                name: _.find(this.ageRangeLookup, (range) => {
                    return range.attributeId == selectedRange
                }).name
            })
            )
        });

        var primaryContactId = this.model.profile.contactId;

        let smallGroup = new SmallGroup();

        smallGroup.primaryContact = {
          imageUrl: `${this.imageService.ProfileImageBaseURL}${primaryContactId}`,
          contactId: primaryContactId
        };

        smallGroup.groupName = this.model.group.groupName;
        smallGroup.groupDescription = this.model.group.groupDescription;
        smallGroup.groupType = new GroupType({ name: groupType.name });
        smallGroup.contactId = this.model.profile.contactId;
        if (this.model.groupAgeRangeIds !== undefined && this.model.groupAgeRangeIds !== null) {
            smallGroup.ageRange = ageRangeNames;
        }
        smallGroup.address = new Address();
        if (this.model.group.meeting.address !== undefined && this.model.group.meeting.address !== null) {
            smallGroup.address.addressLine1 = this.model.group.meeting.address.street;
            smallGroup.address.addressLine2 = '';
            smallGroup.address.state = this.model.group.meeting.address.state;
            smallGroup.address.zip = this.model.group.meeting.address.zip;
        }
        else {
            smallGroup.address.zip = null;
        }
        smallGroup.kidsWelcome = this.model.group.kidFriendly;
        smallGroup.meetingTimeFrequency = this.getMeetingLocation();

        smallGroup.meetingDayId = this.model.group.meeting.day;
        if(smallGroup.meetingDayId == null || smallGroup.meetingDayId == undefined)
        {
            delete smallGroup.meetingTime;
        }      
        smallGroup.meetingFrequency = this.model.group.meeting.frequency;

        if (this.model.specificDay) {
            smallGroup.meetingDayId = this.model.group.meeting.day;
            smallGroup.meetingTime = moment(this.model.group.meeting.time).format('LT');
        }
        smallGroup.meetingFrequencyId = this.model.group.meeting.frequency;
        smallGroup.groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS;
        smallGroup.ministryId = CONSTANTS.MINISTRY.SPIRITUAL_GROWTH;
        smallGroup.congregationId = this.model.profile.congregationId;
        smallGroup.startDate = this.model.group.startDate;
        smallGroup.availableOnline = this.model.group.availableOnline;
        smallGroup.participants = [new Participant({
            groupRoleId: CONSTANTS.GROUP.ROLES.LEADER
            , nickName: this.model.profile.nickName
            , lastName: this.model.profile.lastName
            , contactId: parseInt(this.session.exists('userId'))
        }
        )];

        smallGroup.profile = new Profile(this.model.profile);

        smallGroup.singleAttributes = {
            "73": {
                "attribute": {
                    "attributeId": this.getGroupTypeAttributeIdFromName(smallGroup.groupType.name)
                },
            }
        }

        var ids = [];
        _.forEach(this.model.groupAgeRangeIds, (id) => {
            ids.push(
                {
                    "attributeId": id,
                    "name": "Middle School Students",
                    "description": null,
                    "selected": true,
                    "startDate": "0001-01-01T00:00:00",
                    "endDate": null,
                    "notes": null,
                    "sortOrder": 0,
                    "category": null,
                    "categoryDescription": null
                })
        });

        var ageRangeJson = {
            "91": {
                "attributeTypeId": 91,
                "name": "Age Range",
                "attributes": ids
            }
        }

        smallGroup.attributeTypes = ageRangeJson;
        return smallGroup;

    }

    getGroupTypeAttributeIdFromName(name) {
        var groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ATTRIBUTE_ANYONE;
        switch (name) {
            case 'Anyone is welcome':
                groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ATTRIBUTE_ANYONE;
                break;
            case 'Men only':
                groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ATTRIBUTE_MENONLY;
                break;
            case 'Women only':
                groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ATTRIBUTE_WOMENONLY;
                break;
            case 'Married couples':
                groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ATTRIBUTE_COUPLES;
                break;
            default:
                groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ATTRIBUTE_ANYONE;
        }
        return groupTypeId;
    }
}