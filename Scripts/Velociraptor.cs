using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velociraptor : EnemyBase
{
    [Header("Especific Variables")]
    public float rotationSpeed;
    public float speed;
    public float atackDistance;
    private Vector3 direction;
    private bool atacking;
    public float atackTime;
    public int criticalDamangeMultiply;
    public int atackMultiplyDamange;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (actived && !Died && !atacking)
        {
            #region Rotation
        
            direction = playerTransform.position - transform.position;
            direction.Normalize();
            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Quaternion.LookRotation(direction).eulerAngles.y, 0), rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = rotation;
            
            #endregion

            #region Moviment
            enemyBody.MovePosition(enemyBody.position + transform.forward * speed * Time.fixedDeltaTime);
            #endregion

            if(atackDistance  >= playerDistance)
            {
                Atack();
            }
        }

        if (Died)
        {
            Quaternion deadRotationAjust = Quaternion.Euler(0,transform.eulerAngles.y,0);
            transform.rotation = deadRotationAjust;
        }
    }

    public override void DamangeCicle(int damange, Collider hitArea)
    {
        if (hitArea != headCollider)
        {
            life -= damange;
        }
        else
        {
            life -= damange * criticalDamangeMultiply;
        }

        if (life <= 0)
        {
            //Destroy(gameObject);
            enemyAnim.SetBool("Atack", false);
            enemyAnim.SetBool("Dead", true);
            Died = true;
            enemyBody.velocity = Vector3.zero;
        }
    }
    public void Atack()
    {
        atacking = true;
        enemyAnim.SetBool("Atack", true);
        StartCoroutine(AtackCicle());
    }

    IEnumerator AtackCicle()
    {
        int damangeBefore = damange;
        damange = damange * atackMultiplyDamange;
        
        yield return new WaitForSeconds(atackTime);

        damange = damangeBefore;
        atacking = false;
        enemyAnim.SetBool("Atack", false);
    }
}
