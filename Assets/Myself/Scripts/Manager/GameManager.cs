using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{

    public CharacterStates playerStates;
    private CinemachineFreeLook followCamera;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    /// <summary>
    /// 注册玩家的数据CharacterStates数据，让GameManager获取这些数据，同时让摄像机跟随玩家。
    /// </summary>
    /// <param name="player">要注册的玩家状态</param>
    public void RigisterPlayer(CharacterStates player)
    {
        playerStates = player;

        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if (followCamera != null)
        {
            followCamera.Follow = playerStates.transform.GetChild(2);
            followCamera.LookAt = playerStates.transform.GetChild(2);
        }
    }
    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }
    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }
    public void NotifyObservers()
    {
        foreach(var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach(var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.C)
            {
                return item.transform;
            }
            
        }
        return null;
    }
}
