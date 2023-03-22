using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    Rigidbody rigid;
    [SerializeField] Player player;
    public GameObject target;
    public float playerMagnetDistance = 2.5f; //자석범위

    public ExpData expData; //경험치 관련

    //public bool magnetTime;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); //자석관련
    }
    void Update()
    {   //자석관련
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < playerMagnetDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.55f);
        }
    }       
    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) //플레이어랑 닿으면 삭제
        {
            Destroy(gameObject);
        }
    }

}