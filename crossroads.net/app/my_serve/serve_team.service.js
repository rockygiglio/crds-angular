export default class ServeTeamService {
    /*@ngInject*/
    constructor($log) {
        this.log = $log;
    }

    getAllTeamMembersByLeader() {
    return [
        {
            id: 1001,
            name: 'Genie Simmons',
            email: 'gsimmons@gmail.com',
            role: 'Leader'
        },
        {
            id: 1002,
            name: 'Holly Gennaro',
            email: 'hgennaro@excite.com',
            role: null
        },
    ]
    }

    getTeamDetailsByLeader(){
        return [{
                id: 1,
                name: 'FI Oakley Coffee Team',
                count: 43
            },
            {
                id: 2,
                name: 'FI Florence Info Center Team',
                count: 37
            },
            {
                id: 3,
                name: 'Wes Donaldson',
                isLeader: true
            }
        ];
    }
}