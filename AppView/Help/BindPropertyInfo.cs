using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppView.Help
{
    /// <summary>
    /// Used for binding in the details screen
    /// </summary>
    public class BindPropertyInfo : SimpleBindPropertyInfo
    {
        public bool HasVariants { get; set; }
        public Type VariantsType { get; set; }
        public string SelectedValuePath { get; set; }
        public string DisplayMemberPath { get; set; }
        public string SelectedValue { get; set; }
        public IEnumerable Variants { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadOnlyOnEditing { get; set; }
        public bool HideOnEditing { get; set; }
        public IEnumerable<ValidationRule> ValidationRules { get; set; } 
    }
}
