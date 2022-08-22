using Faith.Shared.DTOs;
using Faith.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Faith.Shared.Domain
{
    public class User
    {
        public Guid UserID { get; set; } = new Guid();
        public UserRole Role { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public UserGender Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public ICollection<User> Youngsters { get; set; } // Voor de Begeleiders
        public TimeLine TimeLine { get; set; } // Voor de Jongeren

        public User()
        {
            Youngsters = new List<User>();
        }
        public User(UserRole role)
        {
            Role = role;
            switch (role)
            {
                case UserRole.ADMIN:
                case UserRole.MONITOR:
                    Youngsters = new List<User>();
                    TimeLine = null;
                    break;
                case UserRole.YOUNGSTER:
                    Youngsters = null;
                    TimeLine = new TimeLine();
                    break;
            }
        }
        public User(UserDTO user)
        {
            UserID = user.UserID;
            Role = user.Role;
            FirstName = user.FirstName;
            FamilyName = user.FamilyName;
            Gender = user.Gender;
            Birthdate = user.Birthdate;
            Address = user.Address;
            Email = user.Email;
            Telephone = user.Telephone;
            if (user.Youngsters != null)
            {
                Youngsters = new List<User>();
                foreach (var youngster in user.Youngsters)
                {
                    Youngsters.Add(new User(youngster));
                };
            }
            if (user.TimeLine != null)
            {
                TimeLine = new TimeLine(user.TimeLine);
            }
        }
    }
}
