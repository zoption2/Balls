using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TheGame;

public class ChooseSceneInstantiator : MonoInstaller
{
    [SerializeField] private SelectPlayerPanel selectPlayerPanel;
    [SerializeField] private Lobby lobby;

    public override void InstallBindings()
    {
        Container.Bind<ISelectPlayer>().FromInstance(selectPlayerPanel).AsSingle();
        Container.Bind<Lobby>().FromInstance(lobby).AsSingle();
    }
}
