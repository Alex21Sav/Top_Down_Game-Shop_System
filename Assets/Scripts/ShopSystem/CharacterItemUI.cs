using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;

public class CharacterItemUI : MonoBehaviour
{
    [SerializeField] private Color _itemColorNotSelected;
    [SerializeField] private Color _itemColorSelected;

    [Space(20f)]
    [SerializeField] private Image _imageCharacter;
    [SerializeField] private TMP_Text _nameCharacter;
    [SerializeField] private Image _imageSpeedCharacter;
    [SerializeField] private Image _imagePowerCharacter;
    [SerializeField] private TMP_Text _priceTextCharacter;
    [SerializeField] private Button _buttonPurchaseCharacter;

    [Space(20f)]
    [SerializeField] private Button _itemButton;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Outline _itemOutline;
 
    public void SetItemPosition( Vector2 position)
    {
        GetComponent<RectTransform>().anchoredPosition += position;
    }

    public void SetCharacterImage(Sprite sprite)
    {
        _imageCharacter.sprite = sprite;
    }

    public void SetCharacterName(string name)
    {
        _nameCharacter.text = name;
    }

    public void SetCharacterSpeed(float speed)
    {
        _imageSpeedCharacter.fillAmount = speed / 100 ;
    }

    public void SetCharacterPower(float power)
    {
        _imagePowerCharacter.fillAmount = power / 100;
    }

    public void SetCharacterPrice(int price)
    {
        _priceTextCharacter.text = price.ToString();
    }

    public void SetCharacterPurchase()
    {
        _buttonPurchaseCharacter.gameObject.SetActive(false);
        _itemButton.interactable = true;

        _itemImage.color = _itemColorNotSelected;
    }

    public void OnItemPurchase(int itemIndex, UnityAction<int> action)
    {
        _buttonPurchaseCharacter.onClick.RemoveAllListeners();
        _buttonPurchaseCharacter.onClick.AddListener(() => action.Invoke(itemIndex));
    }

    public void OnItemSelect(int itemIndex, UnityAction<int> action)
    {
        _itemButton.interactable = true;
        _itemButton.onClick.RemoveAllListeners();
        _itemButton.onClick.AddListener(() => action.Invoke(itemIndex));
    }

    public void SelectItem()
    {
        _itemOutline.enabled = true;
        _itemImage.color = _itemColorSelected;
        _itemButton.interactable = false;
    }
    public void DeSelectItem()
    {
        _itemOutline.enabled = false;
        _itemImage.color = _itemColorNotSelected;
        _itemButton.interactable = true;
    }

    public void AnimateShakeItem()
    {
        // «авершаем анимацию (если она запущена)
        transform.DOComplete();

        transform.DOShakePosition(1f, new Vector3(8f, 0f, 0f), 10, 0).SetEase(Ease.Linear);
    }

}
