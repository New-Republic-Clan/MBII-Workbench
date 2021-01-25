using MB2_Workbench.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MB2_Workbench.Classes.SiegeDeserializer
{
    public class SiegeDeserializer
    {

        private int LineNumber = 0;
        private string[] Lines;

        private object Output;

        public T Deserialize<T>(string input)
        {

            /* Remove Comments */
            input = Regex.Replace(input, "//.*", "");

            /* Replace Tabs with Spaces */
            input = input.Replace("\t", " ");

            /* All Multiple Spaces into Single Space */
            input = string.Join(" ", input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            /* Split and clean lines */
            Lines = CleanLines(input.Split('\n'));

            /* Create object we are building for output */
            Output = Activator.CreateInstance(typeof(T));
  
            /* Process and save to result */
            Process(Output);

            return (T)Output;
        }

        /* Used to clean / prepare all lines */
        private string[] CleanLines(string[] lines)
        {

            for (int i = 0; i < lines.Count(); i++)
            {
                lines[i] = lines[i].Trim();
            }

            return lines;
        }

        /* Main Processing Method */
        private void Process(object output)
        {

            /* Read Lines and Process */
            do
            {
                ProcessLine(output);
                NextLine();

                if (EndOfObject())
                    break;
            }
            while (MoreLines());

        }

        /* Actually Processes the Current Line */
        private void ProcessLine(object output)
        {

            /* is this line the start of an object if so we create a blank object of that type */
            if (LineIsObject())
            {

                if(ShouldSkip())
                    SkipObject();

                PropertyInfo propertyInfo = output.GetType().GetProperties().Where(x => x.Name.ToLower() == HandleSwaps(Line().ToLower())).FirstOrDefault();

                if (propertyInfo == null)
                {
                    throw new Exception($"Property {Line().ToLower()} not found  in {output.GetType().Name} \n\n Line {LineNumber}: {Line()}");
                }

                propertyInfo.SetValue(output, GenerateObject());
                NextLine();
                Process(propertyInfo.GetValue(output));
            }

            if (LineIsProperty())
            {
                string propertyName = GetPropertyName();

                propertyName = HandleSwaps(propertyName);

                PropertyInfo propertyInfo = output.GetType().GetProperties().Where(x => x.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();

                if (propertyInfo == null)
                {
                    var i = Lines;
                    throw new Exception($"Property {propertyName} not found  in {output.GetType().Name} \n\n Line: {Line()}");
                }

                Type propertyType = propertyInfo.PropertyType;

                string propertyValue = GetPropertyValue();

                if(propertyType == typeof(string))
                {
                    propertyInfo.SetValue(output, propertyValue);
                }

                if (propertyType == typeof(int?) || propertyType == typeof(int))
                {
                    propertyInfo.SetValue(output, int.Parse(propertyValue));

                }

                if (propertyType == typeof(double?) || propertyType == typeof(double))
                {
                    propertyInfo.SetValue(output, double.Parse(propertyValue));
                }

            }

        }

        /* Retieve a Line */
        private string Line(int line = 0)
        {

            if(line == 0)
                if (LineNumber < Lines.Count())
                    return Lines[LineNumber];

            if (line <= Lines.Count())
                return Lines[line];

            return "";
        }

        /* Preview Next Line */
        private string PeekNextLine()
        {

            if (LineNumber+1 < Lines.Count())
                return Lines[LineNumber + 1];

            return "";
        }

        private void SkipObject()
        {

            /* Properties we wish to skip */
           
                do
                {
                    NextLine();

                    /* If finds another object within */
                    if (PeekNextLine().Contains("{"))
                    {
                        SkipObject();
                    }
                }
                while (EndOfObject() == false);

            NextLine();
            return;
   
        }

        private Boolean ShouldSkip()
        {

            if (Line().Contains("SemiAuthPoints"))
                return true;

            return false;

        }

        /* Increments the line */
        private void NextLine()
        {
           
            if (LineNumber <= Lines.Count())
                LineNumber++;
        }

        /* Still lines to process */
        private Boolean MoreLines()
        {
            if (LineNumber > Lines.Count())
                return false;
            return true;
        }

 

        /* Is current line start of an object */
        private Boolean LineIsObject()
        {

            if (!Line().Contains(" ") && PeekNextLine() == "{")
                return true;
            return false;
        }

        /* Properites have a space seperator between name and value */
        private Boolean LineIsProperty()
        {

            if (Line().Contains(" ") && Line().Length > 1)
                return true;
            return false;

        }

        /* Get a property name */
        private string GetPropertyName()
        {
            return Line().Split(" ")[0];
        }

        /* Gets the value of a property as a string */
        private string GetPropertyValue()
        {

            /* 2 Quotes, is a string */
            if (Line().ToCharArray().Where(x => x == '"').Count() == 2)
            {
                return new Regex("\".*?\"").Matches(Line()).FirstOrDefault().Value.Replace("\"", "");
            }
            
            /* 1 Quote, is a string array */
            if (Line().ToCharArray().Where(x => x == '"').Count() == 1)
            {
                StringBuilder value = new StringBuilder();

                value.Append(Line().Split('"')[1]);

                /* Loop until the end of the multi-line string */
                do
                {
                    NextLine();
                    value.Append(Line() + Environment.NewLine);
                    
                }
                while (!PeekNextLine().EndsWith("\""));

                /* Increment a final time */
                NextLine();

                return value.ToString();
            }

            /* Otherwise we return a string of whatever is left */
            return Line().Split(" ")[1].Replace("\"", ""); ;


        }

        /* Generates an object */
        private object GenerateObject()
        {

            Type objectType = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "MB2_Workbench.Classes" && t.Name.ToLower() == Line().ToLower())
                      .ToList().FirstOrDefault();

            if (objectType == null)
            {

                /* Try without the number to see if we get a match*/
                objectType = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "MB2_Workbench.Classes" && t.Name.ToLower() == Regex.Replace(Line(), @"[\d-]", string.Empty).ToLower())
                      .ToList().FirstOrDefault();

                if (objectType == null)
                {
                    throw new Exception($"Unable to find class for MB2_Workbench.Classes.{ Line() }");
                }
            }

            var obj = Activator.CreateInstance(objectType);
            return obj;

        }

        /* Have we reached the end of an object? */
        private Boolean EndOfObject()
        {
            if (Line() == ("}"))
                return true;
            return false;

        }

        /* Any property names we want to swap for another name used by matching in class */
        private string HandleSwaps(string name)
        {

            if (name.ToLower() == "customclip")
                name = "clipsize";



            return name;

        }



    }
}
