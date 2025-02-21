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
    
    public async Task<List<Product>?> GetProducts()
    {
        var response = await client.GetAsync("products");
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);    
        }
        return JsonSerializer.Deserialize<List<Product>>(content, options);
        
    }

    public async Task AddProduct(Product product)
    {
        var content = new StringContent(JsonSerializer.Serialize(product, options), 
        Encoding.UTF8, "application/json");
        
        await client.PostAsync("products", content);
    }

    public async Task UpdateProduct(Product product)
    {
        var content = new StringContent(JsonSerializer.Serialize(product, options), Encoding.UTF8, "application/json");
        await client.PutAsync($"products/{product.Id}", content);
    }

    public async Task DeleteProduct(int id)
    {
        await client.DeleteAsync($"products/{id}");
    }
}


public interface IProductService
{
    Task<List<Product>?> GetProducts();
    Task AddProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(int id);
}