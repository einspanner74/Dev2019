using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace HistoryManager
{
    public class CHistoryManager
    {
        private static HistoryWindow HistoryWnd = new HistoryWindow();
        private static string ProjectName;

        public bool IsShowHistoryWindow = false;

        public enum LOG_TYPE { INFO = 0, WARN, ERR }
        private static string[] LogType = new string[3] { "INFO", "WARN", "ERR" };

        static string INSERT_string = "";
        static string CreateComm = "";

        public CHistoryManager(string _ProjectName, string _ProjectType)
        {
            ProjectName = _ProjectName;

            HistoryWnd.Initialize(ProjectName, _ProjectType);

            if (_ProjectType == "DISPENSER")
            {
                INSERT_string = "INSERT INTO HistoryFile (Date, RecipeName, ID, LastResult, InspImagePath) ";
                CreateComm = string.Format("{0} (Date Datetime, RecipeName char, ID char, LastResult char, InspImagePath char);", SqlDefine.CREATE_TABLE);
            }

            else if (_ProjectType == "BLOWER")
            {
                INSERT_string = "INSERT INTO HistoryFile (Date, RecipeName, LastResult, IDResult, InspImagePath) ";
                CreateComm = string.Format("{0} (Date Datetime, RecipeName char, LastResult char, IDResult char, InspImagePath char);", SqlDefine.CREATE_TABLE);
            }
        }

        /// <summary>
        /// AddHistory(RecipeName, LastResult, ID Result, InspImagePath) 
        /// </summary>
        /// <param name="HistoryItem"></param>
        public static void AddHistory(string[] HistoryItem)
        {

            DateTime _NowDate = DateTime.Now;
            string _NowDateFormat = _NowDate.ToString("yyyy-MM-dd HH:mm:ss.ffff");

            bool CreateTable = CheckDBFile();

            string SendQuery = string.Format("VALUES ('{0}', ", _NowDateFormat);
            for(int iLoopCount = 0; iLoopCount < HistoryItem.Count(); iLoopCount++)
            {
                if(iLoopCount < HistoryItem.Count() - 1) SendQuery = SendQuery + "'" + HistoryItem[iLoopCount] + "',";
                else                                     SendQuery = SendQuery + "'" + HistoryItem[iLoopCount] + "');";
            }

            SendQuery = INSERT_string + SendQuery;

            SqlQuery.HistoryInsertQuery(SendQuery, CreateTable, CreateComm);
        }

        public void ShowLogWindow(bool _IsShow)
        {
            IsShowHistoryWindow = _IsShow;

            if (true == _IsShow)
            {
                HistoryWnd.Show();
            }

            else if (false == _IsShow)
            {
                HistoryWnd.Hide();
            }
        }

        public void ShowHistoryWindow()
        {
            if (true == HistoryWnd.CheckDBFile())
            {
                HistoryWnd.ClearDataGridViewHistory();
                HistoryWnd.ClearSearchOption();
                HistoryWnd.ShowDialog();
            }
        }

        private static bool CheckDBFile()
        {
            bool CreateTable = false;
            //string connStrFolderPath = @"D:\VisionInspectionData\CIPOSLeadInspection\HistoryData";
            string connStrFolderPath = String.Format(@"D:\VisionInspectionData\{0}\HistoryData", ProjectName);

            if (false == Directory.Exists(connStrFolderPath))
            {
                Directory.CreateDirectory(connStrFolderPath);
                CreateTable = true;
            }
            string StrFilePath = String.Format(@"{0}\History.db", connStrFolderPath);
            if (false == File.Exists(StrFilePath))
            {
                CreateTable = true;
            }

            return CreateTable;
        }
    }
}
