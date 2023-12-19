using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator_OPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static bool errorActive = false;

        /* Logic */
        static char token;

        static string inputString = "";
        static int inputStringIndex = 0;

        static double t(string input)
        {
            inputString = input;
            inputStringIndex = 0;

            token = DequeueToken();

            double result = Expr();

            return result;
        }

        static void error(string msg)
        {
            Console.Error.WriteLine(msg);
            Environment.Exit(1);
        }

        static char DequeueToken()
        {
            if (inputString.Length > inputStringIndex)
            {
                return inputString[inputStringIndex++];
            }

            return '\0';
        }

        // sin -> s
        // cos -> c
        // tg - t
        // ctg -> b

        static double Factor()
        {
            double value = double.MaxValue;

            Console.WriteLine("Input token = " + token);
            if (token == '(')
            {
                match('(');
                value = Expr();
                match(')');
            }
            else if (token == ')')
            {
                value = 0;
            }
            else if (char.IsDigit(token) || token == '.' || token == '+' || token == '-' || token == 'π' || token == 'e')
            {
                 char previousToken = token;
                
                if (char.IsDigit(token))
                {
                    value = double.Parse(token.ToString());
                } 
                else if (token == 'π')
                {
                    value = System.Math.PI;
                } 
                else if (token == 'e')
                {
                    value = System.Math.E;
                }
                
                token = DequeueToken();
                
                if (token == '.')
                {
                    previousToken = '.';
                    token = DequeueToken();
                }
                
                if (char.IsDigit(token))
                {
                    double newFactor = Factor();
                
                    if (previousToken == '.')
                    {
                        value = Convert.ToDouble(value.ToString() + "," + newFactor.ToString());
                    }
                    else if (value == double.MaxValue)
                    {
                        int negative = (previousToken == '-') ? -1 : 1;
                
                        value = negative * newFactor;
                    }
                    else
                    {
                        value = double.Parse(value.ToString() + newFactor.ToString());
                    }
                }
            }
            else if (char.IsLetter(token))
            {
                string functionName = "";
                while (char.IsLetter(token))
                {
                    functionName += token;
                    token = DequeueToken();
                }

                if (token == '(')
                {
                    match('(');
                    double expresion = Expr();
                    match(')');

                    switch (functionName.ToLower())
                    {
                        case "sin":
                            value = System.Math.Sin(transformToDegMode(expresion));
                            break;
                        case "cos":
                            value = System.Math.Cos(transformToDegMode(expresion));
                            break;
                        case "tan":
                            value = System.Math.Tan(transformToDegMode(expresion));
                            break;
                        case "cot":
                            value = 1 / System.Math.Tan(transformToDegMode(expresion));
                            break;
                        case "sec":
                            value = 1 / System.Math.Cos(transformToDegMode(expresion));
                            break;
                        case "csc":
                            value = 1 / System.Math.Sin(transformToDegMode(expresion));
                            break;
                        case "ln":
                            value = System.Math.Log(expresion);
                            break;
                        case "log":
                            value = System.Math.Log10(expresion);
                            break;
                        case "abs":
                            value = System.Math.Abs(expresion);
                            break;
                        default:
                            errorActive = true;
                            break;
                    }
                }
                else
                {
                    errorActive = true;
                }
            }
            else
            {
                errorActive = true;
            }

            Console.WriteLine("value = " + value.ToString());

            return value;
        }

        static double SomeLevel()
        {
            double value = Factor();

            while (token == '^')
            {
                switch (token)
                {
                    case '^':
                        match('^');
                        value = System.Math.Pow(value, Factor());
                        break;
                    default:
                        errorActive = true;
                        break;
                }
            }

            return value;
        }


        static double Term()
        {
            double value = SomeLevel();

            while (token == '*' || token == '/')
            {
                switch (token)
                {
                    case '*':
                        match('*');
                        value *= SomeLevel();
                        break;
                    case '/':
                        match('/');
                        value /= SomeLevel();
                        break;
                    default:
                        errorActive = true;
                        break;
                }
            }

            return value;
        }

        static double Expr()
        {
            double value = Term();

            while (token == '+' || token == '-')
            {
                switch (token)
                {
                    case '+':
                        match('+');
                        value += Term();
                        break;
                    case '-':
                        match('-');
                        value -= Term();
                        break;
                    default:
                        errorActive = true;
                        break;
                }
            }

            return value;
        }

        static void match(char expected)
        {
            if (token == expected)
            {
                token = DequeueToken();
                return;
            }

            errorActive = true;
        }

        static double transformToDegMode(double value)
        {
            if (degreeMode == "rad")
            {
                return value;
            }

            return value * System.Math.PI / 180;
        }

        /* Logic end */

        static string degreeMode = "deg";
        static int afterDotNumbers = 2;
        static int AFTER_DOT_MAX = 5;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button_0_Click(object sender, EventArgs e)
        {
            textBox1.Text += '0';
        }

        private void button_result_Click(object sender, EventArgs e)
        {
            double answer = t(textBox1.Text.Replace(',', '.'));

            if (errorActive == true)
            {
                MessageBox.Show("Error!");
                answer = 0;
            }

            textBox1.Text = answer.ToString("0." + new string('0', afterDotNumbers));
        }

        private void button_3_Click(object sender, EventArgs e)
        {
            textBox1.Text += '3';
        }

        private void button_2_Click(object sender, EventArgs e)
        {
            textBox1.Text += '2';
        }

        private void button_1_Click(object sender, EventArgs e)
        {
            textBox1.Text += '1';
        }

        private void button_4_Click(object sender, EventArgs e)
        {
            textBox1.Text += '4';
        }

        private void button_5_Click(object sender, EventArgs e)
        {
            textBox1.Text += '5';
        }

        private void button_6_Click(object sender, EventArgs e)
        {
            textBox1.Text += '6';
        }

        private void button_7_Click(object sender, EventArgs e)
        {
            textBox1.Text += '7';
        }

        private void button_8_Click(object sender, EventArgs e)
        {
            textBox1.Text += '8';
        }

        private void button_9_Click(object sender, EventArgs e)
        {
            textBox1.Text += '9';
        }

        private void button_plus_Click(object sender, EventArgs e)
        {
            textBox1.Text += '+';
        }

        private void button_minus_Click(object sender, EventArgs e)
        {
            textBox1.Text += '-';
        }

        private void button_multiply_Click(object sender, EventArgs e)
        {
            textBox1.Text += '*';
        }

        private void button_division_Click(object sender, EventArgs e)
        {
            textBox1.Text += '/';
        }

        private void button_del_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }
        }

        private void button_c_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button_pow_Click(object sender, EventArgs e)
        {
            textBox1.Text += '^';
        }

        private void button_dot_Click(object sender, EventArgs e)
        {
            textBox1.Text += ',';
        }

        private void button_pi_Click(object sender, EventArgs e)
        {
            textBox1.Text += 'π';
        }

        private void button_exp_Click(object sender, EventArgs e)
        {
            textBox1.Text += 'e';
        }

        private void button_hint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by @ltlaitoff | Ivan Shchedrovskyi<ltlaitoff@gmail.com>");
        }

        private void button_log_Click(object sender, EventArgs e)
        {
            textBox1.Text += "log()";
        }

        private void button_ln_Click(object sender, EventArgs e)
        {
            textBox1.Text += "ln()";
        }

        private void button_change_after_dot_Click(object sender, EventArgs e)
        {
       
            afterDotNumbers++;

            if (afterDotNumbers > AFTER_DOT_MAX)
            {
                afterDotNumbers = 1;
            }

            button_after_dot_1.Text = "." + afterDotNumbers.ToString();
            button_after_dot_2.Text = "." + afterDotNumbers.ToString();
        }

        private void button_deg_mode_Click(object sender, EventArgs e)
        {
            if (degreeMode == "rad")
            {
                degreeMode = "deg";
            } else
            {
                degreeMode = "rad";
            }

            deg_mode_1.Text = degreeMode;
            deg_mode_2.Text = degreeMode;
        }

        private void button_abs_Click(object sender, EventArgs e)
        {
            textBox1.Text += "abs()";
        }

        private void button_sin_Click(object sender, EventArgs e)
        {
            textBox1.Text += "sin()";
        }

        private void button_cos_Click(object sender, EventArgs e)
        {
            textBox1.Text += "cos()";
        }

        private void button_tg_Click(object sender, EventArgs e)
        {
            textBox1.Text += "tg()";
        }

        private void button_ctg_Click(object sender, EventArgs e)
        {
            textBox1.Text += "ctg()";
        }

        private void button_scs_Click(object sender, EventArgs e)
        {
            textBox1.Text += "csc()";
        }

        private void button_sec_Click(object sender, EventArgs e)
        {
            textBox1.Text += "sec()";
        }

        private void button_left_quot_Click(object sender, EventArgs e)
        {
            textBox1.Text += "(";
        }

        private void button_right_quot_Click(object sender, EventArgs e)
        {
            textBox1.Text += ")";
        }
    }
}

