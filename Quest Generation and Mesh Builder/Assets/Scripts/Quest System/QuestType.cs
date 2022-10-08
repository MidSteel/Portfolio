using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestType : ScriptableObject
{
    [SerializeField] private string _questName = "";

    protected int _questId = -1;
    protected int _questType = -1;
    protected bool _isQuestComplete = false;

    public delegate void OnQuestComplete(int questId);              
    public OnQuestComplete onQuestComplete;                                                                                       // Gets called when Quest is Complete;

    public int QuestId { get { return _questId; } }
    public int _QuestType { get { return _questType; } }
    public bool IsQuestComplete { get { return _isQuestComplete; } set { _isQuestComplete = value; } }

    public virtual void SetId (int id) { _questId = id; }                                                                         // Sets Quest Id;    
    public virtual void SetTarget (int targetId, int amount) { }                                                                  // Sets target for Gather and Kill Quest;
    public virtual void SetTarget (Transform deliveryLocationTransform, int objectToDeliver, int amountToDeliver) { }             // Sets target destination for Delivery and Escort Quest;
    public virtual string GetDescription() { return null; }                                                                       // Returns the quest description;
    public virtual void SetQuestType(int id) { _questType = id; }                                                                 // Sets Quest ID;

    public IEnumerator DelayAction (Action action, float time)
    {
        yield return new WaitForSeconds(time);

        action?.Invoke();
    }
}
