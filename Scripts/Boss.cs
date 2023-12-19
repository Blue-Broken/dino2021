using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    private GameManager gameM;
    public SpriteRenderer blackCover;

    [Header("Specifics Variables")]
    public Transform groudCheckPos;
    public float groundChckRadius;
    public LayerMask Ground;
    public float jumpForce;
    public float jumpDistance;
    public float jumpDelay;
    private float distance = 20;
    public float rotateSpeed;
    private bool rotating;
    private Collider[] checkGround;
    private Vector3 direction;
    public int headShotMultiply;

    protected override void Start()
    {
        base.Start();
        gameM = GameManager.gameManager;
        checkGround = Physics.OverlapSphere(groudCheckPos.position, groundChckRadius, Ground);
        StartCoroutine(JumpDelay());
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(playerDistance > distance)
        {
            Debug.DrawLine(transform.position, playerTransform.position, Color.red);
        }
        else
        {
            Debug.DrawLine(transform.position, playerTransform.position, Color.green);
        }

        checkGround = Physics.OverlapSphere(groudCheckPos.position, groundChckRadius, Ground);

        enemyAnim.SetBool("Rotating", rotating);
        enemyAnim.SetFloat("VelocityY", enemyBody.velocity.y);
        enemyAnim.SetBool("Grounded", checkGround.Length > 0);
    }

    private void Update()
    {
        
    }

    private void Jump()
    {
        Vector3 direcao = (playerTransform.position - transform.position);
        direcao.Normalize();
        
        enemyBody.AddForce(new Vector3(direcao.x / (playerDistance > (distance / 2) ? 0.85f: 2), jumpForce * Mathf.Sqrt(Mathf.Pow(direcao.x,2) + Mathf.Pow(direcao.z, 2)), direcao.z / (playerDistance > (distance / 2) ? 0.85f : 2)), ForceMode.Impulse);
    }

    private void Rotate()
    {
        direction = playerTransform.position - transform.position;
        direction.Normalize();
        Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Quaternion.LookRotation(direction).eulerAngles.y, 0), rotateSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }

    public override void DamangeCicle(int damange, Collider hitArea)
    {
        if (hitArea != headCollider)
        {
            life -= damange;
        }
        else
        {
            life -= damange * headShotMultiply;
        }

        if (life <= 0 && !Died)
        {
            Died = true;
            //StopAllCoroutines();
            //StopCoroutine(JumpDelay());
            //StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        enemyAnim.SetTrigger("Death");
        
        Vector4 colorAlpha = Vector4.zero;

        for(int i = 0; i < 10; i++)
        {
            colorAlpha += new Vector4(0,0,0,0.1f);
            blackCover.color = colorAlpha;
            yield return new WaitForSeconds(0.2f);
        }

        blackCover.color = Color.black;
        
        gameM.LoadNextStage();
    }

    IEnumerator JumpDelay()
    {
        float time = 5f;

        direction = playerTransform.position - transform.position;
        direction.Normalize();
        float angle = Quaternion.LookRotation(direction).eulerAngles.y;

        rotating = true;

        while (time > 0 && Mathf.Abs(Mathf.Round(transform.eulerAngles.y) - Mathf.Round(angle)) > 3f)
        {
            time -= Time.deltaTime;
            Rotate();
            direction = playerTransform.position - transform.position;
            direction.Normalize();
            angle = Quaternion.LookRotation(direction).eulerAngles.y;
            yield return null;
        }

        rotating = false;

        yield return new WaitForSeconds(0.5f);

        enemyBody.velocity = Vector3.zero;
        enemyBody.angularVelocity = Vector3.zero;

        enemyAnim.SetTrigger("Jump");

        yield return new WaitForSeconds(0.4f);
        
        Jump();

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => checkGround.Length > 0);
        
        enemyBody.velocity = Vector3.zero;
        enemyBody.angularVelocity = Vector3.zero;
        CameraControl.SetShake(0.5f, 0.5f);

    //    yield return new WaitForSeconds(jumpDelay);

        if(!Died)
        {
            StartCoroutine(JumpDelay());
        } 
        else
        {
            StartCoroutine(Death());
        }
    }
}
