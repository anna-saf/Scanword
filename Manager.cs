using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scanword
{
    public class Words
    {
        public int LenOfWords;
        public int Peresechenia;
        public Words(int l, int p)
        {
            LenOfWords = l;
            Peresechenia = p;
        }


    }
    class Manager
    {
        public StreamReader file = new StreamReader("cross.txt");
        public List<Words> ShemaWords = new List<Words>();
        public int width;
        public int heaght;
        public int[,] Shema; // Если клетка равна -1, то это номер слова, если - 2, то черный квадрат
        public int NumOfWords = 0;
        public Button[,] buttons;
        Tuple<string, string>[] data = File.ReadAllLines("cross.txt", System.Text.Encoding.Default)
                                        .OrderByDescending(s => Regex.Match(s, @"(\w*)\s-\s(\D*)").Groups[1].Value.Length)
                                        .Select(x => Tuple.Create(Regex.Match(x, @"(\w*)\s-\s(\D*)").Groups[1].Value, Regex.Match(x, @"(\w*)\s-\s(\D*)").Groups[2].Value))
                                        .ToArray();
        public Manager(int w, int h, Button[,] b)
        {
            width = w;
            heaght = h;
            buttons = b;
            Shema = new int[heaght, width];
        }

        //Подсчет количества слов и длины каждого слова 
        public void LengthOfWords(int heaght, int width, Button[,] buttons)
        {
            for (int i = 0; i < heaght; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((buttons[i, j].Text == null) && (buttons[i, j].BackColor != Color.Black))
                    {
                        ShemaWords.Add(new Words(0, 0));
                        ShemaWords[NumOfWords++].LenOfWords++;
                    }
                    else if (ShemaWords[NumOfWords].LenOfWords != 0)
                    {
                        NumOfWords++;
                    }
                }
            }
            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < heaght; i++)
                {
                    if ((buttons[i, j].Text == null) && (buttons[i, j].BackColor != Color.Black))
                    {
                        ShemaWords.Add(new Words(0, 0));
                        ShemaWords[NumOfWords].LenOfWords++;
                    }
                    else if (ShemaWords[NumOfWords].LenOfWords != 0)
                    {
                        NumOfWords++;
                    }

                }
            }
        }

        public void Sort(List<int> LenOfWords) // Сначала сортировка по пересечениям, потом - по длине
        {


            //Сортировка по сложности установки. Сложность установки – расчетный параметр. Понятно, что слова одинаковой длины
            //могут пересекаться как с одним словом, так и сразу с пятью, при этом сложность установки будет совершенно разная.


            ZapolnenieShemi();
            //заполнение схемы 0 и 1, где 1 - наличие пересечения со словом в данной ячейке, 0 - отсутствие
            for (int i = 1; i < heaght - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    if ((Shema[i, j] != -1) && (Shema[i, j] != -2))
                    {
                        if (((Shema[i + 1, j] == 0) && (Shema[i, j + 1] == 0)) || ((Shema[i + 1, j] == 0) && (Shema[i, j - 1] == 0)) || ((Shema[i - 1, j] == 0) && (Shema[i, j - 1] == 0)) || ((Shema[i - 1, j] == 0) && (Shema[i, j + 1] == 0)))
                        {
                            Shema[i, j] = 1;
                        }
                        else Shema[i, j] = 0;
                    }

                }
            }
            for (int j = 1; j < width - 2; j++)
            {
                if ((Shema[heaght - 1, j] != -1) && (Shema[heaght - 1, j] != -2))
                {
                    if (((Shema[heaght - 2, j] == 0) && (Shema[heaght - 1, j - 1] == 0)) || ((Shema[heaght - 2, j] == 0) && (Shema[heaght - 1, j + 1] == 0)))
                    {
                        Shema[heaght - 1, j] = 1;
                    }
                    else Shema[heaght - 1, j] = 0;
                }
            }
            for (int i = 1; i < heaght - 2; i++)
            {
                if ((Shema[i, width - 1] != -1) && (Shema[i, width - 1] != -2))
                {
                    if (((Shema[i, width - 2] == 0) && (Shema[i - 1, width - 1] == 0)) || ((Shema[i, width - 2] == 0) && (Shema[i + 1, width - 1] == 0)))
                    {
                        Shema[i, width - 1] = 1;
                    }
                    else Shema[i, width - 1] = 0;
                }
            }
            if ((Shema[heaght - 1, width - 1] != -1) && (Shema[heaght - 1, width - 1] != -2))
            {
                if ((Shema[heaght - 1, width - 2] == 0) && (Shema[heaght - 2, width - 1] == 0))
                {
                    Shema[heaght - 1, width - 1] = 1;
                }
                else Shema[heaght - 1, width - 1] = 0;
            }
            int k = 0;
            for (int i = 0; i < heaght; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((Shema[i, j] != -1) && (Shema[i, j] != -2))
                    {
                        ShemaWords[k].Peresechenia += Shema[i, j];
                    }
                    else if (ShemaWords[k].Peresechenia != 0) k++;

                }
            }
            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < heaght; i++)
                {
                    if ((Shema[i, j] != -1) && (Shema[i, j] != -2))
                    {
                        ShemaWords[k].Peresechenia += Shema[i, j];
                    }
                    else if (ShemaWords[k].Peresechenia != 0) k++;
                }
            }
            ShemaWords.OrderBy(a => a.LenOfWords).ThenBy(b => b.Peresechenia);
            ShemaWords.Reverse();
        }





        public void Fragments()
        {

        }

    }
}