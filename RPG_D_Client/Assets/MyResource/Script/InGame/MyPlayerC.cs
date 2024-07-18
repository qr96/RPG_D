using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerC : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Animator animator;
    public TriggerEvent2D attackTrigger;

    public float speed;
    public float pushPower;
    public float attackDelay = 1f;

    Transform nameTag;

    HashSet<IPushable> targets = new HashSet<IPushable>();

    Vector2 input;
    DateTime attackEnd;
    bool isOnAttack;

    private void Start()
    {
        attackTrigger.SetTriggerEvent(onEnter: OnAttackStart, onExit: OnAttackEnd);
    }

    private void Update()
    {
        input = Managers.input.GetPlayerInputAxis();

        if (isOnAttack)
            return;
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
        StartCoroutine(AttackCoroutine(attackDelay));
    }

    void OnAttackStart(Collider2D collider)
    {
        var target = collider.gameObject.GetComponent<IPushable>();

        if (target != null)
            targets.Add(target);

        OnAttack();
    }

    void OnAttackEnd(Collider2D collider)
    {
        targets.Remove(collider.GetComponent<IPushable>());
    }

    IEnumerator AttackCoroutine(float delay)
    {
        rigid.velocity = Vector3.zero;
        attackEnd = DateTime.Now.AddSeconds(attackDelay);
        animator.SetBool("Moving", false);
        animator.SetTrigger("Attack");

        foreach (var target in targets)
        {
            var enemyVec = target.gameObject.transform.position - transform.position;
            var inputVec = new Vector2(input.x, input.y);
            var deg = Vector2.Angle(enemyVec, inputVec);
            var pushVec = RDUtil.VectorToOcta(inputVec);

            if (Mathf.Abs(deg) > 90)
                yield break;

            if (inputVec.Equals(Vector2.zero))
                yield break;

            target.OnPushed(pushVec * pushPower);
        }

        isOnAttack = true;
        yield return new WaitForSeconds(delay);
        isOnAttack = false;
    }
}
