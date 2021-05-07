using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelingSalesman.Models
{ 
    public class Node
    {
        public char Name { get; set; }
        public char Parent { get; set; }
       
        public string Children { get; set; }

        public double Value { get; set; }
    }
    public static class Utils
    {
        

        public static void Permute(List<string> paths,String str,
                               int l, int r)
        {
            if (l == r)
            {
                //Console.WriteLine(str);
                paths.Add(str);
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    str = Swap(str, l, i);
                    Permute(paths, str, l + 1, r);
                    str = Swap(str, l, i);
                }
            }
        }

        public static String Swap(String a, int i, int j)
        {
            char temp;
            char[] charArray = a.ToCharArray();
            temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
            string s = new string(charArray);
            return s;
        }

        public static String Reverse(String a, int x, int y)
        {
            //reverse the subsection of the string delineated by start and end
            int start, end;
            if (x > y)
            {
                start =y;
                end= x;
            } else
            {
                start =x;
                end =y;
            }
            int width = end- start+1;
            String sub = a.Substring(start, width);
            String head = a.Substring(0, start);
            String tail = a.Substring(end+1, a.Length-end-1);
            char[] substring = sub.ToCharArray();
            Array.Reverse(substring);
            String rev = new string(substring); 
            //Console.WriteLine(string.Format("original : {0}, head:{1}, rev :{2},  tail:{3}", a, head, rev, tail));

            return String.Concat(head,rev,tail);
        }

        public static String Transport(String a, int x, int y, Random rand)
        {
            //slice out the substring and move it to some other point
            int start, end;
            if (x > y)
            {
                start = y;
                end = x;
            }
            else
            {
                start = x;
                end = y;
            }
            int width = end - start + 1;
            String sub = a.Substring(start, width);
            String head = a.Substring(0, start);
            String tail = a.Substring(end + 1, a.Length - end - 1);
            String trans = String.Concat(head, tail);
            int insertPoint = rand.Next(trans.Length);
            String newString = trans.Insert(insertPoint, sub);
            //Console.WriteLine(string.Format("original : {0}, trans:{1}, sub :{2},  ne:{3}", a, trans, sub, newString));
            return newString;
        }

        public static int Factorial(int n)
        {
            int ans = 1;
            for(int i = 2; i <= n; i++)
            {
                ans *= i;
            }
            return ans;
        }

        public static void OrderedCrossover(String a, String b, int x ,int y, ref String c1, ref String c2)
        {
            int start, end;
            if (x > y)
            {
                start = y;
                end = x;
            }
            else
            {
                start = x;
                end = y;
            }
            //mutation scheme where ti splices strings at a point and either swaps the front half or back half based on the bool
            List<char> headA = a.Substring(0, start).ToList<char>();
            List<char> headB = b.Substring(0, start).ToList<char>(); 
            List<char> midA = a.Substring(start,end-start).ToList<char>(); 
            List<char> midB = b.Substring(start,end-start).ToList<char>();
            List<char> tailA = a.Substring(end).ToList<char>();
            List<char> tailB = b.Substring(end).ToList<char>();

            List<char> C1 = new List<char> ();
            List<char> C2 = new List<char>();
            List<char> removeC1 = new List<char>();
            List<char> removeC2 = new List<char>();
            List<char> newTailC1 = new List<char>();
            List<char> newTailC2 = new List<char>();
            List<char> newHeadC1 = new List<char>();
            List<char> newHeadC2 = new List<char>();
            //Console.WriteLine(String.Format("original a:{0}, b:{1} start:{2}, end{3}",a,b,start,end));
            //Console.WriteLine(String.Format("headA :{0}, midA:{1}, tailA:{2}", PrintList(headA), PrintList(midA), PrintList(tailA)));
            //Console.WriteLine(String.Format("headB :{0}, midB:{1}, tailB:{2}", PrintList(headB), PrintList(midB), PrintList(tailB)));
            //Build first child

            //start with midB list
            //take midB list and remove any common letters with midA put into remove list
            //take midA and remove any common chars with midB
            removeC1.AddRange(midB);
            removeC2.AddRange(midA);
            newHeadC1.AddRange(headA);
            newHeadC2.AddRange(headB);
            newTailC1.AddRange(tailA);
            newTailC2.AddRange(tailB);
            foreach ( char letter in IntersectList(midA, midB))
            {
                removeC1.Remove(letter);
                removeC2.Remove(letter);

            }
            newHeadC1.AddRange(removeC2);
            newHeadC2.AddRange(removeC1);
            foreach( char letter in removeC1){
                newHeadC1.Remove(letter);
                newTailC1.Remove(letter);
            }
            foreach (char letter in removeC2)
            {
                newHeadC2.Remove(letter);
                newTailC2.Remove(letter);
            }
            //Remove any items in removeC1 from newHeadC1 and add them to newTailC1
            if (newTailC1.Count < tailA.Count)
            {
                //Console.WriteLine("C1 Tail is shorter by {0}", tailA.Count - newTailC1.Count);

                for (int i =0; i< tailA.Count-newTailC1.Count; i++)
                {
                    char temp = newHeadC1[0];
                    newHeadC1.Remove(temp);
                    newTailC1.Add(temp);
                }
            }
            if(newTailC2.Count < tailB.Count)
            {
                //Console.WriteLine("C2 Tail is shorter by {0}", tailB.Count - newTailC2.Count);
                for (int i = 0; i < tailB.Count - newTailC2.Count; i++)
                {
                    char temp = newHeadC2[0];
                    newHeadC2.Remove(temp);
                    newTailC2.Add(temp);
                }
            }
            C1.AddRange(newHeadC1);
            C1.AddRange(midB);
            C1.AddRange(newTailC1);
            //Console.WriteLine(String.Format("C1:new head :{0}, mid : {1}, new tail :{2}", PrintList(newHeadC1), PrintList(midB), PrintList(newTailC1)));

            C2.AddRange(newHeadC2);
            C2.AddRange(midA);
            C2.AddRange(newTailC2);
            //Console.WriteLine(String.Format("C2:new head :{0}, mid : {1}, new tail :{2}", PrintList(newHeadC2), PrintList(midA), PrintList(newTailC2)));

            c1 = PrintList(C1);
            c2 = PrintList(C2);
           // Console.WriteLine(String.Format("a:{0} ,b : {1}, c1 : {2}, c2:{3}", a, b, c1,c2));

        }

        public static string PrintList(List<char> cList)
        {
            StringBuilder sb = new StringBuilder();
            foreach(char c in cList)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static List<char> IntersectList(List<char>A, List<char> B)
        {
            //Return a list of all chars contained in both lists
            List<char> bothContain = new List<char>();
            foreach (char c in A)
            {
                if (B.Contains(c))
                {
                    bothContain.Add(c);
                }  
            }
            return bothContain;
        }

        public static String SwapMutation(String a , double p, Random rand)
        {
            //Mutate a string with each charcter having a possibility, p, of flipping with the next letter
            StringBuilder sb = new StringBuilder(a);
            double coin;
            for(int i=0; i < a.Length - 1; i++)
            {
                coin = rand.NextDouble();
                if(p > coin)
                {
                    char temp = sb[i];
                    sb[i] = sb[i + 1];
                    sb[i+1] = temp;
                }
            }

            return sb.ToString();
        }


    }
}
