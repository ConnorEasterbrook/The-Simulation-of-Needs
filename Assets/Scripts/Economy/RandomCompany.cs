using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RandomCompany : MonoBehaviour
{
     public static RandomCompany instance;

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

    public void CreateRandomProduct()
    {
        Product product = new Product();
        GetProductInformation(product);
        CreateJob.instance.AddProduct(this, product);
    }

    private void GetProductInformation(Product product)
    {
        string randomName = Random.RandomRange(0, 1000).ToString();
        product.Name = randomName;
        product.Type = JobType.Game.ToString();
        product.Language = "C#";
        product.Complexity = 3;
        product.Price = Random.RandomRange(1, 100);

        product.Age = 0;

        int quality = product.Complexity * (product.Complexity * Random.Range(1, 10));
        product.Quality = quality;

        int popularityModifier = product.Complexity * quality;
        product.Popularity = popularityModifier;
    }

    private enum JobType
    {
        Game,
        Software,
        Web,
        Security
    }
}
