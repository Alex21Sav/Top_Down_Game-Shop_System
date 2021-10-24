using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterShopUI : MonoBehaviour
{
    [Header("Layot elements")]
    [SerializeField] private float _itemSpacing = 0.5f;

    [Header("Ui elements")]
    [SerializeField] private Image _selectedCharacterIcon;
    [SerializeField] private Transform _shopMenu;
    [SerializeField] private Transform _shopItemsContainer;
    [SerializeField] private GameObject _itemPrefab;
    [Space(20)]
    [SerializeField] private CharacterShopDatabase _characterDB;

    [Header("Shop event")]
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _closeButton;

    [Space(20)]
    [Header("Main menu")]
    [SerializeField] private Image _mainMenuCharacterIcon;
    [SerializeField] private TMP_Text _mainMenuCharacterNameText;


    private float _itemHeight;
    private int _newSelectedItemIndex = 0;
    private int _previousSelectedItemIndex = 0;

    private void Start()
    {
        AddShopEvents();

        // ��������� ������ ����������������� ���������� �������� ����������
        GenerateShopItemsUI();

        // ������������� ���������� ��������� � playerDataManager.
        SetSelectedCharacter();

        // �������� ������� ����������������� ����������
        ItemSelectedUI(GameDataManager.GetSelectedCharacterIndex());

        // ��������� ���� ������ (������� ����)
        ChangePlayerSkin();
    }

    private void SetSelectedCharacter()
    {
        int index = GameDataManager.GetSelectedCharacterIndex();

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
            Debug.Log("no coins!");

        }
    }

    private void AddShopEvents()
    {
        _shopButton.onClick.RemoveAllListeners();
        _shopButton.onClick.AddListener(OpenShop);

        _closeButton.onClick.RemoveAllListeners();
        _closeButton.onClick.AddListener(CloseShop);
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
