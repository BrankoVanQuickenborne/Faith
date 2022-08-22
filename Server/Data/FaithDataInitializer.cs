using Faith.Shared.Domain;
using Faith.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faith.Server.Data
{
    public class FaithDataInitializer
    {
        private readonly FaithContext _faithContext;
        private List<User> CreatedUsers;
        private List<User> CreatedMonitors;
        private List<User> CreatedYoungsters;

        public FaithDataInitializer(FaithContext faithContext)
        {
            _faithContext = faithContext;
            CreatedUsers = new List<User>();
            CreatedMonitors = new List<User>();
            CreatedYoungsters = new List<User>();
        }

        public async Task InitializeData()
        {
            //_faithContext.Database.EnsureDeleted();
            //if (_faithContext.Database.EnsureCreated())
            //{
                // --[[MONITORS]]--
                //await CreateUser(new User(UserRole.MONITOR) { FirstName = "Branko", FamilyName = "Van Quickenborne", Email = "brankovq@gmail.com", Gender = UserGender.MALE, Birthdate = DateTime.Parse("1998-07-15 00:00:00.000000") });
                //await CreateUser(new User(UserRole.MONITOR) { FirstName = "Benjamin", FamilyName = "Vertonghen", Email = "benjamin.v@gmail.com", Gender = UserGender.MALE, Birthdate = DateTime.Parse("1990-01-01 00:00:00.000000") });
                //await CreateUser(new User(UserRole.MONITOR) { FirstName = "Mario", FamilyName = "Mario", Email = "mario.m@gmail.com", Gender = UserGender.MALE, Birthdate = DateTime.Parse("1990-02-04 00:00:00.000000") });
                //await CreateUser(new User(UserRole.MONITOR) { FirstName = "Vanessa", FamilyName = "Vanessa", Email = "vanessa.v@gmail.com", Gender = UserGender.FEMALE, Birthdate = DateTime.Parse("1988-12-14 00:00:00.000000") });

                // --[[YOUNGSTERS]]--
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Alexane", FamilyName = "Alexane", Email = "alexane.a@gmail.com", Gender = UserGender.FEMALE, Birthdate = DateTime.Parse("2005-01-03 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Bert", FamilyName = "Bert", Email = "bert.b@gmail.com", Gender = UserGender.MALE, Birthdate = DateTime.Parse("2005-02-07 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Claire", FamilyName = "Claire", Email = "claire.c@gmail.com", Gender = UserGender.FEMALE, Birthdate = DateTime.Parse("2005-06-11 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "David", FamilyName = "David", Email = "david.d@gmail.com", Gender = UserGender.MALE, Birthdate = DateTime.Parse("2005-08-31 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Ella", FamilyName = "Ella", Email = "ella.e@gmail.com", Gender = UserGender.FEMALE, Birthdate = DateTime.Parse("2005-01-12 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Frieda", FamilyName = "Frieda", Email = "frieda.f@gmail.com", Gender = UserGender.FEMALE, Birthdate = DateTime.Parse("2005-02-28 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Geert", FamilyName = "Geert", Email = "geert.g@gmail.com", Gender = UserGender.MALE, Birthdate = DateTime.Parse("2005-03-09 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Hannah", FamilyName = "Hannah", Email = "hannah.h@gmail.com", Gender = UserGender.FEMALE, Birthdate = DateTime.Parse("2005-04-16 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Ine", FamilyName = "Ine", Email = "ine.i@gmail.com", Gender = UserGender.FEMALE, Birthdate = DateTime.Parse("2005-05-25 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Jonas", FamilyName = "Jonas", Email = "jonas.j@gmail.com", Gender = UserGender.MALE, Birthdate = DateTime.Parse("2005-06-18 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Koen", FamilyName = "Koen", Email = "koen.k@gmail.com", Gender = UserGender.MALE, Birthdate = DateTime.Parse("2005-07-16 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Lieze", FamilyName = "Lieze", Email = "Lieze.l@gmail.com", Gender = UserGender.FEMALE, Birthdate = DateTime.Parse("2005-08-20 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Maxim", FamilyName = "Maxim", Email = "maxim.m@gmail.com", Gender = UserGender.MALE, Birthdate = DateTime.Parse("2005-09-18 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Nina", FamilyName = "Nina", Email = "nina.n@gmail.com", Gender = UserGender.FEMALE, Birthdate = DateTime.Parse("2005-01-09 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Ona", FamilyName = "Ona", Email = "ona.o@gmail.com", Gender = UserGender.FEMALE, Birthdate = DateTime.Parse("2005-11-11 00:00:00.000000") });
                //await CreateUser(new User(UserRole.YOUNGSTER) { FirstName = "Pieter", FamilyName = "Pieter", Email = "pieter.p@gmail.com", Gender = UserGender.MALE, Birthdate = DateTime.Parse("2005-12-06 00:00:00.000000") });

                //AssignMonitors();

                //_faithContext.SaveChanges();

                // --[[POSTS]]--
                //Random helloWorldPost = new Random();
                //foreach (var youngster in CreatedYoungsters)
                //{
                //    int i = helloWorldPost.Next(0, 4); // 25% chance of generating a Hello World Post
                //    if (i == 2)
                //        youngster.TimeLine.Posts.Add(new Post { Text = $"Hello World from {youngster.FirstName}!" });
                //}

                //_faithContext.SaveChanges();
            //}
        }

        private async Task CreateUser(User user)
        {
            await _faithContext.FUsers.AddAsync(user);
            CreatedUsers.Add(user);
            if (user.Role == UserRole.MONITOR)
                CreatedMonitors.Add(user);
            else if (user.Role == UserRole.YOUNGSTER)
                CreatedYoungsters.Add(user);
        }

        private void AssignMonitors()
        {
            Random random = new Random();
            foreach (var youngster in CreatedYoungsters)
            {
                int i = random.Next(0, 4); // Choose a random Monitor
                for (int j = 0; j < CreatedMonitors.Count; j++)
                {
                    if (CreatedMonitors[j].Youngsters.Count >= 10) // If the Monitor already has 100 Youngsters
                    {

                        i++; // Add to the next Monitor
                        if (i >= CreatedMonitors.Count) // If this were to happen for the last Monitor
                        {
                            i = 0; // Loop back around to the first
                            CreatedMonitors[i].Youngsters.Add(youngster); // Manual add since the first Monitor already has been passed
                            break; // Break the For-loop of the Monitors
                        }

                    }
                    if (i == j)
                    {
                        CreatedMonitors[j].Youngsters.Add(youngster);
                        break; // Break the For-loop of the Monitors
                    }
                }
            }
        }
    }
}
