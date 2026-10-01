using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Order UI")]
    public GameObject orderPanel;
    public TMP_Text orderText;

    public GameObject checklistPanel;
    public TMP_Text checklistText;

    [Header("Game UI")]
    public TMP_Text scoreText;

    public GameObject feedbackPanel;
    public TMP_Text feedbackText;

    public GameObject patiencePanel;
    public Slider patienceBar;

    [Header("Buttons")]
    public Button serveButton;
    public Button trashButton;

    float feedbackTimer;

    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.onScoreChanged.AddListener(UpdateScore);
            ScoreManager.Instance.onFeedback.AddListener(ShowFeedback);

            UpdateScore(ScoreManager.Instance.score);
        }

        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.onNewCustomer.AddListener(UpdateOrderText);
            OrderManager.Instance.onOrderCleared.AddListener(ClearOrderText);
        }

        if (CustomerSpawner.Instance != null)
        {
            CustomerSpawner.Instance.onPatienceTick.AddListener(UpdatePatience);
        }

        // Gameplay UI starts disabled.
        if (orderPanel != null)
            orderPanel.SetActive(false);

        if (checklistPanel != null)
            checklistPanel.SetActive(false);

        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

        if (patiencePanel != null)
            patiencePanel.SetActive(false);

        if (serveButton != null)
            serveButton.gameObject.SetActive(false);

        if (trashButton != null)
            trashButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (OrderManager.Instance != null &&
            OrderManager.Instance.currentCustomer != null)
        {
            checklistText.text =
                $"Cup: {(OrderManager.Instance.hasCup ? "Yes" : "No")}\n" +
                $"Drink: {FormatDrink(OrderManager.Instance.madeDrink)}\n" +
                $"Food: {FormatFood(OrderManager.Instance.madeFood)}";

            if (checklistPanel != null)
                checklistPanel.SetActive(true);

            // Serve is visible while there is a customer.
            serveButton.gameObject.SetActive(true);

            // Serve only becomes clickable after starting the order.
            serveButton.interactable =
                OrderManager.Instance.HasStartedOrder;

            // Trash only appears after starting the order.
            trashButton.gameObject.SetActive(
                OrderManager.Instance.HasStartedOrder
            );
        }

        if (feedbackTimer > 0)
        {
            feedbackTimer -= Time.deltaTime;

            if (feedbackTimer <= 0)
            {
                feedbackText.text = "";

                if (feedbackPanel != null)
                    feedbackPanel.SetActive(false);
            }
        }
    }

    void UpdateOrderText(CustomerData c)
    {
        orderText.text = $"{c.customerName}: {c.orderText}";

        if (orderPanel != null)
            orderPanel.SetActive(true);

        if (checklistPanel != null)
            checklistPanel.SetActive(true);

        if (patiencePanel != null)
            patiencePanel.SetActive(true);

        // New customer:
        // Serve is visible but disabled until the player starts the order.
        serveButton.gameObject.SetActive(true);
        serveButton.interactable = false;

        trashButton.gameObject.SetActive(false);

        // Reset patience display.
        patienceBar.value = 1f;
    }

    void ClearOrderText()
    {
        orderText.text = "";
        checklistText.text = "";

        if (orderPanel != null)
            orderPanel.SetActive(false);

        if (checklistPanel != null)
            checklistPanel.SetActive(false);

        if (patiencePanel != null)
            patiencePanel.SetActive(false);

        serveButton.gameObject.SetActive(false);
        trashButton.gameObject.SetActive(false);

        patienceBar.value = 0f;
    }

    void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    void ShowFeedback(string msg)
    {
        feedbackText.text = msg;
        feedbackTimer = 1.5f;

        if (feedbackPanel != null)
            feedbackPanel.SetActive(true);
    }

    void UpdatePatience(float remaining, float total)
    {
        patienceBar.value =
            total > 0 ? remaining / total : 0f;
    }

    string FormatDrink(DrinkType drink)
    {
        switch (drink)
        {
            case DrinkType.Coffee:
                return "Coffee";

            case DrinkType.Matcha:
                return "Matcha";

            case DrinkType.Strawberry:
                return "Strawberry";

            case DrinkType.Blueberry:
                return "Blueberry";

            default:
                return "None";
        }
    }

    string FormatFood(FoodType food)
    {
        switch (food)
        {
            case FoodType.ChocolateDonut:
                return "Chocolate Donut";

            case FoodType.MatchaDonut:
                return "Matcha Donut";

            case FoodType.VanillaDonut:
                return "Vanilla Donut";

            case FoodType.StrawberryDonut:
                return "Strawberry Donut";

            case FoodType.LemonCake:
                return "Lemon Cake";

            case FoodType.Cupcake:
                return "Cupcake";

            default:
                return "None";
        }
    }

    void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.onScoreChanged.RemoveListener(UpdateScore);
            ScoreManager.Instance.onFeedback.RemoveListener(ShowFeedback);
        }

        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.onNewCustomer.RemoveListener(UpdateOrderText);
            OrderManager.Instance.onOrderCleared.RemoveListener(ClearOrderText);
        }

        if (CustomerSpawner.Instance != null)
        {
            CustomerSpawner.Instance.onPatienceTick.RemoveListener(UpdatePatience);
        }
    }
}