using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Delivery Quest", menuName = "Quests/Quest Types/Delivery Quest", order = 3)]
public class DeliveryQuest : QuestType
{
    [SerializeField] private int _objectToDeliverId = -1;
    [SerializeField] private Transform _destinationLocationTransform = null;
    [SerializeField] private int _amountToDeliver = 0;
    [SerializeField] private string _itemToDeliver = "";

    public override void SetTarget(Transform deliveryLocationTransform, int objectToDeliver, int amountToDeliver)
    {
        _objectToDeliverId = objectToDeliver;
        _destinationLocationTransform = deliveryLocationTransform;
        _amountToDeliver = amountToDeliver;
        _itemToDeliver = QuestManager.instance.ItemTypes.GetItem(_objectToDeliverId).entityType;
    }

    public void OnDelivery(int questId)
    {
        if (questId != _questId || !_isQuestComplete) { return; }

        onQuestComplete(_questId);
    }

    public override string GetDescription()
    {
        if (!_isQuestComplete)
        {
            return "Deliver " + _amountToDeliver + " " + _itemToDeliver + " to : " + _destinationLocationTransform.localPosition;
        }
        else
        {
            return "Quest Complete";
        }
    }
}
