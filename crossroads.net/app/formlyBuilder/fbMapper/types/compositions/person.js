import CONSTANTS from '../../../../constants';

export default function Person(fbMapperConfig, $resource) {
    fbMapperConfig.setComposition({
        name: 'person',
        elements: [
            "firstName",
            "lastName",
            "nickName",
            "middleName",
            { name: "gender", alias: "genderId" },
            { name: "site", alias: "congregationId"}
        ],
        prePopulate: $resource(__API_ENDPOINT__ + 'api/profile')
    });
}
