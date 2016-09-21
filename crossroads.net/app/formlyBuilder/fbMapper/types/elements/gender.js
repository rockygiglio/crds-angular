export default function Gender(fbMapperConfig) {
    var getModel = () => {
        return {
            "data-type": "string",
            "max-length": 50,
            "name": "gender",
            "nullable": true,
            "validations": [
                {
                    "description": "required to save",
                    "rule": "required"
                }
            ],
            "lookup": {
                "location": "api/lookup/genders",
                "valueProp": "dp_RecordID",
                "labelProp": "dp_RecordName"
            }
        }
    }
    fbMapperConfig.setElement({
        name: 'gender',
        model: getModel()
    });
}