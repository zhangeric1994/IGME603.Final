using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventOnEquipmentChange : EventOnDataChange2<ItemData> { }


public class Equipment : IAttributeCollection
{
    private WeaponData weapon;
    private AttributeSet attributes = new AttributeSet();

    EventOnDataChange2<WeaponData> OnWeaponChange { get; } = new EventOnDataChange2<WeaponData>();
    EventOnAttributeChange IAttributeCollection.OnAttributeChange { get; } = new EventOnAttributeChange();

    float IAttributeCollection.this[AttributeType type]
    {
        get
        {
            return attributes[type];
        }
    }

    public WeaponData Weapon
    {
        get
        {
            return weapon;
        }

        private set
        {
            if (value != weapon)
            {
                WeaponData previousWeapon = weapon;

                if (previousWeapon != null)
                    attributes.Decrement(previousWeapon.Attributes);

                weapon = value;

                attributes.Increment(value.Attributes);

                OnWeaponChange.Invoke(previousWeapon, weapon);
            }
        }
    }


    public WeaponData ChangeWeapon(WeaponData weapon)
    {
        WeaponData output = this.weapon;

        Weapon = weapon;

        return output;
    }


    IEnumerator<KeyValuePair<AttributeType, float>> IEnumerable<KeyValuePair<AttributeType, float>>.GetEnumerator()
    {
        return attributes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return attributes.GetEnumerator();
    }
}
