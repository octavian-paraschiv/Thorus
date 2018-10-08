using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ThorusViewer.WinForms
{
    public class PropertyGridEx : PropertyGrid
    {
        public PropertyGridEx()
            : base()
        {
        }

        public bool ResizeDescriptionArea(int nNumLines)
        {
            try
            {
                System.Reflection.PropertyInfo pi = this.GetType().GetProperty("Controls");
                Control.ControlCollection cc = (ControlCollection)pi.GetValue(this, null);

                foreach (Control c in cc)
                {
                    Type ct = c.GetType();
                    string sName = ct.Name;

                    if (sName == "DocComment")
                    {
                        pi = ct.GetProperty("Lines");
                        pi.SetValue(c, nNumLines, null);

                        System.Reflection.FieldInfo fi = ct.BaseType.GetField("userSized",
                            System.Reflection.BindingFlags.Instance |
                            System.Reflection.BindingFlags.NonPublic);

                        fi.SetValue(c, true);
                    }
                }

                return true;
            }
            catch (Exception error)
            {
#if(DEBUG)
                MessageBox.Show(error.Message, "ResizeDescriptionArea()");
#endif

                return false;
            }
        }

        protected override void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
        {
            bool isValid = false;
            foreach (Attribute a in e.ChangedItem.PropertyDescriptor.Attributes)
            {
                RangeAttribute dra = a as RangeAttribute;
                if (dra != null)
                {
                    try
                    {
                        dra.Validate(e.ChangedItem.Value, e.ChangedItem.PropertyDescriptor.Name);
                        isValid = true;
                    }
                    catch (ValidationException ve)
                    {
                        MessageBox.Show(ve.Message, "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        PropertyFallback(e.ChangedItem.PropertyDescriptor.Name, e.OldValue);
                    }

                    break;
                }
            }

            if (isValid)
                base.OnPropertyValueChanged(e);

            this.Select();
            this.Focus();
        }

        private void PropertyFallback(string propName, object propValue)
        {
            var propArray = this.SelectedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propArray != null)
            {
                foreach (PropertyInfo p in propArray)
                {
                    if (p.Name == propName)
                    {
                        p.SetValue(this.SelectedObject, propValue, null);
                        break;
                    }
                }
            }
        }
    }
}
