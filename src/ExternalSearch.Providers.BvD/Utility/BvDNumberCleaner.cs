using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CluedIn.ExternalSearch.Providers.BvD.Utility
{
    public class BvDNumberCleaner
    {

        public string CheckBvDNumber(string toCheck)
        {

            var defCCode = "DK";

            var vatexp = new string[]
            {
               "^(AT)(U\\d{8})$",
               "^(BE)(0?\\d{9})$",
               "^(BG)(\\d{9,10})$",
               "^(CHE)(\\d{9})(MWST)?$",
               "^(CY)([0-59]\\d{7}[A-Z])$",
               "^(CZ)(\\d{8,10})(\\d{3})?$",
               "^(DE)([1-9]\\d{8})$",
               "^(DK)(\\d{8})$",
               "^(EE)(10\\d{7})$",
               "^(EL)(\\d{9})$",
               "^(ES)([A-Z]\\d{8})$",
               "^(ES)([A-HN-SW]\\d{7}[A-J])$",
               "^(ES)([0-9YZ]\\d{7}[A-Z])$",
               "^(ES)([KLMX]\\d{7}[A-Z])$",
               "^(EU)(\\d{9})$",
               "^(FI)(\\d{8})$",
               "^(FR)(\\d{11})$",
               "^(FR)([A-HJ-NP-Z]\\d{10})$",
               "^(FR)(\\d[A-HJ-NP-Z]\\d{9})$",
               "^(FR)([A-HJ-NP-Z]{2}\\d{9})$",
               "^(GB)?(\\d{9})$",
               "^(GB)?(\\d{12})$",
               "^(GB)?(GD\\d{3})$",
               "^(GB)?(HA\\d{3})$",
               "^(HR)(\\d{11})$",
               "^(HU)(\\d{8})$",
               "^(IE)(\\d{7}[A-W])$",
               "^(IE)([7-9][A-Z\\*\\+)]\\d{5}[A-W])$",
               "^(IE)(\\d{7}[A-W][AH])$",
               "^(IT)(\\d{11})$",
               "^(LV)(\\d{11})$",
               "^(LT)(\\d{9}|\\d{12})$",
               "^(LU)(\\d{8})$",
               "^(MT)([1-9]\\d{7})$",
               "^(NL)(\\d{9})B\\d{2}$",
               "^(NO)(\\d{9})$",
               "^(PL)(\\d{10})$",
               "^(PT)(\\d{9})$",
               "^(RO)([1-9]\\d{1,9})$",
               "^(RU)(\\d{10}|\\d{12})$",
               "^(RS)(\\d{9})$",
               "^(SI)([1-9]\\d{7})$",
               "^(SK)([1-9]\\d[2346-9]\\d{7})$",
               "^(SE)(\\d{10}01)$"
            };

            var VATNumber = toCheck.ToUpper();

            VATNumber = Regex.Replace(VATNumber, @"[^a-zA-Z0-9]", string.Empty);

            var valid = "";

            for(var i = 0; i<vatexp.Length; i++)
            {
                var test1 = vatexp[i];
                var test2 = VATNumber;
                var matches = Regex.Match(VATNumber, vatexp[i]);
                if (matches.Success)
                {

                    var cCode = matches.Groups[1].Value;                        
                    var cNumber = matches.Groups[2].Value;         
                    if (cCode.Length == 0)
                        cCode = defCCode;

                    if (Evaluate(cCode + "VATCheckDigit", cNumber))
                    {
                        valid = VATNumber;
                    }

                    break;
                }
            }
            return valid;
        }

        public bool ATVATCheckDigit(string vatnumber)
        {
            var total = 0.0;
            var multipliers = new int[]{
                1,2,1,2,1,2,1
            };

            for (var i = 0; i < 7; i++)
            {
                var temp = Convert.ToDouble(vatnumber[i] * multipliers[i]);
                if (temp > 9)
                {
                    total += Math.Floor(temp/10) + temp % 10;
                }
                else
                {
                    total += temp;
                }
            }

            total = 10 - (total + 4) % 10;
            if (total == 10)
            {
                total = 0;
            }

            if (total.ToString() == vatnumber.Substring(7,1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BEVATCheckDigit(string vatnumber)
        {

            if (vatnumber.Length == 9)
            {
                vatnumber = "0" + vatnumber;
            }

            if (vatnumber.Substring(1, 2).Equals(0))
            {
                return false;
            }

            if (97 - int.Parse(vatnumber.Substring(0,8)) % 97 == int.Parse(vatnumber.Substring(8,2)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BGVATCheckDigit(string vatnumber)
        {
            var total = 0;

            if (vatnumber.Length == 9)
            {

                var temp = 0;
                for (var i = 0; i < 8; i++)
                {
                    temp += int.Parse(vatnumber[i].ToString()) * (i + 1);
                }

                total = temp % 11;
                if (total != 10)
                {
                    if (total == int.Parse(vatnumber.Substring(8)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                var temp2 = 0.0;
                for (var i = 0; i < 8; i++)
                    temp2 += int.Parse(vatnumber[i].ToString()) * (i + 3);

                total = temp % 11;
                if (total == 10)
                {
                    total = 0;
                }

                if (total == int.Parse(vatnumber.Substring(8)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            var match = Regex.Match(vatnumber, "(^\\d\\d[0-5]\\d[0-3]\\d\\d{4}$)");
            var multipliers = new int[]{
                        2, 4, 8, 5, 10, 9, 7, 3, 6
                    };

            var total2 = 0;
            if (match.Success) {

                var month = double.Parse((vatnumber.Substring(2,2)));
                if ((month > 0 && month < 13) || (month > 20 && month < 33) || (month > 40 && month < 53))
                {

                    for (var i = 0; i < 9; i++)
                        total2 += int.Parse(vatnumber[i].ToString()) * multipliers[i];

                    total2 = total2 % 11;
                    if (total2 == 10)
                        total2 = 0;

                    if (total2 == int.Parse(vatnumber.Substring(9, 1)))
                        return true;
                }
            }

            var multipliers2 = new int[]{
                21, 19, 17, 13, 11, 9, 7, 3, 1
            };
            var total3 = 0;
            for (var i = 0; i < 9; i++)
                total3 += int.Parse(vatnumber[i].ToString()) * multipliers[i];

            if (total3 % 10 == int.Parse(vatnumber.Substring(9, 1)))
                return true;

            var multipliers3 = new int[]{
                4, 3, 2, 7, 6, 5, 4, 3, 2
            };
            var total4 = 0.0;
            for (var i = 0; i < 9; i++)
                total4 += int.Parse(vatnumber[i].ToString()) * multipliers[i];

            total4 = 11 - total4 % 11;
            if (total4 == 10)
                return false;
            if (total4 == 11)
                total4 = 0;

            if (total4 == int.Parse(vatnumber.Substring(9, 1)))
                return true;
            else
                return false;
        }

        public bool CHEVATCheckDigit(string vatnumber)
        {

            var multipliers = new int[]{
                5, 4, 3, 2, 7, 6, 5, 4
            };
            var total = 0;
            for (var i = 0; i < 8; i++)
            {
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];
            }

            total = 11 - total % 11;
            if (total == 10)
            {
                return false;
            }

            if (total == 11)
            {
                total = 0;
            }

            return total == int.Parse(vatnumber.Substring(8, 1)) ? true : false;
        }

        public bool CYVATCheckDigit(string vatnumber)
        {
            if (int.Parse(vatnumber.Substring(0, 2)) == 12)
            {
                return false;
            }

            var total = 0;
            for (var i = 0; i < 8; i++)
            {
                var temp = int.Parse(vatnumber[i].ToString());
                if (i % 2 == 0)
                {
                    switch (temp)
                    {
                        case 0:
                            temp = 1;
                            break;
                        case 1:
                            temp = 0;
                            break;
                        case 2:
                            temp = 5;
                            break;
                        case 3:
                            temp = 7;
                            break;
                        case 4:
                            temp = 9;
                            break;
                        default:
                            temp = temp * 2 + 3;
                            break;
                    }
                }
                total += temp;
            }

            total = total % 26;
            var totalChar = Convert.ToChar(total + 65);

            return totalChar.ToString() == vatnumber.Substring(8, 1) ? true : false;

        }

        public bool CZVATCheckDigit(string vatnumber)
        {

            var total = 0.0;
            var multipliers = new int[]{
                8, 7, 6, 5, 4, 3, 2
            };

            var czexp = new string[] {
                "^\\d{8}$", "^[0-5][0-9][0|1|5|6]\\d[0-3]\\d\\d{3}$", "^6\\d{8}$", "^\\d{2}[0-3|5-8]\\d[0-3]\\d\\d{4}$"
            };

            var legalMatch = Regex.Match(vatnumber, czexp[0]);
            var type1Match = Regex.Match(vatnumber, czexp[1]);
            var type2Match = Regex.Match(vatnumber, czexp[2]);
            var type3Match = Regex.Match(vatnumber, czexp[3]);

            if (legalMatch.Success) {

                for (var i = 0; i < 7; i++)
                {
                    total += double.Parse(vatnumber[i].ToString()) * multipliers[i];
                }

                total = 11 - total % 11;
                if (total == 10)
                {
                    total = 0;
                }

                if (total == 11)
                {
                    total = 1;
                }

                return total == double.Parse(vatnumber.Substring(7, 1)) ? true : false;
            }

            else if (type1Match.Success) {
                return (double.Parse(vatnumber.Substring(0, 2)) > 53) ? false : true;
            }

            else if (type2Match.Success) {

                for (var i = 0; i < 7; i++)
                {
                    total += double.Parse(vatnumber[i + 1].ToString()) * multipliers[i];
                }

                total = 11 - total % 11;
                if (total == 10)
                {
                    total = 0;
                }

                if (total == 11)
                {
                    total = 1;
                }
                var lookup = new int[]{
                    8, 7, 6, 5, 4, 3, 2, 1, 0, 9, 10
                };
                return lookup[(int)total - 1] == int.Parse(vatnumber.Substring(8, 1)) ? true : false;
            }
  
            else if (type3Match.Success) {
            var temp = double.Parse(vatnumber.Substring(0, 2)) + double.Parse(vatnumber.Substring(2, 2)) + double.Parse(vatnumber.Substring(4, 2)) + double.Parse(vatnumber.Substring(6, 2)) + double.Parse(vatnumber.Substring(8));
                return temp % 11 == 0 && double.Parse(vatnumber) % 11 == 0 ? true : false;
            }

            return false;
        }

        public bool DEVATCheckDigit(string vatnumber)
        {
            var product = 10;
            var sum = 0;
            var checkdigit = 0;
            for (var i = 0; i < 8; i++)
            {

                sum = (int.Parse(vatnumber[i].ToString()) + product) % 10;
                if (sum == 0)
                {
                    sum = 10;
                }
                product = (2 * sum) % 11;
            }

            if (11 - product == 10)
            {
                checkdigit = 0;
            }
            else
            {
                checkdigit = 11 - product;
            }

            return checkdigit == int.Parse(vatnumber.Substring(8, 1)) ? true : false;
        }

        public bool DKVATCheckDigit(string vatnumber)
        {

            var total = 0;
            var multipliers = new int[]{
                2, 7, 6, 5, 4, 3, 2, 1
            };
            for (var i = 0; i < 8; i++)
            {
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];
                total = total % 11;
            }
            if (total == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EEVATCheckDigit(string vatnumber)
        {
            var total = 0;
            var multipliers = new int[]{
                3, 7, 1, 3, 7, 1, 3, 7
            };

            for (var i = 0; i < 8; i++)
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];

            total = 10 - total % 10;
            if (total == 10)
                total = 0;
            
            return total == int.Parse(vatnumber.Substring(8, 1)) ? true : false;
        }

        public bool ELVATCheckDigit(string vatnumber)
        {
            var total = 0;
            var multipliers = new int[]{
                256, 128, 64, 32, 16, 8, 4, 2
            };

            if (vatnumber.Length == 8)
            {
                vatnumber = "0" + vatnumber;
            }

            for (var i = 0; i < 8; i++)
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];
           
            total = total % 11;
            if (total > 9)
            { total = 0; };

            return total == int.Parse(vatnumber.Substring(8, 1)) ? true : false;
        }

        public bool ESVATCheckDigit(string vatnumber)
        {
            var total = 0.0;
            var temp = 0.0;
            var multipliers = new int[]{
                2, 1, 2, 1, 2, 1, 2
            };
            var esexp = new string[] {
                "^[A-H|J|U|V]\\d{8}$","^[A-H|N-S|W]\\d{7}[A-J]$","^[0-9|Y|Z]\\d{7}[A-Z]$","^[K|L|M|X]\\d{7}[A-Z]$"
            };

            var juridicalNationalMatch = Regex.Match(vatnumber, esexp[0]);
            var juridicalNonNationalMatch = Regex.Match(vatnumber, esexp[1]);
            var personalNumberMatch1 = Regex.Match(vatnumber, esexp[2]);
            var personalNumberMatch2 = Regex.Match(vatnumber, esexp[3]);

            if (juridicalNationalMatch.Success) {

                for (var i = 0; i < 7; i++) {
                    temp = double.Parse(vatnumber[i+1].ToString()) * multipliers[i];
                    if (temp > 9)
                        total += Math.Floor(temp / 10) + temp % 10;
                    else
                        total += temp;
                }

                total = 10 - total % 10;
                if (total == 10) {
                    total = 0;
                }

                return total == double.Parse(vatnumber.Substring(8, 1)) ? true : false;
            }

            else if (juridicalNonNationalMatch.Success)
            {
                for (var i = 0; i < 7; i++)
                {
                    temp = double.Parse(vatnumber[i + 1].ToString()) * multipliers[i];
                    if (temp > 9)
                        total += Math.Floor(temp / 10) + temp % 10;
                    else
                        total += temp;
                }

                total = 10 - total % 10;
                var totalChar = Convert.ToChar(total + 64);

                return totalChar.ToString() == vatnumber.Substring(8, 1) ? true : false;
            }

            else if (personalNumberMatch1.Success)
            {
                var tempnumber = vatnumber;
                if (tempnumber.Substring(0, 1) == "Y")
                    tempnumber = tempnumber.Replace('Y', '1');
                if (tempnumber.Substring(0, 1) == "Z")
                    tempnumber = tempnumber.Replace('Z', '2');
                return tempnumber[8] == "TRWAGMYFPDXBNJZSQVHLCKE"[int.Parse(tempnumber.Substring(0, 8)) % 23];
            }

            else if (personalNumberMatch2.Success)
            {
                return vatnumber[8] == "TRWAGMYFPDXBNJZSQVHLCKE"[int.Parse(vatnumber.Substring(1, 8)) % 23];
            }

            else
            {
                return false;
            }
        }

        public bool FIVATCheckDigit(string vatnumber)
        {
            var total = 0;
            var multipliers = new int[]{
                7, 9, 10, 5, 8, 4, 2
            };
            
            for (var i = 0; i < 7; i++)
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];

            total = 11 - total % 11;
            if (total > 9)
            {
                total = 0;
            };

            return total == int.Parse(vatnumber.Substring(7, 1)) ? true : false;
        }

        public bool FRVATCheckDigit(string vatnumber)
        {

            var match = Regex.Match(vatnumber, "(\\d{11})$");
            if (match.Success)
            {
                return true;
            }

            var total = double.Parse(vatnumber.Substring(2));

            total = (total * 100 + 12) % 97;

            if (total.ToString() == vatnumber.Substring(1, 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GBVATCheckDigit(string vatnumber)
        {

            var multipliers = new int[]{
            8, 7, 6, 5, 4, 3, 2
            };

            if (vatnumber.Substring(0, 2) ==  "GD")
            {
            if (int.Parse(vatnumber.Substring(2, 1)) < 500)
            {
                return true;
            }
            else
            {
                return false;
            }
            }

            if (vatnumber.Substring(0, 2) == "HA")
            {
            if (int.Parse(vatnumber.Substring(2, 3)) > 499)
            {
                return true;
            }
            else
            {
                return false;
            }
            }

            var total = 0.0;

            if (int.Parse(vatnumber.Substring(0)) == 0)
            return false;

            var no = int.Parse(vatnumber.Substring(0, 7));

            for (var i = 0; i < 7; i++)
            total += Convert.ToDouble(int.Parse(vatnumber[i].ToString()) * multipliers[i]);

            var cd = total;
            while (cd > 0)
            { cd = cd - 97; }

            cd = Math.Abs(cd);
            if (cd == int.Parse(vatnumber.Substring(7, 2)) && no < 9990001 && (no < 100000 || no > 999999) && (no < 9490001 || no > 9700000))
            {
            return true;
            }

            if (cd >= 55)
            {
            cd = cd - 55;
            }
            else
            {
            cd = cd + 42;
            }

            if (cd == int.Parse(vatnumber.Substring(7, 2)))
            {
            return true;
            }
            else
            {
            return false;
            }
        }

        public bool HRVATCheckDigit(string vatnumber)
        {
            var product = 10;
            var sum = 0;

            for (var i = 0; i < 10; i++)
            {

                sum = (int.Parse(vatnumber[i].ToString()) + product) % 10;
                if (sum == 0)
                {
                    sum = 10;
                }
                product = (2 * sum) % 11;
            }

            return (product + int.Parse(vatnumber.Substring(10, 1)) * 1) % 10 == 1 ? true : false;
        }

        public bool HUVATCheckDigit(string vatnumber)
        {
            var total = 0;
            var multipliers = new int[]{
                9, 7, 3, 1, 9, 7, 3
            };

            for (var i = 0; i < 7; i++)
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];

            total = 10 - total % 10;
            if (total == 10)
                total = 0;

            return total == int.Parse(vatnumber.Substring(7, 1)) ? true : false;
        }

        public bool IEVATCheckDigit(string vatnumber)
        {
            var total = 0;
            var multipliers = new int[]{
                8, 7, 6, 5, 4, 3, 2
            };
            var match = Regex.Match(vatnumber, "^\\d[A-Z\\*\\+]");
            var match2 = Regex.Match(vatnumber, "^\\d{7}[A-Z][AH]$");
            var totalChar = '\0';
            if (match.Success) {
                vatnumber = "0" + vatnumber.Substring(2, 7) + vatnumber.Substring(0, 1) + vatnumber.Substring(7, 1);
            }
            for (var i = 0; i < 7; i++)
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];

            if (match2.Success) {

                if (vatnumber[8] == 'H')
                    total += 72;
                else
                    total += 9;    
            }

            total = total % 23;
            if (total == 0)
            {
                total = 'W';
            }
            else
            {
                totalChar = Convert.ToChar(total + 64);
            }

            return totalChar.ToString() == vatnumber.Substring(7, 1) ? true : false;
        }

        public bool ITVATCheckDigit(string vatnumber)
        {
            var total = 0.0;
            var multipliers = new int[]{
                1, 2, 1, 2, 1, 2, 1, 2, 1, 2
            };
            var temp = 0.0;
            
            if (double.Parse(vatnumber.Substring(0, 7)) == 0)
                return false;
            temp = double.Parse(vatnumber.Substring(7, 3));
            if ((temp < 1) || (temp > 201) && temp != 999 && temp != 888)
                return false;

            for (var i = 0; i < 10; i++)
            {
                temp = double.Parse(vatnumber[i].ToString()) * multipliers[i];
                if (temp > 9)
                    total += Math.Floor(temp / 10) + temp % 10;
                else
                    total += temp;
            }

            total = 10 - total % 10;
            if (total > 9)
            { total = 0; };

            return total == double.Parse(vatnumber.Substring(10, 1)) ? true : false;
        }

        public bool LTVATCheckDigit(string vatnumber)
        {
            if (vatnumber.Length == 9)
            {
                var match = Regex.Match(vatnumber, "(^\\d{7}1)");
                if (!match.Success)
                {
                    return false;
                }

                var total = 0;
                for (var i = 0; i < 8; i++)
                    total += int.Parse(vatnumber[i].ToString()) * (i + 1);

                if (total % 11 == 10)
                {
                    var multipliers = new int[]{
                        3, 4, 5, 6, 7, 8, 9, 1
                    };
                    total = 0;
                    for (var i = 0; i < 8; i++)
                    {
                        total += int.Parse(vatnumber[i].ToString()) * multipliers[i];
                    }
                }

                total = total % 11;
                if (total == 10)
                {
                    total = 0;
                }
                return total == int.Parse(vatnumber.Substring(8, 1)) ? true : false;
            }
            else
            {

                var match2 = Regex.Match(vatnumber, "(^\\d{10}1)");
                if (!match2.Success)
                {
                    return false;
                }

                var total = 0;
                var multipliers = new int[]{
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2
                };
                for (var i = 0; i < 11; i++)
                    total += int.Parse(vatnumber[i].ToString()) * multipliers[i];

                if (total % 11 == 10)
                {
                    var multipliers2 = new int[]{
                        3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4
                    };
                    total = 0;
                    for (var i = 0; i < 11; i++)
                        total += int.Parse(vatnumber[i].ToString()) * multipliers2[i];
                }

                total = total % 11;
                if (total == 10)
                { total = 0; };

                return total == int.Parse(vatnumber.Substring(11, 1)) ? true : false;
            }
        }

        public bool LUVATCheckDigit(string vatnumber)
        {
            return int.Parse(vatnumber.Substring(0, 6)) % 89 == int.Parse(vatnumber.Substring(6, 2)) ? true : false;
        }

        public bool LVVATCheckDigit(string vatnumber)
        {
            var match = Regex.Match(vatnumber, "(^[0-3])");
            if(match.Success)
            {
                var match2 = Regex.Match(vatnumber, "(^[0 - 3][0 - 9][0 - 1][0 - 9])");
                return match2.Success ? true : false;
            }

            else
            {

                var total = 0;
                var multipliers = new int[]{
                    9, 1, 4, 8, 3, 10, 2, 5, 7, 6
                };

                for (var i = 0; i < 10; i++)
                    total += int.Parse(vatnumber[i].ToString()) * multipliers[i];

                if (total % 11 == 4 && vatnumber[0] == 9)
                    total = total - 45;
                if (total % 11 == 4)
                    total = 4 - total % 11;
                else if (total % 11 > 4)
                    total = 14 - total % 11;
                else if (total % 11 < 4)
                    total = 3 - total % 11;

                return total == int.Parse(vatnumber.Substring(10, 1)) ? true : false;
            }
        }

        public bool MTVATCheckDigit(string vatnumber)
        {
            var total = 0;
            var multipliers = new int[]{
                3, 4, 6, 7, 8, 9
            };

            for (var i = 0; i < 6; i++)
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];

            total = 37 - total % 37;

            return total == int.Parse(vatnumber.Substring(6, 2)) * 1 ? true : false;
        }

        public bool NLVATCheckDigit(string vatnumber)
        {

            var total = 0.0;
            var multipliers = new int[]{
                9, 8, 7, 6, 5, 4, 3, 2
            };

            for (var i = 0; i < 8; i++)
                total += double.Parse(vatnumber[i].ToString()) * multipliers[i];

            total = total % 11;
            if (total > 9)
            { total = 0; };

            if (total == double.Parse(vatnumber.Substring(8, 1)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NOVATCheckDigit(string vatnumber)
        {
            var total = 0;
            var multipliers = new int[]{
                3, 2, 7, 6, 5, 4, 3, 2
            };

            for (var i = 0; i < 8; i++)
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];
            total = 11 - total % 11;
            if (total == 11)
            { total = 0; }
            if (total < 10)
            {
                return total == int.Parse(vatnumber.Substring(8, 1)) ? true : false;
            }

            return false;
        }

        public bool PLVATCheckDigit(string vatnumber)
        {
            var total = 0;
            var multipliers = new int[]{
                6, 5, 7, 2, 3, 4, 5, 6, 7
            };

            for (var i = 0; i < 9; i++)
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];

            total = total % 11;
            if (total > 9)
            {
                total = 0;
            }

            return total == int.Parse(vatnumber.Substring(9, 1)) ? true : false;
        }

        public bool PTVATCheckDigit(string vatnumber)
        {
            var total = 0;
            var multipliers = new int[]{
                9, 8, 7, 6, 5, 4, 3, 2
            };

            for (var i = 0; i < 8; i++)
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];

            total = 11 - total % 11;
            if (total > 9)
            {
                total = 0;
            }

            return total == int.Parse(vatnumber.Substring(8, 1)) ? true : false;
        }

        public bool ROVATCheckDigit(string vatnumber)
        {
            int[] multipliers = new int[]{
                7, 5, 3, 2, 1, 7, 5, 3, 2
            };

            var VATlen = vatnumber.Length;
            var calculation = 10 - VATlen;
            var slice = multipliers.Slice(calculation, multipliers.Length);
            var total = 0;
            for (var i = 0; i < vatnumber.Length - 1; i++)
            {
                total += int.Parse(vatnumber[i].ToString()) * slice[i];
            }

            total = (10 * total) % 11;
            if (total == 10)
                total = 0;

            return total == int.Parse(vatnumber.Substring(vatnumber.Length - 1, 1)) ? true : false;
        }

        public bool RSVATCheckDigit(string vatnumber)
        {
            var product = 10;
            var sum = 0;;

            for (var i = 0; i < 8; i++)
            {

                sum = (int.Parse(vatnumber[i].ToString()) + product) % 10;
                if (sum == 0)
                {
                    sum = 10;
                }
                product = (2 * sum) % 11;
            }

            return (product + int.Parse(vatnumber.Substring(8, 1)) * 1) % 10 == 1 ? true : false;
        }

        public bool RUVATCheckDigit(string vatnumber)
        {
            if (vatnumber.Length == 10)
            {
                var total = 0;
                var multipliers = new int[]
                {
                    2, 4, 10, 3, 5, 9, 4, 6, 8, 0
                };

                for (var i = 0; i < 10; i++)
                {
                    total += int.Parse(vatnumber[i].ToString()) * multipliers[i];
                }
                total = total % 11;

                if (total > 9)
                {
                        total = total % 10;
                }

                    return total == int.Parse(vatnumber.Substring(9, 1)) ? true : false;
                }
            else if (vatnumber.Length == 12)
            {
                var total1 = 0;

                var multipliers1 = new int[]
                {
                    7, 2, 4, 10, 3, 5, 9, 4, 6, 8, 0
                };

                var total2 = 0;

                var multipliers2 = new int[]
                {
                    3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8, 0
                };
      
                for (var i = 0; i < 11; i++)
                            total1 += int.Parse(vatnumber[i].ToString()) * multipliers1[i];
                        total1 = total1 % 11;
      
                if (total1 > 9)
                {
                        total1 = total1 % 10;
                }

                    for (var i = 0; i < 11; i++)
                        total2 += int.Parse(vatnumber[i].ToString()) * multipliers2[i];
                    total2 = total2 % 11;
      
                if (total2 > 9)
                {              
                            total2 = total2 % 10;

                }

                return (total1 == int.Parse(vatnumber.Substring(10, 1))) && (total2 == int.Parse(vatnumber.Substring(11, 1))) ? true : false;
            }

            return false;
        }

        public bool SEVATCheckDigit(string vatnumber)
        {


            var R = 0.0;
            var digit = 0.0;
            for (var i = 0; i < 9; i = i + 2)
            {
                digit = double.Parse(vatnumber[i].ToString());
                R += Math.Floor(digit / 5) + ((digit * 2) % 10);
            }

            var S = 0.0;
            for (var i = 1; i < 9; i = i + 2)
            {
                S += double.Parse(vatnumber[i].ToString());
            }

            var cd = (10 - (R + S) % 10) % 10;

            if (cd == int.Parse(vatnumber.Substring(9, 1)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SIVATCheckDigit(string vatnumber)
        {
            var total = 0;
            var multipliers = new int[]{
                8, 7, 6, 5, 4, 3, 2
            };

            for (var i = 0; i < 7; i++)
                total += int.Parse(vatnumber[i].ToString()) * multipliers[i];

            total = 11 - total % 11;
            if (total == 10)
            { total = 0; };

            return total != 11 && total == int.Parse(vatnumber.Substring(7, 1)) ? true : false;
        }

        public bool SKVATCheckDigit(string vatnumber)
        {
            return int.Parse(vatnumber) % 11 == 0 ? true : false;
        }

        public bool Evaluate(string expression, string vat)
        {
            var type = GetType();
            var method = type.GetMethod(expression);
            object result = null;
            result = method.Invoke(this, new object[]{
                vat
            });
            return (bool)result;

        }

    }

    public static class Extensions
    {

        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;

            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }
    }
}
