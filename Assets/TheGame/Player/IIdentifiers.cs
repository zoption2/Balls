namespace TheGame
{
    public interface IIdentifiers
    {
        int ID { get; }
        int TeamID { get; }
    }

    public struct Identifiers : IIdentifiers
    {
        public int ID { get; private set; }
        public int TeamID { get; private set; }

        public Identifiers(int id, int teamID)
        {
            ID = id;
            TeamID = teamID;
        }
    }
}

