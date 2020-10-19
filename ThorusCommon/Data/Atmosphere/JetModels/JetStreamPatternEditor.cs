using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ThorusCommon.Data
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]

    public class JetStreamPatternEditor : UITypeEditor
    {
        private IWindowsFormsEditorService _editorService;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool IsDropDownResizable => false;

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (_editorService != null)
            {
                // Display an angle selection control and retrieve the value.
                ListBox lb = new ListBox();
                lb.BorderStyle = BorderStyle.None;
                lb.Dock = DockStyle.Fill;

                lb.SelectedIndexChanged += new EventHandler(lb_SelectedIndexChanged);

                lb.Items.AddRange(GetJetModels());

                lb.Height = (lb.Items.Count + 1) * lb.Font.Height;

                lb.SelectedIndex = lb.FindString(value as string);

                _editorService.DropDownControl(lb);

                return (string)lb.SelectedItem;
            }

            return null;
        }

        private string[] GetJetModels()
        {
            var allTypes = GetType().Assembly.GetTypes();
            return (from type in allTypes
                    where type.IsSubclassOf(typeof(JetLevel))
                    orderby type.Name ascending
                    select type.Name).ToArray();
        }

        void lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            _editorService?.CloseDropDown();
        }
    }

    public class JetStreamPatternAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return AtmosphericLevelFactory.IsValidJetStreamPattern(value as string);
        }
    }
}
