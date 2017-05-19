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
        public static void Add_Record(string name, string group, string subject, string mark)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=" + @"appdata.db");
            connect.Open();
            
            SQLiteCommand command = new SQLiteCommand("INSERT INTO 'Students' ('Name'," +
                    "'Group','Subject','Mark') VALUES ('" + name + "','" + group +
                    "','" + subject + "','" + mark + "');", connect);
                command.ExecuteNonQuery();
            

            connect.Close();
        }

        public static void Delete_Name(string name)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=" + @"appdata.db");
            connect.Open();
            SQLiteCommand command = new SQLiteCommand("DELETE FROM 'Students' WHERE Name='" + name + "';", connect);
            command.ExecuteNonQuery();
            connect.Close();
        }


        public static void Delete_Group(string group)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=" + @"appdata.db");
            connect.Open();
            SQLiteCommand command = new SQLiteCommand("DELETE FROM 'Students' WHERE Group='" + group + "';", connect);
            command.ExecuteNonQuery();
            connect.Close();
        }

        public static void Delete_Subject(string subject)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=" + @"appdata.db");
            connect.Open();
            SQLiteCommand command = new SQLiteCommand("DELETE FROM 'Students' WHERE Subject='" + subject + "';", connect);
            command.ExecuteNonQuery();
            connect.Close();
        }


        public static string Load_Group(string group)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=" + @"appdata.db");
            connect.Open();
            string result = "";
           
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Students';", connect);
            SQLiteDataReader reader = command.ExecuteReader();
           
            foreach (DbDataRecord record in reader)
            {
                string getgroup = record["Group"].ToString();
                if (group == getgroup)
                {
                    
                    string name = record["Name"].ToString();
                    string subject = record["Subject"].ToString();
                    string mark = record["Mark"].ToString();
                    string srecord = "\n" + name + " " + group + " " + subject + " " + mark;


                    result += srecord;
                }

            }

            connect.Close();

            
            return result;
        }

        public static string Load_Subject(string subject)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=" + @"appdata.db");
            connect.Open();
            string result = "";
            
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Students';", connect);
            SQLiteDataReader reader = command.ExecuteReader();
            
            foreach (DbDataRecord record in reader)
            {
                string getsubject = record["Subject"].ToString();
                if (subject == getsubject)
                {
                    
                    string name = record["Name"].ToString();
                    string group = record["Group"].ToString();
                    string mark = record["Mark"].ToString();
                    string srecord = "\n" + name + " " + group + " " + subject + " " + mark;

                    

                    result += srecord;
                }

            }

            connect.Close();

            
            return result;
        }

        public static string Load_Surname(string surname)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=" + @"appdata.db");
            connect.Open();
            string result = "";
            
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Students';", connect);
            SQLiteDataReader reader = command.ExecuteReader();
            
            foreach (DbDataRecord record in reader)
            {
                string getsubject = record["Name"].ToString();
                if (surname == getsubject)
                {
                    
                    string subject = record["Subject"].ToString();
                    string group = record["Group"].ToString();
                    string mark = record["Mark"].ToString();
                    string srecord = "\n" + surname + " " + group + " " + subject + " " + mark;

                    result += srecord;
                }

            }

            connect.Close();

            return result;
        }
    }
}
