namespace sklad
{
    partial class FormularPolozky
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
            label1 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label2 = new Label();
            label3 = new Label();
            textBox3 = new TextBox();
            label4 = new Label();
            label5 = new Label();
            textBox4 = new TextBox();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            button1 = new Button();
            button2 = new Button();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            comboBox3 = new ComboBox();
            textBox5 = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 0;
            label1.Text = "Číslo položky ";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(186, 6);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(280, 27);
            textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(186, 39);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(280, 27);
            textBox2.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 42);
            label2.Name = "label2";
            label2.Size = new Size(50, 20);
            label2.TabIndex = 2;
            label2.Text = "Název";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 75);
            label3.Name = "label3";
            label3.Size = new Size(74, 20);
            label3.TabIndex = 4;
            label3.Text = "Kategorie";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(186, 101);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(280, 27);
            textBox3.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 108);
            label4.Name = "label4";
            label4.Size = new Size(168, 20);
            label4.TabIndex = 6;
            label4.Text = "Počet položek na skladu";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 141);
            label5.Name = "label5";
            label5.Size = new Size(113, 20);
            label5.TabIndex = 8;
            label5.Text = "Měrná jednotka";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(186, 167);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(280, 27);
            textBox4.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 174);
            label6.Name = "label6";
            label6.Size = new Size(72, 20);
            label6.TabIndex = 10;
            label6.Text = "Minimum";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 207);
            label7.Name = "label7";
            label7.Size = new Size(67, 20);
            label7.TabIndex = 12;
            label7.Text = "Umístění";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 240);
            label8.Name = "label8";
            label8.Size = new Size(42, 20);
            label8.TabIndex = 14;
            label8.Text = "Cena";
            // 
            // button1
            // 
            button1.Location = new Point(12, 295);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 16;
            button1.Text = "Přidat";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(385, 295);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 17;
            button2.Text = "Strono";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Ruční nářadí", "Jističe ", "Kabely", "Spojovací material ", "Rozvadeče ", "Lišty" });
            comboBox1.Location = new Point(186, 72);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(280, 28);
            comboBox1.TabIndex = 18;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "Ks", "M", "Kg" });
            comboBox2.Location = new Point(186, 134);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(280, 28);
            comboBox2.TabIndex = 19;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "Sklad 1", "Sklad 2" });
            comboBox3.Location = new Point(186, 199);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(280, 28);
            comboBox3.TabIndex = 20;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(186, 233);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(280, 27);
            textBox5.TabIndex = 21;
            // 
            // FormularPolozky
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(491, 336);
            Controls.Add(textBox5);
            Controls.Add(comboBox3);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(textBox4);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(textBox3);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Name = "FormularPolozky";
            Text = "FormularPolozky";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label2;
        private Label label3;
        private ComboBox comboBox1;
        private Label label4;
        private TextBox textBox3;
        private Label label5;
        private ComboBox comboBox2;
        private Label label6;
        private Label label7;
        private ComboBox comboBox3;
        private Label label8;
        private TextBox textBox4;
        private Button button1;
        private Button button2;
        private TextBox textBox5;
    }
}