namespace Winforms.Gauge._180
{
    partial class Gauge180Example
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
            this.gauge1 = new LiveCharts.WinForms.Gauge();
            this.SuspendLayout();
            // 
            // gauge1
            // 
            this.gauge1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gauge1.Location = new System.Drawing.Point(0, 0);
            this.gauge1.Name = "gauge1";
            this.gauge1.Size = new System.Drawing.Size(284, 261);
            this.gauge1.TabIndex = 0;
            this.gauge1.Text = "gauge1";
            // 
            // Gauge180Example
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.gauge1);
            this.Name = "Gauge180Example";
            this.Text = "Gauge180Example";
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.Gauge gauge1;
    }
}