(function(){
    "use strict";

    const constants = require('../constants');

    require("./leaveyourmark.html");

    let app = angular.module(constants.MODULES.CROSSROADS);

    app.controller("LeaveYourMarkController", require("./leaveyourmark.controller"));
    app.factory("LeaveYourMark", require("./leaveyourmark.service"));

    /**
     * require components
     */
}());