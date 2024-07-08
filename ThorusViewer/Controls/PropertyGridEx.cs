using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Forms;

namespace ThorusViewer.Controls
{
    public class PropertyGridEx : PropertyGrid
    {
        public PropertyGridEx()
            : base()
        {
            CausesValidation = false;
        }

        public bool ResizeDescriptionArea(int nNumLines)
        {
            try
            {
                PropertyInfo pi = GetType().GetProperty("Controls");
                ControlCollection cc = (ControlCollection)pi.GetValue(this, null);

                foreach (Control c in cc)
                {
                    Type ct = c.GetType();
                    string sName = ct.Name;

                    if (sName == "DocComment")
                    {
                        pi = ct.GetProperty("Lines");
                        pi.SetValue(c, nNumLines, null);

                        FieldInfo fi = ct.BaseType.GetField("userSized",
                            BindingFlags.Instance |
                            BindingFlags.NonPublic);

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

            Select();
            Focus();
        }

        private void PropertyFallback(string propName, object propValue)
        {
            var propArray = SelectedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propArray != null)
            {
                foreach (PropertyInfo p in propArray)
                {
                    if (p.Name == propName)
                    {
                        p.SetValue(SelectedObject, propValue, null);
                        break;
                    }
                }
            }
        }
    }
}
