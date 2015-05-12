'use strict';

var app = require("angular").module('crossroads');

require('./corkboard-listings.html');
require('./corkboard-listing-detail.html');
require('./corkboard-create.html');
require('./forms/give-something.html');
require('./forms/post-event.html');
require('./forms/post-job.html');
require('./forms/post-need.html');

app.controller("CorkboardCtrl", ['$log', '$state', require("./corkboard.controller")]);