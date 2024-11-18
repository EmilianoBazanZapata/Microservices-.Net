using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Banking.Domain.Events;

public class TransferCreateEvent(int from, int to, decimal amount) : Event 
{
    public int From { get; set; } = from;
    public int To { get; set; } = to;
    public decimal Amount { get; set; } = amount;
}