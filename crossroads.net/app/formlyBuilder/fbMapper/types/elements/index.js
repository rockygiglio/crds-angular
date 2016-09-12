import addLastName from './lastName';
import addFirstName from './firstName';
import addPreferredName from './preferredName';

export default ngModule => {
    ngModule.run(addLastName);
    ngModule.run(addFirstName);
    ngModule.run(addPreferredName);
}