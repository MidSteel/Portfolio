using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kill Quest", menuName = "Quests/Quest Types/Kill Quests", order = 0)]
public class KillQuest : QuestType
{
    [SerializeField] private int _targetId = -1;
    [SerializeField] private int _amountToKill = 0;

    private int _amountOfTargetsKilled = 0;
    private string _enemyType = "";

    public override void SetTarget(int targetId, int amount)
    {
        if (targetId < 0) { return; }
        _targetId = targetId;
        _amountToKill = amount;
        QuestManager qManager = QuestManager.instance;
        _enemyType = qManager.EnemyTypes.GetEnemy(targetId).entityType;
        qManager.onKill += OnKill;
    }

    public void SetEnemyTypes(string type)
    {
        _enemyType = type;
    }

    public void OnKill(int id, int questId)
    {
        if (questId != _questId) { return; }
        if (_isQuestComplete) { return; }
        if (id != _targetId) { return; }
        //if (id != _targetId || !_isQuestComplete) { return; }
        _amountOfTargetsKilled++;
        if (_amountOfTargetsKilled >= _amountToKill) { onQuestComplete?.Invoke(_questId); QuestManager.instance.EnableQuestCompleteText(); _isQuestComplete = true; }
    }

    public override string GetDescription()
    {
        if (!_isQuestComplete)
        {
            return "Kill " + _amountToKill + " " + _enemyType;
        }
        else
        {
            return "Quest Complete";
        }
    }
}
