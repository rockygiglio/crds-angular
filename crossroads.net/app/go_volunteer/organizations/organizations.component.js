import controller from './organizations.controller';
import template from './organizations.template.html';

export default function organizationsComponent() {
  let component = {
    template,
    controller
  };

  return component;
}
