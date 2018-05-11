using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tem.Action;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class PatrolUI : SSActionManager, ISSActionCallback, Observer
{

    private Animator ani;
    private SSAction currentAction;
    private bool actionState;
    private const float walkSpeed = 1f;
    private const float runSpeed = 3f;
	new void Start ()
    {
        ani = this.gameObject.GetComponent<Animator>();
        Publish publisher = Publisher.GetInstance();
        publisher.Add(this);
        actionState = false;
        idle();
	}
	new void Update ()
    {
        base.Update();
	}

    public void SSEventAction(SSAction source, SSActionEventType events = SSActionEventType.COMPLETED, int intParam = 0, string strParam = null, Object objParam = null)
    {
        if (actionState)
            walkLeft();
        else
            idle();
        actionState = !actionState;
    }

    public void idle()
    {
        currentAction = IdleAction.GetIdleAction(Random.Range(1, 1.5f), ani);
        this.runAction(this.gameObject, currentAction, this);
    }

    public void walkLeft()
    {
        Vector3 target = new Vector3(Random.Range(-1, 1),0,Random.Range(-1,1)) +transform.position;
        currentAction = WalkAction.GetWalkAction(target, walkSpeed, ani);
        this.runAction(this.gameObject, currentAction, this);
    }
    public void getGoal(GameObject gameobject)
    {
        currentAction.destory = true;
        currentAction = RunAction.GetRunAction(gameobject.transform, runSpeed, ani);
        this.runAction(this.gameObject, currentAction, this);
    }

    public void loseGoal()
    {
        currentAction.destory = true;
        idle();
    }

    public void stop()
    {
        currentAction.destory = true;
        currentAction = IdleAction.GetIdleAction(-1f, ani);
        this.runAction(this.gameObject, currentAction, this);
    }
    public void Notified(ActorState state, int pos, GameObject actor)
    {
        if (state == ActorState.ALIVE)
        {
            getGoal(actor);
        }
        else stop();
    }
}
