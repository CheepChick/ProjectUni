using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MonsterObj : StatBase
{
    [Header("Option")]
    [SerializeField] float _moveSpeed; 
    [SerializeField] float _limitRangeLnR = 5;
    [SerializeField] float _limitRanageFnB = 5;

    [Header("Connected Objects")]
    [SerializeField] BoxCollider[] _damageZone;
    [SerializeField] ShightRange _sightCS;
    [SerializeField] Transform _damageFont;
    [SerializeField] GameObject[] _damageText;
    [SerializeField] MonsterUI _monsterUI;


    Animator _monsterAni;
    NavMeshAgent _navMeshAgent;
    PlayerController _target;
    MapController _map;

    int _mapIndex;
    int _nowHP;
    int _exp;
    int _gold;
    List<int> _potionItemIDList;
    List<int> _etcItemIDList;
    List<int> _equipmentIDList;
    float _attackDistance;
    float _ShightDistance =10;
    Vector3 _originPos;

    DefineEnum.eMonsterActType _nowActType;
    float _timeCheck;
    bool _isAct = false;
    bool _isBattle = false;
    int _r = 0;

    public override int _maxHP
    {
        get { return _hp; }
    }
    public override int _finalDamage
    {
        get { return _att; }
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


    private void Awake()
    {
        ComponentSet();
    }
    private void Update()
    {
        if (_isDead)
            return;
        switch ( _nowActType)
        {
            case DefineEnum.eMonsterActType.IDLE:
                if (!_isAct)
                {
                    _r = Random.Range(2, 5);
                }
                _timeCheck += Time.deltaTime;
                if (_r <= _timeCheck)
                {
                    _isAct = false;
                    _timeCheck = 0;
                }               
                break;
            case DefineEnum.eMonsterActType.MOVE:
                if(_target != null)
                {
                    if (!_isAct)
                    {
                        if (Vector3.Distance(_target.transform.position, transform.position) <= _attackDistance)
                        {
                            ChangeMonsterAni(DefineEnum.eMonsterActType.ATTACK);
                            transform.LookAt(_target.transform);
                            _navMeshAgent.destination = transform.position;
                            _isAct=true;
                        }
                        else if (Vector3.Distance(_target.transform.position, transform.position) >= _ShightDistance)
                        {
                            ChangeMonsterAni(DefineEnum.eMonsterActType.MOVE);
                            _target = null;
                            _isBattle = false;
                        }
                        else
                        {
                            ChangeMonsterAni(DefineEnum.eMonsterActType.MOVE);
                            _navMeshAgent.destination = _target.transform.position;
                        }
                    }
                    else
                    {  
                        _navMeshAgent.destination = transform.position;
                    }
                }
                else
                {
                    if(_navMeshAgent.remainingDistance <= 0)
                        _isAct=false;
                }
                break;
            case DefineEnum.eMonsterActType.DAMAGE:
                _isAct = true;
                break;
        }
        if (!_isBattle)
        {
            if (!_isAct)
            {
                int rate = Random.Range(0, 2);
                MonActSet(rate);
                if (_nowActType == DefineEnum.eMonsterActType.IDLE && _navMeshAgent.remainingDistance >= 0.1f)
                {
                    _navMeshAgent.destination = transform.position;
                }
                _isAct = true;
            }
        }
    }

    void ComponentSet()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _monsterAni = GetComponent<Animator>();
        _originPos = transform.position;
        _potionItemIDList = new List<int>();
        _etcItemIDList = new List<int>();
        _equipmentIDList = new List<int>();
    }

    void HittingMonster(int damage, GameObject damageText)
    {
        int finalDamage = (damage * (1 - (_def / (300 + _def))));
        _nowHP -= finalDamage;

        float _rateHP = (float)_nowHP / (float)_maxHP;
        _monsterUI.SetHPRate(_rateHP,_target.gameObject.transform);

        ChangeMonsterAni(DefineEnum.eMonsterActType.DAMAGE);
        _isAct=true;
        if (_nowHP <= 0)
        {
            if (!_isDead)
            {
                _nowHP = 0;
                gameObject.layer = 0;
                GetComponent<Rigidbody>().Sleep();
                ChangeMonsterAni(DefineEnum.eMonsterActType.DIE);
                GoldDrop();
                ItemDrop();
                _target.GetExp(_exp);
                StartCoroutine(SetDisabled());

            }

        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        //데미지를 받았을 때 
        if (other.CompareTag("Damage"))
        {
           DamageZone  damage= other.GetComponent<DamageZone>();

            if (!_isBattle)
            {
                _isBattle = true;
                SightOn(damage._owner);
            }

            int[] fDamage = damage._finalDamage ;
            Vector3 vec = transform.position;
            
            Vector3 fontTransform = _damageFont.position;
            for(int i=0; i< fDamage.Length; i++)
            {
                int r = Random.Range(0, 1000);
                vec.z += 0.1f;
                fontTransform.y += 0.2f;
                fontTransform.z += 0.2f;
                if (r < damage._owner._criticalRate * 10)
                {

                    GameObject go = Instantiate(ResourcesPoolManager._instance.GetHitFXPrefab(DefineEnum.eHitFXPrefabs.CriticalHit), vec, Quaternion.identity);
                    Destroy(go, 1.0f);
                    fDamage[i] = (int)(fDamage[i] * damage._owner._criticalDemage);

                    GameObject damgeFont = Instantiate(damage._damageFont[1], _damageFont);

                    damgeFont.GetComponent<DamageText>().SetText(fDamage[i].ToString(), fontTransform);
                    HittingMonster(fDamage[i], damage._damageFont[1]);
                    

                }
                else
                {

                    GameObject go = Instantiate(ResourcesPoolManager._instance.GetHitFXPrefab(DefineEnum.eHitFXPrefabs.Hit), vec, Quaternion.identity);
                    Destroy(go, 1.0f);
                    GameObject damgeFont = Instantiate(damage._damageFont[0], _damageFont);
                    damgeFont.GetComponent<DamageText>().SetText(fDamage[i].ToString(), fontTransform);
                    HittingMonster(fDamage[i], damage._damageFont[0]);
                    
                }
            }
           
        }
    }

    Vector3 NextGoalPositon()
    {
        Vector3 goal = _originPos;
        float px = Random.Range(-_limitRangeLnR, _limitRangeLnR);
        float pz = Random.Range(-_limitRanageFnB, _limitRanageFnB);
        goal += new Vector3(px, 0, pz);

        return goal;
    }
    void MonsterAI()
    {
        _navMeshAgent.destination = NextGoalPositon();
    }
    void MonActSet(int rate)
    {
        switch (rate)
        {
            case 0:
                ChangeMonsterAni(DefineEnum.eMonsterActType.IDLE);

                break;
            case 1:
                ChangeMonsterAni(DefineEnum.eMonsterActType.MOVE);
                MonsterAI();
                break;
        }
    }
    void ChangeMonsterAni(DefineEnum.eMonsterActType type)
    {
        if (_isDead)
            return;

        switch (type)
        {
            case DefineEnum.eMonsterActType.IDLE:
                break;
            case DefineEnum.eMonsterActType.MOVE:
                _navMeshAgent.stoppingDistance = 0;
                _navMeshAgent.speed = _moveSpeed;
                break;
            case DefineEnum.eMonsterActType.ATTACK:
                _damageZone[0].enabled = true;
                break;
            case DefineEnum.eMonsterActType.DIE:
                _isDead = true;
                _navMeshAgent.destination = transform.position; 
                _navMeshAgent.speed = 0;
                _monsterAni.SetTrigger("IsDead");
                _navMeshAgent.enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                for(int i=0; i <_damageZone.Length; i++)
                {
                    _damageZone[i].enabled = false;
                }
                break;
        }
        _monsterAni.SetInteger("AniStat", (int)type);
    }

    //죽었을 때 
    IEnumerator SetDisabled()
    {
        yield return new WaitForSeconds(3.0f);
        Invoke("Respawn", 4.0f);
        transform.gameObject.SetActive(false);
    }
    void Respawn()
    {
        _map.RespawnMonster(_mapIndex);
        Destroy(gameObject);
    }

    void GoldDrop()
    {
        GameObject gold = Instantiate(ResourcesPoolManager._instance.GetEtcItemPrefab(DefineEnum.eEtcItemPrefabs.Gold), transform.position, transform.rotation);
        gold.TryGetComponent<DropItem>(out DropItem dropItem);
        dropItem.SetGold(_gold);
        dropItem.SetEndPos(transform.position);
    }
    void ItemDrop()
    {
        for(int i=0; i< _potionItemIDList.Count; i++)
        {
            int r = Random.Range(0, 100);
            PotionItemData potionItem = DataBaseManager._instance._potionItemList[_potionItemIDList[i]];
            if(r < potionItem.DropRate)
            {
                DefineEnum.ePotionItemPrefabs Name = DefineEnum.StringToEnum<DefineEnum.ePotionItemPrefabs>(potionItem.Name);
                GameObject Item = Instantiate(ResourcesPoolManager._instance.GetPotionItemPrefab(Name),transform.position,transform.rotation);
                Item.TryGetComponent<DropItem>(out DropItem dropItem);
                dropItem.SetItem(potionItem);
                dropItem.SetEndPos(transform.position);
            }
        }
    }

    // 플레이어 감지
    public override bool SightOn(StatBase target)
    {
        if(_target == target)
        {
            return true;
        }
        _isBattle = true;
        _nowActType = DefineEnum.eMonsterActType.MOVE;
        _target = (PlayerController)target;
        _isAct = false;
        return false;
    }

    //몬스터 데이터 세팅
    public void InitSetData(int Level,MonsterData data , MapController map, int mapIndex)
    {
        _name = data.Name ;
        _att = data.Atk;
        _def = data.Def;
        _hp = data.Hp;
        _nowHP = _maxHP;
        _attackDistance = data.Range;
        for(int i=0; i<Level; i++)
        {
            _att += (int)(data.Atk * 3 * Level * 0.05);
            _def += (int)(data.Def * 3 * Level * 0.001);
            _hp += (int)(data.Hp * 3 * Level * 0.1);
        }
        float combatpower = _att * 4 + _hp + _def * 3;
        _exp = (int)(combatpower * 0.1);
        _gold = (int)(combatpower * 0.5);
        for (int i = 0; i < _damageZone.Length; i++)
        {
            DamageZone damage = _damageZone[i].GetComponent<DamageZone>();
            damage.MonsterInitDataSet(this, _damageText);
            _damageZone[i].enabled = false;
        }
        _sightCS.InitSet(this);
        _monsterUI.SetMonsterUI(_name);
        _map = map;
        _mapIndex = mapIndex;

        for(int i=0; i< data.DropEquipmentItem.Length; i++)
        {
            _equipmentIDList.Add(data.DropEquipmentItem[i]);
        }
        for(int i=0; i< data.DropPotionItem.Length; i++)
        {
            _potionItemIDList.Add(data.DropPotionItem[i]);
        }
        for(int i=0; i<data.DropEtcItem.Length; i++)
        {
            _etcItemIDList.Add(data.DropEtcItem[i]);
        }
    }
   
    //몬스터 공격쿨타임
    //애니메이션 이벤트함수
    public IEnumerator MonsterAttackEnd(int num)
    {
        ChangeMonsterAni(DefineEnum.eMonsterActType.IDLE);
        _damageZone[num].enabled = false;

        yield return new WaitForSeconds(1.5f);
        _isAct = false;    
    }
    public void MonsterHitDamageEnd()
    {
        ChangeMonsterAni(DefineEnum.eMonsterActType.IDLE);
        _isAct = false;
    }
}
