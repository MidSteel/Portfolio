using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDestinationReciever : MonoBehaviour
{
    private int _reciveObjectId = -1;

    public int AmountToRecieve { get; set; }
    public int RecieveObjectId { get { return _reciveObjectId; } set { _reciveObjectId = value; } }
    public QuestType _QuestType { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        Player player;

        if (other.TryGetComponent(out player))
        {
            if (player.PlayerInventory.ContainsItem(_reciveObjectId) >= AmountToRecieve)
            {
                _QuestType.onQuestComplete?.Invoke(_QuestType.QuestId); QuestManager.instance.EnableQuestCompleteText(); _QuestType.IsQuestComplete = true;
            }
        }
    }
}
