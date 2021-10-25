using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CharacterShopUI : MonoBehaviour
{
    [Header("Layot elements")]
    [SerializeField] private float _itemSpacing = 0.5f;

    [Space(20)]
    [Header("Ui elements")]
    [SerializeField] private Image _selectedCharacterIcon;
    [SerializeField] private Transform _shopMenu;
    [SerializeField] private Transform _shopItemsContainer;
    [SerializeField] private GameObject _itemPrefab;
    [Space(20)]
    [SerializeField] private CharacterShopDatabase _characterDB;

    [Space(20)]
    [Header("Shop event")]
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _scrollUpButton;
 

    [Space(20)]
    [Header("Main menu")]
    [SerializeField] private Image _mainMenuCharacterIcon;
    [SerializeField] private TMP_Text _mainMenuCharacterNameText;

    [Space(20)]
    [Header("Scroll View")]
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private GameObject _topScrollFade;
    [SerializeField] private GameObject _bottonScrollFade;

    [Space(20)]
    [Header("Purchase Fx && Error messages")]
    [SerializeField] private ParticleSystem _purchaseFx;
    [SerializeField] private Transform _transformPurchaseFx;
    [SerializeField] private TMP_Text _messagesNoCoins;

    private float _itemHeight;
    private int _newSelectedItemIndex = 0;
    private int _previousSelectedItemIndex = 0;

    private void Start()
    {
        // ����������� ������� � ��������� �������
        _purchaseFx.transform.position = _transformPurchaseFx.position;

        AddShopEvents();

        // ��������� ������ ����������������� ���������� �������� ����������
        GenerateShopItemsUI();

        // ������������� ���������� ��������� � playerDataManager.
        SetSelectedCharacter();

        // �������� ������� ����������������� ����������
        ItemSelectedUI(GameDataManager.GetSelectedCharacterIndex());

        // ��������� ���� ������ (������� ����)
        ChangePlayerSkin();

        // ������������� � ���������� ��������� � ��������
        AutoScrollShopList(GameDataManager.GetSelectedCharacterIndex());
    }

    private void AutoScrollShopList(int itemIndex)
    {
        // scrollRect.verticalNormalizedPosition = 0f; // �������� ��������� ����
        // scrollRect.verticalNormalizedPosition = 1f; // �������� ��������� �����

        _scrollRect.verticalNormalizedPosition = Mathf.Clamp01(1f - (itemIndex / (float)(_characterDB.CharactersCount -1)));
    }

    private void SetSelectedCharacter()
    {
        // �������� ����������� ������
        int index = GameDataManager.GetSelectedCharacterIndex();

        // ������������� ���������� ������
        GameDataManager.SetSelectedCharacter(_characterDB.GetCharacter(index), index);
    }
    private void GenerateShopItemsUI()
    {
        // ���� �����������, ��������� ��������� �������� � ������ �� ���������� � ������� ���� ������
        for (int i = 0; i < GameDataManager.GetAllCharacterPurchase().Count; i++)
        {
            int PurchaseCharacterIndex = GameDataManager.GetCharacterPurchase(i);
            _characterDB.PurchaseCharacter(PurchaseCharacterIndex);
        }

        // ������� itemTemplate ����� ���������� ������ ��������:
        _itemHeight = _shopItemsContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        // DetachChildren () ����������� ������ ��� �� ��������, �����, ���� ��
        // �������� ShopItemsContainer.ChildCount, �� �������� "1"
        Destroy(_shopItemsContainer.GetChild(0).gameObject);
        _shopItemsContainer.DetachChildren();


        // �������� ���������
        for (int i = 0; i < _characterDB.CharactersCount; i++)
        {
            Character character = _characterDB.GetCharacter(i);
            CharacterItemUI characterItemUI = Instantiate(_itemPrefab, _shopItemsContainer).GetComponent<CharacterItemUI>();

            // ���������� ������� �� �����
            characterItemUI.SetItemPosition(Vector2.down * i * (_itemHeight + _itemSpacing));

            // ������������� ��� �������� � �������� (�� �����������)
            characterItemUI.gameObject.name = "Item" + i + "-" + character.Name;

            // ��������� ���������� � UI (���� �������)
            characterItemUI.SetCharacterName(character.Name);
            characterItemUI.SetCharacterImage (character.Image);
            characterItemUI.SetCharacterSpeed(character.Speed);
            characterItemUI.SetCharacterPower(character.Power);
            characterItemUI.SetCharacterPrice(character.Price);

            if (character.IsPurchase)
            {
                // �������� ����������
                characterItemUI.SetCharacterPurchase();
                characterItemUI.OnItemSelect(i, OnItemSelected);
            }
            else
            {
                // �������� ��� �� ������
                characterItemUI.SetCharacterPrice(character.Price);
                characterItemUI.OnItemPurchase(i, OnItemPurchased);
            }
            // �������� ������ ���������� ���������
            _shopItemsContainer.GetComponent<RectTransform>().sizeDelta =
                Vector2.up * ((_itemHeight + _itemSpacing) * _characterDB.CharactersCount + _itemSpacing);
        }    
    }

    private void OnItemSelected(int index)
    {
        // �������� ������� � ���������������� ����������
        ItemSelectedUI(index);        

        // ��������� ������
        GameDataManager.SetSelectedCharacter(_characterDB.GetCharacter(index), index);

        // �������� ���� ������
        ChangePlayerSkin();
    }
    private void ChangePlayerSkin()
    {
        Character character = GameDataManager.GetSelectedCharacter();

        if(character.Image != null)
        {
            // �������� ���������� �������� ���� (����������� � �����)
            _mainMenuCharacterIcon.sprite = character.Image;
            _mainMenuCharacterNameText.text = character.Name;

            // ������������� ��������� ����������� ��������� � ������� ����� ���� ��������
            _selectedCharacterIcon.sprite = GameDataManager.GetSelectedCharacter().Image;


        }

    }

    private void ItemSelectedUI(int itemIndex)
    {
        _previousSelectedItemIndex = _newSelectedItemIndex;
        _newSelectedItemIndex = itemIndex;

        CharacterItemUI previousUiItem = GetItemUI(_previousSelectedItemIndex);
        CharacterItemUI newUiItem = GetItemUI(_newSelectedItemIndex);

        previousUiItem.DeSelectItem();
        newUiItem.SelectItem();
    }
    private CharacterItemUI GetItemUI(int index)
    {
        return _shopItemsContainer.GetChild(index).GetComponent<CharacterItemUI>();
    }


    private void OnItemPurchased(int index)
    {
        Character character = _characterDB.GetCharacter(index);
        CharacterItemUI itemUI = GetItemUI(index);

        if (GameDataManager.CanSpendCoins(character.Price))
        {
            // ��������� �������� �������
            GameDataManager.SpendCoins(character.Price);

            // ������ ������� �������
            _purchaseFx.Play();

            // ��������� ����� ����������������� ���������� Coin
            GameSharedUI.Instance.UpdateCoinsUI();

            // �������� ������ ��
            _characterDB.PurchaseCharacter(index);

            itemUI.SetCharacterPurchase();
            itemUI.OnItemSelect(index, OnItemSelected);

            // �������� ��������� ����� � ������ ��������
            GameDataManager.AddCharacterPurchase(index);
        }
        else
        {
            // ������������ �����
            AnimateNoMoreCoinsText();

            itemUI.AnimateShakeItem();
        }
    }

    private void AnimateNoMoreCoinsText()
    {
        // ��������� �������� (���� ��� ��������)
        _messagesNoCoins.transform.DOComplete();
        _messagesNoCoins.DOComplete();


        _messagesNoCoins.transform.DOShakePosition(3f, new Vector3(5f, 0f, 0f), 10, 0);
        _messagesNoCoins.DOFade(1f, 3f).From(0f).OnComplete(() =>
        {
            _messagesNoCoins.DOFade(0f, 1f);
        });        
    }

    private void AddShopEvents()
    {
        _shopButton.onClick.RemoveAllListeners();
        _shopButton.onClick.AddListener(OpenShop);

        _closeButton.onClick.RemoveAllListeners();
        _closeButton.onClick.AddListener(CloseShop);

        _scrollRect.onValueChanged.RemoveAllListeners();
        _scrollRect.onValueChanged.AddListener(OnScrollShopList);

        _scrollUpButton.onClick.RemoveAllListeners();
        _scrollUpButton.onClick.AddListener(OnScrollUpClicked);
    }

    private void OnScrollUpClicked()
    {
        _scrollRect.DOVerticalNormalizedPos(1f, 0.5f).SetEase(Ease.OutBack);
    }
    private void OnScrollShopList(Vector2 value)
    {
        float scrollY = value.y;

        if(scrollY < 1f)
        {
            _topScrollFade.SetActive(true);
        }
        else
        {
            _topScrollFade.SetActive(false);
        }

        if (scrollY > 0f)
        {
            _bottonScrollFade.SetActive(true);
        }
        else
        {
            _bottonScrollFade.SetActive(false);
        }

        if(scrollY < 0.7f)
        {
            _scrollUpButton.gameObject.SetActive(true);
        }
        else
        {
            _scrollUpButton.gameObject.SetActive(false);
        }
    }

    private void OpenShop()
    {
        _shopUI.SetActive(true);
    }

    private void CloseShop()
    {
        _shopUI.SetActive(false);
    }

}
