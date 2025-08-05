# A High-Performance ScriptableObject Database System for Unity

Created by Krolti

DatabaseSO is a simple yet effective way to use ScriptableObjects in Unity to create structures that resemble databases. 
It maintains editor-friendly workflows while offering quick search operations, support for serialization, and a query system.

## Key Features:


##### Lightweight, performant
 - Binary Search implementation enables millions of operations on large indexed datasets
 - Efficient memory management via appropriate ScriptableObject implementation

##### Advanced Querying
 - Supports filtering, ordering, skipping, and taking

##### Editor Integration
 - Automatic ID assignment and validation
 - Null reference cleaning
 - JSON import/export functionality with UniTask
 - Dirty flag handling for proper asset saving

##### Extensible Design
 - Generic implementation works with any data type
 - Virtual methods allow custom validation logic


# Examples


```C#
using UnityEngine;

namespace Your.Namespace
{
    [CreateAssetMenu(fileName = "Color Database", menuName = "DatabaseSO/Examples/Color Database")]
    public class ColorDatabase : Database<ColorData>
    {
		
    }

    [System.Serializable]
    public class ColorData : DatabaseItem
    {
        [SerializeField] private Color color;
        public Color Color => color;
        
        /// <summary>
        /// Just transparensy check.
        /// </summary>
        public override bool IsValid => color.a > 0;
		
        public ColorData(Color color)
        {
            this.color = color;
        }
        
		// Method to fix database item
        public override bool TryFix()
        {
            if(color != null)
            {
                color.a = 1;
                return true;
            }
            return false;
        }
		
    }
}
```

To begin with creating Databases you can create a simple database.

Note that class that inherits Database<> should have [CreateAssetMenu] attribute, and T type should include [System.Serializable]