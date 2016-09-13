export default class fbMapperConfig {
    /*@ngInject*/
    constructor($log) {
        this.log = $log;
        this.elementMap = {};
        this.compositionMap = {};
    }

    setElement(element)
    {
        this.elementMap[element.name] = element;
        return this.elementMap[element.name];
    }

    setComposition(composition)
    {
        this.compositionMap[composition.name] = composition;
        return this.compositionMap[composition.name];
    }

    getElement(elementName) {
        return this.elementMap[elementName];
    }

    getComposition(compositionName) {
        return this.compositionMap[compositionName];
    }
}