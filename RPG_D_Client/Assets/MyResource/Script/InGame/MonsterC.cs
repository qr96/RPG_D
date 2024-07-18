using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterC : MonoBehaviour, IDamageable, IPushable
{
    public Rigidbody2D rigid;

    GameObject IPushable.gameObject { get => gameObject; }

    public void OnDamage(long damage)
    {

    }

    public void OnPushed(Vector2 pushVector)
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(pushVector, ForceMode2D.Impulse);
    }
}
