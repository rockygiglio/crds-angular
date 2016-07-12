"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require('@angular/core');
var http_1 = require('@angular/http');
var streamspot_service_1 = require('./streamspot.service');
// TODO - placeholder for schedule if StreamspotService fails
var ScheduleComponent = (function () {
    function ScheduleComponent(streamspotService) {
        this.streamspotService = streamspotService;
        this.events = [];
    }
    ScheduleComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.streamspotService.byDate()
            .then(function (events) {
            _this.events = events;
        });
    };
    ScheduleComponent.prototype.dayOfYear = function () {
        return Object.keys(this.events);
    };
    ScheduleComponent.prototype.displayDate = function (dayOfYear, type) {
        var format = 'M/D';
        if (type == 'day') {
            format = "dddd";
        }
        return moment().dayOfYear(dayOfYear).format(format);
    };
    ScheduleComponent = __decorate([
        core_1.Component({
            selector: 'schedule',
            template: "\n    <aside>\n      <div class=\"well\">\n        <h3>Live Stream Schedule</h3>\n        <hr>\n        <div class=\"row\" *ngFor=\"let key of dayOfYear()\">\n          <div class=\"date\">\n            <strong>{{ displayDate(key, 'day') }}</strong>{{ displayDate(key) }}\n          </div>\n          <div class=\"time\">\n            <ul class=\"list-unstyled\">\n              <li *ngFor=\"let event of events[key]\">\n                {{ event.time }}\n              </li>\n            </ul>\n          </div>\n        </div>\n      </div>\n    </aside>\n  ",
            providers: [streamspot_service_1.StreamspotService, http_1.HTTP_PROVIDERS]
        })
    ], ScheduleComponent);
    return ScheduleComponent;
}());
exports.ScheduleComponent = ScheduleComponent;
