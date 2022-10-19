namespace TheGame.Events
{
    public class GameplayPhaseCompleteArgs : GameEventArgs
    {
        public RoundStates Phase { get; }
        public IIdentifiers Identifiers { get; }
        public GameplayPhaseCompleteArgs(IIdentifiers identifiers, RoundStates phase)
        {
            Identifiers = identifiers;
            Phase = phase;
        }
    }
}

