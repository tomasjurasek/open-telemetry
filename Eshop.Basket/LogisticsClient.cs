namespace Eshop.Basket;

public class LogisticsClient(HttpClient httpClient)
{ 
    public async Task<bool> IsAvailable(Guid productId)
    {
        var response = await httpClient.GetAsync($"/products/{productId}/available");
        return response.EnsureSuccessStatusCode().IsSuccessStatusCode;
    }
}
