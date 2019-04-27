using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scanword
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Begin b = new Begin();
            Application.Run(b);
            Scan scan = new Scan(b.heigth, b.width);
            Application.Run(scan);
            Manager1 Scanword = new Manager1(scan.width, scan.height, scan.buttons);
            Scanword.Play();
            Scanword.PasteScan(scan);
            Application.Run(scan);
        }
    }
}
