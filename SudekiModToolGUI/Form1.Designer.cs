namespace SudekiModToolGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnCheckConflicts;
        private System.Windows.Forms.Button btnMergeMods;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.TextBox textBox1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnCheckConflicts = new System.Windows.Forms.Button();
            this.btnMergeMods = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();

            // btnCheckConflicts
            this.btnCheckConflicts.Location = new System.Drawing.Point(12, 12);
            this.btnCheckConflicts.Name = "btnCheckConflicts";
            this.btnCheckConflicts.Size = new System.Drawing.Size(180, 40);
            this.btnCheckConflicts.TabIndex = 2;
            this.btnCheckConflicts.Text = "Check Conflicts";
            this.btnCheckConflicts.Click += new System.EventHandler(this.btnCheckConflicts_Click);
            // btnMergeMods
            this.btnMergeMods.Location = new System.Drawing.Point(200, 12);
            this.btnMergeMods.Name = "btnMergeMods";
            this.btnMergeMods.Size = new System.Drawing.Size(180, 40);
            this.btnMergeMods.TabIndex = 1;
            this.btnMergeMods.Text = "Merge Mods";
            this.btnMergeMods.Click += new System.EventHandler(this.btnMergeMods_Click);
            // txtOutput
            this.txtOutput.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtOutput.Location = new System.Drawing.Point(12, 60);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(459, 466);
            this.txtOutput.TabIndex = 0;
            // Form1
            this.ClientSize = new System.Drawing.Size(784, 538);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnMergeMods);
            this.Controls.Add(this.btnCheckConflicts);
            this.Name = "Form1";
            this.Text = "Sudeki Mod Merger v1.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

