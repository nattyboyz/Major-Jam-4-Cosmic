using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}

//[CustomPropertyDrawer(typeof(AudioID))]
//[CustomPropertyDrawer(typeof(particleID))]

//public class AudioIDDrawer : SerializableDictionaryPropertyDrawer { }


//[CustomPropertyDrawer(typeof(AnimationListDict))]

//[CustomPropertyDrawer(typeof(ThemeDataDictonary))]
//public class ThemeDataDrawer : SerializableDictionaryPropertyDrawer { }

//[CustomPropertyDrawer(typeof(CategoryDataDictonary))]
//public class CategoryDataDrawer : SerializableDictionaryPropertyDrawer { }

//[CustomPropertyDrawer(typeof(ShopCategoryDatabaseDictonary))]
//public class ShopCategoryDatabaseDictonaryDrawer : SerializableDictionaryPropertyDrawer { }


public class AnimationListDrawer : SerializableDictionaryPropertyDrawer 
{ 
   // [SerializeField]
}
