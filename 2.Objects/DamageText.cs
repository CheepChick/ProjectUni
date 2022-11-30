using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float _moveSpeed;
    private float _alphaSpeed;
    private float _destroyTime;
    TextMeshPro _textMesh;
    Color alpha;
    Vector3 _move = Vector3.zero;
    Transform _target;
    public float damage { get; set; }
    string _text;
    private void Start()
    {
        _moveSpeed = 1f;
        _alphaSpeed = 1f;
        _destroyTime = 2.0f;       
        _textMesh = GetComponent<TextMeshPro>();
        alpha = _textMesh.color;
        Destroy(gameObject, _destroyTime);
    }
    private void Update()
    {
        _target = GameObject.FindGameObjectWithTag("MainCamera").transform;
        transform.parent.LookAt(_target);
        transform.Translate(new Vector3(0, _moveSpeed * Time.deltaTime, 0));
        _textMesh.text = _text;
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * _alphaSpeed);
        _textMesh.color = alpha;

    }
    public void SetText(string s ,Vector3 trans)
    {
        _text = s;
        transform.position = trans;
    }
    
}
