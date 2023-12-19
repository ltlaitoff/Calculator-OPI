using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator_OPI
{
    public class Proizv
    {
        /// <summary>
        /// Элементарные производные
        /// </summary>
        public static string Diff(String func)
        {
            String result = "";
            if (Constant.IsIt(func))
            {
                result = Constant.Diff(func);
            }
            else if (X.IsIt(func))
            {
                result = X.Diff(func);
            }
            else if (Monome.IsIt(func))
            {
                result = Monome.Diff(func);
            }
            else if (Sinus.IsIt(func))
            {
                result = Sinus.Diff(func);
            }
            else if (Cosinus.IsIt(func))
            {
                result = Cosinus.Diff(func);
            }
            else if (Exp.IsIt(func))
            {
                result = Exp.Diff(func);
            }

            return result;
        }

        public static string Diff(String func, Dictionary<string, string> DiffsList)
        {
            String dfunc = Diff(func);
            if (dfunc == "")
            {
                foreach (KeyValuePair<string, string> pair in DiffsList)
                {
                    if (pair.Key == func)
                    {
                        dfunc = pair.Value;
                    }
                }
            }

            return dfunc;
        }

        public static string SimplifyMult(string func1, string func2)
        {
            String result = "(" + func1 + ")*(" + func2 + ")";
            if (func1 == "0" || func2 == "0")
            {
                result = "0";
            }
            else if (func1 == "1")
            {
                result = func2;
            }
            else if (func2 == "1")
            {
                result = func1;
            }
            else if (Constant.IsIt(func1))
            {
                result = func1 + "*(" + func2 + ")";
            }
            else if (Constant.IsIt(func2))
            {
                result = func2 + "*(" + func1 + ")";
            }

            return result;
        }

        public static string SimplifyAdd(string func1, string func2, bool isAdd)
        {
            String result = "";
            if (func2 == "0")
            {
                result = func1;
            }
            else if (func1 == "0")
            {
                result = (isAdd) ? func2 : "-" + func2;
            }
            else
            {
                result = (isAdd) ?
                    func1 + "+" + func2 :
                    func1 + "-" + func2;
            }

            return result;
        }

        /// <summary>
        /// Композиции
        /// </summary>
        public static String Diff(string func1, string dfunc1, string func2, string dfunc2, string oper)
        {
            String result = "";
            if (oper == "+")
            {
                result = SimplifyAdd(dfunc1, dfunc2, true);
            }
            else if (oper == "-")
            {
                result = SimplifyAdd(dfunc1, dfunc2, false);
            }
            else if (oper == "*")
            {
                String result1 = SimplifyMult(func1, dfunc2);
                String result2 = SimplifyMult(func2, dfunc1);
                result = result1 + "+" + result2;
            }
            else if (oper == "/")
            {
                result = "(" + SimplifyMult(func1, dfunc2) + "-" + SimplifyMult(func2, dfunc1) + ")/((" + func2 + ")^2)";
            }
            else if (oper == "^" && X.IsIt(func1) && Constant.IsIt(func2))
            {
                result = Diff(func1 + "^" + func2);
            }
            else if (oper == "Compose")
            {
                result = SimplifyMult(dfunc1 + "(" + func2 + ")", dfunc2);
            }

            return result;
        }
    }
}