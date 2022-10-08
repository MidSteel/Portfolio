using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    [SerializeField] private EnemyTypes _enemyTypeList;
    [SerializeField] private ItemTypes _itemTypeList;
    [SerializeField] private GameObject _groundObj;

    private int _questType = -1;
    private QuestManager _questManager = null;
    private int _randomAmount = 0;
    private int _randomId = -1;

    private void Start()
    {
        _questManager = QuestManager.instance;

        //for (int i = 0; i < 5; i++)
        //{
        //    Test();
        //}
    }

    [ContextMenu("Add A Random Quest")]
    public void Test()
    {
        GenerateQuest();
    }

    public void GenerateQuest()
    {
        _questType = Random.Range(1, 4);

        switch (_questType)
        {
            case 1:
                GenerateKillQuest();
                break;
            case 2:
                GenerateGatherQuest();
                break;
            case 3:
                GenerateDeliveryQuest();
                break;
        }
    }

    public void GenerateKillQuest()
    {
        KillQuest quest = ScriptableObject.CreateInstance<KillQuest>();
        _randomId = Random.Range(1, _enemyTypeList.EnemyTypeList.Count);
        _randomAmount = Random.Range(1, 5);
        //_randomId = 2;
        //_randomAmount = 2;
        quest.SetTarget(_randomId, _randomAmount);
        quest.SetQuestType(1);
        AddQuestToQuestManager(quest);
    }

    public void GenerateGatherQuest()
    {
        GatherQuest quest = ScriptableObject.CreateInstance<GatherQuest>();
        _randomAmount = Random.Range(1, 5);
        _randomId = Random.Range(1, _itemTypeList.ItemTypeList.Count + 1);
        //_randomAmount = Random.Range(1, 20);
        //_randomId = 3;
        //_randomAmount = 2;
        quest.SetTarget(_randomId, _randomAmount);
        quest.SetQuestType(2);
        AddQuestToQuestManager(quest);
    }

    public void GenerateEscortQuest()
    {
        EscortQuest quest = ScriptableObject.CreateInstance<EscortQuest>();
        _randomId = Random.Range(1, 1000);
        //_randomAmount = Random.Range(1, 20);
        _randomAmount = 1;

        GameObject destinationObject = InstantiateDestination();
        QuestDestinationReciever destinationReciever = destinationObject.AddComponent<QuestDestinationReciever>();
        destinationReciever.RecieveObjectId = _randomId;
        destinationReciever.AmountToRecieve = _randomAmount;
        destinationReciever._QuestType = quest;
        //destinationObject.AddComponent<>

    }

    public void GenerateDeliveryQuest()
    {
        DeliveryQuest quest = ScriptableObject.CreateInstance<DeliveryQuest>();
        _randomId = Random.Range(1, _itemTypeList.ItemTypeList.Count);
        _randomAmount = Random.Range(1, 5);
        //_randomId = 3;
        //_randomAmount = 1;
        GameObject destinationObject = InstantiateDestination();
        QuestDestinationReciever destinationReciever = destinationObject.AddComponent<QuestDestinationReciever>();
        destinationReciever.RecieveObjectId = _randomId;
        destinationReciever.AmountToRecieve = _randomAmount;
        destinationReciever._QuestType = quest;
        quest.SetTarget(destinationObject.transform, _randomId, _randomAmount);
        quest.SetQuestType(2);
        AddQuestToQuestManager(quest);
    }

    public GameObject InstantiateDestination()
    {
        Vector3 pickAPosFrom = new Vector3(_groundObj.transform.localScale.x * 1.5f, _groundObj.transform.localScale.y, _groundObj.transform.localScale.z * 1.5f);
        //GameObject destinationObject = Instantiate(new GameObject());
        GameObject destinationObject = new GameObject();
        SphereCollider col = destinationObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        destinationObject.transform.SetParent(_groundObj.transform);
        col.radius = 1.5f;
        Vector3 destinationPos = new Vector3(Random.Range(0f, pickAPosFrom.x), 0f, Random.Range(0, pickAPosFrom.z));
        destinationObject.transform.localPosition = destinationPos;
        return destinationObject;
    }

    public void AddQuestToQuestManager(QuestType type)
    {
        int questId = Random.Range(0, 100000);

        while (!_questManager.AddNewQuest(questId, type))
        {
            questId = Random.Range(0, 100000);
        }
    }
}
