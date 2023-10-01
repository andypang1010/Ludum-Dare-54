using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 1;

    private void Update() {
        Vector3 moveDir = (player.position - transform.position).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}