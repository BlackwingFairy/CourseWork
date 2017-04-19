using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class DataWorker
    {
        public static String Load_Group(string group)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=" + @"appdata.db");
            connect.Open();
            string result = "";
            //SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Competitors' WHERE 'Tournir'='"+Tournir+"';", connect);
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Students';", connect);
            SQLiteDataReader reader = command.ExecuteReader();
            //List<Competitor> CompList = new List<Competitor>();
            foreach (DbDataRecord record in reader)
            {
                string getgroup = record["Group"].ToString();
                if (group == getgroup)
                {
                    //string id = record["Id"].ToString();
                    //int rnum = Convert.ToInt32(record["RatingNum"].ToString());
                    //string name = record["Name"].ToString();
                    //bool exist = Convert.ToBoolean(record["Exist"].ToString());

                    string name = record["Name"].ToString();
                    string subject = record["Subject"].ToString();
                    string mark = record["Mark"].ToString();
                    string srecord = "\n" + name + " " + subject + " " + mark;

                    //CompList.Add(new Competitor(rnum, name, exist, tournir));

                    result += srecord;
                }

            }

            connect.Close();

            //CompetitorsList RList = new CompetitorsList(CompList.Count);
            //RList.List = CompList.ToArray();

            return result;
        }

        public static String Load_Subject(string subject)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=" + @"appdata.db");
            connect.Open();
            string result = "";
            //SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Competitors' WHERE 'Tournir'='"+Tournir+"';", connect);
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Students';", connect);
            SQLiteDataReader reader = command.ExecuteReader();
            //List<Competitor> CompList = new List<Competitor>();
            foreach (DbDataRecord record in reader)
            {
                string getsubject = record["Subject"].ToString();
                if (subject == getsubject)
                {
                    //string id = record["Id"].ToString();
                    //int rnum = Convert.ToInt32(record["RatingNum"].ToString());
                    //string name = record["Name"].ToString();
                    //bool exist = Convert.ToBoolean(record["Exist"].ToString());

                    string name = record["Name"].ToString();
                    string group = record["Group"].ToString();
                    string mark = record["Mark"].ToString();
                    string srecord = "\n" + name + " " + group + " " + mark;

                    //CompList.Add(new Competitor(rnum, name, exist, tournir));

                    result += srecord;
                }

            }

            connect.Close();

            //CompetitorsList RList = new CompetitorsList(CompList.Count);
            //RList.List = CompList.ToArray();

            return result;
        }

        public static String Load_Surname(string surname)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=" + @"appdata.db");
            connect.Open();
            string result = "";
            //SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Competitors' WHERE 'Tournir'='"+Tournir+"';", connect);
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Students';", connect);
            SQLiteDataReader reader = command.ExecuteReader();
            //List<Competitor> CompList = new List<Competitor>();
            foreach (DbDataRecord record in reader)
            {
                string getsubject = record["Name"].ToString();
                if (surname == getsubject)
                {
                    //string id = record["Id"].ToString();
                    //int rnum = Convert.ToInt32(record["RatingNum"].ToString());
                    //string name = record["Name"].ToString();
                    //bool exist = Convert.ToBoolean(record["Exist"].ToString());

                    string subject = record["Subject"].ToString();
                    string group = record["Group"].ToString();
                    string mark = record["Mark"].ToString();
                    string srecord = "\n" + subject + " " + group + " " + mark;

                    //CompList.Add(new Competitor(rnum, name, exist, tournir));

                    result += srecord;
                }

            }

            connect.Close();

            //CompetitorsList RList = new CompetitorsList(CompList.Count);
            //RList.List = CompList.ToArray();

            return result;
        }
    }
}
