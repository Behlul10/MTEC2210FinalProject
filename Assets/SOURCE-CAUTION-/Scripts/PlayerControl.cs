using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject bodyObject; // Reference to the object with Body control script
    public GameObject tailTipObject; // Reference to the object with TailTip control script
    private HingeJoint2D tailTipHingeJoint; // Reference to the Hinge Joint 2D component on TailTip object

    private int SpecialCoinCount = 0;

    public float speed = 200;
    private float xMove;
    private float yMove;
    bool quit = false;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI specialCoinText;


    public AudioClip clip;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyObject = GameObject.Find("Body");
        tailTipObject = GameObject.Find("TailTip");
        tailTipHingeJoint = tailTipObject.GetComponent<HingeJoint2D>();

        // Ensure TextMeshPro components are assigned
        if (coinText == null)
        {
            coinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
            Debug.Log("coinText assigned via GameObject.Find");
        }
        else
        {
            Debug.Log("coinText assigned via Inspector");
        }

        if (specialCoinText == null)
        {
            specialCoinText = GameObject.Find("SpecialCoinText").GetComponent<TextMeshProUGUI>();
            Debug.Log("specialCoinText assigned via GameObject.Find");
        }
        else
        {
            Debug.Log("specialCoinText assigned via Inspector");
        }

        // Ensure the text fields are updated initially
        UpdateCoinText();
        UpdateSpecialCoinText();

        // Subscribe to the coin and special coin count changed events
        CoinManager.Instance.OnCoinCountChanged += UpdateCoinText;
        CoinManager.Instance.OnSpecialCoinCountChanged += UpdateSpecialCoinText;


        //audio
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled) return; // Check if this script is enabled

        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");

        // Quit function
        quit = Input.GetKey(KeyCode.Escape);
        if (quit)
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        if (!enabled) return; // Check if this script is enabled

        rb.velocity = new Vector2(xMove * speed * Time.deltaTime, speed * yMove * Time.deltaTime);

  

        if (Input.GetKey(KeyCode.Space))
        {
            if (CoinManager.Instance.SpecialCoinCount == 3)
            {
                speed = 850;
            } else if (SpecialCoinCount < 3) { speed = 550; }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            //play audio when detecting coin
            audioSource.PlayOneShot(clip);

            Debug.Log("Coin collected by: " + gameObject.name); // Log which object collected the coin
            Debug.Log("speed when coin: " + speed);

            Coin coin = other.gameObject.GetComponent<Coin>();
            if (coin != null && !coin.isCollected)
            {
                coin.isCollected = true;
                StartCoroutine(CollectCoin(other.gameObject));
            }
        }

        if (other.gameObject.CompareTag("SpecialCoin"))
        {
            audioSource.PlayOneShot(clip);

            SpecialCoinCount++;
            Debug.Log("Special coin collected by: " + gameObject.name); // Log which object collected the special coin
            Debug.Log("speed when special coin: " + speed);
            SpecialCoin specialCoin = other.gameObject.GetComponent<SpecialCoin>();
            if (specialCoin != null && !specialCoin.isCollected)
            {
                specialCoin.isCollected = true;
                StartCoroutine(CollectSpecialCoin(other.gameObject));
            }
        }
    }

    private IEnumerator CollectCoin(GameObject coin)
    {
        Debug.Log("CollectCoin coroutine started for: " + coin.name);
        CoinManager.Instance.AddCoin();
        yield return null; // Wait for the end of the frame
        Destroy(coin);
    }

    private IEnumerator CollectSpecialCoin(GameObject specialCoin)
    {
        Debug.Log("CollectSpecialCoin coroutine started for: " + specialCoin.name);
        CoinManager.Instance.AddSpecialCoin();
        yield return null; // Wait for the end of the frame
        Destroy(specialCoin);
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + CoinManager.Instance.CoinCount;
            Debug.Log("Coin text updated to: " + CoinManager.Instance.CoinCount);
        }
        else
        {
            Debug.LogWarning("coinText is not assigned!");
        }
    }

    private void UpdateSpecialCoinText()
    {
        if (specialCoinText != null)
        {
            specialCoinText.text = "Special Coins: " + CoinManager.Instance.SpecialCoinCount;
            Debug.Log("Special coin text updated to: " + CoinManager.Instance.SpecialCoinCount);
        }
        else
        {
            Debug.LogWarning("specialCoinText is not assigned!");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the events when the object is destroyed
        CoinManager.Instance.OnCoinCountChanged -= UpdateCoinText;
        CoinManager.Instance.OnSpecialCoinCountChanged -= UpdateSpecialCoinText;
    }
}
