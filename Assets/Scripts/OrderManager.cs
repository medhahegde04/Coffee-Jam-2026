using UnityEngine;
using UnityEngine.Events;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [Header("Spawn point")]
    public Transform customerSpawnPoint;
    GameObject currentModelInstance;

    [Header("Current state")]
    public CustomerData currentCustomer;
    public bool hasCup = false;
    public DrinkType madeDrink = DrinkType.None;
    public FoodType madeFood = FoodType.None;

    public UnityEvent<CustomerData> onNewCustomer;
    public UnityEvent onOrderCleared;

    void Awake() => Instance = this;

    public void SetCustomer(CustomerData customer)
    {
        currentCustomer = customer;

        hasCup = false;
        heldDrinkType = DrinkType.None;
        madeDrink = DrinkType.None;
        madeFood = FoodType.None;

        if (currentModelInstance != null)
            Destroy(currentModelInstance);

        if (customer.modelPrefab != null && customerSpawnPoint != null)
        {
            currentModelInstance = Instantiate(
                customer.modelPrefab,
                customerSpawnPoint.position,
                customerSpawnPoint.rotation
            );
        }

        Debug.Log(
            $"New customer: {customer.customerName}, " +
            $"wants Drink={customer.requiredDrink}, " +
            $"Food={customer.requiredFood}"
        );

        onNewCustomer?.Invoke(customer);
    }

    public bool HasStartedOrder =>
        hasCup ||
        madeDrink != DrinkType.None ||
        madeFood != FoodType.None;

    public DrinkType heldDrinkType = DrinkType.None;

    public void GrabCup(DrinkType drink)
    {
        if (currentCustomer == null)
            return;

        hasCup = true;
        heldDrinkType = drink;

        Debug.Log($"Cup grabbed: {drink}");
    }

    public void MakeDrink()
    {
        if (currentCustomer == null)
            return;

        if (!hasCup)
        {
            Debug.Log("Tried to use machine without a cup first");
            return;
        }

        madeDrink = heldDrinkType;

        Debug.Log($"Made drink: {madeDrink}");
    }

    public void PickFood(FoodType food)
    {
        if (currentCustomer == null)
            return;

        madeFood = food;

        Debug.Log($"Picked food: {food}");
    }

    public void Serve()
    {
        if (currentCustomer == null)
        {
            Debug.Log("Serve pressed but no current customer");
            return;
        }

        bool drinkOk =
            currentCustomer.requiredDrink == DrinkType.None ||
            currentCustomer.requiredDrink == madeDrink;

        bool foodOk =
            currentCustomer.requiredFood == FoodType.None ||
            currentCustomer.requiredFood == madeFood;

        if (drinkOk && foodOk)
        {
            float elapsed =
                CustomerSpawner.Instance != null
                    ? CustomerSpawner.Instance.ElapsedOnCurrent()
                    : 0f;

            bool wasFast =
                elapsed < currentCustomer.patienceSeconds * 0.5f;

            ScoreManager.Instance.CorrectOrder(
                currentCustomer.isVagueOrder,
                wasFast
            );
        }
        else
        {
            ScoreManager.Instance.WrongItem();
        }

        ClearOrderAndAdvance();
    }

    public void ClearOrderAndAdvance()
    {
        if (currentModelInstance != null)
            Destroy(currentModelInstance);

        currentCustomer = null;

        onOrderCleared?.Invoke();

        if (CustomerSpawner.Instance != null)
            CustomerSpawner.Instance.NextCustomer();
    }

    public void ResetProgress()
    {
        hasCup = false;
        heldDrinkType = DrinkType.None;
        madeDrink = DrinkType.None;
        madeFood = FoodType.None;

        Debug.Log("Progress reset");
    }
}