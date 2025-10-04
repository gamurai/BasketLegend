using UnityEngine;

public class GameAutoSetup : MonoBehaviour
{
    [Header("Setup Settings")]
    public bool setupOnStart = true;
    
    private void Start()
    {
        if (setupOnStart)
        {
            SetupGame();
        }
    }
    
    public void SetupGame()
    {
        SetupBasketballGameManager();
        SetupBasketDetectors();
        Debug.Log("Basketball game setup complete! Swipe to shoot balls at the basket.");
    }
    
    private void SetupBasketballGameManager()
    {
        if (FindObjectOfType<BasketballGameManager>() == null)
        {
            var gameManagerObject = new GameObject("BasketballGameManager");
            gameManagerObject.AddComponent<BasketballGameManager>();
            Debug.Log("BasketballGameManager created and configured.");
        }
    }
    
    private void SetupBasketDetectors()
    {
        GameObject[] rings = GameObject.FindGameObjectsWithTag("Untagged");
        
        foreach (GameObject obj in rings)
        {
            if (obj.name.ToLower().Contains("ring"))
            {
                SetupBasketRing(obj);
            }
        }
    }
    
    private void SetupBasketRing(GameObject ring)
    {
        var existingDetector = ring.GetComponent<BasketDetector>();
        if (existingDetector == null)
        {
            ring.AddComponent<BasketDetector>();
        }
        
        var collider = ring.GetComponent<Collider>();
        if (collider == null)
        {
            var cylinderCollider = ring.AddComponent<CapsuleCollider>();
            cylinderCollider.isTrigger = true;
            cylinderCollider.radius = 0.5f;
            cylinderCollider.height = 0.2f;
        }
        else
        {
            collider.isTrigger = true;
        }
        
        Debug.Log($"Basketball detector setup on {ring.name}");
    }
}