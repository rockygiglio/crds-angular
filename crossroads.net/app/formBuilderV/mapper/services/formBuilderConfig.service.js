
export default class formBuilderConfig {
    /*@ngInject*/
    constructor($log) {
        this.log = $log;
        this.elementMap = {};
        this.compositionMap = {};
    }

    setElement(element)
    {
        //check check it yeah. 
        this.elementMap[element.name] = element;
        return this.elementMap[element.name];
    }

    setComposition(composition)
    {
        this.compositionMap[composition.name] = composition;
        return this.compositionMap[composition.name];
    }
}