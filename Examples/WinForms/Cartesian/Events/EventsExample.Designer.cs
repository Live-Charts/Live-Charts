namespace Winforms.Cartesian.Events
{
    partial class EventsExample
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
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartesianChart1.Location = new System.Drawing.Point(0, 0);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(382, 343);
            this.cartesianChart1.TabIndex = 0;
            this.cartesianChart1.Text = "cartesianChart1";
            this.cartesianChart1.DataClick += new LiveCharts.Events.DataClickHandler(this.ChartOnDataClick);
            this.cartesianChart1.DataHover += new LiveCharts.Events.DataHoverHandler(this.Chart_OnDataHover);
            this.cartesianChart1.UpdaterTick += new LiveCharts.Events.UpdaterTickHandler(this.ChartOnUpdaterTick);
            // 
            // EventsExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 343);
            this.Controls.Add(this.cartesianChart1);
            this.Name = "EventsExample";
            this.Text = "EventsExample";
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart cartesianChart1;
    }
}