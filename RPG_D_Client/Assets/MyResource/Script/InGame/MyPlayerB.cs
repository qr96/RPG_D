using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerB : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Animator animator;

    public float speed;
    public float pushPower;
    public float attackDelay;

    Transform nameTag;

    Vector2 input;
    DateTime attackEnd;

    private void Update()
    {
        if (DateTime.Now < attackEnd)
            return;
        else
            animator.SetBool("AttackEnd", true);

        input = Managers.input.GetPlayerInputAxis();

        if (input.sqrMagnitude > 1)
            input = input.normalized;

        rigid.velocity = new Vector2(input.x * speed * Time.fixedDeltaTime, input.y * speed * Time.fixedDeltaTime);

        animator.SetFloat("Speed", input.magnitude);

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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider == null) return;
        if (collider.CompareTag("Collectable"))
            OnAttack(collider);
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

    void OnAttack(Collider2D collider)
    {
        var pushVec = RDUtil.InputToOctaVector(input) * -1; // transform.position - collider.transform.position;

        rigid.velocity = Vector2.zero;
        rigid.AddForce(pushVec * pushPower, ForceMode2D.Impulse);

        attackEnd = DateTime.Now.AddSeconds(attackDelay);
        animator.SetBool("AttackEnd", false);
        animator.Play("Attack");
    }
}
