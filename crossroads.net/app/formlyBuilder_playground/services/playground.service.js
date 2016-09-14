export default class PlaygroundService {
    PlaygroundService(fbMapperConfig, $log) {
        this.fbMapperConfig = fbMapperConfig;
        this.log = $log;
    }

    getFields() {
        return [
            {
                formlyConfig: {
                    key: 'person.firstName',
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'First Name',
                        required: true
                    }
                },
                prePopulate: true
            }, {
                formlyConfig: {
                    key: 'person.lastName',
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'Last Name',
                        required: true
                    }
                },
                prePopulate: false
            }, {
                formlyConfig: {
                    key: 'person.nickName',
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'Nick Name',
                        required: true
                    }
                }
            }, {
                formlyConfig: {
                    key: 'person.gender',
                    type: 'formlyBuilderSelect',
                    templateOptions: {
                        label: 'Gender',
                        required: true
                    }
                },
                prePopulate: true
            }, {
                formlyConfig: {
                    key: 'person.site',
                    type: 'formlyBuilderSelect',
                    templateOptions: {
                        label: 'Site',
                        required: true
                    }
                },
                prePopulate: true
            }
        ]
    }
}