using UnityEngine;

public class BasketDetector : MonoBehaviour
{
    [Header("Scoring")]
    public int pointValue = 2;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip scoreSound;
    
    private static int totalScore = 0;
    
    public static int TotalScore => totalScore;
    
    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged") && other.GetComponent<Rigidbody>() != null)
        {
            var rb = other.GetComponent<Rigidbody>();
            
            if (rb.linearVelocity.y < 0)
            {
                ScoreBasket();
                Debug.Log($"BASKET! +{pointValue} points. Total Score: {totalScore}");
            }
        }
    }
    
    private void ScoreBasket()
    {
        totalScore += pointValue;
        
        if (audioSource != null && scoreSound != null)
        {
            audioSource.PlayOneShot(scoreSound);
        }
        
        BasketballGameManager gameManager = FindObjectOfType<BasketballGameManager>();
        if (gameManager != null)
        {
            // Future enhancement: Add celebration effects or sounds here
        }
    }
    
    public static void ResetScore()
    {
        totalScore = 0;
    }
}