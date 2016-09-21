export default function addLastName(fbMapperConfig) {
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
            name: "lastName"
        };
    };

    fbMapperConfig.setElement({
        name: 'lastName',
        model: getModel(),
        defaultComposition: defaultComposition;
    });
}
