using System;
using System.Collections.Generic;

namespace Matrix
{
    class Matrix
    {
        public Matrix(int[,] given)
        {
            Values = given;
            Rows = given.GetLength(0);
            Columns = given.GetLength(1);
        }
        public Matrix(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
                throw new WrongRowcolumnException();
            Rows = rows;
            Columns = columns;
            Values = new int[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    Values[i, j] = 0;
        }
        public int[,] Values { get; set; }
        public int Rows { get; }
        public int Columns { get; }

        public void Generate()
        {
            Random random = new Random();
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    Values[i, j] = random.Next(0, 10); 
        }
        public override string ToString()
        {
            string toReturn = "";
            for (int i = 0; i < Rows; i++)
            {
                toReturn += "[";
                for (int j = 0; j < Columns; j++)
                    toReturn += Values[i, j].ToString() + " ";
                toReturn += "]\n";
            }
            return toReturn;
        }
        public int this[int i, int j]
        {
            get
            {
                if (i < 0 || i >= Rows)
                    throw new IndexOutOfRangeException("Row is out of range");
                if (j < 0 || j >= Columns)
                    throw new IndexOutOfRangeException("Column is out of range");
                return Values[i, j];
            }
            set
            {
                if (i < 0 || i >= Rows)
                    throw new IndexOutOfRangeException("Row is out of range");
                if (j < 0 || j >= Columns)
                    throw new IndexOutOfRangeException("Column is out of range");
                Values[i, j] = value;
            }
        }
        
        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Columns != right.Rows)
                throw new ArgumentException("Can't multply"); 
            Matrix toReturn = new Matrix(left.Rows, right.Columns);
            for (int i = 0; i < toReturn.Rows; i++)
                for (int j = 0; j < toReturn.Columns; j++)
                    for (int q = 0; q < left.Columns; q++)
                        toReturn[i, j] += left[i, q] * right[q, j];
            return toReturn;
        }
        
        public void EnterValues()
        {
            for (int i = 0; i < Rows; i++)
            {
                Console.WriteLine("Enter row values:");
                string[] line = Console.ReadLine().Split();
                List<int> newLine = new List<int>();
                try
                {
                    foreach (var x in line)
                        newLine.Add(Convert.ToInt32(x));
                    if (newLine.Count != Columns)
                        throw new WrongRowcolumnException("Wrong number of elements");
                    for (int j = 0; j < newLine.Count; j++)
                        Values[i, j] = newLine[j];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Enter other row values");
                    i--;
                }
            }
        }
    }
    class WrongRowcolumnException : ApplicationException
    {
        public WrongRowcolumnException() : base(message: "Wrong column or row number") { }
        public WrongRowcolumnException(string errorMsg) : base(message: errorMsg) { }
    }
}