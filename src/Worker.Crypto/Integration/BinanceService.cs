using Core.Lib.Domain.Entities;
using Core.Lib.DTOs;
using System.Text.Json;
using Worker.Crypto.Integration.DTOs;

namespace Worker.Crypto.Integration
{
    public class BinanceService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public BinanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PriceUpdateDto>> SearchCoins(List<CryptoAsset> activeCoins)
        {
            List<PriceUpdateDto> listCoins = [];

            var coins = await GetPrices();
            foreach (var coin in activeCoins)
            {
                var binanceTicker = coins.FirstOrDefault(b => b.Symbol.Equals(coin.Symbol, StringComparison.OrdinalIgnoreCase));

                if (binanceTicker != null)                
                    listCoins.Add(new PriceUpdateDto(coin.DisplayName, coin.Key, binanceTicker.Price));                
            }

            return listCoins;
        }

        private async Task<List<TicketOutputDto>> GetPrices()
        {
            HttpResponseMessage respose = await _httpClient.GetAsync($"ticker/price");

            if (!respose.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar cryptos: {respose.StatusCode} - {respose.ReasonPhrase}");

            string jsonResponse = await respose.Content.ReadAsStringAsync();

            List<TicketOutputDto>? tickets = JsonSerializer.Deserialize<List<TicketOutputDto>>(jsonResponse, _jsonSerializerOptions);

            return tickets ?? [];
        }
    }
}
