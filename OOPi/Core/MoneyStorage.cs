namespace VendingMachineApp.Core;

// Деньги
public class MoneyStorage
{
    private Dictionary<decimal, int> _cash = new()
    {
        {1,10},{2,10},{5,10},{10,10},{50,5},{100,5},{200,2},{500,1}
    };

    private static readonly decimal[] _denomsDesc = {500,200,100,50,10,5,2,1};

    public bool IsValidDenomination(decimal d) => _cash.ContainsKey(d);

    public bool TryAddMoney(decimal d)
    {
        if (!IsValidDenomination(d)) return false;
        _cash[d]++;
        return true;
    }

    public decimal TotalCash() => _cash.Sum(kv => kv.Key * kv.Value);
    
    public Dictionary<decimal,int> GetChange(decimal amount)
    {
        var result = new Dictionary<decimal,int>();
        decimal left = amount;
        foreach (var d in _denomsDesc)
        {
            if (left <= 0) break;
            if (_cash[d] == 0) continue;
            int need = (int)(left / d);
            if (need <= 0) continue;
            int give = Math.Min(need, _cash[d]);
            if (give > 0)
            {
                result[d] = give;
                left -= give * d;
            }
        }
        foreach (var kv in result)
            _cash[kv.Key] -= kv.Value;
        return result;
    }
    
    public Dictionary<decimal,int>? TryExtractExact(decimal amount)
    {
        if (amount <= 0) return null;
        var temp = _cash.ToDictionary(k=>k.Key,v=>v.Value);
        var result = new Dictionary<decimal,int>();
        decimal left = amount;

        foreach (var d in _denomsDesc)
        {
            if (left <= 0) break;
            int avail = temp[d];
            if (avail == 0) continue;
            int need = (int)(left / d);
            if (need <= 0) continue;
            int take = Math.Min(need, avail);
            if (take > 0)
            {
                result[d] = take;
                temp[d] -= take;
                left -= take * d;
            }
        }

        if (left == 0)
        {
            _cash = temp;
            return result;
        }
        return null;
    }
}