﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cryptographic_Assistant
{
    public partial class Form1 : Form {

        //Holder for the textboxes with their values
        Dictionary<string, TextBox> textList = new Dictionary<string, TextBox>();
        Dictionary<TextBox, string> boxList = new Dictionary<TextBox, string>();
        bool textChange = true;

        public Form1()
        {
            InitializeComponent();
            textBoxE.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxT.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxA.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxO.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxI.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxN.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxS.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxH.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxR.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxD.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxL.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxU.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxC.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxM.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxF.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxW.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxY.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxP.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxV.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxB.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxG.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxK.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxJ.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxQ.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxX.TextChanged += new EventHandler(this.text_boxChanger);
            textBoxZ.TextChanged += new EventHandler(this.text_boxChanger);
        }

        private void buttonLoadCiphertext_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (d.OpenFile() != null)
                    {
                        textBoxCiphertext.Text = File.ReadAllText(d.FileName, Encoding.UTF8);

                        boxList.Clear();
                        textList.Clear();
                        analyzeData(true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void buttonETAOIN_Click(object sender, EventArgs e)
        {
            textBoxFrequency.Text = "ETAOINSHRDLUCMFWYPVBGKJQXZ";
            boxList.Clear();
            textList.Clear();
            analyzeData(true);
        }

        private void buttonLoadFrequency_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Open a file that will be parsed for letter frequencies");
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (d.OpenFile() != null)
                    {
                        String s = File.ReadAllText(d.FileName, Encoding.UTF8).ToUpper();
                        List<KeyValuePair<char, int>> l = new List<KeyValuePair<char, int>>();
                        l = parseText(s);
                        String f = "";
                        for(int i=0; i<26; i++)
                        {
                            f += l.ElementAt(i).Key.ToString().ToUpper();
                        }
                        textBoxFrequency.Text = f;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            boxList.Clear();
            textList.Clear();
            analyzeData(true);
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cryptographic Assistant\nCreated by Michael Hayes\nJanuary 2018");
        }

        private void buttonClearCiphertext_Click(object sender, EventArgs e)
        {
            textBoxCiphertext.Text = "";
        }

        private void buttonClearPlaintext_Click(object sender, EventArgs e)
        {
            textBoxPlaintext.Text = "";
        }

        private void buttonCalculatePlaintext_Click(object sender, EventArgs e)
        {
            analyzeData(true);
        }

        private void buttonUpdatePlaintext_Click(object sender, EventArgs e)
        {
            boxList.Clear();
            textList.Clear();
            analyzeData(false);
        }

        private void buttonPermute_Click(object sender, EventArgs e)
        {
            if (textBoxFrequency.Text == "")
            {
                MessageBox.Show("Nothing to Permute!");
                return;
            }
            Random r = new Random();
            char[] s = textBoxFrequency.Text.ToCharArray();
            for(int i=0; i<26; i++)
            {
                int random1 = r.Next(0, 26);
                int random2 = r.Next(0, 26);
                char temp = s[random1];
                s[random1] = s[random2];
                s[random2] = temp;
            }
            string permuted = new string(s);
            textBoxFrequency.Text = permuted;
            boxList.Clear();
            textList.Clear();
            analyzeData(true);
        }

        //Funtion to swap

        private void analyzeData(bool findFrequencies)
        {
            if(textBoxCiphertext.Text == "")
            {
                MessageBox.Show("No Ciphertext!");
                return;
            }
            //a list that contains each letter in ciphertext and its # of occurrences
            //sorted by which occurs most frequently
            List<KeyValuePair<char, int>> l = new List<KeyValuePair<char, int>>();
            if (findFrequencies)
            {
               l = parseText(textBoxCiphertext.Text);
            }
            
            //an array list that specifies which frequency mapping we are using
            ArrayList a = retrieveFrequencies();
            if(a == null) //if no frequencies were defined
            {
                return;
            }

            SortedDictionary<char, char> charMapping = new SortedDictionary<char, char>();

            //determine the letter mapping
            for(int i=0; i<26; i++)
            {
                TextBox t = getTextBoxFromLetter((char)a[i]);
                if (findFrequencies)
                {
                    textChange = false;
                    t.Text = l.ElementAt(i).Key.ToString().ToUpper();
                    textList.Add(t.Text, t);
                    boxList.Add(t, t.Text);
                    textChange = true;

                    //cipher text maps to real text
                    charMapping.Add(l.ElementAt(i).Key, (char)a[i]);
                }
                else
                {
                    textChange = false;
                    t.Text = t.Text.ToUpper();
                    textChange = true;
                    //cipher text maps to real text
                    try
                    {
                        charMapping.Add(t.Text.ElementAt(0), (char)a[i]);
                    }
                    catch(Exception x)
                    {
                        MessageBox.Show("You have the same ciphertext letter mapped to multiple plaintext letters!\nThis could be due to an incorrect frequency table, or an incorrect letter mapping.\nPlease double check and try again.");
                    }

                    textList.Add(t.Text, t);
                    boxList.Add(t, t.Text);
                }
            }
            string plaintext = "";

            //decode the mapping
            foreach(char c in textBoxCiphertext.Text)
            {
                if(charMapping.ContainsKey(c))
                {
                    plaintext += (charMapping[c].ToString());
                }
                else
                {
                    plaintext += (c.ToString());
                }
            }

            textBoxPlaintext.Text = plaintext;
        }

        //function that counts the number of occurrences of letters in a given ciphertext
        private List<KeyValuePair<char, int>> parseText(String s)
        {
            SortedDictionary<char, int> d = new SortedDictionary<char, int>();

            //count occurrences of each letter and store in a dictionary
            d.Add('E', s.Split('E').Length - 1);
            d.Add('T', s.Split('T').Length - 1);
            d.Add('A', s.Split('A').Length - 1);
            d.Add('O', s.Split('O').Length - 1);
            d.Add('I', s.Split('I').Length - 1);
            d.Add('N', s.Split('N').Length - 1);
            d.Add('S', s.Split('S').Length - 1);
            d.Add('H', s.Split('H').Length - 1);
            d.Add('R', s.Split('R').Length - 1);
            d.Add('D', s.Split('D').Length - 1);
            d.Add('L', s.Split('L').Length - 1);
            d.Add('U', s.Split('U').Length - 1);
            d.Add('C', s.Split('C').Length - 1);
            d.Add('M', s.Split('M').Length - 1);
            d.Add('F', s.Split('F').Length - 1);
            d.Add('W', s.Split('W').Length - 1);
            d.Add('Y', s.Split('Y').Length - 1);
            d.Add('P', s.Split('P').Length - 1);
            d.Add('V', s.Split('V').Length - 1);
            d.Add('B', s.Split('B').Length - 1);
            d.Add('G', s.Split('G').Length - 1);
            d.Add('K', s.Split('K').Length - 1);
            d.Add('J', s.Split('J').Length - 1);
            d.Add('Q', s.Split('Q').Length - 1);
            d.Add('X', s.Split('X').Length - 1);
            d.Add('Z', s.Split('Z').Length - 1);

            //the dictionary sorts on key, so we need to re-sort it by value
            List<KeyValuePair<char, int>> l = d.ToList();

            l.Sort(
                delegate (KeyValuePair<char, int> pair1,
                KeyValuePair<char, int> pair2)
                {
                    return pair1.Value.CompareTo(pair2.Value);
                }
            );

            //but after sorting by value, 0 is at the top, so reverse so that the letter with most occurrences is at the top
            l.Reverse();

            return l;
        }

        //function to set the chart data
        private void setChartData(List<KeyValuePair<char, int>> cipherText) {
            
        }
 
        

        private ArrayList retrieveFrequencies()
        {
            if(textBoxFrequency.Text == "")
            {
                DialogResult dialogResult = MessageBox.Show("No letter frequency was specified,\nUse ETAOIN SHRDLU?", "Error - Letter Frequency", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    textBoxFrequency.Text = "ETAOINSHRDLUCMFWYPVBGKJQXZ";
                }
                else if (dialogResult == DialogResult.No)
                {
                    return null;
                }
            }
            ArrayList a = new ArrayList();
            foreach(char c in textBoxFrequency.Text)
            {
                if (!a.Contains(c))
                {
                    a.Add(c);
                }
            }

            return a;
        }

        private TextBox getTextBoxFromLetter(char letter)
        {
            switch (letter)
            {
                case 'E': return textBoxE;
                case 'T': return textBoxT;
                case 'A': return textBoxA;
                case 'O': return textBoxO;
                case 'I': return textBoxI;
                case 'N': return textBoxN;
                case 'S': return textBoxS;
                case 'H': return textBoxH;
                case 'R': return textBoxR;
                case 'D': return textBoxD;
                case 'L': return textBoxL;
                case 'U': return textBoxU;
                case 'C': return textBoxC;
                case 'M': return textBoxM;
                case 'F': return textBoxF;
                case 'W': return textBoxW;
                case 'Y': return textBoxY;
                case 'P': return textBoxP;
                case 'V': return textBoxV;
                case 'B': return textBoxB;
                case 'G': return textBoxG;
                case 'K': return textBoxK;
                case 'J': return textBoxJ;
                case 'Q': return textBoxQ;
                case 'X': return textBoxX;
                case 'Z': return textBoxZ;
                default: return null;
            }
        }

       private void text_boxChanger(object sender, EventArgs e)
        {
            TextBox tempBox = (TextBox) sender;
            if (!textChange) return;

            if (tempBox.Text == "") return;

            string oldVal = boxList[tempBox];
            TextBox second = textList[tempBox.Text];

            boxList[tempBox] = tempBox.Text;
            textList[tempBox.Text] = tempBox;
            boxList[second] = oldVal;
            textList[oldVal] = second;
            textChange = false;
            second.Text = oldVal;
            textChange = true;
        }
    }
}
