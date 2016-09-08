export default class PlaygroundController {
    constructor() {
    }

    getFields() {
        return [{
            key: 'profile.addressLine1',
            type: 'formlyBuilderInput',
            templateOptions: {
                label: 'Street',
                required: true,
            }
        }];
    }
}