using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : StatBase
{
    [Header("Option")]
    [SerializeField] float _speed;
    [SerializeField] float _runSpeed;
    [SerializeField] float _jumpSpeed;
    [SerializeField] float _gravity;
    [SerializeField] float _rotAngle;
    [SerializeField] LayerMask _layerMask;
    
    [Header("Connected Objects")]
    [SerializeField] Transform _hitDamage;
    [SerializeField] GameObject _Wepon;
    [SerializeField] GameObject _Wepon_hold;


    //임시
    [SerializeField] GameObject[] _eppect;
    [SerializeField] GameObject[] _damageFont;

    PlayerUI _playerUI;
    Animator _charaterAni;
    CharacterController _characterController;
    private Vector3 _moveDir;
    
    bool _isDie = false;
    bool _isBattle = false;
    bool _isSkill = false;
    int _nowHP;
    float _nowExp;
    float _remainingExp;

    List<Transform> _respwanPoint;
    public bool _isAttack { get; set; }
    
    public override int _maxHP
    {
        get { return _hp; }
    }
    public override int _finalDamage
    {
        get 
        {
           return _att;
        }

    }
    public override int _finalDeffence
    {
        get { return _def; }
    }
    public override float _criticalRate
    {
        get { return _critical; }
    }
    public override float _criticalDemage
    {
        get { return _cDamage; }
    }

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        if (_isDie)
            return;

        if (!_isSkill)
        {
            if (IsGrounded())
            {
                float rx = Input.GetAxis("Horizontal");
                float mz = Input.GetAxis("Vertical");

                _moveDir = transform.forward * mz * _speed;
                transform.Rotate(Vector3.up * rx * _rotAngle * Time.deltaTime);

                if (!_isBattle)
                {
                    if (mz != 0 || rx != 0)
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                            ChangeCharaterAnimation(DefineEnum.ePlayerActType.RUN);
                        else
                            ChangeCharaterAnimation(DefineEnum.ePlayerActType.WALK);

                    }
                    else
                    {
                        ChangeCharaterAnimation(DefineEnum.ePlayerActType.STAND);
                    }
                    if (Input.GetKey("x"))
                    {

                        ChangeCharaterAnimation(DefineEnum.ePlayerActType.Battle);
                        _Wepon_hold.SetActive(false);
                        _Wepon.SetActive(true);
                    }
                    if (Input.GetKey("c"))
                        _moveDir.y = _jumpSpeed;
                }
                else
                {
                    if (mz != 0 || rx != 0)
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                            ChangeCharaterAnimation(DefineEnum.ePlayerActType.WGS_RUN);
                        else
                            ChangeCharaterAnimation(DefineEnum.ePlayerActType.WGS_WALK);
                    }
                    else
                    {
                        if (!_isAttack)
                            ChangeCharaterAnimation(DefineEnum.ePlayerActType.WGS_STAND);
                    }
                    if (Input.GetKeyDown("x"))
                    {

                        ChangeCharaterAnimation(DefineEnum.ePlayerActType.ATTACK);
                    }
                    if (Input.GetKeyDown("c"))
                    {
                        _moveDir.y = _jumpSpeed;
                    }

                    if (Input.GetKeyDown("q"))
                    {
                        ChangeCharaterAnimation(DefineEnum.ePlayerActType.SKILL1);
                    }

                    //if (Input.GetKeyDown("w"))
                    //{
                    //    ChangeCharaterAnimation(DefineEnum.ePlayerActType.SKILL2);
                    //}
                }
            }
            else
            {
                if (Input.GetKeyDown("e"))
                {
                    _moveDir.y = _jumpSpeed;
                    ChangeCharaterAnimation(DefineEnum.ePlayerActType.SKILL3);

                }
            }
        }
        else
        {
            _moveDir = Vector3.zero;
        }
        if (_isAttack)
            _moveDir *= 0.1f;
        InputKey();
        InGameManager._instance.TryOpenInventory();
        LevelUP();
        UpdateUI();
        _moveDir.y -= _gravity * Time.deltaTime;
        _characterController.Move(_moveDir * Time.deltaTime);
        FallPlayer();
    }
    void Init() 
    {
        _moveDir = Vector3.zero;
        _characterController = GetComponent<CharacterController>();
        _charaterAni = GetComponent<Animator>();
        InitSetPlayer(DataBaseManager._instance._playerInfo);
        _playerUI = InGameManager._instance.MainUI.GetComponent<PlayerUI>();
        _playerUI.SetPlayerUI(_nowHP, _maxHP, _nowExp, _remainingExp, _level);
        InGameManager._instance.GetPlayer(this);
    }
    bool IsGrounded()
    {
        float extraHeight = -0.3f;
        RaycastHit raycastHit;
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        Physics.Raycast(ray, out raycastHit, _characterController.bounds.extents.y + extraHeight, _layerMask);
        if (raycastHit.collider != null)
            return true;
        else
            return false;
    }

    void FallPlayer()
    {
        if(transform.position.y < -20.0f)
        {
            _characterController.enabled = false;
            transform.position = _respwanPoint[0].position;
            _characterController.enabled = true;
        }
    }

    void InputKey()
    {
        if (Input.GetKeyDown(InGameManager._instance.QuickSlot.QuickSlotList[0].SlotKey))
        {
            InGameManager._instance.Inventory.Use(InGameManager._instance.QuickSlot.QuickSlotList[0].DataIndex);
            InGameManager._instance.Inventory.SendQuick(InGameManager._instance.QuickSlot.QuickSlotList[0], InGameManager._instance.QuickSlot.QuickSlotList[0].DataIndex);
        }
        if (Input.GetKeyDown(InGameManager._instance.QuickSlot.QuickSlotList[1].SlotKey))
        {
            InGameManager._instance.Inventory.Use(InGameManager._instance.QuickSlot.QuickSlotList[1].DataIndex);
            InGameManager._instance.Inventory.SendQuick(InGameManager._instance.QuickSlot.QuickSlotList[1], InGameManager._instance.QuickSlot.QuickSlotList[1].DataIndex);
        }
        if (Input.GetKeyDown(InGameManager._instance.QuickSlot.QuickSlotList[2].SlotKey))
        {
            InGameManager._instance.Inventory.Use(InGameManager._instance.QuickSlot.QuickSlotList[2].DataIndex);
            InGameManager._instance.Inventory.SendQuick(InGameManager._instance.QuickSlot.QuickSlotList[2], InGameManager._instance.QuickSlot.QuickSlotList[2].DataIndex);
        }
        if (Input.GetKeyDown(InGameManager._instance.QuickSlot.QuickSlotList[3].SlotKey))
        {
            InGameManager._instance.Inventory.Use(InGameManager._instance.QuickSlot.QuickSlotList[3].DataIndex);
            InGameManager._instance.Inventory.SendQuick(InGameManager._instance.QuickSlot.QuickSlotList[3], InGameManager._instance.QuickSlot.QuickSlotList[3].DataIndex);
        }
        if (Input.GetKeyDown(InGameManager._instance.QuickSlot.QuickSlotList[4].SlotKey))
        {
            InGameManager._instance.Inventory.Use(InGameManager._instance.QuickSlot.QuickSlotList[4].DataIndex);
            InGameManager._instance.Inventory.SendQuick(InGameManager._instance.QuickSlot.QuickSlotList[4], InGameManager._instance.QuickSlot.QuickSlotList[4].DataIndex);
        }
    }
    void ChangeCharaterAnimation(DefineEnum.ePlayerActType type)
    {
        switch (type)
        {
            case DefineEnum.ePlayerActType.STAND:
                _charaterAni.SetBool("IsBattle", _isBattle);
                break;
            case DefineEnum.ePlayerActType.WALK:
                _moveDir *= _speed;
                break;
            case DefineEnum.ePlayerActType.RUN:
                _moveDir *= _runSpeed;
                break;
            case DefineEnum.ePlayerActType.ATTACK:
                _isAttack = true;
                _charaterAni.SetBool("IsAttack", _isAttack);
                break;
            case DefineEnum.ePlayerActType.WGS_STAND:
                break;
            case DefineEnum.ePlayerActType.WGS_WALK:
                _moveDir *= _speed;
                break;
            case DefineEnum.ePlayerActType.WGS_RUN:
                _moveDir *= _runSpeed;
                break;
            case DefineEnum.ePlayerActType.SKILL1:
                _isSkill = true;
                break;
            case DefineEnum.ePlayerActType.SKILL2:
                _isSkill = true;
                break;
            case DefineEnum.ePlayerActType.SKILL3:
                _isSkill = true;
                break;
            case DefineEnum.ePlayerActType.Battle: 
                _isBattle = true;
                _charaterAni.SetBool("IsBattle", _isBattle);
                _charaterAni.SetTrigger("ISBattle");
                break;
            case DefineEnum.ePlayerActType.Die:
                _charaterAni.SetTrigger("IsDie");
                _isDie = true;
               
                break;
        }
        _charaterAni.SetInteger("AniStat", (int)type);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UserHit"))
        {
            DamageZone damage = other.gameObject.GetComponent<DamageZone>();
            Hitting(damage._finalDamage, damage._damageFont[0]);
        }
    }
    void Hitting(int[] damage, GameObject font)
    {
        for (int i = 0; i < damage.Length; i++)
        {
            _nowHP -= damage[i];
            GameObject go = Instantiate(font, _hitDamage);
            go.GetComponent<DamageText>().SetText(damage[i].ToString(),transform.position);
            if (_nowHP <= 0)
            {
                _nowHP = 0;
                _characterController.enabled = false;
                ChangeCharaterAnimation(DefineEnum.ePlayerActType.Die);
                UpdateUI();
                InGameManager._instance.MainUI.GetComponent<PlayerUI>().PlayerDead();
            }
        }
    }
    void LevelUP()
    {
        if(_nowExp > _remainingExp)
        {
            GameObject go =  Instantiate(ResourcesPoolManager._instance.GetEtcFxPrefab(DefineEnum.eEtcFxPrefabs.LevelUP), transform);
            Destroy(go,1.0f);
            go = Instantiate(ResourcesPoolManager._instance.GetFontPrefab(DefineEnum.eFontPrefabs.LevelUp), _hitDamage);
            go.GetComponent<DamageText>().SetText("Level UP",transform.position);
            int n = 3;
            _level ++;
            _att += (int)(n*(25 + n * _level) * 0.15);
            _def += (int)(n*(1 + n * _level) * 0.01);
            _hp += (int)(n * (25 + n * _level));
            _nowHP = _hp;
            _nowExp = _nowExp - _remainingExp;
            _remainingExp = (int)(_remainingExp + _remainingExp * 0.12f);
            
            PlayerInfo playerinfo = new PlayerInfo();
            playerinfo._level = _level;
            playerinfo._att = _att;
            playerinfo._def = _def;
            playerinfo._hp = _hp;
            playerinfo._nowExp = (int)_nowExp;
            playerinfo._remainingExp = (int)_remainingExp;
            DataBaseManager._instance._playerInfo = playerinfo;
            DataBaseManager._instance.SavePlayerStat();
        }
    }
    void UpdateUI()
    {
        _playerUI.SetExpRate(_level,_nowExp, _remainingExp);
        _playerUI.SetHPRate(_nowHP, _maxHP);
    }



    public void InitSetPlayer(PlayerInfo player)
    {
        _level = player._level;
        _att = player._att;
        _def = player._def;
        _hp = player._hp;
        _nowHP = player._hp;
        _critical = player._Cri;
        _cDamage = player._CriDamage;
        _nowExp = player._nowExp;
        _remainingExp = player._remainingExp;

    }
    public void GetExp(float exp)
    {
        _nowExp += exp;
        DataBaseManager._instance._playerInfo._nowExp += (int)exp;
        DataBaseManager._instance.SavePlayerStat();
    }
    
    public void UsePotion(PotionItemData potion)
    {

        _nowHP += (int)potion.IntValue;
        _nowHP += (int)(_maxHP * potion.FloatValue);
        if(_nowHP >= _maxHP)
        {
            _nowHP = _maxHP;
        }
    }

    public void GetRespwanPoint(List<Transform> playerTF) => _respwanPoint = playerTF;

    public void RespawnPlayer()
    {
        _nowHP = _maxHP;
        transform.position = _respwanPoint[0].position;
        ChangeCharaterAnimation(DefineEnum.ePlayerActType.STAND);
        _characterController.enabled = true;
        UpdateUI();
        _isDead = false;
        _isDie = false;
    }
    // 애니메이션 이벤트 함수들
    public void ChangeBattle()
    {
        if (_isBattle)
        {
            _Wepon_hold.SetActive(false);
            _Wepon.SetActive(true);
        }
    }

    
    public void AttackStart(int attacknum)
    {
        _isAttack = false;
        _charaterAni.SetBool("IsAttack", _isAttack);
        DefineEnum.eSfxSound sfxName;
        if (attacknum > 3)
        {
            sfxName = DefineEnum.StringToEnum<DefineEnum.eSfxSound>("Attack");
        }
        else
            sfxName = DefineEnum.StringToEnum<DefineEnum.eSfxSound>("Attack2");
        AudioClip sfx =  ResourcesPoolManager._instance.GetSfxAudio(sfxName);
        SoundManager._instance.SfxSoundPlay(sfx);
    }
    public void AttackEnd(int attacknum)
    {
        
        GameObject go = Instantiate(_eppect[attacknum],transform);
        DamageZone damage = go.GetComponent<DamageZone>();
        damage.UserInitDataSet(this,_damageFont);
        Destroy(go, 1.0f);
        
        _charaterAni.SetBool("IsAttack", _isAttack);
    }

    public void AttackFinal()
    {
        _isAttack = false;
        _charaterAni.SetBool("IsAttack", _isAttack);
    }

    public void SkillStart()
    {
        GameObject go = Instantiate(ResourcesPoolManager._instance.GetSkillPrefab(DefineEnum.eSkillPrefab.Skill),transform);
        DamageZone damage = go.GetComponent<DamageZone>();
        damage.UserInitDataSet(this,_damageFont);
        Destroy (go, 1.0f);
        _isSkill = true;
    } 
    public void SkillEnd()
    {
        _isSkill = false;
    }

   
}
