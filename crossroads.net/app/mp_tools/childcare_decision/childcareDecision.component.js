import controller  from './childcareDecision.controller';

ChildcareDecisionComponent.$inject = [ ];

function ChildcareDecisionComponent() {

    let ChildcareDecisionComponent = {
        restrict: 'E',
        templateUrl: 'childcare_decision/childcareDecision.html',
        controller: controller,
        controllerAs: 'decision',
        bindToController: true
    };

    return ChildcareDecisionComponent;

}
export default ChildcareDecisionComponent;

