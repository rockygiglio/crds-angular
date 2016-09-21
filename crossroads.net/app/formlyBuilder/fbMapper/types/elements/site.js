export default function Gender(fbMapperConfig) {
    var getModel = () => {
        return {
            "data-type": "string",
            "max-length": 50,
            "name": "site",
            "nullable": true,
            "validations": [
                {
                    "description": "required to save",
                    "rule": "required"
                }
            ],
            "lookup": {
                "location": "api/lookup/sites",
                "valueProp": "dp_RecordID",
                "labelProp": "dp_RecordName"
            }
        };
    }
    fbMapperConfig.setElement({
        name: 'site',
        model: getModel()
    });
}