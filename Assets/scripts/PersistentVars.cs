using UnityEngine;

public class PersistentVars : MonoBehaviour {

    private static PersistentVars _instance;

    // Time limits
    public float taskTimeQP;
    public float taskTimeRT;

    public float taskTime;
    public float oobLimit;

    // Aircraft stats
    public float diveRate;
    public float climbRate;
    public float yawRate;
    public float rollRate;

    // Reticle drift rate
    public float drift;

    void Awake()
    {
        // If this hasn't been created yet (no _instance), create _instance
        if (!_instance)
        {
            _instance = this;
        }
        // Else if it does exist already, destroy this version
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
