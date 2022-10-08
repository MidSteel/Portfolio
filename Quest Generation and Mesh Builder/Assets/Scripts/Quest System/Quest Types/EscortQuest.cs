using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Escort Quest", menuName = "Quests/Quest Types/Escort Quest", order = 2)]
public class EscortQuest : QuestType
{
    [SerializeField] private Transform _destinationLocation;
    [SerializeField] private int _npcToEscortId;

    public override void SetTarget(Transform destinationLocationTransform, int objectToDeliver, int amountToDeliver)
    {
        _destinationLocation = destinationLocationTransform;
        _npcToEscortId = objectToDeliver;
    }

    public void OnEscort(int questId)
    {
        if (questId != _questId || !_isQuestComplete) { return; }
        
        onQuestComplete(_questId);
    }
}
