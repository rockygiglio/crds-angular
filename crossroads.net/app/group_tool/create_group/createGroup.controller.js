export default class CreateGroupController {
    /*@ngInject*/
    constructor(Participant, $location, $log) {
        this.log = $log;
        this.log.debug("CreateGroupController constructor");
        this.location = $location;
        this.participantService = Participant;

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

        this.model = {};
        this.fields = [
            {
                key: 'profile.congregationId',
                type: 'select',
                templateOptions: {
                    label: 'What site do you regularly attend service at?',
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
                    label: 'Birth Date',
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
            }, {
                key: 'profile.address.street',
                type: 'input',
                templateOptions: {
                    label: 'Street'
                }
            }, {
                key: 'profile.address.street2',
                type: 'input',
                templateOptions: {
                    label: 'Street 2'
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
                type: 'radio',
                templateOptions: {
                    labelProp: 'label',
                    inline: true,
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
                key: 'group.meeting.frequency',
                type: 'input',
                templateOptions: {
                    label: 'Frequency'
                }
            }, {
                key: 'group.meeting.online',
                type: 'radio',
                templateOptions: {
                    label: 'Where will your group meet?',
                    labelProp: 'label',
                    valueProp: 'online',
                    inline: true,
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
                key: 'group.meeting.address.street2',
                type: 'input',
                templateOptions: {
                    label: 'Street 2'
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
                    inline: true,
                    options: [{
                        label: 'yes',
                        childcare: true
                    },{
                        label: 'no',
                        childcare: false
                    }]
                }
            }, {
                key: 'group.meeting.pets',
                type: 'radio',
                templateOptions: {
                    label: 'Will there be cats or dogs at your meeting place?',
                    labelProp: 'label',
                    valueProp: 'pets',
                    inline: true,
                    options: [{
                        label: 'yes',
                        pets: true
                    },{
                        label: 'no',
                        pets: false
                    }]
                }
            }, {
                key: 'group.startDate',
                type: 'radio',
                templateOptions: {
                    labelProp: 'label',
                    valueProp: 'date',
                    inline: true,
                    options: [{
                        label: 'yes',
                    },{
                        label: 'no',
                        date: Date.now()
                    }]
                }
            }, {
                key: 'group.typeId',
                type: 'radio',
                templateOptions: {
                    labelProp: 'label',
                    valueProp: 'typeId',
                    options: [{
                        label: 'Anyone is welcome',
                        typeId: 0
                    },{
                        label: 'Married couples group',
                        typeId: 1
                    },{
                        label: 'Men only group',
                        typeId: 2
                    },{
                        label: 'Women only group',
                        typeId: 3
                    },{
                        label: 'Coed',
                        typeId: 4
                    }]
                }
            }, {
                key: 'groupAgeRangeIds',
                type: 'multiCheckbox',
                templateOptions: {
                    valueProp: 'groupAgeRangeId',
                    labelProp: 'ageRangeName',
                    options: [{
                            groupAgeRangeId: 1,
                            ageRangeName: 'Middle School Students'
                        }, {
                            groupAgeRangeId: 2,
                            ageRangeName: 'High School Students'
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
          }, {
                key: 'group.groupName',
                type: 'input',
                templateOptions: {
                    label: 'Group Name'
                }
          },{
                key: 'group.groupDescription',
                type: 'textarea',
                templateOptions: {
                    label: 'Group Description'
                }
          },{
                key: 'group.availableOnline',
                type: 'radio',
                templateOptions: {
                    valueProp: 'accessId',
                    labelProp: 'accessLabel',
                    options: [{
                        accessId: 0,
                        accessLabel: 'Public'
                    },{
                        accessId: 1,
                        accessLabel: 'Private'
                    }]
                }
            }
        ];
    }
}