﻿using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MixERP.Net.Common;
using MixERP.Net.Common.jQueryHelper;
using MixERP.Net.i18n.Resources;
using MixERP.Net.WebControls.ScrudFactory.Helpers;

namespace MixERP.Net.WebControls.ScrudFactory.Controls.TextBoxes
{
    internal static class ScrudDateTextBox
    {
        public static string GetLocalizedDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            DateTime date = Conversion.TryCastDate(value);

            if (date.Date == date)
            {
                return date.ToString("d");
            }

            return date.ToString("f");
        }

        internal static void AddDateTextBox(HtmlTable htmlTable, string resourceClassName, string columnName, string label, string description,
            string defaultValue, bool isNullable, string validatorCssClass, bool disabled)
        {
            if (htmlTable == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(columnName))
            {
                return;
            }

            string id = columnName + "_textbox";

            using (TextBox textBox = ScrudTextBox.GetTextBox(id, 100))
            {
                jQueryUI.AddjQueryUIDatePicker(null, id, null, null);

                if (string.IsNullOrWhiteSpace(label))
                {
                    label = ScrudLocalizationHelper.GetResourceString(resourceClassName, columnName);
                }

                textBox.Text = GetLocalizedDate(defaultValue);
                textBox.ReadOnly = disabled;
                textBox.CssClass = "date";

                if (!string.IsNullOrWhiteSpace(description))
                {
                    textBox.CssClass += " activating element";
                    textBox.Attributes.Add("data-content", description);
                }

                using (CompareValidator dateValidator = GetDateValidator(textBox, validatorCssClass))
                {
                    if (!isNullable)
                    {
                        using (
                            RequiredFieldValidator required = ScrudFactoryHelper.GetRequiredFieldValidator(textBox,
                                validatorCssClass))
                        {
                            ScrudFactoryHelper.AddRow(htmlTable, label + Labels.RequiredFieldIndicator, textBox,
                                dateValidator, required);
                            return;
                        }
                    }

                    ScrudFactoryHelper.AddRow(htmlTable, label, textBox, dateValidator);
                }
            }
        }

        private static CompareValidator GetDateValidator(Control controlToValidate, string cssClass)
        {
            if (controlToValidate == null)
            {
                return null;
            }

            using (CompareValidator validator = new CompareValidator())
            {
                validator.ID = controlToValidate.ID + "CompareValidator";
                validator.ErrorMessage = @"<br/>" + Titles.InvalidDate;
                validator.CssClass = cssClass;
                validator.ControlToValidate = controlToValidate.ID;
                validator.EnableClientScript = true;
                validator.SetFocusOnError = true;
                validator.Display = ValidatorDisplay.Dynamic;
                validator.Type = ValidationDataType.Date;
                validator.Operator = ValidationCompareOperator.DataTypeCheck;

                return validator;
            }
        }
    }
}