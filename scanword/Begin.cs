using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scanword
{
    public partial class Begin : Form
    {
        public uint heigth = 10;
        public uint width = 10;
        public Begin()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (uint.TryParse(textBoxWidth.Text, out width) && uint.TryParse(textBoxHeight.Text, out heigth))
            {
                this.Close();
            }
            else
                MessageBox.Show("Введите правильно данные!");
        }
    }
}
