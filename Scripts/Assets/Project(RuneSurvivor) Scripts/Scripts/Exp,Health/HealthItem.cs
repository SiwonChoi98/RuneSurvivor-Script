using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] Player player;
    public GameObject target;
    public float playerMagnetDistance = 2.5f; //�ڼ�����
    public HealthItemData healthItemData; //����ġ ����
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); //�ڼ�����
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
        if (other.CompareTag("Player")) //�÷��̾�� ������ ����
        {
            Destroy(gameObject);
        }
    }
}
