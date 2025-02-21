using System.Text;
using System.Text.Json;
using web_app.Models;

namespace web_app.Services;

public class CategoryService: ICategoryService
{
    private readonly HttpClient client;

    private readonly JsonSerializerOptions options;

    public CategoryService(HttpClient client)
    {
        this.client = client;
        this.options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
    }
    
    public async Task<List<Category>?> GetCategories()
    {
        var response = await client.GetAsync("categories");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Category>>(content, options);
        }
        return null;
    }

    public async Task AddCategory(Category category)
    {
        var content = new StringContent(JsonSerializer.Serialize(category, options), Encoding.UTF8, "application/json");
        await client.PostAsync("categories", content);
    }

    public async Task UpdateCategory(Category category)
    {
        var content = new StringContent(JsonSerializer.Serialize(category, options), Encoding.UTF8, "application/json");
        await client.PutAsync($"categories/{category.Id}", content);
    }

    public async Task DeleteCategory(int id)
    {
        await client.DeleteAsync($"categories/{id}");
    }
}


public interface ICategoryService
{
    Task<List<Category>?> GetCategories();
    Task AddCategory(Category category);
    Task UpdateCategory(Category category);
    Task DeleteCategory(int id);
}