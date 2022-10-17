using Zenject;
using TheGame;
using TheGame.Events;
using UnityEngine;

public class ServicesInstaller : MonoInstaller
{
    [SerializeField] private MonoReferences references;

    public override void InstallBindings()
    {
        BindServices();
        BindToNewGameObjects();
        BindMonoReferences();

    }

    private void BindServices()
    {
        Container.Bind<IGlobalFixedUpdateProvider>().To<GlobalFixedUpdateProvider>().FromNewComponentOnNewGameObject().AsSingle();
        Container.Bind<IGlobalUpdateProvider>().To<GlobalUpdateProvider>().FromNewComponentOnNewGameObject().AsSingle();
        Container.Bind<IPoolController>().To<PoolController>().AsSingle();
        Container.Bind<IGameplayController>().To<GameplayController>().AsSingle();

        Container.BindFactory<Player
                             , GameplayController
                             , GameplayController.IndividualSubcontroller
                             , GameplayController.IndividualSubcontroller.Factory>()
                             .AsSingle();
        Container.Bind<IEventBus>().To<EventBus>().AsSingle();
    }

    private void BindToNewGameObjects()
    {
        Container.Bind<IMonoInstantiator>().To<MonoInstantiator>().FromNewComponentOnNewGameObject().AsSingle();
    }

    private void BindMonoReferences()
    {
        Container.Bind<IBallsFactory>().To<BallsFactory>().FromInstance(references.BallsFactory).AsSingle();
        Container.Bind<IFieldFactory>().To<FieldFactory>().FromInstance(references.FieldsFactory).AsSingle();
        Container.Bind<IPlayerFactory>().To<PlayerFactory>().FromInstance(references.PlayerFactory).AsSingle();
    }
}


