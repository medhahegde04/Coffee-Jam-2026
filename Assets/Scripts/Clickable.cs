using UnityEngine;
using UnityEngine.EventSystems;

public enum ClickableKind { Cup, EspressoMachine, Dessert }

public class Clickable : MonoBehaviour
{
    public ClickableKind kind;
    public DrinkType drinkMade = DrinkType.None;
    public FoodType foodItem = FoodType.None;

    public void OnClicked()
    {
        switch (kind)
        {
            case ClickableKind.Cup:
                OrderManager.Instance.GrabCup();
                break;

            case ClickableKind.EspressoMachine:
                OrderManager.Instance.MakeDrink(drinkMade);
                break;

            case ClickableKind.Dessert:
                OrderManager.Instance.PickFood(foodItem);
                break;
        }
    }
}