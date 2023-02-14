using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    GameObject player;
    NavMeshAgent playerAgent;
    public GameObject playerPrefab;
    public SceneFade sceneFadePrefab;
    bool fadeFinished;

    private void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinished = true;
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DiffentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }
    IEnumerator Transition(string sceneName,TransitionDestination.DestinationTag destinationTag)
    {
        //TODO:保存数据
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveUIData();
        QuestManager.Instance.SaveQuestManager();

        if(SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            SaveManager.Instance.LoadPlayerData();
            yield break;
        }
        else 
        { 
        player = GameManager.Instance.playerStates.gameObject;
        playerAgent = player.GetComponent<NavMeshAgent>();
        playerAgent.enabled = false;
        player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
        playerAgent.enabled = true;
        yield return null;
        }
    }
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        foreach(var i in entrances)
        {
            if(i.destinationTag == destinationTag)
            {
                return i;
            }
        }
        return null;
    }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("SampleScene"));
    }

    public void TransitionToLoadGame()
    {

        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }
    IEnumerator LoadLevel(string scene)
    {
        SceneFade fade = Instantiate(sceneFadePrefab);
        if(scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab,GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            //保存数据
            SaveManager.Instance.SavePlayerData();
            InventoryManager.Instance.SaveUIData();
            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }
    }
    IEnumerator LoadMain()
    {
        SceneFade fade = Instantiate(sceneFadePrefab);
        yield return StartCoroutine(fade.FadeOut(2f));
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(fade.FadeIn(2f));
        yield break;
    }
    public void EndNotify()
    {
        if (fadeFinished)
        {
            fadeFinished = false;
            StartCoroutine(LoadMain());
        }
    }
}
