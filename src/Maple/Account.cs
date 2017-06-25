﻿using Destiny.Data;
using System;
using System.Data;

namespace Destiny.Maple
{
    public sealed class Account
    {
        public MapleClient Client { get; private set; }

        public int ID { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Pin { get; set; }
        public string Pic { get; set; }
        public bool IsBanned { get; set; }
        public bool IsMaster { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime Creation { get; set; }

        private bool Assigned { get; set; }

        public Account(MapleClient client)
        {
            this.Client = client;
        }

        public void Load(string username)
        {
            Datum datum = new Datum("accounts");

            try
            {
                datum.Populate("Username = '{0}'", username);
            }
            catch (RowNotInTableException)
            {
                throw new NoAccountException();
            }

            this.ID = (int)datum["ID"];
            this.Assigned = true;

            this.Username = (string)datum["Username"];
            this.Password = (string)datum["Password"];
            this.Salt = (string)datum["Salt"];
            this.Pin = (string)datum["Pin"];
            this.Pic = (string)datum["Pic"];
            this.IsBanned = (bool)datum["IsBanned"];
            this.IsMaster = (bool)datum["IsMaster"];
            this.Birthday = (DateTime)datum["Birthday"];
            this.Creation = (DateTime)datum["Creation"];
        }


        public void Save()
        {
            Datum datum = new Datum("accounts");

            datum["Username"] = this.Username;
            datum["Password"] = this.Password;
            datum["Salt"] = this.Salt;
            datum["Pin"] = this.Pin;
            datum["Pic"] = this.Pic;
            datum["IsBanned"] = this.IsBanned;
            datum["IsMaster"] = this.IsMaster;
            datum["Birthday"] = this.Birthday;
            datum["Creation"] = this.Creation;

            if (this.Assigned)
            {
                datum.Update("ID = '{0}'", this.ID);
            }
            else
            {
                this.ID = datum.InsertAndReturnID();
                this.Assigned = true;
            }

            Log.Inform("Saved account '{0}' to database.", this.Username);
        }
    }
}
