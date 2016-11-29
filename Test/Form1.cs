using ProcessMemoryReaderLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ProcessMemoryReader reader;
        byte[] data1, data2;
        bool[] compare;

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var procs = Process.GetProcesses();
            var ms = procs.FirstOrDefault(p => p.ProcessName == "MineSweeper");
            reader = new ProcessMemoryReader()
            {
                ReadProcess = ms
            };

            reader.OpenProcess();
            int read = -1;
            data1 = reader.ReadProcessMemory((IntPtr)0x10000, 57344, out read);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var procs = Process.GetProcesses();
            var ms = procs.FirstOrDefault(p => p.ProcessName == "MineSweeper");
            reader = new ProcessMemoryReader()
            {
                ReadProcess = ms
            };

            reader.OpenProcess();
            int read = -1;
            data2 = reader.ReadProcessMemory((IntPtr)0x10000, 57344, out read);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string result = "";
            compare = new bool[Math.Min(data1.Length, data2.Length)];
            for (int i = 0; i < compare.Length; i++)
            {
                compare[i] = (data1[i] != data2[i]);
                if (compare[i])
                    result += i.ToString() + "\r\n";
            }
            quickwrite("E:\\diff.txt", result);
        }

        void quickwrite(string filename, string content)
        {
            using (var writer = new StreamWriter(filename))
            {
                writer.Write(content);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string result = "";
            for (int i = 0; i < data1.Length; i++)
            {
                result += i.ToString("X5") + " " + data1[i].ToString("X2") + "\r\n";
            }
            quickwrite("E:\\data1.txt", result);
        }
    }
}
