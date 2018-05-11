using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
//Done
public interface Publish
{
    void Notify(ActorState state, int pos, GameObject obj);
    void Add(Observer obs);
    void Del(Observer obs);
}
public interface Observer
{
    void Notified(ActorState state, int pos, GameObject obj);
}
public enum ActorState
{
    ALIVE,
    DEATH
}
public class Publisher:Publish
{
    private delegate void ActionUpdate(ActorState state, int pos, GameObject obj);
    private ActionUpdate updatelist;

    private static Publish instance;
    public static Publish GetInstance()
    {
        if (instance == null)
            instance = new Publisher();
        return instance;
    }
    public void Notify(ActorState state, int pos, GameObject obj)
    {
        if (updatelist != null)
            updatelist(state, pos, obj);
    }
    public void Add(Observer observer)
    {
        updatelist += observer.Notified;
    }
    public void Del(Observer observer)
    {
        updatelist -= observer.Notified;
    }
}
