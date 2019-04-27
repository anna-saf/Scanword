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
    public partial class PressButton : Form
    {
        public int number;
        public object send;
        public PressButton(object s)
        {
            send = s;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxNumber.Text, out number))
            {
                this.Close();
            }
            else
                MessageBox.Show("Введите правильно данные!");
        }
    }
}
