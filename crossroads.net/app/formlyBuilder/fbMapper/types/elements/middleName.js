export default function addMiddleName(fbMapperConfig) {
    var getModel = () => {
        return {
            "data-type": "string",
            "max-length": 50,
            "name": "lastName",
            "nullable": true,
            "validations": [
                {
                    "description": "required to save",
                    "rule": "required"
                },
                {
                    "description": "max length",
                    "rule": 45
                }
            ]
        };
    };

    var defaultComposition = () => {
        return {
            name: "middleName"
        };
    };

    fbMapperConfig.setElement({
        name: 'middleName',
        model: getModel(),
        defaultComposition: defaultComposition;
    });
}
