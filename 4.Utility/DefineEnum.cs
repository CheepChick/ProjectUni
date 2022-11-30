using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefineEnum
{

    //임시
    public enum eSceneIndex
    {
        StartScene,
        IngameScene, 
    }
    public enum ePlayerActType
    {
        STAND = 0,
        WALK,
        RUN,
        ATTACK,
        WGS_STAND,
        WGS_WALK,
        WGS_RUN,
        SKILL1,
        SKILL2,
        SKILL3,
        SKILL4,
        HIT_FRONT,
        Hit_BACK,
        Battle,
        NonBattle,


        Die,
    }
    public enum eSkillPrefab
    {
        Skill,
    }
    public enum eHitFXPrefabs
    {
        Hit,
        CriticalHit
    }
    public enum eEtcFxPrefabs
    {
        LevelUP,
    }
    #region [몬스터]
    public enum eMonsterPrefab
    {
        Turnipa_Bitter,
        Turnipa_Sour,
        Turnipa_Sweet,
        Flower_Dryad,
    }

    public enum eMonsterActType
    {
        IDLE,
        MOVE,
        ATTACK,
        DAMAGE,
        DIE
    }

    #endregion[몬스터]

    #region [UI]
   
    public enum eUIType
    {
        InventoryUI,
    }
    #endregion 
    public enum eItemType
    {
        WeponItem,
        HatItem,
        AmorItem,
        AccItem,
        PotionItem,
        EtcItem,
    }

    public enum eEtcItemPrefabs
    {
        Gold,
    }
    public enum ePotionItemPrefabs
    {
        Red_Potion,
        Purple_Potion,
    }
    public enum eFontPrefabs
    {
        NormalDamage,
        CriticalDamage,
        MonsterDamage,
        LevelUp,
    }

    public enum eBGSound
    {
        See_The_Light,
        To_Mars_And_Back
    }
    public enum eSfxSound
    {
        Attack,
        Attack2
    }
    public static T StringToEnum<T>(string e)
    {
        return (T)Enum.Parse(typeof(T), e);
    }
}
