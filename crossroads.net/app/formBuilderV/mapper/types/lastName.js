
export default class LastName {

    constructor(jsonObject) {
        if (jsonObject) {

            Object.assign(this, jsonObject);
        }
    }

    createTestObject() {
        let mockJSON =
            {
                'data-type': 'string',
                'max-length': 50,
                'name': 'lastName',
                'nullable': true,
                'validations': [
                    {
                        'description': 'required to save',
                        'rule': 'required',
                    },
                    {
                        'description': 'max length',
                        'rule': 45,
                    },
                ]
            };

        return new LastName(mockJSON);
    }

}

