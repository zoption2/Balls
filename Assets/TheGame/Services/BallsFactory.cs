namespace TheGame
{
    public class BallsFactory : AddressablesFactory<BallBase, BallType>, IBallsFactory
    {

    }

    public interface IBallsFactory : IAddressableFactory<BallBase, BallType>
    {

    }
}

