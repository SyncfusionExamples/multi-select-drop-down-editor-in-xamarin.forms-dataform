using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Syncfusion.XForms.DataForm;
using System.Collections;
using System.ComponentModel;
using Syncfusion.XForms.DataForm.Editors;
using Syncfusion.XForms.ComboBox;

namespace DataFormDropDown
{
    public class DataFormBehavior : Behavior<ContentPage>
    {
        SfDataForm dataForm;
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            dataForm = bindable.FindByName<SfDataForm>("dataForm");
            dataForm.SourceProvider = new SourceProviderContactForm();
            dataForm.AutoGeneratingDataFormItem += DataForm_AutoGeneratingDataFormItem;
            dataForm.DataObject = new Address();
            dataForm.RegisterEditor("DropDown", new CustomDropDownEditor(dataForm));
            dataForm.RegisterEditor("Country", "DropDown");
        }

        private void DataForm_AutoGeneratingDataFormItem(object sender, AutoGeneratingDataFormItemEventArgs e)
        {
            if (e.DataFormItem != null && e.DataFormItem.Name == "Country")
            {
                if (Device.RuntimePlatform != Device.UWP)
                {
                    (e.DataFormItem as DataFormDropDownItem).DisplayMemberPath = "label";
                    (e.DataFormItem as DataFormDropDownItem).SelectedValuePath = "value";
                }
            }
        }
    }


    public class TestData
    {
        public string label { get; set; }
        public string value { get; set; }
    }

    public class SourceProviderContactForm : SourceProvider
    {
        public override IList GetSource(string sourceName)
        {
            var list = new List<TestData>();
            if (sourceName == "Country")
            {
                list.Add(new TestData { value = "1", label = "item1" });
                list.Add(new TestData { value = "2", label = "item2" });
                list.Add(new TestData { value = "3", label = "item3" });

            }
            return list;
        }
    }


    public class CustomDropDownEditor : DataFormDropDownEditor
    {
        private Address Address;
        SfDataForm sfDataForm;
        public CustomDropDownEditor(SfDataForm dataForm) : base(dataForm)
        {
            this.sfDataForm = dataForm;
            this.Address = this.DataForm.DataObject as Address;
        }
        protected override void OnInitializeView(DataFormItem dataFormItem, SfComboBox view)
        {
            base.OnInitializeView(dataFormItem, view);
            view.MultiSelectMode = MultiSelectMode.Token;
            view.TokensWrapMode = TokensWrapMode.Wrap;
            view.SelectionChanged += View_SelectionChanged;
        }

        protected override void OnCommitValue(SfComboBox view)
        {
            if (view.MultiSelectMode == MultiSelectMode.None)
            {
                // Use existing method for single selection. 
                base.OnCommitValue(view);
            }
            else
            {
                // Multi Selection needs to be updated with all selected items. 
                if (view != null && view.SelectedItem != null && view.SelectedItem is IList)
                {
                    string country = string.Empty;
                    foreach (TestData item in (IList)view.SelectedItem)
                    {
                        if (country.Contains(item.label))
                        {
                            continue;
                        }

                        country = string.IsNullOrEmpty(country) ? item.label : country + "," + item.label;
                    }

                    this.Address.Country = country;
                }
            }
        }

        protected override void OnUpdateValue(DataFormItem dataFormItem, SfComboBox view)
        {
            if (view.MultiSelectMode == MultiSelectMode.None)
            {
                base.OnUpdateValue(dataFormItem, view);
            }
            else
            {
                var list = (dataFormItem as DataFormDropDownItem).ItemsSource;
                if (list != null)
                {
                    view.DataSource = list.OfType<object>().ToList();
                }
                else
                {
                    view.DataSource = null;
                }
            }
        }

        protected override bool OnValidateValue(SfComboBox view)
        {
            if (view.MultiSelectMode == MultiSelectMode.None)
            {
                return base.OnValidateValue(view);
            }
            else
            {
                this.OnCommitValue(view);

                // Here country is multi selection property. 
                if (string.IsNullOrEmpty(this.Address.Country))
                {
                    return false;
                }

                return true;
            }
        }
        private void View_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

        }
    }
}
