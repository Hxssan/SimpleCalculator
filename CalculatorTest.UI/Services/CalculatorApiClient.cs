using System.Text.Json;

namespace CalculatorTest.UI.Services;

public class CalculatorApiClient(HttpClient httpClient)
{
    public async Task<CalculatorApiResult> AddAsync(int start, int amount, CancellationToken cancellationToken = default)
    {
        return await SendAsync("/calculator/add", start, amount, cancellationToken);
    }

    public async Task<CalculatorApiResult> SubtractAsync(int start, int amount, CancellationToken cancellationToken = default)
    {
        return await SendAsync("/calculator/subtract", start, amount, cancellationToken);
    }

    private async Task<CalculatorApiResult> SendAsync(string route, int start, int amount, CancellationToken cancellationToken)
    {
        var requestUri = $"{route}?start={start}&amount={amount}";
        using var response = await httpClient.PostAsync(requestUri, content: null, cancellationToken);

        string content;
        try
        {
            content = await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return new CalculatorApiResult(false, null, ex.Message);
        }

        if (response.IsSuccessStatusCode)
        {
            if (int.TryParse(content, out var value))
            {
                return new CalculatorApiResult(true, value, null);
            }

            try
            {
                value = JsonSerializer.Deserialize<int>(content);
                return new CalculatorApiResult(true, value, null);
            }
            catch (Exception ex)
            {
                return new CalculatorApiResult(false, null, $"Could not parse calculator result. {ex.Message}");
            }
        }

        if (!string.IsNullOrWhiteSpace(content))
        {
            return new CalculatorApiResult(false, null, content);
        }

        return new CalculatorApiResult(false, null, $"Request failed with status code {(int)response.StatusCode}.");
    }
}

public record CalculatorApiResult(bool Succeeded, int? Result, string? ErrorMessage);
