export default function FirstName(fbMapperConfig) {
    var getModel = () => {
        return {
            "data-type": "string",
            "max-length": 50,
            "name": "firstName",
            "nullable": true,
            "validations": [
                {
                    "description": "required to save",
                    "rule": "required"
                },
                {
                    "description": "max length",
                    "rule": 40
                }
            ]
        };
    };
    
    fbMapperConfig.setElement({
        name: 'firstName',
        model: getModel()
    });
}