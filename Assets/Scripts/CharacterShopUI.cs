using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShopUI : MonoBehaviour
{
    [Header("Layot elements")]
    [SerializeField] private float _itemSpacing = 0.5f;
    
    [Header("Ui elements")]    
    [SerializeField] private Transform _shopMenu;
    [SerializeField] private Transform _shopItemsContainer;
    [SerializeField] private GameObject _itemPrefab;
    [Space(20)]
    [SerializeField] private CharacterShopDatabase _characterDB;

    [Header("Shop event")]
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _closeButton;

    private float _itemHeight;

    private void Start()
    {
        AddShopEvents();

        // Заполняем список пользовательского интерфейса магазина предметами
        GenerateShopItemsUI();
    }

    private void GenerateShopItemsUI()
    {
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
        Debug.Log("select" + index);
    }
    private void OnItemPurchased(int index)
    {
        Debug.Log("purchase" + index);
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
