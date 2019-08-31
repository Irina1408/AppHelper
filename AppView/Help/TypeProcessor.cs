using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppView.Help
{
    using AppCommon.Attributes;
    using AppCommon.ValidationRules;
    using AppUtils;
    using AppUtils.Settings;

    public static class TypeProcessor
    {
        public static IEnumerable<SimpleBindPropertyInfo> GenerateColumnsInfo<T>()
        {
            var columnsInfo = new List<SimpleBindPropertyInfo>();

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                // check should be shown this property
                if (!ShowProperty(propertyInfo) || !ShowInDataGridProperty(propertyInfo))
                    continue;


                // create column info object
                SimpleBindPropertyInfo columnInfo = null;

                if (propertyInfo.PropertyType.IsClass)
                    columnInfo = HandleColumnClassTypeProperty(propertyInfo);
                else
                {
                    columnInfo = new SimpleBindPropertyInfo
                    {
                        Name = propertyInfo.Name, 
                        DataType = propertyInfo.PropertyType
                    };
                }

                // format for date
                columnInfo.Format = propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?)
                    ? AppSettings.DateFormat 
                    : GetPropertyFormat(propertyInfo);

                // check IsReadOnly
                columnInfo.IsReadOnly = IsReadOnlyProperty(propertyInfo);
                // set header
                columnInfo.Header = GetPropertyHeader(propertyInfo);
                // set order
                columnInfo.Order = GetPropertyOrder(propertyInfo);

                // add to all columns
                columnsInfo.Add(columnInfo);
            }

            return columnsInfo.OrderBy(item => item.Order);
        }

        public static IEnumerable<BindPropertyInfo> GenerateDetailsInfo<T>()
        {
            var detailsInfo = new List<BindPropertyInfo>();

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                // check should be shown this property
                if (!ShowProperty(propertyInfo) || !ShowInDetailsProperty(propertyInfo))
                    continue;

                // create column info object
                BindPropertyInfo detailInfo = null;
                Type propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

                if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string))
                    detailInfo = HandleDetailClassTypeProperty(propertyInfo);
                else if (propertyType.IsEnum)
                    detailInfo = new BindPropertyInfo
                    {
                        HasVariants = true,
                        VariantsType = propertyInfo.PropertyType,
                        DisplayMemberPath = "Description",
                        SelectedValuePath = "Value",
                        SelectedValue = propertyInfo.Name,
                        Name = propertyInfo.Name,
                        DataType = propertyInfo.PropertyType,
                        Variants = EnumHelper.ToListOfOriginalValueAndDescription(propertyType)
                    };
                else
                    detailInfo = new BindPropertyInfo
                    {
                        Name = propertyInfo.Name,
                        DataType = propertyInfo.PropertyType
                    };

                // check IsReadOnly
                detailInfo.IsReadOnly = IsReadOnlyProperty(propertyInfo);
                // check IsReadOnly on editing item
                detailInfo.IsReadOnlyOnEditing = IsReadOnlyOnEditingProperty(propertyInfo);
                // check HideOnEditingInDetails
                detailInfo.HideOnEditing = HideOnEditingInDetailsProperty(propertyInfo);
                // check IsRequired
                detailInfo.IsRequired = IsRequiredProperty(propertyInfo);
                // get validation rules
                detailInfo.ValidationRules = GetValidationRules(propertyInfo);
                // set format
                detailInfo.Format = GetPropertyFormat(propertyInfo);
                // set header
                detailInfo.Header = GetPropertyHeader(propertyInfo);
                // set order
                detailInfo.Order = GetPropertyOrder(propertyInfo);

                // add to all columns
                detailsInfo.Add(detailInfo);
            }

            return detailsInfo.OrderBy(item => item.Order);
        }

        private static SimpleBindPropertyInfo HandleColumnClassTypeProperty(PropertyInfo propertyInfo)
        {
            var columnInfo = new SimpleBindPropertyInfo();

            // get link
            var showLinkedAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is ShowLinkedAttribute) as ShowLinkedAttribute;

            if (showLinkedAttribute != null && !string.IsNullOrEmpty(showLinkedAttribute.PropertyName))
            {
                // check does property exists
                var linkedProperty = propertyInfo.PropertyType.GetProperty(showLinkedAttribute.PropertyName);
                if (linkedProperty != null)
                {
                    columnInfo.Name = propertyInfo.Name + "." + linkedProperty.Name;
                    columnInfo.DataType = linkedProperty.PropertyType;
                    return columnInfo;
                }
            }

            // default values
            columnInfo.Name = propertyInfo.Name;
            columnInfo.DataType = propertyInfo.PropertyType;

            return columnInfo;
        }

        private static BindPropertyInfo HandleDetailClassTypeProperty(PropertyInfo propertyInfo)
        {
            var detailInfo = new BindPropertyInfo
            {
                HasVariants = true,
                VariantsType = propertyInfo.PropertyType,
                SelectedValue = propertyInfo.Name,
                Name = propertyInfo.Name
            };

            // get link
            var showLinkedAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is ShowLinkedAttribute) as ShowLinkedAttribute;

            if (showLinkedAttribute != null && !string.IsNullOrEmpty(showLinkedAttribute.PropertyName))
            {
                // check does property exists
                var linkedProperty = propertyInfo.PropertyType.GetProperty(showLinkedAttribute.PropertyName);
                if (linkedProperty != null)
                {
                    detailInfo.DisplayMemberPath = linkedProperty.Name;
                    detailInfo.DataType = linkedProperty.PropertyType;
                }
            }

            // get binding settings
            var bindSelectionAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is BindSelectionAttribute) as BindSelectionAttribute;

            if (bindSelectionAttribute != null && !string.IsNullOrEmpty(bindSelectionAttribute.SourcePath))
            {
                detailInfo.SelectedValuePath = bindSelectionAttribute.SourcePath;
            }
            
            return detailInfo;
        }

        private static bool ShowProperty(PropertyInfo propertyInfo)
        {
            // get bindable attribute
            var bindableAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is BindableAttribute) as BindableAttribute;

            // check should be shown this property
            if (bindableAttribute != null)
                return bindableAttribute.Bindable;

            return true;
        }

        private static bool ShowInDataGridProperty(PropertyInfo propertyInfo)
        {
            // get bindable attribute
            var hideInDataDridAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is HideInDataDridAttribute) as HideInDataDridAttribute;

            // check should be shown this property
            if (hideInDataDridAttribute != null)
                return !hideInDataDridAttribute.Hide;

            return true;
        }

        private static bool ShowInDetailsProperty(PropertyInfo propertyInfo)
        {
            // get bindable attribute
            var hideInDataDridAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is HideInDetailsAttribute) as HideInDetailsAttribute;

            // check should be shown this property
            if (hideInDataDridAttribute != null)
                return !hideInDataDridAttribute.Hide;

            return true;
        }

        private static bool HideOnEditingInDetailsProperty(PropertyInfo propertyInfo)
        {
            // get bindable attribute
            var hideInDataDridAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is HideInDetailsOnEditingAttribute) as HideInDetailsOnEditingAttribute;

            // check should be shown this property
            if (hideInDataDridAttribute != null)
                return hideInDataDridAttribute.Hide;

            return false;
        }

        private static bool IsReadOnlyProperty(PropertyInfo propertyInfo)
        {
            // get IsReadOnly attribute
            var isReadOnlyAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is IsReadOnlyAttribute) as IsReadOnlyAttribute;

            // check should be shown this property
            if (isReadOnlyAttribute != null)
                return isReadOnlyAttribute.IsReadOnly;

            return false;
        }

        private static bool IsReadOnlyOnEditingProperty(PropertyInfo propertyInfo)
        {
            // get IsReadOnly attribute
            var isReadOnlyAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is IsReadOnlyOnEditingInDetailsAttribute) as IsReadOnlyOnEditingInDetailsAttribute;

            // check should be shown this property
            if (isReadOnlyAttribute != null)
                return isReadOnlyAttribute.IsReadOnly;

            return false;
        }

        private static IEnumerable<ValidationRule> GetValidationRules(PropertyInfo propertyInfo)
        {
            var validationRules = new List<ValidationRule>();

            if (IsRequiredProperty(propertyInfo))
            {
                validationRules.Add(new RequiredValidationRule(true));
            }

            //// get Range attribute
            //var rangeAttribute = propertyInfo.GetCustomAttributes(false)
            //        .FirstOrDefault(item => item is RangeAttribute) as RangeAttribute;

            //// get acceptable range
            //if (rangeAttribute != null)
            //    validationRules.Add(new RangeValidationRule(rangeAttribute.OperandType, rangeAttribute.Minimum, rangeAttribute.Maximum));

            return validationRules;
        }

        private static bool IsRequiredProperty(PropertyInfo propertyInfo)
        {
            // get IsRequired attribute
            var isRequiredAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is RequiredAttribute) as RequiredAttribute;

            // check should be shown this property
            if (isRequiredAttribute != null)
                return true;

            return false;
        }

        private static string GetPropertyHeader(PropertyInfo propertyInfo)
        {
            // get header
            var descriptionAttribute = propertyInfo.GetCustomAttributes(false)
                .FirstOrDefault(item => item is DescriptionAttribute) as DescriptionAttribute;

            return descriptionAttribute != null ? descriptionAttribute.Description : propertyInfo.Name;
        }

        private static int GetPropertyOrder(PropertyInfo propertyInfo)
        {
            // get order
            var orderAttribute = propertyInfo.GetCustomAttributes(false)
                .FirstOrDefault(item => item is OrderAttribute) as OrderAttribute;

            return orderAttribute != null ? orderAttribute.Order : OrderAttribute.DefaultOrder;
        }

        private static string GetPropertyFormat(PropertyInfo propertyInfo)
        {
            // get format
            var formatAttribute = propertyInfo.GetCustomAttributes(false)
                    .FirstOrDefault(item => item is FormatAttribute) as FormatAttribute;

            return formatAttribute != null ? formatAttribute.Format : null;
        }
    }
}
