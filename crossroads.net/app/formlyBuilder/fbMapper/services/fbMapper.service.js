export default class fbMapperService {
    /*@ngInject*/
    constructor(fbMapperConfig, $log, $resource) {
        this.log = $log;
        this.resource = $resource;
        this.fbMapperConfig = fbMapperConfig;
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

    //This takes an array of composition Names
    prepopulateCompositions(compositions){
        var compositions = fbMapperConfig.getComposition(composition);
        

    }
}