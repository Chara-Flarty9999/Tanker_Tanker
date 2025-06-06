using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// ナイフと名前がついてはいるが実際は指定した方向にスプライトが飛んでいくもの。
///汎用性だけは普通に高い。
///別の空オブジェクトで_rote, _magnificationを設定し、あと座標をInstantiateで設定するだけ。
///サウンドはインスペクターで設定してください。
///敵用のスクリプトである。
/// </summary>
public class EnemyExplodeBullet : MonoBehaviour
{
    Collider collider;
    MeshRenderer mesh;
    Vector3 movement;
    Rigidbody rigidbody;
    AudioSource audioSource;
    /// <summary>
    /// 召喚時の音
    /// </summary>
    [SerializeField] AudioClip hit;
    /// <summary>
    /// 飛んでく時の音
    /// </summary>
    [SerializeField] AudioClip fly;
    [SerializeField] AudioClip explode;
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
    [SerializeField] GameObject player;
    [SerializeField] GameObject explode_Effect;
    GameObject[] delete_exploEffect;

    // Start is called before the first frame update
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mesh = GetComponent<MeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        audioSource.PlayOneShot(fly);
        //rigidbody.AddForce((player.transform.forward / 30 + player.transform.up / 15) * 150, ForceMode.Impulse);
    }




    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider collision)
    {
        string tagcheck = collision.gameObject.tag;
        if (tagcheck != "Enemy" && tagcheck != "Outside_Explode" && tagcheck != "Inside_Explode")
        {
            Instantiate(explode_Effect, transform.position, Quaternion.identity);
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.useGravity = false;
            collider.enabled = false;
            delete_exploEffect = GameObject.FindGameObjectsWithTag("Outside_Explode");
            AudioSource.PlayClipAtPoint(explode, transform.position);
            for (int i = 0; i < delete_exploEffect.Length; i++)
            {
                Destroy(delete_exploEffect[i], 1.8f);
            }
            Destroy(gameObject, 1.8f);
        }
    }

    public void OnDestroy()
    {

    }
}