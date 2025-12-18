using Core.Lib.Configuration;
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

        public async Task<List<PriceUpdateDto>> SearchCoins()
        {
            List<PriceUpdateDto> listCoins = [];

            foreach (var coin in CryptoCatalog.Coins)
            {
                TicketOutputDto ticketOutputDto = await GetPrice(coin.Symbol);                

                listCoins.Add(new PriceUpdateDto(coin.DisplayName, coin.RoutingKey, ticketOutputDto.Price));
            }

            return listCoins;
        }

        private async Task<TicketOutputDto> GetPrice(string symbolPair)
        {
            HttpResponseMessage respose = await _httpClient.GetAsync($"ticker/price?symbol={symbolPair.ToUpper()}");

            if (!respose.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar {symbolPair}: {respose.StatusCode} - {respose.ReasonPhrase}");

            string jsonResponse = await respose.Content.ReadAsStringAsync();

            TicketOutputDto? ticket = JsonSerializer.Deserialize<TicketOutputDto>(jsonResponse, _jsonSerializerOptions);

            return ticket 
                ?? throw new Exception($"Erro ao desserializar dados de preço da Binance para o símbolo {symbolPair}");
        }
    }
}
