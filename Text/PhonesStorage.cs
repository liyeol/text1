using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text
{
    class PhonesStorage
    {
        private readonly string DbFilePath = @"C:\Users\Елизавета\source\repos\Text\phonedb.csv";
        private readonly char ColumnSeparator = ',';

        public PhonesStorage()
        {
            EnshureDbFile();
        }

        public void Edit(int orderNumber)
        {
            var records = GetAllRecord();
            var index = orderNumber - 1;

            // Print error order number;
            if (index < 0 || index > records.Length - 1)
            {
                var redBuffer = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong order number.");
                Console.ForegroundColor = redBuffer;
            }

            var recordObj = DeserializeRecord(records[index]);

            var yellowBuffer = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You are going to edit:");
            Print(recordObj);
            Console.ForegroundColor = yellowBuffer;

            var updatedRecord = new PhoneRecord();

            records[index] = SerializeRecord(updatedRecord);

            File.WriteAllLines(DbFilePath, records.Select(x => x.Trim()));
        }

        public void PrintAll()
        {
            var records = GetAllRecord();

            for (int i = 0; i < records.Length; i++)
            {
                string? record = records[i];
                var desirializedRecord = DeserializeRecord(record);

                Print(desirializedRecord, i + 1);
            }
        }

        public void Print(PhoneRecord phoneRecord)
        {
            Console.WriteLine($"+{phoneRecord.PhoneNumber} - {phoneRecord.FirstName} {phoneRecord.LastName}");
        }

        public void Print(PhoneRecord phoneRecord, int orderNumber)
        {
            Console.WriteLine($"[{orderNumber}]: +{phoneRecord.PhoneNumber} - {phoneRecord.FirstName} {phoneRecord.LastName}");
        }

        public void Save(PhoneRecord phoneRecord)
        {
            File.AppendAllLines(DbFilePath, new[] { SerializeRecord(phoneRecord) });
        }

        private string[] GetAllRecord()
        {
            return File.ReadAllText(DbFilePath).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        }

        private string SerializeRecord(PhoneRecord phoneRecord)
        {
            return $"{phoneRecord.FirstName}{ColumnSeparator}" +
                $"{phoneRecord.LastName}{ColumnSeparator}" +
                $"{phoneRecord.PhoneNumber}{ColumnSeparator}";
        }

        private PhoneRecord DeserializeRecord(string record)
        {
            string[] cellValues = record.Split(ColumnSeparator);

            var parsedRecord = new PhoneRecord(cellValues[0], cellValues[1], cellValues[2]);

            return parsedRecord;
        }

        private void EnshureDbFile()
        {
            if (!File.Exists(DbFilePath))
            {
                using (File.Create(DbFilePath))
                {

                }
            }
        }

        public void SearchWord()
        {
            string[] words = File.ReadAllText(DbFilePath).Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine("Write a word: ");
            string wordtobesearch = Console.ReadLine();
            bool condition = false;
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Contains(wordtobesearch)==true)
                {
                    condition = true; 
                    break;
                }
                else
                {
                    condition = false;
                }
            }
            if (condition == true)
            {
                Console.WriteLine("{0} found in data", wordtobesearch);
            }
            else
            {
                Console.WriteLine("{0} not found in data", wordtobesearch);
            }
        }
        private void AlphSortByLastName()
        {
            var words = File.ReadAllText(DbFilePath).Split('\n', StringSplitOptions.RemoveEmptyEntries);


            words.Sort((fName, lName) => fName.Split(" ")[1].CompareTo(lName.Split(" ")[1]));
            Console.WriteLine(string.Join(",", words));


        }

        public void AlphSortByFirstName(PhoneRecord phoneRecord)
        {
            var words = File.ReadAllText(DbFilePath).Split('\n', StringSplitOptions.RemoveEmptyEntries);

            words.Sort();
            Console.WriteLine(string.Join(",", words));

            Console.ReadKey();
        }

    }
}