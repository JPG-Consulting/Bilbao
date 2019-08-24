#if NET35
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Bilbao.Web.UI
{
    /// <summary>
    ///   
    /// </summary>
    /// <remarks>
    ///   Este control introduce cambios que se presentan en NET Framework 4.0 y posteriores.
    /// </remarks>
    [ToolboxItem(false)]
    [Serializable]
    public class StandardControl : System.Web.UI.Control
    {

        private System.Web.UI.ClientIDMode _clientIDMode = System.Web.UI.ClientIDMode.Inherit;

        public override string ClientID
        {
            get
            {
                switch (_clientIDMode)
                {
                    case System.Web.UI.ClientIDMode.AutoID:
                        return base.ClientID;
                    case System.Web.UI.ClientIDMode.Static:
                        return this.ID;
                    case System.Web.UI.ClientIDMode.Inherit:
                        try
                        {
                            return GetParentControlClientID(this);
                        }
                        catch
                        {
                            return base.ClientID;
                        }
                    default:
                        return base.ClientID;
                }
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
        [
            DefaultValue(System.Web.UI.ClientIDMode.Inherit),
            System.Web.UI.Themeable(false)
        ]
        public virtual System.Web.UI.ClientIDMode ClientIDMode
        {
            get { return _clientIDMode; }
            set { _clientIDMode = value; }
        }

        private string GetParentControlClientID(System.Web.UI.Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");

            PropertyInfo propertyInfo = control.GetType().GetProperty("ClientIDMode");
            if ((propertyInfo == null) || (!propertyInfo.CanRead))
                return control.ClientID;

            object objMode = propertyInfo.GetValue(this, null);
            if ((objMode == null) || (!Enum.IsDefined(typeof(System.Web.UI.ClientIDMode), objMode)))
                return control.ClientID;
            
            System.Web.UI.ClientIDMode mode = (System.Web.UI.ClientIDMode)objMode;

            if (mode == System.Web.UI.ClientIDMode.Inherit)
            {
                if (control.Parent == null)
                    return control.ClientID;

                System.Web.UI.Control parent = control.Parent as System.Web.UI.Control;
                if (parent == null)
                    return control.ClientID;

                return GetParentControlClientID(control.Parent);
            }

            if (mode == System.Web.UI.ClientIDMode.Static)
                return control.ID;

            return control.ClientID;
        }
    }
}
#endif