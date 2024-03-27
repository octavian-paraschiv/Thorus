using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Forms;

namespace ThorusViewer.WinForms
{
    public class PropertyGridEx : PropertyGrid
    {
        public PropertyGridEx()
            : base()
        {
            base.CausesValidation = false;
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
                    catch (ValidationException)
                    {
                        var msg = $"Value '{e.ChangedItem.Value}' is not a valid value for field '{e.ChangedItem.PropertyDescriptor.Name}'";
                        MessageBox.Show(ParentForm, msg, "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        PropertyFallback(e.ChangedItem.PropertyDescriptor.Name, e.OldValue);
                    }

                    break;
                }

                /*
                JetStreamPatternAttribute jspa = a as JetStreamPatternAttribute;
                if (jspa != null)
                {
                    try
                    {
                        jspa.Validate(e.ChangedItem.Value, e.ChangedItem.PropertyDescriptor.Name);
                        isValid = true;
                    }
                    catch (ValidationException ve)
                    {
                        var msg = $"Value '{e.ChangedItem.Value}' is not a valid value for field '{e.ChangedItem.PropertyDescriptor.Name}'";
                        MessageBox.Show(ParentForm, msg, "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        PropertyFallback(e.ChangedItem.PropertyDescriptor.Name, e.OldValue);
                    }

                    break;
                }
                */
            }

            if (isValid)
            {
                try
                {
                    base.OnPropertyValueChanged(e);
                }
                catch (ValidationException ve)
                {
                    var s = ve.Message;
                }
            }

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
