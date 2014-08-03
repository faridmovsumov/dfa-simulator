using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DfaSimulator
{
    public partial class Form1 : Form
    {
        public Dfa dfa = new Dfa();
        
        
        
        public Form1()
        {
            InitializeComponent();
        }

        public bool isState(String s)
        {
            if (s.Length != 2)
            {
                return false;
            }
            else if (s[0].ToString() != "q")
            {
                return false;
            }
            else
            {
                try
                {
                    int k=Convert.ToInt32(s[1].ToString());
                    if (!dfa.states.Contains(k))
                    {
                        MessageBox.Show("DİKKAT: q" + k + " adlı state sistemde daha önce tanımlanmadı");
                        return false;
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            dfa.sifirla();
            richTextBox2.Text = "";
            bool noProblem = true;
            String errorMessage="";
            richTextBox2.BackColor = Color.Aqua;
            int numberOfStates;
            numberOfStates = Convert.ToInt32(numericUpDown1.Value);
            if (numberOfStates == 0)
            {
                errorMessage += "State sayısı 0 olarak girdiğiniz için hesaplama yapılamıyor\n";
                noProblem = false;
            }
            //MessageBox.Show("Number of states is: "+numberOfStates);

            String possibleInputs = textBox1.Text;
            //MessageBox.Show("Possible Inputs' Set: "+possibleInputs);

            String inputData = richTextBox1.Text;
            //MessageBox.Show("Input Data is: " + inputData);

            String inputStirng = textBox2.Text;
            //MessageBox.Show("Input String is: "+inputStirng);

            String startState = textBox3.Text;
            //MessageBox.Show("Start State is: " + startState);

            String acceptedStates = textBox4.Text;
            //MessageBox.Show("Accepted states are: " + acceptedStates);

            //Öncelikle Statelerimizi Tanımlarız
            
            //Kaç tane state varsa herbiri için qi isminde bir state oluştururuz 0 dan başlatırız
            for (int i = 0; i <= numberOfStates - 1; i++)
            {
                dfa.states.Add(i); 
            }

            //Start state tanımlama

            startState = startState.Replace(" ", "");
            if (startState.Length < 1)
            {
                errorMessage +="Start state tanımlamasında hata var! \n" +
                "qn şeklinde tanımlayınız lütfen. Burada n sayısı state sayısından en az 1 küçük olmalıdır\n";
                noProblem = false;
            }
            else if (startState[0] == 'q')
            {
                
                try
                {
                    Char c = startState[1];
                    int a = Convert.ToInt32(c.ToString());
                    if (a <= numberOfStates - 1)
                    {
                        //MessageBox.Show("Start State is: q" + a);
                        dfa.startState = a;
                    }
                    else
                    {
                        errorMessage += "Tanımlanan Start State tanımlanmış stateler arasında bulunamadı.\n";
                        noProblem = false;
                    }
                }
                catch
                {
                    errorMessage += "Start index formatı yanlış girilmiş\n";
                    noProblem = false;
                }

            }
            else
            {
                errorMessage += "Start state tanımlamasında hata var format qn şeklinde olmalıdır\n";
                noProblem = false;
            }


            //Accepted State'lerimizi Tanımlıyoruz...
            acceptedStates = acceptedStates.Replace(" ", "");
            String[] acceptedtemp = acceptedStates.Split(',');
            for (int i = 0; i <= acceptedtemp.Length - 1; i++)
            {
                if (acceptedtemp[i][0] == 'q' && acceptedtemp[i].Length == 2)
                {
                    Char c = acceptedtemp[i][1];
                    try
                    {
                        int j = Convert.ToInt32(c.ToString());
                        if (j <= numberOfStates - 1)
                        {
                            dfa.acceptedStates.Add(j);
                            //MessageBox.Show("Accepted States are q"+j);
                        }
                        else
                        {
                            errorMessage += "Tanımladığınız q" + j + " isimli accepted state tanımlanmış stateler içerisinde bulunamadı bulunamadı.\n";
                            noProblem = false;
                        }
                    }
                    catch {
                        errorMessage += "Accepted State Tanımlamasında hata var\n";
                        noProblem = false;
                    }  
                }
                else
                {
                    errorMessage +="Accepted State Tanımlamasında Hata algılandı\n"+
                        "Tanımladığınız accepted stateleri virgülle ayırınız ve qn formatında giriniz\n";
                    noProblem = false;
                }
                
            }
            //Possible Input Tanımlama
            possibleInputs = possibleInputs.Replace(" ","");
            String[] tempPI=possibleInputs.Split(',');

            for (int i = 0; i <= tempPI.Length - 1; i++)
            {
                if (tempPI[i] != "")
                {
                    dfa.possibleInputs.Add(tempPI[i]);
                    //MessageBox.Show("Possible input= " + tempPI[i]);
                }
            }

            //Input Data Okuma En Önemli Kısım
            inputData = inputData.Replace(" ", "");
            inputData = inputData.Replace("\n", "");
            String[] tempID=inputData.Split('|');

            if (inputData == "")
            {
                errorMessage += "Statelerin hangi inputlar için hangi statelere geçeceğini tanımlamayı unuttunuz.";
                noProblem = false;
            }
            try
            {
                for (int i = 0; i <= tempID.Length - 1; i++)
                {
                    if (tempID[i] != "")
                    {
                        String[] tempSplit = tempID[i].Split(',');
                        if (isState(tempSplit[0]) && tempSplit[1] != "" && isState(tempSplit[2]))
                        {
                            InputData id = new InputData(Convert.ToInt32(tempSplit[0][1].ToString()), tempSplit[1], Convert.ToInt32(tempSplit[2][1].ToString()));
                            //MessageBox.Show("Input: From state "+tempSplit[0]+" when input= "+
                            //    tempSplit[1]+" to state "+tempSplit[2]);

                            dfa.inputData.Add(id);
                        }
                        else
                        {
                            errorMessage += "Problem in Dfa Definition\n";
                            noProblem = false;
                        }
                    }
                }
            }
            catch
            {
                errorMessage += "Problem in Dfa Definition\n";
                noProblem = false;
            }

            //input tanımlıyoruz

            string input = textBox2.Text;
            input=input.Replace(" ","");
            bool flag = true;
            if (input == "")
            {
                errorMessage += "\nInput Girmeyi Unuttunuz\n";
                noProblem = false;
            }
            else
            {
                for (int i = 0; i <= input.Length - 1; i++)
                {
                    if (!possibleInputs.Contains(input[i]))
                    {
                        flag = false;
                    }
                }

                if (!flag)
                {
                    errorMessage +="Yanlış input girdiniz.\n";
                    noProblem = false;
                }
                else
                {
                    dfa.input = input;
                }
            }

            if (noProblem)
            {
                if (dfa.isDfa())
                {

                    if (dfa.run())
                    {
                        richTextBox2.Text = dfa.output;
                        richTextBox2.Text += "\ninput DFA tarafından kabul edildi";
                        richTextBox2.BackColor = Color.GreenYellow;
                        button2.Enabled = true;
                    }
                    else
                    {
                        richTextBox2.Text = dfa.output;
                        richTextBox2.Text += "\nInput DFA tarafından kabul edilmedi!";
                        richTextBox2.BackColor = Color.Red;
                        button2.Enabled = true;

                    }
                }
                else
                {
                    richTextBox2.Text = "Input Data kısmında yaptığınız tanımlar dfa olma şartını sağlamamaktadır!";
                    richTextBox2.BackColor = Color.Olive;
                }
            }
            else
            {
                richTextBox2.Text += "Yaptığınız Tanımlamalarda sorunlar algılandı.\n";
                richTextBox2.Text += errorMessage;
                richTextBox2.BackColor = Color.Orange;
            }
            
            
            
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            DfaCizim dc = new DfaCizim(dfa);
            dc.ShowDialog();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "q0,0,q2\n|q0,1,q1\n|q1,0,q2\n|q1,1,q0\n|"+
                "q2,0,q4\n|q2,1,q1\n|q4,0,q3\n|q4,1,q2\n|q3,0,q1\n|q3,1,q3";
            textBox2.Text = "10100";
        }

        private void textBox2_MouseHover(object sender, EventArgs e)
        {
            textBox2.BackColor = Color.Yellow;
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            textBox2.BackColor = Color.White;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("DFA Simulator designed by Ferid Mövsümov 25.04.2011\nmail: faridmovsumov@gmail.com");
        }

        
    }
}
