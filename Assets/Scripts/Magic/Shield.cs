using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Player player;

    [Header("Shield")]
    [SerializeField] GameObject magicPrehub;
    [SerializeField] int IntGamePudNumber4;
    [SerializeField] int IntGamePudNumber5;
    GameObject magic;
    float coolTime;
    float coolTimeCounter;
    bool coolDown;                                  // クールタイムに入ったか
    bool holdButton;

    private void Start()
    {
        player = GetComponent<Player>();
        coolTime = magicPrehub.GetComponent<ShieldStatus>().GetCoolTime();
        coolTimeCounter = coolTime;
        coolDown = false;
        magic = Instantiate(magicPrehub, transform.position, transform.rotation);
        magic.transform.SetParent(transform);
        magic.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(magic != null)
        {
            if (coolDown)
            {
                coolTimeCounter -= Time.deltaTime;
                if (coolTimeCounter <= 0)
                {
                    coolDown = !coolDown;
                    coolTimeCounter = coolTime;
                }
            }

            if(HoldButton())
            {
                magic.SetActive(true);
            }
            else
            {
                magic.SetActive(false);
            }
        }
    }

    bool HoldButton()
    {
        if (player.Pressd(player.GetButton(IntGamePudNumber4))|| player.Pressd(player.GetButton(IntGamePudNumber5)))
        {
            if (IntGamePudNumber4 == 4 && !holdButton)
            {
                holdButton = true;
            }
            if (IntGamePudNumber5 == 5 && !holdButton)
            {
                holdButton = true;
            }
        }
        else if (player.Release(player.GetButton(IntGamePudNumber4)) || player.Release(player.GetButton(IntGamePudNumber5)))
        {
            if (IntGamePudNumber4 == 4 && holdButton)
            {
                holdButton = false;
            }
            if (IntGamePudNumber5 == 5 && holdButton)
            {
                holdButton = false;
            }
        }
        return holdButton;
    }
}
