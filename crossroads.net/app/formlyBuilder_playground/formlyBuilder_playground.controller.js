export default class PlaygroundController {
    constructor() {
        this.fields = [{
            key: 'profile.addressLine1',
            type: 'formlyBuilderInput',
            templateOptions: {
                label: 'Street',
                required: true,
            }
        }];
    }

    getFields(){
        return [{
            key: 'person.firstName',
            type: 'formlyBuilderInput',
            templateOptions: {
                label: 'First Name',
                required: true,
            }
        }];
    }
}