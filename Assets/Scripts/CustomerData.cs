using UnityEngine;

public enum DrinkType { None, Coffee, Matcha, Strawberry, Blueberry }
public enum FoodType { None, ChocolateDonut, MatchaDonut, VanillaDonut, StrawberryDonut, LemonCake, Cupcake }
public enum QuirkType { None, FastTip, ChangesOrder, StrictNoMistakes }

[CreateAssetMenu(fileName = "NewCustomer", menuName = "CoffeeJam/Customer")]
public class CustomerData : ScriptableObject
{
    [Header("Identity")]
    public string customerName;
    public GameObject modelPrefab;

    [Header("Order (ground truth)")]
    public DrinkType requiredDrink;
    public FoodType requiredFood;

    [Header("Display")]
    [TextArea] public string orderText;
    public bool isVagueOrder;
    public Sprite hintIcon;

    [Header("Balance")]
    public float patienceSeconds = 20f;
    public QuirkType quirk;
}