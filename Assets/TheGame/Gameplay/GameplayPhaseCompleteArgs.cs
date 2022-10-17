namespace TheGame.Events
{
    public class GameplayPhaseCompleteArgs : GameEventArgs
    {
        public PlayPhase Phase { get; }
        public IIdentifiers Identifiers { get; }
        public GameplayPhaseCompleteArgs(IIdentifiers identifiers, PlayPhase phase)
        {
            Identifiers = identifiers;
            Phase = phase;
        }
    }
}

