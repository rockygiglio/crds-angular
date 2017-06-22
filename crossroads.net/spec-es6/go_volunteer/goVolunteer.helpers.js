export default {
  person: {
    firstName: 'Matt',
    nickName: 'Matt',
    lastName: 'LastName',
    emailAddress: 'something@test',
    dateOfBirth: '11/02/1980',
    mobilePhone: '513-111-1111'
  },
  arch_organization: {
    contactId: 0,
    endDate: '0001-01-01T00:00:00',
    name: 'Archdiocese of Cincinnati',
    openSignup: false,
    organizationId: 2,
    startDate: '2016-02-29T00:00:00'
  },
  other_organization: {
    contactId: 0,
    endDate: '0001-01-01T00:00:00',
    name: 'other',
    openSignup: true,
    organizationId: 2,
    startDate: '2016-02-29T00:00:00'
  },
  cities: [
    {
      projectId: 123,
      city: 'Cleveland',
      state: 'OH'
    },
    {
      projectId: 980,
      city: 'Phoenix',
      state: 'AZ'
    },
    {
      projectId: 88,
      city: 'Atlanta',
      state: 'GA'
    }
  ],
  organizations: [
    {
      name: 'Crossroads',
      imageURL: 'http://blah.com/image.jpg'
    },
    {
      name: 'Other',
      imageURL: 'http://blah.com/image2.jpg'
    },
    {
      name: 'Archdiocese of Cincinnati',
      imageURL: 'http://blah.com/image3.jpg'
    }
  ],
  crossroads_organization: {
    contactId: 0,
    endDate: '0001-01-01T00:00:00',
    name: 'crossroads',
    openSignup: true,
    organizationId: 2,
    startDate: '2016-02-29T00:00:00'
  },
  crossroads_group_connectors: [
    {
      name: 'Canterbury, Andy ',
      organizationId: 6,
      preferredLaunchSite: 'Anywhere',
      projectName: 'Annette\'s Defect Party'
    },
    {
      name: 'Maddox, TJ',
      organizationId: 1,
      preferredLaunchSite: 'Anywhere',
      projectName: null
    }
  ],
  otherOrganizations: [
    { id: 32, name: 'Peoples Church' },
    { id: 31, name: 'Prince of Peace' },
    { id: 30, name: 'Quinn Chapel AME Church' },
    { id: 29, name: 'Redeemer' },
    { id: 28, name: 'Redemption bible church' },
    { id: 27, name: 'Revival Center Ministries' },
    { id: 26, name: 'River of life' },
    { id: 25, name: 'Rivers Crossing Community Church' },
    { id: 24, name: 'Rockdale Temple' },
    { id: 23, name: 'Seven Hills' },
    { id: 22, name: 'Sharonville United Method' },
    { id: 21, name: 'Solid Rock' },
    { id: 20, name: 'Southern baptist' },
    { id: 19, name: 'Southwest Church' },
    { id: 18, name: 'Summit Church of Christ' },
    { id: 17, name: 'Sycamore Presbyterian Church' },
    { id: 16, name: 'The Church of Jesus Christ of Latter-day Saints' },
    { id: 15, name: 'Trinity Christian' },
    { id: 14, name: 'Tryed Stone New Beginning Church' },
    { id: 13, name: 'Urbancrest Baptist' },
    { id: 12, name: 'Vineyard' },
    { id: 11, name: 'Vineyard Cincinnati' },
    { id: 10, name: 'Vineyard in eastgate' },
    { id: 9, name: 'Vineyard Westside' },
    { id: 8, name: 'Wellspring Community Church' },
    { id: 7, name: 'White Oak Christian Church' },
    { id: 6, name: 'Whitewater Crossing' },
    { id: 2, name: 'Xenia Nazarene' },
    { id: 1, name: 'Xenos Christian Fellowship ' }
  ],
  equipment: [
    { id: 500, name: 'Shovel' },
    { id: 501, name: 'Truck' },
    { id: 502, name: 'Bush Hog' },
    { id: 503, name: 'Hammer ' }
  ],
  prepWork: [
    { id: 230, name: 'Preparation Work 12PM - 3PM' },
    { id: 231, name: 'Preparation Work 12PM - 6PM' },
    { id: 232, name: 'Preparation Work 3PM - 6PM' },
    { id: 233, name: 'Preparation Work 6PM - 9PM' },
    { id: 234, name: 'Preparation Work 3PM - 9PM' },
  ],
  project: {
    contact: 'Matt Silbernagel',
    email: 'matt.silbernagel@ingagepartners.com',
    contactId: 2186211,
    projectId: 564,
    projectName: 'Make Cleveland Great (Again?)',
    projectStatusId: 1,
    locationId: 12,
    projectTypeId: 3,
    projectType: 'Gardening',
    organizationId: 2,
    initiativeId: 3,
    addressId: 5299057,
    location: 'Cleveland, OH'
  },
  participants: [
    { name: 'Jenny Shultz', email: 'jshultz@hotmail.com', phone: '205-333-5962', adults: 1, children: 2 },
    { name: 'Jamie Hanks', email: 'jaha95@gmail.com', phone: '205-333-5962', adults: 0, children: 2 },
    { name: 'Jennie Jones', email: 'jjgirl@yahoo.com', phone: '205-334-5988', adults: 2, children: 0 },
    { name: 'Jimmy Hatfield', email: 'jhattyhat@yahoo.com', phone: '205-425-5772', adults: 0, children: 2 },
    { name: 'Elisha Underwood', email: 'eu@yahoo.com', phone: '205-259-2777', adults: 0, children: 2 },
    { name: 'Terry Washington', email: 'tdub777@yahoo.com', phone: '205-259-8954', adults: 1, children: 1 },
    { name: 'Jim Wolf', email: 'dwolf@yahoo.com', phone: '205-334-9584', adults: 1, children: 2 }
  ]
};
