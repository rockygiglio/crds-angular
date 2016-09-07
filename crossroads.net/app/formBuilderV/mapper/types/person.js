//import CONSTANTS from '../../constants';

export default class Person {

    constructor(jsonObject) {
        if (jsonObject) {

            Object.assign(this, jsonObject);
        }
    }

    createTestObject() {
        let mockJSON = {
           
        };

        return new person(mockJSON);
    }

}

