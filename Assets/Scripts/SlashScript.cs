using UnityEngine;
using TMPro;

public class SlashScript : MonoBehaviour
{
    [Header("Player Stats")]
    public int score;
    public int slicesThisSlash;
    public int maxAttacks;
    public int attacksLeft;

    [Header("Player movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float airDrag;
    [SerializeField] private float slashTime;
    [SerializeField] private float slashVelocityMultiplier;
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float boostTime;
    [SerializeField] private float boostMultiplier;
    [SerializeField] private float boostDuration;

    [SerializeField] private float attackRegenDuration;

    [Header("Player Animation")]
    [SerializeField] private AudioSource sounds;
    public AudioClip[] swooshes;
    public AudioClip boost;
    public AudioClip boostReady;

    [SerializeField] private GameObject slice;
    [SerializeField] private GameObject fall;
    [SerializeField] private GameObject aiming;

    public bool isSlicing;
    public bool isFalling;
    public bool isSwiping = false;
    public bool isBoosting = false;
    public bool willBoost = false;

    private float slashTimer;
    private float boostTimer;
    private float attackRegenTimer;
    
    private Vector2 swipeStartPosition;
    private Vector2 screenCenter;

    [Header("Components")]

    [SerializeField] private LayerMask spike;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Animator scoreTextAnimator;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject boostObject;
    [SerializeField] private GameObject boostEffect;
    [SerializeField] private GameObject comboEffect;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private NinjaGameManager gameScript;
    [SerializeField] private AttackBarScript barScript;
    private Vector2 normalizedTouchPos;
    private bool isTimerExpired = false;

    private bool boostSound;

    private void Start()
    {
        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        line.positionCount = 2;
        attacksLeft = maxAttacks;
        attackRegenTimer = attackRegenDuration;
        barScript.fillTime = attackRegenDuration;
    }

    public void OnEnable()
    {
        isSlicing = false;
        isFalling = true;
        isSwiping = false;
        isBoosting = false;
        willBoost = false;

        Debug.Log("Stats Updated");
        airDrag += PlayerPrefs.GetFloat("BoostTime", 0f);
        attackRegenDuration += PlayerPrefs.GetFloat("RegenerationTime", 0f);
        moveSpeed += PlayerPrefs.GetFloat("SliceSpeed", 0f);

        barScript.fillTime = attackRegenDuration;
        attacksLeft = maxAttacks;
    }

    private void Update()
    {
        barScript.attacksLeft = attacksLeft;
        barScript.fillTimer = attackRegenTimer;

        slashTimer -= Time.deltaTime;
        if (attacksLeft < maxAttacks)
        {
            attackRegenTimer -= Time.deltaTime;
            if (attackRegenTimer <= 0f)
            {
                attacksLeft += 1;
                attackRegenTimer = attackRegenDuration;
            }
        }

        if (isSwiping)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f && attacksLeft > 1)
            {
                willBoost = true;
                if (!boostSound)
                {
                    boostSound = true;
                    sounds.clip = boostReady;
                    sounds.Play();
                }

            }
        }
        else
        {
            boostTimer = boostTime;
            willBoost = false;
        }
        scoreText.text = score.ToString("F0");

        #region Animations
        slice.SetActive(isSlicing);

        boostObject.SetActive(isBoosting);

        boostEffect.SetActive(willBoost);

        fall.SetActive(isFalling && !isSwiping);

        aiming.SetActive(isFalling && isSwiping);
        #endregion

        // Touch input handling
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            normalizedTouchPos = (touch.position - screenCenter) / screenCenter.x;

            if (touch.phase == TouchPhase.Began)
            {
                isSwiping = true;
                swipeStartPosition = normalizedTouchPos;
            }
            else if (touch.phase == TouchPhase.Ended && !willBoost && attacksLeft > 0)
            {
                // Normal Slash
                isBoosting = false;
                isSwiping = false;
                Vector2 deltaPosition = normalizedTouchPos - swipeStartPosition;
                float swipeMagnitude = deltaPosition.magnitude;
                float moveAmount = moveCurve.Evaluate(swipeMagnitude);
                if (moveAmount >= 0.1f)
                {
                    attacksLeft -= 1;
                    int randomIndex = Random.Range(0, swooshes.Length);
                    sounds.clip = swooshes[randomIndex];
                    sounds.Play();
                    Vector2 moveDirection = deltaPosition.normalized;
                    rb.velocity = moveDirection * moveSpeed * moveAmount;
                    slashTimer = slashTime;
                }
            }
            else if (touch.phase == TouchPhase.Ended && willBoost && attacksLeft > 1)
            {
                // Boost Slash
                isBoosting = true;
                isSwiping = false;
                boostSound = false;
                Vector2 deltaPosition = normalizedTouchPos - swipeStartPosition;
                float swipeMagnitude = deltaPosition.magnitude;
                float moveAmount = moveCurve.Evaluate(swipeMagnitude);
                if (moveAmount >= 0.1f)
                {
                    attacksLeft -= 2;
                    sounds.clip = boost;
                    sounds.Play();
                    Vector2 moveDirection = deltaPosition.normalized;
                    rb.velocity = moveDirection * moveSpeed * moveAmount;
                    slashTimer = slashTime + boostDuration;
                }
            }
        }
        else
        {
            // Mouse input handling
            if (Input.GetMouseButton(0))
            {
                
                normalizedTouchPos = ((Vector2)Input.mousePosition - screenCenter) / screenCenter.x;

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Swipe Started");
                    isSwiping = true;
                    swipeStartPosition = normalizedTouchPos;
                }
            }
            else if (!Input.GetMouseButton(0) && isSwiping)
            {
                if (!willBoost && attacksLeft > 0)
                {
                    // Normal Slash
                    isBoosting = false;
                    isSwiping = false;
                    Vector2 deltaPosition = normalizedTouchPos - swipeStartPosition;
                    float swipeMagnitude = deltaPosition.magnitude;
                    float moveAmount = moveCurve.Evaluate(swipeMagnitude);
                    if (moveAmount >= 0.1f)
                    {
                        attacksLeft -= 1;
                        int randomIndex = Random.Range(0, swooshes.Length);
                        sounds.clip = swooshes[randomIndex];
                        sounds.Play();
                        Vector2 moveDirection = deltaPosition.normalized;
                        rb.velocity = moveDirection * moveSpeed * moveAmount;
                        slashTimer = slashTime;
                    }
                }
                else if (willBoost && attacksLeft > 1)
                {
                    // Boost Slash
                    isBoosting = true;
                    isSwiping = false;
                    boostSound = false;
                    Vector2 deltaPosition = normalizedTouchPos - swipeStartPosition;
                    float swipeMagnitude = deltaPosition.magnitude;
                    float moveAmount = moveCurve.Evaluate(swipeMagnitude);
                    if (moveAmount >= 0.1f)
                    {
                        attacksLeft -= 2;
                        sounds.clip = boost;
                        sounds.Play();
                        Vector2 moveDirection = deltaPosition.normalized;
                        rb.velocity = moveDirection * moveSpeed * moveAmount;
                        slashTimer = slashTime + boostDuration;
                    }
                }
            }
        }

        if (isSwiping && Input.touchCount == 0 && !Input.GetMouseButton(0))
        {
            isSwiping = false;
        }
    }


    private void FixedUpdate()
    {
        if (isSwiping)
        {
            Vector2 deltaPosition = normalizedTouchPos - swipeStartPosition;
            float swipeMagnitude = deltaPosition.magnitude;
            float moveAmount = moveCurve.Evaluate(swipeMagnitude);
            Vector2 moveDirection = deltaPosition.normalized;
            Vector2 playerPosition = transform.position;
            Vector2 endPoint;
            if (!willBoost)
            {
                endPoint = playerPosition + moveDirection * (moveAmount + 0.15f) * 5f;
            }
            else
            {
                endPoint = playerPosition + moveDirection * ((moveAmount + 0.15f) * 5f) * 3f;
            }

            line.SetPosition(0, playerPosition);
            line.SetPosition(1, endPoint);
        }
        else
        {
            Vector2 playerPosition = transform.position;
            line.SetPosition(0, playerPosition);
            line.SetPosition(1, playerPosition);
        }

        Vector2 lookDirection = rb.velocity.normalized;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        playerObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        boostObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (slashTimer <= 0f)
        {
            isFalling = true;
            isSlicing = false;
            isBoosting = false;
            rb.drag = airDrag;
            rb.mass = 5f;
            rb.gravityScale = 1f;

            if (!isTimerExpired)
            {
                rb.velocity *= slashVelocityMultiplier;
                Debug.Log(slicesThisSlash);
                slicesThisSlash = 0;
                isTimerExpired = true;
            }
        }
        else
        {
            if (isBoosting)
            {
                isSlicing = true;
                isFalling = false;
                rb.drag = 0f;
                rb.mass = 0f;
                rb.gravityScale = 0f;
                isTimerExpired = false;
                Vector2 moveDirection = rb.velocity.normalized;
                rb.velocity = rb.velocity * boostMultiplier;
            }
            else
            {
                isSlicing = true;
                isFalling = false;
                rb.drag = 0f;
                rb.mass = 0f;
                rb.gravityScale = 0f;
                isTimerExpired = false; 
            }
            
        }
    }

    public void DetectCombo()
    {
        scoreTextAnimator.SetTrigger("ScoreIncrease");
        attacksLeft += 1;
        if (slicesThisSlash > 1)
        {
            GameObject newCombo = Instantiate(comboEffect, transform.position, transform.rotation);
            ComboScript comboScript = newCombo.GetComponent<ComboScript>();
            comboScript.number = slicesThisSlash;
            score += 3;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Col");
        if (other.gameObject.layer == LayerMask.NameToLayer("Spike"))
        {
            Debug.Log("Spike col");
            Instantiate(deathEffect, transform.position, transform.rotation);
            Die();
        }
    }

    public void Die()
    {  
        Debug.Log("die");
        if (score > PlayerPrefs.GetInt("highScore", 0))
        {
            PlayerPrefs.SetInt("highScore", score);
            
        }
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) + Mathf.RoundToInt(score * 2f));
        score = 0;
        gameScript.GameLose();
        
    }

    

}
