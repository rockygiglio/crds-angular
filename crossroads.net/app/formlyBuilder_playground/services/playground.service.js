export default class PlaygroundService {
    PlaygroundService(fbMapperConfig, $log) {
        this.fbMapperConfig = fbMapperConfig;
        this.log = $log;
    }

    getFields() {
        return [
            {
                mapperSuperPath: 'person.firstName',
                prePopulate: true,
                formlyConfig: {
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'First Name',
                        required: true
                    }
                }
            }, {
                mapperSuperPath: 'person.lastName',
                prePopulate: false,
                formlyConfig: {
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'Last Name',
                        required: true
                    }
                }
            }, {
                mapperSuperPath: 'person.nickName',
                formlyConfig: {
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'Nick Name',
                        required: true
                    }
                }
            }, {
                mapperSuperPath: 'person.gender',
                prePopulate: true,
                formlyConfig: {
                    type: 'formlyBuilderSelect',
                    templateOptions: {
                        label: 'Gender',
                        required: true
                    }
                }
            }, {
                mapperSuperPath: 'person.site',
                prePopulate: true,
                formlyConfig: {
                    type: 'formlyBuilderSelect',
                    templateOptions: {
                        label: 'Site',
                        required: true
                    }
                }
            }
        ]
    }
}