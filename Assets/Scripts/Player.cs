using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _playerImage;
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody2D;
    private bool _isMoving = false;
    private float _x, _y;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        ChangePlayerSkin();
    }

    private void ChangePlayerSkin()
    {
        Character character = GameDataManager.GetSelectedCharacter();

        if (character.Image != null)
        {            
            _playerImage.sprite = character.Image;
            _playerNameText.text = character.Name;
        }

    }

    private void Update()
    {
        _x = Input.GetAxis("Horizontal");
        _y = Input.GetAxis("Vertical");

        _isMoving = (_x != 0 || _y != 0);
    }

    private void FixedUpdate()
    {
        _rigidbody2D.position += new Vector2(_x, _y) * _speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.collider.tag;

        if (tag.Equals("Coin"))
        {
            GameDataManager.AddCoins(32);


        #if UNITY_EDITOR
            if (Input.GetKey(KeyCode.C))
            {
                GameDataManager.AddCoins(200);
            }
#endif

            GameSharedUI.Instance.UpdateCoinsUI();

            Destroy(other.gameObject);
        }
    }
}
