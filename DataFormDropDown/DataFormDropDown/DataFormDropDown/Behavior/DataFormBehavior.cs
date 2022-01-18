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
            dataForm.DataObject = new ContactInfo();
            dataForm.RegisterEditor("DropDown", new MultiSelectDropDownEditor(dataForm));
            dataForm.RegisterEditor("Country", "DropDown");
        }

        private void DataForm_AutoGeneratingDataFormItem(object sender, AutoGeneratingDataFormItemEventArgs e)
        {
            if (e.DataFormItem != null && e.DataFormItem.Name == "Country")
            {
                    (e.DataFormItem as DataFormDropDownItem).DisplayMemberPath = "City";
                    (e.DataFormItem as DataFormDropDownItem).SelectedValuePath = "City";
            }
        }
    }

    public class SourceProviderContactForm : SourceProvider
    {
        public override IList GetSource(string sourceName)
        {
            var list = new List<Address>();
            if (sourceName == "Country")
            {
                list.Add(new Address { PostalCode = "1", City = "India" });
                list.Add(new Address { PostalCode = "2", City = "Aus" });
                list.Add(new Address { PostalCode = "3", City = "NZ" });

            }
            return list;
        }
    }
}
