using UnityEngine;

public class Spellcast : MonoBehaviour
{
    public Rigidbody rb;
    public LayerMask Magic;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }  

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.collider.CompareTag("LandMagic"))
        {
            GameObject landMagicObject = collision.gameObject;
            LandMagicScript landMagicScript = landMagicObject.GetComponent<LandMagicScript>();

            // Check if the LandMagicScript component exists before calling Activate()
            if (landMagicScript != null)
            {
                landMagicScript.Activate();
            }
            else
            {
                Debug.LogWarning("LandMagicScript not found.");
            }
        }
        
        Destroy(gameObject);
    }
}
