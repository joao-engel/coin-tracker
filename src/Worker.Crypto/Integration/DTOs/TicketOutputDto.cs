using Core.Lib.DTOs;

namespace Worker.Crypto.Integration.DTOs;
public class TicketOutputDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
}
