using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;

    [SerializeField] private EnemyTypes _enemyTypes = null;
    [SerializeField] private ItemTypes _itemTypes = null;
    [SerializeField] private GameObject _questCompleteUIObject = null;
    [SerializeField] private ButtonQuestPair[] _buttonQuestPairs;

    private Dictionary<int,QuestType> _currentQuests = new Dictionary<int, QuestType>();
    private UIManager _uiManager = null;
    private Player _player = null;

    public delegate void OnKill(int id, int questId);
    public delegate void OnGather(int id, int questId);
    public delegate void OnEscort(int questId);
    public delegate void OnDelivery(int questId);

    public OnKill onKill;                                                       // Gets Called everytime player kills Anything;
    public OnGather onGather;                                                   // Gets Called everytime anything is Added to playerInventory;
    public OnEscort onEscort;                                                   // Gets called From the destination location;
    public OnDelivery onDelivery;                                               // Gets Called From Destinatrion location;

    public EnemyTypes EnemyTypes { get { return _enemyTypes; } }
    public ItemTypes ItemTypes { get { return _itemTypes; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _uiManager = UIManager.instance;

        foreach (ButtonQuestPair bqPair in _buttonQuestPairs)
        {
            bqPair.button.onClick.AddListener(() => DisplayQuestDescription(bqPair.button));
        }
    }

    public void EnableQuestCompleteText()
    {
        _questCompleteUIObject.SetActive(true);
        Action action = new Action(() => _questCompleteUIObject.SetActive(false));
        StartCoroutine(QuestDelayAction(action, 0.5f));
    }

    public bool AddNewQuest(int questId,QuestType quest)
    {
        quest.SetId(questId);
        if (_currentQuests.ContainsKey(questId)) { return false; }
        _currentQuests.Add(questId, quest);

        foreach (ButtonQuestPair bqPair in _buttonQuestPairs)
        {
            if (bqPair.questType == null)
            {
                bqPair.questType = quest;
                SubscribeDelegetaesForCompletionChecking(quest);
                return true;
            }
        }

        return true;
    }

    public void SubscribeDelegetaesForCompletionChecking(QuestType type)
    {
        if (type._QuestType == 1)
        {
            _player._EnemyKiller.onKill += (x) => OnCompletionCheck(type, x);
        }
        else if (type._QuestType == 2)
        {
            _player.PlayerInventory.onItemAdded += (x) => OnCompletionCheck(type, x);
        }
    }

    public void OnCompletionCheck(QuestType questType, int id)
    {
        if (questType._QuestType == 1)             //Kill quest
        {
            onKill?.Invoke(id, questType.QuestId);
        }
        else if (questType._QuestType == 2)        //Gather quest
        {
            onGather?.Invoke(id, questType.QuestId);
        }
    }

    public void DisplayQuestDescription(Button button)
    {
        foreach (ButtonQuestPair bqPair in _buttonQuestPairs)
        {
            if (bqPair.button == button)
            {
                _uiManager.QuestDescription.text = bqPair.questType.GetDescription();
                return;
            }
        }
    }

    public void SetQuestDescriptionUI(int id)
    {
        _uiManager.QuestDescription.text = _currentQuests[id].GetDescription();
    }

    public IEnumerator QuestDelayAction(Action action, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        action?.Invoke();
    }
}

[System.Serializable]
public class ButtonQuestPair
{
    public Button button;
    public QuestType questType;
}