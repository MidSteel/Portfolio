using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gather Quest", menuName = "Quests/Quest Types/Gather Quest", order = 1)]
public class GatherQuest : QuestType
{
    [SerializeField] int _targetId = -1;
    [SerializeField] int _amountTogather = 0;

    private int _gatheredAmount = 0;
    private string _targetedItemType = "";
    public bool canInteract = true;

    public override void SetTarget(int targetId, int amount)
    {
        if (targetId < 0) { return; }

        _targetId = targetId;
        _amountTogather = amount;
        QuestManager qManager = QuestManager.instance;
        _targetedItemType = qManager.ItemTypes.GetItem(targetId).entityType;
        qManager.onGather += OnGather;
    }

    public void OnGather(int id, int questId)
    {
        if (!canInteract) { return; }
        if (_isQuestComplete) { return; }
        if (id != _targetId || questId != _questId) { return; }

        Action action = () => canInteract = true;
        DelayAction(action , 0.2f);
        _gatheredAmount++;
        if ( _gatheredAmount>= _amountTogather) { onQuestComplete?.Invoke(_questId); QuestManager.instance.EnableQuestCompleteText(); _isQuestComplete = true; }
    }

    public override string GetDescription()
    {
        if (!_isQuestComplete)
        {
            return "Gather " + _amountTogather + " " + _targetedItemType;
        }
        else
        {
            return "Quest Complete";
        }
    }
}
