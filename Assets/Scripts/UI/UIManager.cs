using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;

    [SerializeField] private Transform Background;
    [SerializeField] private Transform Main;
    [SerializeField] private Transform Content;
    [SerializeField] private Transform Popup;
    [SerializeField] private Transform Front;

    private Dictionary<UIType, UIBase> _createdUI = new Dictionary<UIType, UIBase>();
    private HashSet<UIType> _openedUI = new HashSet<UIType>();

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        this.InitStart();
    }

    private void CreateUI(UIRootType root, UIType type)
    {
        if (_createdUI.ContainsKey(type))
        {
            return;
        }

        string path = $"Prefabs/UI/{root}/{type}";
        GameObject ui = Resources.Load<GameObject>(path);

        if (ui != null)
        {
            GameObject uiObject = Instantiate(ui, GetUIRootTransform(root));

            UIBase uiBase = uiObject.GetComponent<UIBase>();

            _createdUI[type] = uiBase;
            _openedUI.Add(type);
        }
    }

    public UIBase OpenUI(UIRootType root, UIType type)
    {
        if (_createdUI.ContainsKey(type))
        {
            _createdUI[type].gameObject.SetActive(true);
            _openedUI.Add(type);
        }
        else
        {
            CreateUI(root, type);
        }

        return _createdUI[type];
    }

    public void CloseUI(UIType type)
    {
        if (_openedUI.Contains(type))
        {
            _createdUI[type].gameObject.SetActive(false);
            _openedUI.Remove(type);
        }
    }

    public Transform GetUIRootTransform(UIRootType root)
    {
        Transform tranform = Background;

        switch (root)
        {
            case UIRootType.Background:
                tranform = Background;
                break;

            case UIRootType.Main:
                tranform = Main;
                break;

            case UIRootType.Content:
                tranform = Content;
                break;

            case UIRootType.Popup:
                tranform = Popup;
                break;

            case UIRootType.Front:
                tranform = Front;
                break;
        }

        return tranform;
    }

    public UIBase OpenMainUI(UIType type, bool isActive = true)
    {
        UIBase ui = OpenUI(UIRootType.Main, type);

        if (!isActive)
        {
            CloseUI(type);
        }

        return ui;
    }

    public UIBase OpenContentUI(UIType type)
    {
        UIBase ui = OpenUI(UIRootType.Content, type);

        return ui;
    }

    public UIBase OpenPopupUI(UIType type, bool isActive = true)
    {
        UIBase ui = OpenUI(UIRootType.Popup, type);

        if (!isActive)
        {
            CloseUI(type);
        }
        else
        {
            SoundManager.Inst.SetSFXAndPlay("Audio/Popup").Forget();
        }

        return ui;
    }

    public bool IsOpenedUI(UIType type)
    {
        if (_openedUI.Contains(type))
        {
            return true;
        }
        
        return false;
    }
}
