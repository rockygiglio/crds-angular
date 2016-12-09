import CONSTANTS from 'crds-constants';
import AddEventToolController from '../../../app/mp_tools/add_event_tool/addEventTool.controller';

describe('AddEventTool', () => {
    let fixture,
        log,
        AddEvent,
        rootScope,
        window,
        MPTools,
        AuthService,
        EventService,
        CRDS_TOOLS_CONSTANTS = {
            SECURITY_ROLES: {
                EventsRoomsEquipment: 'test'
            }
        };

    beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

    beforeEach(inject(($injector) => {
        log = $injector.get('$log');
        rootScope = jasmine.createSpyObj('$rootScope', ['$emit']);
        window = $injector.get('$window');
        MPTools = jasmine.createSpyObj('MPTools', ['getParams']);
        AuthService = jasmine.createSpyObj('AuthService', ['isAuthenticated', 'isAuthorized']);
        //AddEvent = jasmine.createSpyObj('AddEvent', ['test']);
        AddEvent = {
            currentPage: 5,
            editMode: true,
            eventData: {
            }
        };
        EventService = $injector.get('EventService');
        CRDS_TOOLS_CONSTANTS = {};

        fixture = new AddEventToolController(rootScope, window, log, MPTools, AuthService, EventService, CRDS_TOOLS_CONSTANTS, AddEvent);
    }));

    it('should go back()', () => {
        fixture.back();
        expect(fixture.AddEvent.currentPage).toBe(1);
    });

    it('should return correct currentPage()', () => {
        let curPage = fixture.currentPage();
        expect(curPage).toBe(5);
    });

    it('should return if isEditMode()', () => {
        let editMode = fixture.isEditMode();
        expect(editMode).toBe(true);
    });

    it('should move page to next()', () => {
        fixture.allData = {
            eventForm: {
                $setSubmitted: () => {
                    return;
                },
                $valid: true,
                maximumChildren: {
                    $valid: true
                },
                minimumChildren: {
                    $valid: true
                }
            }
        };
        fixture.event = {
            test: 'test'
        };
        fixture.next();
        expect(fixture.AddEvent.currentPage).toBe(2);
        expect(fixture.AddEvent.eventData.event.test).toBe('test');
    });

    it('should error when trying to move page to next()', () => {
        fixture.allData = {
            eventForm: {
                $setSubmitted: () => {
                    return;
                },
                $valid: false,
                maximumChildren: {
                    $valid: true
                },
                minimumChildren: {
                    $valid: true
                }
            }
        };
        fixture.event = {
            test: 'test'
        }
        fixture.rootScope.MESSAGES = {
            generalError: 'error'
        };
        fixture.next();
        expect(fixture.AddEvent.currentPage).toBe(5);
        expect(fixture.AddEvent.eventData.event.test).toBe('test');
        expect(fixture.rootScope.$emit).toHaveBeenCalled();
    });

    describe('Submit()', () => {
        beforeEach(() => {
            fixture.AddEvent.getEventDto = (data) => {
                return {
                    startDateTime: '9:30:00',
                    endDateTime: '10:30:00'
                };
            };
            fixture.allData = {
                roomForm: {
                    $setSubmitted: () => {
                        return;
                    },
                    equipmentForm: {
                        $setSubmitted: () => {
                            return;
                        }
                    }
                },
                $valid: true
            };
            fixture.rooms = [];
        });

        it('should submit() as save mode', () => {
            fixture.AddEvent.editMode = false;
            fixture.processSave = jasmine.createSpy('processSave');
            fixture.submit();
            expect(fixture.processSave).toHaveBeenCalled();
            expect(fixture.rootScope.$emit).not.toHaveBeenCalled();
            expect(fixture.processing).toBe(true);
        });

        it('should submit() as edit mode', () => {
            fixture.AddEvent.editMode = true;
            fixture.processEdit = jasmine.createSpy('processEdit');
            fixture.submit();
            expect(fixture.processEdit).toHaveBeenCalled();
            expect(fixture.rootScope.$emit).not.toHaveBeenCalled();
            expect(fixture.processing).toBe(true);
        });

        it('should not submit() it is invalid', () => {
            fixture.allData.$valid = false;
            fixture.rootScope.MESSAGES = {
                generalError: 'error'
            };
            fixture.submit();
            expect(fixture.rootScope.$emit).toHaveBeenCalled();
            expect(fixture.processing).toBe(false);
        });
    });

    describe('processEdit', () => {
        beforeEach(() => {
            fixture.processing = true;
            fixture.rootScope.MESSAGES = {
                generalError: 'error',
                eventUpdateSuccess: 'success',
                eventToolProblemSaving: 'err'
            };
            spyOn(window, 'close');
        });

        it('should processEdit()', () => {
            let expectedResult = {};
            spyOn(EventService.eventTool, 'update').and.callFake((data, event, callbackFn, errCallbackFn) => {
                callbackFn(expectedResult);
            });

            fixture.processEdit({});
            expect(fixture.processing).toBe(false);
            expect(fixture.rootScope.$emit).toHaveBeenCalled();
            expect(fixture.AddEvent.eventData).toEqual({});
            expect(window.close).toHaveBeenCalled();
        })

        it('should processEdit() with err', () => {
            let err = {};
            spyOn(EventService.eventTool, 'update').and.callFake((data, event, callbackFn, errCallbackFn) => {
                errCallbackFn(err);
            });

            fixture.processEdit({});
            expect(fixture.processing).toBe(false);
            expect(fixture.rootScope.$emit).toHaveBeenCalled();
        })
    })

    describe('processSave', () => {
        beforeEach(() => {
            fixture.processing = true;
            fixture.rootScope.MESSAGES = {
                generalError: 'error',
                eventSuccess: 'success',
                eventToolProblemSaving: 'err'
            };
            spyOn(window, 'close');
        });

        it('should processSave()', () => {
            let expectedResult = {};
            spyOn(EventService.create, 'save').and.callFake((event, callbackFn, errCallbackFn) => {
                callbackFn(expectedResult);
            });

            fixture.processSave({});
            expect(fixture.processing).toBe(false);
            expect(fixture.rootScope.$emit).toHaveBeenCalled();
            expect(fixture.AddEvent.eventData.event).toEqual({});
            expect(window.close).toHaveBeenCalled();
            expect(fixture.rooms).toEqual([]);
            expect(fixture.event).toEqual({});
            expect(fixture.AddEvent.currentPage).toBe(1);
        })

        it('should processSave() with err', () => {
            let err = {};
            spyOn(EventService.create, 'save').and.callFake((event, callbackFn, errCallbackFn) => {
                errCallbackFn(err);
            });

            fixture.processSave({});
            expect(fixture.processing).toBe(false);
            expect(fixture.rootScope.$emit).toHaveBeenCalled();
        })
    })

    describe('AllowAccess()', () => {

        beforeEach(() => {
            fixture.CRDS_TOOLS_CONSTANTS = {
                SECURITY_ROLES: {
                    EventsRoomsEquipment: 'test'
                }
            };
        });

        it('should allowAccess()', () => {
            AuthService.isAuthenticated.and.returnValue(true);
            AuthService.isAuthorized.and.returnValue(true);

            let allowAccessReturn = fixture.allowAccess();
            expect(allowAccessReturn).toBe(true)
        });

        it('should not allowAccess() because not authenticated', () => {
            AuthService.isAuthenticated.and.returnValue(false);
            AuthService.isAuthorized.and.returnValue(true);

            let allowAccessReturn = fixture.allowAccess();
            expect(allowAccessReturn).toBe(false)
        });

        it('should not allowAccess() because not authorized', () => {
            AuthService.isAuthenticated.and.returnValue(true);
            AuthService.isAuthorized.and.returnValue(false);

            let allowAccessReturn = fixture.allowAccess();
            expect(allowAccessReturn).toBe(false)
        });

        it('should not allowAccess() because not authenticated or authorized', () => {
            AuthService.isAuthenticated.and.returnValue(false);
            AuthService.isAuthorized.and.returnValue(false);

            let allowAccessReturn = fixture.allowAccess();
            expect(allowAccessReturn).toBe(false)
        });
    });
});