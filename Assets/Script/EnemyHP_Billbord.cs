using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(RectTransform))]
public class EnemyHP_Billbord : MonoBehaviour
{
    [SerializeField]
    private Transform target = default;
    [SerializeField]
    private Image arrow = default;

    GameObject playerCamera;
    private Camera mainCamera;
    private RectTransform rectTransform;

    void Update()
    {
        Vector3 p = Camera.main.transform.position;
        p.y = transform.position.y;
        transform.LookAt(p);
    }
}