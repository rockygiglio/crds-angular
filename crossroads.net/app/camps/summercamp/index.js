import SummerCampComponent from './summercamp.component';
import constants from '../../constants'


angular.module(constants.MODULES.CAMPS)
    .component('summerCamp', SummerCampComponent)
    .controller('SummercampController',SummercampController);
require('./summercamp.html');


