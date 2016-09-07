
export default class PreferredName {

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
                'name': 'preferredName',
                'nullable': true,
                'validations': [
                    {
                        'description': 'required to save',
                        'rule': 'required',
                    },
                    {
                        'description': 'max length',
                        'rule': 30,
                    },
                ]
            };

        return new PreferredName(mockJSON);
    }

}

