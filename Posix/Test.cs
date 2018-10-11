using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Posix
{
    class Test
    {
        public enum TestType { isBlockDevice, isCharacterDevice, isDirectory,
            isExtant, isFile, isSetGID, isSymlink, isNonEmptyString, isPipe,
            isReadable, isSocket, isNonEmptyFile, isTerminal, isSetUID,
            isWriteable, isExecutable, isZeroLength, isEqualString,
            isInequalString, isEqualNumber, isNotEqualNumber, isGreaterThan,
            isGreaterThanOrEqual, isLessThan, isLessThanOrEqual };

        public static bool TestOne(TestType testing, string toTest)
        {
            bool matchesCondition = false;

            switch (testing)
            {
                case TestType.isBlockDevice:
                    matchesCondition = IsBlockDevice(toTest);
                    break;
                case TestType.isCharacterDevice:
                    matchesCondition = IsCharacterDevice(toTest);
                    break;
                case TestType.isDirectory:
                    matchesCondition = IsDirectory(toTest);
                    break;
                case TestType.isExecutable:
                    matchesCondition = CheckPermission(toTest, System.Security.AccessControl.FileSystemRights.ExecuteFile);
                    break;
                case TestType.isExtant:
                    break;
                case TestType.isFile:
                    matchesCondition = IsFile(toTest);
                    break;
                case TestType.isNonEmptyFile:
                    if (IsFile(toTest))
                    {
                        matchesCondition = IsNonZeroLength(toTest);
                    }
                    else
                    {
                        matchesCondition = false;
                    }
                    break;
                case TestType.isNonEmptyString:
                    matchesCondition = (toTest.Length != 0) ? true : false;
                    break;
                case TestType.isPipe:
                    break;
                case TestType.isReadable:
                    matchesCondition = CheckPermission(toTest, System.Security.AccessControl.FileSystemRights.Read);
                    break;
                case TestType.isSetGID:
                    matchesCondition = false;
                    break;
                case TestType.isSetUID:
                    matchesCondition = false;
                    break;
                case TestType.isSocket:
                    matchesCondition = false;
                    break;
                case TestType.isSymlink:
                    matchesCondition = false;
                    break;
                case TestType.isTerminal:
                    matchesCondition = false;
                    break;
                case TestType.isWriteable:
                    matchesCondition = CheckPermission(toTest, System.Security.AccessControl.FileSystemRights.Write);
                    break;
                case TestType.isZeroLength:
                    matchesCondition = (toTest.Length == 0) ? true : false;
                    break;
                default:
                    throw new ArgumentException("Cannot perform specified action on a single item");
            }

            return matchesCondition;
        }

        public static bool TestTwo(string toCompare, string toCompareTo, TestType comparisonType)
        {
            bool matchesCondition = false;
            int compared;
            switch (comparisonType)
            {
                case TestType.isEqualString:
                    if (toCompare == toCompareTo)
                    {
                        matchesCondition = true;
                    }
                    break;
                case TestType.isInequalString:
                    if (toCompare != toCompareTo)
                    {
                        matchesCondition = true;
                    }
                    break;
                case TestType.isEqualNumber:
                    if (CompareNumberStrings(toCompare, toCompareTo) == 0)
                    {
                        matchesCondition = true;
                    }
                    break;
                case TestType.isNotEqualNumber:
                    if (CompareNumberStrings(toCompare, toCompareTo) != 0)
                    {
                        matchesCondition = true;
                    }
                    break;
                case TestType.isGreaterThan:
                    if (CompareNumberStrings(toCompare, toCompareTo) == 1)
                    {
                        matchesCondition = true;
                    }
                    break;
                case TestType.isGreaterThanOrEqual:
                    if (CompareNumberStrings(toCompare, toCompareTo) >= 0)
                    {
                        matchesCondition = true;
                    }
                    break;
                case TestType.isLessThan:
                    compared = CompareNumberStrings(toCompare, toCompareTo);
                    if (compared < 0 && compared != 2)
                    {
                        matchesCondition = true;
                    }
                    break;
                case TestType.isLessThanOrEqual:
                    compared = CompareNumberStrings(toCompare, toCompareTo);
                    if (compared <= 0 && compared != 2)
                    {
                        matchesCondition = true;
                    }
                    break;
            }
            return matchesCondition;
        }

        public static bool IsBlockDevice(string path)
        {
            // The CLR doesn't know about the existence of block devices, so returning false for now
            bool isBlock = false;

            return isBlock;
        }

        public static bool IsCharacterDevice(string path)
        {
            // The CLR doesn't know about the existence of block devices, so returning false for now
            bool isChar = false;

            return isChar;
        }

        public static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public static bool CheckPermission(string path, System.Security.AccessControl.FileSystemRights filePerm)
        {
            bool canPerform = false;
            System.Security.AccessControl.FileSecurity acl = File.GetAccessControl(path);
            string user = Environment.UserName;
            System.Security.Principal.NTAccount uid = new System.Security.Principal.NTAccount(user);
            foreach (System.Security.AccessControl.FileSystemAccessRule rule in acl.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
            {
                if (rule.IdentityReference == uid)
                {
                    if ((rule.FileSystemRights & filePerm) != 0)
                    {
                        canPerform = true;
                    }
                }
            }

            return canPerform;
        }

        static bool IsFile(string toCheck)
        {
            return File.Exists(toCheck);
        }

        static bool IsNonZeroLength(string fileName)
        {
            bool hasContent = false;

            if (new FileInfo(fileName).Length != 0)
            {
                hasContent = true;
            }
            
            return hasContent;
        }

        static int CompareNumberStrings(string numberOne, string numberTwo)
        {
            int result;
            int firstNumber;
            int secondNumber;

            bool isNumber;

            isNumber = int.TryParse(numberOne, out firstNumber);
            isNumber &= int.TryParse(numberTwo, out secondNumber);

            if (isNumber)
            {
                if (firstNumber > secondNumber)
                {
                    result = 1;
                }
                else if (firstNumber == secondNumber)
                {
                    result = 0;
                }
                else
                {
                    result = -1;
                }
            }
            else
            {
                result = -2;
            }
            return result;
        }
    }
}
