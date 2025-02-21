using System.Text;
using System.Text.Json;
using web_app.Models;

namespace web_app.Services;

public class ProductService: IProductService
{
    private readonly HttpClient client;

    private readonly JsonSerializerOptions options;

    public ProductService(HttpClient client)
    {
        this.client = client;
        this.options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
    }
    
    public async Task<List<Product>> Get()
    {
        var response = await client.GetAsync("products");
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);    
        }
            
        var products = JsonSerializer.Deserialize<List<Product>>(content, options);
        if (products == null)
        {
            throw new ApplicationException("Error deserializing the product list.");
        }

        return products;
        
    }

    public async Task Add(Product product)
    {
        var content = new StringContent(JsonSerializer.Serialize(product, options), 
        Encoding.UTF8, "application/json");
        
        await client.PostAsync("products", content);
    }

    public async Task Update(Product product)
    {
        var content = new StringContent(JsonSerializer.Serialize(product, options), Encoding.UTF8, "application/json");
        await client.PutAsync($"products/{product.Id}", content);
    }

    public async Task Delete(int id)
    {
        await client.DeleteAsync($"products/{id}");
    }
}


public interface IProductService
{
    Task<List<Product>> Get();
    Task Add(Product product);
    Task Update(Product product);
    Task Delete(int id);
}