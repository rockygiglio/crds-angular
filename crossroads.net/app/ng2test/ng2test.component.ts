import { Component } from '@angular/core';
import { DynamicContentNg2Component } from '../../core/dynamic_content/dynamic-content-ng2.component'
declare var moment: any;

@Component({
  selector: 'ng2-test',
  template: `
    <h2>Welcome from our first angular2 component</h2>
    <h2>{{today}}</h2>
    <dynamic-content-ng2 title="groupFinderHostExpectations"></dynamic-content-ng2>
    <dynamic-content-ng2 title="cmsChildcareEventReminder"></dynamic-content-ng2>
  `,
  directives: [DynamicContentNg2Component]
})
export class Ng2TestComponent {
  today: string = moment().format('D MMM YYYY');
  html: string = `<div>
    <p>Dynamic HTML Fragment</p>
    <div *ngIf="false">Hello</div>
    <a href="anywhere"> <img src="https://crds-cms-uploads.imgix.net/content/images/anywhere.jpg" class="img-responsive img-full-width" alt="" title="" /><span>Crossroads Anywhere</span> </a>
    <script>alert("evil never sleeps")</script>
</div>`

}
