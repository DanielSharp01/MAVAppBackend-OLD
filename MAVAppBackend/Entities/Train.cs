﻿using System;
using System.Data.Common;
using MAVAppBackend.DataAccess;
using SharpEntities;

namespace MAVAppBackend.Entities
{
    public class Train : Entity<int>
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnChange();
            }
        }

        private string type;
        public string Type
        {
            get => type;
            set
            {
                type = value;
                OnChange();
            }
        }

        private Polyline polyline;
        public Polyline Polyline
        {
            get => polyline;
            set
            {
                polyline = value;
                OnChange();
            }
        }

        private DateTime? expiryDate;
        public DateTime? ExpiryDate
        {
            get => expiryDate;
            set
            {
                expiryDate = value;
                OnChange();
            }
        }
        public override void Fill(DbDataReader reader)
        {
            name = reader.GetStringOrNull("name");
            type = reader.GetStringOrNull("type");
            polyline = reader.GetPolylineOrNull("polyline");
            expiryDate = reader.GetDateTimeOrNull("expiry_date");
            Filled = true;
        }

        public override void Fill(Entity<int> other)
        {
            if (!(other is Train train)) return;

            name = train.name;
            type = train.type;
            polyline = train.polyline;
            expiryDate = train.expiryDate;
            Filled = train.Filled;
        }
    }
}
