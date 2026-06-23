using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimworldReadableNumbers.Utility
{
    public static class Processing
    {
        public static (bool isSuccess, object[] modifiedObjects) ProcessPatchArguments(ref object[] arguments)
        {
            if (arguments == null || arguments.Length > byte.MaxValue) return (false, null);
            object[] modifiedObjects = new object[arguments.Length];
            bool hasModified = false;
            for(byte i = 0; i < arguments.Length; i++)
            {
                // Skip if argument is not a string or NamedArgument
                if (Validation.IsAllowedType(ref arguments[i]) == false) continue;

                var formatNumberResult = Text.FormatNumber(ref arguments[i]);
                if (formatNumberResult.isSuccess)
                {
                    hasModified = true;
                    modifiedObjects[i] = formatNumberResult.formattedObject;
                }
                else
                {
                    modifiedObjects[i] = arguments[i];
                }
                
            }
            return (hasModified, modifiedObjects);
        }

        public static (bool isSuccess, IEnumerable<object> modifiedObjects) ProcessPatchArguments(ref IEnumerable<object> arguments)
        {
            if (arguments == null) return (false, null);
            arguments = arguments.ToArray();
            List<object> modifiedObjects = arguments.ToList();
            if (!arguments.Any() || arguments.Count() > byte.MaxValue) return (false, null);
            var firstArgument = arguments.First();
            if (Validation.IsAllowedType(ref firstArgument) == false) return (false, null);
            
            bool hasModified = false;
            for(byte i = 0; i < arguments.Count(); i++)
            {
                var arg = arguments.ElementAt(i);
                // Skip if argument is not a string or NamedArgument
                if (Validation.IsAllowedType(ref arg) == false) continue;

                var formatNumberResult = Text.FormatNumber(ref arg);
                if (formatNumberResult.isSuccess)
                {
                    hasModified = true;
                    modifiedObjects[i] = formatNumberResult.formattedObject;
                }
                else
                {
                    modifiedObjects[i] = arguments.ElementAt(i);
                }
            }
            return (hasModified, modifiedObjects);
        }

    }
}