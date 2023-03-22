using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] Player player;
    public GameObject target;
    public float playerMagnetDistance = 2.5f; //자석범위
    public HealthItemData healthItemData; //경험치 관련
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); //자석관련
    }

    // Update is called once per frame
    void Update()
    {
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
