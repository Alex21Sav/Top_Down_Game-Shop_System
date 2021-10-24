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

        // Заполняем список пользовательского интерфейса магазина предметами
        GenerateShopItemsUI();

        // Устанавливаем выбранного персонажа в playerDataManager.
        SetSelectedCharacter();

        // Выбираем элемент пользовательского интерфейса
        ItemSelectedUI(GameDataManager.GetSelectedCharacterIndex());

        // обновляем скин плеера (Главное меню)
        ChangePlayerSkin();
    }

    private void SetSelectedCharacter()
    {
        int index = GameDataManager.GetSelectedCharacterIndex();

        GameDataManager.SetSelectedCharacter(_characterDB.GetCharacter(index), index);
    }

    private void GenerateShopItemsUI()
    {
        // Цикл выбрасывает, сохраняет купленные предметы и делает их купленными в массиве базы данных
        for (int i = 0; i < GameDataManager.GetAllCharacterPurchase().Count; i++)
        {
            int PurchaseCharacterIndex = GameDataManager.GetCharacterPurchase(i);
            _characterDB.PurchaseCharacter(PurchaseCharacterIndex);
        }

        // Удаляем itemTemplate после вычисления высоты элемента:
        _itemHeight = _shopItemsContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        // DetachChildren () обязательно удалит его из иерархии, иначе, если вы
        // напишите ShopItemsContainer.ChildCount, вы получите "1"
        Destroy(_shopItemsContainer.GetChild(0).gameObject);
        _shopItemsContainer.DetachChildren();


        // Создание элементов
        for (int i = 0; i < _characterDB.CharactersCount; i++)
        {
            Character character = _characterDB.GetCharacter(i);
            CharacterItemUI characterItemUI = Instantiate(_itemPrefab, _shopItemsContainer).GetComponent<CharacterItemUI>();

            // Перемещаем элемент на место
            characterItemUI.SetItemPosition(Vector2.down * i * (_itemHeight + _itemSpacing));

            // Устанавливаем имя элемента в иерархии (не обязательно)
            characterItemUI.gameObject.name = "Item" + i + "-" + character.Name;

            // Добавляем информацию в UI (один элемент)
            characterItemUI.SetCharacterName(character.Name);
            characterItemUI.SetCharacterImage (character.Image);
            characterItemUI.SetCharacterSpeed(character.Speed);
            characterItemUI.SetCharacterPower(character.Power);
            characterItemUI.SetCharacterPrice(character.Price);

            if (character.IsPurchase)
            {
                // Персонаж приобретен
                characterItemUI.SetCharacterPurchase();
                characterItemUI.OnItemSelect(i, OnItemSelected);
            }
            else
            {
                // Персонаж еще не куплен
                characterItemUI.SetCharacterPrice(character.Price);
                characterItemUI.OnItemPurchase(i, OnItemPurchased);
            }
            // Изменить размер контейнера элементов
            _shopItemsContainer.GetComponent<RectTransform>().sizeDelta =
                Vector2.up * ((_itemHeight + _itemSpacing) * _characterDB.CharactersCount + _itemSpacing);

        }
    
    }

    private void OnItemSelected(int index)
    {
        // Выбираем элемент в пользовательском интерфейсе
        ItemSelectedUI(index);        

        // Сохраняем данные
        GameDataManager.SetSelectedCharacter(_characterDB.GetCharacter(index), index);

        // Изменить скин игрока
        ChangePlayerSkin();
    }
    private void ChangePlayerSkin()
    {
        Character character = GameDataManager.GetSelectedCharacter();

        if(character.Image != null)
        {
            // Изменяем информацию главного меню (изображение и текст)
            _mainMenuCharacterIcon.sprite = character.Image;
            _mainMenuCharacterNameText.text = character.Name;

            // Устанавливаем выбранное изображение персонажа в верхней части меню магазина
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
            // Савершаем операцию покупки
            GameDataManager.SpendCoins(character.Price);

            // Обновляем текст пользовательского интерфейса Coin
            GameSharedUI.Instance.UpdateCoinsUI();

            // Обновить данные БД
            _characterDB.PurchaseCharacter(index);

            itemUI.SetCharacterPurchase();
            itemUI.OnItemSelect(index, OnItemSelected);

            // Добавить купленный товар в данные магазина
            GameDataManager.AddCharacterPurchase(index);


        }
        else
        {
            // Недостаточно монет
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
