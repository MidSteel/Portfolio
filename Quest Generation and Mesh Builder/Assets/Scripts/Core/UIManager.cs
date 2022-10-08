using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField] private GameObject _optionsMenu = null;
    [SerializeField] private GameObject _questPanel = null;
    [SerializeField] private Text _questDescription = null;

    private QuestManager _questManager = null;

    public Text QuestDescription { get { return _questDescription; } }

    private void Awake()
    {
        instance = this;
    }

    public void OpenCloseMenu()
    {
        bool open = !_optionsMenu.activeInHierarchy;
        _optionsMenu.gameObject.SetActive(open);
    }
}
