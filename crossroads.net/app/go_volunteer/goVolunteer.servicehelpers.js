export function getChildrenDto(data) {
  const childDto = {};

  if (_.has(data, 'attributeId')) {
    childDto.id = data.attributeId;
  }

  if (_.has(data, 'value')) {
    childDto.count = data.value;
  }

  return childDto;
}

export const children = options => _.map(options, c => getChildrenDto(c));

export function createGroupConnector(groupConnector) {
  if (groupConnector === null || groupConnector === undefined) {
    return true;
  }

  return false;
}

export function getEquipmentDto(equip) {
  if (_.has(equip, 'name')) {
    if (equip.name !== null) {
      const equipmentDto = {};
      equipmentDto.notes = equip.name;

      if (_.has(equip, 'attributeId')) {
        equipmentDto.id = equip.attributeId;
      }

      return equipmentDto;
    }
  }
  return null;
}

export function equipment(myEquipment, otherEquipment) {
  const equip = _.map(myEquipment, e => getEquipmentDto(e));
  const other = _.map(otherEquipment, e => getEquipmentDto(e.equipment));
  if (_.isEmpty(other)) {
    return equip;
  }
  return equip.concat(other);
}

export function preferredLaunchSite(site, groupConnector) {
  if (site === null || site === undefined) {
    return { id: 0, name: groupConnector.preferredLaunchSite };
  }

  return { id: site.locationId, name: site.location };
}

export function prepWork(myPrepTime, spousePrepTime) {
  const dto = [];
  if (myPrepTime) {
    const my = { id: myPrepTime.attributeId, spouse: false, name: myPrepTime.name };
    dto.push(my);
  }

  if (spousePrepTime) {
    const spouse = { id: spousePrepTime.attributeId,
      spouse: true,
      name: spousePrepTime.name
    };
    dto.push(spouse);
  }

  return dto;
}

export function projectPreferenceDto(preference, priority) {
  if (preference === null || preference === undefined) {
    return null;
  }

  return { id: preference.projectTypeId,
    priority,
    name: preference.desc
  };
}


export function projectPreferences(pref1, pref2, pref3) {
  const projectPrefs = [];
  if (pref1 !== null) {
    projectPrefs.push(projectPreferenceDto(pref1, 1));
  }

  if (pref2 != null) {
    projectPrefs.push(projectPreferenceDto(pref2, 2));
  }

  if (pref3 != null) {
    projectPrefs.push(projectPreferenceDto(pref3, 3));
  }

  return projectPrefs;
}

export function personDto(person) {
  const dto = {
    lastName: person.lastName,
    dob: person.dateOfBirth,
    mobile: person.mobilePhone
  };

  if (_.has(person, 'contactId')) {
    dto.contactId = person.contactId;
  }

  if (_.has(person, 'nickName')) {
    dto.firstName = person.nickName;
  }

  if (_.has(person, 'preferredName')) {
    dto.firstName = person.preferredName;
  }

  if (_.has(person, 'emailAddress')) {
    dto.emailAddress = person.emailAddress;
  }

  if (_.has(person, 'email')) {
    dto.emailAddress = person.email;
  }

  return dto;
}
