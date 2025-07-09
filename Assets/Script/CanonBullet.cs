using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
///キャノン砲の弾クラス。
///敵味方問わず、キャノン砲の弾はこのクラスを使用する。
///自身が撃った球も自分自身にダメージ判定があるため、自爆しないように注意すること。
/// </summary>
public class CanonBullet : MonoBehaviour
{
    Collider _collider;
    MeshRenderer _mr;
    Rigidbody _rigidbody;
    AudioSource _audioSource;
    /// <summary>
    /// 召喚時の音
    /// </summary>
    [SerializeField] AudioClip _hit;
    /// <summary>
    /// 飛んでく時の音
    /// </summary>
    [SerializeField] AudioClip _fly;
    [SerializeField] AudioClip _explode;
    /// <summary>
    /// ナイフが飛んでく角度
    /// </summary>
    int _rote;
    float _rotation;
    /// <summary>
    /// ナイフの加速度
    /// </summary>
    float _magnification;
    float _waitTime;

    public bool m_play = true;
    [SerializeField] GameObject _explodeEffect;
    GameObject[] _deleteExploEffect;

    // Start is called before the first frame update
    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _mr = GetComponent<MeshRenderer>();
        _audioSource.PlayOneShot(_fly);
        //_rigidbody.AddForce((player.transform.forward / 30 + player.transform.up / 15) * 150, ForceMode.Impulse);
    }




    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Hit : " +  collision.gameObject.name);
        string tagcheck = collision.gameObject.tag;
        if (tagcheck != "Outside_Explode" && tagcheck != "Inside_Explode")
        {
            if (_explodeEffect)
                Instantiate(_explodeEffect, transform.position, Quaternion.identity);

            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.useGravity = false;
            _collider.enabled = false;
            _mr.enabled = false;
            _deleteExploEffect = GameObject.FindGameObjectsWithTag("Outside_Explode");
            AudioSource.PlayClipAtPoint(_explode, transform.position);
            for (int i = 0; i < _deleteExploEffect.Length; i++)
            {
                Destroy(_deleteExploEffect[i], 1.8f);
            }
            Destroy(gameObject, 1.8f);
        }
    }

    public void OnDestroy()
    {

    }
}