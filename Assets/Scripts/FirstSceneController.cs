using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
//Almost
public class FirstSceneController : MonoBehaviour, Observer
{
    public bool gameStatus = false;
    private GuardianFactory factory;
    private TimeRecorder timer;
    void Start()
    {
        factory = Singleton<GuardianFactory>.Instance;
        timer = new TimeRecorder();
        Publish publisher = Publisher.GetInstance();
        publisher.Add(this);
        LoadResource();
    }

    private void LoadResource()
    {
        Instantiate(
            Resources.Load<GameObject>("Prefabs/Player"),
            new Vector3(1,1,-1),
            Quaternion.Euler(new Vector3(0,180,0)));
        factory = Singleton<GuardianFactory>.Instance;
        GameObject guardian = factory.GetGuardian();
        guardian.name = "Guardian";
    }
    public void Notified(ActorState state, int pos, GameObject obj)
    {
        if (state == ActorState.ALIVE)
            timer.PassTime();
        else
            gameStatus = false;
    }
}

