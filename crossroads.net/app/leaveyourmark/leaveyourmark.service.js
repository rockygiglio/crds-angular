(function() {
    "use strict";

    module.exports = LeaveYourMark;

    LeaveYourMark.$inject = ['$resource'];

    function LeaveYourMark($resource) {
        return {
            campaignSummary: $resource(__API_ENDPOINT__ + "api/campaign/summary/:pledgeCampaignId", {
                pledgeCampaignId: "@pledgeCampaignId"
            })
        }
    }
}())