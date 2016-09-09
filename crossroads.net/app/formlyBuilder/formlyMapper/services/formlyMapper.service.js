export default class formlyMapperService {
    /*@ngInject*/
    constructor(formlyMapperConfig, $log, $resource) {
        this.log = $log;
        this.resource = $resource;
        this.formlyMapperConfig = formlyMapperConfig;
    }

    saveFormlyFormData(model) {        
        let promise = this.resource(`${__API_ENDPOINT__}api/formbuilder/hugeEndPoint`)
            .save({}, model).$promise;
        return promise.then(() => {
            this.log.debug("Formly Service save endpoint returned");
        }, (err) => {
            throw err;
        });
    }
}