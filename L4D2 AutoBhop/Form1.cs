// Shit code starting here, enjoy!

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using Memory;

namespace L4D2_AutoBhop
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        public static string Jump = "client.dll+0x757DF0"; // Updated Offsets(April 6, 2022)
        public static string InAir = "client.dll+0x6E1008"; // Updated Offsets(April 6, 2022)
        Mem m = new Mem();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int procID = m.GetProcIdFromName("left4dead2");
            if (procID > 0) // Checking game activity
            {
                m.OpenProcess(procID); // Open if found the process
                Thread BH = new Thread(BHOP); // Starting BHOP Function if process is opened
                BH.Start();
            }
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
        
        private void BHOP()
        {
            while (true)
            {
                if (checkBox1.Checked)
                {
                    if (GetAsyncKeyState(Keys.Space) < 0) // Holding spacebar
                    {
                        int ground = m.ReadInt(InAir);
                        if (ground == 0) // If we are on the ground (0)
                        {
                            m.WriteMemory(Jump, "int", "5"); // Force Jump if ground value changed to 5
                            Thread.Sleep(100);
                            m.WriteMemory(Jump, "int", "4"); // Changing value to 4, so we can force jump again in the next time
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}
