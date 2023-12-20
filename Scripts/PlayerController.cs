using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject pauseScreen;
    private GameObject mainCam;
    private GameManager gameM;
    private Rigidbody body;
    private CharacterController controller;
    private AudioSource audioS;
    public SkinnedMeshRenderer playerRender;
    private bool isLoading;

    public Transform[] checkPointsPositions;

    [Header("Variables of Moviment")]
    public float speed;
    public float aimedSpeed;
    public float runSpeed;
    private Vector3 direction;

    [Header("Variables of Camera Rotation")]
    [SerializeField] private float sensibilityX = 50f;
    private float rotationX;
    private bool stoped;

    [Header("Variables of Animation")]
    public Animator animator;
    public GameObject spine;
    public AudioSource gfxaudio;

    [Header("Variables of Interacion")]
    public Transform[] colliderPivots = new Transform[2];
    public float radius = 2f;
    public LayerMask interactionObject;
    public Text ReadText;
    public Text PointerText;
    private TextProgressive ReadTextScript;
    private bool reading;
    private AudioSource audioTriggerS;
    private GameObject audioTriggerObj;
    public Text audioTriggerLegend;

    [Header("Variables of Raycast")]
    public SpriteRenderer pointerSprite;
    public float range;
    public LayerMask targets;
    public bool isInteracting;
    private RaycastHit hit;

    [Header("Variables of Shot")]
    public AudioClip shotSound;
    public static bool armed;
    public GameObject gun;
    public GameObject bloodEfect;
    public float fireRate;
    private float nextFire;
    public int munitionCartridgeSize = 10;
    public static int currentCartridges;
    public static int currentMunition = 10;
    private EnemyBase currentEnemyScript;
    private bool rec;
    public float rechageDelay;
    private bool atacking;
    public float shortAtackDelay;

    [Header("Variables of Health")]
    public int life;
    public int maxLife;
    public int healthPackSize = 10;
    public static int currentHealthPacks;
    public GameObject lowLifeScreen;

    [Header("Variables of damange")]
    public bool tookingDamange;
    private int touchingEnemies;
    public float damangeDelay;
    private int currentDamange;

    [Header("Variables of Death")]
    public bool Died;
    public SpriteRenderer blackCover;
    
    public GameObject ammo;
    public Text ammoValue;

    private void Awake()
    {
        gameM = GameManager.gameManager;

        if (checkPointsPositions.Length > 0 && gameM.currentCheckPoint >= 1)
        {
            transform.position = checkPointsPositions[gameM.currentCheckPoint - 1].position;
        }

        armed = gameM.armed;
        currentMunition = gameM.currentMunition;
        currentHealthPacks = gameM.currentHealthPacks;
        currentCartridges = gameM.currentCartrigdes;
    }

    private void Start()
    {
        gameM = GameManager.gameManager;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        body = GetComponent<Rigidbody>();
        audioS = GetComponent<AudioSource>();
        ReadTextScript = ReadText.GetComponent<TextProgressive>();
    }

    private void Update()
    {
        ammoValue.text = currentMunition;

        if(!Died && !isLoading)
        {
            if (!gameM.paused)
            {
                direction = transform.TransformVector(new Vector3(0, 0, Input.GetAxisRaw("Vertical"))).normalized;

                if (Input.GetButton("Aim") && armed)
                {
                    if(Input.GetButtonDown("Rec") && currentCartridges > 0)
                    {
                        StartCoroutine(RechargeLoop());
                    }
                    
                    if(!rec)
                        Shot();
                }
            }
            
            //Interaction();

            if (reading)
            {
                TextRead(null);
            }

            Pause();
        }

        if(Input.GetButtonDown("Action") && !atacking)
        {
            atacking = true;
            StartCoroutine(ShortAtack());
        }
        //Test Area
        if (Input.GetButtonDown("Cure"))
        {
            Recover();
        }

        if(audioTriggerS != null)
        {
            if (audioTriggerS.isPlaying)
            {
                //audioTriggerLegend.text = audioTriggerObj.name;
                //audioTriggerLegend.enabled = true;

                if (Input.GetButtonDown("Action"))
                {
                    //audioTriggerLegend.enabled = false;
                    audioTriggerS.Stop();
                }
            }
            else
            {
                //audioTriggerLegend.enabled = false;
                audioTriggerS.Stop();
                audioTriggerS = null;
                Destroy(audioTriggerObj);
                audioTriggerObj = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if(!Died && !isLoading)
        {
            #region Moviment
            if (!Input.GetButton("Aim") || !armed)
            {

                ammo.SetActive(true);
                if (direction.magnitude != 0 && !reading)
                {
                    body.MovePosition(body.position + direction * speed * Time.fixedDeltaTime);
                    /*Vector3 input = transform.forward * Input.GetAxisRaw("Vertocal");
                    controller.Move(input * speed * Time.fixedDeltaTime);*/
                }
                else
                {
                    body.velocity = Vector3.zero;
                    body.angularVelocity = Vector3.zero;
                }

                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    rotationX = Mathf.Lerp(rotationX, Input.GetAxisRaw("Horizontal") * 2, sensibilityX * Time.fixedDeltaTime);
                }
                else
                {
                    rotationX = 0;
                }

                if (stoped)
                {
                    stoped = false;
                   
                }

                if(life < maxLife / 3)
                {
                    lowLifeScreen.SetActive(true);
                }
                else
                {
                    lowLifeScreen.SetActive(false);
                }
            }
            else
            {
                ammo.SetActive(false);
                if (!stoped)
                {
                    stoped = true;
                    rotationX = 0;
                }

                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;

                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    rotationX = Mathf.Lerp(rotationX, Input.GetAxisRaw("Horizontal") * 2, sensibilityX * Time.fixedDeltaTime);
                }
                else
                {
                    rotationX = 0;
                }
            }
         
            transform.Rotate(0, rotationX, 0, Space.World);
            #endregion


            gun.SetActive(armed);

            RayCastController();

            DamangeCheck();
        }
        
        #region Animations
        animator.SetFloat("Velocity", Mathf.Abs(Input.GetAxisRaw("Vertical")));

        animator.SetBool("Aim", (Input.GetButton("Aim") && armed));
        #endregion
    }

    IEnumerator ShortAtack()
    {
        anim.SetTrigger("ShortAtack");
        yield return new WaitForSeconds(shortAtackDelay());
        atacking = false;
    } 

    void Pause()
    {
        if (Input.GetButtonDown("Submit") && !reading)
        {
            if (gameM.paused)
            {
                pauseScreen.SetActive(false);
                gameM.paused = false;
                Time.timeScale = 1f;
            }
            else
            {
                if(gfxaudio)
                {
                  gfxaudio.Stop();
                }              
                pauseScreen.SetActive(true);
                gameM.paused = true;
                Time.timeScale = 0f;
            }
        }
    }

    void Interaction()
    {
        Collider[] collision = Physics.OverlapCapsule(colliderPivots[0].position, colliderPivots[1].position, radius, interactionObject);

        if(collision.Length > 0)
        {
            if (collision[0].CompareTag("Munition"))
            {
                GetMunition(collision[0]);
                PointerText.color = Color.yellow;
                PointerText.text = "PEGAR";

            }
            else if (collision[0].CompareTag("HealthRecover"))
            {
                GetHealthPack(collision[0]);
                PointerText.color = Color.green;
                PointerText.text = "PEGAR";
            } 
            else if (collision[0].CompareTag("ReadObject"))
            {
                TextRead(collision[0]);
                PointerText.color = Color.blue;
                PointerText.text = "OLHAR";
            }
        }
    }

    void RayCastController()
    {
        Ray rayMouse = Camera.main.ScreenPointToRay(Pointer.mousePos); 
        
        if(Physics.Raycast(rayMouse, out hit, range, targets))
        {
            #region SetPointerColor
            if (hit.collider.CompareTag("ReadObject"))
            {
                pointerSprite.color = Color.blue;
            } 
            else if (hit.collider.CompareTag("HealthRecover")) 
            {
                pointerSprite.color = Color.green;
            } 
            else if (hit.collider.CompareTag("Munition"))
            {
                pointerSprite.color = Color.yellow;
            } 
            else if (hit.collider.CompareTag("Enemy") && Input.GetButton("Aim"))
            {
                try
                {
                    currentEnemyScript = hit.collider.transform.parent.gameObject.GetComponent<EnemyBase>();
                }
                catch (System.Exception)
                {
                    Debug.LogWarning("O Collider com Layer Enemy e tag Enemy deve ser colocado em um objeto dentro do inimigo, não no proprio inimigo!");
                }

                if (!currentEnemyScript.Died)
                {
                    pointerSprite.color = Color.red;
                }
                else
                {
                    pointerSprite.color = Color.white;
                }
            }
            else
            {
                currentEnemyScript = null;
                pointerSprite.color = Color.white;
            }
            #endregion

            #region Interaction
            if (isInteracting)
            {
                if (hit.collider.CompareTag("ReadObject"))
                {
                    TextRead(hit.collider);
                    PointerText.text = "OLHAR";
                    PointerText.enabled = true;
                }
                else if (hit.collider.CompareTag("HealthRecover"))
                {
                    GetHealthPack(hit.collider);
                    PointerText.text = "PEGAR";
                    PointerText.enabled = true;
                }
                else if (hit.collider.CompareTag("Munition"))
                {
                    GetMunition(hit.collider);
                    PointerText.text = "PEGAR";
                    PointerText.enabled = true;
                }
                else
                {
                    PointerText.enabled = false;
                }
            }
            #endregion
        }
        else
        {
            currentEnemyScript = null;
            PointerText.enabled = false;
            pointerSprite.color = Color.white;
        }
    }

    void Shot()
    {
        if(Input.GetButtonDown("Action") && Time.time > nextFire && currentMunition > 0 && !tookingDamange)
        {
            if (shotSound)
            {
                audioS.clip = shotSound;
                audioS.PlayOneShot(audioS.clip);
            }

            animator.SetTrigger("Shoot");
            nextFire = Time.time + fireRate;
            currentMunition--;

            if (currentEnemyScript != null)
            {
                if(bloodEfect)
                {
                  Quaternion lookCamera = Quaternion.LookRotation(Camera.main.transform.position, hit.point);
                  Instantiate(bloodEfect, hit.point, lookCamera);
                }          
                currentEnemyScript.DamangeCicle(1, hit.collider);
            }
        }
    }

    void DamangeCheck()
    {
        if(touchingEnemies > 0 && !tookingDamange)
        {
            tookingDamange = true;
            StartCoroutine(DamangeEfetc());
        }
    }

    void GetMunition(Collider collision)
    {
        if (Input.GetButtonDown("Action"))
        {
            currentCartridges++;
            Destroy(collision.gameObject);
        }
    }

    void GetHealthPack(Collider collision)
    {
        if (Input.GetButtonDown("Action"))
        {
            currentHealthPacks++;
            Destroy(collision.gameObject);
        }
    }

    void TextRead(Collider collision)
    {

        string text = "";

        if (!reading)
        {
            for (int i = 7; i < collision.name.Length; i++)
            {
                text += collision.name[i];
            }
        }

        if ((Input.GetButtonDown("Action")) && !reading)
        {
            reading = true;
            ReadText.enabled = true; 
            gameM.paused = true;
            Time.timeScale = 0f;
            ReadTextScript.PlayText(text, collision.gameObject.GetComponent<AudioSource>().clip);
        }

        if (Input.GetButtonDown("Action") && reading)
        {
            if (ReadTextScript.canSkip)
            {
                ReadText.enabled = false;
                gameM.paused = false;
                Time.timeScale = 1f;
                reading = false;
            }
        }

    }

    void Recover()
    {
        if(currentHealthPacks > 0)
        {
            currentHealthPacks--;
            life = ((life + healthPackSize) > maxLife) ? maxLife : life + healthPackSize;
        }
    }

    IEnumerator RechargeLoop()
    {
            rec = true;
            currentCartridges--;
            currentMunition = munitionCartridgeSize;

            animator.SetTrigger("Reload");
            yield return new WaitForSeconds(rechageDelay);

            rec = false;
    }

    IEnumerator DamangeEfetc()
    {
        if (!Died)
        {
            life -= currentDamange;

            if (life <= 0)
            {
                animator.ResetTrigger("Damage");
                if(!Died)
                {
                    Died = true;
                    StartCoroutine(Death());
                }
            }
            else
            {
                animator.SetTrigger("Damage");
                yield return new WaitForSeconds(damangeDelay);

            }
        }

        tookingDamange = false;
    }

    IEnumerator Death()
    {
        Vector4 colorAlpha = Vector4.zero;
        animator.SetBool("Dead", true);

        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < 10; i++)
        {
            colorAlpha += new Vector4(0,0,0,0.1f);
            blackCover.color = colorAlpha;
            yield return new WaitForSeconds(0.1f);
        }

        blackCover.color = Color.black;

        var loading = SceneManager.LoadSceneAsync("RestartScene");

        while (!loading.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadNext()
    {
        Vector4 colorAlpha = Vector4.zero;

        for(int i = 0; i < 10; i++)
        {
            colorAlpha += new Vector4(0,0,0,0.1f);
            blackCover.color = colorAlpha;
            yield return new WaitForSeconds(0.1f);
        }

        blackCover.color = Color.black;

        gameM.LoadNextStage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.transform.parent.gameObject.GetComponent<EnemyBase>();

            if (!enemy.Died)
            {
                currentDamange = enemy.damange;
                touchingEnemies++;
            }
        }

        if (other.CompareTag("PlayCutscene"))
        {
            CutscenesControl.PlayCutsceneParameters(other);
        }

        if (other.CompareTag("AudioTrigger"))
        {
            audioTriggerS = other.gameObject.GetComponent<AudioSource>();
            audioTriggerS.PlayOneShot(audioTriggerS.clip);
            audioTriggerObj = other.gameObject;
        }

        if(other.CompareTag("LoadNextStage"))
        {
            isLoading = true;
            StartCoroutine(LoadNext());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.transform.parent.gameObject.GetComponent<EnemyBase>();

            if (!enemy.Died)
            {
                currentDamange = enemy.damange;
            }
            else
            {
                currentDamange = 0;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")  && !other.transform.parent.gameObject.GetComponent<EnemyBase>().Died)
        {
            touchingEnemies--;
        }
    }

    public void SetArmed()
    {
        armed = true;
        gameM.armed = armed;
    }
}