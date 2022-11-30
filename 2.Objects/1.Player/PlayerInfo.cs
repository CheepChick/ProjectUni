using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInfo
{
    public int _level;
    public int _att;
    public int _def;
    public int _hp;
    public int _Cri;
    public float _CriDamage;
    public int _nowExp;
    public int _remainingExp;
    public StageClear _stageClear;
    public void SetPlayerInfo(PlayerInfo player)
    { 
        if(player == null)
        {
            _level = 1;
            _att = 25;
            _def = 20;
            _hp = 100;
            _Cri = 15;
            _CriDamage = 1.5f;
            _nowExp = 0;
            _remainingExp = 100;
            _stageClear = new StageClear(0, 0);
        }
        else
        {
            _level = player._level;
            _att = player._att;
            _def = player._def;
            _hp = player._hp;
            _Cri = player._Cri;
            _CriDamage = player._CriDamage;
            _nowExp = player._nowExp;
            _remainingExp = player._remainingExp;
            _stageClear = player._stageClear;
        }
    }
}
[Serializable] 
public struct StageClear
{
    public int _stage;
    public int _area;

    public StageClear(int stage, int area)
    {
        _stage = stage;
        _area = area;
    }
}

