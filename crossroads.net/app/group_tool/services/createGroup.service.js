
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
        this.primaryContact = null;
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
        //this.originalAttributeTypes is added by setEditModel and used in mapToSmallGroup
        //this.originalSingleAttributes is added by setEditModel and used in mapToSmallGroup
    }

    setEditModel(groupData, profileData){
        //this.log.debug("GroupDataFromServer:");
        this.log.debug(groupData);
        if (!this.resolved){
            this.originalAttributeTypes = groupData.attributeTypes;
            this.originalSingleAttributes = groupData.singleAttributes;
            this.primaryContact = groupData.contactId;
            this.preloadModel(profileData);
            this.mapFromSmallGroup(groupData);
            this.resolved = true;
        }
    }

    setCreateModel(profileData) {
        if (!this.resolved){
            this.preloadModel(profileData);
            delete this.model.profile.householdMembers;
            delete this.model.profile.congregationId;
            this.resolved = true;
        }
    }

    preloadModel(profile) {
        this.model.profile = profile;
        this.model.profile.oldEmail = profile.emailAddress;
        if(this.model.group !== undefined || this.model.group !== null) {
            this.model.group = {
                meeting: {
                    time: "1983-07-16T21:00:00.000Z"
                }
            };
        }
        else {
            this.model.group.meeting = {
                time: "1983-07-16T21:00:00.000Z"
            };
        }

        this.model.specificDay = true;
    }

    getFields() {
        //this.log.debug(this.model);
        var profileAboutFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: '$root.MESSAGES.groupToolCreateGroupProfile.content | html',
                sectionHelp: '$root.MESSAGES.groupToolCreateGroupProfileHelp.content | html'
            },
            fieldGroup: [{
                key: 'profile.congregationId',
                type: 'formlyBuilderSelect',
                templateOptions: {
                    label: 'What site do you regularly attend service?',
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
                sectionLabel: '$root.MESSAGES.groupToolCreateGroupAddress.content | html',
                sectionHelp: '$root.MESSAGES.groupToolCreateGroupAddressHelp.content | html'
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
                        required: true,
                        valueProp: 'dp_RecordName',
                        labelProp: 'dp_RecordName',
                        options: this.countryLookup
                    }
                }]
        };
        var groupMeetingDateTimeFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: '$root.MESSAGES.groupToolCreateGroupMeetingTime.content | html',
                sectionHelp: '$root.MESSAGES.groupToolCreateGroupMeetingTimeHelp.content | html'
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
                            label: 'Flexible Meeting Time/Not Sure Yet',
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
                sectionLabel: '$root.MESSAGES.groupToolCreateGroupMeetingLocation.content | html',
                sectionHelp: '$root.MESSAGES.groupToolCreateGroupMeetingLocationHelp.content | html'
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
                        label: 'In person',
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
                        'templateOptions.required': '!model.group.meeting.online'
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
                        'templateOptions.required': '!model.group.meeting.online'
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
                        'templateOptions.required': '!model.group.meeting.online'
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
                        'templateOptions.required': '!model.group.meeting.online'
                    }
                }, {
                    key: 'group.kidFriendly',
                    type: 'formlyBuilderRadio',
                    hideExpression: 'model.group.meeting.online',
                    templateOptions: {
                        required: true,
                        label: 'Are kids welcome at the group?',
                        labelProp: 'label',
                        valueProp: 'kidFriendly',
                        inline: false,
                        options: [{
                            label: 'Yep. Kids are welcome.  As a group, we’ll decide what to do with them.',
                            kidFriendly: true
                        }, {
                                label: 'No. Adults only please.',
                                kidFriendly: false
                            }]
                    },
                    expressionProperties: {
                        'templateOptions.required': '!model.group.meeting.online'
                    }
                }]
        };
        var groupStartFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: '$root.MESSAGES.groupToolCreateGroupStartDate.content | html',
                sectionHelp: '$root.MESSAGES.groupToolCreateGroupStartDateHelp.content | html'
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
                sectionLabel: '$root.MESSAGES.groupToolCreateGroupType.content | html',
                sectionHelp: '$root.MESSAGES.groupToolCreateGroupTypeHelp.content | html'
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
                        //$log.debug(CreateGroupService.model)
                        return response;
                    });
                }
            }]
        };
        var groupAgeFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: '$root.MESSAGES.groupToolCreateGroupAge.content | html',
                sectionHelp: '$root.MESSAGES.groupToolCreateGroupAgeHelp.content | html'
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
                sectionLabel: '$root.MESSAGES.groupToolCreateGroupAbout.content | html',
                sectionHelp: '$root.MESSAGES.groupToolCreateGroupAboutHelp.content | html',
                groupExample: '$root.MESSAGES.groupToolCreateGroupAboutExample.content | html'
            },
            fieldGroup: [{
                key: 'group.groupName',
                type: 'formlyBuilderInput',
                templateOptions: {
                    label: 'Group Name',
                    placeholder:'Ex. Brewing Brothers' ,
                    required: true,
                    maxlength: 75
                }
            }, {
                    key: 'group.groupDescription',
                    type: 'formlyBuilderTextarea',
                    templateOptions: {
                        label: 'Group Description',
                        required: true,
                        placeholder: 'Ex:This group is for men in their 30s who like to brew their own beer. We’ll meet regularly to come up with a new beer and brew it together, and share some beers while we build friendships. We’ll meet in Pleasant Ridge weekly in my home.',
                        rows: 6,
                        maxlength: 2000
                    }
                }]
        };
        var groupVisibilityFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: '$root.MESSAGES.groupToolCreateGroupVisibility.content | html',
                sectionHelp: '$root.MESSAGES.groupToolCreateGroupVisibilityHelp.content | html'
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
                        accessLabel: 'Public (Your group will be viewable in search results for everyone to see.)'
                    }, {
                            accessId: false,
                            accessLabel: 'Private (Your group will NOT be publically viewable in search results.)'
                        }]
                }
            }]
        };

        var groupCategoryFields = {
            wrapper: 'createGroup',
            templateOptions: {
                sectionLabel: '$root.MESSAGES.groupToolCreateGroupCategory.content | html',
                sectionHelp: '$root.MESSAGES.groupToolCreateGroupCategoryHelp.content | html'
            },
            fieldGroup: [{
                key: 'categories',
                type: 'multiCheckBoxCombo',
                templateOptions: {
                    required: true,
                    valueProp: 'categoryId',
                    labelProp: 'label',
                    descProp: 'labelDesc',
                    maxFieldLength: '25',
                    placeholder: 'placeholder',
                    options: [
                        {
                            categoryId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.INTEREST,
                            label: 'Interest',
                            labelDesc: '$root.MESSAGES.groupToolInterestDetail.content | html',
                            placeholder: 'Ex. Boxing, XBox'
                        },{
                            categoryId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.NEIGHBORHOODS,
                            label: 'Neighborhoods',
                            labelDesc: '$root.MESSAGES.groupToolNeighborhoodDescription.content | html',
                            placeholder: 'Ex. Norwood, Gaslight'
                        },{
                            categoryId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.SPIRITUAL_GROWTH,
                            label: 'Spirtual Growth',
                            labelDesc: '$root.MESSAGES.groupToolSpiritualGrowthDescription.content | html',
                            placeholder: 'Ex. Huddle, James'
                        },{
                            categoryId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.LIFE_STAGES,
                            label: 'Life Stage',
                            labelDesc: '$root.MESSAGES.groupToolLifeStageDescription.content | html',
                            placeholder: 'Ex. new family, young married, college, empty nesters'
                        },{
                            categoryId: CONSTANTS.ATTRIBUTE_CATEGORY_IDS.HEALING,
                            label: 'Healing',
                            labelDesc: '$root.MESSAGES.groupToolHealingDescription.content | html',
                            placeholder: 'Ex. grief, infertility, addiction, divorce, crisis'
                        }],
                }
            }]
        }

        return [profileAboutFields, profileAddressFields, groupTypeFields, groupAgeFields,
            groupStartFields, groupMeetingDateTimeFields, groupMeetingLocationFields, 
            groupCategoryFields, groupAboutFields, groupVisibilityFields];
    }
    
    //this badly needs to be unit tested
    mapFromSmallGroup(groupData){
        this.model.group.meeting.frequency = groupData.meetingFrequencyID;
        this.model.group.groupName = groupData.groupName;
        this.model.group.groupDescription = groupData.groupDescription;
        if (groupData.address != null && groupData.address != undefined){
            this.model.group.meeting.address = {
                street: groupData.address.addressLine1,
                city: groupData.address.city,
                state: groupData.address.state,
                zip: groupData.address.zip,
                addressId: groupData.address.addressId 
            }
            this.model.group.meeting.online = false;
        } else {
            this.model.group.meeting.online = true;
        }
        this.model.group.kidFriendly = groupData.kidsWelcome;
        this.model.group.availableOnline = groupData.availableOnline;
        this.model.group.startDate = moment(new Date(groupData.startDate)).toDate();
        this.model.group.meeting.time = moment(new Date('1983-07-16 ' + groupData.meetingTime)).toDate();
        this.model.group.meeting.day = groupData.meetingDayId;
        groupData.meetingDayId == null || groupData.meetingDayId == undefined ? this.model.specificDay = false : this.model.specificDay = true;
        this.model.group.typeId = groupData.singleAttributes[CONSTANTS.GROUP.GROUP_TYPE_ATTRIBUTE_TYPE_ID].attribute.attributeId;
        var ageRangeIds = [];
        _.forEach(groupData.attributeTypes[CONSTANTS.GROUP.AGE_RANGE_ATTRIBUTE_TYPE_ID].attributes, (value, key) => {
            if (value.selected)
                ageRangeIds.push(value.attributeId)
        });
        var categories = [];
        _.forEach(groupData.attributeTypes[CONSTANTS.GROUP.ATTRIBUTE_TYPE_ID].attributes, (value, key) => {
            if (value.selected)
                categories.push({
                    value: this.getIdFromCategory(value.category),
                    detail: value.name
                })
        });
        this.model.groupAgeRangeIds = ageRangeIds;
        this.model.categories = categories;
        this.model.groupId = groupData.groupId;
    }

    //this also needs unit tests
    mapToSmallGroup() {
    //group setup
        let smallGroup = new SmallGroup();
        //on an edit, we shouldn't change the contactId of a group because then if a co-leader edits the 
        //group they will be the new primary contact, and we don't want that.
        if (this.primaryContact != null || this.primaryContact != undefined){
            smallGroup.contactId = this.primaryContact;
        } else {
            smallGroup.contactId = this.model.profile.contactId;
        }
        smallGroup.groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ID.SMALL_GROUPS;
        smallGroup.ministryId = CONSTANTS.MINISTRY.SPIRITUAL_GROWTH;
        smallGroup.groupId = this.model.groupId;
        smallGroup.participants = [new Participant({
            groupRoleId: CONSTANTS.GROUP.ROLES.LEADER
            , nickName: this.model.profile.nickName
            , lastName: this.model.profile.lastName
            , contactId: parseInt(this.session.exists('userId'))
        })];
        
    //profile
        smallGroup.primaryContact = {
          imageUrl: `${this.imageService.ProfileImageBaseURL}${this.model.profile.contactId}`,
          contactId: this.model.profile.contactId
        };
        smallGroup.congregationId = this.model.profile.congregationId;
        smallGroup.profile = new Profile(this.model.profile);

    //groupType
        let groupType = _.find(this.typeIdLookup, (groupType) => {
            return groupType.attributeId == this.model.group.typeId
        });

        smallGroup.groupType = new GroupType({ name: groupType.name });
        
        //add the single attributes this group came in with to the small group model
        smallGroup.singleAttributes = {};
        if (this.originalSingleAttributes != null || this.originalSingleAttributes != undefined){
            smallGroup.singleAttributes = this.originalSingleAttributes;
            smallGroup.singleAttributes[CONSTANTS.GROUP.GROUP_TYPE_ATTRIBUTE_TYPE_ID].attribute.attributeId = this.getGroupTypeAttributeIdFromName(smallGroup.groupType.name);
        } else {
            smallGroup.singleAttributes[CONSTANTS.GROUP.GROUP_TYPE_ATTRIBUTE_TYPE_ID] = {
                "attribute": {
                    "attributeId": this.getGroupTypeAttributeIdFromName(smallGroup.groupType.name)
                }
            }
        }

    //groupAge
        let ageRangeNames = [];
        _.forEach(this.model.groupAgeRangeIds, (selectedRange) => {
            ageRangeNames.push(new AgeRange({
                name: _.find(this.ageRangeLookup, (range) => {
                    return range.attributeId == selectedRange
                }).name
            })
            )
        });
        if (this.model.groupAgeRangeIds != undefined && this.model.groupAgeRangeIds != null) {
            smallGroup.ageRange = ageRangeNames;
        }

        smallGroup.attributeTypes = {};
        if (this.originalAttributeTypes != null || this.originalAttributeTypes != undefined){
            // set the original attribute types on to the small group
            smallGroup.attributeTypes = this.originalAttributeTypes;
            // set selected age ranges to true, all others to false
            _.forEach(smallGroup.attributeTypes[CONSTANTS.GROUP.AGE_RANGE_ATTRIBUTE_TYPE_ID].attributes, (ageRange) => {
                if (_.includes(this.model.groupAgeRangeIds, ageRange.attributeId, 0)) {
                    ageRange.selected = true;
                } else {
                    ageRange.selected = false;
                }
            });
        } else {
            var ids = [];
            _.forEach(this.model.groupAgeRangeIds, (id) => {
                ids.push(
                    {
                        "attributeId": id,
                        "name": "",
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

            var ageRangeJson = {};
            ageRangeJson[CONSTANTS.GROUP.AGE_RANGE_ATTRIBUTE_TYPE_ID] = {
                "attributeTypeId": CONSTANTS.GROUP.AGE_RANGE_ATTRIBUTE_TYPE_ID,
                "name": "Age Range",
                "attributes": ids
            }
            smallGroup.attributeTypes = ageRangeJson;
        }

    //groupStartDate
        smallGroup.startDate = this.model.group.startDate;

    //groupMeetingTime
        
        smallGroup.meetingFrequency = this.model.group.meeting.frequency;
        if (this.model.specificDay) {
            smallGroup.meetingDayId = this.model.group.meeting.day;
            smallGroup.meetingTime = moment(this.model.group.meeting.time).format('LT');
            smallGroup.meetingFrequencyId = this.model.group.meeting.frequency;
        } else {
            smallGroup.meetingDayId = null;
            smallGroup.meetingTime = null;
            smallGroup.meetingFrequencyId = this.model.group.meeting.frequency;
        }
        
        
    //groupMeetingPlace
        if (!this.model.group.meeting.online){
            smallGroup.address = new Address();
            smallGroup.address.addressLine1 = this.model.group.meeting.address.street;
            smallGroup.address.addressLine2 = '';
            smallGroup.address.city = this.model.group.meeting.address.city;
            smallGroup.address.state = this.model.group.meeting.address.state;
            smallGroup.address.zip = this.model.group.meeting.address.zip;
            smallGroup.kidsWelcome = this.model.group.kidFriendly;
        } else {
            smallGroup.address = null;
            smallGroup.kidsWelcome = false;
        }
            
            smallGroup.meetingTimeFrequency = this.getMeetingLocation();
    //groupCategory
        var ids = []

        //set every category that the group came in with to selected = false if this is a load and 
        //let the database worry about whether or not what we've added is new.
        if (this.originalAttributeTypes != null || this.originalAttributeTypes != undefined){
            _.forEach(smallGroup.attributeTypes[CONSTANTS.GROUP.ATTRIBUTE_TYPE_ID].attributes, (attribute) => {
                attribute.selected = false;
            });
        }

        _.forEach(this.model.categories, (category) => {
            ids.push(
                {
                    attributeId: 0,
                    attributeTypeId: 90,
                    name: category.detail,
                    description: category.description,
                    selected: true,
                    startDate: category.startDate,
                    endDate: null,
                    notes: null,
                    sortOrder: 0,
                    category: this.getCategoryFromId(category.value),
                    categoryId: category.value,
                    categoryDescription: null

                }
            )
        });
        var categoriesJson = {};
        categoriesJson[CONSTANTS.GROUP.ATTRIBUTE_TYPE_ID]= {
            "attributeTypeid": CONSTANTS.GROUP.ATTRIBUTE_TYPE_ID,
            "name": "Group Category",
            "attributes": ids
        };
        smallGroup.mapCategories(categoriesJson);
        //double check this stuff after the merge
        smallGroup.attributeTypes = $.extend({}, smallGroup.attributeTypes, categoriesJson);
    //groupAbout
        smallGroup.groupName = this.model.group.groupName;
        smallGroup.groupDescription = this.model.group.groupDescription;

    //groupVisibilityFields
        smallGroup.availableOnline = this.model.group.availableOnline;  
        return smallGroup;

    }

    convertAttributeTypes(list) {
        var results = {};
        _.each(list, function (item) {
            results[item.attributeTypeId] = item;
        });

        return results;
    }

    getCategoryFromId(id) {
        var returnString = '';
        switch (id) {
            case CONSTANTS.ATTRIBUTE_CATEGORY_IDS.LIFE_STAGES:
                returnString = "Life Stage";
                break;
            case CONSTANTS.ATTRIBUTE_CATEGORY_IDS.NEIGHBORHOODS:
                returnString = "Neighborhoods";
                break;
            case CONSTANTS.ATTRIBUTE_CATEGORY_IDS.SPIRITUAL_GROWTH:
                returnString = "Spiritual Growth";
                break;
            case CONSTANTS.ATTRIBUTE_CATEGORY_IDS.INTEREST:
                returnString = "Interest";
                break;
            case CONSTANTS.ATTRIBUTE_CATEGORY_IDS.HEALING:
                returnString = "Healing";
                break;
        }
        return returnString;
    }

    getIdFromCategory(category) {
        var categoryId = null;
        switch (category) {
            case "Life Stage":
                categoryId = CONSTANTS.ATTRIBUTE_CATEGORY_IDS.LIFE_STAGES;
                break;
            case "Neighborhoods":
                categoryId = CONSTANTS.ATTRIBUTE_CATEGORY_IDS.NEIGHBORHOODS;
                break;
            case "Spiritual Growth":
                categoryId = CONSTANTS.ATTRIBUTE_CATEGORY_IDS.SPIRITUAL_GROWTH;
                break;
            case "Interest":
                categoryId = CONSTANTS.ATTRIBUTE_CATEGORY_IDS.INTEREST;
                break;
            case "Healing":
                categoryId = CONSTANTS.ATTRIBUTE_CATEGORY_IDS.HEALING;
                break;
        }
        return categoryId;
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

    getMeetingLocation() {
        let meetingDay = 'Flexible Meeting Time';
        let meetingFreq = _.find(this.meetingFrequencyLookup, (freq) => { return freq.meetingFrequencyId == this.model.group.meeting.frequency });
        if (this.model.specificDay) {
            meetingDay = _.find(this.meetingDaysLookup, (day) => { return day.dp_RecordID == this.model.group.meeting.day });
            return meetingDay.dp_RecordName + '\'s at ' + moment(this.model.group.meeting.time).format('LT') + ', ' + meetingFreq.meetingFrequencyDesc;
        }
        else {
            return meetingDay + ", " + meetingFreq.meetingFrequencyDesc;
        }
    }
}