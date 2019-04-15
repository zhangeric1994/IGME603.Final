using System.Collections.Generic;

public interface IAttributeCollection : IEnumerable<KeyValuePair<AttributeType, float>>
{
    //float this[int id] { get; }
    EventOnAttributeChange OnAttributeChange { get; }
    float this[AttributeType type] { get; }
}
