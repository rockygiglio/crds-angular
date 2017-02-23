export default class OrganizationsController {
  /* @ngInject */
  constructor() {
    this.viewReady = false;
    this.selectedCity = null;

    this.organizations = [
      {
        name: 'Crossroads Community Church',
        imageUrl: 'https://crds-cms-uploads.imgix.net/content/images/gc-crossroads.jpg',
        cities: [
          {
            name: 'Cincinnati or Central Kentucky Crossroads Sites'
          },
          {
            name: 'Aurora, IL'
          },
          {
            name: 'Columbus, OH'
          },
          {
            name: 'Houston, TX'
          },
          {
            name: 'Seattle, WA'
          }
        ]
      },
      {
        name: 'Archdiocese',
        subtitle: 'of Cincinnati',
        imageUrl: 'https://crds-cms-uploads.imgix.net/content/images/gc-archdiocese.jpg?dpr=2&amp;ixjsv=2.2.3&amp;q=50&amp;w=80'
      },
      {
        name: 'Other',
        subtitle: 'Affiliate or Organization',
        imageUrl: 'https://crds-cms-uploads.imgix.net/content/images/gc-other.jpg?dpr=2&amp;ixjsv=2.2.3&amp;q=50&amp;w=80'
      }
    ];
  }

  $onInit() {
    this.viewReady = true;
  }
}
