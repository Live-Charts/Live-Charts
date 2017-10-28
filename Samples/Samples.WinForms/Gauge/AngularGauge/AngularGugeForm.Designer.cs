namespace Winforms.Gauge.AngularGauge
{
    partial class AngularGugeForm
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
            this.angularGauge1 = new LiveCharts.WinForms.AngularGauge();
            this.SuspendLayout();
            // 
            // angularGauge1
            // 
            this.angularGauge1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.angularGauge1.Location = new System.Drawing.Point(0, 0);
            this.angularGauge1.Name = "angularGauge1";
            this.angularGauge1.Size = new System.Drawing.Size(624, 439);
            this.angularGauge1.TabIndex = 0;
            this.angularGauge1.Text = "angularGauge1";
            // 
            // AngularGugeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(624, 439);
            this.Controls.Add(this.angularGauge1);
            this.Name = "AngularGugeForm";
            this.Text = "AngularGugeForm";
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.AngularGauge angularGauge1;
    }
}