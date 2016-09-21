export default function addNickName(fbMapperConfig) {
    var getModel = () => {
        return {
            "data-type": "string",
            "max-length": 50,
            "name": "nickName",
            "nullable": true,
            "validations": [
                {
                    "description": "required to save",
                    "rule": "required"
                },
                {
                    "description": "max length",
                    "rule": 30
                }
            ]
        };
    };

    fbMapperConfig.setElement({
        name: 'nickName',
        model: getModel()
    });
}