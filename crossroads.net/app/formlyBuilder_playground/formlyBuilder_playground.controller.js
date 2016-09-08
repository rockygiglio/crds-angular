export default class PlaygroundController {
    constructor() {
    }

    getFields() {
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