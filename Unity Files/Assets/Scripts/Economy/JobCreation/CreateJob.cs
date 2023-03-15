using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateJob : MonoBehaviour
{
    public static CreateJob instance = null; // The instance of the SmartObjectManager

    public List<Slider> sliders = new List<Slider>();
    private List<Slider> _availableSliders = new List<Slider>();
    private List<Slider> _activeSliders = new List<Slider>();
    private string _taskName;
    private string _SliderText;

    // Store all task names created to make sure there are no duplicates
    private Dictionary<string, string> _taskNames = new Dictionary<string, string>();

    public bool isCreatingTask;

    public GameObject productDetailPanelPrefab;
    public Transform productDetailPanelParent;
    public static List<Product> products = new List<Product>();
    public static List<Product> completeProducts = new List<Product>();
    public static List<GameObject> productDetailPanels = new List<GameObject>();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _availableSliders = sliders;
    }

    private void Update()
    {
        if(productDetailPanels.Count > 0)
        {
            UpdateMarketedTasks();
        }
    }

    public bool CheckForAvailableSlider()
    {
        for(int i = 0; i < _availableSliders.Count; i++)
        {
            if(!_availableSliders[i].gameObject.activeInHierarchy)
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckForActiveSlider()
    {
        for(int i = 0; i < _activeSliders.Count; i++)
        {
            if(_activeSliders[i].value < _activeSliders[i].maxValue)
            {
                return true;
            }
        }

        return false;
    }

    public void WorkOnTask(float percentageAmount, float increaseSpeed)
    {
        int randomTask = Random.Range(0, _activeSliders.Count);
        IncreaseProgress(percentageAmount, increaseSpeed, _activeSliders[randomTask]);
    }

    public void IncreaseProgress(float percentageAmount, float increaseSpeed, Slider slider)
    {
        slider.GetComponent<ProgressOnBar>().IncreaseProgress(percentageAmount, increaseSpeed);
    }

    private int _taskIDs = 0;

    public void SetTaskName(TMP_InputField inputField, TMP_Dropdown projectType, TMP_Dropdown programmingLanguage, TMP_Dropdown complexity, TMP_InputField priceInput)
    {
        if(inputField.text == "" || priceInput.text == "")
        {
            return;
        }

        _taskName = inputField.text;
        _SliderText = inputField.text;

        for(int i = 0; i < _availableSliders.Count; i++)
        {
            if(!_availableSliders[i].gameObject.activeInHierarchy && !_taskNames.ContainsKey(_taskName))
            {
                _availableSliders[i].gameObject.SetActive(true);
                _availableSliders[i].GetComponentInChildren<TextMeshProUGUI>().text = _SliderText;
                _taskNames.Add(_taskName, _taskName);
                _activeSliders.Add(_availableSliders[i]);
                _availableSliders[i].name = _taskName;
                _availableSliders[i].GetComponent<ProgressOnBar>().ChangeTaskID(_taskIDs, complexity.value);
                _taskIDs++;
                _availableSliders.RemoveAt(i);

                Product product = new Product();
                product.isPlayer = true;
                product.Name = inputField.text;
                product.Company = "Player";
                product.Type = projectType.options[projectType.value].text;
                product.Language = programmingLanguage.options[programmingLanguage.value].text;
                product.Complexity = complexity.value;
                product.Price = int.Parse(priceInput.text);

                product.Age = 0;

                int quality = complexity.value * (complexity.value * Random.Range(1, 10));
                product.Quality = quality;

                int popularityModifier = complexity.value * quality;
                product.Popularity = popularityModifier;

                products.Add(product);
                break;
            }
            else if(_taskNames.ContainsKey(_taskName))
            {
                Debug.Log("Task name already exists");
                break;
            }
        }
    }

    public void MarketTask(int taskID)
    {
        GameObject productDetailPanel = Instantiate(productDetailPanelPrefab, productDetailPanelParent);
        InitializeProductPanel(productDetailPanel, taskID);

        completeProducts.Add(products[taskID]);
        productDetailPanels.Add(productDetailPanel);
    }

    public void AddProduct(RandomCompany randCompScript, Product product)
    {
        GameObject productDetailPanel = Instantiate(productDetailPanelPrefab, productDetailPanelParent);

        TextMeshProUGUI taskName = productDetailPanel.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI taskCompany = productDetailPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI taskType = productDetailPanel.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI taskLanguage = productDetailPanel.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI taskComplexity = productDetailPanel.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI taskPrice = productDetailPanel.transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>();

        taskName.text = product.Name;
        taskCompany.text = product.Company;
        taskType.text = product.Type;
        taskLanguage.text = product.Language;
        taskComplexity.text = product.Complexity.ToString();
        taskPrice.text = product.Price.ToString();

        completeProducts.Add(product);
        productDetailPanels.Add(productDetailPanel);
    }

    public void InitializeProductPanel(GameObject panelDetails, int taskID = 0)
    {
        TextMeshProUGUI taskName = panelDetails.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI taskCompany = panelDetails.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI taskType = panelDetails.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI taskLanguage = panelDetails.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI taskComplexity = panelDetails.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI taskPrice = panelDetails.transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>();

        taskName.text = products[taskID].Name;
        taskCompany.text = products[taskID].Company;
        taskType.text = products[taskID].Type;
        taskLanguage.text = products[taskID].Language;
        taskComplexity.text = products[taskID].Complexity.ToString();
        taskPrice.text = products[taskID].Price.ToString();
    }

    // TODO - Update the task list in the market

    public void UpdateMarketedTasks()
    {
        foreach (GameObject productDetailPanel in productDetailPanels)
        {
            productDetailPanel.transform.GetChild(6).GetComponentInChildren<TextMeshProUGUI>().text = completeProducts[productDetailPanels.IndexOf(productDetailPanel)].Sales.ToString();
        }
    }

    public void CompleteTask(Slider slider, int taskID)
    {
        // Check if Slider name matches a product name
        for (int i = 0; i < products.Count; i++)
        {
            if (slider.name == products[i].Name)
            {
                MarketTask(taskID);
                break;
            }
        }

        _activeSliders.Remove(slider);
        _availableSliders.Add(slider);
        slider.gameObject.SetActive(false);
    }
}
