export default class PlaygroundService {
    PlaygroundService(fbMapperConfig, $log) {
        this.fbMapperConfig = fbMapperConfig;
        this.log = $log;
    }

    getFields() {
        return [
            {
                key: 'person.firstName',
                type: 'formlyBuilderInput',
                templateOptions: {
                    label: 'First Name',
                    required: true
                }
            }, {
                key: 'person.lastName',
                type: 'formlyBuilderInput',
                templateOptions: {
                    label: 'Last Name',
                    required: true
                }
            }, {
                key: 'person.preferredName',
                type: 'formlyBuilderInput',
                templateOptions: {
                    label: 'Preferred Name',
                    required: true
                }
            }
        ]
    }
}