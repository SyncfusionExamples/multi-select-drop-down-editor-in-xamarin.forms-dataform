using Syncfusion.XForms.ComboBox;
using Syncfusion.XForms.DataForm;
using Syncfusion.XForms.DataForm.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFormDropDown
{
    public class MultiSelectDropDownEditor : DataFormDropDownEditor
    {
        private ContactInfo contactInfo;
        public MultiSelectDropDownEditor(SfDataForm dataForm) : base(dataForm)
        {
            this.contactInfo = this.DataForm.DataObject as ContactInfo;
        }
        protected override void OnInitializeView(DataFormItem dataFormItem, SfComboBox view)
        {
            base.OnInitializeView(dataFormItem, view);
            view.MultiSelectMode = MultiSelectMode.Token;
            view.TokensWrapMode = TokensWrapMode.Wrap;
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
                    foreach (Address address in (IList)view.SelectedItem)
                    {
                        if (country.Contains(address.City))
                        {
                            continue;
                        }

                        country = string.IsNullOrEmpty(country) ? address.City : country + "," + address.City;
                    }

                    this.contactInfo.Country = country;
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
                if (string.IsNullOrEmpty(this.contactInfo.Country))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
