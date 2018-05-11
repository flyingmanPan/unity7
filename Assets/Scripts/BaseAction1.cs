using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseAct
{
    public enum BaseActionEventType:int
    {
        STARTED,
        COMPLETED
    }
    public interface IBaseActionCallback
    {
        void EventAction(
            BaseAction source,
            BaseActionEventType events = BaseActionEventType.COMPLETED,
            int intpar = 0,
            string strpar = null,
            Object objpar = null);
    }
    public class BaseAction : ScriptableObject
    {
        public bool enable = true;
        public bool destory = false;
        public GameObject gameObject { get; set; }
        public Transform transform { get; set; }
        public IBaseActionCallback callback { get; set; }

        public virtual void Start()
        {

        }
        public virtual void FixedUpdate()
        {

        }
        // Update is called once per frame
        public virtual void Update()
        {

        }
    }
    public class IdleAction:BaseAction
    {
        private float time;
        private Animator animator;
        public static IdleAction GetIdleAction(float time,Animator animator)
        {
            IdleAction currenAction = ScriptableObject.CreateInstance<IdleAction>();
            currenAction.time = time;
            currenAction.animator = animator;
            return currenAction;
        }
        public override void Start()
        {
            if (time == -1)
                return;
            time -= Time.deltaTime;
            if(time<0)
            {
                this.destory = true;
                this.callback.EventAction(this);
            }
        }
    }
    public class WalkAction:BaseAction
    {
        private float speed;
        private Vector3 target;
        private Animator animator;
        public static WalkAction GetWalkAction(Vector3 target, float speed,Animator animator)
        {
            WalkAction currentAction = ScriptableObject.CreateInstance<WalkAction>();
            currentAction.speed = speed;
            currentAction.target = target;
            currentAction.animator = animator;
            return currentAction;
        }
        public override void Start()
        {
            //animator.SetFloat("speed", 0.5f);
        }
        public override void Update()
        {
            Quaternion rotate = Quaternion.LookRotation(target - transform.position);
            if (transform.rotation != rotate)
                transform.rotation = Quaternion.Slerp(transform.rotation,rotate, Time.deltaTime * speed * 5);
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            if(this.transform.position==target)
            {
                this.destory = true;
                this.callback.EventAction(this);
            }
        }
    }
    public class RunAction : BaseAction
    {
        private float speed;
        private Transform target;
        private Animator animator;
        public static RunAction GetRunAction(Transform target, float speed, Animator animator)
        {
            RunAction currentAction = ScriptableObject.CreateInstance<RunAction>();
            currentAction.speed = speed;
            currentAction.target = target;
            currentAction.animator = animator;
            return currentAction;
        }
        public override void Start()
        {
            animator.SetFloat("speed", 1);
        }
        public override void Update()
        {
            Quaternion rotate = Quaternion.LookRotation(target.position - transform.position);
            if (transform.rotation != rotate)
                transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * speed * 5);
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
            if (Vector3.Distance(this.transform.position,target.position)<0.5f)
            {
                this.destory = true;
                this.callback.EventAction(this);
            }
        }
    }
    public class BaseActionManager : MonoBehaviour
    {
        private Dictionary<int, BaseAction> dictionary = new Dictionary<int, BaseAction>();
        private List<BaseAction> waitingAddAction = new List<BaseAction>();
        private List<int> waitingDel = new List<int>();
        protected void Start()
        {
            
        }
        protected void Update()
        {
            foreach (var item in waitingAddAction)
            {
                dictionary[item.GetInstanceID()] = item;
            }
            waitingAddAction.Clear();
            foreach (var item in dictionary)
            {
                BaseAction action = item.Value;
                if (action.destory)
                    waitingDel.Add(action.GetInstanceID());
                else if (action.enable)
                    action.Update();
            }
            foreach (var item in waitingDel)
            {
                BaseAction action = dictionary[item];
                dictionary.Remove(item);
                Destroy(action);
            }
            waitingDel.Clear();
        }
        public void runAction(GameObject obj, BaseAction action, IBaseActionCallback callback)
        {
            action.gameObject = obj;
            action.transform = obj.transform;
            action.callback = callback;
            waitingAddAction.Add(action);
            action.Start();
        }
    }
    
}

