using System.Collections.Generic;
using Xamarin.Forms;

namespace ReactNative.Forms.Views
{
    public class ReactView : View
    {
        public static readonly BindableProperty PackagerUrlProperty = BindableProperty.Create(
           propertyName: nameof(PackagerUrl),
           returnType: typeof(string),
           declaringType: typeof(ReactView),
           defaultValue: null);

        public string PackagerUrl
        {
            get { return (string)GetValue(PackagerUrlProperty); }
            set { SetValue(PackagerUrlProperty, value); }
        }

        public static readonly BindableProperty BundleNameProperty = BindableProperty.Create(
           propertyName: nameof(BundleName),
           returnType: typeof(string),
           declaringType: typeof(ReactView),
           defaultValue: null);

        public string BundleName
        {
            get { return (string)GetValue(BundleNameProperty); }
            set { SetValue(BundleNameProperty, value); }
        }

        public static readonly BindableProperty ModulePathProperty = BindableProperty.Create(
           propertyName: nameof(ModulePath),
           returnType: typeof(string),
           declaringType: typeof(ReactView),
           defaultValue: null);

        public string ModulePath
        {
            get { return (string)GetValue(ModulePathProperty); }
            set { SetValue(ModulePathProperty, value); }
        }

        public static readonly BindableProperty ModuleNameProperty = BindableProperty.Create(
           propertyName: nameof(ModuleName),
           returnType: typeof(string),
           declaringType: typeof(ReactView),
           defaultValue: null);

        public string ModuleName
        {
            get { return (string)GetValue(ModuleNameProperty); }
            set { SetValue(ModuleNameProperty, value); }
        }

        public static readonly BindableProperty PropertiesProperty = BindableProperty.Create(
            propertyName: nameof(Properties),
            returnType: typeof(Dictionary<string, object>),
            declaringType: typeof(ReactView),
            defaultValue: new Dictionary<string, object>());

        public Dictionary<string, object> Properties
        {
            get { return (Dictionary<string, object>)GetValue(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }
    }
}

