using MB2_Workbench.Classes.Exceptions;
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

            LineNumber = 0;

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

            if (Line().Trim() == "")
                return;

            /* is this line the start of an object if so we create a blank object of that type */
            if (LineIsObject())
            {

                if(ShouldSkip())
                    SkipObject();

                PropertyInfo propertyInfo = output.GetType().GetProperties().Where(x => x.Name.ToLower() == HandleSwaps(Line().ToLower())).FirstOrDefault();

                if (propertyInfo == null)
                {

                    /* FIX: for teams in siege file, in serialise, this must be done in reverse, the team name will never be a class, use "teamdetails" */
                    if(output.GetType().GetProperties().Where(x => x.Name.ToLower() == "teams").Any())
                    {
                        var t = output.GetType().GetProperties().Where(x => x.Name.ToLower() == "teams").FirstOrDefault().GetValue(output);

                        var g = (string)t.GetType().GetProperties().Where(x => x.Name.ToLower() == "team1").FirstOrDefault().GetValue(t);
                        var c = Line();

                        if (Line().ToLower() == (string) t.GetType().GetProperties().Where(x => x.Name.ToLower() == "team1").FirstOrDefault().GetValue(t).ToString().ToLower())
                        {
                            propertyInfo = output.GetType().GetProperties().Where(x => x.Name.ToLower() == "team1").FirstOrDefault();
                        }

                        if (Line().ToLower() == (string)t.GetType().GetProperties().Where(x => x.Name.ToLower() == "team2").FirstOrDefault().GetValue(t).ToString().ToLower())
                        {
                            propertyInfo = output.GetType().GetProperties().Where(x => x.Name.ToLower() == "team2").FirstOrDefault();
                        }

                        if (propertyInfo == null)
                        {
                            throw new Exception($"Property {Line().ToLower()} not found  in {output.GetType().Name} \n\n Line {LineNumber}: {Line()}");
                        }

                    }
                    else
                    {
                        throw new Exception($"Property {Line().ToLower()} not found  in {output.GetType().Name} \n\n Line {LineNumber}: {Line()}");
                    }

                }

                propertyInfo.SetValue(output, GenerateObject(propertyInfo.PropertyType.ToString()));
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

                /* is a string */
                if(propertyType == typeof(string))
                {
                    try
                    {
                        propertyInfo.SetValue(output, propertyValue);
                    }
                    catch(Exception e)
                    {
                        throw new Exception($"Property {propertyName} with value {propertyValue} was not a string");
                    }
                   
                }

                /* is an integer */
                if (propertyType == typeof(int?) || propertyType == typeof(int))
                {
                    try
                    {
                        propertyInfo.SetValue(output, int.Parse(propertyValue));
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Property {propertyName} with value {propertyValue} was not a integer");
                    }
                }

                /* Is a double */
                if (propertyType == typeof(double?) || propertyType == typeof(double))
                {
                    try
                    {
                        propertyInfo.SetValue(output, double.Parse(propertyValue));
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Property {propertyName} with value {propertyValue} was not a double");
                    }
                }

                /* Is an enum */
                if (propertyType.IsEnum) {

                    try
                    {
                        propertyInfo.SetValue(output, EnumFromString(propertyValue));
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Property {propertyName} with value {propertyValue} was not an valid enum");
                    }

                }
                /* Is a single enum */
                if (propertyType.IsEnum)
                {

                    try
                    {
                        propertyInfo.SetValue(output, EnumFromString(propertyValue));
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Property {propertyName} with value {propertyValue} was not an valid enum");
                    }

                }

                /* A List holding MULTIPLE Enums */
                /* These are the only ultra complex bits, these have to split a string, and entirely use reflection to find the correct enum */
                if (propertyType.FullName.Contains("List") && !propertyType.FullName.Contains("Tuple"))
                {

                    string[] splitPropertyValues = propertyValue.Split("|");

                    foreach(string splitPropertyValue in splitPropertyValues)
                    {
                       

                        var enumList = propertyInfo.GetValue(output);
                        var enumValue = EnumFromString(splitPropertyValue);

                        if (enumValue == null)
                        {
                            throw new EnumNotFoundException($"Enum Value {splitPropertyValue} was not found");
                        }

                        try
                        {

                            if (enumList == null)
                            {
                                var listType = typeof(List<>).MakeGenericType(enumValue.GetType());
                                enumList = Activator.CreateInstance(listType);
                            }

                            enumList.GetType().GetMethod("Add").Invoke(enumList, new[] { enumValue });

                            propertyInfo.SetValue(output, enumList);
                        }
                        catch (Exception e)
                        {

                            throw new Exception($"Error parsing {propertyName} with enum/s {propertyValue}");
                        }
                    }

                }

                /* A List of Tuple<enum,int> ie, the enum has an int attached */
                if (propertyType.FullName.Contains("List") && propertyType.FullName.Contains("Tuple"))
                {

                    string[] splitPropertyValues = propertyValue.Split("|");

                    foreach (string splitPropertyValue in splitPropertyValues)
                    {
                       

                            var tupleList = propertyInfo.GetValue(output);
                            var splitEnumValues = splitPropertyValue.Split(",");
                            var enumValue = EnumFromString(splitEnumValues[0]);
                            int? intValue = null;

                            if (splitEnumValues.Count() == 2) {
                                intValue = int.Parse(splitEnumValues[1]);
                            }

                            if(enumValue == null)
                            {
                                throw new EnumNotFoundException($"Enum Value {splitEnumValues[0]} was not found");
                            }

                        try
                        {

                            Type turpleType = typeof(Tuple<,>).MakeGenericType(enumValue.GetType(), typeof(int?));
                            object turpleValue = Activator.CreateInstance(turpleType, enumValue, intValue);

                            if (tupleList == null)
                            {
                                var listType = typeof(List<>).MakeGenericType(turpleValue.GetType());
                                tupleList = Activator.CreateInstance(listType);
                            }

                            tupleList.GetType().GetMethod("Add").Invoke(tupleList, new[] { turpleValue });

                            propertyInfo.SetValue(output, tupleList);
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"Error parsing {propertyName} with enum/s {propertyValue}");
                        }
                    }

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
        private object GenerateObject(string type = null)
        {

            if (type == null)
                type = Line().ToLower();


            Type objectType = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "MB2_Workbench.Classes" && t.Name.ToLower() == type.ToLower() || t.FullName.ToLower() == type.ToLower())
                      .ToList().FirstOrDefault();

            if (objectType == null)
            {

                /* Try without the number to see if we get a match*/
                objectType = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "MB2_Workbench.Classes" && t.Name.ToLower() == Regex.Replace(type.ToLower(), @"[\d-]", string.Empty).ToLower() || t.FullName.ToLower() == Regex.Replace(type.ToLower(), @"[\d-]", string.Empty).ToLower())
                      .ToList().FirstOrDefault();

                if (objectType == null)
                {
                    throw new Exception($"Unable to find class for MB2_Workbench.Classes.{ type }");
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
            name = name.Trim();

            /* FIX: a common spelling mistake in imported files */
            if (name.ToLower().Contains("missle"))
                name = name.ToLower().Replace("missle", "missile");

            if (name.ToLower() == "customclip")
                name = "clipsize";

            if (name.ToLower() == "bpmodifier")
                name = "BPMultiplier";

            if (name.ToLower() == "apmodifier")
                name = "APMultiplier";

            if (name.ToLower() == "ammo")
                name = "customAmmo";

            if (name.ToLower() == "startarmor")
                name = "armor";

            if(name.ToLower() == "altflashsound")
                name = "AltFlashSound1";

            if (name.ToLower() == "redcustomicon")
                name = "redicon";

            if (name.ToLower() == "bluecustomicon")
                name = "blueicon";

            if (name.ToLower() == "speccustomicon")
                name = "specicon";

            /* FIX: as automap and automap0 are different but identify the same otherwise */
            if (name.ToLower() == "automap")
                name = "automaps";
            return name;

        }

        /* We pass in enum string and this finds the enum */
        private object EnumFromString(string value)
        {

            /* Fetch all Enums */
            List<Type> enumTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace == "MB2_Workbench.Enums").ToList();
 
            object enumValue = null;

            /* Loop Until we find the correct one */
            foreach(Type enumType in enumTypes)
            {
                System.Enum.TryParse(enumType, value, out enumValue);
                if (enumValue != null)
                {
                    enumValue = Convert.ChangeType(enumValue, enumType);
                    break;

                }

            }

            return enumValue;

        }

    }
}
