//import CONSTANTS from '../../constants';

export default class Person {

    constructor(jsonObject) {
        if (jsonObject) {

            Object.assign(this, jsonObject);
        }
    }

    createTestObject() {
        let mockJSON = {
            'Person': {
                'saveURL': '/api/formBuilder/saveStrategy',
                'fields': [
                    {
                        'data-type': 'string',
                        'max-length': 50,
                        'multi-select': false,
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
                    },
                    {
                        'data-type': 'string',
                        'max-length': 50,
                        'multi-select': false,
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
                    },
                    {
                        'data-type': 'string',
                        'max-length': 50,
                        'multi-select': false,
                        'name': 'nickName',
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
                    },
                ]
            }
        };

        return new person(mockJSON);
    }

}

