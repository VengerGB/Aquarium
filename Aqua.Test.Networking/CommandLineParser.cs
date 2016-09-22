//-----------------------------------------------------------------------
// <copyright file="CommandLineParser.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>Yet another really crappy command line parser class.</summary>
//-----------------------------------------------------------------------

namespace NetworkingTest
{
    using System;

    /// <summary>
    ///     A really poor class to aid with command line parsing...
    /// </summary>
    public class CommandLineParser
    {
        #region Fields

        /// <summary>
        ///     Reference of command line
        /// </summary>
        private readonly string[] commandLineArguments;

        /// <summary>
        ///     Switches required for operation.
        /// </summary>
        private readonly char[] requiredSwitches;

        /// <summary>
        ///     Possible command line switch values
        /// </summary>
        private readonly char[] switchValues = {'/', '-'};

        /// <summary>
        ///     Usage method;
        /// </summary>
        private readonly Action usageMethod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     constructor accepts the existing commandline.
        /// </summary>
        /// <param name="args">Command Line</param>
        public CommandLineParser(string[] args)
        {
            commandLineArguments = args;
        }

        /// <summary>
        ///     constructor accepts the existing commandline.
        /// </summary>
        /// <param name="args">Command Line</param>
        public CommandLineParser(string[] args, char[] requiredSwitches, Action a)
        {
            commandLineArguments = args;
            this.requiredSwitches = requiredSwitches;
            usageMethod = a;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns true if the switch exists within the current command line.
        /// </summary>
        /// <param name="switchChar">specified char</param>
        /// <returns>returns true if exists</returns>
        public bool Exists(char switchChar)
        {
            bool returnValue = false;

            foreach (char switchValue in switchValues)
            {
                string fullSwitchName = string.Format("{0}{1}", switchValue, switchChar);

                foreach (string argument in commandLineArguments)
                {
                    if (argument == fullSwitchName)
                    {
                        returnValue = true;
                        break;
                    }
                }

                if (returnValue)
                {
                    break;
                }
            }

            return returnValue;
        }

        /// <summary>
        ///     Returns true if the switch exists
        /// </summary>
        /// <param name="arg">specified full-text switch</param>
        /// <returns>True if it exists</returns>
        public bool Exists(string arg)
        {
            bool returnValue = false;

            foreach (char switchValue in switchValues)
            {
                string fullSwitchName = string.Format("{0}{1}", switchValue, arg);

                foreach (string argument in commandLineArguments)
                {
                    if (argument == fullSwitchName)
                    {
                        returnValue = true;
                        break;
                    }
                }

                if (returnValue)
                {
                    break;
                }
            }

            return returnValue;
        }

        /// <summary>
        ///     Returns the string just after the specified char switch value
        /// </summary>
        /// <param name="switchCharacter">char value</param>
        /// <returns>string</returns>
        public string GetStringForSwitch(char switchCharacter)
        {
            string returnString = string.Empty;
            int index = 0;

            foreach (char switchValue in switchValues)
            {
                string fullSwitchName = string.Format("{0}{1}", switchValue, switchCharacter);

                index = 0;
                foreach (string arguement in commandLineArguments)
                {
                    if (arguement == fullSwitchName)
                    {
                        if (commandLineArguments.Length > index + 1)
                        {
                            returnString = commandLineArguments[index + 1];
                            break;
                        }
                        // There isn't a corresponding value for this switch in the arguements...
                        returnString = string.Empty;
                    }

                    index++;
                }

                if (!string.IsNullOrEmpty(returnString))
                {
                    break;
                }
            }

            return returnString;
        }

        public void Validate()
        {
            foreach (char requiredSwitch in requiredSwitches)
            {
                if (!Exists(requiredSwitch))
                {
                    usageMethod();
                    Environment.Exit(-1);
                }
            }
        }

        #endregion
    }
}