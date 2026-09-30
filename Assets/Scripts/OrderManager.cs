using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

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
        madeDrink = DrinkType.None;
        madeFood = FoodType.None;
        onNewCustomer?.Invoke(customer);
    }

    public void GrabCup()
    {
        if (currentCustomer == null) return;
        hasCup = true;
    }

    public void MakeDrink(DrinkType drink)
    {
        if (currentCustomer == null || !hasCup) return;
        madeDrink = drink;
    }

    public void PickFood(FoodType food)
    {
        if (currentCustomer == null) return;
        madeFood = food;
    }

    public void Serve()
    {
        if (currentCustomer == null) return;

        bool drinkOk = currentCustomer.requiredDrink == DrinkType.None || currentCustomer.requiredDrink == madeDrink;
        bool foodOk = currentCustomer.requiredFood == FoodType.None || currentCustomer.requiredFood == madeFood;

        if (drinkOk && foodOk)
        {
            float elapsed = CustomerSpawner.Instance.ElapsedOnCurrent();
            bool wasFast = elapsed < currentCustomer.patienceSeconds * 0.5f;
            ScoreManager.Instance.CorrectOrder(currentCustomer.isVagueOrder, wasFast);
        }
        else
        {
            ScoreManager.Instance.WrongItem();
        }

        ClearOrderAndAdvance();
    }

    public void ClearOrderAndAdvance()
    {
        currentCustomer = null;
        onOrderCleared?.Invoke();
        CustomerSpawner.Instance.NextCustomer();
    }
}