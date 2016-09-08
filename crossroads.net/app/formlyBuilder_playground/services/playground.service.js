export default class PlaygroundService {
    PlaygroundService(formlyMapperConfig, $log) {
        this.formlyMapperConfig = formlyMapperConfig;
        this.log=$log;
    }

    getFields(){
        return [
            {
                name: 'person.firstName',
                formlyConfig: {
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'First Name',
                        required: true
                    }
                }
            },{
                name: 'person.lastName',
                formlyConfig: {
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'Last Name',
                        required: true
                    }
                }
            },{
                name: 'person.preferredName',
                formlyConfig: {
                    type: 'formlyBuilderInput',
                    templateOptions: {
                        label: 'Preferred Name',
                        required: true
                    }
                }
            }
        ]
    }
}