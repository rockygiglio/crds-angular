//our root app component
import {
  Component,
  ComponentFactory,
  ComponentMetadata,
  ComponentResolver,
  Input,
  ReflectiveInjector,
  ViewContainerRef,
  OnChanges
  
} from '@angular/core'

import { ContentMessageService } from '../services/contentMessage.service';

export function createComponentFactory(resolver: ComponentResolver, metadata: ComponentMetadata): Promise<ComponentFactory<any>> {
    const cmpClass = class DynamicComponent {};
    const decoratedCmp = Component(metadata)(cmpClass);
    return resolver.resolveComponent(decoratedCmp);
}

@Component({
    selector: 'dynamic-content-ng2',
    template: ``
})
export class DynamicContentNg2Component implements OnChanges {
  @Input() title: string;

  constructor(private vcRef: ViewContainerRef, private resolver: ComponentResolver, private cms: ContentMessageService) {
  }

  ngOnChanges() {

    if (!this.title) return;

    this.cms.get().subscribe(
      messages => {
        const metadata = new ComponentMetadata({
          selector: 'dynamic-html',
          template: messages[this.title].content,
        });
        createComponentFactory(this.resolver, metadata)
          .then(factory => {
            const injector = ReflectiveInjector.fromResolvedProviders([], this.vcRef.parentInjector);
            this.vcRef.createComponent(factory, 0, injector, []);
          });
      }

    );

  }
}