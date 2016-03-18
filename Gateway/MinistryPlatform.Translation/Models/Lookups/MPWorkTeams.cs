namespace MinistryPlatform.Translation.Models.Lookups
{
    public class MPWorkTeams
    {
        public MPWorkTeams(int workTeamId, string workTeamName)
        {
            WorkTeamId = workTeamId;
            WorkTeam = workTeamName;
        }

        public int WorkTeamId { get; set; }
        public string WorkTeam { get; set; }
    }
}
