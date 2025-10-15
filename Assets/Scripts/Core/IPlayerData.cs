namespace BasketLegend.Core
{
    public interface IPlayerData
    {
        string PlayerName { get; set; }
        int PlayerLevel { get; set; }
        int Experience { get; set; }
        int UnlockedLevels { get; set; }
        void SaveData();
        void LoadData();
    }
}