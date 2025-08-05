# A High-Performance ScriptableObject Database System for Unity


![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white) ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) ![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white)

<!-- Proudly created with GPRM ( https://gprm.itsvg.in ) -->

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


## Getting Started

> **Note:** Examples in package are optional

- You can use template to create your own database

Right click in Project tab >

```
Create/Database SO/Database Script
```

- It will automatically create a simple empty database implementation:

```C#
using UnityEngine;
using Krolti.DatabaseSO;

[CreateAssetMenu(fileName = "ClassName", menuName = "Database/ClassName")]
public class ClassName : Database<ClassNameData>
{

}
[System.Serializable]
public class ClassNameData : DatabaseItem
{

}
```


- Let's create Simple Color Database

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


### Usage


You can search items in the database:

```C#
// ...
ColorData founditem = colorDatabase.Search(searchID);
// ...
```

Or with safe method:

```C#
if(colorDatabase.TrySearch(searchID, out ColorData data))
{
    data.DataTag = "Modified tag!";
}
```


### Unitask / Async

Also you can write async JSON saving with Task/UniTask

```C#

public async UniTaskVoid StartExportUniTask()
{
    // ...
    string json = await database.ConvertToJsonUniTaskAsync(true, _cts.Token);
    // ...
}

public async void StartExport()
{
    // ...
    string json = await database.ConvertToJsonAsync(true, _cts.Token);
    // ...
}
```


## Tag repository

If you want to have access your database with tags there's a solution in Core folder:


```C#
[CreateAssetMenu(fileName = "Respond Database", menuName = "DatabaseSO/Examples/Respond Database")]
public class RespondDatabase : TagRepository<RespondData>
{

}
[System.Serializable]

public class RespondData : TaggedData
{
    // Take input as a database tag
    [SerializeField] private string output;

    public string GetRespond() => output;
}
```


### Tag Repository Usage

```C#
[SerializeField] private TagRepository<RespondData> respondData;

// or with direct acess
// [SerializeField] private RespondDatabase respondData;

[SerializeField] private string input;
[SerializeField] private string tryInput;

private void Awake()
{
    string respond = respondData.GetByTag(input).GetRespond();
    Debug.Log($"Assistant's respond: {respond}");

    if(respondData.TryGetValue(tryInput, out var data))
    {
        Debug.Log($"Assistant's second respond, successfully: {data.GetRespond()}");
    }
}
```

## Thread Safety


>Although not all of this code is thread-safe, some of it is. It functions well when used on a single thread (such as Unity's main loop), but if you use it on multiple threads, you'll need to manage >synchronization yourself to prevent problems.



## Install package

> Window → Package Manager → + → Add package from Git URL...  
> Paste:

```
https://github.com/krolti/com.krolti.databaseso.git
```

Or Install via manifest.json in "dependencies": {}

```
"com.krolti.database-so": "https://github.com/krolti/com.krolti.databaseso.git"
```



## Contribution

If you find this library useful, please consider starring the repository and contributing improvements!
For issues, suggestions, or feedback, [open an issue here](https://github.com/krolti/krolti-com.krolti.databaseso/issues).
