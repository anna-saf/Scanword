using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace scanword
{
    public partial class Scan : Form
    {
        public int height;
        public int width;
        public Button[,] buttons;
        TableLayoutPanel tableLayoutPanel3;
        GroupBox box;
        Button button_ok;
        public Scan(uint h, uint w)
        {
            height = Convert.ToInt32(h);
            width = Convert.ToInt32(w);
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(width*50,height*60);
            buttons = new Button[h,w];
        }

        private void Scan_Load(object sender, EventArgs e)
        {
            BuildGroupBox();
            BuildTableLayoutPanel();
            BuildButtons();
            BuildButtonOk();
        }
        private void BuildButtonOk()
        {
            button_ok = new Button();
            button_ok.Location = new Point(width * 50 - button_ok.Size.Width-10 , height * 60 - button_ok.Size.Height-10);
            button_ok.Text = "ok";
            button_ok.Click += button_ClickOk;
            Controls.Add(button_ok);
        }
        private void BuildGroupBox()
        {
            box = new GroupBox();
            box.Size = new Size(width * 50, height * 50);
            box.Name = "TablePanel";
            Controls.Add(box);
        }
        private void BuildTableLayoutPanel()
        {
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel3.ColumnCount = width;
            tableLayoutPanel3.Dock = DockStyle.Fill;
            for (int i = 0; i<width;i++)
                tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = height;
            for (int i = 0; i < height; i++)
                tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel3.TabIndex = 0;
            tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            box.Controls.Add(tableLayoutPanel3);
        }
        private void BuildButtons()
        {
            for (int i=0;i<height; i++)
                for (int j = 0; j < width; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Dock = DockStyle.Fill;
                    buttons[i, j].Margin = new Padding(0);
                    buttons[i, j].MouseDown += button_Click;
                    tableLayoutPanel3.Controls.Add(buttons[i,j]);
                }
        }
        private void button_ClickOk(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (((Button)sender).BackColor != Color.Black)
                    ((Button)sender).BackColor = Color.Black;
                else
                    ((Button)sender).BackColor = Color.Empty;
            }
            else
            {
                if (((Button)sender).Text != "-")
                    ((Button)sender).Text = "-";
                else
                    ((Button)sender).Text = null;
            }
        }
    }
    
}
