
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
    constructor($log, Profile, GroupService, Session, $rootScope) {
        this.log = $log;
        this.profile = Profile;
        this.groupService = GroupService;
        this.session = Session;
        this.rootScope = $rootScope;
        this.model = {};
        this.meetingFrequencyLookup = [{
            meetingFrequencyId: 1,
            meetingFrequencyDesc: 'Every week'
        }, {
                meetingFrequencyId: 2,
                meetingFrequencyDesc: 'Every other week'
            }];
        //this.statesLookup is added by the route resolve of the createGroupController.
        //this.profileData is added by the route resolve of the createGroupController.
    }

    preloadModel() {
            this.model = {
                profile: {
                    nickName: this.profileData.nickName,
                    lastName: this.profileData.lastName,
                    birthDate: this.profileData.dateOfBirth,
                    genderId: this.profileData.genderId,
                    householdId: this.profileData.householdId,
                    addressId: this.profileData.addressId,
                    contactId: this.profileData.contactId,
					congregationId: this.profileData.congregationId,
                    address: {
                        street: this.profileData.addressLine1,
                        city: this.profileData.city,
                        state: () => { 
                            _.find(this.statesLookup, (state) => {
                                return state.dp_RecordName == this.profileData.state
                            }).then(function (data) {
                                if(data === undefined) {
                                    return '';
                                }
                                else
                                {
                                    return datap.dp_RecordID;
                                }
                            })
                        },
                        zip: this.profileData.postalCode,
                        country: this.profileData.foreignCountry
                    }
                },
                categories: {
                    lifeStageId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.LIFE_STAGES,
                    neighborhoodId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.NEIGHBORHOODS,
                    spiritualGrowthId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.SPIRITUAL_GROWTH,
                    interestId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.INTEREST,
                    healingId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.HEALING
            },
            specificDay: true
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
                type: 'select',
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
                    key: 'profile.birthDate',
                    type: 'datepicker',
                    templateOptions: {
                        label: 'Birth Date',
                        required: true,
                        type: 'text',
                        datepickerPopup: 'MM/dd/yyyy'
                    }
                }, {
                    key: 'profile.genderId',
                    type: 'radio',
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
                key: 'profile.address.street',
                type: 'input',
                templateOptions: {
                    label: 'Street',
                    required: true,
                }
            }, {
                    key: 'profile.address.city',
                    type: 'input',
                    templateOptions: {
                        label: 'City',
                        required: true,
                    }
                }, {
                    key: 'profile.address.state',
                    type: 'select',
                    templateOptions: {
                        label: 'State',
                        required: true,
                        valueProp: 'dp_RecordID',
                        labelProp: 'dp_RecordName',
                        options: this.statesLookup
                    }
                }, {
                    key: 'profile.address.zip',
                    type: 'input',
                    templateOptions: {
                        label: 'Zip',
                        required: true,
                    }
                }, {
                    key: 'profile.address.country',
                    type: 'input',
                    templateOptions: {
                        label: 'Country',
                        required: true,
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
                type: 'radio',
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
                    type: 'select',
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
                    type: 'select',
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
                type: 'radio',
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
                    type: 'input',
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
                    type: 'input',
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
                    type: 'select',
                    hideExpression: 'model.group.meeting.online',
                    templateOptions: {
                        label: 'State',
                        required: true,
                        valueProp: 'dp_RecordID',
                        labelProp: 'dp_RecordName',
                        options: this.statesLookup
                    },
                    expressionProperties: {
                        'templateOptions.required': 'model.group.meeting.online'
                    }
                }, {
                    key: 'group.meeting.address.zip',
                    type: 'input',
                    optionsTypes: ['zipcode'],
                    hideExpression: 'model.group.meeting.online',
                    templateOptions: {
                        required: true,
                    },
                    expressionProperties: {
                        'templateOptions.required': 'model.group.meeting.online'
                    }
                }, {
                    key: 'group.kidFriendly',
                    type: 'radio',
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
                type: 'radio',
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
                type: 'multiCheckbox',
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
                type: 'input',
                templateOptions: {
                    label: 'Group Name',
                    required: true,
                    maxlength: 75
                }
            }, {
                key: 'group.groupDescription',
                type: 'textarea',
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
                type: 'radio',
                templateOptions: {
                    valueProp: 'accessId',
                    labelProp: 'accessLabel',
                    required: true,
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

        var groupCategoryFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: 'What kind of group would you like to lead?',
                sectionHelp: 'Not much to say when you\'re high abouve the helpy help'
            },
            fieldGroup: [{
                key: 'categories',
                type: 'multiCheckBoxCombo',
                templateOptions: {
                    required: true,
                    options: [{
                            id: 1, 
                            label: 'Life Stages',
                            labelDesc: 'For people in a similar life stage like empty nesters, singles, foster parents, moms, young married couples, etc.'
                        }, {
                            id: 2, 
                            label: 'Neighborhoods',
                            labelDesc: 'Your group is primarily focused on building community with the people who live closest together in your town, zip code or on your street.'
                        }, {
                            id: 3, 
                            label: 'Spirtual Growth',
                            labelDesc: 'Grow together through Huddle, reading a book or studying the Bible and applying what you learn to your everyday life.'
                        }, {
                            id: 4, 
                            label: 'Interest',
                            labelDesc: 'For people who share a common activity. From cooking to karate, motorcycles to frisbee golf, veterans or entrepreneurs, whatever your interest, we bet there’s a group looking for it.'
                        }, {
                            id: 5, 
                            label: 'Healing',
                            labelDesc: 'For people looking for healing and recovery in an area of life like grief, infertility, addiction, divorce, crisis, etc.'
                        }
                    ],
                    valueProp: 'id',
                    labelProp: 'label'
                }
            }, {
                key: 'group.categories.lifestages',
                type: 'boldcheckbox',
                wrapper: 'checkboxdescription',
                templateOptions: {
                    label: 'Life Stage',
                    labelDesc: 'For people in a similar life stage like empty nesters, singles, foster parents, moms, young married couples, etc.'
                }
            }, {
                    key: 'group.categories.lifeStageDetail',
                    type: 'input',
                    hideExpression: '!model.group.categories.lifestages',
                    templateOptions: {
                        placeholder: 'Life Stage detail...'
                    }
                }, {

                    key: 'group.categories.neighborhood',
                    type: 'boldcheckbox',
                    wrapper: 'checkboxdescription',
                    templateOptions: {
                        label: 'Neighborhoods',
                        labelDesc: 'Your group is primarily focused on building community with the people who live closest together in your town, zip code or on your street.'
                    }
                }, {
                    key: 'group.categories.neighborhoodDetail',
                    type: 'input',
                    hideExpression: '!model.group.categories.neighborhood',
                    templateOptions: {
                        placeholder: 'Neighborhood detail...'
                    }
                }, {
                    key: 'group.categories.spiritualgrowth',
                    type: 'boldcheckbox',
                    wrapper: 'checkboxdescription',
                    templateOptions: {
                        label: 'Spirtual Growth',
                        labelDesc: 'Grow together through Huddle, reading a book or studying the Bible and applying what you learn to your everyday life.'
                    }
                }, {
                    key: 'group.categories.spiritualgrowthDetail',
                    type: 'input',
                    hideExpression: '!model.group.categories.spiritualgrowth',
                    templateOptions: {
                        placeholder: 'Spritual Growth detail...'
                    }
                }, {
                    key: 'group.categories.interest',
                    type: 'boldcheckbox',
                    wrapper: 'checkboxdescription',
                    templateOptions: {
                        label: 'Interest',
                        labelDesc: 'For people who share a common activity. From cooking to karate, motorcycles to frisbee golf, veterans or entrepreneurs, whatever your interest, we bet there’s a group looking for it.'
                    }
                }, {
                    key: 'group.categories.interestDetail',
                    type: 'input',
                    hideExpression: '!model.group.categories.interest',
                    templateOptions: {
                        placeholder: 'Interest detail...'
                    }
                }, {
                    key: 'group.categories.healing',
                    type: 'boldcheckbox',
                    wrapper: 'checkboxdescription',
                    templateOptions: {
                        label: 'Healing',
                        labelDesc: 'For people looking for healing and recovery in an area of life like grief, infertility, addiction, divorce, crisis, etc.'
                    }
                }, {
                    key: 'group.categories.healingDetail',
                    type: 'input',
                    hideExpression: '!model.group.categories.healing',
                    templateOptions: {
                        placeholder: 'Healing detail...'
                    }
                }]
        }

        return [profileAboutFields, profileAddressFields, groupTypeFields,
            groupAgeFields, groupStartFields, groupMeetingDateTimeFields,
            groupMeetingLocationFields, groupCategoryFields, groupAboutFields, groupVisibilityFields];
    }

    getMeetingLocation() {
        debugger;
        let meetingDay = 'Flexible Meeting Time';
        let meetingFreq = _.find(this.meetingFrequencyLookup, (freq) => { return freq.meetingFrequencyId == this.model.group.meeting.frequency });
        if(this.model.group.meeting.day != 'undefined' && this.model.group.meeting.day != null)
        {
            meetingDay = _.find(this.meetingDaysLookup, (day) => { return day.dp_RecordID == this.model.group.meeting.day });
            return meetingDay.dp_RecordName + '\'s at ' + moment(this.model.group.meeting.time).format('LT') + ', ' + meetingFreq.meetingFrequencyDesc;
        }
        else
        {
            return meetingDay + ", " + meetingFreq.meetingFrequencyDesc;
        }
    }

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

        let smallGroup = new SmallGroup();

        smallGroup.groupName = this.model.group.groupName;
        smallGroup.groupDescription = this.model.group.groupDescription;
        smallGroup.groupType = new GroupType({ name: groupType.name });
        if (this.model.groupAgeRangeIds !== undefined && this.model.groupAgeRangeIds !== null) {
            smallGroup.ageRange = ageRangeNames;
        }
        if (this.model.group.meeting.address !== undefined && this.model.group.meeting.address !== null) {
            smallGroup.address = new Address();
            smallGroup.address.addressLine1 = this.model.group.meeting.address.street;
            smallGroup.address.state = this.model.group.meeting.address.state;
            smallGroup.address.zip = this.model.group.meeting.address.zip;
        }
        smallGroup.kidsWelcome = this.model.group.kidFriendly;
        smallGroup.meetingTimeFrequency = this.getMeetingLocation();
        smallGroup.meetingDay = this.model.group.meeting.day ;
        smallGroup.meetingTime = this.model.group.meeting.time;
        smallGroup.meetingFrequency = this.model.group.meeting.frequency;
        smallGroup.groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS;
        smallGroup.ministryId = CONSTANTS.MINISTRY.SPIRITUAL_GROWTH;
        smallGroup.congregationId = this.model.profile.congregationId;
        smallGroup.startDate = this.model.group.startDate;
        smallGroup.availableOnline = this.model.group.availableOnline;
        smallGroup.contactId = parseInt(this.session.exists('userId'));
        smallGroup.participants = [new Participant({
            groupRoleId: CONSTANTS.GROUP.ROLES.LEADER
            , nickName: this.model.profile.nickName
            , lastName: this.model.profile.lastName
            , contactId: parseInt(this.session.exists('userId'))
        }
        )];
        smallGroup.profile = new Profile({
            addressId: this.model.profile.addressId
            , addressLine1: this.model.profile.address.street
            , city: this.model.profile.address.city
            , congregationId: this.model.profile.congregationId
            , contactId: parseInt(this.session.exists('userId'))
            , dateOfBirth: this.model.profile.birthDate
            , emailAddress: this.rootScope.email
            , foreignCountry: this.model.profile.address.country
            , genderId: this.model.profile.genderId
            , householdId: this.model.profile.householdId
            , oldEmail: this.rootScope.email
            , postalCode: this.model.profile.address.zip
            , state: this.model.profile.address.state
        }

        );

        smallGroup.kidsWelcome = this.model.group.kidFriendly;

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

        // this.model.groupAgeRangeIds

        // _.forEach(this.model.groupAgeRangeIds, (selectedRange) => {
        //     ageRangeNames.push(new AgeRange({
        //         name: _.find(this.ageRangeLookup, (range) => {
        //             return range.attributeId == selectedRange
        //         }).name
        //     })
        //     )
        // });
        //           "91": {
        //     "attributeTypeId": 91,
        //     "name": "Age Range",
        //     "attributes": [
        //       {
        //         "attributeId": 7089,
        //         "name": "Middle School Students",
        //         "description": null,
        //         "selected": true,
        //         "startDate": "0001-01-01T00:00:00",
        //         "endDate": null,
        //         "notes": null,
        //         "sortOrder": 0,
        //         "category": null,
        //         "categoryDescription": null
        //       },
        //   }            

        //           "91": {
        //     "attributeTypeId": 91,
        //     "name": "Age Range",
        //     "attributes": [
        //       {
        //         "attributeId": 7089,
        //         "name": "Middle School Students",
        //         "description": null,
        //         "selected": true,
        //         "startDate": "0001-01-01T00:00:00",
        //         "endDate": null,
        //         "notes": null,
        //         "sortOrder": 0,
        //         "category": null,
        //         "categoryDescription": null
        //       },
        //   }

        //       "90": {
        //     "attributeTypeId": 90,
        //     "name": "Group Category",
        //     "attributes": [
        //       {
        //         "attributeId": 7099,
        //         "name": "Boxing",
        //         "description": null,
        //         "selected": true,
        //         "startDate": "2016-07-08T00:00:00",
        //         "endDate": null,
        //         "notes": null,
        //         "sortOrder": 0,
        //         "category": "Interest",
        //         "categoryDescription": null
        //       },
        //       {
        //         "attributeId": 7097,
        //         "name": "Landen, Ohio",
        //         "description": null,
        //         "selected": true,
        //         "startDate": "2016-07-07T13:00:00",
        //         "endDate": null,
        //         "notes": null,
        //         "sortOrder": 0,
        //         "category": "Neighborhoods",
        //         "categoryDescription": null
        //       },
        //       {
        //         "attributeId": 7098,
        //         "name": "Oakley, Ohio",
        //         "description": null,
        //         "selected": false,
        //         "startDate": "0001-01-01T00:00:00",
        //         "endDate": null,
        //         "notes": null,
        //         "sortOrder": 0,
        //         "category": "Neighborhoods",
        //         "categoryDescription": null
        //       },
        //       {
        //         "attributeId": 7100,
        //         "name": "Pokèmon GO",
        //         "description": "Gotta catch 'em all",
        //         "selected": true,
        //         "startDate": "2016-07-14T11:00:00",
        //         "endDate": null,
        //         "notes": null,
        //         "sortOrder": 0,
        //         "category": "Interest",
        //         "categoryDescription": null
        //       }
        //     ]
        //   },
        
        // TODO singleAttributes and multiAttributes
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