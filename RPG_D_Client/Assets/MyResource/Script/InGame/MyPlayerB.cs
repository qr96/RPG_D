using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerB : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Animator animator;
    public TriggerEvent2D attackTrigger;

    public float speed;
    public float pushPower;
    public float attackDelay = 1f;

    Transform nameTag;

    HashSet<IDamageable> targets = new HashSet<IDamageable>();

    Vector2 input;
    DateTime attackEnd;
    bool isOnAttack;

    private void Start()
    {
        //attackTrigger.SetTriggerEvent(onStay: OnAttack);
        attackTrigger.SetTriggerEvent(onEnter: OnAttackStart, onExit: OnAttackEnd);
    }

    private void Update()
    {
        input = Managers.input.GetPlayerInputAxis();

        if (isOnAttack)
            OnAttack();
        else
            animator.SetBool("Moving", input.sqrMagnitude > 0);

        rigid.velocity = new Vector2(input.x * speed * Time.fixedDeltaTime, input.y * speed * Time.fixedDeltaTime);
        
        if (input.x > 0)
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        else if (input.x < 0)
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10f);

        if (nameTag != null)
            nameTag.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 1f, 0f));
    }

    public void SetNameTag(string name)
    {
        if (nameTag != null)
        {
            Managers.ui.GetLayout<UILayoutNameTag>().RemoveNameTag(gameObject);
            nameTag = null;
        }

        nameTag = Managers.ui.GetLayout<UILayoutNameTag>().AcquireNameTag(gameObject, name);
    }

    void OnAttack()
    {
        if (DateTime.Now < attackEnd)
            return;
        StartCoroutine(AttackCoroutine(0.3f));
    }

    void OnAttackStart(Collider2D collider)
    {
        var target = collider.gameObject.GetComponent<IDamageable>();

        if (target != null)
            targets.Add(target);
        if (targets.Count > 0)
            isOnAttack = true;
    }

    void OnAttackEnd(Collider2D collider)
    {
        targets.Remove(collider.GetComponent<IDamageable>());
        if (targets.Count == 0)
            isOnAttack = false;
    }

    IEnumerator AttackCoroutine(float delay)
    {
        rigid.velocity = Vector3.zero;
        attackEnd = DateTime.Now.AddSeconds(attackDelay);
        animator.SetBool("Moving", false);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(delay);

        foreach (var target in targets)
            target.OnDamage(10);
    }
}
