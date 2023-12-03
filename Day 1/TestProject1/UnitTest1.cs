namespace TestProject1
{
    using AdventOfCode2023;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow("two1nine", 29)]
        [DataRow("eightwothree", 83)]
        [DataRow("abcone2threexyz", 13)]
        [DataRow("xtwone3four", 24)]
        [DataRow("4nineeightseven2", 42)]
        [DataRow("zoneight234", 14)]
        [DataRow("7pqrstsixteen", 76)]
        [DataRow("bskfrcfvc5", 55)]
        [DataRow("2573", 23)]
        [DataRow("pfflhh6gvlrkrdscthree", 63)]
        [DataRow("4onethreekzpkpkpmxlpnsvqtlmtrsgznxkckrpsqskbz6", 46)]
        [DataRow("six777", 67)]
        [DataRow("22fourninetzfourfsnxjglthreeeight", 28)]
        [DataRow("mhcvqmsg7bdj", 77)]
        [DataRow("nine9", 99)]
        [DataRow("2one34threetwoone", 21)]
        [DataRow("2one34threetwone", 21)]
        [DataRow("4b", 44)]
        [DataRow("4", 44)]
        [DataRow("four", 44)]
        public void TestMethod1(string input, int expected)
        {
            var inputParser = new InputParser();

            var lines = new string[] { input };

            var total = inputParser.Parse(lines);

            Assert.AreEqual(expected, total);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var inputParser = new InputParser();

            var lines = new string[]
            {
                "two1nine",
                "eightwothree",
                "abcone2threexyz",
                "xtwone3four",
                "4nineeightseven2",
                "zoneight234",
                "7pqrstsixteen",
            };

            var total = inputParser.Parse(lines);

            Assert.AreEqual(281, total);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var inputParser = new InputParser();

            var lines = new string[]
            {
               "nine92jnhgqzctpgbcbpz", // 92
                "sevensddvc73three", // 73
                "9986fmfqhdmq8", // 98
                "7onexmxbzllfqb", // 71
                "six777", //67
                "1zbngsixxrfrpr", //16
                "threeeight9seven", //37
                "nhds975three6", //96
                "ninepgp9", //99
                "22fourninetzfourfsnxjglthreeeight", //28
                "mhcvqmsg7bdj", //77
                "seven67", //77
                "fourone5", //45
                "twofour7", //27
                "5sixonesix3pzhd",//53
                "3htvgrzpznhjts52one", //31
                "52cmzhfrxdfmtgvtfqx7three4szcfchxj", //54
                "sixtwonine7", //67
                "three7938", //38
                "67four4", //64
                "7zr9",//79
                "4qseventwoqqf9bbqg4", //44
                "sevenone1srmghlzg", //71
                "ctwonenxmhspdmnineone7", //27
                "8mgzsgmphgceight", //88
                "ktznbbmkbhln4six", //46
                "cbtpgzc4", //44
                "rxzgrqeightseven18five4txv", //84
                "jgb95ninetwonine", //99
                "45mxfg9twodsnnjsfnk1five",  //45
                "29lhfhfkdqfntwo", //22
                "tssixsixdxjzjjhq35hone", //61
                "cjfjpcrpcn7rlrlrxslmhpt56189", //79
                "5llmdmqgt149sevenoneq6", //56
                "l9649twothree", //93
                "8two34fjxt42",//82
                "seven3threelxd66",//76
                "46248mmfblpgql9fournine",//49
                "3cmvxcskh4",//34
                "491mzklmbt7bgcrbmspprjgsgv",//47
            };

            var total = inputParser.Parse(lines);

            Assert.AreEqual(2436, total);
        }
    }
}