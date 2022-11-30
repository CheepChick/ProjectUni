using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesPoolManager : TSingleton<ResourcesPoolManager>
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject[] _monsters;
    [SerializeField] GameObject[] _AttackFx;
    [SerializeField] GameObject[] _skillFx;
    [SerializeField] GameObject[] _hitFx;
    [SerializeField] GameObject[] _etcFx;
    [SerializeField] GameObject[] _damageFont;
    [SerializeField] GameObject[] _etcItems;
    [SerializeField] GameObject[] _potionItems;
    [SerializeField] AudioClip[] _backGroundSound;
    [SerializeField] AudioClip[] _sfxSound;

    public GameObject Player => _player;
    private void Awake()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();

    }

    public GameObject GetMonsterPrefab(DefineEnum.eMonsterPrefab name)
    {
        return _monsters[(int)name];
    }
    public GameObject GetEtcItemPrefab(DefineEnum.eEtcItemPrefabs name)
    {
        return _etcItems[(int)name];
    }
    public GameObject GetPotionItemPrefab(DefineEnum.ePotionItemPrefabs name)
    {
        return _potionItems[(int)name];
    }
    public GameObject GetSkillPrefab(DefineEnum.eSkillPrefab name)
    {
        return _skillFx[(int)name];
    }
    public GameObject GetHitFXPrefab(DefineEnum.eHitFXPrefabs name)
    {
        return _hitFx[(int)name];
    }
    public GameObject GetEtcFxPrefab(DefineEnum.eEtcFxPrefabs name)
    {
        return _etcFx[(int)name];
    }
    public GameObject GetFontPrefab(DefineEnum.eFontPrefabs name)
    {
        return _damageFont[(int)name];
    }
    public AudioClip GetBGAudio(DefineEnum.eBGSound name)
    {
        return _backGroundSound[(int)name];
    }
    public AudioClip GetSfxAudio(DefineEnum.eSfxSound name)
    {
        return _sfxSound[(int)name];
    }
}
