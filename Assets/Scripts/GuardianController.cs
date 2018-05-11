using UnityEngine;
using System.Collections;
using BaseAct;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class GuardianController : BaseActionManager,IBaseActionCallback,Observer
{
    private Animator animator;
    private BaseAction baseAction;
    private bool actionState;
    private const float walkSpeed = 1f;
    private const float runSpeed = 3f;
    // Use this for initialization
    new void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        Publish publisher = Publisher.GetInstance();
        publisher.Add(this);
        actionState = false;
        idle();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    public void EventAction(
        BaseAction source,
        BaseActionEventType events = BaseActionEventType.COMPLETED,
        int intParam = 0,
        string strParam = null,
        Object objParam = null)
    {
        if (actionState)
            walk();
        else
            idle();
        actionState = !actionState;
    }
    public void idle()
    {
        baseAction = IdleAction.GetIdleAction(Random.Range(1, 1.5f), animator);
        this.runAction(this.gameObject, baseAction, this);
    }
    public void walk()
    {
        Vector3 target = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) + transform.position;
        baseAction = WalkAction.GetWalkAction(target, walkSpeed, animator);
        this.runAction(this.gameObject, baseAction, this);
    }
    public void ChasePlayer(GameObject obj)
    {
        baseAction.destory = true;
        baseAction = RunAction.GetRunAction(obj.transform, runSpeed, animator);
        this.runAction(this.gameObject, baseAction, this);
    }
    public void losePlayer()
    {
        baseAction.destory = true;
        idle();
    }
    public void stop()
    {
        baseAction.destory = true;
        baseAction = IdleAction.GetIdleAction(-1f, animator);
        this.runAction(this.gameObject, baseAction, this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Transform transform = collision.gameObject.transform.parent;
        if (transform != null && transform.CompareTag("Wall"))
            idle();
    }
    public void Notified(ActorState state,int pos,GameObject obj)
    {
        if (true)
            ChasePlayer(obj);
        else
            stop();
    }
}
