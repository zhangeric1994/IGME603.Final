using System.Collections.Generic;

public interface IAttributeCollection : IEnumerable<KeyValuePair<int, float>>
{
    float this[int id] { get; }
    float this[AttributeType attribute] { get; }
}
