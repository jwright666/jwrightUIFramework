﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jwrightUIFramework.UI
{
    public partial class AbstractForm : Form
    {
        public AbstractForm()
        {
            InitializeComponent();
        }

        private void AbstractForm_Load(object sender, EventArgs e)
        {
            if (!this.Name.ToString().Equals("frmLogin"))
                setAllTextBoxToUppercase(this);
            assignformKeyEvent(this);
        }

        #region Validate all Inputs
        protected bool isAlphaNumeric(string strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("^[A-Za-z0-9]*$");
            return objAlphaNumericPattern.IsMatch(strToCheck);
        }

        protected bool isDate(string strToCheck)
        {
            DateTime dateValue;
            return DateTime.TryParse(strToCheck, out dateValue);
        }

        protected bool isInteger(string strToCheck)
        {
            if (strToCheck.Trim().Equals(string.Empty))
                return false;

            if (strToCheck.StartsWith("-"))
                strToCheck = strToCheck.Substring(1, strToCheck.Length - 1);

            Regex objNumericPattern = new Regex("^([0-9]+(, +)?)+$");
            return objNumericPattern.IsMatch(strToCheck);
        }

        protected bool isIntegerDecimal(string strToCheck)
        {
            if (strToCheck.Trim().Equals(string.Empty))
                return false;

            if (strToCheck.StartsWith("-"))
                strToCheck = strToCheck.Substring(1, strToCheck.Length - 1);

            Regex objNumericPattern = new Regex("^([\\d])+(.[0]+)?$");
            return objNumericPattern.IsMatch(strToCheck);
        }

        protected bool isNumeric(string strToCheck)
        {
            if (strToCheck.Trim().Equals(string.Empty) || strToCheck.StartsWith("-") || strToCheck.StartsWith(".") || strToCheck.StartsWith(","))
                return false;

            Regex objNumercPattern = new Regex("^([0-9]*(, *)?)*[.]?[0-9]*$");
            return objNumercPattern.IsMatch(strToCheck);
        }
        #endregion

        #region Textbox and other Controls Settings
        protected void setAllTextBoxToUppercase(Control control)
        {
            if (control.HasChildren && (control as DataGridView) == null)
            {
                foreach (Control childControl in control.Controls)
                {
                    setAllTextBoxToUppercase(childControl);
                }
            }
            else
            {
                if (control is TextBox)
                {
                    TextBox obj = control as TextBox;
                    if (obj.PasswordChar.Equals('\0') && !obj.Name.Equals("txtEmailAdd"))
                        (control as TextBox).CharacterCasing = CharacterCasing.Upper;
                }
                if (control is DataGridView)
                {
                    control.BackColor = Color.DodgerBlue;
                    (control as DataGridView).SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    foreach (DataGridViewColumn col in (control as DataGridView).Columns)
                    {
                        //20140701 - gerry modified to format date and time columns
                        if (col.ValueType == typeof(DateTime))
                        {
                            if (col.Name.Contains("DateTime") || col.Name.Contains("dateTime"))
                                col.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                            else if (col.Name.Contains("Time") || col.Name.Contains("time"))
                                col.DefaultCellStyle.Format = "HH:mm";
                            else if (col.Name.Contains("Date") || col.Name.Contains("date"))
                                col.DefaultCellStyle.Format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

                            //20140624 - gerry added display null values
                            //col.DefaultCellStyle.Format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;// "dd/MM/yyyy";
                        }
                        else if (col.ValueType == typeof(Decimal))
                        {
                            col.DefaultCellStyle.Format = "#0.00";
                            if (col.Name.Contains("Vol") || col.Name.Contains("vol"))
                                col.DefaultCellStyle.Format = "#0.0000";
                        }
                    }
                }
                if (control is DateTimePicker)
                {
                    ((DateTimePicker)control).ValueChanged += delegate(object sender, EventArgs e)
                    {
                        ((DateTimePicker)sender).Format = DateTimePickerFormat.Custom;
                        if (((DateTimePicker)sender).Value == new DateTime(1900, 1, 1, 0, 0, 0))
                        {
                            ((DateTimePicker)sender).CustomFormat = " ";
                        }
                        else
                        {
                            if (((DateTimePicker)sender).Name.Contains("Time") || ((DateTimePicker)sender).Name.Contains("time"))
                                ((DateTimePicker)sender).CustomFormat = "HH:mm";
                            else if (((DateTimePicker)sender).Name.Contains("Date") || ((DateTimePicker)sender).Name.Contains("date"))
                                ((DateTimePicker)sender).CustomFormat = "dd/MM/yyyy";
                            else if (((DateTimePicker)sender).Name.Contains("DateTime") || ((DateTimePicker)sender).Name.Contains("dateTime"))
                                ((DateTimePicker)sender).CustomFormat = "dd/MM/yyyy hh:mm";
                        }
                    };
                }
            }
        }

        protected void clearTextBoxes(Control control)
        {
            if (control is TextBox)
            {
                (control as TextBox).Clear();
            }

            else if (control.HasChildren)
            {
                foreach (Control child in control.Controls)
                {
                    clearTextBoxes(child);
                }
            }
        }

        protected void setControlFocus(Control obj)
        {
            obj.Focus();
        }

        protected void setControlBackgroundColor(Control control, bool compulsory, bool readOnly)
        {
            if (compulsory)
                control.BackColor = Color.FromArgb(150, 190, 255);
            else
            {
                if (readOnly)
                    control.BackColor = Color.FromArgb(180, 220, 180);
                else
                    control.BackColor = DefaultBackColor;
            }
        }
        #endregion

        #region Get Values
        // this will return the value as string. If null it will return String.Empty
        protected string GetValue(object objVal)
        {
            if (objVal != null)
                return objVal.ToString().Trim();
            else
                return String.Empty;
        }

        protected Int32 getIntValue(object obj)
        {
            Int32 retval = 0;
            try
            {
                retval = Convert.ToInt32(obj.ToString().Replace(",", "").Split('.')[0]);
            }
            catch (Exception)
            {
                retval = 0;
            }
            return retval;
        }

        protected decimal getDecimalValue(object objVal)
        {
            Decimal retval = Convert.ToDecimal(0.0);
            try
            {
                retval = Convert.ToDecimal(objVal);
            }
            catch (Exception)
            {
                retval = Convert.ToDecimal(0.0);
            }
            return retval;
        }
        #endregion

        #region KeyHandler Events
        protected virtual void assignformKeyEvent(Control recepient)
        {
            recepient.KeyDown += new KeyEventHandler(formKeyDownHandler);
            recepient.KeyPress += new KeyPressEventHandler(formKeyPressHandler);
            if (recepient.HasChildren)
            {
                foreach (Control ctrl in recepient.Controls)
                {
                    assignformKeyEvent(ctrl);
                    if (ctrl is ComboBox)
                    {
                        ctrl.Text.ToUpper();
                    }
                }
            }
        }

        protected virtual void formKeyDownHandler(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape: this.Close(); break;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Alert"); }
        }
        protected virtual void formKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (sender is TextBox)
                {
                    TextBox txtBox = sender as TextBox;
                    if (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == ' ')
                    {
                        int maxLength = txtBox.MaxLength;
                        if (txtBox.TextLength == maxLength && txtBox.SelectionLength == 0)
                            throw new Exception(string.Format("You've keyin more {0} characters which is the maximum allowed length.", maxLength.ToString()));

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Alert"); }
        }

        protected virtual void NumbersOnlyTextBox(object sender, KeyPressEventArgs e)
        {
            string decimalPoint = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            TextBox txtBox = (TextBox)sender;
            if (e.KeyChar == '-')
            {
                if (txtBox.SelectionStart > 0)
                    e.Handled = true;
            }
            else
            {
                if (decimalPoint == ".")
                {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                    {
                        e.Handled = true;
                    }
                    if (e.KeyChar == '.')
                    {
                        if (txtBox.Text.IndexOf(".") > -1)
                            e.Handled = true;
                        if (txtBox.Text.Length < 1)
                            txtBox.Text = "0";
                        txtBox.SelectionStart = txtBox.Text.Length;
                    }
                    if (char.IsControl(e.KeyChar))
                    {
                        if (txtBox.Text.IndexOf(".") > -1 && txtBox.Text.Substring(txtBox.Text.IndexOf(".")).Length > 4)
                        {
                            e.Handled = true;
                        }
                    }
                }
                else if (decimalPoint == ",")
                {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
                    {
                        e.Handled = true;
                    }
                    if (e.KeyChar == ',')
                    {
                        if (txtBox.Text.IndexOf(".") > -1)
                            e.Handled = true;
                        if (txtBox.Text.Length < 1)
                            txtBox.Text = "0";
                        txtBox.SelectionStart = txtBox.Text.Length;
                    }
                    if (char.IsControl(e.KeyChar))
                    {
                        if (txtBox.Text.IndexOf(",") > -1 && txtBox.Text.Substring(txtBox.Text.IndexOf(",")).Length > 4)
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
            if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = false;
            }
        }

        public virtual void AutoCompleteComboBox(object sender, KeyPressEventArgs e, bool blnLimitToList)
        {
            ComboBox cb = (ComboBox)sender;
            cb.DropDownStyle = ComboBoxStyle.DropDown;
            cb.DroppedDown = true;
            string strFindStr = "";
            if (e.KeyChar == (char)8)
            {
                if (cb.SelectionStart <= 1)
                {
                    cb.Text = "";
                    return;
                }

                if (cb.SelectionLength == 0)
                    strFindStr = cb.Text.Substring(0, cb.Text.Length - 1);
                else
                    strFindStr = cb.Text.Substring(0, cb.SelectionStart - 1);
            }
            else
            {
                if (e.KeyChar == '\r')
                    return;

                if (cb.SelectionLength == 0)
                    strFindStr = cb.Text + e.KeyChar;
                else
                    strFindStr = cb.Text.Substring(0, cb.SelectionStart) + e.KeyChar;
            }

            int intIdx = -1;

            // Search the string in the ComboBox list.

            intIdx = cb.FindString(strFindStr);

            if (intIdx != -1)
            {
                cb.SelectedText = "";
                cb.SelectedIndex = intIdx;
                cb.SelectionStart = strFindStr.Length;
                cb.SelectionLength = cb.Text.Length;
                e.Handled = true;
            }
            else
            {
                e.Handled = blnLimitToList;
            }
        }
        protected virtual void ValidTimeOnly(object sender, KeyPressEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            if (e.KeyChar != '\b' && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                if (e.KeyChar != '\b')
                {
                    int input = Convert.ToInt32(e.KeyChar.ToString());
                    switch (txtBox.SelectionStart)
                    {
                        case 0:
                            if (input > 2)
                                e.Handled = true;
                            break;
                        case 1:
                            if ((Convert.ToInt32(txtBox.Text.Substring(0, 1)) > 1) && (input > 3))
                                e.Handled = true;
                            break;
                        case 2:
                            if (input > 5)
                                e.Handled = true;
                            break;
                    }
                }
            }
        }
        #endregion


        #region Not yet in use
        public void TextOnChanged(object sender, EventArgs e)
        {
            //This handles if they paste crap in the textbox since that doesn't fire the KeyDown event
            TextBox tb = (sender as TextBox);
            decimal i;
            if (!string.IsNullOrEmpty(tb.Text) && !decimal.TryParse(tb.Text, out i))
            {
                StringBuilder sb = new StringBuilder();
                for (int i1 = 0; i1 < tb.Text.Length; i1++)
                {
                    if (char.IsNumber(tb.Text[i1]))
                        sb.Append(tb.Text[i1]);
                }
                tb.Text = sb.ToString();
            }
        }
        private void DrawMultiColComboBox(object sender)
        {
            ((ComboBox)sender).DrawMode = DrawMode.OwnerDrawFixed;

            ((ComboBox)sender).DrawItem += delegate(object cmb, DrawItemEventArgs args)
            {
                args.DrawBackground();

                DataRowView drv = (DataRowView)(((ComboBox)sender).Items[args.Index]);

                string id = drv["Code"].ToString();
                string name = drv["Name"].ToString();

                Rectangle r1 = args.Bounds;
                r1.Width /= 2;
                r1.Width = r1.Width - 65;

                using (SolidBrush sb = new SolidBrush(args.ForeColor))
                {
                    args.Graphics.DrawString(id, args.Font, sb, r1);
                }

                using (Pen p = new Pen(Color.Black))
                {
                    args.Graphics.DrawLine(p, r1.Right, 0, r1.Right, r1.Bottom);
                }

                Rectangle r2 = args.Bounds;
                r2.X = args.Bounds.Width / 2 - 65;

                r2.Width /= 2;
                r2.Width = r2.Width + 65;

                using (SolidBrush sb = new SolidBrush(args.ForeColor))
                {
                    args.Graphics.DrawString(name, args.Font, sb, r2);
                }
            };
        }
        #endregion
    }
}
