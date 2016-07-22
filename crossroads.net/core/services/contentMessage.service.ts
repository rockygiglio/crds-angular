import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class ContentMessageService {

    private MESSAGES = undefined;

    private messages_subject = new Subject();

    get() {
        if (this.MESSAGES === undefined) {
            return this.messages_subject.asObservable();
        } else {
            return Observable.of(this.MESSAGES);
        }
    }

    set(messages: any) {
        this.MESSAGES = messages;
        this.messages_subject.next(messages);
    }
}
