namespace RawInputApi.Views
{
    partial class TestView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label selectLabel;
            System.Windows.Forms.Label confirmLabel;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
            this.textBox = new System.Windows.Forms.TextBox();
            this.comboBox = new System.Windows.Forms.ComboBox();
            selectLabel = new System.Windows.Forms.Label();
            confirmLabel = new System.Windows.Forms.Label();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectLabel
            // 
            selectLabel.AutoSize = true;
            selectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            selectLabel.Location = new System.Drawing.Point(20, 10);
            selectLabel.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            selectLabel.Name = "selectLabel";
            selectLabel.Size = new System.Drawing.Size(262, 15);
            selectLabel.TabIndex = 23;
            selectLabel.Text = "Select a keyboard for which to capture all input:";
            selectLabel.UseMnemonic = false;
            // 
            // confirmLabel
            // 
            confirmLabel.AutoSize = true;
            confirmLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            confirmLabel.Location = new System.Drawing.Point(20, 66);
            confirmLabel.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            confirmLabel.Name = "confirmLabel";
            confirmLabel.Size = new System.Drawing.Size(442, 30);
            confirmLabel.TabIndex = 28;
            confirmLabel.Text = "Confirm that all keys entered on the above keyboard, and no keys from any other k" +
    "eyboard, appear below:";
            confirmLabel.UseMnemonic = false;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(this.textBox, 0, 3);
            tableLayoutPanel.Controls.Add(selectLabel, 0, 0);
            tableLayoutPanel.Controls.Add(confirmLabel, 0, 2);
            tableLayoutPanel.Controls.Add(this.comboBox, 0, 1);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.Padding = new System.Windows.Forms.Padding(10);
            tableLayoutPanel.RowCount = 4;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Size = new System.Drawing.Size(491, 350);
            tableLayoutPanel.TabIndex = 29;
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Enabled = false;
            this.textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(20, 106);
            this.textBox.Margin = new System.Windows.Forms.Padding(10);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(451, 224);
            this.textBox.TabIndex = 27;
            // 
            // comboBox
            // 
            this.comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(20, 35);
            this.comboBox.Margin = new System.Windows.Forms.Padding(10);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(451, 21);
            this.comboBox.TabIndex = 29;
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // TestView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 350);
            this.Controls.Add(tableLayoutPanel);
            this.Icon = global::RawInputApi.Properties.Resources.Icon;
            this.Name = "TestView";
            this.Text = "Raw Input API Keyboard Test";
            this.Shown += new System.EventHandler(this.TestView_Shown);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.ComboBox comboBox;
    }
}

