using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitLibrary.Editor.XiaoJiang.AssetPathField {
    public class AssetPathField: BaseField<string> {

        private VisualElement input;
        private ObjectField objectField;
        
        // public bool AllowSceneObjects {
        //     get => objectField.allowSceneObjects;
        //     set => objectField.allowSceneObjects = value;
        // }
        
        public System.Type ObjectType {
            get => objectField.objectType;
            set => objectField.objectType = value;
        }
        
        public AssetPathField() : this(null) { }

        public AssetPathField(string label) : base(label, null) {
            input = this.Q(className: "unity-base-field__input");
            
            objectField = new ObjectField {
                objectType = typeof(Object),
                allowSceneObjects = false
            };

            objectField.RegisterValueChangedCallback(evt => {
                if (evt.newValue == null) {
                    value = string.Empty;
                }
                
                var path = AssetDatabase.GetAssetPath(evt.newValue);
                if (string.IsNullOrEmpty(path)) {
                    value = string.Empty;
                    Debug.LogError($"[AssetPathField] failed to get asset path, object: {evt.newValue}");
                } else {
                    value = path;
                }
            });
            
            input.Add(objectField);
        }

        public new class UxmlFactory : UxmlFactory<AssetPathField, UxmlTraits> { }
        
        public new class UxmlTraits : BaseField<string>.UxmlTraits {
            // private UxmlBoolAttributeDescription allowSceneObjectsAttr;
            private UxmlTypeAttributeDescription<UnityEngine.Object> objectTypeAttr;
            
            public UxmlTraits() {
                // allowSceneObjectsAttr = new UxmlBoolAttributeDescription {
                //     name = "allow-scene-objects",
                //     defaultValue = true
                // };

                objectTypeAttr = new UxmlTypeAttributeDescription<UnityEngine.Object> {
                    name = "type",
                    defaultValue = typeof (UnityEngine.Object)
                };
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                
                // ((AssetPathField) ve).AllowSceneObjects = allowSceneObjectsAttr.GetValueFromBag(bag, cc);
                ((AssetPathField) ve).ObjectType = objectTypeAttr.GetValueFromBag(bag, cc);
            }
        }
    }
}