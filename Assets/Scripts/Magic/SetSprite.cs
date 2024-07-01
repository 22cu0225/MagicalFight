using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSprite : MonoBehaviour
{
    GameObject Player_obj;
    Player player;
    Magic magic;

    [SerializeField] Sprite[] magicImages;          // ñÇèpÇÃäOå©

    void Start()
    {
        magic = GetComponent<Magic>();
        Player_obj = magic.Player;
        player = Player_obj.GetComponent<Player>();
        GetComponent<SpriteRenderer>().sprite = magicImages[Player_obj.GetComponent<PlayerDB>().MyNumber];
    }
}
