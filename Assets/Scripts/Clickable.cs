using UnityEngine;

public enum ClickableKind { Cup, EspressoMachine, Dessert }

public class Clickable : MonoBehaviour
{
    public ClickableKind kind;
    public DrinkType drinkMade = DrinkType.None;
    public FoodType foodItem = FoodType.None;

    public void OnClicked()
    {
        Debug.Log($"Clicked: {gameObject.name}, Kind={kind}");
        switch (kind)
        {
            case ClickableKind.Cup:
                OrderManager.Instance.GrabCup(drinkMade);
                break;

            case ClickableKind.EspressoMachine:
                OrderManager.Instance.MakeDrink();
                break;

            case ClickableKind.Dessert:
                OrderManager.Instance.PickFood(foodItem);
                break;
        }
    }
}