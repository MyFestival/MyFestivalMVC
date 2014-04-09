using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyFestival.Models
{
    public class MyFestivalDb : DbContext
    {
        public MyFestivalDb()
            : base("MyFestivalDb")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Festival> Festivals { get; set; }

        public DbSet<Events> Events { get; set; }

        public DbSet<County> Counties { get; set; }

        public DbSet<FestivalType> FestivalTypes { get; set; }

        public DbSet<EventType> EType { get; set; }

        public DbSet<Town> Towns { get; set; }
    }

    /*public class MyFestivalDb : DbContext
    {
        public MyFestivalDb()
            : base("MyFestivalAzure")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Festival> Festivals { get; set; }

        public DbSet<Events> Events { get; set; }

        public DbSet<County> Counties { get; set; }

        public DbSet<FestivalType> FestivalTypes { get; set; }

        public DbSet<EventType> EType { get; set; }

        public DbSet<Town> Towns { get; set; }
    }*/
}