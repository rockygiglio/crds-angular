import CONSTANTS from '../../../../constants';

export default function Person(fbMapperConfig, $resource) {
    fbMapperConfig.setComposition({
        name: 'person',
        elements: [
            {
                name: "firstName",    
            },
            {
                name: "lastName"
            },
            {
                name: "nickName"
            },
            {
                name: "middleName"
            },
            {
                name: "gender",
                alias: "genderId"
            },
            {
                name: "site",
                alias: "congregationId"
            }],
        prePopulate: $resource(__API_ENDPOINT__ + 'api/profile')
    });
}