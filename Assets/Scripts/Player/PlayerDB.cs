using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDB : MonoBehaviour
{
    [SerializeField] private int number;
    [SerializeField] private float maxHp;
    [SerializeField] private float hp;
    [SerializeField] private int winPoints;
    [SerializeField] private bool isAlive;


    //?
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject icon;


    private void Start()
    {
        ////hp = maxHp;
        //Debug.Log("HPèâä˙ílë„ì¸ÅF" + hp);
        //Debug.Log("éØï î‘çÜÅF" + number);
    }

    public int MyNumber { get { return number; } }
    public float MyMaxHp { get { return maxHp; } }
    public float MyHp { get { return hp; } }
    public int MyWinPoints { get { return winPoints; } }
    public bool MyIsAlive { get { return isAlive; } }


    public GameObject MyHpBar { get { return hpBar; } }
    public GameObject MyIcon { get { return icon; } }

    public void SetNumber(int _number) { number = _number; }
    public void SetMaxHp(float _maxHP) { maxHp = _maxHP; }
    public void SetHp(float _hp) { hp = _hp; }
    public void SetWinPoints(int _winPoints) { winPoints = _winPoints; }
    public void SetIsAlive(bool _isAlive) { isAlive = _isAlive; }

    public void SetHpBar(GameObject _hpBar) { hpBar = _hpBar; }
    public void SetIcon(GameObject _icon) { icon = _icon; }

}
