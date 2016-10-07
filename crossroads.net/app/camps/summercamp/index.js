import SummerCampComponent from './summercamp.component';
import constants from '../../constants';


angular.module(constants.MODULES.CAMPS)
    .component('summerCamp', SummerCampComponent);
    
require('./summercamp.html');
require('./signup_intro/signup_intro.html');
require('./signup_intro')

