using UnityEngine;
using UnityEngine.Events;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;

    public CustomerData[] allCustomers;
    public float delayBetweenCustomers = 2f;
    public UnityEvent<float, float> onPatienceTick;

    float timer;
    float customerStartTime;
    bool active = false;

    void Awake() => Instance = this;

    void Start() => NextCustomer();

    void Update()
    {
        if (!active ||
            OrderManager.Instance == null ||
            OrderManager.Instance.currentCustomer == null)
            return;

        timer -= Time.deltaTime;

        onPatienceTick?.Invoke(
            Mathf.Max(0, timer),
            OrderManager.Instance.currentCustomer.patienceSeconds
        );

        if (timer <= 0f)
        {
            ScoreManager.Instance.MissedOrder();
            OrderManager.Instance.ClearOrderAndAdvance();
        }
    }

    public float ElapsedOnCurrent()
    {
        return Time.time - customerStartTime;
    }

    public void NextCustomer()
    {
        active = false;
        Invoke(nameof(SpawnRandom), delayBetweenCustomers);
    }

    void SpawnRandom()
    {
        if (allCustomers.Length == 0)
            return;

        CustomerData pick =
            allCustomers[Random.Range(0, allCustomers.Length)];

        OrderManager.Instance.SetCustomer(pick);

        timer = pick.patienceSeconds;
        customerStartTime = Time.time;
        active = true;
    }
}