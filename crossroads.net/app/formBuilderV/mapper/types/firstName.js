
export default class FirstName {

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
                'name': 'firstName',
                'nullable': true,
                'validations': [
                    {
                        'description': 'required to save',
                        'rule': 'required',
                    },
                    {
                        'description': 'max length',
                        'rule': 40,
                    },
                ]
            };

        return new FirstName(mockJSON);
    }

}

