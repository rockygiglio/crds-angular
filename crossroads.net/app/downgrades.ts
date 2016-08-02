import { upgradeAdapter } from './upgrade-adapter';
import { Ng2TestComponent } from './ng2test/ng2test.component';
import { StreamingComponent } from './streaming/streaming.component';
import { DynamicContentNg2Component } from '../core/dynamic_content/dynamic-content-ng2.component';
import { ContentMessageService } from '../core/services/contentMessage.service';
import { VideoComponent } from './streaming/video.component';
import { PageScroll }  from './ng2-page-scroll/ng2-page-scroll.component';

declare let angular:any;

angular.module('crossroads')
    .directive('ng2Test', upgradeAdapter.downgradeNg2Component(Ng2TestComponent))
    .directive('streaming', upgradeAdapter.downgradeNg2Component(StreamingComponent))
    .directive('dynamic-content-ng2', upgradeAdapter.downgradeNg2Component(DynamicContentNg2Component))
    .directive('streamingVideo', upgradeAdapter.downgradeNg2Component(VideoComponent))
    .directive('pageScroll', upgradeAdapter.downgradeNg2Component(PageScroll));

upgradeAdapter.addProvider(ContentMessageService);
angular.module('crossroads')
  .factory('contentMessageService', upgradeAdapter.downgradeNg2Provider(ContentMessageService));
