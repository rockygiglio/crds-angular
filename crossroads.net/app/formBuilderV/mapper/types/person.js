//import CONSTANTS from '../../constants';

export default class Person {

  constructor(jsonObject) {
    if (jsonObject) {

      Object.assign(this, jsonObject);
    }
  }

}


// {
//     'Object': {
//         'name': '',
//         'saveURL': '',
//         'fields': [
//             {
//                 'data-type': '',
//                 'max-length': '',
//                 'multi-select': '',
//                 'name': '',
//                 'nullable': true,
//                 'saveURL': '',
//                 'validations': [
//                     {
//                         'description': '',
//                         'rule': '',
//                         'type': '',
//                     },
//                     {
//                         'description': '',
//                         'rule': '',
//                         'type': '',
//                     },
//                 ]
//             },
//             {
//                 'data-type': '',
//                 'max-length': '',
//                 'multi-select': '',
//                 'name': '',
//                 'nullable': true,
//                 'saveURL': '',
//                 'validations': [
//                     {
//                         'description': '',
//                         'rule': '',
//                         'type': '',  
//                     },
//                     {
//                         'description': '',
//                         'rule': '',
//                         'type': '',
//                     },
//                 ]
//             },
//         ]
//     }
// }