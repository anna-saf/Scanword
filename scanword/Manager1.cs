using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace scanword
{
    class Words//Класс для каждого слова в сканворде
    {
        public int LenOfWord;// Длина слова
        public string definition;// Определение слова
        public string word;//Подобранное слово
        public int indexI;//Индекс начала слова
        public int indexJ;//Индекс начала слова
        public int VerticalOrHorizontal;//1 - горизонталь, 2 - вертикаль
        public int IndexWordInData;//Номер слова в базе данных
        public List<int> Peresechenia = new List<int>();// Номера слов, с которыми пересекается данное слово
        public Words(int L)
        {
            LenOfWord = L;
        }

    }
    class Manager1
    {
        public int width;
        public int heaght;
        public Button[,] buttons;
        readonly Tuple<string, string>[] data = File.ReadAllLines("cross.txt", System.Text.Encoding.Default)
                                        .OrderByDescending(s => Regex.Match(s, @"(\w*)\s-\s(\D*)").Groups[1].Value.Length)
                                        .Select(x => Tuple.Create(Regex.Match(x, @"(\w*)\s-\s(\D*)").Groups[1].Value, Regex.Match(x, @"(\w*)\s-\s(\D*)").Groups[2].Value))
                                        .ToArray();

        public int NumOfWords = 0;//Количество слов в сканворде.Точное число на 1 больше, т.к. это счетчик List, а он начинается с 0, а не 1(т.е. если слов 20, то значение поля 19,
                                    //т.к. [0,1,...,19], а не [1,2,...,20]
        public List<Words> Word = new List<Words>();//Список слов сканворда
        public char[,] Shema;// массив, который представляет весь сканворд, как сетку из char элементов. 
                            //По сути можно было и через кнопки сразу сделать, но я почему-то уже так начала.
        public Manager1(int w, int h, Button[,] b)
        {
            width = w;
            heaght = h;
            buttons = b;
        }

        public void NumOfWordsANDLenOfWords()// Подсчет количества слов и длины каждого слова

        {
            int t=1;//Переменная, которая помогает делать проверку, что перед клеткой с первой буквой слова стоит клетка с текстом "-"(в которой будет определение слова).
            Word.Add(new Words(1));//Заранее добавляю одно слово, потому что как минимум одно слово с длиной минимум 1 должно существовать.
            for (int i = 0; i < heaght; i++)//Здесь считаются горизонтальные слова, далее будут вертикальные.
            {
                for (int j = 1; j < width; j++)//Делаю счетчик от 1, потому что вначале горизонтального слова должна стоять клетка с определением.
                {
                    while ((buttons[i, j].Text != "-") && (buttons[i, j].BackColor != Color.Black)&&(buttons[i, j-t].Text == "-"))//Последняя проверка на то, что
                                                                                                                                  //перед словом есть клетка с "-"
                    {                                                                                                             //(клетка может быть черной)
                        
                        if (j < width - 1)//Т.к. длина у слова по умолчанию 1, то искусственно уменьшаем увеличение длины на 1 + делаем механизм для прерывания while.
                        {
                            Word[NumOfWords].LenOfWord++;
                            
                            j++;
                            t++;//Постоянно увеличивается, чтобы проверка клетки с "-" работала.
                        }
                        else break;
                    }
                    Word[NumOfWords].indexI = i;// запоминаем индекс i клетки с определением слова
                    Word[NumOfWords].indexJ = j - t;// запоминаем индекс j клетки с определением слова
                    Word[NumOfWords].VerticalOrHorizontal = 1;// запоминаем, что это горизонтальное слово
                    t = 1;//Приравниваем к 1 для нового слова
                    Word.Add(new Words(1));
                    NumOfWords++;
                }
            }
            
            for (int j = 0; j < width; j++) // Подсчет вертикальных слов
            {
                for (int i = 1; i < heaght; i++)
                {
                    while ((buttons[i, j].Text != "-") && (buttons[i, j].BackColor != Color.Black)&& (buttons[i-t, j].Text == "-"))
                    {
                        
                        if (i < heaght - 1)
                        {
                            Word[NumOfWords].LenOfWord++;
                            i++;
                            t++;
                        }
                        else break;
                    }
                    Word[NumOfWords].indexI = j;
                    Word[NumOfWords].indexJ = i - t;
                    Word[NumOfWords].VerticalOrHorizontal = 2;
                    t = 1;
                    Word.Add(new Words(1));
                    NumOfWords++;
                }
            }
            Word.Remove(Word.Last());// Удаление последнего лишнего элемента
        }

        public void Sort()
        {
            PeresecheniaMethod();
            Word.OrderBy(a => a.LenOfWord).ThenBy(a => a.Peresechenia.Count);//Сортирует по возрастанию слова сначала по длине, потом по пересечениям.
            Word.Reverse();//Переворачивает массив из слов, чтобы он стал по убыванию.
        }
        
        public void Generation(int w, int d)
        {
            for (int i = 0; i < Word.Count; i++)//До 121 строки: Делается, потому что после сортировки данные в листе пересечений каждого слова будут неактуальны 
            {                                   //и необходимо будет удалить эти списки и посчитать их еще раз
                Word[i].Peresechenia.Clear();
            }
            PeresecheniaMethod();
            for (int i = w; i < Word.Count; i++)// Цикл от w, потому что функция построена на рекурсии
            {

                for (int i1 = d; i1 < data.Count(); i1++)//Аналогично вышесказанному, от d
                {
                    if (Word[i].LenOfWord == data[i1].Item1.Length)//До 132 строки подбираем первое попавшееся в базе слово с такой же длиной и приравниваем к слову
                    {
                        Word[i].word = data[i1].Item1;
                        Word[i].definition = data[i1].Item2;
                        Word[i].IndexWordInData = i1;
                        if (Word[i].VerticalOrHorizontal == 1)
                        {
                            for (int j = Word[i].indexJ+1; j<Word[i].LenOfWord; j++)
                            {
                                if (Shema[Word[i].indexI, j] == '\0')
                                {
                                    Shema[Word[i].indexI, j] = Word[i].word[j - Word[i].indexJ - 1];
                                } else
                                {
                                    if (Shema[Word[i].indexI, j] == Word[i].word[j - Word[i].indexJ - 1])
                                    {
                                        
                                        continue;
                                    } else
                                    {                                     
                                        Generation(i,i1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = Word[i].indexI + 1; j < Word[i].LenOfWord; j++)
                            {
                                if (Shema[j, Word[i].indexJ] == '\0')
                                {
                                    Shema[j, Word[i].indexJ] = Word[i].word[j - Word[i].indexI - 1];
                                }
                                else
                                {
                                    if (Shema[Word[i].indexI, j] == Word[i].word[j - Word[i].indexI - 1])
                                    {

                                        continue;
                                    }
                                    else
                                    {
                                        Generation(i, i1);
                                    }
                                }
                            }
                        }

                        break;
                    }
                    if ((i1 == Word.Count)&&(Word[i].word == null))
                    {
                        int index = Word[i].Peresechenia.FindAll(a => a < i).Max();                       
                        i1 = Word[i].IndexWordInData;
                        Generation(i, i1);

                    }
                }
            }
        }
        public void PeresecheniaMethod()//Считает количество пересечений слов. Для каждого слова есть список из пересечений, каждый элемент которого - индекс пересекаемых слов
        {
            for ( int i = 0; i < Word.Count; i++)
            {
                if (Word[i].VerticalOrHorizontal == 1)
                {
                    for (int j = 0; j < Word.Count; j++)
                        if (Word[j].VerticalOrHorizontal == 2)
                        {
                            if (((Word[j].indexI + Word[j].LenOfWord) >= Word[i].indexI) && (Word[j].indexI < Word[i].indexI) &&
                                (Word[j].indexJ > Word[i].indexJ) && (Word[j].indexJ <= (Word[i].indexJ + Word[i].LenOfWord)))
                            {
                                Word[i].Peresechenia.Add(j);
                            }
                        }
                }
                else
                {
                    for (int j = 0; j < Word.Count; j++)
                        if (Word[j].VerticalOrHorizontal == 1)
                        {
                            if (((Word[j].indexI - Word[i].indexI) <= Word[i].LenOfWord) && (Word[j].indexI > Word[i].indexI) &&
                                (Word[j].indexJ < Word[i].indexJ) && (Word[i].indexJ <= (Word[j].indexJ + Word[j].LenOfWord)))
                            {
                                Word[i].Peresechenia.Add(j);
                            }

                        }
                }
            }
            
        }

        public void PasteScan(Scan b)
        {
            for (int i = 0; i <Word.Count; i++)
            {
                if (Word[i].VerticalOrHorizontal == 1)
                {
                    b.buttons[Word[i].indexI, Word[i].indexJ].Text = Convert.ToString(i);
                    for (int j = 0; j<Word[i].LenOfWord; j++)
                    {
                        b.buttons[Word[i].indexI, j].Text = Convert.ToString(Word[i].word[j]) + '\u2192';
                    }
                }
                else
                {
                    b.buttons[Word[i].indexI, Word[i].indexJ].Text = Convert.ToString(i);
                    for (int j = 0; j < Word[i].LenOfWord; j++)
                    {
                        b.buttons[i, Word[i].indexJ].Text = Convert.ToString(Word[i].word[j]) + '\u2193';
                    }
                }
            }
        }
        public void Play()
        {
            NumOfWordsANDLenOfWords();
            Sort();
            Generation(0, 0);
        }
    }
}
