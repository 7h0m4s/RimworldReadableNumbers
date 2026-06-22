using System;
using Verse;

namespace RimworldReadableNumbers.Utility
{
    public static class Processing
    {
        public static object[] ProcessArguments(ref object[] arguments)
        {
            bool hasModified = false;
            var argumentCounter = -1; // start at -1 so we can increment Counter at the start of the loop
            var modifiedArguments = new object[arguments.Length];
            foreach (var argument in arguments)
            {
                argumentCounter++;
                modifiedArguments[argumentCounter] = argument;
                string stringArg = null;
                Type argType = null;
                // Skip if argument is not a string or NamedArgument
                if (argument is string == false && argument is NamedArgument == false) {continue;}
                
                // Skip if is NamedArgument but doesn't contain a string
                else if(argument is NamedArgument && ((NamedArgument)argument).arg is string == false) {continue;}
                else if (argument is string)
                {
                    argType = typeof(string);
                    stringArg = argument as string;
                }
                else if (argument is NamedArgument)
                {
                    argType = typeof(NamedArgument);
                    stringArg = (string)((NamedArgument)argument).arg;
                }

                if (stringArg == null) continue; // Here for redundancy
                if (!Validation.IsValidNumberToConvert(stringArg)) continue;

                 if(Utility.Text.FormatNumber(ref stringArg))
                 {
                     if (argType == typeof(string))
                     {
                         modifiedArguments[argumentCounter] = stringArg;
                     }
                     else if (argType == typeof(NamedArgument))
                     {
                         var modifiedArgument = (NamedArgument)modifiedArguments[argumentCounter];
                         modifiedArgument.arg = stringArg;
                         modifiedArguments[argumentCounter] = modifiedArgument;
                     }
                     else
                     {
                         continue;
                     }
                     hasModified = true;
                 }
            }
            return hasModified ? modifiedArguments : null;
        }
    }
}