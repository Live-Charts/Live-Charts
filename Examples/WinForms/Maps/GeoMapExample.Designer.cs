namespace Winforms.Maps
{
    partial class GeoMapExample
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
            this.geoMap1 = new LiveCharts.WinForms.GeoMap();
            this.SuspendLayout();
            // 
            // geoMap1
            // 
            this.geoMap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.geoMap1.Location = new System.Drawing.Point(0, 0);
            this.geoMap1.Name = "geoMap1";
            this.geoMap1.Size = new System.Drawing.Size(695, 469);
            this.geoMap1.TabIndex = 0;
            this.geoMap1.Text = "geoMap1";
            // 
            // GeoMapExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(695, 469);
            this.Controls.Add(this.geoMap1);
            this.Name = "GeoMapExample";
            this.Text = "GeoMapExample";
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.GeoMap geoMap1;
    }
}