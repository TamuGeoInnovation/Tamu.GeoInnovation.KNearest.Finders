using System;
using System.Collections.Generic;
using System.Windows.Forms;
using USC.GISResearchLab.Common.Databases.FieldMappings;
using USC.GISResearchLab.Common.Databases.SchemaManagers;
using USC.GISResearchLab.Common.Databases.TypeConverters;
using USC.GISResearchLab.Common.FieldMappings;

namespace USC.GISResearchLab.Common.KNearest.Panels.OutputFieldPanels
{
    public partial class FrmOutputKNearestFieldsPanel : Form
    {

        public ISchemaManager SchemaManager { get; set; }
        public string TableName { get; set; }
        public OutputDatabaseFieldMappings OutputFieldMappings { get; set; }
        public string Prefix { get; set; }
        public string FormName { get; set; }
        public bool IsDataBound { get; set; }

        public FrmOutputKNearestFieldsPanel(OutputDatabaseFieldMappings outputFieldMappings, string prefix)
        {
            OutputFieldMappings = outputFieldMappings;
            Prefix = prefix;
            InitializeComponent();

        }

        public void BindToConfiguration()
        {
            try
            {

                if (!IsDataBound)
                {

                    chkInclude.DataBindings.Add("Checked", OutputFieldMappings, "Enabled", true, DataSourceUpdateMode.OnPropertyChanged);

                    BindComboBox(cboStateFips, OutputFieldMappings.GetFieldMapping("StateFips"));
                    BindComboBox(cboCountyFips, OutputFieldMappings.GetFieldMapping("CountyFips"));
                    BindComboBox(cboCBSAFips, OutputFieldMappings.GetFieldMapping("CBSAFips"));
                    BindComboBox(cboCBSAMicro, OutputFieldMappings.GetFieldMapping("CBSAMicro"));
                    BindComboBox(cboMCDFips, OutputFieldMappings.GetFieldMapping("MCDFips"));
                    BindComboBox(cboMetDivFips, OutputFieldMappings.GetFieldMapping("MetDivFips"));
                    BindComboBox(cboMSAFips, OutputFieldMappings.GetFieldMapping("MSAFips"));
                    BindComboBox(cboPlaceFips, OutputFieldMappings.GetFieldMapping("PlaceFips"));
                    BindComboBox(cboTract, OutputFieldMappings.GetFieldMapping("Tract"));
                    BindComboBox(cboBlockGroup, OutputFieldMappings.GetFieldMapping("BlockGroup"));
                    BindComboBox(cboBlock, OutputFieldMappings.GetFieldMapping("Block"));

                    IsDataBound = true;
                }
            }
            catch (Exception ex)
            {
                string message = "Error binding to configuration:";
                message += Environment.NewLine;
                message += ex.Message;

                MessageBox.Show(this, message, "Exception Occurred", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
            }
        }


        public void SetAllComboBoxItems()
        {
            string[] columns = SchemaManager.GetColumnNames(TableName);
            foreach (Control control in Controls)
            {
                if (control.GetType() == typeof(ComboBox))
                {
                    SetComboBoxItems((ComboBox)control, columns);
                }
            }
        }

        public void SetComboBoxItems(ComboBox cbo, string[] items)
        {
            cbo.Items.Clear();
            cbo.Items.AddRange(items);
        }


        public void BindComboBox(ComboBox cbo, object dataSource)
        {
            cbo.DataBindings.Add("Text", dataSource, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            SetSelectedComboBoxItem(cbo, txtPrefix.Text + Prefix + ((FieldMapping)dataSource).DefaultValue);
        }

        public void SetSelectedComboBoxItem(ComboBox cbo, string value)
        {
            foreach (string item in cbo.Items)
            {
                if (String.Compare(item, value, true) == 0)
                {
                    cbo.SelectedItem = item;
                    break;
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> columnNames = new List<string>();
                List<int> columnMaxLengths = new List<int>();
                List<int> columnPrecisions = new List<int>();
                List<DatabaseSuperDataType> dataTypes = new List<DatabaseSuperDataType>();

                foreach (DatabaseFieldMapping fieldMapping in OutputFieldMappings.FieldMappings)
                {
                    columnNames.Add(txtPrefix.Text + Prefix + fieldMapping.Name);
                    columnMaxLengths.Add(fieldMapping.MaxLength);
                    columnPrecisions.Add(fieldMapping.Precision);
                    dataTypes.Add(fieldMapping.Type);
                }

                if (chkInclude.Checked)
                {
                    SchemaManager.AddColumnsToTable(TableName, columnNames, dataTypes, columnMaxLengths, columnPrecisions, true);
                    SetAllComboBoxItems();

                    SetSelectedComboBoxItem(cboStateFips, txtPrefix.Text + Prefix + "StateFips");
                    SetSelectedComboBoxItem(cboCountyFips, txtPrefix.Text + Prefix + "CountyFips");
                    SetSelectedComboBoxItem(cboTract, txtPrefix.Text + Prefix + "Tract");
                    SetSelectedComboBoxItem(cboBlockGroup, txtPrefix.Text + Prefix + "BlockGroup");
                    SetSelectedComboBoxItem(cboBlock, txtPrefix.Text + Prefix + "Block");
                    SetSelectedComboBoxItem(cboCBSAFips, txtPrefix.Text + Prefix + "CBSAFips");
                    SetSelectedComboBoxItem(cboCBSAMicro, txtPrefix.Text + Prefix + "CBSAMicro");
                    SetSelectedComboBoxItem(cboMCDFips, txtPrefix.Text + Prefix + "MCDFips");
                    SetSelectedComboBoxItem(cboMetDivFips, txtPrefix.Text + Prefix + "MetDivFips");
                    SetSelectedComboBoxItem(cboMSAFips, txtPrefix.Text + Prefix + "MSAFips");
                    SetSelectedComboBoxItem(cboPlaceFips, txtPrefix.Text + Prefix + "PlaceFips");
                }
            }
            catch (Exception ex)
            {
                string message = "Error creating fields:";
                message += Environment.NewLine;
                message += ex.Message;

                MessageBox.Show(this, message, "Exception Occurred", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
            }
        }

        private void FrmOutputAddressFieldsPanel_Load(object sender, EventArgs e)
        {
            SetAllComboBoxItems();
            BindToConfiguration();
            this.Text = FormName + " Output Fields";
        }

        private void FrmOutputAddressFieldsPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
