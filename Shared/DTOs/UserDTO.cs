using Faith.Shared.Domain;
using Faith.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Faith.Shared.DTOs
{
    public class UserDTO
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
        public ICollection<UserDTO> Youngsters { get; set; } // Voor de Begeleiders
        public TimeLineDTO TimeLine { get; set; } // Voor de Jongeren

        public UserDTO()
        {
            Youngsters = new List<UserDTO>();
        }
        public UserDTO(UserRole role)
        {
            switch (role)
            {
                case UserRole.ADMIN:
                case UserRole.MONITOR:
                    Youngsters = new List<UserDTO>();
                    TimeLine = null;
                    break;
                case UserRole.YOUNGSTER:
                    Youngsters = null;
                    TimeLine = new TimeLineDTO();
                    break;
            }
        }
        public UserDTO(User user)
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
                Youngsters = new List<UserDTO>();
                foreach (var youngster in user.Youngsters)
                {
                    Youngsters.Add(new UserDTO(youngster));
                };
            }
            if (user.TimeLine != null)
            {
                TimeLine = new TimeLineDTO(user.TimeLine);
            }
        }
    }
}
