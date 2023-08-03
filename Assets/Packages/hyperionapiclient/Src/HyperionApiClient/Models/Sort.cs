using System.Runtime.Serialization;

namespace HyperionApiClient.Models
{
    /// <summary>sort direction</summary>
    public enum Sort
    {
        [EnumMember(Value = @"desc")]
        Desc = 0,
    
        [EnumMember(Value = @"asc")]
        Asc = 1,
    
        [EnumMember(Value = @"1")]
        _1 = 2,
    
        [EnumMember(Value = @"-1")]
        Minus1 = 3,
    
    }
}